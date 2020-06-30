using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// class for changed gravity when falling down
public class BetterJump : MonoBehaviour
{
    // get player controller script
    private PlayerController _playerController;

    // get RB for velocity
    Rigidbody playerRb;

    // when falling down
    public float fallMp_f = 2.5f;

    // when holding jump button
    public float lowJumpMp_f = 2f;

    // pressing shift to fall down even faster
    public float fallDownFast_f = 7f;

    // cooldown variables for pressing v and holding space
    private bool pressingShiftCooldown_b;
    private bool holdingSpaceCooldown_b;

    // check if time of pressing shift is over
    private bool isTimeOver_b;

    // count shift click time
    private float clickTime_f;

    // count cooldown length
    private int cooldownSeconds_i;

    void Start()
    {
        _playerController = GameObject.FindObjectOfType<PlayerController>();

        pressingShiftCooldown_b = false;
        holdingSpaceCooldown_b = false;

        clickTime_f = 0.0f;
        cooldownSeconds_i = 5;
    }

    void Awake()
    {
        playerRb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // when holding button
        if ((playerRb.velocity.y < 0) && (holdingSpaceCooldown_b == false) && _playerController.playerJumpHeight.transform.position.y < 20.0f)
        {
            // implement same stuff as in shift event
            playerRb.velocity += Vector3.up * Physics.gravity.y * (fallMp_f - 1) * Time.deltaTime;

            Invoke("ResetLongJumpCooldown", 5.0f);
            holdingSpaceCooldown_b = false;
        }
        // if player model is to high and user is still holding "space"
        else if (_playerController.playerJumpHeight.transform.position.y > 21.0f)
        {
            playerRb.velocity += Vector3.up * Physics.gravity.y * (lowJumpMp_f - 1) * Time.deltaTime;
        }
        // when clicking short, when falling, gravity gets higher
        else if (playerRb.velocity.y > 0 && !Input.GetButton("Jump"))
        {
            playerRb.velocity += Vector3.up * Physics.gravity.y * (lowJumpMp_f - 1) * Time.deltaTime;
        }

        // fast fall down on shift hold - with cooldown
        if (Input.GetButton("Fire3"))
        {
            if ((playerRb.velocity.y != 0.0f) && pressingShiftCooldown_b == false)
            {
                clickTime_f += Time.deltaTime;

                playerRb.velocity += Vector3.up * Physics.gravity.y * (fallDownFast_f - 1) * Time.deltaTime;

                if (clickTime_f > 1.0f)
                {
                    Debug.Log("Time Over");
                    Invoke("ResetFalldownCooldown", 5.0f);
                    Invoke("CountTimeLeft", 0);

                    pressingShiftCooldown_b = true;
                    isTimeOver_b = true;
                    clickTime_f = 0.0f;
                    cooldownSeconds_i = 5;
                }
            }
        }
    }

    // time when ability is up again
    void CountTimeLeft()
    {
        if (isTimeOver_b)
        {
            cooldownSeconds_i -= 1;
            Debug.Log("Current cooldown time: " + cooldownSeconds_i);
            if (cooldownSeconds_i == 0)
            {
                isTimeOver_b = false;
            }
            Invoke("CountTimeLeft", 1);
        }
    }

    // resets the cooldown for falling down
    void ResetFalldownCooldown()
    {
        pressingShiftCooldown_b = false;
    }

    // resets the cooldown for longer jump
    void ResetLongJumpCooldown()
    {
        holdingSpaceCooldown_b = false;
    }
}
