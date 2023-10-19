using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class IntroScene : MonoBehaviour
{
    private void Awake()
    {
        //���� ������ ������ ��Ŀ���� ���� �ʾƵ� ������ ����� �� �ֵ���
        Application.runInBackground = true;

        //������ ���� �����Ͽ��� �� stage�� index�� 0���� �ʱ�ȭ
        PlayerPrefs.SetInt("StageIndex", 0);

        //��ó���� : ���� ���� �ϱ� ���� ���� �����Ѵ�.
#if UNITY_EDITOR_WIN
        DirectoryInfo directory = new DirectoryInfo(Application.streamingAssetsPath);
        Stagecontroller.MaxStageCount = directory.GetFiles().Length / 2;
#elif PLATFORM_STANDALONE_WIN
        Stagecontroller.MaxStageCount = directory.GetFiles().Length;
#endif
    }
    private void Update()
    {
        if (Input.anyKeyDown)
        {
            SceneLoader.LoadScene("MainGame");
        }
    }
}
