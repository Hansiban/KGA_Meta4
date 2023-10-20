using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class LivingEntity : MonoBehaviour, IDamage
{
    //모든 생명체에게 붙힐 컴포넌트
    /*
    전체체력
    현재체력
    죽었는지 살았는지-> Event처리
     */
    public float StartHealth = 100f;
    public float health { get; protected set; }
    public bool isDead { get; protected set; }
    public event Action onDead;

    protected virtual void OnEnable()
    {
        isDead = false;
        health = StartHealth;
    }
    public virtual void OnDamage(float Damage, Vector3 hitposition, Vector3 hitNomal)
    {
        health -= Damage;
        //죽었는지 안죽었는지
        if (health <= 0 && !isDead)
        {
            //죽는 메소드를 호출
            Die();
        }
    }
    public virtual void Die()
    {
        if (onDead != null)
        {
            onDead();
        }
        isDead = true;
    }
    public virtual void Restore_health(float newHealth)
    {
        if (isDead)
        {
            return;
        }
        health += newHealth;
    }
}
