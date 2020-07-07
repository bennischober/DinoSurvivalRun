using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    // get isGameOver_b
    private PlayerController _playerController;

    // hp changes in GUI
    public GameObject changeHPPanel, loseHP_GO, gainHP_GO;
    [NonSerialized] public Animator loseHP_Anim, gainHP_Anim;

    // get whole menu panel
    public GameObject menuPanel, statsPanel, currentStatsPanel, gameOverPanel, getName;

    // text fields in stats window
    public GameObject getNameStats, allJumpsStats, currentJumpsStats, bestJumpsStats, ScoreSystemStats, meterWalkedStats;

    // get text fields in GUI -> could be moved to a GUI manager
    public GameObject scoreText, endScoreText, playerHp, metersRan, bestScoreText;

    // variables for stats output
    private string playerName_s, obstacleJump_s, scoreSystem_s, meterRan_s, playerJumps_s, highestJumps_s;

    // checks, if game is paused
    [NonSerialized] public bool isGamePaused_b;

    // check, if stat window is opened
    private bool isStatOpen_b;


    // Start is called before the first frame update
    void Start()
    {
        _playerController = GameObject.FindObjectOfType<PlayerController>();

        loseHP_Anim = loseHP_GO.GetComponent<Animator>();
        gainHP_Anim = gainHP_GO.GetComponent<Animator>();

        loseHP_GO.GetComponent<Text>().text = "-1";
        gainHP_GO.GetComponent<Text>().text = "+1";

        isGamePaused_b = false;
        isStatOpen_b = false;
    }

    // Update is called once per frame
    void Update()
    {
        // open pause menu on ESC click and set pause boolean to true
        if (Input.GetKeyDown(KeyCode.Escape) && menuPanel != null && !_playerController.isGameOver_b)
        {
            if (!isStatOpen_b)
            {
                isGamePaused_b = !isGamePaused_b;
                menuPanel.SetActive(!menuPanel.gameObject.activeSelf);

                playerName_s = PlayerPrefs.GetString("CurrentPlayerName", "DefaultName");
                getName.GetComponent<Text>().text = playerName_s;
            }
        }

        // pause time in game!
        if (isGamePaused_b)
        {
            Time.timeScale = 0;
        }
        else if (!isGamePaused_b)
        {
            Time.timeScale = 1;
        }

        if (_playerController.isGameOver_b)
        {
            currentStatsPanel.SetActive(false);
            gameOverPanel.SetActive(true);
        }

        // set stats

        if (isStatOpen_b)
        {
            scoreSystem_s = "How the score system works: Your valid obstacle jumps get multiplied by the meters you walked and by another value. Calculation: (int)(ObstacleJump * MetersWalked * 0.1)";
            ScoreSystemStats.GetComponent<Text>().text = scoreSystem_s;

            meterRan_s = "Meters walked: " + _playerController.realMeterWalked_i;
            meterWalkedStats.GetComponent<Text>().text = meterRan_s;

            playerJumps_s = "Times jumped: " + _playerController.countPlayerJumps_i;
            allJumpsStats.GetComponent<Text>().text = playerJumps_s;

            obstacleJump_s = "Obstacle jumps: " + _playerController.countObstacleJump_i;
            currentJumpsStats.GetComponent<Text>().text = obstacleJump_s;

            highestJumps_s = "Best score: " + _playerController.highestScore_i;
            bestJumpsStats.GetComponent<Text>().text = highestJumps_s;
        }
    }

    // function to close the pause menu
    public void CloseMenu()
    {
        if (isGamePaused_b)
        {
            isGamePaused_b = !isGamePaused_b;
            menuPanel.SetActive(!menuPanel.gameObject.activeSelf);
        }
    }

    public void OpenStats()
    {
        if (isGamePaused_b)
        {
            isStatOpen_b = !isStatOpen_b;
            menuPanel.SetActive(!menuPanel.gameObject.activeSelf);
            statsPanel.SetActive(!statsPanel.gameObject.activeSelf);

            playerName_s = PlayerPrefs.GetString("CurrentPlayerName", "DefaultName");
            getNameStats.GetComponent<Text>().text = playerName_s;
        }
    }

    public void RestartGame()
    {
        if (_playerController.isGameOver_b)
        {
            StartCoroutine(ReloadGame());
        }
    }

    public void ExitButtonPressed()
    {
        StartCoroutine(LoadMainMenu());
    }

    IEnumerator LoadMainMenu()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("MenuScene", LoadSceneMode.Single);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }

    IEnumerator ReloadGame()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("GameSceneMain", LoadSceneMode.Single);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}
