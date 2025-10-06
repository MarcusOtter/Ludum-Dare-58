using UnityEngine;

public class Player : MonoBehaviour
{
    public int Health { get; private set; }
    public bool IsStunned { get; private set; }

    [Header("Settings")]
    [SerializeField] private int startingHealth = 3;

    [SerializeField] private int invulnerabilityTime = 2;

    [Header("References")]
    [SerializeField] private Transform healthUiContainer;
    [SerializeField] private GameObject heartUiPrefab;

    private Vector3 _startPosition;
    private float _lastTimeTakenDamage;
    
    private void Awake()
    {
        Health = startingHealth;
        for (var i = 0; i < startingHealth; i++)
        {
            Instantiate(heartUiPrefab, healthUiContainer.transform);
        }
        
        _startPosition = transform.position;
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
    }

    public void Respawn()
    {
        transform.position = _startPosition;
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
