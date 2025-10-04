using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Collider))]
public class Hand : MonoBehaviour
{
    [SerializeField] private UnityEvent onHandPressed;
    [SerializeField] private UnityEvent onHandReleased;

    private InputAction _useHandAction;
    private Pickable _heldPickable;
    
    private bool isPressed = false;
    
    private void Awake()
    {
        _useHandAction = InputSystem.actions.FindAction("Attack");
    }
    
    private void Update()
    {
        isPressed = _useHandAction.IsPressed();
        
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
        if (!isPressed || _heldPickable)
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
