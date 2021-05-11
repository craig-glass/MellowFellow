using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Lives : MonoBehaviour
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
        t.text = "Lives: " + Fellow.lives.ToString();
    }
}
