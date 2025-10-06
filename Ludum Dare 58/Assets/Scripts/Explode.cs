using UnityEngine;
// using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class Explode : MonoBehaviour
{
    [SerializeField] private Transform explosionPosition;
    [SerializeField] private float explosionForce = 10f;

    private Rigidbody _rigidbody;
    
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    // private void Update()
    // {
    //     var space = InputSystem.actions.FindAction("Jump", true);
    //     if (space.WasPressedThisFrame())
    //     {
    //         ExplodeNow();
    //     }
    // }
    
    public void ExplodeNow()
    {
        _rigidbody.isKinematic = false;
        _rigidbody.AddExplosionForce(explosionForce, explosionPosition.position, 1f, 9f, ForceMode.Impulse);
    }
}
