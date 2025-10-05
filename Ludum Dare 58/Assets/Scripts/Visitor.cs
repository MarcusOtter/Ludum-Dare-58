using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

[RequireComponent(typeof(NavMeshAgent))]
public class Visitor : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform detectionCone;
    [SerializeField] private Transform exclamationPoint;
    
    [Header("Path settings")]
    [SerializeField] private Transform path;
    [SerializeField] private Vector2 waitTimeRange = new(2, 5);
    
    [Header("Detection settings")]
    [SerializeField] private float timeUntilDetected = 2f;
    [SerializeField] private float delayUntilUndetected = 1f;
    [SerializeField] private AnimationCurve detectionConeScaleX;
    [SerializeField] private AnimationCurve detectionConeScaleZ;
    [SerializeField] private Gradient detectionConeColor;

    [Header("Events")]
    [SerializeField] private UnityEvent onDetected;
    
    private NavMeshAgent _agent;
    private Hand _detectedHand;
    private MeshRenderer _detectionConeRenderer;
    
    private Vector3[] _wayPoints;
    private int _currentWaypointIndex;
    private float _waitTimer;
    private float _timeCaught;
    
    private float DetectionMeter => Mathf.Clamp01(_timeCaught / timeUntilDetected);
    
    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _detectionConeRenderer = detectionCone.GetComponentInChildren<MeshRenderer>();
        UpdateDetectionBehavior();

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
        UpdateDetectionBehavior();
        UpdateWaypoints();
    }

    private void UpdateDetectionBehavior()
    {
        _detectionConeRenderer.material.color = detectionConeColor.Evaluate(DetectionMeter);
        var newScaleX = detectionConeScaleX.Evaluate(DetectionMeter);
        var newScaleZ = detectionConeScaleZ.Evaluate(DetectionMeter);
        detectionCone.localScale = detectionCone.localScale.With(x: newScaleX, z: newScaleZ);

        if (!_detectedHand)
        {
            return;
        }
        
        // Caught
        if (_timeCaught >= timeUntilDetected)
        {
            _agent.isStopped = true;
            _agent.transform.forward = (_detectedHand.player.position - transform.position).With(y: 0);
            onDetected?.Invoke();
            return;
        }
        
        if (_detectedHand.IsInJail)
        {
            _timeCaught = 0;
            return;
        }
        
        if (!_detectedHand.IsInJail)
        {
            _timeCaught += Time.deltaTime;
        }


    }
    
    private void UpdateWaypoints()
    {
        if (!HasReachedDestination())
        {
            return;
        }

        if (_waitTimer > 0)
        {
            _waitTimer -= Time.deltaTime;
            return;
        }
        
        _currentWaypointIndex = (_currentWaypointIndex + 1) % _wayPoints.Length;
        _agent.SetDestination(_wayPoints[_currentWaypointIndex]);
        _waitTimer = Random.Range(waitTimeRange.x, waitTimeRange.y);
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
    
    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent<Hand>(out var hand))
        {
            return;
        }
        
        _detectedHand = hand;
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.TryGetComponent<Hand>(out _))
        {
            return;
        }

        _detectedHand = null;
    }
}
