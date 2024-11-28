using UnityEngine;
using System.Collections.Generic;

public class BuildManager : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private GameObject objectToPlace;
    [SerializeField] private float placementHeight = 0f;
    [SerializeField] private Material validPlacementMaterial;
    [SerializeField] private Material invalidPlacementMaterial;
    [SerializeField] private float maxPlacementDistance = 100f;
    [SerializeField] private LayerMask ignoreLayers;
    [SerializeField] private float rotationSpeed = 2.0f;
    [SerializeField] private KeyCode placeKey = KeyCode.F;
    [SerializeField] private KeyCode removeKey = KeyCode.V;
    [SerializeField] private KeyCode rotateKey = KeyCode.R;

    private GameObject previewObject;
    private bool canPlace = false;
    private Vector3 currentPosition;
    private Renderer previewRenderer;
    private float lastMouseX;
    private bool firstUpdateAfterModeChange = false;
    
    private enum BuildMode
    {
        Placement,
        Rotation
    }
    
    private BuildMode currentMode = BuildMode.Placement;

    void Start()
    {
        if (GameManager.Instance != null && GameManager.Instance.selectedPrefab != null)
        {
            previewObject = Instantiate(GameManager.Instance.selectedPrefab);
        }
        else if (objectToPlace != null)
        {
            previewObject = Instantiate(objectToPlace);
        }
        else
        {
            Debug.LogError("No object to place assigned!");
            enabled = false;
            return;
        }

        previewRenderer = previewObject.GetComponent<Renderer>();
        if (previewRenderer == null)
        {
            Debug.LogWarning("No renderer found on preview object!");
        }
        
        DisablePhysics(previewObject);
        
        // 초기 머티리얼 설정
        UpdatePreviewMaterials(validPlacementMaterial);
    }

    void DisablePhysics(GameObject obj)
    {
        if (obj == null) return;

        // 메인 오브젝트의 물리 컴포넌트 비활성화
        Collider[] colliders = obj.GetComponents<Collider>();
        foreach (Collider collider in colliders)
        {
            collider.isTrigger = true;
        }
        
        // 자식 오브젝트들의 물리 컴포넌트 비활성화
        foreach (Collider collider in obj.GetComponentsInChildren<Collider>())
        {
            collider.isTrigger = true;
        }

        // Rigidbody 제거
        Rigidbody[] rigidBodies = obj.GetComponentsInChildren<Rigidbody>(true);
        foreach (Rigidbody rb in rigidBodies)
        {
            Destroy(rb);
        }

        Rigidbody mainRb = obj.GetComponent<Rigidbody>();
        if (mainRb != null)
        {
            Destroy(mainRb);
        }
    }

    void Update()
    {
        if (GameManager.Instance == null) return;

        // 선택된 프리팹 변경 감지
        if (GameManager.Instance.selectedPrefab != null && 
            (previewObject == null || previewObject.name != GameManager.Instance.selectedPrefab.name + "(Clone)"))
        {
            if (previewObject != null)
            {
                Destroy(previewObject);
            }
            previewObject = Instantiate(GameManager.Instance.selectedPrefab);
            previewRenderer = previewObject.GetComponent<Renderer>();
            DisablePhysics(previewObject);
            UpdatePreviewMaterials(validPlacementMaterial);
        }

        // ESC로 회전 모드 빠져나오기
        if (Input.GetKeyDown(KeyCode.Escape) && currentMode == BuildMode.Rotation)
        {
            currentMode = BuildMode.Placement;
            firstUpdateAfterModeChange = true;
            return;
        }
        
        // 회전 모드 전환
        if (Input.GetKeyDown(rotateKey))
        {
            currentMode = BuildMode.Rotation;
            lastMouseX = Input.mousePosition.x;
            return;
        }

        // 오브젝트 제거
        if (Input.GetKeyDown(KeyCode.V))
        {
            Debug.Log("Remove object");
            RemoveObject();
        }

        // 배치 모드이고 첫 업데이트가 아닐 때만 위치 업데이트
        if (currentMode == BuildMode.Placement && !firstUpdateAfterModeChange)
        {
            UpdatePosition();
        }

        // 첫 업데이트 플래그 리셋
        firstUpdateAfterModeChange = false;

        // 모드에 따른 처리
        switch (currentMode)
        {
            case BuildMode.Placement:
                HandlePlacement();
                break;
            case BuildMode.Rotation:
                HandleRotation();
                HandlePlacement();
                break;
        }

        // 충돌 체크 및 머티리얼 업데이트
        canPlace = !CheckCollision();
        UpdatePreviewMaterials(canPlace ? validPlacementMaterial : invalidPlacementMaterial);
    }

    void UpdatePosition()
    {
        if (previewObject == null) return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, maxPlacementDistance, ~ignoreLayers))
        {
            currentPosition = hit.point + Vector3.up * placementHeight;
            previewObject.transform.position = currentPosition;
        }
        else
        {
            currentPosition = ray.GetPoint(maxPlacementDistance);
            previewObject.transform.position = currentPosition;
        }
    }

    void HandleRotation()
    {
        if (previewObject == null) return;

        float mouseDelta = Input.mousePosition.x - lastMouseX;
        float rotationAmount = mouseDelta * rotationSpeed;
        
        currentPosition = previewObject.transform.position;
        previewObject.transform.Rotate(Vector3.up, rotationAmount);
        lastMouseX = Input.mousePosition.x;
    }

private List<GizmoData> gizmoDataList = new List<GizmoData>();

private struct GizmoData
{
    public Vector3 center;
    public Vector3 size;
    public Quaternion rotation;
    public bool hasCollision;

    public GizmoData(Vector3 center, Vector3 size, Quaternion rotation, bool hasCollision)
    {
        this.center = center;
        this.size = size;
        this.rotation = rotation;
        this.hasCollision = hasCollision;
    }
}
bool CheckCollision()
{
    if (previewObject == null) return true;

    gizmoDataList.Clear();
    bool hasAnyCollision = false;

    Collider[] previewColliders = previewObject.GetComponentsInChildren<Collider>();
    foreach (Collider previewCollider in previewColliders)
    {
        // BoxCollider인 경우
        BoxCollider boxCollider = previewCollider as BoxCollider;
        if (boxCollider != null)
        {
            // 로컬 크기와 중심점을 월드 좌표로 변환
            Vector3 worldCenter = previewCollider.transform.TransformPoint(boxCollider.center);
            Vector3 worldSize = Vector3.Scale(boxCollider.size, previewCollider.transform.lossyScale) * 0.8f;
            
            Collider[] hitColliders = Physics.OverlapBox(
                worldCenter,
                worldSize * 0.5f,  // OverlapBox는 half-extents를 사용하므로 0.5를 곱함
                previewCollider.transform.rotation,
                ~ignoreLayers
            );

            bool currentCollision = false;
            foreach (Collider hitCollider in hitColliders)
            {
                if (hitCollider.gameObject != previewObject && 
                    !hitCollider.transform.IsChildOf(previewObject.transform) &&
                    !previewObject.transform.IsChildOf(hitCollider.transform))
                {
                    currentCollision = true;
                    hasAnyCollision = true;
                    break;
                }
            }

            gizmoDataList.Add(new GizmoData(
                worldCenter,
                worldSize,
                previewCollider.transform.rotation,
                currentCollision
            ));
        }
    }
    return hasAnyCollision;
}

private void OnDrawGizmos()
{
    if (!Application.isPlaying) return;
    
    foreach (var gizmoData in gizmoDataList)
    {
        // 충돌이 있으면 빨간색, 없으면 초록색으로 표시
        Gizmos.color = gizmoData.hasCollision ? 
            new Color(1, 0, 0, 0.5f) : // 반투명 빨간색
            new Color(0, 1, 0, 0.5f);  // 반투명 초록색

        Matrix4x4 originalMatrix = Gizmos.matrix;
        Gizmos.matrix = Matrix4x4.TRS(gizmoData.center, gizmoData.rotation, Vector3.one);
        
        // 와이어프레임 박스
        Gizmos.DrawWireCube(Vector3.zero, gizmoData.size);
        
        // 반투명한 실선 박스
        Gizmos.color = new Color(Gizmos.color.r, Gizmos.color.g, Gizmos.color.b, 0.2f);
        Gizmos.DrawCube(Vector3.zero, gizmoData.size);
        
        Gizmos.matrix = originalMatrix;
    }
}
    
    private void UpdatePreviewMaterials(Material material)
    {
        if (material == null || previewObject == null) return;

        // 메인 렌더러 업데이트
        Renderer renderer = previewObject.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material = material;
        }

        // 자식 오브젝트의 렌더러들 업데이트
        Renderer[] childRenderers = previewObject.GetComponentsInChildren<Renderer>();
        foreach (Renderer childRenderer in childRenderers)
        {
            childRenderer.material = material;
        }
    }

    void HandlePlacement()
    {
        if (Input.GetKeyDown(placeKey) && canPlace)
        {
            GameObject prefabToPlace = GameManager.Instance.selectedPrefab ?? objectToPlace;
            GameObject placedObject = Instantiate(prefabToPlace, currentPosition, previewObject.transform.rotation);
            
            // 물리 컴포넌트 설정
            Collider mainCollider = placedObject.GetComponent<Collider>();
            if (mainCollider != null)
            {
                mainCollider.isTrigger = false;
                mainCollider.enabled = true;
            }

            // 자식 콜라이더들 설정
            foreach (Collider childCollider in placedObject.GetComponentsInChildren<Collider>())
            {
                childCollider.isTrigger = false;
                childCollider.enabled = true;
            }
            
            // 레이어 설정
            placedObject.layer = LayerMask.NameToLayer("Objects");
            foreach (Transform child in placedObject.GetComponentsInChildren<Transform>())
            {
                child.gameObject.layer = LayerMask.NameToLayer("Objects");
            }
            
            // Rigidbody 설정
            Rigidbody[] rigidbodies = placedObject.GetComponentsInChildren<Rigidbody>();
            foreach (Rigidbody rb in rigidbodies)
            {
                rb.isKinematic = true;
            }
        
            if (placedObject.GetComponent<Rigidbody>() == null)
            {
                Rigidbody rb = placedObject.AddComponent<Rigidbody>();
                rb.isKinematic = true;
            }
        }
    }

    void RemoveObject()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        
        if (Physics.Raycast(ray, out hit, maxPlacementDistance))
        {
            GameObject hitObject = hit.collider.gameObject;
            Debug.Log("hit");
            if (hitObject.tag == "Stuff")
            {
                // 부모 오브젝트 찾기
                Transform parent = hitObject.transform;
                while (parent.parent != null && 
                       parent.parent.gameObject.layer == LayerMask.NameToLayer("Objects"))
                {
                    parent = parent.parent;
                }
                
                Destroy(parent.gameObject);
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
}