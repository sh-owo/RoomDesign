using UnityEngine;

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

    private GameObject previewObject;
    private bool canPlace = false;
    private Vector3 currentPosition;
    private Renderer previewRenderer;
    private float lastMouseX;
    private bool firstUpdateAfterModeChange = false; // 모드 변경 직후 첫 업데이트 체크
    
    private enum BuildMode
    {
        Placement,
        Rotation
    }
    
    private BuildMode currentMode = BuildMode.Placement;

    void Start()
    {
        previewObject = Instantiate(objectToPlace);
        previewRenderer = previewObject.GetComponent<Renderer>();
        DisablePhysics(previewObject);
    }

    void DisablePhysics(GameObject obj)
    {
        Collider[] colliders = obj.GetComponents<Collider>();
        foreach (Collider collider in colliders)
        {
            collider.isTrigger = true;
        }
        
        foreach (Collider collider in obj.GetComponentsInChildren<Collider>())
        {
            collider.isTrigger = true;
        }

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
        // ESC로 회전 모드 빠져나오기
        if (Input.GetKeyDown(KeyCode.Escape) && currentMode == BuildMode.Rotation)
        {
            currentMode = BuildMode.Placement;
            firstUpdateAfterModeChange = true; // 모드 변경 직후 플래그 설정
            return; // 이번 프레임에서는 더 이상 처리하지 않음
        }
        
        // R키로 회전 모드 전환
        if (Input.GetKeyDown(KeyCode.R))
        {
            currentMode = BuildMode.Rotation;
            lastMouseX = Input.mousePosition.x;
            return; // 이번 프레임에서는 더 이상 처리하지 않음
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

        // 충돌 체크 및 머티리얼 업데이트는 항상 수행
        canPlace = !CheckCollision();
        UpdatePreviewMaterials(canPlace ? validPlacementMaterial : invalidPlacementMaterial);
    }

    void UpdatePosition()
    {
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
        float mouseDelta = Input.mousePosition.x - lastMouseX;
        float rotationAmount = mouseDelta * rotationSpeed;
        
        currentPosition = previewObject.transform.position;
        previewObject.transform.Rotate(Vector3.up, rotationAmount);
        lastMouseX = Input.mousePosition.x;
    }

    bool CheckCollision()
    {
        Collider[] previewColliders = previewObject.GetComponentsInChildren<Collider>();
        foreach (Collider previewCollider in previewColliders)
        {
            Vector3 center = previewCollider.bounds.center;
            Vector3 halfExtents = previewCollider.bounds.extents * 0.8f;
            
            Collider[] hitColliders = Physics.OverlapBox(
                center,
                halfExtents,
                previewObject.transform.rotation,
                ~ignoreLayers
            );

            foreach (Collider hitCollider in hitColliders)
            {
                if (hitCollider.gameObject != previewObject && 
                    !hitCollider.transform.IsChildOf(previewObject.transform) &&
                    !previewObject.transform.IsChildOf(hitCollider.transform))
                {
                    return true;
                }
            }
        }
        return false;
    }
    
    private void UpdatePreviewMaterials(Material material)
    {
        Renderer renderer = previewObject.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material = material;
        }

        Renderer[] childRenderers = previewObject.GetComponentsInChildren<Renderer>();
        foreach (Renderer childRenderer in childRenderers)
        {
            childRenderer.material = material;
        }
    }

    void HandlePlacement()
    {
        if (Input.GetKeyDown(KeyCode.F) && canPlace)
        {
            GameObject placedObject = Instantiate(objectToPlace, currentPosition, previewObject.transform.rotation);
            placedObject.GetComponent<Collider>().enabled = true;
            placedObject.layer = LayerMask.NameToLayer("Objects");
            
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
        
    }

    void OnDestroy()
    {
        if (previewObject != null)
        {
            Destroy(previewObject);
        }
    }
}