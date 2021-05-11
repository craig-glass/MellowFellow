using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class StartGame : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject game;
    private YellowFellowGame yellowScript;
    private Button button;
    public int difficulty;
    
    

    // Start is called before the first frame update
    void Start()
    {
        yellowScript = game.GetComponent<YellowFellowGame>();

        button = GetComponent<Button>();
        button.onClick.AddListener(SetDifficulty);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SetDifficulty()
    {
        
        yellowScript.StartGame(difficulty);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        button.GetComponentInChildren<Text>().fontSize = 30;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        button.GetComponentInChildren<Text>().fontSize = 26;
    }
}
