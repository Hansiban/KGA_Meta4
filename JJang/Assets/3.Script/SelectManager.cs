using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectManager : MonoBehaviour
{
    #region ����ġ
    /*
    <ĳ���� ���� â �����>

    //����
        //ĳ���� ������Ʈ �迭
        //�ε��� ��ȣ

    //�޼���
        //������Ʈ
            -> �Է¹ޱ�
        //�Է¹ޱ�
            -> ������ ����Ű ������ �Ѿ��(�ε���+1)
            -> ���� ����Ű ������ �Ѿ��(�ε���-1)
        //�Ѿ��
            -> ������ �����ִ� ������Ʈ ��Ȱ��ȭ�ϱ�
            -> �ε��� �ȿ� �ִ� ������Ʈ Ȱ��ȭ�ϱ�
        //���� �����ϱ�
            -> ���� Ȱ��ȭ�Ǿ� �ִ� ĳ���� �÷��̾�� �����ϱ�
            -> �÷��̾� ���� ������ �ѱ��(DontDestroy)
     */
    #endregion

    [Header("ĳ���� ����")]
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

    private void InputKey() //�Է¹ޱ�
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

    private void CharActive()//�Ѿ��
    {
        //������ �����ִ� ������Ʈ ��Ȱ��ȭ�ϱ�
        for (int i = 0; i < characters.Length; i++)
        {
            if (characters[i].activeSelf)
            {
                characters[i].SetActive(false);
                break;
            }
        }

        //�ε��� �ȿ� �ִ� ������Ʈ Ȱ��ȭ�ϱ�
        characters[index].SetActive(true);
    }

    private void Selected()//���� �����ϱ�
    {

        //���� Ȱ��ȭ�Ǿ� �ִ� ĳ���� �÷��̾�� �����ϱ�
          
        //�÷��̾� ���� ������ �ѱ��(DontDestroy)
        //SceneManager.LoadScene(1);
        Debug.Log("SceneManager.LoadScene(1)");
    }


}
