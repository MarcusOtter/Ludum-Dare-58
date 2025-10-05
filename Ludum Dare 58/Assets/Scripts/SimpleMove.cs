using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class SimpleMove : MonoBehaviour
{
    [SerializeField] private float speed = 1f;
    
    private Rigidbody _rigidbody;
    private InputAction _moveAction;
    private Player _player;
    
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _moveAction = InputSystem.actions.FindAction("Move");
        _player = FindFirstObjectByType<Player>();
    }

    private void Update()
    {
        if (_player && _player.IsStunned)
        {
            _rigidbody.linearVelocity = Vector3.zero;
            return;
        }
        
        var moveValue = _moveAction.ReadValue<Vector2>();
        _rigidbody.linearVelocity = new Vector3(moveValue.x, 0, moveValue.y) * speed;
    }
}
