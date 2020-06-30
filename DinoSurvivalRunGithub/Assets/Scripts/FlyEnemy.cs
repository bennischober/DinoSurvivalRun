using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyEnemy : MonoBehaviour
{
    // get flyEnemy animator
    private Animator flyEnemyAnimator;

    // get isGameOver_b
    private PlayerController _playerController;


    // Start is called before the first frame update
    void Start()
    {
        _playerController = GameObject.FindObjectOfType<PlayerController>();
        flyEnemyAnimator = GameObject.FindWithTag("FlyEnemy").GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!_playerController.isGameOver_b && flyEnemyAnimator != null)
        {
            flyEnemyAnimator.enabled = true;
        }
        else
        {
            if (flyEnemyAnimator != null) flyEnemyAnimator.enabled = false;
        }
    }
}
