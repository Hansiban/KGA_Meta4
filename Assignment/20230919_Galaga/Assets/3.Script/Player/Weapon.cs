using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    /*
    총알은 나갈때 딜레이가 있어야 함 
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
            //코루틴
            //유니티가 가지고 있는 제어권을 잠시 오브젝트에게 옮김
            // yield return : 유니티한테 제어권을 다시 돌려준다 
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
