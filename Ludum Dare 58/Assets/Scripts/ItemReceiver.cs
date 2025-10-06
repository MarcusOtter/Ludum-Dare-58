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
        _player = FindFirstObjectByType<Player>();
        UpdateUI();
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
        if (isPoints)
        {
            _player.AddScore(pickable.Item.score);
        }
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
        progressText.text = isPoints ? $"{_player.Score} pts" : $"{_currentAmount}/{requiredAmount}";
        iconSlot.texture = wantedItem.icon;
    }
}
