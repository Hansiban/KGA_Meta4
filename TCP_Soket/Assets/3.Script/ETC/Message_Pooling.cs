using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Message_Pooling : MonoBehaviour
{
    [SerializeField] private Text[] message_Box;

    public Action<string> Message;

    string current_me = string.Empty;
    string past_me;


    private void Start()
    {
        message_Box = transform.GetComponentsInChildren<Text>();
        Message = Adding_Message;
        past_me = current_me;
    }

    private void Update()
    {
        if (past_me.Equals(current_me)) return;
        ReadText(current_me);
        past_me = current_me;
    }

    public void Adding_Message(string m)
    {
        current_me = m;
    }

    public void ReadText(string m)
    {
        bool isinput = false;
        for (int i = 0; i < message_Box.Length; i++)
        {
            if (message_Box[i].text.Equals(""))
            {
                message_Box[i].text = m;
                isinput = true;
                break;
            }
        }
        if (!isinput)
        {
            for (int i = 1; i < message_Box.Length; i++)
            {
                message_Box[i - 1].text = message_Box[i].text;
                //�̴� �۾�
            }
            message_Box[message_Box.Length - 1].text = m;
        }
    }

}
