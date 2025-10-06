using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Collider))]
public class Hand : MonoBehaviour
{
    public Player player;
    
    internal bool IsSafe = true;
    internal bool IsHoldingItem => _heldPickable;
    internal ItemType HeldItemType => _heldPickable?.Item.type ?? ItemType.None;
    
    [SerializeField] private UnityEvent onHandPressed;
    [SerializeField] private UnityEvent onHandReleased;

    private InputAction _useHandAction;
    [CanBeNull] private Pickable _heldPickable;

    private bool _isPressed;
    
    private void Awake()
    {
        _useHandAction = InputSystem.actions.FindAction("Attack");
    }
    
    private void Update()
    {
        if (player && player.IsStunned)
        {
            _isPressed = false;
            return;
        }
        
        _isPressed = _useHandAction.IsPressed();
        
        if (_useHandAction.WasPressedThisFrame())
        {
            onHandPressed?.Invoke();
        }

        if (_useHandAction.WasReleasedThisFrame())
        {
            DropItem();
        }
    }

    public Pickable DropItem()
    {
        var pickableToReturn = _heldPickable;
        onHandReleased?.Invoke();
        _heldPickable?.Drop();
        _heldPickable = null;
        
        return pickableToReturn;
    }
    
    private void TryPickUp(Collider other)
    {
        if (!_isPressed || _heldPickable)
        {
            return;
        }

        var pickable = other.GetComponentInParent<Pickable>();
        if (!pickable)
        {
            return;
        }
        
        pickable.PickUp(transform);
        _heldPickable = pickable;
    }

    private void OnTriggerEnter(Collider other)
    {
        TryPickUp(other);

        if (other.CompareTag("Safe"))
        {
            IsSafe = true;
        }
    }
    
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Safe"))
        {
            IsSafe = true;
        }
    }

    
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Safe"))
        {
            IsSafe = false;
        }
    }
}
