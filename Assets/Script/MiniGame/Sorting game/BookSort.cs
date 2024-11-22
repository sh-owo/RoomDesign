using System.Collections.Generic;
using UnityEngine;
using TMPro; // TextMeshPro를 사용하려면 추가

public class BookSort : MonoBehaviour
{
    public GameObject objectPrefab; 
    public GameObject booksParent; 
    public int objectCount;
    
    public static List<int> objectNumbers = new List<int>();
    private int placedIndex = 0;
    private const int rowSize = 10;

    void Start()
    {
        for (int index = 1; index <= objectCount; index++)
        {
            objectNumbers.Add(index);
        }

        ShuffleList(objectNumbers);

        Debug.Log("Shuffled Numbers: " + string.Join(", ", objectNumbers));

        foreach (int index in objectNumbers)
        {
            SpawnObject(index);
        }
    }
    
    private void ShuffleList<T>(List<T> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            T temp = list[i];
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }

    // 오브젝트를 생성하는 메서드
    public void SpawnObject(int index)
    {
        if (objectPrefab == null || booksParent == null)
        {
            Debug.LogError("Prefab 또는 Books Parent가 설정되지 않았습니다!");
            return;
        }
        Debug.Log("spawned");

        GameObject newObject = Instantiate(objectPrefab);
        newObject.name = "Book " + index;
        newObject.transform.SetParent(booksParent.transform);
        
        newObject.transform.position = new Vector3((float)(placedIndex % rowSize - (rowSize / 2)) / 2.5f, - placedIndex / rowSize + (objectCount / rowSize), 0);
        placedIndex++;

        UpdateObjectNumber(newObject, index);

        Renderer renderer = newObject.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material.color = Random.ColorHSV();
        }
    }

    // TMP 숫자 업데이트 함수
    private void UpdateObjectNumber(GameObject obj, int number)
    {
        TextMeshPro tmp = obj.GetComponentInChildren<TextMeshPro>();

        if (tmp != null)
        {
            tmp.text = number.ToString(); 
        }
        else
        {
            Debug.LogWarning("TextMeshPro 컴포넌트를 찾을 수 없습니다!");
        }
    }
}