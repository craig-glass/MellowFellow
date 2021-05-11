using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShrinkPlane : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale -= new Vector3(0.000005f * (Level.levelIndex), 0f, 0.000005f * (Level.levelIndex));
    }
}
