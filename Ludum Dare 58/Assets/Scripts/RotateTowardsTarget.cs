using UnityEngine;

public class RotateTowardsTarget : MonoBehaviour
{
    [SerializeField] private Transform target;

    private void Update()
    {
        transform.right = target.position - transform.position;
    }
}
