using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public static float TimePlayed { get; private set; } 
        
    public int Health { get; private set; }
    public bool IsStunned { get; private set; }
    public int Score { get; private set; }

    [Header("Settings")]
    [SerializeField] private int startingHealth = 3;

    [SerializeField] private int invulnerabilityTime = 2;

    [Header("References")]
    [SerializeField] private Transform healthUiContainer;
    [SerializeField] private GameObject heartUiPrefab;
    [SerializeField] private GameObject gameOverUi;

    private InputAction _restartAction;
    private Vector3 _startPosition;
    private float _lastTimeTakenDamage;
    
    private void Awake()
    {
        TimePlayed = 0;
        Health = startingHealth;
        for (var i = 0; i < startingHealth; i++)
        {
            Instantiate(heartUiPrefab, healthUiContainer.transform);
        }
        
        _startPosition = transform.position;
        _restartAction = InputSystem.actions.FindAction("Jump", true);
    }

    private void Update()
    {
        TimePlayed += Time.deltaTime;

        if (Health <= 0 && _restartAction.WasPressedThisFrame())
        {
            Time.timeScale = 1;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
    
    public void TakeDamage(int amount)
    {
        if (Time.time < _lastTimeTakenDamage + invulnerabilityTime)
        {
            return;
        }
        
        Health -= amount;
        UpdateHealthUi();
        _lastTimeTakenDamage = Time.time;

        if (Health <= 0)
        {
            gameOverUi.SetActive(true);
            Time.timeScale = 0;
        }
    }

    public void Respawn()
    {
        transform.position = _startPosition;
    }

    public void AddScore(int amount)
    {
        Score += amount;
    }

    private void UpdateHealthUi()
    {
        var heartsToEnable = Health;
        foreach (Transform heart in healthUiContainer)
        {
            heart.gameObject.SetActive(heartsToEnable > 0);
            heartsToEnable--;
        }
    }

    public void SetStunned(bool stunned)
    {
        IsStunned = stunned;
    }
}
