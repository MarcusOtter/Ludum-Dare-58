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
    [SerializeField] private float timeUntilResumeAfterDetected = 3f;
    [SerializeField] private float detectionMultiplierIfHoldingItem = 2f;
    [SerializeField] private float detectionCooldownMultiplier = 0.5f;
    [SerializeField] private AnimationCurve detectionConeScaleX;
    [SerializeField] private AnimationCurve detectionConeScaleZ;
    [SerializeField] private Gradient detectionConeColor;

    [Header("Events")]
    [SerializeField] private UnityEvent onDetected;
    
    private NavMeshAgent _agent;
    private Hand _seenHand;
    private MeshRenderer _detectionConeRenderer;
    private Player _player;
    
    private Vector3[] _wayPoints;
    private int _currentWaypointIndex;
    private float _waitTimer;
    private float _timeSeen;
    private bool _isDetected;
    
    private float DetectionMeter => Mathf.Clamp01(_timeSeen / timeUntilDetected);
    
    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _player = FindFirstObjectByType<Player>();
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

        if (_isDetected)
        {
            return;
        }
        
        if (!_seenHand || _seenHand.IsInJail)
        {
            _timeSeen = Mathf.Max(_timeSeen - Time.deltaTime * detectionCooldownMultiplier, 0);
            return;
        }
        
        if (!_seenHand.IsInJail)
        {
            _timeSeen += _seenHand.IsHoldingItem ? Time.deltaTime * detectionMultiplierIfHoldingItem : Time.deltaTime;
        }
        
        if (_timeSeen >= timeUntilDetected)
        {
            _agent.isStopped = true;
            _agent.transform.forward = (_player.transform.position - transform.position).With(y: 0);
            _seenHand.DropItem();
            _player.SetStunned(true);
            onDetected?.Invoke();
            _isDetected = true;
            this.SetTimeout(timeUntilResumeAfterDetected, () =>
            {
                _isDetected = false;
                _agent.isStopped = false;
                _timeSeen = 0;
                _player.SetStunned(false);
            });
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
        
        _seenHand = hand;
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.TryGetComponent<Hand>(out _))
        {
            return;
        }

        _seenHand = null;
    }
}
