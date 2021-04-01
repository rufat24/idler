using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    public GameObject tree;

    public static List<Vector3> transforms = new List<Vector3>();
    public static int instanceID;

    void Start()
    {
        instanceID = tree.GetInstanceID();
        PoolManager.Instance.CreatePool(tree, 10);
        for (int i = 0; i < 3; i++)
        {
            var temp = new Vector3(Random.Range(2, 20), 0, Random.Range(2, 20));
            PoolManager.Instance.Reuse(instanceID, temp, Quaternion.Euler(0, Random.Range(0, 360), 0));
            temp.x -= 1.5f;
            temp.z -= 1.5f;
            transforms.Add(temp);
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ManualTreePlant();
        }
    }

    void ManualTreePlant()
    {
        Ray r = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit h;
        if (Physics.Raycast(r, out h))
        {
            PoolManager.Instance.Reuse(instanceID, new Vector3(h.point.x, 0, h.point.z), Quaternion.Euler(0, Random.Range(0, 360), 0));
            transforms.Add(new Vector3(h.point.x - 1.5f, 0f, h.point.z - 1.5f));
        }
    }
}