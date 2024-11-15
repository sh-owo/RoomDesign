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
        previewRenderer.material = canPlace ? validPlacementMaterial : invalidPlacementMaterial;
        previewObject.GetComponent<Collider>().enabled = false;
    }

    bool CheckCollision()
    {
        // 오브젝트의 콜라이더 크기를 기반으로 충돌 체크
        Collider previewCollider = previewObject.GetComponent<Collider>();
        Vector3 center = previewObject.transform.position;
        Vector3 halfExtents = previewCollider.bounds.extents;

        // 모든 레이어의 오브젝트와 충돌 검사
        Collider[] colliders = Physics.OverlapBox(center, halfExtents, previewObject.transform.rotation);
        return colliders.Length > 0;
    }

    void HandlePlacement()
    {
        if (Input.GetKeyDown(KeyCode.F) && canPlace)
        {
            // 실제 오브젝트 배치
            GameObject placedObject = Instantiate(objectToPlace, currentPosition, previewObject.transform.rotation);
            placedObject.GetComponent<Collider>().enabled = true;
            
            
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