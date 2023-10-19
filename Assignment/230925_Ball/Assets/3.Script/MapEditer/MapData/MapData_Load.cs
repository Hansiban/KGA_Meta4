using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;

/*
Json 데이터 접근방법
1. String으로 읽어온 데이터 키로 접근
2. 외부 Dll을 사용-> 클래스 형식 이거나 데이터 컨테이너 형식의 데이터를 직렬화 - 역직렬화
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

        //역직렬화
        mapdata = JsonConvert.DeserializeObject<MapData>(ReadData);
        return mapdata;

    }
}
