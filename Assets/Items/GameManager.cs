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

    [Tooltip("For bunker ending; how many items inside the bunker area.")] public int keyItemsCount = 0;
    public int itemsNeeded = 16;
    public float exfilTime = 600f;
    public bool playerInExfilZone = false;

    public bool carFixed = false;
    public bool playerInCar = false;
    
    public GameState gameState = GameState.PLAYING;
    public TextMeshProUGUI healthText;


    void Update()
    {
        if (exfilTime > 0)
        {
            exfilTime -= Time.deltaTime;
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
