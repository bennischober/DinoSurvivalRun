using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadScores : MonoBehaviour
{
    // get containers of high score table
    private Transform entryContainer;
    private Transform entryTemplate;

    //private List<PlayerHighscore> highscoreEntryList;
    private List<Transform> highscoreEntryTransformList;


    private void Awake()
    {
        // parent object
        entryContainer = transform.Find("highscoreEntryContainer");
        //child
        entryTemplate = entryContainer.Find("highscoreEntryTemplate");

        if (entryTemplate != null) entryTemplate.gameObject.SetActive(false);

        LoadPlayerHighscore();
    }

    // creates the 3 text objects and assigns the text to them - based on the current object of the foreach loop in LoadPlayerHighscore()
    private void CreateHighscoreEntryTransform(PlayerHighscore highscoreEntry, Transform container,
        List<Transform> transformList)
    {
        var templateHeight = 50f;
        var entryTransform = Instantiate(entryTemplate, container);
        var entryRectTransform = entryTransform.GetComponent<RectTransform>();
        entryRectTransform.anchoredPosition = new Vector2(0, -templateHeight * transformList.Count);
        entryTransform.gameObject.SetActive(true);

        var rank = transformList.Count + 1;
        string rankString;
        switch (rank)
        {
            default:
                rankString = rank + "TH";
                break;
            case 1:
                rankString = "1ST";
                break;
            case 2:
                rankString = "2ND";
                break;
            case 3:
                rankString = "3RD";
                break;
        }

        entryTransform.Find("posText").GetComponent<Text>().text = rankString;

        var score = highscoreEntry.score;
        entryTransform.Find("scoreText").GetComponent<Text>().text = score.ToString();

        var name = highscoreEntry.name;
        entryTransform.Find("nameText").GetComponent<Text>().text = name;

        transformList.Add(entryTransform);
    }

    // loads the JSON file and creates the highscore table
    public void LoadPlayerHighscore()
    {
        var jsonString = PlayerPrefs.GetString("allHighscores");
        var highscores = JsonUtility.FromJson<Highscores>(jsonString);
        Debug.Log(jsonString);

        // set transform for score table and so on!
        highscoreEntryTransformList = new List<Transform>();
        foreach (PlayerHighscore highscoreEntry in highscores.playerHighscoreList)
        {
            CreateHighscoreEntryTransform(highscoreEntry, entryContainer, highscoreEntryTransformList);
        }
    }

    // class for PlayerHighscore list -> to open and save scores
    private class Highscores
    {
        public List<PlayerHighscore> playerHighscoreList;
    }

    // class for scores
    [Serializable]
    private class PlayerHighscore
    {
        public string name;
        public int score;
        public int jumps;
        public int walked;
    }
}
