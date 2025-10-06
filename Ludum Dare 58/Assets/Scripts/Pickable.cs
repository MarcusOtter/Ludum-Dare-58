using UnityEngine;

public class Pickable : MonoBehaviour
{
    public Item Item => item;
    
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Collider coll;
    [SerializeField] private Item item;
    
    private Transform _target;
    
    private void Update()
    {
        if (!_target)
        {
            return;
        }    
        
        rb.transform.position = _target.position;
        rb.transform.rotation = _target.rotation;
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
        if (!rb || !coll)
        {
            return;
        }
        
        rb.isKinematic = false;
        coll.enabled = true;
    }
}
