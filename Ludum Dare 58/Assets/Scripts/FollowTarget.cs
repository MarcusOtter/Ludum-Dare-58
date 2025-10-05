using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 positionOffset = Vector3.zero;
    [SerializeField] private bool followPosition = true;
    [SerializeField] private bool followRotation;
    [SerializeField] private bool followScale;

    private void Update()
    {
        if (!target)
        {
            return;
        }

        if (followPosition)
        {
            transform.position = target.position + positionOffset;
        }

        if (followRotation)
        {
            transform.rotation = target.rotation;
        }
        
        if (followScale)
        {
            transform.localScale = target.localScale;
        }
    }
}
