using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class MazeAgent : Agent {

    Rigidbody rBody;
    void Start() {
        rBody = GetComponent<Rigidbody>();
    }

    public GameObject ViewCamera;

    public override void OnEpisodeBegin() {
        var maze = FindFirstObjectByType<MazeSpawner>();
        // Remove current maze
        while (maze.transform.childCount > 0) {
            DestroyImmediate(maze.transform.GetChild(0).gameObject);
        }
        maze.GenerateMaze();

        rBody.angularVelocity = Vector3.zero;
        rBody.linearVelocity = Vector3.zero;
        var spawnPos = FindAnyObjectByType<AgentSpawner>();
        if (spawnPos) {
            transform.SetPositionAndRotation(spawnPos.transform.position + new Vector3(0, this.transform.localScale.y, 0), spawnPos.transform.rotation);
        }
    }

    void FixedUpdate() {
        if (ViewCamera != null) {
            Vector3 direction = (Vector3.up * 2 + Vector3.back) * 2;
            RaycastHit hit;
            Debug.DrawLine(transform.position, transform.position + direction, Color.red);
            if (Physics.Linecast(transform.position, transform.position + direction, out hit)) {
                ViewCamera.transform.position = hit.point;
            } else {
                ViewCamera.transform.position = transform.position + direction;
            }
            ViewCamera.transform.LookAt(transform.position);
        }
    }

    public float forceMultiplier = 10;
    public float rotationMultiplier = 10;
    public override void OnActionReceived(ActionBuffers actionBuffers) {
        Vector3 forward = transform.forward;
        transform.Rotate(Vector3.up, actionBuffers.ContinuousActions[0] * rotationMultiplier);
        forward *= actionBuffers.ContinuousActions[1];
        rBody.AddForce(forward * forceMultiplier);

        // allow agent to manipulate the ray sensor
        var raySensor = GetComponent<RayPerceptionSensorComponent3D>();
        if (raySensor && false) {
            raySensor.EndVerticalOffset += actionBuffers.ContinuousActions[2] * 0.001f;
        }
    }

    public override void Heuristic(in ActionBuffers actionsOut) {
        var _ = actionsOut.ContinuousActions;
        _[0] = Input.GetAxis("Horizontal");
        _[1] = Input.GetAxis("Vertical");
    }
}
