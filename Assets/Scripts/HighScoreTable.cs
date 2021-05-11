using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;
using UnityEngine.SceneManagement;

public class HighScoreTable : MonoBehaviour
{
    [SerializeField]
    string highscoreFile = "scores.txt";
    public static string playerName;
    

    public struct HighScoreEntry
    {
        public int score;
        public string name;
        public string difficulty;
    }

    public static List<HighScoreEntry> rookieScores = new List<HighScoreEntry>();
    public static List<HighScoreEntry> amateurScores = new List<HighScoreEntry>();
    public static List<HighScoreEntry> proScores = new List<HighScoreEntry>();


    // Start is called before the first frame update
    void Start()
    {
        
        
        
    }

   void Update()
    {
        
    }


    public void LoadHighScoreTable()
    {
        using (TextReader file = File.OpenText(highscoreFile))
        {
            string text = null;
            
            while ((text = file.ReadLine()) != null)
            {
                
                string[] splits = text.Split(',');
                HighScoreEntry entry;
                entry.name = splits[0];
                entry.score = int.Parse(splits[1]);
                entry.difficulty = splits[2];
                if(entry.difficulty == "Rookie")
                {
                    rookieScores.Add(entry);
                }
                else if(entry.difficulty == "Amateur")
                {
                    amateurScores.Add(entry);
                }
                else
                {
                    proScores.Add(entry);
                }
                
               
            }
            
        }
       
    }

    [SerializeField]
    Font scoreFont;

    public static void SaveHighScores(string difficultyLevel)
    {
        
        using (StreamWriter writer = new StreamWriter("scores.txt", true))
        {
            writer.WriteLine(playerName + "," + Fellow.score + "," + difficultyLevel);
            
        }
    }

    public void CreateHighScoreText(List<HighScoreEntry> scores)
    {
        for(int i = 0; i < scores.Count && i < 11; ++i)
        {
            GameObject o = new GameObject();
            o.transform.parent = transform;
            
            Text t = o.AddComponent<Text>();

            
            t.text = scores[i].name; 
            t.font = scoreFont;
            t.fontSize = 50;
            

            o.transform.localPosition = new Vector3(0, -(i) * 6, 0);
            o.transform.localRotation = Quaternion.identity;
            o.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);

            o.GetComponent<RectTransform>().sizeDelta = new Vector2(600, 100);

            GameObject s = new GameObject();
            s.transform.parent = transform;

            Text t2 = s.AddComponent<Text>();
            t2.text = String.Format("{0, 37}", scores[i].score.ToString());
            t2.font = scoreFont;
            t2.fontSize = 50;


            s.transform.localPosition = new Vector3(0, -(i) * 6, 0);
            s.transform.localRotation = Quaternion.identity;
            s.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);

            s.GetComponent<RectTransform>().sizeDelta = new Vector2(600, 100);

            
        }
    }
  
    public void SortHighScoreEntries()
    {
        rookieScores.Sort((x, y) => y.score.CompareTo(x.score));
        amateurScores.Sort((x, y) => y.score.CompareTo(x.score));
        proScores.Sort((x, y) => y.score.CompareTo(x.score));
    }

   
}
