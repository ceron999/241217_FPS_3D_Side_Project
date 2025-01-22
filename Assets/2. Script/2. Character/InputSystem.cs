using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Player�� ���� ���� Event ����
/// </summary>

public class InputSystem : MonoBehaviour
{
    public static InputSystem Instance;

    // Ŀ��
    public bool isStopCameraMove = false;
    private bool isShowCursor = false;

    // �̵�
    public Vector2 Movement => movement;
    private Vector2 movement;

    public bool IsRun => isRun;
    private bool isRun = false;
    public bool IsCrouch => isCrouch;
    private bool isCrouch = false;

    // ȸ��
    public Vector2 Look => look;
    private Vector2 look;

    // ���� �׼�
    public System.Action OnClickAlpha1;                 // �� ���� ����Ī
    public System.Action OnClickAlpha2;                 // ���� ���� ����Ī
    public System.Action OnClickAlpha3;                 // Į ����Ī
    public System.Action OnClickAlpha4;                 // ����ź ����Ī
    public System.Action OnClickAlpha5;                 // C4 ����Ī

    public System.Action OnClickSpace;                  // ����

    public System.Action OnClickLeftMouseButtonDown;    // ��Ŭ�� down
    public System.Action OnClickLeftMouseButton;        // ��Ŭ�� ���� ����
    public System.Action OnClickLeftMouseButtonUp;      // ��Ŭ�� up
    public System.Action OnClickRightMouseButtonDown;   // ��Ŭ��

    public System.Action OnClickR;                      // ������

    // UI ����
    public System.Action OnClickTabDown;                    // ��Ȳ�� ǥ��
    public System.Action OnClickTabUp;                    // ��Ȳ�� ����

    private void Awake()
    {
        if(Instance == null)
            Instance = this;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
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
        float inputY = Input.GetAxis("Vertical");
        movement = new Vector2(inputX, inputY);

        // �̵� ���� ��ȯ
        isRun = Input.GetKey(KeyCode.LeftShift);
        isCrouch = Input.GetKey(KeyCode.LeftControl);

        // ȸ�� ���� ��ȯ
        if (!isStopCameraMove)
        {
            float lookX = Input.GetAxis("Mouse X");
            float lookY = Input.GetAxis("Mouse Y");
            look = isShowCursor ? Vector2.zero : new Vector2(lookX, lookY * (-1f));
        }
        else
        {
            look = Vector2.zero;
        }

        // ����
        if (Input.GetKeyDown(KeyCode.Space))
        {
            OnClickSpace?.Invoke();
        }

        // ������
        if(Input.GetKeyDown(KeyCode.R))
        {
            OnClickR?.Invoke();
        }

        // ����
        if (Input.GetMouseButtonDown(0))
        {
            OnClickLeftMouseButtonDown?.Invoke();
        }

        if (Input.GetMouseButton(0))
        {
            OnClickLeftMouseButton?.Invoke();
        }

        if (Input.GetMouseButtonUp(0))
        {
            OnClickLeftMouseButtonUp?.Invoke();
        }

        if (Input.GetMouseButtonDown(1))
        {
            OnClickRightMouseButtonDown?.Invoke();
        }

        // UI
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            OnClickTabDown?.Invoke();
        }

        if (Input.GetKeyUp(KeyCode.Tab))
        {
            OnClickTabUp?.Invoke();
        }
    }

    public void Clear()
    {
        // ���� �׼�
        OnClickAlpha1 = null;                 
        OnClickAlpha2 = null;
        OnClickAlpha3 = null;
        OnClickAlpha4 = null;
        OnClickAlpha5 = null;

        OnClickR = null;

        OnClickAlpha3 = null;

        OnClickLeftMouseButtonDown = null;
        OnClickLeftMouseButtonUp = null;
        OnClickRightMouseButtonDown = null;

    }
}
