using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class SpawnManager : MonoBehaviour
{
    // Get trees
    [SerializeField] private GameObject[] _treePrefabs;

    // Get GO
    public GameObject obstaclePrefab;

    // Get hp GO
    public GameObject lifePrefab;

    // Get FlyEnemy GO
    public GameObject flyEnemy;

    // Spawn pos for life
    private Vector3 hpSpawnPos = new Vector3(-60.09998f, 9, -100);

    // Spawn pos for flyEnemy
    private Vector3 flyEnemyPos = new Vector3(-60.1f, 18.2f, -100.0f);

    // Get isGameOver_b
    private PlayerController _playerController;

    // Spawn coords
    private static float spawn_x = -60.09998f;
    private static float spawn_y = 9;
    private static float spawn_z = -120;

    // Spawn location for obstacle
    private Vector3 spawnPos = new Vector3(spawn_x, spawn_y, spawn_z);

    // Random range for random numbers
    private float f_repeatRate;
    private float treeSpawnRate_f;
    private float flyEnemySpawnRate_f;

    // Random range for int numbers
    private int repeatRateX_i;
    private int repeatRateZ_i;
    private int randomTree_i;
    private int randomTreeNumbers_i;
    private int randomNumberHp_i;

    // spawn location of trees
    private Vector3 spawnPosTree;

    // Start is called before the first frame update
    void Start()
    {
        _playerController = GameObject.FindObjectOfType<PlayerController>();

        Invoke("SpawnObstacle", 0.5f);
        Invoke("SpawnTree", 0);
        Invoke("SpawnFlyEnemy", 0.5f);

        // get random amount of trees
        randomTreeNumbers_i = Random.Range(15, 26);

        // spawn trees for start map
        for (int i = 0; i < randomTreeNumbers_i; i++)
        {
            // random number for index of tree
            randomTree_i = Random.Range(0, 4);

            // random spawn location of trees
            repeatRateX_i = Random.Range(-35, -5);
            repeatRateZ_i = Random.Range(-350, 351);

            // instantiate tree with random values set above
            Instantiate(_treePrefabs[randomTree_i], new Vector3(repeatRateX_i, 4.7f, repeatRateZ_i), Quaternion.Euler(0, 0, 0));
        }

    }

    // Update is called once per frame
    void Update()
    {
        if ((GameObject.FindWithTag("PickupLifeParent") == null) && !_playerController.isGameOver_b && _playerController.playerHp_i < 3 && _playerController.countObstacleHit_i == 1)
        {
            _playerController.countObstacleHit_i++;
            randomNumberHp_i = Random.Range(0, 4); // 101
            Debug.Log("current number for hp: " + randomNumberHp_i);
            if (randomNumberHp_i == 2) // % 3 == 0
            {
                Instantiate(lifePrefab, hpSpawnPos, lifePrefab.transform.rotation);
            }
        }
    }

    // spawns fly enemy
    void SpawnFlyEnemy()
    {
        if (!_playerController.isGameOver_b)
        {
            flyEnemySpawnRate_f = Random.Range(4.0f, 6.0f);

            Instantiate(flyEnemy, flyEnemyPos, flyEnemy.transform.rotation);
            Invoke("SpawnFlyEnemy", flyEnemySpawnRate_f);
        }
    }

    // spawns trees at random locations
    void SpawnTree()
    {
        if (!_playerController.isGameOver_b)
        {
            // set time to spawn next tree
            treeSpawnRate_f = Random.Range(0.2f, 0.5f);

            // random spawn location of trees
            repeatRateX_i = Random.Range(-35, -5);
            repeatRateZ_i = Random.Range(-701, -350);

            // random number for index of tree
            randomTree_i = Random.Range(0, 4);

            // instantiate tree with random values set above
            Instantiate(_treePrefabs[randomTree_i], new Vector3(repeatRateX_i, 4.7f, repeatRateZ_i), Quaternion.Euler(0, 0, 0));

        }
        Invoke("SpawnTree", treeSpawnRate_f);
    }

    // spawns obstacles (cactus) random
    void SpawnObstacle()
    {
        if (!_playerController.isGameOver_b)
        {
            // create random numbers for obstacle spawn timer
            f_repeatRate = Random.Range(1.5f, 3.0f);

            Instantiate(obstaclePrefab, spawnPos, obstaclePrefab.transform.rotation);

            Debug.Log("Spawn times: " + f_repeatRate);

            Invoke("SpawnObstacle", f_repeatRate);
        }
    }
}
