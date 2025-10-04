using UnityEngine;

public class ScaleWithDistanceToTarget : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float scaleMultiplier = 0.25f;
    [SerializeField] private float maxScale = 2f;
    
    private void Update()
    {
        var newScale = Vector3.Magnitude(target.position - transform.position) * scaleMultiplier;
        transform.localScale = transform.localScale.With(x: Mathf.Min(newScale, maxScale));
    }
}
