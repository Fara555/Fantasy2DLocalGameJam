using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour
{
    public GameObject prefab;
    public int poolSize = 10;

    private List<GameObject> objectsPool;

    void Awake()
    {
        // Инициализация пула
        objectsPool = new List<GameObject>();

        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(prefab);
            obj.transform.parent = gameObject.transform;
            obj.SetActive(false);
            objectsPool.Add(obj);
        }
    }

    public GameObject GetPooledObject()
    {
        foreach (GameObject obj in objectsPool)
        {
            if (!obj.activeInHierarchy)
            {
                ResetObjectState(obj);
                return obj;
            }
        }

        // Если все объекты активны, создаем новый и добавляем в пул
        GameObject newObject = Instantiate(prefab);
        newObject.SetActive(false);
        objectsPool.Add(newObject);
        return newObject;
    }

    private void ResetObjectState(GameObject obj)
    {
        obj.transform.position = Vector3.zero;
        obj.transform.rotation = Quaternion.identity;
        Rigidbody2D rb = obj.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }
    }
}
