using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    // get isGameOver_b
    private PlayerController _playerController;

    // set all maps in editor for usage
    [SerializeField]
    private GameObject[] _mapPrefabs;

    // Map reset location fix
    [SerializeField]
    private float offset_f = -720;

    //[SerializeField]
    private float zedOffset_f = 0;

    [SerializeField]
    private float addable_f = -1250;

    private int counter_i = 0;

    // Start is called before the first frame update
    void Start()
    {
        _playerController = GameObject.FindObjectOfType<PlayerController>();

        // instantiate maps set in editor
        for (int i = 0; i < _mapPrefabs.Length; i++)
        {
            Instantiate(_mapPrefabs[i], new Vector3(0, 0, i * (offset_f)), Quaternion.Euler(0, 0, 0));
        }
    }

    public void MoveMap(GameObject map)
    {
        if (!_playerController.isGameOver_b)
        {
            // -- -- //
            // map reset location fix
            counter_i++;

            if (counter_i == 1)
            {
                zedOffset_f = -10;
            }

            if (counter_i % 2 == 0)
            {
                zedOffset_f += 30;
            }

            if (counter_i == 3)
            {
                zedOffset_f += 35;
                counter_i = 0;
            }
            // -- -- //

            map.transform.position = new Vector3(0, 0, (addable_f + zedOffset_f));

            // reset offset for next usage
            zedOffset_f = 0;
        }
    }
}
