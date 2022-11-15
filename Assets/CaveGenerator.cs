using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CaveGenerator : MonoBehaviour
{
    [Header("Cave Size")]
    [SerializeField] private int caveWidth;
    [SerializeField] private int caveHeight;
    [SerializeField] private Vector2 minMaxSpawnHeight;

    [SerializeField] private GameObject caveBlockPrefab;

    private bool _isGenerated;

    public void Start()
    {
        StartCoroutine(Generate());
    }

    private IEnumerator Generate()
    {
        yield return new WaitForSeconds(.5f);

        for (int i = 0; i < caveWidth; i++)
        {
            int minHeight = caveHeight - (int)minMaxSpawnHeight.x;
            int maxHeight = caveHeight + (int)minMaxSpawnHeight.y;

            caveHeight = Random.Range(minHeight, maxHeight);

            Vector2 spawnPos = new Vector2(i, caveHeight);

            SpawnTile(caveBlockPrefab, spawnPos);

            yield return new WaitForSeconds(0);
        }
    }


    private void SpawnTile(GameObject obj, Vector2 pos)
    {
        obj = Instantiate(obj, pos, Quaternion.identity);
        obj.transform.parent = this.transform;
    }
}
