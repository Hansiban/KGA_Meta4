using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using Newtonsoft.Json;

public class Mapdata_Save : MonoBehaviour
{
    [SerializeField] private TileMap2D tilemap;
    [SerializeField] InputField Name_Inputfield;
    private void Awake()
    {
        Name_Inputfield.text = "Noname.json";
    }
    public void Save()
    {
        MapData data = tilemap.GetMapData();
        string fileName = Name_Inputfield.text;

        if (!fileName.Contains(".json")) //.json 문구가 포함되지 않았다면
        {
            fileName += ".json";
        }
        fileName = Path.Combine("MapData/", fileName);

        string tojson = JsonConvert.SerializeObject(data, Formatting.Indented);
        
        File.WriteAllText(fileName, tojson);
    }
}

