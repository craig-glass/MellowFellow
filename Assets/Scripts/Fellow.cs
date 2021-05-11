using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.SceneManagement;

public class Fellow : MonoBehaviour
{

    public static int score;
    public int pelletsEaten = 0;
    [SerializeField]
    int pointsPerPellet = 100;
    public float powerupDuration = 10f;
    float powerupTime = 0.0f;
    private GameObject leftTeleporter;
    private GameObject rightTeleporter;
    public GameObject ghostPrefab;
    private Ghost ghost;
    private Vector3 teleportOffset = new Vector3(2, 0, 0);
    private Vector3 initialPos;
    public static int lives = 3;
    public AudioClip deathSound;
    private AudioSource audioSource;
    public AudioClip collectPelletSound;
    public AudioClip powerupSound;
    public AudioClip teleportSound;
    YellowFellowGame yellowGameScript;
    Scene activeScene;
    Rigidbody b;
    public GameObject fire;
    public int powerupsUsed;
    public int ghostsEaten;
   
    


    void Start()
    {
        activeScene = SceneManager.GetActiveScene();
        yellowGameScript = GameObject.Find("Game").GetComponent<YellowFellowGame>();

        if (activeScene.buildIndex != 1)
        {
            ghost = GameObject.FindGameObjectWithTag("Ghost").GetComponent<Ghost>();
        }
       
        leftTeleporter = GameObject.Find("LeftTeleporter");
        rightTeleporter = GameObject.Find("RightTeleporter");
        initialPos = transform.position;
        audioSource = GetComponent<AudioSource>();
        b = GetComponent<Rigidbody>();
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Pellet"))
        {
            pelletsEaten++;
            audioSource.volume = 0.9f;
            audioSource.PlayOneShot(collectPelletSound);
            score += pointsPerPellet;
           
            
        }

        if (other.gameObject.CompareTag("LevelTrigger"))
        {
            yellowGameScript.levelComplete = true;
        }

        if (other.gameObject.CompareTag("Powerup"))
        {
            
            if (!PowerupActive())
            {
                if(activeScene.buildIndex != 1)
                {
                    fire.SetActive(true);
                }
                
                powerupTime = powerupDuration;
                audioSource.volume = 0.9f;
                audioSource.PlayOneShot(powerupSound);
                powerupsUsed++;
                other.gameObject.SetActive(false);
               
            }
            
        }

        if (other.gameObject.CompareTag("RightTeleporter"))
        {
            transform.position = leftTeleporter.transform.position + teleportOffset;
            audioSource.volume = 0.9f;
            audioSource.PlayOneShot(teleportSound);
            
        }

        if (other.gameObject.CompareTag("LeftTeleporter"))
        {
           
            transform.position = rightTeleporter.transform.position - teleportOffset;
            audioSource.volume = 0.9f;
            audioSource.PlayOneShot(teleportSound);
           
        }

       

    }

    public bool PowerupActive()
    {
        return powerupTime > 0.0f;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ghost"))
        {
            if (!PowerupActive())
            {
                audioSource.volume = 0.9f;
                audioSource.PlayOneShot(deathSound);
                
                lives -= 1;
                StartCoroutine(Respawn());
                
            }
          
        }

        if (collision.gameObject.CompareTag("Lava"))
        {
            fire.SetActive(true);
            
        }
    }


    public int PelletsEaten()
    {
        return pelletsEaten;
    }

    // Start is called before the first frame update


    [SerializeField]
    float speed = 3f;

    // update is called once per frame
    void Update()
    {
        powerupTime = Mathf.Max(0.0f, powerupTime - Time.deltaTime);
        if (PowerupActive())
        {
            if (activeScene.buildIndex == 1)
            {
                transform.localScale = new Vector3(1f, 1f, 1f);
            }
            speed = 10;
        }
        else
        {
            transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
            
            if(activeScene.buildIndex != 1)
            {
                fire.SetActive(false);
            }
            
        }
       

    }

    void FixedUpdate()
    {

        Vector3 velocity = b.velocity;

        if (PowerupActive())
        {
            speed = 4;
        }
        else
        {
            speed = 3;
        }
        
        if (yellowGameScript.gameMode == YellowFellowGame.GameMode.InGame || activeScene.buildIndex == 1)
        {
            
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                
                velocity.x = -speed;
            }
            if (Input.GetKey(KeyCode.RightArrow))
            {
                velocity.x = speed;
            }
            if (Input.GetKey(KeyCode.UpArrow))
            {
                velocity.z = speed;
            }
            if (Input.GetKey(KeyCode.DownArrow))
            {
                velocity.z = -speed;
            }
            b.velocity = velocity;
        }






    }

    IEnumerator Respawn()
    {

        transform.position = initialPos;
        yield return new WaitForSeconds(0.1f);
        GetComponent<Renderer>().enabled = false;
        yield return new WaitForSeconds(0.1f);
        GetComponent<Renderer>().enabled = true;
        yield return new WaitForSeconds(0.1f);
        GetComponent<Renderer>().enabled = false;
        yield return new WaitForSeconds(0.1f);
        GetComponent<Renderer>().enabled = true;
        yield return new WaitForSeconds(0.1f);
        GetComponent<Renderer>().enabled = false;
        yield return new WaitForSeconds(0.1f);
        GetComponent<Renderer>().enabled = true;
    }

   
}
