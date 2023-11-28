using System.Collections.Generic;
//소켓 통신을 위한 라이브러리
using System.Net;
using System.Net.Sockets;
using System.IO; //데이터를 읽고 쓰고 하기 위한 라이브러리
using UnityEngine;
using UnityEngine.UI;
using System.Threading;
using System;

public class TCPManager : MonoBehaviour
{
    #region 요약
    /*
     1. 메인스레드에다가 TCP통신을 하면 부하 걸림
    ->C# 멀티스레드 사용해서 Message->Action
     2. Server Client 둘다 작업
    -> 각각 개인적으로
     */
    #endregion

    public InputField IP_adress;
    public InputField port;

    [SerializeField] private Text status;

    //기본적인 소켓통신
    //.net -> 패킷 -> Stream
    //데이터를 읽는 부분 -> thread

    StreamReader reader; //데이터를 읽는 친구
    StreamWriter writer; //데이터를 쓰는 친구

    public InputField message_box;
    private Message_Pooling message;

    private Queue<string> log = new Queue<string>();
    string name_ = SQLManager.instance.info.User_name;
    void Status_message()
    {
        if (log.Count > 0)
        {
            status.text = log.Dequeue();
        }
    }

    #region Server
    public void Server_open()
    {
        message = FindObjectOfType<Message_Pooling>();
        Thread thread = new Thread(ServerConnect);
        thread.IsBackground = true;
        thread.Start();
    }
    private void ServerConnect() //서버를 열어주는 쪽 => 서버를 만드는 쪽
    {
        //지속적으로 사용 -> update 문처럼 사용
        //메세지가 들어올 때 마다 열어줌
        //흐름에다가 예외처리 -> try/catch
        try
        {
            TcpListener tcp = new TcpListener(IPAddress.Parse(IP_adress.text), int.Parse(port.text));
            //TcpListener 객체 생성
            tcp.Start();
            //서버 시작 -> 서버 열림
            log.Enqueue("Server Open");

            TcpClient client = tcp.AcceptTcpClient();
            //TcpListener에 연결이 될 때까지 기다렸다가 연결이 되면 client에 할당
            log.Enqueue("Client 접속 확인 완료");

            reader = new StreamReader(client.GetStream());
            writer = new StreamWriter(client.GetStream());
            writer.AutoFlush = true;

            while (client.Connected) //하나라도 연결이 되어있는지
            {
                string readData = reader.ReadLine();
                message.Message(readData);
            }

        }

        catch (Exception e)
        {
            log.Enqueue(e.Message);
        }
    }

    #endregion


    #region Client
    public void Client_Connect()
    {
        message = FindObjectOfType<Message_Pooling>();
        log.Enqueue("Client_connect");
        Thread thread = new Thread(client_connect);
        thread.IsBackground = true;
        thread.Start();

    }
    public void client_connect() //서버에 접근하는 쪽
    {
        try
        {
            TcpClient client = new TcpClient();
            //Server = IP start point -> Client = IP end point
            IPEndPoint ipend = new IPEndPoint(IPAddress.Parse(IP_adress.text), int.Parse(port.text));
            client.Connect(ipend);
            log.Enqueue("Client Server Connect Compelete!");

            reader = new StreamReader(client.GetStream());
            writer = new StreamWriter(client.GetStream());
            writer.AutoFlush = true;

            while (client.Connected) //본인이 연결이 되어있는지
            {
                string readerData = reader.ReadLine();
                message.Message(readerData);
            }
        }
        catch (Exception e)
        {
            log.Enqueue(e.Message);
        }
    }
    #endregion

    public void Sending_btn()
    {
        //만약 메세지를 보냈다면 내가 보낸 메세지도 message box에 넣을 것
        if (sending_message(message_box.text))
        {
            message.Message(message_box.text);
            message_box.text = string.Empty;
        }
    }

    private bool sending_message(string m)
    {
        if (writer != null)
        {
            writer.WriteLine(name_ + ":"+ m);
            return true;
        }
        else
        {
            Debug.Log("Writer null");
            return false;
        }
    }

    private void Update()
    {
        Status_message();
    }

}