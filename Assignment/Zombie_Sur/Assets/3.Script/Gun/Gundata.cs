using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/Gundata",fileName ="Gun_data")]
public class Gundata : ScriptableObject
{
    /*
     * 탄창용량 => int
     * 연사력 => float => 코루틴
     * 공격력  => float
     * 장전시간 => float
     * 처음 주어질 전체 총알량 => int
     * 총소리 => audio clip
     * 재장전소리 => audio clip
     * 
     */
    public float Damage = 25f;
    public float TimebetFire = 0.12f; //연사력
    public float ReloadTime = 1.8f;
    public int MagCapacity = 30;
    public int StartAmmoRemain = 100;
    public AudioClip Shot_clip;
    public AudioClip Reload_clip;


}
