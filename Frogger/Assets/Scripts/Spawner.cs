using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject prefab;
    public float spawnRate = 2f;
    public Vector3 direction = Vector3.right;
    public float speed = 2f;

    public float spawnX;
    public float spawnY;

    private void Start()
    {
        InvokeRepeating(nameof(Spawn), 0f, spawnRate);
    }

    private void Spawn()
    {
        GameObject obj = Instantiate(prefab, transform.position, Quaternion.identity);

        MoveObstacle obstacle = obj.GetComponent<MoveObstacle>();
        if (obstacle != null)
        {
            obstacle.direction = direction;
            obstacle.speed = speed;
        }
    }
}
