using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/Gundata",fileName ="Gun_data")]
public class Gundata : ScriptableObject
{
    /*
     * źâ�뷮 => int
     * ����� => float => �ڷ�ƾ
     * ���ݷ�  => float
     * �����ð� => float
     * ó�� �־��� ��ü �Ѿ˷� => int
     * �ѼҸ� => audio clip
     * �������Ҹ� => audio clip
     * 
     */
    public float Damage = 25f;
    public float TimebetFire = 0.12f; //�����
    public float ReloadTime = 1.8f;
    public int MagCapacity = 30;
    public int StartAmmoRemain = 100;
    public AudioClip Shot_clip;
    public AudioClip Reload_clip;


}
