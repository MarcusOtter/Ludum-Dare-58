using UnityEngine;
using UnityEngine.InputSystem;

public class FollowMouseRaycast : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private float yPosition = 1f;
    [SerializeField] private LayerMask layerMask;
    
    public void Update()
    {
        var ray = cam.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (Physics.Raycast(ray, out var hit, Mathf.Infinity, layerMask))
        {
            transform.position = hit.point.With(y: yPosition);
        }
    }
}