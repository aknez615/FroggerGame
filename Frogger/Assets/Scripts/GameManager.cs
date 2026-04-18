using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private Player player;
    [SerializeField] private GameObject winGoal;
    //[SerializeField] private Text livesText;

    public int lives = 3;

    private void Awake()
    {
        if (Instance != null)
        {
            DestroyImmediate(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    private void Start()
    {
        SetLives(3);
    }

    private void Respawn()
    {
        player.Respawn();
    }

    public void Died()
    {
        SetLives(lives - 1);

        if (lives > 0)
        {
            Invoke(nameof(Respawn), 1f);
        }
        else
        {
            Invoke(nameof(GameOver), 1f);
        }
    }

    private void GameOver()
    {
        player.gameObject.SetActive(false);
        RestartGame();
    }

    private void RestartGame()
    {
        player.gameObject.SetActive(true);
        SetLives(3);
        Respawn();
    }

    private void SetLives(int lives)
    {
        this.lives = lives;
        //livesText.text = lives.ToString();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("WinGoal"))
        {
            Debug.Log("Win");
            RestartGame();
        }
    }
}
