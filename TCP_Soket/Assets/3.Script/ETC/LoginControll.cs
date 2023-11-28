using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoginControll : MonoBehaviour
{
    public InputField id_i;
    public InputField pass_i;

    [SerializeField] private Text Log;

    public void Login_btn()
    {
        if (id_i.text.Equals(string.Empty) || pass_i.text.Equals(string.Empty))
        {
            Log.text = "아이디 비밀번호를 입력하세요.";
            return;
        }

        if (SQLManager.instance.Login(id_i.text, pass_i.text))
        {
            //성공
            user_info info = SQLManager.instance.info;
            Debug.Log(info.User_name+"|" + info.User_Password);
            gameObject.SetActive(false);
        }
        else
        {
            //실패
            Log.text = "아이디 비밀번호를 확인해 주세요.";
        }
    }
}
