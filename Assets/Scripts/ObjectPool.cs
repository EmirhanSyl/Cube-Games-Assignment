using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;
    }

    public static ObjectPool Instance;

    public GameObject diamondCollectedFX;

    public List<Pool> pools;
    public Dictionary<string, Queue<GameObject>> poolDictinory;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
       poolDictinory = new Dictionary<string, Queue<GameObject>>();

        foreach (Pool pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }

            poolDictinory.Add(pool.tag, objectPool);
        }
    }

    public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation)
    {
        if (!poolDictinory.ContainsKey(tag))
        {
            Debug.LogWarning("Pool with tag" + tag + "Dosn't Excist");
            return null;
        }

        GameObject objectToSpawn = poolDictinory[tag].Dequeue();
        objectToSpawn.SetActive(true);
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;

        poolDictinory[tag].Enqueue(objectToSpawn);
        return objectToSpawn;
    }

}
