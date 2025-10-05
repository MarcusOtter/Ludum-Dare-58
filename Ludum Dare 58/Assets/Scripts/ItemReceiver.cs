using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ItemReceiver : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private Item wantedItem;
    [SerializeField] private int requiredAmount;
    
    [Header("References")]
    [SerializeField] private TextMeshProUGUI progressText;
    [SerializeField] private RawImage iconSlot;
    
    [Header("Events")]
    public UnityEvent onProgress;
    public UnityEvent onAllItemsReceived;
    
    private int _currentAmount;

    private void Awake()
    {
        UpdateUI();
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent(out Hand hand))
        {
            return;
        }

        if (hand.HeldItemType == ItemType.None || hand.HeldItemType != wantedItem.type)
        {
            return;
        }
        
        var pickable = hand.DropItem();
        _currentAmount++;
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
        progressText.text = $"{_currentAmount}/{requiredAmount}";
        iconSlot.texture = wantedItem.icon;
    }
}
