using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    AudioSource audioSource;
    public AudioClip bonusMusicClip;
    public AudioClip gameMusicClip;
    Scene activeScene;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        activeScene = SceneManager.GetActiveScene();
        if(activeScene.buildIndex == 1)
        {
            audioSource.PlayOneShot(bonusMusicClip);
        }
        else
        {
            audioSource.PlayOneShot(gameMusicClip);
        }
        
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
