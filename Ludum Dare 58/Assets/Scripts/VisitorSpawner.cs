using UnityEngine;

public class VisitorSpawner : MonoBehaviour
{
    [SerializeField] private float minimumSpawnDelay = 2f;
    [SerializeField] private AnimationCurve visitorCountTarget;

    private float _lastSpawnTime = float.MinValue;
    private int _spawnedVisitors;
    private int _maxSpawnedVisitors;
    
    private void Awake()
    {
        _maxSpawnedVisitors = transform.childCount;
    }
    
    private void Update()
    {
        if (_spawnedVisitors >= _maxSpawnedVisitors)
        {
            return;
        }
        
        var targetAmount = Mathf.FloorToInt(visitorCountTarget.Evaluate(Player.TimePlayed));
        if (_spawnedVisitors >= targetAmount)
        {
            return;
        }
        
        if (Time.time < _lastSpawnTime + minimumSpawnDelay)
        {
            return;
        }

        transform.GetChild(_spawnedVisitors).gameObject.SetActive(true);
        _lastSpawnTime = Time.time;
        _spawnedVisitors++;
    }
}
