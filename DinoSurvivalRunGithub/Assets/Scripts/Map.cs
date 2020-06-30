using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    private MapManager _mapManager;
    private PlayerController _playerController;

    private void OnEnable()
    {
        _mapManager = GameObject.FindObjectOfType<MapManager>();
        _playerController = GameObject.FindObjectOfType<PlayerController>();
    }

    // when player hits Collider at end of map, it gets relocated
    private IEnumerator OnTriggerEnter(Collider other)
    {
        if ((other.tag == "Player" || other.name == "PlayerModel") && !_playerController.isGameOver_b)
        {
            // wait before relocation -> it gets relocated when out of screen
            yield return new WaitForSeconds(1.5f);
            Debug.Log("Moved map!");
            _mapManager.MoveMap(transform.parent.gameObject);
        }
    }
}
