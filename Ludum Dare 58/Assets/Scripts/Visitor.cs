using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Visitor : MonoBehaviour
{
    public ItemType HeldItem { get; private set; } = ItemType.None;
    
    [Header("References")]
    [SerializeField] private Transform detectionCone;
    [SerializeField] private Transform exclamationPoint;
    [SerializeField] private Transform pocketSlot;
    
    [Header("Path settings")]
    [SerializeField] private Transform path;
    [SerializeField] private Vector2 waitTimeRange = new(2, 5);
    [SerializeField] private bool pickRandomWaypoint;
    [SerializeField] private Vector2Int agentPriorityRange = new (20, 100); 
    
    [Header("Detection settings")]
    [SerializeField] private float timeUntilDetected = 2f;
    [SerializeField] private float timeUntilResumeAfterDetected = 3f;
    [SerializeField] private float detectionMultiplierIfHoldingItem = 2f;
    [SerializeField] private float detectionCooldownMultiplier = 0.5f;
    [SerializeField] private AnimationCurve detectionConeScaleX;
    [SerializeField] private AnimationCurve detectionConeScaleZ;
    [SerializeField] private Gradient detectionConeColor;
    
    private NavMeshAgent _agent;
    private MeshRenderer _detectionConeRenderer;
    private Player _player;
    private Hand _hand;
    private HashSet<PlayerPart> _detectedParts = new();
    
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
        _hand = FindFirstObjectByType<Hand>();
        _detectionConeRenderer = detectionCone.GetComponentInChildren<MeshRenderer>();
        
        _agent.avoidancePriority = Random.Range(agentPriorityRange.x, agentPriorityRange.y);
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

        if (pickRandomWaypoint)
        {
            transform.position = _wayPoints[Random.Range(0, _wayPoints.Length)];   
        }
        else
        {
            transform.position = _wayPoints[0];
        }
    }
    
    private void Update()
    {
        UpdateDetectionBehavior();
        UpdateWaypoints();
    }

    public void SpawnAndHoldItem(Item item)
    {
        if (HeldItem != ItemType.None)
        {
            return;
        }

        if (HeldItem == item.type)
        {
            return;
        }

        var spawnedItem = Instantiate(item.prefab);
        spawnedItem.PickUp(pocketSlot);
        HeldItem = item.type;
    }

    public void NotifyItemWasStolen()
    {
        HeldItem = ItemType.None;
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
        
        if (!_detectedParts.Any() || _detectedParts.All(part => part.IsSafe))
        {
            _timeSeen = Mathf.Max(_timeSeen - Time.deltaTime * detectionCooldownMultiplier, 0);
            return;
        }
        
        if (_detectedParts.Any(part => !part.IsSafe))
        {
            _timeSeen += _hand.IsHoldingItem ? Time.deltaTime * detectionMultiplierIfHoldingItem : Time.deltaTime;
        }
        
        if (_timeSeen >= timeUntilDetected)
        {
            _agent.isStopped = true;
            _agent.transform.forward = (_player.transform.position - transform.position).With(y: 0);
            _hand.DropItem();
            _player.SetStunned(true);
            _player.TakeDamage(1);
            _player.Respawn();
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

        if (pickRandomWaypoint)
        {
            _currentWaypointIndex = Random.Range(0, _wayPoints.Length);
        }
        else
        {
            _currentWaypointIndex = (_currentWaypointIndex + 1) % _wayPoints.Length;
        }
        
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
    
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var playerPart = other.GetComponent<PlayerPart>();
            if (playerPart)
            {
                _detectedParts.Add(playerPart);
            }

            // print("detectedParts: " + string.Join(", ", _detectedParts.Select(x => $"{x.name} ({(x.IsSafe ? "safe" : "not safe")})")));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var playerPart = other.GetComponent<PlayerPart>();
            if (playerPart)
            {
                _detectedParts.Remove(playerPart);
            }
            
            // print("detectedParts: " + string.Join(", ", _detectedParts.Select(x => $"{x.name} ({(x.IsSafe ? "safe" : "not safe")})")));
        }
    }
}
