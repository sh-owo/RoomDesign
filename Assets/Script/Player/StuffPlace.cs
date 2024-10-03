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
        if (Input.GetKeyDown(KeyCode.F))
        {
            Instantiate(stuff, pos, Quaternion.identity);
        }
    }

    private void MouseDirect()
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                pos = hit.point;
                Debug.Log("Pos" + hit.point);
            }
        }
    }
    private void OnDrawGizmos()
    {
        if (pos != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(Camera.main.transform.position, pos);
        }
    }
}