using UnityEngine;
using UnityEngine.UI;

public class ComboManager : MonoBehaviour
{
    /*
      콤보가 활성화 되는 시점
        1. miss가 아니면 카운트++
    콤보가 비활성화 되는 시점
        1. miss일때
        2. bad일때는 콤보를 더하지 않음.
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
        //모든 컴포넌트는 상속된 Gameobject를 들고 올 수 있다.
        combo_Txt.gameObject.SetActive(false);
    }
    public void ResetCombo()
    {
        //Bad나 Miss가 나타날 때 사용
        current_combo = 0;
        combo_Txt.text = string.Format("{0:#,##0}", current_combo); // 천단위 자리수 표기

        combo_Img.SetActive(false);
        combo_Txt.gameObject.SetActive(false);
    }
    public void Addcombo(int combo = 1)
    {
        current_combo += combo;
        //{0:#,##0} -> {(들어갈 변수 index : 들어갈 string 형식)}
        combo_Txt.text = string.Format("{0:#,##0}", current_combo);
        if (current_combo >= 2)
        {
            combo_Img.SetActive(true);
            combo_Txt.gameObject.SetActive(true);
            ani.SetTrigger(aniKey);
        }

    }

}
