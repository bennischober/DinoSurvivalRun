using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

public class MoveLeft : MonoBehaviour
{
    // get isGameOver_b
    private PlayerController _playerController;

    // coords for destroying obstacles, etc.
    private float leftBound_f = 50;
    private float leftBoundTrees_f = 100;

    // map and obstacles movement speed
    private float speed_f = -90;


    // Start is called before the first frame update
    void Start()
    {
        _playerController = GameObject.FindObjectOfType<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!_playerController.isGameOver_b)
        {
            if (gameObject.CompareTag("FlyEnemyParent"))
            {
                transform.Translate(Vector3.back * Time.deltaTime * speed_f * 1.2f);
            }
            else
            {
                transform.Translate(Vector3.back * Time.deltaTime * speed_f);
            }
        }



        // delete obstacle when out of screen
        if (transform.position.z > leftBound_f && gameObject.CompareTag("ObstacleParent"))
        {
            Destroy(gameObject);
        }

        // delete tree when out of screen
        if (transform.position.z > leftBoundTrees_f && gameObject.CompareTag("TreeParent"))
        {
            Destroy(gameObject);
        }

        // if player didn't reach the hp it get's destroyed
        if (transform.position.z > leftBound_f && gameObject.CompareTag("PickupLifeParent"))
        {
            Destroy(gameObject);
        }

        // if fly enemy is out of range
        if (transform.position.z > leftBoundTrees_f && gameObject.CompareTag("FlyEnemyParent"))
        {
            Destroy(gameObject);
        }
    }
}
