using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Windows;


/// <summary>
/// AI ��Ʈ�ѷ�
/// 
/// ���� ���
/// - AI�� Search, TakeWarning, Battle, Die, Win 5������ ���·� ����
/// 
/// State
/// 1. Search: AI�� ���� �߰����� ���� ä ������ �������� �̵�
/// 2. TakeWarning: AI�� ���� �߰����� �������� �߼Ҹ�, C4 �Ҹ� ������ ��ġ�� �����ϰ� �ش� �������� �̵�
///     -> C4�� �߰��ϸ� �ش� �������� �̵��Ͽ� ��ü
/// 3. Battle: AI�� ���� �߰��ϰ� ����
/// 4. Die: AI�� ü���� 0�� �Ǿ� �ൿ�� �Ұ����� ����
/// 5. Win: Player�� ��� óġ�Ǿ� ���� ���� ���� ����
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

    public AIBase linkedAIBase;

    public CharacterBase target;                // ��ǥ Ÿ��

    public Vector3 listenPosition;              // �� �Ҹ� �鸰 ��ġ
    public Vector3 findPosition;                // �� �Ҹ�

    public Transform patrolPointParent;
    public List<Transform> patrolPointList;
    public int pointOffset = 0;                // �̵��� ����Ʈ�� �������ִ� offset

    // Ž�� ����
    public LayerMask characterMask;
    private float listenRadius = 15f;           // �Ҹ� Ž�� ����
    private float battleRadius = 7f;            // ���� ���� Ž�� ����
    private float existRadius = 3f;             // ���� Ž�� ����

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        linkedAIBase = GetComponent<AIBase>();
    }

    private void Start()
    {
        // Ž�� ��ġ ����
        SetPatrolPointList();

        _curState = AIState.Search;
        _fsm = new FSM(new SearchState(this));

        navMeshAgent.updatePosition = false;
    }

    private void Update()
    {
        //navMeshAgent.nextPosition = transform.position;
        switch (_curState)
        {
            case AIState.Search:
                // ���� �߰����� ��� -> Battle
                if(IsDetectingPlayer())
                    ChangeState(AIState.Battle);

                // �Ҹ��� ����� ��� or C4�� ��ġ�Ǿ��� ��� -> takeWarning
                if(IsListenSound() || IsInstalledC4())
                    ChangeState(AIState.TakeWarning);

                // �׾��� ��� -> Die
                if (linkedAIBase.curStat.HP <= 0)
                    ChangeState(AIState.Die); 

                // ��� ���� óġ�Ǿ��� ��� -> Win
                break;

            case AIState.TakeWarning:
                // Ž�� ��ǥ ����
                IsListenSound();

                // ���� �߰����� ��� -> Battle
                if (IsDetectingPlayer())
                    ChangeState(AIState.Battle);

                // ���� �Ⱥ��̸� �׳� Search
                if (IsNotExistPlayer())
                    ChangeState(AIState.Search);
                break;

            case AIState.Battle:
                // ���� ������ Die
                if (target.IsDie)
                {
                    linkedAIBase.Shoot(false);
                    ChangeState(AIState.Search);
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

    private void SetPatrolPointList()
    {
        for(int i =0; i< patrolPointParent.childCount; i++)
        {
            patrolPointList.Add(patrolPointParent.GetChild(i));
        }
    }

    #region State ��ȯ ���� �Լ�
    // ��ó���� �Ҹ��� ���� ��� Search -> TakeWarning
    public bool IsListenSound()
    {
        Collider[] colArr = Physics.OverlapSphere(transform.position, listenRadius, characterMask);
        if(colArr.Length == 0)
            return false;

        for (int i = 0; i < colArr.Length; i++)
        {
            // �÷��̾� �ƴϸ� �ѱ�
            if (!colArr[i].CompareTag("Player"))
                continue;

            if (colArr[i].transform.root.TryGetComponent<AudioSource>(out AudioSource audio))
            {
                if (audio.volume > 0.5f)
                {
                    // Ž�� Ÿ�� ����
                    if (target == null && !colArr[i].GetComponent<CharacterBase>().IsDie)
                    {
                        target = colArr[i].GetComponent<CharacterBase>();

                        // Ž�� ��ġ ����
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
        return false;
    }

    // ���� Ȯ������ ��� Search, TakeWarning -> Battle
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

    // �ǽ� �������� �̵��ߴµ� �÷��̾ ã�� ���� ��� TakeWarning -> Search
    public bool IsNotExistPlayer()
    {
        // Ž�� ��ǥ �Ÿ��� �� �����ߴ��� Ȯ��
        if (navMeshAgent.remainingDistance > 0.1f)
            return false;
        
        // �÷��̾� Ž��
        Collider[] colArr = Physics.OverlapSphere(transform.position, existRadius, characterMask);

        // Ž�� ������ �ƹ��� ������ true
        if (colArr.Length == 0)
            return true;
        else
        {
            // Ž�� ������ ĳ���ʹ� �ִµ� �ű⿡ �÷��̾ �ִ��� Ȯ���ϰ� ������ false
            for (int i = 0; i < colArr.Length; i++)
            {
                // �÷��̾� �ƴϸ� �ѱ�
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

    #region AI�� �̵��ϴ� �Լ�
    // �̵��� ���ߴ� �Լ�
    public void StopMove()
    {
        linkedAIBase.AIMove(Vector2.zero);
    }

    // Ÿ�� �������� �̵��ϴ� �Լ�
    public void SetMoveDirection()
    {
        Vector3 moveDirection = (navMeshAgent.steeringTarget - transform.position).normalized;
        Vector3 localDirection = linkedAIBase.transform.InverseTransformDirection(moveDirection);
        Vector2 input = new Vector2(localDirection.x, localDirection.z);

        float distance = Vector3.Distance(navMeshAgent.destination, transform.position);
        if (distance < navMeshAgent.stoppingDistance)
        {
            // �̹� ��ǥ������ ������ �������ִ� ����.
            linkedAIBase.AIMove(Vector2.zero);
        }
        else
        {
            //�ش� ������ ���� �� �ٶ󺸾��� �� �̵��ϵ���
            //if((linkedAIBase.transform.forward.normalized - moveDirection).magnitude < 0.1f)
            linkedAIBase.AIMove(input);
        }
    }

    public bool IsArrivedDestination()
    {
        if (navMeshAgent.remainingDistance < 0.1f)
        {
            UpdatePointOffset();
            return true;
        }
        else
            return false;
    }

    private void UpdatePointOffset()
    {
        // �̵� ��� ��ü�� ������ ����
        if (patrolPointList.Count == 0)
            return;

        // ������ ��ġ���� �̵��ߴٸ� ó�� ��ġ�� �̵�
        if(pointOffset == patrolPointList.Count - 1)
        {
            pointOffset = 0;
            return;
        }

        pointOffset++;
    }
    #endregion

    #region Battle State
    public void SetAimRigWeight(float getWeight)
    {
        linkedAIBase.aimRig.weight = getWeight;
    }

    public void Battle()
    {
        if (target.IsDie)
            return;

        // ��ǥ ���� ����
        Vector3 pivot = transform.position + Vector3.up;
        Vector3 targetPosition = target.transform.position + Vector3.up;
        Vector3 direction = (targetPosition - pivot).normalized;

        // ��ǥ ���⿡ �÷��̾� Ȯ��
        bool isRaycastSuccessToTarget = false;

        Ray ray = new Ray(pivot, direction);
        float maxDistance = 7f;

        // Debug.DrawRay(pivot, direction * 7, Color.red);
        if (Physics.Raycast(ray, out RaycastHit hitInfo, maxDistance,
            characterMask, QueryTriggerInteraction.Ignore))
        {
            Debug.Log(hitInfo.collider.transform.root.name);
            isRaycastSuccessToTarget = true;
        }

        // ���� ���� ����
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

    public void Die()
    {
        Destroy(this.gameObject);
    }
}
