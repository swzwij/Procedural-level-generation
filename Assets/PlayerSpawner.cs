using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;
    private bool _hasSpawned;

    private void Update()
    {
        if (_hasSpawned) return;
        if(Input.GetKeyDown(KeyCode.P))
        {
            _hasSpawned = true;
            Instantiate(playerPrefab, transform);
        }
    }
}
