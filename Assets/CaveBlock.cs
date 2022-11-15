using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaveBlock : MonoBehaviour
{
    private void Start()
    {
        var spawnPos = transform.position;
        spawnPos.y += transform.localScale.y / 2;

        var p = Physics2D.RaycastAll(spawnPos, Vector2.down, 5);

        foreach(var item in p)
        {
            Destroy(item.transform.gameObject);
        }

        Destroy(gameObject);
    }
}
