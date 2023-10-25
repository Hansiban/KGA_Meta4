using UnityEngine;
using UnityEngine.UI;

public class ComboManager : MonoBehaviour
{
    /*
      �޺��� Ȱ��ȭ �Ǵ� ����
        1. miss�� �ƴϸ� ī��Ʈ++
    �޺��� ��Ȱ��ȭ �Ǵ� ����
        1. miss�϶�
        2. bad�϶��� �޺��� ������ ����.
     */

    [SerializeField] private GameObject combo_Img;
    [SerializeField] private Text combo_Txt;

    private int current_combo = 0;
    private Animator ani;
    private string aniKey = "Combo";

    private void Start()
    {
        TryGetComponent(out ani);
        combo_Img.SetActive(false);
        //��� ������Ʈ�� ��ӵ� Gameobject�� ��� �� �� �ִ�.
        combo_Txt.gameObject.SetActive(false);
    }
    public void ResetCombo()
    {
        //Bad�� Miss�� ��Ÿ�� �� ���
        current_combo = 0;
        combo_Txt.text = string.Format("{0:#,##0}", current_combo); // õ���� �ڸ��� ǥ��

        combo_Img.SetActive(false);
        combo_Txt.gameObject.SetActive(false);
    }
    public void Addcombo(int combo = 1)
    {
        current_combo += combo;
        //{0:#,##0} -> {(�� ���� index : �� string ����)}
        combo_Txt.text = string.Format("{0:#,##0}", current_combo);
        if (current_combo >= 2)
        {
            combo_Img.SetActive(true);
            combo_Txt.gameObject.SetActive(true);
            ani.SetTrigger(aniKey);
        }

    }

}
