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

    //client가 server에 connect되었을때 콜백함수
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
    //클라이언트가 server를 나갔을 때  
    
    [ClientCallback]
    private void OnDestroy()
    {
        if (!isLocalPlayer) return;
        onMessage -= newMessage;
    }
    //RPC는 결국 ClienRPC 명령어 < command명령어(서버한테 알려주는거) < Client 명령어?

    [Client]//요청하는거
    public void Send()
    {
        if (!Input.GetKeyDown(KeyCode.Return)) return;
        if (string.IsNullOrWhiteSpace(inputField.text)) return;
        CmdSendMessage(inputField.text);
        inputField.text = string.Empty;
    }

    [Command]//클라이언트가 요청하면 서버가 해주는 작업
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
