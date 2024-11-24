using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StuffPin : MonoBehaviour
{
    private List<int> currentList;
    
    void Start()
    {
        currentList = new List<int>(BookSort.objectNumbers);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
