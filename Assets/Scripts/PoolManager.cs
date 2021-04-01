using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public static PoolManager Instance { get; set; }
    Dictionary<int, Queue<GameObject>> pool = new Dictionary<int, Queue<GameObject>>();

    public void Awake()
    {
        Instance = this;
    }


    public void CreatePool(GameObject gameObj, int numberOfObj)
    {
        int gameObjKey = gameObj.GetInstanceID();
        if (!pool.ContainsKey(gameObjKey))
        {
            pool.Add(gameObjKey, new Queue<GameObject>());
            for (int i = 0; i < numberOfObj; i++)
            {
                GameObject go = Instantiate(gameObj) as GameObject;
                go.SetActive(false);
                pool[gameObjKey].Enqueue(go);
            }
        }
    }

    public void Reuse(int gameObjKey, Vector3 position, Quaternion orientation)
    {
        GameObject go = pool[gameObjKey].Dequeue();
        go.SetActive(true);
        go.transform.position = position;
        go.transform.rotation = orientation;
        pool[gameObjKey].Enqueue(go);
    }

    public void Reuse(int gameObjKey)
    {
        GameObject go = pool[gameObjKey].Dequeue();
        go.SetActive(true);
        pool[gameObjKey].Enqueue(go);
    }

    public void Reuse(int gameObjKey, Vector3 position)
    {
        GameObject go = pool[gameObjKey].Dequeue();
        go.transform.position = position;
        go.SetActive(true);
        pool[gameObjKey].Enqueue(go);
    }

    public void DestroyObj(int gameObjKey, Vector3 position)
    {
        Debug.Log(position);
        for (int i = 0; i < pool[gameObjKey].Count; i++)
        {
            GameObject go = pool[gameObjKey].Dequeue();
            if (go.activeSelf && go.transform.position == position)
            {
                Debug.Log(go.transform.position);
                go.SetActive(false);
            }
            pool[gameObjKey].Enqueue(go);
        }
    }
}
