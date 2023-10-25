using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EffectManager : MonoBehaviour
{
    [SerializeField] private Animator note_HitAnimator;
    [SerializeField] private Animator judgement_Animator;
    [SerializeField] private Image judgement_img;

    //캐싱
    private string Key = "Hit";

    [Header("[Perfect>Cool>Good>Bad]")]
    [SerializeField] private Sprite[] Judgment_sprite; //직접할당

    private void Awake()
    {
        transform.GetChild(0).TryGetComponent(out note_HitAnimator);
        transform.GetChild(1).TryGetComponent(out judgement_Animator);
        transform.GetChild(1).TryGetComponent(out judgement_img);
    }

    public void Judgement_Effect(int index)
    {
        judgement_img.sprite = Judgment_sprite[index];
        judgement_Animator.SetTrigger(Key);
    }

    public void Notehit_Effect()
    {
        note_HitAnimator.SetTrigger(Key);
    }

}
