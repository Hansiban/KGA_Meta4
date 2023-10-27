using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageControll : MonoBehaviour
{
    [SerializeField] GameObject stage = null;
    private Transform[] stage_Plates; //stage�� �ִ� plate �����

    [SerializeField] private float offset_Y = -3f;
    [SerializeField] private float plate_speed = 10f;

    [SerializeField] int step_count = 0; //���� ���� �÷���Ʈ ��
    [SerializeField] int totalPlate_count = 0; //��� �÷���Ʈ ��

    private void Start()
    {
        Setting_Stage();
    }
    public void Setting_Stage()
    {
        step_count = 2;

        //�ʱⰪ ����
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
        //���� �������� ����� �ݽô�.
        Vector3 dest_pos = new Vector3(stage_Plates[index].position.x, stage_Plates[index].position.y - offset_Y, stage_Plates[index].position.z);

        while (Vector3.SqrMagnitude(stage_Plates[index].position - dest_pos) >= 0.001f)
        {
            //������ ����Ҷ� �ݺ������� ������ ��
            stage_Plates[index].position = Vector3.Lerp(stage_Plates[index].position, dest_pos, plate_speed * Time.deltaTime);
            yield return null;
        }
        stage_Plates[index].position = dest_pos;
    }

}
