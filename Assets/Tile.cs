using System.Collections;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private GameObject flowerPrefab;
    [SerializeField] private GameObject treePrefab;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private int spawnChance;

    private Inventory _playerInventory;

    private void Awake()
    {
        _playerInventory = FindObjectOfType<Inventory>();


        var r = Random.Range(0, spawnChance);
        if (r == 1)
        {
            SpawnTile(flowerPrefab, spawnPoint.position);
        }
        else if (r == 2)
        {
            SpawnTile(treePrefab, spawnPoint.position);
        }
    }

    private void OnMouseDown()
    {
        _playerInventory.itemCount += 1;
        Destroy(gameObject);
    }

    private void OnMouseOver()
    {
        GetComponent<SpriteRenderer>().material.color = Color.clear;
    }

    private void SpawnTile(GameObject obj, Vector2 pos)
    {
        obj = Instantiate(obj, pos, Quaternion.identity);
        obj.transform.parent = this.transform;
    }
}
