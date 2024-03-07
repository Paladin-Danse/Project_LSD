/* ==================================================================
   ---------------------------------------------------
   Project   :    Unreal FPS
   Publisher :    Renowned Studio
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017 - 2019 All rights reserved.
   ================================================================ */

using System.Collections.Generic;
using UnityEngine;
using UnrealFPS.Utility;

namespace UnrealFPS.Runtime
{
    public partial class PoolManager : Singleton<PoolManager>, IPoolManager
    {
        private static Transform instanceTransform;
        private static Dictionary<string, PoolObjects> pool;

        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        protected virtual void Awake()
        {
            instanceTransform = transform;
            pool = new Dictionary<string, PoolObjects>();
        }

        /// <summary>
        /// Instantiate or pop gameObject with specific ID.
        /// </summary>
        /// <returns>Return: Gameobject from pool of new created pool object.</returns>
        public static GameObject Instantiate(string ID, GameObject gameObject, Vector3 position, Quaternion rotation)
        {
            if (PoolManager.Instance == null)
                return null;

            if (pool.ContainsKey(ID))
            {
                PoolObjects poolObjects = pool[ID];
                GameObject poolObject = null;
                if (poolObjects.InActivePoolsCount() > 0)
                {
                    poolObject = poolObjects.GetInActivePool();
                    poolObject.transform.SetPositionAndRotation(position, rotation);
                    poolObject.SetActive(true);
                }
                else
                {
                    poolObject = GameObject.Instantiate(gameObject, position, rotation, poolObjects.GetParent());
                    poolObjects.AddPool(poolObject);
                }
                return poolObject;
            }
            else
            {
                Transform subPoolCase = new GameObject(ID).transform;
                subPoolCase.SetParent(instanceTransform);
                pool.Add(ID, new PoolObjects(subPoolCase, null));
                GameObject poolObject = GameObject.Instantiate(gameObject, position, rotation, subPoolCase);
                pool[ID].AddPool(poolObject);
                return poolObject;
            }
        }

        /// <summary>
        /// Instantiate or pop gameObject, ID generated automatically.
        /// </summary>
        /// <returns>Return: Gameobject from pool of new created pool object.</returns>
        public static GameObject Instantiate(GameObject gameObject, Vector3 position, Quaternion rotation)
        {
            if (PoolManager.Instance == null)
                return null;

            string ID = gameObject.name;
            if (pool.ContainsKey(ID))
            {
                PoolObjects poolObjects = pool[ID];
                GameObject poolObject = null;
                if (poolObjects.InActivePoolsCount() > 0)
                {
                    poolObject = poolObjects.GetInActivePool();
                    poolObject.transform.SetPositionAndRotation(position, rotation);
                    poolObject.SetActive(true);
                }
                else
                {
                    poolObject = GameObject.Instantiate(gameObject, position, rotation, poolObjects.GetParent());
                    poolObjects.AddPool(poolObject);
                }
                return poolObject;
            }
            else
            {
                Transform subPoolCase = new GameObject(ID).transform;
                subPoolCase.SetParent(instanceTransform);
                pool.Add(ID, new PoolObjects(subPoolCase, null));
                GameObject poolObject = GameObject.Instantiate(gameObject, position, rotation, subPoolCase);
                pool[ID].AddPool(poolObject);
                return poolObject;
            }
        }

        /// <summary>
        /// Pop game object from pool by specific ID.
        /// </summary>
        /// <returns>Return: Gameobject from pool or null if no free pool.</returns>
        public virtual GameObject Pop(string ID)
        {
            if (pool.ContainsKey(ID))
            {
                PoolObjects poolObjects = pool[ID];
                GameObject poolObject = null;
                if (poolObjects.InActivePoolsCount() > 0)
                {
                    poolObject = poolObjects.GetInActivePool();
                }
                return poolObject;
            }
            return null;
        }

        /// <summary>
        /// Push game object in pool ID generated automatically.
        /// </summary>
        public virtual void Push(GameObject gameObject)
        {
            gameObject.SetActive(false);
        }

        /// <summary>
        /// Get pool object by ID
        /// </summary>
        /// <returns>Return: Null If the pool with the specified ID does not exist.</returns>
        public PoolObjects GetPoolObjects(string ID)
        {
            return pool.ContainsKey(ID) ? pool[ID] : PoolObjects.Empty;
        }

        /// <summary>
        /// Compare two pools.
        /// </summary>
        /// <returns>Return: Compare result. If the pool with the specified ID does not exist return false.</returns>
        public bool ComparePools(string ID1, string ID2)
        {
            return (pool.ContainsKey(ID1) && pool.ContainsKey(ID2)) ? pool[ID1] == pool[ID2] : false;
        }

        /// <summary>
        /// Compare two pools.
        /// </summary>
        /// <returns>Return: Compare result.</returns>
        public bool ComparePools(PoolObjects poolObjects1, PoolObjects poolObjects2)
        {
            return poolObjects1 == poolObjects2;
        }

        /// <summary>
        /// Combine two pools in one.
        /// Pool from will deleted.
        /// </summary>
        public void CombinePools(string ID_From, string ID_To, bool deleteOld = true)
        {
            if (!(pool.ContainsKey(ID_From) && pool.ContainsKey(ID_To)))
            {
                return;
            }
            List<GameObject> pools = pool[ID_From].GetPools();

            for (int i = 0, length = pools.Count; i < length; i++)
            {
                pools[i].transform.SetParent(pool[ID_To].GetParent());
            }

            pool[ID_To].AddPoolRange(pools);

            if (deleteOld)
            {
                Destroy(pool[ID_From].GetParent().gameObject);
                pool.Remove(ID_From);
            }
        }

        /// <summary>
        /// Combine two pools in one.
        /// Will created new pool from two old pool.
        /// Old pools will deleted.
        /// </summary>
        public void CombinePools(string old_ID1, string old_ID2, string newPool_ID, bool deleteOld = true)
        {
            if (!(pool.ContainsKey(old_ID1) && pool.ContainsKey(old_ID2)) || string.IsNullOrEmpty(newPool_ID))
            {
                return;
            }

            Transform subPoolCase = new GameObject(newPool_ID).transform;
            subPoolCase.SetParent(instanceTransform);
            pool.Add(newPool_ID, new PoolObjects(subPoolCase, null));

            List<GameObject> gameObjects = new List<GameObject>();

            gameObjects.AddRange(pool[old_ID1].GetPools());
            gameObjects.AddRange(pool[old_ID2].GetPools());

            for (int i = 0, length = gameObjects.Count; i < length; i++)
            {
                gameObjects[i].transform.SetParent(pool[newPool_ID].GetParent());
            }

            pool[newPool_ID].AddPoolRange(gameObjects);

            if (deleteOld)
            {
                Destroy(pool[old_ID1].GetParent().gameObject);
                Destroy(pool[old_ID2].GetParent().gameObject);
                pool.Remove(old_ID1);
                pool.Remove(old_ID2);
            }
        }

        /// <summary>
        /// Pool dictionary.
        /// </summary>
        public Dictionary<string, PoolObjects> GetPool()
        {
            return pool;
        }
    }
}