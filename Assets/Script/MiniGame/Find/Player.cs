using UnityEngine;

public class Player : MonoBehaviour
{
    public Camera camera;
    public GameObject camPos;
    private Rigidbody rb;

    private float speed;
    private float rotationSpeed;

    private void Start()
    {
        if (camera == null) { camera = Camera.main; }
        rb = GetComponent<Rigidbody>();

        speed = FindGameManager.Instance.speed;
        rotationSpeed = FindGameManager.Instance.rotationSpeed;
        
        int random = UnityEngine.Random.Range(0, FindGameManager.Instance.spawnPos.Count - 1);
        Vector3 newPosition = FindGameManager.Instance.spawnPos[random].transform.position;
        newPosition.x += 1.5f;
        transform.position = newPosition;
        transform.rotation = Quaternion.Euler(0f, UnityEngine.Random.Range(0f, 360f), 0f);
    }

    private void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        Move(y);
        Rotate(x);

        UpdateCamera();
    }

    private void Move(float x)
    {
        Vector3 movement = transform.forward * 1.5f * x * speed * Time.deltaTime;
        rb.AddForce(movement, ForceMode.VelocityChange);
    }

    private void Rotate(float y)
    {
        float rotation = y * rotationSpeed * Time.deltaTime;
        transform.Rotate(0, rotation, 0);
    }

    private void UpdateCamera()
    {
        camera.transform.position = camPos.transform.position;
        camera.transform.rotation = camPos.transform.rotation;
    }
}