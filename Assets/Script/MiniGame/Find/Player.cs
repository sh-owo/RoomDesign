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
       
        Vector3 newPosition = FindGameManager.Instance.StartPos;
        newPosition.x += 1.5f;
        transform.position = newPosition;
        transform.rotation = Quaternion.Euler(0f, UnityEngine.Random.Range(0f, 360f), 0f);
    }

    void Update()
    {
        UpdateCamera();
        if(!FindGameManager.Instance.isGameStart || FindGameManager.Instance.isGameEnd) {Debug.Log("entry");return;}
        Debug.Log("doing");
        // 입력 처리
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        // 이동 및 회전
        Move(y * 0.8f);
        Rotation(x);

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
    
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Target"))
        {
            FindGameManager.Instance.isPlayerWon = true;
            FindGameManager.Instance.isGameEnd = true;
            Debug.Log("Game End!");
        }
    }
}