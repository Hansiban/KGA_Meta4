using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : LivingEntity
{
    public Slider healthSlider;

    public AudioClip deathClip;
    public AudioClip hitClip;
    public AudioClip itemdropClip;
    
    private AudioSource playeraudio;
    private Animator player_ani;

    private PlayerMovement player_move;
    private PlayerShooter player_Shooter;

    private void Awake()
    {
        playeraudio = GetComponent<AudioSource>();
        player_ani = GetComponent<Animator>();
        player_move = GetComponent<PlayerMovement>();
        player_Shooter = GetComponent<PlayerShooter>();
    }

    protected override void OnEnable()
    {
        base.OnEnable(); //부모 클래스 메소드 호출

        healthSlider.gameObject.SetActive(true);
        healthSlider.maxValue = StartHealth;
        healthSlider.value = health;

        //죽었을 때 movement shooter를 비활성화
        //여기서 확인차 활성화
        player_move.enabled = true;
        player_Shooter.enabled = true;

    }
    public override void OnDamage(float Damage, Vector3 hitposition, Vector3 hitNomal)
    {
        if (!isDead)
        {
            playeraudio.PlayOneShot(hitClip);
        }
        base.OnDamage(Damage, hitposition, hitNomal);
        healthSlider.value = health; //health update
    }
    public override void Die()
    {
        base.Die();
        healthSlider.gameObject.SetActive(false);

        //시각적 효과, 청각적 효과
        player_ani.SetTrigger("Die");
        playeraudio.PlayOneShot(deathClip);

        //죽었을때 movement shooter를 비활성화
        player_move.enabled = false;
        player_Shooter.enabled = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!isDead)
        {
            IItem item = other.GetComponent<IItem>();
            if (item != null)
            {
                item.Use(gameObject);
                playeraudio.PlayOneShot(itemdropClip);
            }
        }
    }

}
