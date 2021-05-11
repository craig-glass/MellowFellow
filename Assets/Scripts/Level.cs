using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Level : MonoBehaviour
{
    Scene activeScene;
    public static int levelIndex = 1;
    public Text level;
    public Text levelNo;
    public GameObject panel;
    
    // Start is called before the first frame update
    void Start()
    {
        activeScene = SceneManager.GetActiveScene();

        if(activeScene.buildIndex % 2 == 0)
        {
            level.text = "Level " + (levelIndex);
            levelNo.text = "Level: " + (levelIndex);
            StartCoroutine(DisableLevelText());
        }
        else
        {
            level.text = "!! Bonus Level !!";
            levelNo.text = "!! Bonus !!";
            StartCoroutine(DisableLevelText());
            
        }
        
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator  DisableLevelText()
    {
        yield return new WaitForSeconds(2);
        level.enabled = false;
        panel.SetActive(false);
    }
    
}
