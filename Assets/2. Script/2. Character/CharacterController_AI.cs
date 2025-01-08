using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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
    private AIState _curState;
    private FSM _fsm;

    public AIBase linkedAIBase;

    public Vector3 targetPosition;

    public Transform patrolPointParent;
    private List<Transform> patrolPointList;
    private int pointOffset = 0;                // �̵��� ����Ʈ�� �������ִ� offset

    private void Awake()
    {
        linkedAIBase = GetComponent<AIBase>();
    }

    private void Start()
    {
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
                    ChangeState(AIState.Die);
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
                break;

            case AIState.Battle:
                break;

            case AIState.Die:
                break;

            case AIState.Win:
                break;
        }
    }

    public bool IsListenSound()
    {
        return false;
    }

    public bool IsSearchPlayer()
    {
        return false;
    }


}
