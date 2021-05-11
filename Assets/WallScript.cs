using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WallScript : MonoBehaviour
{
    
    private Rigidbody wallRb;

    // Start is called before the first frame update
    void Start()
    {
       
        wallRb = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    private void OnTriggerEnter(Collider other)
    {
        wallRb.isKinematic = false;
        wallRb.useGravity = true;
        gameObject.isStatic = false;
        wallRb.constraints = RigidbodyConstraints.None;
    }
}
