using System.Collections.Generic;
//���� ����� ���� ���̺귯��
using System.Net;
using System.Net.Sockets;
using System.IO; //�����͸� �а� ���� �ϱ� ���� ���̺귯��
using UnityEngine;
using UnityEngine.UI;
using System.Threading;
using System;

public class TCPManager : MonoBehaviour
{
    #region ���
    /*
     1. ���ν����忡�ٰ� TCP����� �ϸ� ���� �ɸ�
    ->C# ��Ƽ������ ����ؼ� Message->Action
     2. Server Client �Ѵ� �۾�
    -> ���� ����������
     */
    #endregion

    public InputField IP_adress;
    public InputField port;

    [SerializeField] private Text status;

    //�⺻���� �������
    //.net -> ��Ŷ -> Stream
    //�����͸� �д� �κ� -> thread

    StreamReader reader; //�����͸� �д� ģ��
    StreamWriter writer; //�����͸� ���� ģ��

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
    private void ServerConnect() //������ �����ִ� �� => ������ ����� ��
    {
        //���������� ��� -> update ��ó�� ���
        //�޼����� ���� �� ���� ������
        //�帧���ٰ� ����ó�� -> try/catch
        try
        {
            TcpListener tcp = new TcpListener(IPAddress.Parse(IP_adress.text), int.Parse(port.text));
            //TcpListener ��ü ����
            tcp.Start();
            //���� ���� -> ���� ����
            log.Enqueue("Server Open");

            TcpClient client = tcp.AcceptTcpClient();
            //TcpListener�� ������ �� ������ ��ٷȴٰ� ������ �Ǹ� client�� �Ҵ�
            log.Enqueue("Client ���� Ȯ�� �Ϸ�");

            reader = new StreamReader(client.GetStream());
            writer = new StreamWriter(client.GetStream());
            writer.AutoFlush = true;

            while (client.Connected) //�ϳ��� ������ �Ǿ��ִ���
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
    public void client_connect() //������ �����ϴ� ��
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

            while (client.Connected) //������ ������ �Ǿ��ִ���
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
        //���� �޼����� ���´ٸ� ���� ���� �޼����� message box�� ���� ��
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