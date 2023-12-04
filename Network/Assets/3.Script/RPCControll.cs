using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Mirror;
using TMPro;

public class RPCControll : NetworkBehaviour
{
    [SerializeField] private TMP_Text chatText;
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private GameObject canvas;

    private static event Action<string> onMessage;

    //client�� server�� connect�Ǿ����� �ݹ��Լ�
    public override void OnStartAuthority()
    {
        if (isLocalPlayer)
        {
            canvas.SetActive(true);
        }
        onMessage += newMessage;
    }

    private void newMessage(string mess)
    {
        chatText.text += mess;
    }
    //Ŭ���̾�Ʈ�� server�� ������ ��  
    
    [ClientCallback]
    private void OnDestroy()
    {
        if (!isLocalPlayer) return;
        onMessage -= newMessage;
    }
    //RPC�� �ᱹ ClienRPC ��ɾ� < command��ɾ�(�������� �˷��ִ°�) < Client ��ɾ�?

    [Client]//��û�ϴ°�
    public void Send()
    {
        if (!Input.GetKeyDown(KeyCode.Return)) return;
        if (string.IsNullOrWhiteSpace(inputField.text)) return;
        CmdSendMessage(inputField.text);
        inputField.text = string.Empty;
    }

    [Command]//Ŭ���̾�Ʈ�� ��û�ϸ� ������ ���ִ� �۾�
    private void CmdSendMessage(string message)
    {
        RPCHandleMessage($"[{connectionToClient.connectionId}] : {message}");
    }

    [ClientRpc]
    private void RPCHandleMessage(string message)
    {
        onMessage?.Invoke($"\n{message}");
    }
}
