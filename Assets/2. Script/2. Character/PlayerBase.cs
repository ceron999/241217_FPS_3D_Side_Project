using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBase : CharacterBase
{
    // �޸��� �� �ɱ� ���� ������
    
    private void Update()
    {
        // 1. ���� Ȯ��
        CheckGround();

        // 3. animation ����
        
        runningBlend = Mathf.Lerp(runningBlend, IsRun ? 1f : 0f, Time.deltaTime * 10f);
        crouchBlend = Mathf.Lerp(crouchBlend, IsRun ? 1f : 0f, Time.deltaTime * 10f);

        characterAnimator.SetFloat("Speed", speed);
        characterAnimator.SetFloat("Horizontal", horizontal);
        characterAnimator.SetFloat("Vertical", vertical);
        characterAnimator.SetFloat("RunningBlend", runningBlend);
        characterAnimator.SetFloat("CrouchBlend", crouchBlend);
    }

    // ���� ����Ī ���� ������
    public void SwitchWeapon()
    {

    }
}
