using System.Linq;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    [SerializeField] private Item[] itemsToSpawn;
    [SerializeField] private float spawnInterval = 10f;
    
    private Visitor[] _visitors;
    private Camera _camera;

    private float _lastSpawnTime;
    
    private void Awake()
    {
        _visitors = FindObjectsByType<Visitor>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        _camera = Camera.main;
    }

    private void Update()
    {
        var timeSinceLastSpawn = Time.time - _lastSpawnTime;
        if (timeSinceLastSpawn > spawnInterval)
        {
            SpawnRandomItem();
        }
    }

    // ReSharper disable Unity.PerformanceAnalysis
    private void SpawnRandomItem()
    {
        var offscreenVisitor = _visitors.FirstOrDefault(visitor => visitor.isActiveAndEnabled && visitor.HeldItem == ItemType.None && !_camera.IsObjectVisible(visitor.transform));
        if (!offscreenVisitor)
        {
            return;
        }

        var item = itemsToSpawn.PickWeighted(item => item.spawnWeight);
        offscreenVisitor.SpawnAndHoldItem(item);
            
        _lastSpawnTime = Time.time;
    }
}
