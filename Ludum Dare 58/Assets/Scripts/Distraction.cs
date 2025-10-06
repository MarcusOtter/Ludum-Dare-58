using UnityEngine;

[System.Serializable]
public enum DistractionTrigger
{
    None,
    CollideWithVisitor,
    HitByHand,
}

public class Distraction : MonoBehaviour
{
    [SerializeField] private float interruptDuration = 2f;
    [SerializeField] private float cooldown = 0.3f;
    [SerializeField] private bool destroyAfterUse;
    [SerializeField] private DistractionTrigger triggerMechanic = DistractionTrigger.CollideWithVisitor;
    [SerializeField] private float radius;

    private float _lastTriggerTime = float.MinValue;
    
    private void OnCollisionEnter(Collision collision)
    {
        if (Time.time < _lastTriggerTime + cooldown)
        {
            return;
        }

        if (triggerMechanic == DistractionTrigger.CollideWithVisitor)
        {
            CollideWithVisitor(collision.collider);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (Time.time < _lastTriggerTime + cooldown)
        {
            return;
        }
        
        if (triggerMechanic == DistractionTrigger.HitByHand)
        {
            CollideWithHand(other);
        }
    }

    private void CollideWithVisitor(Collider other)
    {
        var visitor = other.GetComponentInParent<Visitor>();
        if (!visitor)
        {
            return;
        }
        
        visitor.Interrupt(interruptDuration, disableCone: true);
        _lastTriggerTime = Time.time;
        if (destroyAfterUse)
        {
            Destroy(transform.root.gameObject);
        }
    }

    private void CollideWithHand(Collider other)
    {
        var hand = other.GetComponent<Hand>();
        if (!hand)
        {
            return;
        }

        var hitColliders = Physics.OverlapSphere(transform.position, radius);

        foreach (var coll in hitColliders)
        {
            var visitor = coll.GetComponentInParent<Visitor>();
            if (!visitor) continue;
            
            visitor.Interrupt(interruptDuration, transform.position, true);
        }
    }
}
