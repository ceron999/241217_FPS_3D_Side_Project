using System.Collections;
using System.Collections.Generic;
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
/// 5. Win: Player가 모두 처치되어 잡을 적이 없는 상태
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

    public Transform patrolPointParent;
    public List<Transform> patrolPointList;
    public int pointOffset = 0;                // 이동할 포인트를 지정해주는 offset

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        linkedAIBase = GetComponent<AIBase>();
    }

    private void Start()
    {
        // 탐색 위치 설정
        SetPatrolPointList();

        _curState = AIState.Search;
        _fsm = new FSM(new SearchState(this));

    }

    private void Update()
    {
        switch (_curState)
        {
            case AIState.Search:
                // 적을 발견했을 경우 -> Battle
                // 소리를 들었을 경우 or C4가 설치되었을 경우 -> takeWarning
                // 죽었을 경우 -> Die
                if (linkedAIBase.curStat.HP <= 0)
                {
                    ChangeState(AIState.Die); 
                }
                // 모든 적이 처치되었을 경우 -> Win
                break;

            case AIState.TakeWarning:
                break;

            case AIState.Battle:
                break;

            case AIState.Die:
                break;

            case AIState.Win:
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

            case AIState.Win:
                _fsm.ChangeState(new WinState(this));
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

    #region Search에서 이동하는 함수
    // 타겟 방향으로 이동하는 함수
    public void SetMoveDirection()
    {
        Vector3 moveDirection = (navMeshAgent.steeringTarget - transform.position).normalized;
        Vector3 localDirection = linkedAIBase.transform.InverseTransformDirection(moveDirection);
        Vector2 input = new Vector2(localDirection.x, localDirection.z);

        linkedAIBase.Move(input);
        linkedAIBase.transform.forward = moveDirection;
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
        // 이동 경로 자체가 없으면 리턴
        if (patrolPointList.Count == 0)
            return;

        // 마지막 위치까지 이동했다면 처음 위치로 이동
        if(pointOffset == patrolPointList.Count - 1)
        {
            pointOffset = 0;
            return;
        }

        pointOffset++;
    }
    #endregion

    public bool IsListenSound()
    {
        return false;
    }

    public bool IsSearchPlayer()
    {
        return false;
    }


}
