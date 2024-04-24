using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AddressableAssets;


public class ObjectPoolManager
{
    private static readonly ObjectPoolManager instance = new ObjectPoolManager();
    public static ObjectPoolManager Instance { get { return instance; } }


    Dictionary<string, Pool> _pools = new Dictionary<string, Pool>();
    Transform _root;

    private ObjectPoolManager() 
    {
        Debug.Log("Created!");
        _root = new GameObject("ObjectPool").transform;
        Object.DontDestroyOnLoad(_root);
    }

    class Pool 
    {
        public GameObject originalObj { get; private set; }
        public Transform rootObj { get; private set; }

        Stack<Poolable> _poolStack = new Stack<Poolable>();
        List<Poolable> _poolList = new List<Poolable>();

        public void InitPool(GameObject original, int count = Defines.DEFAULT_POOL_SIZE) 
        {
            originalObj = original;
            rootObj = new GameObject().transform;
            rootObj.name = $"{originalObj.name}_pool";

            for(int i = 0; i < count; i++) 
            {
                Push(CreateInstance());
            }
        }

        Poolable CreateInstance() 
        {
            GameObject gameObject = Object.Instantiate(originalObj);
            gameObject.name = originalObj.name;
            Poolable poolObj = gameObject.GetOrAddComponent<Poolable>();
            _poolList.Add(poolObj);
            return poolObj;
        }

        internal void Push(Poolable poolable) 
        {
            if (poolable == null)
                return;

            poolable.gameObject.SetActive(false);
            poolable.transform.parent = rootObj;
            _poolStack.Push(poolable);

            Debug.Log(_poolStack.Count);
        }

        internal Poolable Pop() 
        {
            if(_poolStack.Count > 0)
                return _poolStack.Pop();

            ExtendPool();
            return _poolStack.Pop();
        }

        void ExtendPool() 
        {
            for (int i=0; i< Defines.DEFAULT_POOL_SIZE; i++) 
            {
                Push(CreateInstance());
            }
        }

        public void ClearPool() 
        {
            for (int i = 0; i < _poolList.Count; i++)
            {
                if(_poolList[i] != null && _poolList[i].IsDestroyed() == false)
                    GameObject.Destroy(_poolList[i].gameObject);
            }
            _poolList.Clear();
            _poolStack.Clear();
        }
    }

    public void CreatePool(GameObject gameObject, int count = Defines.DEFAULT_POOL_SIZE) 
    {
        Pool pool = new Pool();
        pool.InitPool(gameObject, count);
        pool.rootObj.parent = _root;

        _pools.Add(gameObject.name, pool);
    }

    public void CreatePool(string name, int count = Defines.DEFAULT_POOL_SIZE)
    {
        GameObject gameObject = Addressables.LoadAssetAsync<GameObject>(name).WaitForCompletion();
        CreatePool(gameObject, count);
    }

    public void TryPush(GameObject gameObject) 
    {
        if(gameObject.TryGetComponent<Poolable>(out Poolable poolable)) 
        {
            if (_pools.ContainsKey(gameObject.name)) 
            {
                Push(poolable);
                return;
            }
        }
        GameObject.Destroy(gameObject);
    }

    public void Push(Poolable gameObject) 
    {
        _pools[gameObject.name].Push(gameObject);
    }

    public Poolable Pop(GameObject original) 
    {
        if (!_pools.ContainsKey(original.name))
            CreatePool(original);
        
        return _pools[original.name].Pop();
    }

    public Poolable Pop(string name) 
    {
        if (!_pools.ContainsKey(name))
            CreatePool(name);

        return _pools[name].Pop();
    }

    public void ClearPools() 
    {
        foreach (var pool in _pools.Values) 
        {
            pool.ClearPool();
        }
        // _pools.Clear();
    }
}

