using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public enum GameState
    {
        PLAYING,
        WON_ENDING_BUNKER,
        WON_ENDING_EXTRACT,
        WON_ENDING_CAR
    }

    [Header("Player Settings")]
    [SerializeField] private int maxHealth = 100;
    public int PlayerHealth { get; private set; }

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI healthText;

    [Header("Game Rules")]
    [SerializeField] private int itemsNeeded = 12;
    [SerializeField] private float exfilTime = 600f;

    public GameState CurrentState { get; private set; } = GameState.PLAYING;

    // Trackers
    public int KeyItemsCount { get; private set; }
    public bool PlayerInExfilZone { get; set; }
    public bool CarFixed { get; private set; }
    public bool PlayerInCar { get; private set; }

    // Timers
    private float healthRegenDelayTimer = 0f;
    private float regenTickTimer = 0f;
    private float invincibilityTimer = 0f;

    private const float REGEN_DELAY = 10f;
    private const float REGEN_TICK_RATE = 0.5f; // Regenerates 1 HP every 0.5s after delay
    private const float INVINC_TIME = 0.3f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        PlayerHealth = maxHealth;
    }

    private void Start()
    {
        UpdateHealthUI();
    }

    private void Update()
    {
        if (CurrentState != GameState.PLAYING) return;

        HandleTimers();
        HandleHealthRegen();
        CheckExfilWinCondition();
    }


    private void HandleTimers()
    {
        if (invincibilityTimer > 0) invincibilityTimer -= Time.deltaTime;
        if (healthRegenDelayTimer > 0) healthRegenDelayTimer -= Time.deltaTime;

        if (exfilTime > 0)
        {
            exfilTime -= Time.deltaTime;
            if (exfilTime <= 0)
            {
                Debug.Log("Exfil Timer Ended! Get to the zone..");
            }
        }
    }

    private void HandleHealthRegen()
    {
        if (healthRegenDelayTimer <= 0 && PlayerHealth < maxHealth)
        {
            regenTickTimer += Time.deltaTime;
            if (regenTickTimer >= REGEN_TICK_RATE)
            {
                regenTickTimer = 0f;
                Heal(1);
            }
        }
    }

    public void Damage(int damage)
    {
        if (invincibilityTimer > 0 || CurrentState != GameState.PLAYING) return;

        invincibilityTimer = INVINC_TIME;
        healthRegenDelayTimer = REGEN_DELAY;
        regenTickTimer = 0f;

        PlayerHealth = Mathf.Max(0, PlayerHealth - damage);
        UpdateHealthUI();

        if (PlayerHealth <= 0)
        {
            RestartGame();
        }
    }

    public void Heal(int amount)
    {
        PlayerHealth = Mathf.Min(maxHealth, PlayerHealth + amount);
        UpdateHealthUI();
    }

    public void RegisterHealthText(TextMeshProUGUI textComponent)
    {
        healthText = textComponent;
        UpdateHealthUI(); 
    }

    public void AddKeyItem()
    {
        KeyItemsCount++;
        if (KeyItemsCount >= itemsNeeded)
        {
            TriggerEnding(GameState.WON_ENDING_BUNKER);
        }
    }

    public void RemoveKeyItem()
    {
        KeyItemsCount = Math.Max(0, KeyItemsCount - 1);
    }

    public void SetCarFixed(bool status)
    {
        CarFixed = status;
        CheckCarEnding();
    }

    public void SetPlayerInCar(bool status)
    {
        PlayerInCar = status;
        CheckCarEnding();
    }


    private void CheckExfilWinCondition()
    {
        if (exfilTime <= 0 && PlayerInExfilZone)
        {
            TriggerEnding(GameState.WON_ENDING_EXTRACT);
        }
    }

    private void CheckCarEnding()
    {
        if (CarFixed && PlayerInCar)
        {
            TriggerEnding(GameState.WON_ENDING_CAR);
        }
    }

    private void TriggerEnding(GameState newEndingState)
    {
        if (CurrentState != GameState.PLAYING) return;

        CurrentState = newEndingState;

        switch (CurrentState)
        {
            case GameState.WON_ENDING_BUNKER:
                Debug.Log("Bunker ending");
                break;
            case GameState.WON_ENDING_EXTRACT:
                Debug.Log("Exfil ending");
                break;
            case GameState.WON_ENDING_CAR:
                Debug.Log("Car ending");
                break;
        }
    }

    private void RestartGame()
    {
        PlayerHealth = maxHealth;
        UpdateHealthUI();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void UpdateHealthUI()
    {
        if (healthText != null)
        {
            healthText.text = $"{PlayerHealth} HP";
        }
    }
}