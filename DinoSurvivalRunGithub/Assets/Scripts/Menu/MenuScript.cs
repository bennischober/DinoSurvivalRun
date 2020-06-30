using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuScript : MonoBehaviour
{
    // get player class
    private PlayerController _playerController;

    // get GO's to enable and disable them
    public GameObject mainButtonsPanel;
    public GameObject namePanel;
    public GameObject scoresPanel;

    // text object to input player name
    public GameObject getPlayerName;
    private string playerName_s;

    // get input of text field for player name
    public Text playerName;


    // Start is called before the first frame update
    void Start()
    {
        _playerController = GameObject.FindObjectOfType<PlayerController>();
    }

    // prints name via getString to text field
    void OnGUI()
    {
        playerName_s = "Welcome " + PlayerPrefs.GetString("CurrentPlayerName", "Default Name") + "!";
        getPlayerName.GetComponent<Text>().text = playerName_s;
    }

    // function for click event in name panel, saves the player name to 
    public void SaveName()
    {
        Debug.Log(playerName.text);
        PlayerPrefs.SetString("CurrentPlayerName", playerName.text);
    }

    // opens/enables the name panel and disables the main buttons panel
    public void OpenPanelName()
    {
        if ((namePanel != null))
        {
            namePanel.SetActive(!namePanel.gameObject.activeSelf);
            mainButtonsPanel.SetActive(!mainButtonsPanel.gameObject.activeSelf);
        }
    }

    // opens score and disables all main buttons
    public void OpenPanelScores()
    {
        if ((scoresPanel != null))
        {
            scoresPanel.SetActive(!scoresPanel.gameObject.activeSelf);
            mainButtonsPanel.SetActive(!mainButtonsPanel.gameObject.activeSelf);
        }
    }

    // quits the game
    public void QuitGame()
    {
        Application.Quit();
    }
}
