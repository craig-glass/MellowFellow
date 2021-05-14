using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Playables;


public class YellowFellowGame : MonoBehaviour
{
    private Scene activeScene;

    [SerializeField]
    GameObject highScoreUI;
    [SerializeField]
    GameObject highScores;
    [SerializeField]
    GameObject highScoreDifficultyText;
    
    static GameObject mainMenuUI;

    [SerializeField]
    public GameObject gameUI;

    public GameObject gameover;

    [SerializeField]
    GameObject winUI;

    [SerializeField]
    GameObject timeTakenText;

    [SerializeField]
    GameObject powerupsUsedText;

    [SerializeField]
    GameObject ghostsEatenText;

    [SerializeField]
    GameObject timerText;

    
    public GameObject ghost1;
    public GameObject ghost2;
    public GameObject ghost3;
    

    public GameObject ghostPrefab;
    public GameObject[] pellets;

    Fellow fellowScript;
    public AudioClip menuSelect;
    public AudioClip levelCompleteSound;
    public AudioSource audioSource;
    public bool levelComplete;
    
    NavMeshAgent ghostAgent;
    Ghost ghost;
    public GameObject[] ghosts;
    public int deductScore;
    public GameObject gameOverText;
    
    [SerializeField]
    GameObject playerFellow;
    public GameObject fire;
    public static int level;
    GameObject mainMenuUII;
    private int lessPowerupScore;
    private int lessTimeTakenScore;
    private int addGhostsEatenScore;
    public AudioClip scoreCount;
    public static string difficultyLevel = "";
    InputField playerName;
    public bool timeStopped;
    public GameObject shotList;
    static bool initialLoad = true;
    public int ghostsEaten;
    public static int difficultyGlobal;
    public GameObject highScoreText;
    HighScoreTable highScoreTableScript;
    

    public enum GameMode
    {
        InGame,
        MainMenu,
        HighScores
    }

    
    public GameMode gameMode = GameMode.MainMenu;

    
    // Start is called before the first frame update
    void Start()
    {
        
        ghostsEaten = 0;
        timeStopped = false;
        activeScene = SceneManager.GetActiveScene();
        fellowScript = GameObject.FindGameObjectWithTag("Fellow").GetComponent<Fellow>();
        pellets = GameObject.FindGameObjectsWithTag("Pellet");
        

        mainMenuUI = GameObject.Find("MainMenuUI");
        
        highScoreTableScript = highScores.GetComponent<HighScoreTable>();
        


        highScoreTableScript.LoadHighScoreTable();
        highScoreTableScript.SortHighScoreEntries();

        switch (difficultyGlobal)
        {
            case 1:
                highScoreText.GetComponent<Text>().text = HighScoreTable.rookieScores[0].name + "  "
            + HighScoreTable.rookieScores[0].score;
                highScoreText.GetComponent<Text>().color = new Color(0f, 0f, 1f);
                break;
            case 2:
                highScoreText.GetComponent<Text>().text = HighScoreTable.amateurScores[0].name + "  "
            + HighScoreTable.amateurScores[0].score;
                highScoreText.GetComponent<Text>().color = new Color(0f, 1f, 0f);
                break;
            case 3:
                highScoreText.GetComponent<Text>().text = HighScoreTable.proScores[0].name + "  "
            + HighScoreTable.proScores[0].score;
                highScoreText.GetComponent<Text>().color = new Color(1f, 0f, 0f);
                break;
        }


        if (activeScene.buildIndex == 0 && initialLoad)
        {
            
            StartMainMenu();
            initialLoad = false;

        }
        else if (activeScene.buildIndex == 0 && !initialLoad)
        {
           
            StartMainMenu();
            TurnOffOpeningCutScene();
        }
        else
        {
           
            gameMode = GameMode.InGame;
            fellowScript.powerupDuration = fellowScript.powerupDuration / difficultyGlobal;
        }

      
        

        if (Level.levelIndex == 2 && activeScene.buildIndex != 1)
        {
            ghost1.SetActive(true);
        }
        if(Level.levelIndex == 3 && activeScene.buildIndex != 1)
        {
            ghost1.SetActive(true);
            ghost2.SetActive(true);
        }
        if(Level.levelIndex == 4 && activeScene.buildIndex != 1)
        {
            ghost1.SetActive(true);
            ghost2.SetActive(true);
            ghost3.SetActive(true);
        }


        ghosts = GameObject.FindGameObjectsWithTag("Ghost");

        for (int i = 0; i < ghosts.Length; i++)
        {
            ghosts[i].SetActive(true);
        }

                  
        
        
    }

    // Update is called once per frame
    void Update()
    {
        
       if (fellowScript.PelletsEaten() == pellets.Length)
        {
            playerFellow.SetActive(false);
            levelComplete = true;
            fellowScript.pelletsEaten = 0;

        }
        

        switch (gameMode)
        {
            case GameMode.MainMenu:     UpdateMainMenu(); break;
            case GameMode.HighScores:   UpdateHighScores(); break;
            case GameMode.InGame:       UpdateMainGame(); break;
        }
        
    }

    void UpdateMainMenu()
    {
        
        if(Input.GetKeyDown(KeyCode.R))
        {
            StartHighScores(HighScoreTable.rookieScores);
           
            audioSource.PlayOneShot(menuSelect);
            highScoreDifficultyText.GetComponent<Text>().text = "Rookie";
           
            highScoreDifficultyText.GetComponent<Text>().color = new Color(0f, 0f, 1f);
            

        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            
            audioSource.PlayOneShot(menuSelect);
            highScoreDifficultyText.GetComponent<Text>().text = "Amateur";
            highScoreDifficultyText.GetComponent<Text>().color = new Color(0f, 1f, 0f);
            StartHighScores(HighScoreTable.amateurScores);
        }
        else if (Input.GetKeyDown(KeyCode.P))
        {
            audioSource.PlayOneShot(menuSelect);
            highScoreDifficultyText.GetComponent<Text>().text = "Pro";
            highScoreDifficultyText.GetComponent<Text>().color = new Color(1f, 0f, 0f);
            StartHighScores(HighScoreTable.proScores);
        }
    }

    void UpdateHighScores()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            
            {
                
                StartMainMenu();
                audioSource.PlayOneShot(menuSelect);
                foreach (Transform newGameObject in highScores.transform)
                {
                    GameObject.Destroy(newGameObject.gameObject);
                }
                
            }
            
        }
    }

    void UpdateMainGame()
    {
       if(Fellow.lives < 1)
        {
            
            StartCoroutine(GameOver());
            
        }
        if (levelComplete)
        {
            LevelComplete();
            
            levelComplete = false;
        }
      
    }

    void StartMainMenu()
    {
        
        gameMode                        = GameMode.MainMenu;
        mainMenuUI.gameObject.SetActive(true);
        highScoreUI.gameObject.SetActive(false);
        gameUI.gameObject.SetActive(false);
        
    }


    public void StartHighScores(List<HighScoreTable.HighScoreEntry> highScores)
    {
        highScoreTableScript.CreateHighScoreText(highScores);
        gameMode                = GameMode.HighScores;
        mainMenuUI.gameObject.SetActive(false);
        highScoreUI.gameObject.SetActive(true);
        gameUI.gameObject.SetActive(false);
        gameover.gameObject.SetActive(false);
    }

    

    public void StartGame(int difficulty)
    {

        gameMode                = GameMode.InGame;
        mainMenuUI.gameObject.SetActive(false);
        highScoreUI.gameObject.SetActive(false);
        fellowScript.powerupDuration /= difficulty;
        gameUI.gameObject.SetActive(true);
        gameover.gameObject.SetActive(false);
        playerFellow.gameObject.SetActive(true);
        Fellow.lives = 3;
        Fellow.score = 0;


        if (difficulty == 1)
        {
            difficultyLevel = "Rookie";
        }
        else if (difficulty == 2)
        {
            difficultyLevel = "Amateur";
        }
        else
        {
            difficultyLevel = "Pro";
        }
        difficultyGlobal = difficulty;

        switch (difficulty)
        {
            case 1:
                highScoreText.GetComponent<Text>().text = HighScoreTable.rookieScores[0].name + "  "
            + HighScoreTable.rookieScores[0].score;
                highScoreText.GetComponent<Text>().color = new Color(0f, 0f, 1f);
                break;
            case 2:
                highScoreText.GetComponent<Text>().text = HighScoreTable.amateurScores[0].name + "  "
            + HighScoreTable.amateurScores[0].score;
                highScoreText.GetComponent<Text>().color = new Color(0f, 1f, 0f);
                break;
            case 3:
                highScoreText.GetComponent<Text>().text = HighScoreTable.proScores[0].name + "  "
            + HighScoreTable.proScores[0].score;
                highScoreText.GetComponent<Text>().color = new Color(1f, 0f, 0f);
                break;
        }
    }

    private void LevelComplete()
    {
        timeStopped = true;

        audioSource.PlayOneShot(levelCompleteSound);

        if (activeScene.buildIndex != 1)
        {
            for (int i = 0; i < ghosts.Length; i++)
            {
                ghosts[i].SetActive(false);
            }
            if (activeScene.buildIndex == 2)
            {
                ghost1.SetActive(false);
                ghost2.SetActive(false);
            }
        }
        
        

        StartCoroutine(FinalScore(Fellow.score));   
        
    }


    IEnumerator GameOver()
    {
        timeStopped = true;
        playerFellow.SetActive(false);

        for( int i = 0; i < ghosts.Length; i++)
        {
            ghosts[i].SetActive(false);
        }
        if (activeScene.buildIndex == 2)
        {
            ghost1.SetActive(false);
            ghost2.SetActive(false);
        }
        yield return null;

        gameOverText.SetActive(true);
        yield return new WaitForSeconds(2);

        playerName = GameObject.Find("InputName").GetComponent<InputField>();
        playerName.Select();

    }

    IEnumerator FinalScore(int score)
    {
        winUI.SetActive(true);
        if(activeScene.buildIndex == 1)
        {
            ghostsEatenText.SetActive(false);
        }
        

        int timeTaken = TimeTaken();
       
        lessTimeTakenScore = Fellow.score - (timeTaken * 10);
        
        timeTakenText.GetComponent<Text>().text = "Time Taken = " + timeTaken.ToString() + " Seconds";

        yield return new WaitForSeconds(2);

        while (lessTimeTakenScore != Fellow.score)
        {
            audioSource.PlayOneShot(scoreCount);
            Fellow.score -= 5;
            timeTakenText.GetComponent<Text>().text = "Score = " + Fellow.score;

            yield return null;
        }

        lessPowerupScore = Fellow.score - (fellowScript.powerupsUsed * 100);

       
        powerupsUsedText.GetComponent<Text>().text = "Powerups Used = " + fellowScript.powerupsUsed;

        yield return new WaitForSeconds(2);

        while (lessPowerupScore != Fellow.score) {

            audioSource.PlayOneShot(scoreCount);
            Fellow.score -= 5;
            powerupsUsedText.GetComponent<Text>().text = "Score = " + Fellow.score;

            yield return null;
        
        }

        addGhostsEatenScore = Fellow.score + (fellowScript.ghostsEaten * 100);

        ghostsEatenText.GetComponent<Text>().text = "Ghosts Eaten = " + fellowScript.ghostsEaten;

        yield return new WaitForSeconds(2);

        while (addGhostsEatenScore != Fellow.score)
        {
            audioSource.PlayOneShot(scoreCount);
            Fellow.score += 5;
            ghostsEatenText.GetComponent<Text>().text = "Score = " + Fellow.score;

            yield return null;
        }


        yield return new WaitForSeconds(1);

        if(Level.levelIndex == 4 && activeScene.buildIndex != 1)
        {
            GameComplete(); 
        }
        else if(activeScene.buildIndex != 1 && TimeTaken() < 60)
        {
            
            SceneManager.LoadScene(1);
        }
        else
        {
            Level.levelIndex++;
            SceneManager.LoadScene(2);
        }
        


    }
    
    int TimeTaken()
    {
        string timeTook = timerText.GetComponent<Text>().text;
        string[] timeTookSplit = timeTook.Split(':');
        string minutes = timeTookSplit[1];
        string seconds = timeTookSplit[2];

        int timeTaken = Convert.ToInt32(seconds) + (Convert.ToInt32(minutes) * 60);

        return timeTaken;
    }

    void TurnOffOpeningCutScene()
    {
        shotList.SetActive(false);
    }

    void GameComplete()
    {
        
        winUI.SetActive(false);
        gameOverText.GetComponentInChildren<Text>().text = "Game Complete!";
        gameOverText.GetComponentInChildren<Text>().color = new Color(0f, 1f, 0f);
        gameOverText.SetActive(true);
        new WaitForSeconds(2);

        playerName = GameObject.Find("InputName").GetComponent<InputField>();
        playerName.Select();
    }

}
