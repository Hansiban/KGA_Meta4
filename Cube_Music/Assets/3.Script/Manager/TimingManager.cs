using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimingManager : MonoBehaviour
{
    //node의 판정 기준 : postition
    [Header("Time Rect[prefect->cool->good->bad]")]
    [SerializeField] private RectTransform[] timmingRect;
    private Vector2[] timingBox;

    [SerializeField] private RectTransform center;
    public List<GameObject> BoxNote_List = new List<GameObject>();

    [SerializeField] private EffectManager effectManager;
    [SerializeField] private ComboManager comboManager;
    [SerializeField] private ScoreManager scoreManager;
    private void Start()
    {
        //------------------------------------------------

        effectManager = FindObjectOfType<EffectManager>();
        comboManager = FindObjectOfType<ComboManager>();
        scoreManager = FindObjectOfType<ScoreManager>();

        //계산--------------------------------------------
        timingBox = new Vector2[timmingRect.Length];
        for (int i = 0; i < timingBox.Length; i++)
        {
            //최소값 구하는 방법
            //이미지의 중심 - (이미지의 너비/2)
            //이미지의 중심 + (이미지의 너비/2)

            //배열의 0번째 -> Perfect
            //배열의 마지막 -> Bad
            timingBox[i].Set
                (
                center.localPosition.x - timmingRect[i].rect.width / 2,
                center.localPosition.x + timmingRect[i].rect.width / 2
                );
        }
    }

    public bool Check_Timming()
    {
        for (int i = 0; i < BoxNote_List.Count; i++)
        {
            float notePos_x = BoxNote_List[i].transform.localPosition.x;
            for (int x = 0; x < timingBox.Length; x++)
            {
                /*
                 판정 범위 안에 있는지 확인 -> notePos_x 기준으로 갈겁니다.
                 timingBox 최소값(x)과 최대값(y) 안에 있다면 그 판정으로 ㄱㄱ
                 */

                if (timingBox[x].x <= notePos_x && notePos_x <= timingBox[x].y)
                    //범위 안에 있다면?
                {
                    //판정이 났음
                    if (BoxNote_List[i].transform.TryGetComponent(out Note note))
                    {
                        note.HideNote();
                    }
                    Debug.Log(Debug_note(x));
                    effectManager.Notehit_Effect();
                    effectManager.Judgement_Effect(x);
                    scoreManager.AddScore(x);

                    if (x < 3)
                    {
                        comboManager.Addcombo();
                    }
                    else
                    {
                        comboManager.ResetCombo();
                    }
                    return true;
                }
            }
        }
        return false;
    }
    public string Debug_note(int i)
    {
        //퍼펙트 쿨 굿 배드
        switch (i)
        {
            case 0:
                return "Perfect";
            case 1:
                return "Cool";
            case 2:
                return "Good";
            case 3:
                return "Bad";
            default:
                return string.Empty;
        }
    }

}
