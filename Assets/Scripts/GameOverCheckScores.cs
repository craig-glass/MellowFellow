using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class GameOverCheckScores : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Button button;
    public GameObject cameraMain;
    string difficultyLevel;
    [SerializeField]
    GameObject highScores;
    public Text inputText;
  

    // Start is called before the first frame update
    void Start()
    {
        // Add event listener to continue button so player can 
        // click continue with the mouse
        button = GetComponent<Button>();
        button.onClick.AddListener(ReloadGame);
    }

    // Update is called once per frame
    void Update()
    {
        // Listen for return being pressed to continue
        if (Input.GetKeyUp(KeyCode.Return)){
            
            ReloadGame();
        }
    }

    void ReloadGame()
    {
        // clear high score lists to avoid duplicates being displayed
        HighScoreTable.rookieScores.Clear();
        HighScoreTable.amateurScores.Clear();
        HighScoreTable.proScores.Clear();

        // Store player's inputted name and save name in text file
        HighScoreTable.playerName = inputText.text;
        if (HighScoreTable.playerName != "")
        {
            HighScoreTable.SaveHighScores(YellowFellowGame.difficultyLevel);
            HighScoreTable.playerName = "";
        }
        // Reset level to 1 and load main menu scene
        Level.levelIndex = 1;
        SceneManager.LoadScene(0);

    }

    
    public void OnPointerEnter(PointerEventData eventData)
    {
        button.GetComponentInChildren<Text>().fontSize = 24;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        button.GetComponentInChildren<Text>().fontSize = 20;
    }


}
