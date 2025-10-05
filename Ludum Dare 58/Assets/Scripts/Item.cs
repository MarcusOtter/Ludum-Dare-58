using UnityEngine;

[System.Serializable]
public enum ItemType
{
    None,
    Banana,
}

[CreateAssetMenu(fileName = "Item", menuName = "Scriptable Objects/Item")]
public class Item : ScriptableObject
{
    public ItemType type;
    public int spawnWeight;
    public Pickable prefab;
    public Texture icon;
}
