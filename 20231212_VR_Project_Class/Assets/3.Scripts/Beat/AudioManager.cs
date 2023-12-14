using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    private AudioSource audio_source;
    [SerializeField] private AudioClip BGM;
    // Start is called before the first frame update

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    void Start()
    {
        audio_source = GetComponent<AudioSource>();
        audio_source.clip = BGM;
        audio_source.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
