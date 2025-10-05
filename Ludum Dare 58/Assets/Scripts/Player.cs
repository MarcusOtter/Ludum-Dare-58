using UnityEngine;

public class Player : MonoBehaviour
{
    public int Health { get; private set; }
    public bool IsStunned { get; private set; }

    [Header("Settings")]
    [SerializeField] private int startingHealth = 3;

    [Header("References")]
    [SerializeField] private Transform healthUiContainer;
    [SerializeField] private GameObject heartUiPrefab;

    private void Awake()
    {
        Health = startingHealth;
        for (var i = 0; i < startingHealth; i++)
        {
            Instantiate(heartUiPrefab, healthUiContainer.transform);
        }
    }

    public void TakeDamage(int amount)
    {
        Health -= amount;
        UpdateHealthUi();
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
