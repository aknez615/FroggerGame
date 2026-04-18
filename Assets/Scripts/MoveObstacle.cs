using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveObstacle : MonoBehaviour
{
    public float speed = 2f;
    public Vector3 direction = Vector3.right;

    private void Start()
    {
        gameObject.SetActive(false);
    }

    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);
    }

 
}
