using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.UI;
using Vector3 = UnityEngine.Vector3;

public class PlayerController : MonoBehaviour
{
    // get rigid body of player
    private Rigidbody playerRb;

    // get Animator of player
    private Animator playerAnimator;

    // get Particle System of player
    public ParticleSystem dirtParticle;

    // get isGamePaused_b
    private PauseMenu _pauseMenu;

    // get SHIFT cooldown
    private BetterJump _betterJump;

    // get player y
    public GameObject playerJumpHeight;

    // jump force -> get's its value in editor
    public float jumpForce_f = 10;

    // public float gravityModifier_f;
    public bool isOnGround_b = true, isGameOver_b;

    // name for player high score saves
    private string playerNameHighscore_s;

    // check if HP is lost or gained
    private bool lostHp_b;

    // !-- VARIABLES FOR COUNTER --! \\
    // best score
    [NonSerialized] public int highestScore_i, currentScore_i;

    // count events
    [NonSerialized] public int countObstacleJump_i, countPlayerJumps_i;

    // counts if player hits an obstacle -> used to run a spawn hp method only once!
    [NonSerialized] public int countObstacleHit_i;

    // player hp
    [NonSerialized] public int playerHp_i;

    // collected hp
    [NonSerialized] public int playerHpCollected_i;

    // time to score == meter walked
    [NonSerialized] public int realMeterWalked_i;
    private int meterWalked_i;
    private float timeToInt_f;

    // count in GUI bar on top
    private string highestScore_s, currentScore_s, playerHp_s, meterRan_s;

    // !-- END OF VARIABLES FOR COUNTER --! \\

    // Start is called before the first frame update
    void Start()
    {
        _pauseMenu = GameObject.FindObjectOfType<PauseMenu>();
        _betterJump = GameObject.FindObjectOfType<BetterJump>();

        playerRb = GetComponent<Rigidbody>();
        playerAnimator = GameObject.Find("PlayerCharacter").GetComponent<Animator>();

        // assign start values
        countObstacleJump_i = 0;
        countPlayerJumps_i = 0;
        playerHp_i = 3;
        meterWalked_i = 0;
        timeToInt_f = Time.deltaTime;
        realMeterWalked_i = 0;
        playerHpCollected_i = 0;
        countObstacleHit_i = 0;
        currentScore_i = 0;

        // get highest score
        highestScore_i = PlayerPrefs.GetInt("HighestScore", 0);

        playerAnimator.SetBool("isDead_b", false);
    }

    // Update is called once per frame
    void Update()
    {
        // jump calculation
        if ((Input.GetKeyDown(KeyCode.Space) && isOnGround_b) && !isGameOver_b)
        {
            playerRb.velocity = Vector3.up * jumpForce_f;

            isOnGround_b = false;
            Debug.Log("Player is in Air!");

            //count player jumps
            countPlayerJumps_i++;
            Debug.Log(countPlayerJumps_i);

            // animation to jump
            playerAnimator.SetTrigger("Jump_trig");
            playerAnimator.SetBool("isGrounded_b", false);

            // stop particles in air
            dirtParticle.Stop();
        }

        // get current counters for GUI bar on top
        playerHp_s = "HP: " + playerHp_i;
        _pauseMenu.playerHp.GetComponent<Text>().text = playerHp_s;

        currentScore_i = (int)(countObstacleJump_i * (realMeterWalked_i * 0.1));
        currentScore_s = "Current score: " + currentScore_i;
        _pauseMenu.scoreText.GetComponent<Text>().text = currentScore_s;

        highestScore_s = "Best score: " + highestScore_i;
        _pauseMenu.bestScoreText.GetComponent<Text>().text = highestScore_s;

        // counter for meters walked, doesn't count, if pause menu is opened or gameOver
        if (!_pauseMenu.isGamePaused_b && !isGameOver_b)
        {
            timeToInt_f = Time.deltaTime * 1000;
            meterWalked_i += (int)timeToInt_f;
            realMeterWalked_i = meterWalked_i / 100;

            if (_betterJump.cooldownSeconds_i == 5)
            {
                meterRan_s = "SHIFT not on CD!";
                _pauseMenu.metersRan.GetComponent<Text>().text = meterRan_s;
            }
            else if (_betterJump.cooldownSeconds_i == 0)
            {
                meterRan_s = "SHIFT not on CD!";
                _pauseMenu.metersRan.GetComponent<Text>().text = meterRan_s;
            }
            else
            {
                meterRan_s = "SHIFT up in: " + _betterJump.cooldownSeconds_i;
                _pauseMenu.metersRan.GetComponent<Text>().text = meterRan_s;
            }

        }

        // end screen score
        if (isGameOver_b)
        {
            currentScore_s = "Your score was: " + currentScore_i;
            _pauseMenu.endScoreText.GetComponent<Text>().text = currentScore_s;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // pickup hp and destroy the object + increase hp by 1
        if (collision.gameObject.CompareTag("PickupLife"))
        {
            playerHp_i++;
            playerHpCollected_i++;

            HpChange("gain");

            Destroy(GameObject.FindWithTag("PickupLifeParent"));
            Debug.Log("HP received");
        }

        // check if player is on ground -> can't jump in air!
        if (collision.gameObject.CompareTag("Ground"))
        {
            isOnGround_b = true;
            playerAnimator.SetBool("isGrounded_b", true);

            // play particles
            dirtParticle.Play();
        }

        // hitting obstacles, hp and end states are calculated here
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            countObstacleHit_i = 1;
            if (playerHp_i >= 1)
            {
                playerHp_i--;

                HpChange("lost");

                if (playerHp_i > 0)
                {
                    Destroy(GameObject.FindWithTag("ObstacleParent"));
                }
            }

            if (playerHp_i == 0)
            {
                PlayerDieAction();
            }
        }

        if (collision.gameObject.CompareTag("FlyEnemy"))
        {
            if (playerHp_i >= 1)
            {
                playerHp_i--;

                HpChange("lost");

                if (playerHp_i > 0)
                {
                    Destroy(GameObject.FindWithTag("FlyEnemyParent"));
                }
            }

            if (playerHp_i == 0)
            {
                PlayerDieAction();
            }
        }
    }

    private void PlayerDieAction()
    {
        Debug.Log("Game Over!");
        isGameOver_b = true;

        // set isDead_b to true -> dead / idle animation will start
        playerAnimator.SetBool("isDead_b", true);

        playerNameHighscore_s = PlayerPrefs.GetString("CurrentPlayerName", "DefaultName");

        // save player scores to file with json
        AddPlayerHighscore(playerNameHighscore_s, currentScore_i, countPlayerJumps_i, realMeterWalked_i);

        // check if best score exist
        if (currentScore_i > highestScore_i)
        {
            highestScore_i = currentScore_i;
            PlayerPrefs.SetInt("HighestScore", highestScore_i);
        }

        dirtParticle.Stop();

        Time.timeScale = 0;
    }

    // sets up the animation and text in GUI + calls the HpChangeAnimation method
    private void HpChange(string s)
    {

        if (s == "gain" && !isGameOver_b)
        {
            _pauseMenu.changeHPPanel.SetActive(!_pauseMenu.changeHPPanel.gameObject.activeSelf);
            _pauseMenu.gainHP_GO.SetActive(!_pauseMenu.gainHP_GO.gameObject.activeSelf);
            _pauseMenu.gainHP_Anim.enabled = true;
            _pauseMenu.gainHP_Anim.SetBool("Gained_HP", true);
            _pauseMenu.gainHP_Anim.Play("Gain HP");
            Invoke("GainHPAnimation", 1);
        }
        else if (s == "lost" && !isGameOver_b)
        {
            _pauseMenu.changeHPPanel.SetActive(!_pauseMenu.changeHPPanel.gameObject.activeSelf);
            _pauseMenu.loseHP_GO.SetActive(!_pauseMenu.loseHP_GO.gameObject.activeSelf);
            _pauseMenu.loseHP_Anim.enabled = true;
            _pauseMenu.loseHP_Anim.SetBool("HP_Lost", true);
            _pauseMenu.loseHP_Anim.Play("Lose HP");
            Invoke("LostHPAnimation", 1);
        }
    }

    // deactivates the GO's and stops the animation -> +1 and -1 HP in GUI
    private void GainHPAnimation()
    {
        _pauseMenu.changeHPPanel.SetActive(!_pauseMenu.changeHPPanel.gameObject.activeSelf);
        _pauseMenu.gainHP_GO.SetActive(!_pauseMenu.gainHP_GO.gameObject.activeSelf);
        _pauseMenu.gainHP_Anim.SetBool("Gained_HP", false);
        _pauseMenu.gainHP_Anim.enabled = false;
    }

    // deactivates the GO's and stops the animation -> +1 and -1 HP in GUI
    private void LostHPAnimation()
    {
        _pauseMenu.changeHPPanel.SetActive(!_pauseMenu.changeHPPanel.gameObject.activeSelf);
        _pauseMenu.loseHP_GO.SetActive(!_pauseMenu.loseHP_GO.gameObject.activeSelf);
        _pauseMenu.loseHP_Anim.SetBool("HP_Lost", false);
        _pauseMenu.loseHP_Anim.enabled = false;
    }


    // Player class and methods \\
    // adds ne high score to the PlayerPrefs with JSON, sorts the stats and limits them to 15
    private void AddPlayerHighscore(string name, int score, int jumps, int walked)
    {
        // create new high score entry
        var highscore = new PlayerHighscore
        {
            name = name,
            score = score,
            jumps = jumps,
            walked = walked
        };

        // load saved high score
        var loadListJSON = PlayerPrefs.GetString("allHighscores");
        var highscoresList = JsonUtility.FromJson<Highscores>(loadListJSON);

        // if no data available
        if (highscoresList == null)
        {
            highscoresList = new Highscores()
            {
                playerHighscoreList = new List<PlayerHighscore>()
            };
        }

        // check if current high score > 15. place
        if (highscoresList.playerHighscoreList.Count < 15)
        {
            // add new high score
            highscoresList.playerHighscoreList.Add(highscore);
        }
        else if (highscoresList.playerHighscoreList.Count == 15)
        {
            if (highscore.score > highscoresList.playerHighscoreList[14].score)
            {
                highscoresList.playerHighscoreList[14] = highscore;
            }
        }


        // sort list before saving
        for (var i = 0; i < highscoresList.playerHighscoreList.Count; i++)
        {
            for (var j = i + 1; j < highscoresList.playerHighscoreList.Count; j++)
            {
                if (highscoresList.playerHighscoreList[j].score > highscoresList.playerHighscoreList[i].score)
                {
                    // swap
                    var tmp = highscoresList.playerHighscoreList[i];
                    highscoresList.playerHighscoreList[i] = highscoresList.playerHighscoreList[j];
                    highscoresList.playerHighscoreList[j] = tmp;
                }
            }
        }

        // save list to file
        var saveListJSON = JsonUtility.ToJson(highscoresList);
        Debug.Log(saveListJSON);
        PlayerPrefs.SetString("allHighscores", saveListJSON);
        PlayerPrefs.Save();
    }

    // class for PlayerHighscore list -> to open and save scores
    private class Highscores
    {
        public List<PlayerHighscore> playerHighscoreList;
    }

    // class to save player scores
    [Serializable]
    private class PlayerHighscore
    {
        public string name;
        public int score;
        public int jumps;
        public int walked;
    }
}
