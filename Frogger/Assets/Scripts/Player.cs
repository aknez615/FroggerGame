using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float moveDelay = 0.2f;

    private Vector3 spawnPoint;
    private float lastRow;
    private bool cooldown;

    private void Awake()
    {
        spawnPoint = transform.position;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            Move(Vector3.up);
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            transform.rotation = Quaternion.Euler(0f, 0f, 90f);
            Move(Vector3.left);
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            transform.rotation = Quaternion.Euler(0f, 0f, 180f);
            Move(Vector3.down);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            transform.rotation = Quaternion.Euler(0f, 0f, 270f);
            Move(Vector3.right);
        }
    }

    private void Move(Vector3 direction)
    {
        if (cooldown) return;

        cooldown = true;
        Invoke(nameof(ResetCooldown), moveDelay);
        Vector3 destination = transform.position + direction;

        Collider2D platform = Physics2D.OverlapBox(destination, Vector2.one * 0.5f, 0f, LayerMask.GetMask("Platform"));
        Collider2D obstacle = Physics2D.OverlapBox(destination, Vector2.one * 0.5f, 0f, LayerMask.GetMask("Obstacle"));
        Collider2D barrier = Physics2D.OverlapBox(destination, Vector2.one * 0.5f, 0f, LayerMask.GetMask("Barrier"));

        if (barrier != null)
        {
            return;
        }

        //Move player
        transform.position = destination;

        //Platforming
        if (platform != null)
        {
            transform.SetParent(platform.transform);
        }
        else
        {
            transform.SetParent(null);
        }

        //Hit obstacle
        if (obstacle != null && platform == null)
        {
            transform.position = destination;
            Die();
        }

        //Progress
        if (destination.y > lastRow)
        {
            lastRow = destination.y;
            //GameManager.Instance.AdvancedRow();
        }
    }

    private void ResetCooldown()
    {
        cooldown = false;
    }

    public void Respawn()
    {
        transform.SetPositionAndRotation(spawnPoint, Quaternion.identity);
        lastRow = spawnPoint.y;

        gameObject.SetActive(true);
        enabled = true;
        cooldown = false;
    }

    public void Die()
    {
        enabled = false;

        GameManager.Instance.Died();
    }

    public void AdvancedRow()
    {
        //Can be used for score if wanted
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        bool hitObstacle = other.gameObject.layer == LayerMask.NameToLayer("Obstacle");
        bool onPlatform = transform.parent != null;

        if (enabled && hitObstacle && !onPlatform)
        {
            Die();
        }
    }
}
