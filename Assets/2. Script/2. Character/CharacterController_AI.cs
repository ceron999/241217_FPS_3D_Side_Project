using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Windows;


/// <summary>
/// AI 컨트롤러
/// 
/// 구동 방식
/// - AI는 Search, TakeWarning, Battle, Die, Win 5가지의 상태로 존재
/// 
/// State
/// 1. Search: AI가 적을 발견하지 못한 채 지정된 지점으로 이동
/// 2. TakeWarning: AI가 적을 발견하진 못했지만 발소리, C4 소리 등으로 위치를 유추하고 해당 지점으로 이동
///     -> C4를 발견하면 해당 지점으로 이동하여 해체
/// 3. Battle: AI가 적을 발견하고 공격
/// 4. Die: AI의 체력이 0이 되어 행동이 불가능한 상태
/// </summary>

public enum AIState
{
    Search, 
    TakeWarning, 
    Battle, 
    Die, 
    Win
}

public class CharacterController_AI : MonoBehaviour
{
    public NavMeshAgent navMeshAgent;

    private AIState _curState;
    private FSM _fsm;

    [Header("캐릭터 정보")]
    public AIBase linkedAIBase;
    public CharacterBase target;                // 목표 타겟

    [Header("추적 정보")]
    public Vector3 listenPosition;              // 적 소리 들린 위치

    public Transform c4InstallPosition;
    public Transform patrolPointParent;
    public List<Vector3> patrolPointList;
    public int pointOffset;                // 이동할 포인트를 지정해주는 offset
    public float stoppingDistance;
    public Coroutine updateOffset;


    // 탐지 변수
    public LayerMask characterMask;
    private float listenRadius = 15f;           // 소리 탐지 범위
    private float battleRadius = 7f;            // 전투 변경 탐지 범위
    private float existRadius = 3f;             // 존재 탐지 범위

    // 해체 변수
    public bool isFocus = false;

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        linkedAIBase = GetComponent<AIBase>();

    }

    private void Start()
    {
        _curState = AIState.Search;
        _fsm = new FSM(new SearchState(this));

        navMeshAgent.updatePosition = false;
        navMeshAgent.speed = linkedAIBase.moveSpeed;
        navMeshAgent.stoppingDistance = stoppingDistance;
        navMeshAgent.SetDestination(patrolPointList[linkedAIBase.characterIndex]);
    }

    private void Update()
    {
        //Debug.Log($"{linkedAIBase.characterIndex} 번째 목표지점 : {pointOffset}");
        navMeshAgent.nextPosition = transform.position;

        // 죽었을 경우 -> Die
        if (linkedAIBase.curStat.HP <= 0)
        {
            StopAllCoroutines();
            ChangeState(AIState.Die);
            return;
        }

        switch (_curState)
        {
            case AIState.Search:
                // 적을 발견했을 경우 -> Battle
                if(IsDetectingPlayer())
                    ChangeState(AIState.Battle);

                // 소리를 들었을 경우 or C4가 설치되었을 경우 -> takeWarning
                if(IsListenSound() || IsInstalledC4())
                    ChangeState(AIState.TakeWarning);

                // 모든 적이 처치되었을 경우 -> Win
                break;

            case AIState.TakeWarning:
                // 죽었을 경우 -> Die
                if (linkedAIBase.curStat.HP <= 0)
                {
                    ChangeState(AIState.Die);
                    return;
                }

                // 탐지 목표 갱신(꾸준히)
                IsListenSound();

                // 적을 발견했을 경우 -> Battle
                if (IsDetectingPlayer())
                    ChangeState(AIState.Battle);

                // 적이 안보이고 폭발물 설치가 되어있지 않다면 그냥 Search
                if (IsNotExistPlayer() && !BattleManager.Instance.isC4Install)
                    ChangeState(AIState.Search);
                break;

            case AIState.Battle:

                // 적이 죽으면 Die
                if (target.IsDie)
                {
                    linkedAIBase.Shoot(false);
                    GameManager.Singleton.AIKillAction?.Invoke(linkedAIBase.characterIndex);
                    ChangeState(AIState.Search);
                }
                else
                {
                    if (Vector3.Distance(target.transform.position, this.transform.position) > 7f)
                    {
                        linkedAIBase.Shoot(false);
                        ChangeState(AIState.TakeWarning);
                    }
                }
                break;

            case AIState.Die:
                break;
        }


        _fsm.UpdateState();
    }

    void ChangeState(AIState getState)
    {
        if (_curState == getState)
            return;

        _curState = getState;

        switch(_curState)
        {
            case AIState.Search:
                _fsm.ChangeState(new SearchState(this));
                break;

            case AIState.TakeWarning:
                _fsm.ChangeState(new TakeWarningState(this));
                break;

            case AIState.Battle:
                _fsm.ChangeState(new BattleState(this));
                break;

            case AIState.Die:
                _fsm.ChangeState(new DieState(this));
                break;
        }
    }

    public void SetPatrolPointList()
    {
        for(int i =0; i< patrolPointParent.childCount; i++)
        {
            patrolPointList.Add(patrolPointParent.GetChild(i).position);
        }
    }

    #region State 변환 조건 함수
    // 근처에서 소리가 났을 경우 Search -> TakeWarning
    public bool IsListenSound()
    {
        Collider[] colArr = Physics.OverlapSphere(transform.position, listenRadius, characterMask);
        if(colArr.Length == 0)
            return false;

        for (int i = 0; i < colArr.Length; i++)
        {
            // 플레이어 아니면 넘김
            if (!colArr[i].CompareTag("Player"))
                continue;

            if (colArr[i].transform.root.TryGetComponent<AudioSource>(out AudioSource audio))
            {
                if (audio.volume > 0.5f)
                {
                    // 탐지 타겟 설정
                    if (target == null && !colArr[i].GetComponent<CharacterBase>().IsDie)
                    {
                        target = colArr[i].GetComponent<CharacterBase>();

                        // 탐지 위치 설정
                        listenPosition = colArr[i].transform.position;
                        return true;
                    }
                }
            }
        }

        return false;
    }

    public bool IsInstalledC4()
    {
        if(BattleManager.Instance.isC4Install)
        {
            c4InstallPosition = BattleManager.Instance.c4InstallPosition;
            listenPosition = c4InstallPosition.position;
            return true;
        }
        else
            return false;
    }

    // 적을 확인했을 경우 Search, TakeWarning -> Battle
    public bool IsDetectingPlayer()
    {
        if (target == null)
            return false;

        if (target.IsDie)
            return false;

        if ((target.transform.position - transform.position).magnitude < battleRadius)
        {
            return true;
        }

        return false;
    }

    // 의심 지점으로 이동했는데 플레이어를 찾지 못한 경우 TakeWarning -> Search
    public bool IsNotExistPlayer()
    {
        // 탐지 목표 거리까 지 도착했는지 확인
        if (navMeshAgent.remainingDistance > stoppingDistance)
            return false;
        
        // 플레이어 탐지
        Collider[] colArr = Physics.OverlapSphere(transform.position, existRadius, characterMask);

        // 탐지 범위에 아무도 없으면 true
        if (colArr.Length == 0)
            return true;
        else
        {
            // 탐지 범위에 캐릭터는 있는데 거기에 플레이어도 있는지 확인하고 있으면 false
            for (int i = 0; i < colArr.Length; i++)
            {
                // 플레이어 아니면 넘김
                if (colArr[i].CompareTag("Player"))
                {
                    return false;
                }
            }
            return true;
        }
    }

    public bool IsPlayerDie()
    {
        return false;
    }
    #endregion

    #region AI가 이동하는 함수
    // 이동을 멈추는 함수
    public void StopMove()
    {
        linkedAIBase.AIMove(Vector2.zero);
    }

    // 타겟 방향으로 이동하는 함수
    public void SetMoveDirection()
    {
        Vector3 moveDirection = (navMeshAgent.steeringTarget - transform.position).normalized;
        Vector3 localDirection = linkedAIBase.transform.InverseTransformDirection(moveDirection);
        Vector2 input = new Vector2(localDirection.x, localDirection.z);

        float distance = Vector3.Distance(navMeshAgent.destination, transform.position);
        if (distance < navMeshAgent.stoppingDistance)
        {
            // 이미 목표지점에 가까이 도착해있는 상태.
            linkedAIBase.AIMove(Vector2.zero);
        }
        else
        {
            linkedAIBase.AIMove(input);
        }
    }

    public bool IsArrivedDestination()
    {
        if (navMeshAgent.remainingDistance < stoppingDistance)
        {
            UpdatePointOffset();
            return true;
        }
        else
            return false;
    }

    private void UpdatePointOffset()
    {
        // 이동 경로 자체가 없으면 리턴
        if (patrolPointList.Count == 0)
            return; 
        

        // 마지막 위치까지 이동했다면 처음 위치로 이동
        if (pointOffset == patrolPointList.Count - 1)
        {
            pointOffset = 0;
            return;
        }

        // 업데이트 
        if (updateOffset == null)
        {
            updateOffset = StartCoroutine(UpdateDestination());
        }
    }

    private IEnumerator UpdateDestination()
    {
        pointOffset++;
        navMeshAgent.SetDestination(patrolPointList[pointOffset]);
        yield return new WaitForSeconds(1);
        updateOffset = null;
    }
    #endregion

    #region Search State
    public void SearchPatrolPoints()
    {
        if (navMeshAgent.hasPath)
        {
            SetMoveDirection();
        }
    }
    #endregion

    #region Battle State
    public void SetAimRigWeight(float getWeight)
    {
        linkedAIBase.aimRig.weight = getWeight;
    }

    public void Battle()
    {
        if (target.IsDie || linkedAIBase.IsDie)
            return;

        // 목표 지점 설정
        Vector3 pivot = transform.position + Vector3.up;
        Vector3 targetPosition = target.transform.position + Vector3.up;
        Vector3 direction = (targetPosition - pivot).normalized;

        // 목표 방향에 플레이어 확인
        bool isRaycastSuccessToTarget = false;

        Ray ray = new Ray(pivot, direction);
        float maxDistance = 7f;

        // Debug.DrawRay(pivot, direction * 7, Color.red);
        if (Physics.Raycast(ray, out RaycastHit hitInfo, maxDistance,
            characterMask, QueryTriggerInteraction.Ignore))
        {
            isRaycastSuccessToTarget = true;
        }

        // 에임 방향 설정
        Debug.DrawRay(pivot, linkedAIBase.transform.forward * 7, Color.red);
        Debug.DrawRay(linkedAIBase.nowWeapon.transform.position,
            linkedAIBase.nowWeapon.transform.forward * 7f, Color.blue);

        linkedAIBase.AimingPoint = targetPosition;

        bool isRaycastSuccessFromWeapon = false;
        Ray weaponRay = new Ray(pivot, linkedAIBase.nowWeapon.transform.forward);
        if (Physics.Raycast(weaponRay, out RaycastHit weaponHitInfo,
            maxDistance, characterMask, QueryTriggerInteraction.Ignore))
        {
            if (weaponHitInfo.transform.root.gameObject.CompareTag("Player"))
            {
                isRaycastSuccessFromWeapon = true;
            }
        }

        if (isRaycastSuccessToTarget && isRaycastSuccessFromWeapon)
        {
            if (!target.IsDie)
            {
                linkedAIBase.Shoot(true);
            }
            else
            {
                target = null;
                linkedAIBase.Shoot(false);
            }
        }
    }
    #endregion

    #region TakeWarning State 함수
    public  bool IsFindC4()
    {
        if (Vector3.Distance(c4InstallPosition.position, this.transform.position) > 1f)
            return false;
        else
            return true;
    }

    // C4 해체 함수
    public void UninstallC4()
    {
        if(!isFocus)
        {
            isFocus = true;
            StartCoroutine(UninstallC4Coroutine());
        }
    }

    private IEnumerator UninstallC4Coroutine()
    {
        Debug.Log("C4 해체 시작!");
        float nowTime = 0f;
        float uninstallTime = 8f;

        while(isFocus && nowTime < uninstallTime)
        {
            Debug.Log("C4 해체 중!");
            if (!isFocus)
            {
                nowTime = 0;
                break;
            }
            nowTime += Time.deltaTime;

            yield return null;
        }

        // 성공했을 경우
        if(nowTime >= uninstallTime)
        {
            Debug.Log("C4 해체 끝!");
            GameManager.Singleton.GameEnd?.Invoke(false);
        }
    }

    public void CancelUninstallC4()
    {
        Debug.Log("C4 해체 중단!");
        isFocus = false;
        listenPosition = c4InstallPosition.position;
    }
    #endregion

    public void Die()
    {
        Destroy(this.gameObject);
    }
}
