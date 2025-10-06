using UnityEngine;

[RequireComponent(typeof(Collider))]
public class PlayerPart : MonoBehaviour
{
    public bool IsSafe { get; private set; } = true;
    
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Safe"))
        {
            IsSafe = true;
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Safe"))
        {
            IsSafe = false;
        }
    }
}
