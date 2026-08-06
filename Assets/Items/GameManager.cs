using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public enum GameState
    {
        PLAYING,
        WON_ENDING_BUNKER,
        WON_ENDING_EXTRACT,
        WON_ENDING_CAR
    }
    public int playerHealth = 100;
    public readonly int MAX_HEALTH = 100;

    [Tooltip("For bunker ending; how many items inside the bunker area.")] public int keyItemsCount = 0;
    public int itemsNeeded = 12;
    public float exfilTime = 600f;
    public bool playerInExfilZone = false;

    public bool carFixed = false;
    public bool playerInCar = false;
    
    public GameState gameState = GameState.PLAYING;
    public TextMeshProUGUI healthText;

    private float healthTimer = 0f;
    private float invincTimer = 0f;
    private readonly float REGEN_TIME = 10f;
    private readonly float INVINC_TIME = 0.3f;


    void Update()
    {
        if (invincTimer > 0)
        {
            invincTimer -= Time.deltaTime;
        }

        if (healthTimer > 0)
        {
            healthTimer -= Time.deltaTime;
        }

        if (exfilTime > 0)
        {
            exfilTime -= Time.deltaTime;
        }

        if (healthTimer <= 0 && playerHealth < MAX_HEALTH)
        {
            playerHealth += 1;
        }

        if (playerHealth <= 0) {
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(currentSceneIndex);    
        }

        if (keyItemsCount == itemsNeeded)
        {
            gameState = GameState.WON_ENDING_BUNKER;
            OnBunkerEnding();
        }

        if (exfilTime <= 0 && playerInExfilZone)
        {
            gameState = GameState.WON_ENDING_EXTRACT;
            OnExfilEnding();
        }
        
        if (carFixed && playerInCar)
        {
            gameState = GameState.WON_ENDING_CAR;
            OnCarEnding();
        }

        if (healthText)
        {
            healthText.text = playerHealth + " HP"; 
        }
    }

    public void Damage(int damage)
    {
        if (invincTimer > 0) return;

        invincTimer = INVINC_TIME;
        playerHealth -= damage;
        healthTimer = REGEN_TIME;

        if (playerHealth < 0) playerHealth = 0;
    }

    public void OnBunkerEnding()
    {
        Debug.Log("Bunker ending");
    }

    public void OnExfilEnding()
    {
        Debug.Log("Exfil ending");
    }

    public void OnCarEnding()
    {
        Debug.Log("Car ending");
    }

    public void OnExfilTimerEnd()
    {
        Debug.Log("Exfil Timer Ended! Get to the zone..");
    }
}
