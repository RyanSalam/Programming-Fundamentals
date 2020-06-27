using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    public Dictionary<string, Queue<GameObject>> poolDictionary;
    public List<Pool> pools = new List<Pool>();

    public static ObjectPooler instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;

        else
        {
            Destroy(this.gameObject);
        }
    }

    private void Start()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        foreach(Pool pool in pools)
        {
            Queue<GameObject> poolQueue = new Queue<GameObject>();

            for (int i = 0; i < pool.size; i++)
            {
                GameObject temp = Instantiate(pool.pooledObject);
                temp.SetActive(false);

                poolQueue.Enqueue(temp);
            }

            poolDictionary.Add(pool.tag, poolQueue);
        }
    }

    public void AddToPool(string tag, GameObject objectToAdd)
    {
        Queue<GameObject> pooled = poolDictionary[tag];

        if (pooled == null)
            return;

        pooled.Enqueue(objectToAdd);
    }

    public GameObject spawnFromPool(string tag, Vector3 position, Quaternion rotation)
    {
        if (poolDictionary[tag] == null)
            return null;

        GameObject toSpawn = poolDictionary[tag].Dequeue();

        toSpawn.transform.position = position;
        toSpawn.transform.rotation = rotation;
        toSpawn.SetActive(true);
        toSpawn.tag = tag;

        return toSpawn;
    }

    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject pooledObject;
        public int size;
    }
}
