using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleJumps : MonoBehaviour
{
    private PlayerController _playerController;

    void Start()
    {
        _playerController = GameObject.FindObjectOfType<PlayerController>();
    }

    // counts times the player jumps over an obstacle
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" || other.name == "PlayerModel")
        {
            _playerController.countObstacleHit_i = 1;
            _playerController.countObstacleJump_i++;
            Debug.Log(_playerController.countObstacleJump_i);
        }
    }
}
