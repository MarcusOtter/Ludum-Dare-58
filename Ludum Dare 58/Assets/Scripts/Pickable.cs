using UnityEngine;

public class Pickable : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    private Transform _target;
    
    private void Update()
    {
        if (!_target)
        {
            return;
        }    
        
        rb.transform.position = _target.position;
    }

    public void PickUp(Transform parent)
    {
        _target = parent;
        rb.isKinematic = true;
    }

    public void Drop()
    {
        _target = null;
        rb.isKinematic = false;
    }
}
