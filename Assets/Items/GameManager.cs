using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
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
    }

    public void OnBunkerEnding()
    {
        
    }

    public void OnExfilEnding()
    {
        
    }

    public void OnCarEnding()
    {
        
    }

    public void OnExfilTimerEnd()
    {
        
    }
}
