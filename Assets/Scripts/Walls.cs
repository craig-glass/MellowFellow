using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walls : MonoBehaviour
{
    private bool lavaHit;
    private Rigidbody wallRb;
    public GameObject fire;

    // Start is called before the first frame update
    void Start()
    {
        wallRb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (lavaHit)
        {
            MoveLeft();
        }
                  
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Lava"))
        {
            lavaHit = true;
            fire.SetActive(true);
        }
    }

    void MoveLeft()
    {
        wallRb.constraints = RigidbodyConstraints.FreezePositionY;
        transform.Translate(Vector3.left * Time.deltaTime);
    }
}
