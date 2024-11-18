using UnityEngine;

public class BuildManager : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private GameObject objectToPlace; // 배치할 오브젝트 프리팹
    [SerializeField] private float placementHeight = 0f; // 배치 높이 오프셋
    [SerializeField] private Material validPlacementMaterial; // 배치 가능할 때 머티리얼
    [SerializeField] private Material invalidPlacementMaterial; // 배치 불가능할 때 머티리얼
    [SerializeField] private float maxPlacementDistance = 100f; // 최대 배치 거리
    [SerializeField] private LayerMask ignoreLayers; // 배치할 때 무시할 레이어

    private GameObject previewObject; // 미리보기 오브젝트
    private bool canPlace = false; // 현재 위치에 배치 가능 여부
    private Vector3 currentPosition; // 현재 배치 위치
    private Renderer previewRenderer;

    void Start()
    {
        // 미리보기 오브젝트 생성
        previewObject = Instantiate(objectToPlace);
        previewRenderer = previewObject.GetComponent<Renderer>();
        
        // 미리보기 오브젝트의 콜라이더 비활성화
        Collider[] colliders = previewObject.GetComponents<Collider>();
        foreach (Collider collider in colliders)
        {
            collider.enabled = false;
        }
    }

    void Update()
    {
        UpdatePlacementPreview();
        HandlePlacement();
    }

    void UpdatePlacementPreview()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // 모든 물체에 대해 레이캐스트 수행
        if (Physics.Raycast(ray, out hit, maxPlacementDistance, ~ignoreLayers))
        {
            currentPosition = hit.point + Vector3.up * placementHeight;
        }
        else
        {
            // 레이캐스트가 아무것도 맞추지 못했을 경우, 카메라로부터 일정 거리에 배치
            currentPosition = ray.GetPoint(maxPlacementDistance);
        }

        previewObject.transform.position = currentPosition;
        
        // 충돌 체크
        canPlace = !CheckCollision();
        
        // 배치 가능 여부에 따라 머티리얼 변경
        UpdatePreviewMaterials(canPlace ? validPlacementMaterial : invalidPlacementMaterial);
        previewObject.GetComponent<Collider>().enabled = false;
    }

    bool CheckCollision()
    {
        Collider previewCollider = previewObject.GetComponent<Collider>();
        Vector3 center = previewObject.transform.position;
        Vector3 size = previewCollider.bounds.size;
        Vector3 halfExtents = size / 2f;

        Vector3[] checkPoints = new Vector3[]
        {
            center,  
            center + new Vector3(halfExtents.x, 0, 0),  // 오른쪽
            center - new Vector3(halfExtents.x, 0, 0),  // 왼쪽
            center + new Vector3(0, halfExtents.y, 0),  // 위
            center - new Vector3(0, halfExtents.y, 0),  // 아래
            center + new Vector3(0, 0, halfExtents.z),  // 앞
            center - new Vector3(0, 0, halfExtents.z)   // 뒤
        };

        foreach (Vector3 checkPoint in checkPoints)
        {
            Collider[] colliders = Physics.OverlapBox(
                checkPoint,
                halfExtents * 0.8f,  
                previewObject.transform.rotation
            );

            foreach (Collider collider in colliders)
            {
                if (collider.gameObject != previewObject && 
                    !collider.transform.IsChildOf(previewObject.transform) &&
                    !previewObject.transform.IsChildOf(collider.transform))
                {
                    return true;
                }
            }
        }

        return false;
    }
    
    private void UpdatePreviewMaterials(Material material)
    {
        // 자신의 Renderer가 있다면 변경
        Renderer renderer = previewObject.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material = material;
        }

        // 모든 자식 오브젝트의 Renderer 찾아서 Material 변경
        Renderer[] childRenderers = previewObject.GetComponentsInChildren<Renderer>();
        foreach (Renderer childRenderer in childRenderers)
        {
            childRenderer.material = material;
        }
    }

    void HandlePlacement()
    {
        Debug.Log(canPlace);
        if (Input.GetKeyDown(KeyCode.F) && canPlace)
        {
            Debug.Log("Placed object");
            // 실제 오브젝트 배치
            GameObject placedObject = Instantiate(objectToPlace, currentPosition, previewObject.transform.rotation);
            placedObject.GetComponent<Collider>().enabled = true;
            placedObject.layer = LayerMask.NameToLayer("Objects");
            
            //TODO: 이거 rigidbody를 멍추게 하던가 아님 다르게 하던가 해야함
            Rigidbody[] rigidbodies = placedObject.GetComponentsInChildren<Rigidbody>();
            foreach (Rigidbody rb in rigidbodies)
            {
                rb.isKinematic = true;
            }
        
            // 본체에 Rigidbody가 없는 경우 추가
            if (placedObject.GetComponent<Rigidbody>() == null)
            {
                Rigidbody rb = placedObject.AddComponent<Rigidbody>();
                rb.isKinematic = true;
            }
            
        }
    }

    void OnDestroy()
    {
        if (previewObject != null)
        {
            Destroy(previewObject);
        }
    }

    // 회전 기능 추가 (선택적)
    void HandleRotation()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            previewObject.transform.Rotate(Vector3.up, 90f);
        }
    }
}