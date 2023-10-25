using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteManager : MonoBehaviour
{
    /*
     BPM 120이라는 가정하에
     분당 60개
     노드 1개 당 4분음표 1개(beat)
     <0.5초씩 노드 하나씩 나와야함>
     */
    
    [Header("BPM 설정")]
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
        //if(0.5초)
        if (currentTime >= (60d / BPM))
        {
            //GameObject noteobj = Instantiate(NotePrefabs, noteSpawner.position, Quaternion.identity);
            //noteobj.transform.SetParent(this.transform);

            //큐에서 꺼냄
            GameObject note = ObjectPooling.instance.nodeQueue.Dequeue();
            note.transform.position = noteSpawner.position;
            note.SetActive(true);
            currentTime -= (60d / BPM);

            //타이밍 매니저의 순서대로 넣음.
            timingManager.BoxNote_List.Add(note);
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Note"))
        {
            if (col.TryGetComponent(out Note note))
            {
                if (note.getNoteFlag()) //노트의 이미지가 활성화가 되어있다면?
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
