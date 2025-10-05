using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Collider))]
public class Hand : MonoBehaviour
{
    public Player player;
    
    internal bool IsInJail = true;
    internal bool IsHoldingItem => _heldPickable;
    
    [SerializeField] private Collider insideJailTrigger;
    [SerializeField] private UnityEvent onHandPressed;
    [SerializeField] private UnityEvent onHandReleased;

    private InputAction _useHandAction;
    private Pickable _heldPickable;

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

    public void DropItem()
    {
        onHandReleased?.Invoke();
        _heldPickable?.Drop();
        _heldPickable = null;
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

        if (other == insideJailTrigger)
        {
            IsInJail = true;
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other == insideJailTrigger)
        {
            IsInJail = false;
        }
    }
}
