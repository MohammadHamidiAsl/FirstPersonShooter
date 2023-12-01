using System.Collections.Generic;
using UnityEngine;

namespace GameSystem.Pool
{
    public class GameObjectPool
    {
        private GameObject prefab;
        private Queue<GameObject> pool;

        private int maxPoolSize;

        public GameObjectPool(GameObject prefab, int maxPoolSize)
        {
            this.prefab = prefab;
            this.maxPoolSize = maxPoolSize;
            pool = new Queue<GameObject>();

            // Pre-populate the pool
            for (int i = 0; i < maxPoolSize; i++)
            {
                GameObject obj = GameObject.Instantiate(prefab);
                obj.SetActive(false);
                pool.Enqueue(obj);
            }
        }

        public GameObject Get()
        {
            if (pool.Count > 0)
            {
                return pool.Dequeue();
            }
            else
            {
                return GameObject.Instantiate(prefab);
            }
        }

        public void Release(GameObject gameObject)
        {
            if (pool.Count < maxPoolSize)
            {
                gameObject.SetActive(false);
                pool.Enqueue(gameObject);
            }
            else
            {
                GameObject.Destroy(gameObject);
            }
        }
    }
}