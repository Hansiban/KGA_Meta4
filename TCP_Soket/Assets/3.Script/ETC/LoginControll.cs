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
            Log.text = "���̵� ��й�ȣ�� �Է��ϼ���.";
            return;
        }

        if (SQLManager.instance.Login(id_i.text, pass_i.text))
        {
            //����
            user_info info = SQLManager.instance.info;
            Debug.Log(info.User_name+"|" + info.User_Password);
            gameObject.SetActive(false);
        }
        else
        {
            //����
            Log.text = "���̵� ��й�ȣ�� Ȯ���� �ּ���.";
        }
    }
}
