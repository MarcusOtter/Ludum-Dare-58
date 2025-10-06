using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ItemReceiver : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private Item wantedItem;
    [SerializeField] private int requiredAmount;
    [SerializeField] private bool isPoints;
    
    [Header("References")]
    [SerializeField] private TextMeshProUGUI progressText;
    [SerializeField] private RawImage iconSlot;
    
    [Header("Events")]
    public UnityEvent onProgress;
    public UnityEvent onAllItemsReceived;
    
    private int _currentAmount;

    private Player _player;
    
    private void Awake()
    {
        UpdateUI();
        _player = FindFirstObjectByType<Player>();
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent(out Hand hand))
        {
            return;
        }

        if (hand.HeldItemType == ItemType.None || (wantedItem.type != ItemType.All && hand.HeldItemType != wantedItem.type))
        {
            return;
        }
        
        var pickable = hand.DropItem();
        _currentAmount++;
        _player.AddScore(pickable.Item.score);
        UpdateUI();

        if (_currentAmount >= requiredAmount)
        {
            onAllItemsReceived?.Invoke();
        }
        else
        {
            onProgress?.Invoke();
        }
        
        Destroy(pickable.transform.root.gameObject);
    }

    private void UpdateUI()
    {
        progressText.text = isPoints ? $"{_currentAmount} pts" : $"{_currentAmount}/{requiredAmount}";
        iconSlot.texture = wantedItem.icon;
    }
}
