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
         게임종료
        1. 결과창 나타나게 함 -> result
        2. 노트가 더이상 나오지 않도록 해야함 -> 해결
        3. audio finish 넣어야함 -> 해결
        4. player가 움직이면 안됨 -> bool값 -> isCanpresskey
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
