using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerControl : MonoBehaviour
{
    public Camera camera = new Camera();
    public TMPro.TextMeshProUGUI text;
    private GameObject selected; // 현재 선택된 오브젝트
    private List<int> objectList; // 정렬을 위한 리스트

    void Start()
    {
        UIManager.Instance.SetMode((UIMode.Game));
        StartCoroutine(WaitForBookSortObjectNumbers());
    }

    IEnumerator WaitForBookSortObjectNumbers()
    {
        while (BookSort.objectNumbers == null || BookSort.objectNumbers.Count == 0)
        {
            yield return null;
        }
        objectList = new List<int>(BookSort.objectNumbers);
    }

    void Update()
    {
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100)) 
        {
            if (hit.collider.gameObject.tag != "Book") return;

            if (Input.GetKeyDown(KeyCode.F))
            {
                if (selected == null) 
                {
                    selected = hit.collider.gameObject;
                }
                else 
                {
                    SwapObjects(selected, hit.collider.gameObject);
                    if (objectList != null) CheckEnd();
                    selected = null; 
                }
            }
        }

    }

    private void SwapObjects(GameObject obj1, GameObject obj2)
    {
        try
        {
            // 이름에서 실제 숫자 값 추출
            int value1 = ExtractNumber(obj1.name);
            int value2 = ExtractNumber(obj2.name);

            // 배열에서 값의 위치를 찾음
            int index1 = objectList.FindIndex(x => x == value1);
            int index2 = objectList.FindIndex(x => x == value2);

            if (index1 == -1 || index2 == -1)
            {
                Debug.LogError("One or both objects' values not found in the list.");
                return;
            }

            // 실제 값을 교환
            objectList[index1] = value2;
            objectList[index2] = value1;

            // 오브젝트의 위치 교환
            Vector3 tempPosition = obj1.transform.position;
            obj1.transform.position = obj2.transform.position;
            obj2.transform.position = tempPosition;

            // Debug.Log("Array contents: " + string.Join(", ", objectList));

        }
        catch (System.Exception ex)
        {
            Debug.LogError("Error during swap: " + ex.Message);
        }
    }


    private int ExtractNumber(string objectName)
    {
        try
        {
            string[] parts = objectName.Split(' ');
            return int.Parse(parts[1]); // 이름에서 숫자 부분 추출
        }
        catch
        {
            Debug.LogError("Invalid object name format: " + objectName);
            return -1;
        }
    }

    private void CheckEnd()
    {
        Debug.Log("checking");
        for(int i = 0; i < objectList.Count - 1; i++)
        {
            if(objectList[i] > objectList[i + 1]) return;
        }
        Debug.Log("Game Finish");

        WaitForThreeSeconds();
        int prize = 100 * Random.Range(3, 6);
        GameManager.Instance.Money += prize;
        text.text = $"Finished{prize}!";
        UIManager.Instance.SetMode((UIMode.Normal));
        SceneManager.LoadScene("Scenes/Map/office/office");

    }
    
    IEnumerator WaitForThreeSeconds()
    {
        yield return new WaitForSeconds(3);
    }
    
    
}
