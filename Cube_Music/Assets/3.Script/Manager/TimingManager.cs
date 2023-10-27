using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimingManager : MonoBehaviour
{
    //node�� ���� ���� : postition
    [Header("Time Rect[prefect->cool->good->bad]")]
    [SerializeField] private RectTransform[] timmingRect;
    private Vector2[] timingBox;

    [SerializeField] private RectTransform center;
    public List<GameObject> BoxNote_List = new List<GameObject>();

    private int[] judgement_Record = new int[5];

    [SerializeField] private EffectManager effectManager;
    [SerializeField] private ComboManager comboManager;
    [SerializeField] private ScoreManager scoreManager;
    [SerializeField] private Player_controller player;
    [SerializeField] private StageControll stageControll;
    private void Start()
    {
        //------------------------------------------------

        effectManager = FindObjectOfType<EffectManager>();
        comboManager = FindObjectOfType<ComboManager>();
        scoreManager = FindObjectOfType<ScoreManager>();
        player = FindObjectOfType<Player_controller>();
        stageControll = FindObjectOfType<StageControll>();

        //���--------------------------------------------
        timingBox = new Vector2[timmingRect.Length];
        for (int i = 0; i < timingBox.Length; i++)
        {
            //�ּҰ� ���ϴ� ���
            //�̹����� �߽� - (�̹����� �ʺ�/2)
            //�̹����� �߽� + (�̹����� �ʺ�/2)

            //�迭�� 0��° -> Perfect
            //�迭�� ������ -> Bad
            timingBox[i].Set
                (
                center.localPosition.x - timmingRect[i].rect.width / 2,
                center.localPosition.x + timmingRect[i].rect.width / 2
                );
        }
        Initialized_record();
    }
    public bool Check_canNext_Plate()
    {
        if (Physics.Raycast(player.Dest_Pos, Vector3.down, out RaycastHit hit, 1.1f))
        {
            if (hit.transform.CompareTag("BasicPlate"))
            {
                if (hit.transform.TryGetComponent(out BasicPlate p))
                {
                    if (p.flag)
                    {
                        p.flag = false;
                        return true;
                    }
                }
            }
        }
        return false;
    }
    public void Initialized_record()
    {
        for (int i = 0; i < judgement_Record.Length; i++)
        {
            judgement_Record[i] = 0;
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
                 ���� ���� �ȿ� �ִ��� Ȯ�� -> notePos_x �������� ���̴ϴ�.
                 timingBox �ּҰ�(x)�� �ִ밪(y) �ȿ� �ִٸ� �� �������� ����
                 */

                if (timingBox[x].x <= notePos_x && notePos_x <= timingBox[x].y)
                    //���� �ȿ� �ִٸ�?
                {
                    //������ ����
                    if (BoxNote_List[i].transform.TryGetComponent(out Note note))
                    {
                        note.HideNote();
                    }
                    Debug.Log(Debug_note(x));
                    effectManager.Notehit_Effect();
                    if (Check_canNext_Plate())
                    {
                        effectManager.Judgement_Effect(x);
                        stageControll.ShowNextPlate();
                        scoreManager.AddScore(x);
                        if (x < 3)
                        {
                            comboManager.Addcombo();
                        }
                        else
                        {
                            comboManager.ResetCombo();
                        }
                        judgement_Record[x]++;
                    }
                    else
                    {
                        effectManager.Judgement_Effect(5);
                        comboManager.ResetCombo();
                    }

                    return true;
                }
            }
        }
        comboManager.ResetCombo();
        Judgement_miss();
        return false;
    }
    public string Debug_note(int i)
    {
        //����Ʈ �� �� ���
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
    public void Judgement_miss()
    {
        judgement_Record[4]++;
    }

    public int[] Get_judgement_Record()
    {
        return judgement_Record;
    }

}
