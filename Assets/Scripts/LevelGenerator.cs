using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using Random = UnityEngine.Random;                                                  

public class LevelGenerator : MonoBehaviour
{
    [Header("Level Size")]
    [SerializeField] private int levelWidth;
    [SerializeField] private int levelHeight;

    [SerializeField] private int biomeChange;

    [Serializable]
    public struct BiomeSettings
    {
        [Header("Key")]
        public string name;

        [Header("Tiles")]
        public GameObject topPrefab;
        public GameObject midPrefab;
        public GameObject lowerPrefab;
        public GameObject orePrefab;

        [Header("Spawn Settings")]
        public Vector2 minMaxSpawnHeight;
        public Vector2 minMaxStoneSpawn;
        public int oreSpawnChance;
        public bool isSmooth;
    }

    [SerializeField] private BiomeSettings[] biomeSettings;
    private Dictionary<string, BiomeSettings> _settings = new Dictionary<string, BiomeSettings>();

    private bool _isGenerated;

    private void Awake()
    {
        foreach (var item in biomeSettings)
        {
            _settings.Add(item.name, item);
        }

        StartCoroutine(Generate("Plains"));
    }

    private IEnumerator Generate(string name)
    {
        var key = name;


        for (int x = 0; x < levelWidth; x++)
        {
            if(Random.Range(0, biomeChange) == 3)
            {
                var a = Random.Range(0, _settings.Count);

                if (a == 0) key = "Plains";
                else if (a == 1) key = "Stone";
                else if (a == 2) key = "Stone2";
            }

            int minHeight = levelHeight - (int)_settings[key].minMaxSpawnHeight.x;
            int maxHeight = levelHeight + (int)_settings[key].minMaxSpawnHeight.y;

            if (_settings[key].isSmooth)
            {
                var r = Random.Range(0, 2);

                print(r);

                if (r == 1)
                {
                    levelHeight = Random.Range(minHeight, maxHeight);
                }
            }
            else
            {
                levelHeight = Random.Range(minHeight, maxHeight);
            }

            int minStoneSpawnDistance = levelHeight - (int)_settings[key].minMaxStoneSpawn.x;
            int maxStoneSpawnDistance = levelHeight - (int)_settings[key].minMaxStoneSpawn.y;

            int totalStoneSpawnDistance = Random.Range(minStoneSpawnDistance, maxStoneSpawnDistance);

            for (int y = 0; y < levelHeight; y++)
            {
                Vector2 spawnPos = new Vector2(x, y);

                if (y < totalStoneSpawnDistance)
                {
                    if(Random.Range(0, _settings[key].oreSpawnChance) == 1)
                    {
                        SpawnTile(_settings[key].orePrefab, spawnPos);
                    }
                    else
                    {
                        SpawnTile(_settings[key].lowerPrefab, spawnPos);
                    }
                }
                else
                {
                    SpawnTile(_settings[key].midPrefab, spawnPos);
                }
            }

            Vector2 topPos = new Vector2(x, levelHeight);

            if (totalStoneSpawnDistance == levelHeight)
            {
                SpawnTile(_settings[key].lowerPrefab, topPos);
            }
            else
            {
                SpawnTile(_settings[key].topPrefab, topPos);
            }

            yield return new WaitForSeconds(0);
        }

        _isGenerated = true;
    }

    private void SpawnTile(GameObject obj, Vector2 pos)
    {
        obj = Instantiate(obj, pos, Quaternion.identity);
        obj.transform.parent = this.transform;
    }

    public void ResetLevel()
    {
        if(_isGenerated)
        {
            foreach (Transform child in transform)
            {
                GameObject.Destroy(child.gameObject);
            }

            StartCoroutine(Generate("Plains"));
        }
        else
        {
            StartCoroutine(Generate("Plains"));
        }
    }
}
