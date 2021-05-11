using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class GameUIScore : MonoBehaviour
{
    Text t;
    

    // Start is called before the first frame update
    void Start()
    {
        t = GetComponent<Text>();
      
    }

    // Update is called once per frame
    void Update()
    {
        // update score in game UI
        int score = Fellow.score;
        t.text = "Score: " + score.ToString();
        
    }
}
