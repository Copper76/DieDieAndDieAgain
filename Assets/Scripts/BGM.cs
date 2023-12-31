using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class BGM : MonoBehaviour
{
    public List<AudioClip> clips;

    // Start is called before the first frame update
    void Start()
    {
        AudioSource BGMSource = GetComponent<AudioSource>();
        int i = Random.Range(0, clips.Count);
        BGMSource.clip = clips[i];
        BGMSource.loop = true;
        
        BGMSource.volume = i switch
        {
            0 => 0.1f,
            5 => 0.1f,
            2 => 0.1f,
            6 => 0.1f,
            _ => 0.2f
        };
        BGMSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
}
