
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour
{
    public GameObject prefab;
    public int poolSize = 10;

    private List<GameObject> objectsPool;

    void Awake()
    {
        // ������������� ����
        objectsPool = new List<GameObject>();

        for (int i = 0; i < poolSize; i++)
        {
            GameObject enemy = Instantiate(prefab);
            enemy.transform.parent = gameObject.transform;
            enemy.SetActive(false);
            objectsPool.Add(enemy);
        }
    }

    public GameObject GetPooledObject()
    {
        foreach (GameObject obj in objectsPool)
        {
            if (!obj.activeInHierarchy)
            {
                return obj;
            }
        }

        // ���� ��� ������� �������, ������� ����� � ��������� � ���
        GameObject newObject = Instantiate(prefab);
        newObject.SetActive(false);
        objectsPool.Add(newObject);
        return newObject;
    }
}