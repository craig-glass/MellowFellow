using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public Text timer;
    private float startTime;
    private YellowFellowGame yellowScript;

    // Start is called before the first frame update
    void Start()
    {
        yellowScript = GameObject.Find("Game").GetComponent<YellowFellowGame>();
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (!yellowScript.timeStopped)
        {
            float t = Time.time - startTime;

            string minutes = ((int)t / 60).ToString();
            string seconds = (t % 60).ToString("f0");

            timer.text = "Time: " + minutes + ":" + seconds;
        }
        
    }
}
