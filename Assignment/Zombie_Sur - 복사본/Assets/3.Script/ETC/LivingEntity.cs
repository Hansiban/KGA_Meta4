using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class LivingEntity : MonoBehaviour, IDamage
{
    //��� ����ü���� ���� ������Ʈ
    /*
    ��üü��
    ����ü��
    �׾����� ��Ҵ���-> Eventó��
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
        //�׾����� ���׾�����
        if (health <= 0 && !isDead)
        {
            //�״� �޼ҵ带 ȣ��
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
