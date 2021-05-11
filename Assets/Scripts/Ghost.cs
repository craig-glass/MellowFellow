using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Ghost : MonoBehaviour
{
    public NavMeshAgent agent;
    Fellow playerScript;
    GameObject player;
    [SerializeField]
    Material scaredMaterial;
    [SerializeField]
    Material normalMaterial;
    public Vector3 initialPosition;
    public bool beenEaten;
    YellowFellowGame yellowGameScript;
    private CapsuleCollider capsuleCollider;
    private GameObject leftTeleporter;
    private GameObject rightTeleporter;
    public AudioClip teleportSound;
    private AudioSource audioSource;
    private Vector3 teleportOffset = new Vector3(2, 0, 0);
    private Fellow fellowScript;
    public AudioClip ghostEaten;
    public GameObject extraLife;
    



    // Start is called before the first frame update
    void Start()
    {
        
        
        capsuleCollider = gameObject.GetComponent<CapsuleCollider>();
        initialPosition = transform.position;
        agent = GetComponent<NavMeshAgent>();
        yellowGameScript = GameObject.Find("Game").GetComponent<YellowFellowGame>();
        normalMaterial = GetComponent<Renderer>().material;
        player = GameObject.Find("Fellow");
        playerScript = GameObject.Find("Fellow").GetComponent<Fellow>();
        leftTeleporter = GameObject.Find("LeftTeleporter");
        rightTeleporter = GameObject.Find("RightTeleporter");
        audioSource = GetComponent<AudioSource>();
        fellowScript = GameObject.Find("Fellow").GetComponent<Fellow>();
        
        
    }

    public Vector3 PickRandomPosition()
    {
        Vector3 destination = transform.position;
        Vector2 randomDirection = Random.insideUnitCircle * 8.0f;
        destination.x += randomDirection.x;
        destination.z += randomDirection.y;

        NavMeshHit navHit;
        NavMesh.SamplePosition(destination, out navHit, 8.0f, NavMesh.AllAreas);

        return navHit.position;
    }

    bool hiding = false;

    // Update is called once per frame
    void Update()
    {
        if (playerScript.PowerupActive())
        {
            
            if((!hiding && !beenEaten) || (agent.remainingDistance < 0.5f && !beenEaten))
            {
                hiding = true;
                agent.SetDestination(PickHidingPlace());
                GetComponent<Renderer>().material = scaredMaterial;
                capsuleCollider.isTrigger = true;
            }
            
        }
        else
        {
            beenEaten = false;
            capsuleCollider.isTrigger = false;
            if (hiding)
            {
                
                GetComponent<Renderer>().material = normalMaterial;
                hiding = false;
            }else if (CanSeePlayer())
            {
                agent.SetDestination(player.transform.position);
                                
            }
           

            if (!beenEaten && agent.remainingDistance < 0.5f && yellowGameScript.gameMode == YellowFellowGame.GameMode.InGame && !CanSeePlayer())
            {
                if(gameObject.name == "Ghost_red")
                {
                    agent.SetDestination(player.transform.position);
                }
                else
                {
                    agent.SetDestination(PickRandomPosition());
                    hiding = false;
                    beenEaten = false;
                    
                    GetComponent<Renderer>().material = normalMaterial;
                }
                
               
            }
        }

    }

    bool CanSeePlayer()
    {
        Vector3 rayPos = transform.position;
        Vector3 rayDir = (player.transform.position - rayPos).normalized;

        RaycastHit info;
        if(Physics.Raycast(rayPos, rayDir, out info))
        {
            if (info.transform.CompareTag("Fellow"))
            {
                return true;
            }
        }
        return false;
    }

    Vector3 PickHidingPlace()
    {
        Vector3 directionToPlayer = (player.transform.position - transform.position).normalized;

        NavMeshHit navHit;
        NavMesh.SamplePosition(transform.position - (directionToPlayer * 2.0f), 
            out navHit, 2.0f, NavMesh.AllAreas);

        return navHit.position;
    }


    

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Fellow"))
        {
            if (!beenEaten)
            {
                StartCoroutine(GhostRespawnOnEaten());

                if (yellowGameScript.ghostsEaten > -1)
                {
                    

                    foreach (GameObject ghost in yellowGameScript.ghosts)
                    {

                        if (ghost.GetComponent<Ghost>().beenEaten)
                        {
                            yellowGameScript.ghostsEaten++;
                            
                        }

                    }
                    if (yellowGameScript.ghostsEaten == yellowGameScript.ghosts.Length)
                    {
                        Fellow.lives++;
                        // Play extra life sound


                        // Display extra life text
                        
                        StartCoroutine(ExtraLife());

                        yellowGameScript.ghostsEaten = -1;
                    }
                    else
                    {
                        yellowGameScript.ghostsEaten = 0;
                    }


                }
            }
            
        }

        if (other.gameObject.CompareTag("RightTeleporter"))
        {
            
            agent.Warp(leftTeleporter.transform.position + teleportOffset);
            audioSource.volume = 0.9f;
            audioSource.PlayOneShot(teleportSound);
            
        }

        if (other.gameObject.CompareTag("LeftTeleporter"))
        {
            
            agent.Warp(rightTeleporter.transform.position - teleportOffset);
            audioSource.volume = 0.9f;
            audioSource.PlayOneShot(teleportSound);
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        

        if (collision.gameObject.CompareTag("Fellow") && !playerScript.PowerupActive())
        {
            foreach (GameObject ghost in yellowGameScript.ghosts)
            {
                ghost.gameObject.GetComponent<NavMeshAgent>().Warp(initialPosition);
                ghost.gameObject.GetComponent<NavMeshAgent>().SetDestination(PickRandomPosition());
               
            }
            
        }
    }


    IEnumerator GhostRespawnOnEaten()
    {
        beenEaten = true;
        playerScript.ghostsEaten++;

        audioSource.PlayOneShot(ghostEaten);
        agent.Warp(initialPosition);

        
        GetComponent<Renderer>().enabled = false;
        yield return new WaitForSeconds(0.2f);
        GetComponent<Renderer>().enabled = true;
        yield return new WaitForSeconds(0.2f);
        GetComponent<Renderer>().enabled = false;
        yield return new WaitForSeconds(0.2f);
        GetComponent<Renderer>().enabled = true;
        yield return new WaitForSeconds(0.2f);
        GetComponent<Renderer>().enabled = false;
        yield return new WaitForSeconds(0.2f);
        GetComponent<Renderer>().enabled = true;
        yield return new WaitForSeconds(0.2f);
        GetComponent<Renderer>().enabled = false;
        yield return new WaitForSeconds(0.2f);
        GetComponent<Renderer>().enabled = true;
        yield return new WaitForSeconds(0.2f);

    }

    IEnumerator ExtraLife()
    {
        extraLife.SetActive(true);
        yield return new WaitForSeconds(3);
        extraLife.SetActive(false);
    }
}
