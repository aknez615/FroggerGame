using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] private float moveDelay = 0.2f;

    private Vector3 spawnPoint;
    private float lastRow;
    private bool cooldown;

    public PlayerMovement playerInput;
    public InputAction movement;

    private void Awake()
    {
        playerInput = new PlayerMovement();
        spawnPoint = transform.position;
    }

    private void OnEnable()
    {
        movement = playerInput.Movement.WASD;
        movement.Enable();
    }

    private void OnDisable()
    {
        movement.Disable();
    }

    private void FixedUpdate()
    {
        Move(movement.ReadValue<Vector2>());
    }

    private void Move(Vector3 direction)
    {
        if (cooldown) return;

        cooldown = true;
        Invoke(nameof(ResetCooldown), moveDelay);
        Vector3 destination = transform.position + direction;

        Collider2D platform = Physics2D.OverlapBox(destination, Vector2.one * 0.5f, 0f, LayerMask.GetMask("Platform"));

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
