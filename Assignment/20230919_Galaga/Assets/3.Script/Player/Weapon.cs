using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    /*
    �Ѿ��� ������ �����̰� �־�� �� 
     */

    [SerializeField] private GameObject Player_bullet;
    [SerializeField] private float Attack_Rate = 0.5f;

    public void TryAttack()
    {
        Instantiate(Player_bullet, transform.position, Quaternion.identity);
    }

    private IEnumerator TryAttack_co()
    {
        while (true)
        {
            //�ڷ�ƾ
            //����Ƽ�� ������ �ִ� ������� ��� ������Ʈ���� �ű�
            // yield return : ����Ƽ���� ������� �ٽ� �����ش� 
            Instantiate(Player_bullet, transform.position, Quaternion.identity);
            yield return new WaitForSeconds(Attack_Rate);
        }
    }
    public void StartFire()
    {
        StartCoroutine("TryAttack_co");
    }
    public void StopFire()
    {
        StopCoroutine("TryAttack_co");
    }
}
