using System.Collections;
using UnityEngine;
using System;
using UnityEngine.Android;
using UnityEngine.UI;

public class GPSManager : MonoBehaviour
{
    public Text text;
    public GameObject logo;

    public double[] Lats; //����
    public double[] Longs; //�浵

    public float allow_distance;

    IEnumerator Start()
    {
        while (!Permission.HasUserAuthorizedPermission(Permission.FineLocation)) //���� ������ ���ߴٸ�(GPS)
        {
            yield return null;
            Permission.RequestUserPermission(Permission.FineLocation);
        }

        if (!Input.location.isEnabledByUser)
        {
            yield break;
        }

        Input.location.Start(10, 1);
        int maxwait = 20;

        while (Input.location.status.Equals(LocationServiceStatus.Initializing) && maxwait > 0)
        {
            yield return new WaitForSeconds(1f);
            maxwait--;
        }

        if (maxwait < 1)
        {
            Debug.Log("time out");
            yield break;
        }

        if (Input.location.status.Equals(LocationServiceStatus.Failed))
        {
            Debug.Log("����̽� GPS ����");
            yield break;
        }
        else
        {
            Debug.Log($"Location {Input.location.lastData.latitude}" //����
        + $"{Input.location.lastData.longitude}" //�浵
        + $"{Input.location.lastData.altitude}" //��
        + $"{Input.location.lastData.horizontalAccuracy}" //������Ȯ�� 
        + $"{Input.location.lastData.timestamp}"//Ÿ�ӽ�����
        );
            while (true)
            {
                yield return null;
                text.text = string.Format("���� : {0}\n�浵 :{1}", Input.location.lastData.latitude, Input.location.lastData.longitude);
            }
        }
    }

    //��ǥ�� �Ÿ� ���(�Ϲ����� ����)
    private double distance(double current_Lat, double current_lon, double tar_lat, double tar_lon)
    {
        double theda = current_lon - tar_lon;
        double dist = Math.Sin(Deg2Rad(current_Lat)) * Math.Sin(Deg2Rad(tar_lat)) + Math.Cos(Deg2Rad(current_Lat) * Math.Cos(Deg2Rad(tar_lat))) * Math.Cos(Deg2Rad(theda));

        dist = Math.Acos(dist);
        dist = Rad2Deg(dist);
        dist = dist * 60 * 1.1515;

        //���ͷ� ��ȯ�ϴ� �۾�
        dist = dist * 1609.344;
        return dist;
    }

    private double Deg2Rad(double deg)
    {
        return (deg * Mathf.PI / 180.0f);
    }

    private double Rad2Deg(double rad)
    {
        return (rad * 180f / Mathf.PI);
    }

    private void Update()
    {
        if(Input.location.status.Equals(LocationServiceStatus.Running))
        {
            double myLat = Input.location.lastData.latitude;
            double mylong = Input.location.lastData.longitude;

            double remaindistance = distance(myLat, mylong, Lats[0], Longs[0]);
            Debug.Log(remaindistance);

            if (remaindistance <= allow_distance)
            {
                if (!logo.activeSelf)
                {
                    logo.SetActive(true);
                }
            }
            else
            {
                if (logo.activeSelf)
                {
                    logo.SetActive(false);
                }
            }
        }
    }
}

