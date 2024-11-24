using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Camera camera;
    public GameObject camPos;
    private Rigidbody rb;

    private float speed;
    private float rotationSpeed;

    void Start()
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

    void Update()
    {
        // 입력 처리
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        // 이동 및 회전
        Move(y);
        Rotation(x);

        // 카메라 위치 업데이트
        UpdateCamera();
    }

    void Move(float x)
    {
        Vector3 movement = transform.forward* 1.5f * x * speed * Time.deltaTime;
        rb.AddForce(movement, ForceMode.VelocityChange);
    }

    void Rotation(float y)
    {
        float rotation = y * rotationSpeed * Time.deltaTime;
        transform.Rotate(0, rotation, 0);
    }

    void UpdateCamera()
    {
        camera.transform.position = camPos.transform.position;
        camera.transform.rotation = camPos.transform.rotation;
    }
}