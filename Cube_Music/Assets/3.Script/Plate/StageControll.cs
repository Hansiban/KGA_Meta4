using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageControll : MonoBehaviour
{
    [SerializeField] GameObject stage = null;
    private Transform[] stage_Plates; //stage에 있는 plate 갖고옴

    [SerializeField] private float offset_Y = -3f;
    [SerializeField] private float plate_speed = 10f;

    [SerializeField] int step_count = 0; //현재 밟은 플레이트 수
    [SerializeField] int totalPlate_count = 0; //모든 플레이트 수

    private void Start()
    {
        Setting_Stage();
    }
    public void Setting_Stage()
    {
        step_count = 2;

        //초기값 셋팅
        stage_Plates = stage.GetComponent<Stage>().plates;
        totalPlate_count = stage_Plates.Length;

        for (int i = 0; i < totalPlate_count; i++)
        {
            if (!stage_Plates[i].gameObject.activeSelf)
            {
                stage_Plates[i].transform.position = new Vector3(stage_Plates[i].position.x, stage_Plates[i].position.y + offset_Y, stage_Plates[i].position.z);
            }
        }
    }
    public void ShowNextPlate()
    {
        if (step_count < totalPlate_count)
        {
            StartCoroutine(MovePlate_co(step_count++));
        }
    }
    private IEnumerator MovePlate_co(int index)
    {
        stage_Plates[index].gameObject.SetActive(true);
        //목적 포지션을 만들어 줍시다.
        Vector3 dest_pos = new Vector3(stage_Plates[index].position.x, stage_Plates[index].position.y - offset_Y, stage_Plates[index].position.z);

        while (Vector3.SqrMagnitude(stage_Plates[index].position - dest_pos) >= 0.001f)
        {
            //보간법 사용할때 반복문에서 진행할 것
            stage_Plates[index].position = Vector3.Lerp(stage_Plates[index].position, dest_pos, plate_speed * Time.deltaTime);
            yield return null;
        }
        stage_Plates[index].position = dest_pos;
    }

}
