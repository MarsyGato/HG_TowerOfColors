using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
	static public Dictionary<string, List<GameObject>> enabledPools = new Dictionary<string, List<GameObject>>();
	static public Dictionary<string, List<GameObject>> disabledPools = new Dictionary<string, List<GameObject>>();

	static private PoolManager _instance = null;

	static private int _maxPoolSize = 1000;

	private static string POOLED = "_pooled";

	private void Awake()
	{
		if (_instance != null)
		{
			Destroy(gameObject);
		}
		else
		{
			DontDestroyOnLoad(gameObject);
			_instance = this;
		}
	}

	static public GameObject Create(GameObject pObject)
	{
		GameObject pooledObject;
		string key = pObject.name + POOLED;

		List<GameObject> pool;

		if (disabledPools.TryGetValue(key, out pool))
		{
			pool = disabledPools[key];

			pooledObject = pool[0];
			pool.RemoveAt(0);

			if (pool.Count == 0) disabledPools.Remove(key);
		}
		else
		{
			pooledObject = Instantiate(pObject);
			pooledObject.name = pObject.name + POOLED;
			pooledObject.transform.SetParent(_instance.transform);
		}

		if (enabledPools.TryGetValue(key, out pool))
		{
			if (!pool.Contains(pObject))
				pool.Add(pooledObject);
		}
		else
		{
			pool = new List<GameObject>();
			pool.Add(pooledObject);

			enabledPools.Add(key, pool);
		}

		pooledObject.transform.SetParent(_instance.transform);
		pooledObject.SetActive(true);

		return pooledObject;
	}

	static public void DisableAllByType(string pObject)
	{
		List<GameObject> pool;

		if(enabledPools.TryGetValue(pObject + POOLED, out pool))
		{
			int length = pool.Count;
			for (int i = length - 1; i >= 0; i--)
			{
				if (pool[i] != null)
					Remove(pool[i]);
			}
		}
	}

	static public void Remove(GameObject pObject)
	{

		string key = pObject.name;

		List<GameObject> pool;

		if (enabledPools.TryGetValue(key, out pool))
		{
			pool.Remove(pObject);
		}
		else return;

		pObject.GetComponent<MonoBehaviour>().StopAllCoroutines();
		pObject.SetActive(false);

		if (disabledPools.TryGetValue(key, out pool))
		{
			if (pool.Contains(pObject)) return;

			if (pool.Count < _maxPoolSize)
			{
				pool.Add(pObject);
				pObject.GetComponent<MonoBehaviour>().StopAllCoroutines();

			}
			else
			{
				Destroy(pObject);
			}
		}
		else
		{
			pool = new List<GameObject>();
			pool.Add(pObject);

			pObject.GetComponent<MonoBehaviour>().StopAllCoroutines();

			disabledPools.Add(pObject.name, pool);
		}
	}
}
