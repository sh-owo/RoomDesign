using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ObjectDatabaseSO : ScriptableObject
{
    public List<ObjectData> ObjectsData;
}

[Serializable]
public class ObjectData
{
    [field:SerializeField]
    public string Name { get; private set; }
    
    [field:SerializeField]
    public GameObject Prefab { get; private set; }
    
    [field:SerializeField]
    public int ID { get; private set; }
    
    [field:SerializeField]
    public float yOffset { get; private set; }
    
    [field:SerializeField]
    public int Price { get; private set; }
    
    [field:SerializeField]
    public List<String> Tags { get; private set; }
    
}
