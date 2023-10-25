using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteManager : MonoBehaviour
{
    /*
     BPM 120�̶�� �����Ͽ�
     �д� 60��
     ��� 1�� �� 4����ǥ 1��(beat)
     <0.5�ʾ� ��� �ϳ��� ���;���>
     */
    
    [Header("BPM ����")]
    public int BPM = 0;
    double currentTime = 0d;

    [Header("ETC")]
    [SerializeField] private Transform noteSpawner;
    [SerializeField] private GameObject NotePrefabs;

    [SerializeField] private TimingManager timingManager;
    [SerializeField] private EffectManager effectManager;
    [SerializeField] private ComboManager comboManager;
    private void Awake()
    {
        timingManager = FindObjectOfType<TimingManager>();
        effectManager = FindObjectOfType<EffectManager>();
        comboManager = FindObjectOfType<ComboManager>();
    }

    private void Update()
    {
        currentTime += Time.deltaTime;
        //if(0.5��)
        if (currentTime >= (60d / BPM))
        {
            //GameObject noteobj = Instantiate(NotePrefabs, noteSpawner.position, Quaternion.identity);
            //noteobj.transform.SetParent(this.transform);

            //ť���� ����
            GameObject note = ObjectPooling.instance.nodeQueue.Dequeue();
            note.transform.position = noteSpawner.position;
            note.SetActive(true);
            currentTime -= (60d / BPM);

            //Ÿ�̹� �Ŵ����� ������� ����.
            timingManager.BoxNote_List.Add(note);
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Note"))
        {
            if (col.TryGetComponent(out Note note))
            {
                if (note.getNoteFlag()) //��Ʈ�� �̹����� Ȱ��ȭ�� �Ǿ��ִٸ�?
                {
                    Debug.Log("Miss");
                    effectManager.Judgement_Effect(4);
                    comboManager.ResetCombo();


                }
            }
            //Destroy(col.gameObject);
            timingManager.BoxNote_List.Remove(col.gameObject);
            ObjectPooling.instance.nodeQueue.Enqueue(col.gameObject);
            col.gameObject.SetActive(false);
        }
    }

}
