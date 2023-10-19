using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooter : MonoBehaviour
{
    /*
    �ѽ��, ������ -> Gun
    Player input

    Gun Object �տ� ���߱� -> animator
     
     
     */
    public Gun gun;
    
    //�ѱ� ��ġ ���߱� ���� Transform��
    [SerializeField]public Transform gunpivot;
    [SerializeField] public Transform LeftHand_mount;
    [SerializeField] public Transform RightHand_mount;

    [SerializeField] private Animator animator;
    [SerializeField] private PlayerInput playerinput;

    private void Start()
    {
        playerinput = GetComponent<PlayerInput>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        //input�� ���õ� �̺�Ʈ ȣ��
        if (playerinput.isFire)
        {
            gun.Fire();
        }

        else if (playerinput.isReload)
        {
            if (gun.Reload())
            {
                animator.SetTrigger("Reload");
            }
        }
    }
    private void OnAnimatorIK(int layerIndex)
    {
        //���� �������� ������ �Ȳ�ġ�� �̵�
        gunpivot.position = animator.GetIKHintPosition(AvatarIKHint.RightElbow);

        //IK�� ����Ͽ� �޼��� ��ġ�� ȸ���� �� ���� �����̿� ����
        animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1.0f);
        animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1.0f);

        animator.SetIKPosition(AvatarIKGoal.LeftHand, LeftHand_mount.position);
        animator.SetIKRotation(AvatarIKGoal.LeftHand, LeftHand_mount.rotation);

        //ik�� ����Ͽ� �������� ��ġ�� ȸ���� �� �����̿� ����
        animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1.0f);
        animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1.0f);

        animator.SetIKPosition(AvatarIKGoal.RightHand, RightHand_mount.position);
        animator.SetIKRotation(AvatarIKGoal.RightHand, RightHand_mount.rotation);
        
    }
}
