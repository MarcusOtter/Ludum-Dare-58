using UnityEngine;

public class Distraction : MonoBehaviour
{
    [SerializeField] private float interruptDuration = 2f;
    [SerializeField] private float cooldown = 1f;
    [SerializeField] private bool destroyAfterUse;

    private float _lastTriggerTime = float.MinValue;
    
    private void OnCollisionEnter(Collision other)
    {
        if (Time.time < _lastTriggerTime + cooldown)
        {
            return;
        }
        
        var visitor = other.collider.GetComponentInParent<Visitor>();
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
}
