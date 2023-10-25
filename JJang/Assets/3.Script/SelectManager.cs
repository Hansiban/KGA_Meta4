using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectManager : MonoBehaviour
{
    #region 스케치
    /*
    <캐릭터 선택 창 만들기>

    //변수
        //캐릭터 오브젝트 배열
        //인덱스 번호

    //메서드
        //업데이트
            -> 입력받기
        //입력받기
            -> 오른쪽 방향키 누르면 넘어가기(인덱스+1)
            -> 왼쪽 방향키 누르면 넘어가기(인덱스-1)
        //넘어가기
            -> 기존에 켜져있는 오브젝트 비활성화하기
            -> 인덱스 안에 있는 오브젝트 활성화하기
        //최종 선택하기
            -> 현재 활성화되어 있는 캐릭터 플레이어로 지정하기
            -> 플레이어 다음 씬으로 넘기기(DontDestroy)
     */
    #endregion

    [Header("캐릭터 선택")]
    [SerializeField] private GameObject[] characters;
    private int index = 0;
    private void Start()
    {
        characters[0].gameObject.SetActive(true);
        for (int i = 1; i < characters.Length; i++)
        {
            characters[i].gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        InputKey();
        if (Input.GetKeyDown(KeyCode.Return))
        {
            Selected();
        }
    }

    private void InputKey() //입력받기
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            index++;
            index %= characters.Length;
            CharActive();
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            index--;
            index += characters.Length;
            index %= characters.Length;
            CharActive();
        }
    }

    private void CharActive()//넘어가기
    {
        //기존에 켜져있는 오브젝트 비활성화하기
        for (int i = 0; i < characters.Length; i++)
        {
            if (characters[i].activeSelf)
            {
                characters[i].SetActive(false);
                break;
            }
        }

        //인덱스 안에 있는 오브젝트 활성화하기
        characters[index].SetActive(true);
    }

    private void Selected()//최종 선택하기
    {

        //현재 활성화되어 있는 캐릭터 플레이어로 지정하기
          
        //플레이어 다음 씬으로 넘기기(DontDestroy)
        //SceneManager.LoadScene(1);
        Debug.Log("SceneManager.LoadScene(1)");
    }


}
