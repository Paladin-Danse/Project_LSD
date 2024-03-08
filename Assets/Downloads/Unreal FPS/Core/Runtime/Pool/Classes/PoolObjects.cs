/* ==================================================================
   ---------------------------------------------------
   Project   :    Unreal FPS
   Publisher :    Renowned Studio
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017 - 2019 All rights reserved.
   ================================================================ */

using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnrealFPS.Runtime
{
    public partial class PoolManager
    {
        public struct PoolObjects : IEquatable<PoolObjects>
        {
            private Transform parent;
            private List<GameObject> pools;

            public PoolObjects(Transform parent, List<GameObject> pools)
            {
                this.parent = parent;
                this.pools = pools != null ? pools : new List<GameObject>();
            }

            public Transform GetParent()
            {
                return parent;
            }

            public List<GameObject> GetPools()
            {
                return pools;
            }

            public GameObject GetPool(int index)
            {
                return pools[index];
            }

            public GameObject GetInActivePool()
            {
                for (int i = 0, length = pools.Count; i < length; i++)
                {
                    GameObject gameObject = pools[i];
                    if (!gameObject.activeSelf)
                    {
                        return gameObject;
                    }
                }
                return null;
            }

            public List<GameObject> GetInActivePools()
            {
                List<GameObject> inActivePools = new List<GameObject>();
                for (int i = 0, length = pools.Count; i < length; i++)
                {
                    GameObject gameObject = pools[i];
                    if (!gameObject.activeSelf)
                    {
                        inActivePools.Add(gameObject);
                    }
                }
                return inActivePools;
            }

            public void SetPools(List<GameObject> value)
            {
                pools = value;
            }

            public int PoolsCount()
            {
                return pools.Count;
            }

            public int InActivePoolsCount()
            {
                int count = 0;
                for (int i = 0, length = pools.Count; i < length; i++)
                {
                    if (!pools[i].activeSelf)
                    {
                        count++;
                    }
                }
                return count;
            }

            public void AddPool(GameObject value)
            {
                pools.Add(value);
            }

            public void AddPoolRange(List<GameObject> value)
            {
                pools.AddRange(value);
            }

            public void RemovePool(GameObject value)
            {
                pools.Remove(value);
            }

            public void RemovePool(int index)
            {
                pools.RemoveAt(index);
            }

            public readonly static PoolObjects Empty = new PoolObjects(null, null);

            public static bool operator ==(PoolObjects left, PoolObjects right)
            {
                return Equals(left, right);
            }

            public static bool operator !=(PoolObjects left, PoolObjects right)
            {
                return !Equals(left, right);
            }

            public override bool Equals(object obj)
            {
                return (obj is PoolObjects metrics) && Equals(metrics);
            }

            public bool Equals(PoolObjects other)
            {
                return (parent.name) == (other.parent.name);
            }

            public override int GetHashCode()
            {
                return (parent, pools).GetHashCode();
            }
        }
    }
}