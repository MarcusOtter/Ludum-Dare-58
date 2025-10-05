using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class SoftlockFixer : MonoBehaviour
{
    [SerializeField] private Transform respawnPoint;
    
    private void OnCollisionEnter(Collision collision)
    {
        collision.transform.position = respawnPoint.position;
    }
}
