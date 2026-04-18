using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public List<GameObject> prefab;
    public float spawnRate = 2f;
    public Vector3 direction = Vector3.right;
    public float speed = 2f;

    public float spawnX;
    public float spawnY;

    private int poolIndex = 0;

    private void Start()
    {
        InvokeRepeating(nameof(Spawn), 0f, spawnRate);
    }

    private void Spawn()
    {
        if(poolIndex > prefab.Count-1)
        {
            poolIndex = 0;
        }

        GameObject obj = prefab[poolIndex];
        obj.SetActive(true);
        obj.transform.position = transform.position;
        obj.transform.rotation = Quaternion.identity;

        MoveObstacle obstacle = obj.GetComponent<MoveObstacle>();
        if (obstacle != null)
        {
            obstacle.direction = direction;
            obstacle.speed = speed;
        }
        poolIndex++;
    }
}
