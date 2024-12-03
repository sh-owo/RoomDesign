using UnityEngine;

public class InventoryItem
{
    public string Name { get; private set; }
    public string Tags { get; private set; }
    public int Count { get; private set; }
    public Sprite Icon { get; private set; }
    public GameObject Prefab { get; private set; }

    public InventoryItem(string name, string tags, int count, Sprite icon, GameObject prefab)
    {
        Name = name;
        Tags = tags;
        Count = count;
        Icon = icon;
        Prefab = prefab;
    }

    public void AddCount(int amount)
    {
        Count += amount;
    }

    public void RemoveCount(int amount)
    {
        Count = Mathf.Max(0, Count - amount);
    }
}