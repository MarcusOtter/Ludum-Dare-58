using UnityEngine;

public class ScaleWithDistanceToTarget : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float scaleMultiplier = 0.25f;
    [SerializeField] private float maxScale = 2f;

    [SerializeField] private float scaleUpSpeed = 10f;
    [SerializeField] private float scaleDownSpeed = 50f;
    [SerializeField] private float speedIfHoldingItem = 50f;

    private Hand _hand;
    
    private void Awake()
    {
        _hand = FindFirstObjectByType<Hand>();
    }
    
    private void Update()
    {
        var newScale = target
            ? Vector3.Magnitude(target.position - transform.position) * scaleMultiplier
            : 1;
        var targetScale = Mathf.Min(newScale, maxScale);
        
        var speed = target ? scaleUpSpeed : scaleDownSpeed;
        if (_hand && _hand.IsHoldingItem)
        {
            speed = speedIfHoldingItem;
        }
        
        var scaleThisFrame = Mathf.MoveTowards(transform.localScale.x, targetScale, speed * Time.deltaTime);
        
        transform.localScale = transform.localScale.With(x: scaleThisFrame);
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    public void ClearTarget()
    {
        target = null;
    }
}
