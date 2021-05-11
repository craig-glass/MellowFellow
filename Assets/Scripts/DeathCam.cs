// Script controlling the camera when player falls in lava scene

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathCam : MonoBehaviour
{
    public GameObject player;
    public Vector3 offset;
    Vector3 velocity;

    // Start is called before the first frame update
    void Start()
    {
        offset = transform.position - player.transform.position;
        transform.position = player.transform.position + offset;
        velocity = player.GetComponent<Rigidbody>().velocity;
    }

    // Update is called once per frame
    void Update()
    {
        // Set camera to follow player when player falls
        if(player.transform.position.y < 0f)
        {
            velocity.y = -8f;
            transform.position = (player.transform.position + offset) - new Vector3(0f, 10f, 3f);
            player.GetComponent<Rigidbody>().velocity = velocity;
        }
        else
        {
            transform.position = (player.transform.position + offset);
        }        
    }

}
