using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

[CreateAssetMenu]
public class ObjectDatabaseSO : ScriptableObject
{
    public List<ObjectData> ObjectsData;
}

public enum Tag
{
    Favorite,
    Furniture,
    Food,
    Ride,
    Etc
}

// Name, Prefab, Price, Icon, Tags

[Serializable]
public class ObjectData
{
    [field:SerializeField]
    public string Name { get; private set; }
    
    [field:SerializeField]
    public GameObject Prefab { get; private set; }
    
    [field:SerializeField]
    public int Price { get; private set; }
    
    [field:SerializeField]
    public Sprite Icon { get; private set; }

    [field: SerializeField] 
    public List<Tag> Tags { get; private set; }

}
