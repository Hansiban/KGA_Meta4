using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MySql.Data;
using MySql.Data.MySqlClient;
using System;
using System.IO;
using LitJson;

/*
 sql ����ܰ�
1. connection ����
2. connection ����

������ ��������
1. connection open ��Ȳ���� Ȯ��
2. reader ���°� �а� �ִ� ��Ȳ���� Ȯ�� -> �� ������ �� 1���� ������ ����
3. �����͸� �� �о����� reader�� ���� Ȯ�� �� close (�ȴ����� �ٽ� ����)
4. 
 */

public class user_info
{
    public string User_name { get; private set; }
    public string User_Password { get; private set; }

    public user_info(string name, string password)
    {
        User_name = name;
        User_Password = password;
    }
}

public class SQLManager : MonoBehaviour
{
    public user_info info;

    public MySqlConnection connection;
    public MySqlDataReader reader;

    public string DB_Path = string.Empty;

    public static SQLManager instance = null;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DB_Path = Application.dataPath + "/Database";
        string serverinfo = ServerSet(DB_Path);
        try
        {
            if (serverinfo.Equals(string.Empty))
            {
                Debug.Log("SQL Server Json Error!");
                return;
            }
            connection = new MySqlConnection(serverinfo);
            connection.Open();
            Debug.Log("SQL Server Open!");

        }

        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
    }

    private string ServerSet(string path)
    {
        if (!File.Exists(path)) //�� ��ο� ������ ������?
        {
            Directory.CreateDirectory(path); //������ ���� ����
        }
        string Jsonstring = File.ReadAllText(path + "/config.json"); //�����͸� ������ ��Ʈ������ �޾ƿ�
        JsonData itemdata = JsonMapper.ToObject(Jsonstring);//���̽� ���·� �����͸� �ٲ���
        string serverInfo =
            $"Server={itemdata[0]["IP"]};" + //0��° �ִ� ip��(key)�� �ҷ�����
            $"Database={itemdata[0]["TableName"]};" +
            $"Uid={itemdata[0]["ID"]};" +
            $"Pwd={itemdata[0]["PW"]};" +
            $"Port={itemdata[0]["PORT"]};" +
            $"CharSet = utf8;";
        return serverInfo;
    }

    private bool connection_check(MySqlConnection con)
    {
        //���� MySqlConnection open ���°� �ƴ϶��?
        if (con.State != System.Data.ConnectionState.Open)
        {
            con.Open();
            if (con.State != System.Data.ConnectionState.Open)
            {
                return false;
            }
        }
        return true;
    }

    public bool Login(string id, string password)
    {
        //���������� DB���� �����͸� ������ ���� �޼ҵ�
        //��ȸ�Ǵ� �����Ͱ� ���ٸ�, false
        //�ִٸ�, ������ ������ info���ٰ� ���� ������ true

        try
        {
            if (!connection_check(connection))
            {
                return false;
            }

            string SQL_command = string.Format(@"SELECT User_name, User_Password FROM user_info WHERE User_name = '{0}' AND User_Password = '{1}';",id,password);
            MySqlCommand cmd = new MySqlCommand(SQL_command,connection);
            reader = cmd.ExecuteReader();
            //reader ���� �����Ͱ� 1�� �̻� ������?
            if (reader.HasRows)
            {
                //���� �����͸� �ϳ��� ����
                while (reader.Read())
                {
                    /*
                     ���׿�����
                     */
                    string name = (reader.IsDBNull(0)) ? string.Empty : (string)reader["User_Name"].ToString();
                    string pass = (reader.IsDBNull(0)) ? string.Empty : (string)reader["User_Password"].ToString();
                    if (!name.Equals(string.Empty) || !pass.Equals(string.Empty)) //���������� �����͸� �ҷ��� ��Ȳ
                    {
                        info = new user_info(name, pass);
                        if (!reader.IsClosed) reader.Close();
                        return true;
                    }
                    else //�α��� ����
                    {
                        break;
                    }
                }//while��
            }//if��
            if (!reader.IsClosed) reader.Close();
            return false;
        }
        catch(Exception e)
        {
            Debug.Log(e.Message);
            if (!reader.IsClosed) reader.Close();
            return false;
        }
    }

}
