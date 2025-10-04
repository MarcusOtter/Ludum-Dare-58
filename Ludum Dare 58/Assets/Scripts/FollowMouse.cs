using UnityEngine;
using UnityEngine.InputSystem;

public class FollowMouse : MonoBehaviour
{
    [SerializeField] private Camera cam;
    private float _cameraDistance;
    
    private void Awake()
    {
        _cameraDistance = cam.transform.position.z * -1;
    }
    
    public void Update()
    {
        var mousePosition = Mouse.current.position.ReadValue().ToVector3(z: _cameraDistance);
        var worldPosition = cam.ScreenToWorldPoint(mousePosition);
        
        transform.position = worldPosition.With(z: 0);
    }
}
