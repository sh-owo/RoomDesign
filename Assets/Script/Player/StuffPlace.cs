using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StuffPlace : MonoBehaviour
{
    Vector3 pos;
    [SerializeField] private GameObject stuff;

    void Update()
    {
        MouseDirect();
        PutStuff();
        DeleteStuff();
    }
    
    private void PutStuff()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            Instantiate(stuff, pos, Quaternion.identity);
        }
    }
    
    private void DeleteStuff()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Debug.Log("Hit" + hit.collider.gameObject.tag);
                if (hit.collider.gameObject.CompareTag("Stuff"))
                {
                    Destroy(hit.collider.gameObject);
                }
            }
        }
    }

    private void MouseDirect()
    {
        
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            pos = hit.point;
            // Debug.Log("Pos" + hit.point);
        }
        
    }
}