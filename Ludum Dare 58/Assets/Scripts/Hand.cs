using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class Hand : MonoBehaviour
{
    [SerializeField] private UnityEvent onHandPressed;
    [SerializeField] private UnityEvent onHandReleased;

    private InputAction _useHandAction;
    
    private void Awake()
    {
        _useHandAction = InputSystem.actions.FindAction("Attack");
    }
    
    private void Update()
    {
        var isPressed = _useHandAction.IsPressed();
        
        if (_useHandAction.WasPressedThisFrame())
        {
            onHandPressed?.Invoke();
        }

        if (_useHandAction.WasReleasedThisFrame())
        {
            onHandReleased?.Invoke();
        }
    }
}
