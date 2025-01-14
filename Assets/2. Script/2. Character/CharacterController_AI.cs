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

    public Transform patrolPointParent;
    public List<Transform> patrolPointList;
    public int pointOffset = 0;                // �̵��� ����Ʈ�� �������ִ� offset

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

    }

    private void Update()
    {
        switch (_curState)
        {
            case AIState.Search:
                // ���� �߰����� ��� -> Battle
                // �Ҹ��� ����� ��� or C4�� ��ġ�Ǿ��� ��� -> takeWarning
                // �׾��� ��� -> Die
                if (linkedAIBase.curStat.HP <= 0)
                {
                    ChangeState(AIState.Die); 
                }
                // ��� ���� óġ�Ǿ��� ��� -> Win
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

    #region Search���� �̵��ϴ� �Լ�
    // Ÿ�� �������� �̵��ϴ� �Լ�
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

    public bool IsListenSound()
    {
        return false;
    }

    public bool IsSearchPlayer()
    {
        return false;
    }


}
