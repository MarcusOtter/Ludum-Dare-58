using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Collider))]
public class Hand : MonoBehaviour
{
    public Transform player;
    
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
        _isPressed = _useHandAction.IsPressed();
        
        if (_useHandAction.WasPressedThisFrame())
        {
            onHandPressed?.Invoke();
        }

        if (_useHandAction.WasReleasedThisFrame())
        {
            onHandReleased?.Invoke();
            _heldPickable?.Drop();
            _heldPickable = null;
        }
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

    private void TryPickUp(Collider other)
    {
        if (!_isPressed || _heldPickable)
        {
            return;
        }

        if (!other.TryGetComponent<Pickable>(out var pickable))
        {
            return;
        }
        
        pickable.PickUp(transform);
        _heldPickable = pickable;
    }
}
