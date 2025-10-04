using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class SimpleMove : MonoBehaviour
{
    [SerializeField] private float speed = 1f;
    
    private Rigidbody _rigidbody;
    private InputAction _moveAction;
    
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _moveAction = InputSystem.actions.FindAction("Move");
    }

    private void Update()
    {
        var moveValue = _moveAction.ReadValue<Vector2>();
        _rigidbody.linearVelocity = new Vector3(moveValue.x, 0, moveValue.y) * speed;
    }
}
