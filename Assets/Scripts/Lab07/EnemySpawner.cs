using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public List<Transform> spawnPoints = new List<Transform>();

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SpawnFromPool();
        }
    }

    private void SpawnFromPool()
    {
        Transform wheretoSpwn = spawnPoints[Random.Range(0, spawnPoints.Count)];

        GameObject spawned = ObjectPooler.instance.spawnFromPool("Enemy", wheretoSpwn.position, wheretoSpwn.rotation);

        spawned.GetComponent<Goblin>().Respawn();
    }
}
