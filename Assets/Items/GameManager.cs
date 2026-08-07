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

    [Header("Ending Objects")]
    public GameObject helicopter;

    [Header("Player Settings")]
    [SerializeField] private int maxHealth = 100;
    public int PlayerHealth { get; private set; }

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI fuelText;
    [SerializeField] private TextMeshProUGUI fixedText;
    [SerializeField] private TextMeshProUGUI bunkerText;
    

    [Header("Game Rules")]
    [SerializeField] private int itemsNeeded = 2;
    [SerializeField] private float exfilTime = 6f;

    public GameState CurrentState { get; private set; } = GameState.PLAYING;

    // Trackers
    public int KeyItemsCount { get; private set; }
    public bool PlayerInExfilZone { get; set; }
    public bool CarFixed { get; private set; }
    public bool PlayerInCar { get; private set; }
    private bool won = false;
    private bool winState => CurrentState == GameState.WON_ENDING_BUNKER || CurrentState == GameState.WON_ENDING_EXTRACT || CurrentState == GameState.WON_ENDING_CAR;

    // Timers
    private float healthRegenDelayTimer = 0f;
    private float regenTickTimer = 0f;
    private float invincibilityTimer = 0f;

    private const float REGEN_DELAY = 10f;
    private const float REGEN_TICK_RATE = 0.5f; // Regenerates 1 HP every 0.5s after delay
    private const float INVINC_TIME = 0.3f;


    private int carFuelCount = 0;
    private int carFuelCapacity = 5;

    public bool carFull => carFuelCount == carFuelCapacity;

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
        UpdateBunkerUI();
    }

    private void Update()
    {
        if (winState && !won) OnWin();
        if (CurrentState != GameState.PLAYING) return;

        HandleTimers();
        HandleHealthRegen();
        CheckExfilWinCondition();
    }

    private void OnWin()
    {
        won = true;
        helicopter.SetActive(false);
    }


    private void HandleTimers()
    {
        if (invincibilityTimer > 0) invincibilityTimer -= Time.deltaTime;
        if (healthRegenDelayTimer > 0) healthRegenDelayTimer -= Time.deltaTime;

        if (exfilTime > 0)
        {
            exfilTime -= Time.deltaTime;
            UpdateTimerUI();
            if (exfilTime <= 0)
            {
                SpawnHelicopterZone();  
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
                Heal(2);
            }
        }
    }

    public void FillCar()
    {
        if (!CarFixed) return;

        carFuelCount++;

        if (carFull) {
            TriggerEnding(GameState.WON_ENDING_CAR);
        }
        UpdateFuelUI();
    }

    public void SpawnHelicopterZone()
    {
        helicopter.SetActive(true);
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

    public void RegisterTimerText(TextMeshProUGUI textComponent)
    {
        timerText = textComponent;

    }

    public void AddKeyItem()
    {
        KeyItemsCount++;
        UpdateBunkerUI();
        if (KeyItemsCount >= itemsNeeded)
        {
            TriggerEnding(GameState.WON_ENDING_BUNKER);
        }
    }

    public void RemoveKeyItem()
    {
        KeyItemsCount = Math.Max(0, KeyItemsCount - 1);
        UpdateBunkerUI();
    }

    public void SetCarFixed(bool status)
    {
        CarFixed = status;
        UpdateFixedUI();
    }

  
    private void CheckExfilWinCondition()
    {
        if (exfilTime <= 0 && PlayerInExfilZone)
        {
            TriggerEnding(GameState.WON_ENDING_EXTRACT);
        }
    }



    private void TriggerEnding(GameState newEndingState)
    {
        if (CurrentState != GameState.PLAYING) return;

        CurrentState = newEndingState;

        string endingMessage = "";

        switch (CurrentState)
        {
            case GameState.WON_ENDING_BUNKER:
                endingMessage = "BUNKER ENDING\n\nYou secured the bunker and survived the night.";
                break;

            case GameState.WON_ENDING_EXTRACT:
                endingMessage = "EXFIL ENDING\n\nThe evacuation team picked you up in time.";
                break;

            case GameState.WON_ENDING_CAR:
                endingMessage = "CAR ENDING\n\nYou fixed the car and escaped the house.";
                break;
        }

        if (EndingUI.Instance != null)
        {
            EndingUI.Instance.ShowEnding(endingMessage);
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

    private void UpdateTimerUI()
    {
        if (timerText != null)
        {
            timerText.text = $"{Mathf.RoundToInt(exfilTime)}s";
        }
    }

    private void UpdateFuelUI()
    {
        if (fuelText != null)
        {
            fuelText.text = $"{carFuelCount}/{carFuelCapacity} fuel";
        }
    }

    private void UpdateFixedUI()
    {
        if (fixedText != null)
        {
            fixedText.text = $"{(CarFixed ? "Fixed" : "Unfixed")}";
        }
    }

    private void UpdateBunkerUI()
    {
        if (bunkerText != null)
        {
            bunkerText.text = $"{KeyItemsCount}/{itemsNeeded} carrots";
        }
    }

    public void RegisterFuelUI(TextMeshProUGUI c)
    {
        fuelText = c;
    }

    public void RegisterFixedUI(TextMeshProUGUI c)
    {
        fixedText = c;
    }

    public void RegisterBunkerUI(TextMeshProUGUI c)
    {
        bunkerText = c;
    }
}