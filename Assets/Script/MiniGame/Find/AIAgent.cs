using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class AIAgent : Agent
{
    private Rigidbody rb;
    private Vector3 startPos;
    private Vector3 targetPos;
    [SerializeField] private GameObject target;

    public void Start()
    {
        rb = GetComponent<Rigidbody>();
        startPos = transform.position;
        targetPos = target.transform.position;
    }

    public override void OnEpisodeBegin()
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        List<GameObject> posList = FindGameManager.Instance.spawnPos;
        int random = UnityEngine.Random.Range(0, posList.Count - 1);
        transform.position = posList[random].transform.position;
        transform.rotation = Quaternion.Euler(0f, UnityEngine.Random.Range(0f, 360f), 0f);

        targetPos = posList[UnityEngine.Random.Range(0, posList.Count - 1)].transform.position;
        target.transform.position = new Vector3(UnityEngine.Random.Range(targetPos.x - 11, targetPos.x + 11), 1f, UnityEngine.Random.Range(targetPos.z - 11, targetPos.z + 11));
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.InverseTransformDirection(rb.velocity));
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        int inputs = actions.DiscreteActions[0];
        Movement(inputs);

        AddReward(-1/MaxStep);
    }

    public void Movement(int inputs)
    {
        Vector3 move = Vector3.zero;
        Vector3 rotation = Vector3.zero;
        float speed = FindGameManager.Instance.speed;
        float rotationSpeed = FindGameManager.Instance.rotationSpeed;

        switch (inputs)
        {
            case 1: move = transform.forward * speed * Time.deltaTime; break;
            case 2: move = -transform.forward * speed * Time.deltaTime; break;
            case 3: rotation = transform.up * rotationSpeed * Time.deltaTime; break;
            case 4: rotation = -transform.up * rotationSpeed * Time.deltaTime; break;
        }

        rb.AddForce(move * 1.5f, ForceMode.VelocityChange);
        transform.Rotate(rotation);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Wall"))
        {
            // EndEpisode();
        }
        if (other.gameObject.CompareTag("Target"))
        {
            Debug.Log("Reached target");
            AddReward(4f);
            EndEpisode();
        }
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var discreteActions = actionsOut.DiscreteActions;

        // Set the action based on key input
        if (Input.GetKey(KeyCode.W)) discreteActions[0] = 1;
        else if (Input.GetKey(KeyCode.S)) discreteActions[0] = 2;
        else if (Input.GetKey(KeyCode.D)) discreteActions[0] = 3;
        else if (Input.GetKey(KeyCode.A)) discreteActions[0] = 4;
        else discreteActions[0] = 0;
    }
}