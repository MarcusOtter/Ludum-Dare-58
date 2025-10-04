using UnityEngine;
using UnityEngine.InputSystem;

public class FollowMouseRaycast : MonoBehaviour
{
    [SerializeField] private Camera cam;
    
    public void Update()
    {
        // var ray = cam.ScreenPointToRay(Mouse.current.position.ReadValue());
        // var hit = Physics.Raycast
        // var worldPosition = cam.ScreenToWorldPoint(mousePosition);
        
        // transform.position = worldPosition.With(z: 0);
    }
}