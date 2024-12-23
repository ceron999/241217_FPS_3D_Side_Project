using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Player�� ���� ���� Event ����
/// </summary>

public class InputSystem : MonoBehaviour
{
    public static InputSystem Instance;

    // �̵�
    public Vector2 Movement => movement;
    public Vector2 movement;

    public bool IsRun => isRun;
    private bool isRun = false;
    public bool IsSit => isSit;
    private bool isSit = false;

    // ���� �׼�
    public System.Action OnClickAlpha1;                 // �� ���� ����Ī
    public System.Action OnClickAlpha2;                 // ���� ���� ����Ī
    public System.Action OnClickAlpha3;                 // Į ����Ī
    public System.Action OnClickAlpha4;                 // ����ź ����Ī
    public System.Action OnClickAlpha5;                 // C4 ����Ī

    public System.Action OnClickSpace;                  // ����

    public System.Action OnClickMouseLeftButtonDown;    // ��Ŭ��
    public System.Action OnClickMouseRightButton;       // ��Ŭ��

    private void Awake()
    {
        if(Instance == null)
            Instance = this;
    }

    private void Update()
    {
        // ���� ��ȯ
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            OnClickAlpha1?.Invoke();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            OnClickAlpha2?.Invoke();
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            OnClickAlpha3?.Invoke();
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            OnClickAlpha4?.Invoke();
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            OnClickAlpha5?.Invoke();
        }

        // �̵� ����
        float inputX = Input.GetAxis("Horizontal");
        float inputY = Input.GetAxis("vertical");
        movement = new Vector2(inputX, inputY);

        // �̵� ���� ��ȯ
        isRun = Input.GetKey(KeyCode.LeftShift);
        isSit = Input.GetKey(KeyCode.LeftControl);

        // ����
        if (Input.GetKeyDown(KeyCode.Space))
        {
            OnClickSpace?.Invoke();
        }

    }
}
