using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceDestry : MonoBehaviour
{
    private Rigidbody rb;
    private List<GameObject> stufflist;
    private bool isInitialized = false;

    void Start()
    {
        // Start에서 초기화를 시도하고, 실패하면 코루틴으로 재시도
        if (!InitializeStuffList())
        {
            StartCoroutine(WaitForGameManager());
        }
    }

    private bool InitializeStuffList()
    {
        if (GameManager.Instance != null && GameManager.Instance.stuffList != null)
        {
            stufflist = GameManager.Instance.stuffList;
            isInitialized = true;
            Debug.Log("StuffList initialized with " + stufflist.Count + " items");
            return true;
        }
        return false;
    }

    private IEnumerator WaitForGameManager()
    {
        while (!isInitialized)
        {
            if (InitializeStuffList())
            {
                break;
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    void FixedUpdate()
    {
        if (isInitialized)
        {
            PlaceStuff();
            RemoveStuff();
        }
    }

    private void PlaceStuff()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            Vector3 pos;
        
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit)) 
            {
                pos = hit.point;
                int selectedIndex = 0; //임시설정
                //TODO: 인덱스 어떻게든 입력받도록 수정해야함
                GameObject stuff = Instantiate(stufflist[selectedIndex], pos, Quaternion.identity);
            }
        }
    }

    private void RemoveStuff()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.CompareTag("Stuff"))
                {
                    Destroy(hit.transform.gameObject);
                }
            }
        }
    }
}