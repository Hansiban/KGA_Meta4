using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stagecontroller : MonoBehaviour
{
    public static int MaxStageCount = 10;
    //Tilemap 2D 컴포넌트를 참조하여서 맵을 직접적으로 만드는 곳
    [SerializeField] TileMap2D tilemap2d;

    [SerializeField] PlayerController PlayerController;
    [SerializeField] Cameracontroll_G cameracontroll;

    [SerializeField] private Stage_UI stage_UI;
    private void Awake()
    {
        MapData_Load load = new MapData_Load();
        int index = PlayerPrefs.GetInt("StageIndex") + 1;
        string current_stage = index < 10 ? $"Stage0{index}" : $"Stage{index}"; 

        MapData map = load.Load(current_stage);
        tilemap2d.Generate_tilemap(map);

        PlayerController.Setup(map.PlayerPosition,map.Mapsize.y);
        cameracontroll.Setup(map.Mapsize.x, map.Mapsize.y);
        stage_UI.UpdateTextStage(current_stage);
    }

    public void Gameclear()
    {
        int index = PlayerPrefs.GetInt("StageIndex");

        if (index < MaxStageCount - 1)
        {
            index++;
            PlayerPrefs.SetInt("StageIndex", index);
            SceneLoader.LoadScene();
        }
    }

}
