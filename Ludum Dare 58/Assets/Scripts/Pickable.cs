using UnityEngine;

public class Pickable : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Collider coll;
    
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
        if (_target)
        {
            var visitor = _target.GetComponentInParent<Visitor>();
            visitor.NotifyItemWasStolen();
        }
        
        _target = parent;
        rb.isKinematic = true;
        coll.enabled = false;
    }

    public void Drop()
    {
        _target = null;
        rb.isKinematic = false;
        coll.enabled = true;
    }
}
