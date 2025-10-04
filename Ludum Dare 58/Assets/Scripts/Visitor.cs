using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

[RequireComponent(typeof(NavMeshAgent))]
public class Visitor : MonoBehaviour
{
    [SerializeField] private Transform path;
    [SerializeField] private Vector2 waitTimeMinMax = new(2, 5);

    private NavMeshAgent _agent;
    
    private Vector3[] _wayPoints;
    private int _currentWaypointIndex;
    
    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();

        if (!path)
        {
            return;
        }
        
        _wayPoints = new Vector3[path.childCount];
        var i = 0;
        foreach (Transform child in path)
        {
            _wayPoints[i] = child.position;
            i++;
        }
        
        transform.position = _wayPoints[0];
    }
    
    private void Update()
    {
        if (!HasReachedDestination())
        {
            return;
        }
        
        _currentWaypointIndex = (_currentWaypointIndex + 1) % _wayPoints.Length;
        _agent.SetDestination(_wayPoints[_currentWaypointIndex]);
    }

    private bool HasReachedDestination()
    {
        if (_agent.pathPending)
        {
            return false;
        }

        if (_agent.remainingDistance > _agent.stoppingDistance)
        {
            return false;
        }

        if (_agent.hasPath)
        {
            return false;
        }

        if (_agent.velocity.sqrMagnitude > 0)
        {
            return false;
        }

        return true;
    }
}
