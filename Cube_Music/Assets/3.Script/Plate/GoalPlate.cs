using UnityEngine;

public class GoalPlate : MonoBehaviour
{

    private Result result;
    private NoteManager noteManager;

    private void Start()
    {
        result = FindObjectOfType<Result>();
        noteManager = FindObjectOfType<NoteManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        /*
         ��������
        1. ���â ��Ÿ���� �� -> result
        2. ��Ʈ�� ���̻� ������ �ʵ��� �ؾ��� -> �ذ�
        3. audio finish �־���� -> �ذ�
        4. player�� �����̸� �ȵ� -> bool�� -> isCanpresskey
         */
        if (other.CompareTag("Player"))
        {
            result.ShowResult();
            noteManager.Remove_note();
            AudioManager.instance.PlaySFX("Finish");
            Player_controller.isCanpressKey = false;

        }
    }
}
