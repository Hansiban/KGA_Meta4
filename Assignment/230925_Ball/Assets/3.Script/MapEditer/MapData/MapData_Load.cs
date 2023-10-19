using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;

/*
Json ������ ���ٹ��
1. String���� �о�� ������ Ű�� ����
2. �ܺ� Dll�� ���-> Ŭ���� ���� �̰ų� ������ �����̳� ������ �����͸� ����ȭ - ������ȭ
 */

public class MapData_Load : MonoBehaviour
{
    public MapData Load(string filename)
    {
        filename = "Stage05";
        if (!filename.Contains(".json"))
        {
            filename += ".json";
        }
        filename = Path.Combine(Application.streamingAssetsPath, filename);
        string ReadData = File.ReadAllText(filename);
        MapData mapdata = new MapData();

        //������ȭ
        mapdata = JsonConvert.DeserializeObject<MapData>(ReadData);
        return mapdata;

    }
}
