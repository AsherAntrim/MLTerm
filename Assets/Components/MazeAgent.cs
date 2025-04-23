using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class MazeAgent : Agent {

    public AgentManager agentManager;
    public bool alterMove = false;

    private bool moveToggle;

    Rigidbody rBody;
    void Start() {
        rBody = GetComponent<Rigidbody>();
    }

    public override void OnEpisodeBegin() {
        if (agentManager) {
            agentManager.OnEpisodeBegin();
        }

        rBody.angularVelocity = Vector3.zero;
        rBody.linearVelocity = Vector3.zero;
        var spawnPos = FindAnyObjectByType<AgentSpawner>();
        if (spawnPos) {
            transform.SetPositionAndRotation(spawnPos.transform.position + new Vector3(0, this.transform.localScale.y, 0), spawnPos.transform.rotation);
        }
    }

    public float forceMultiplier = 10;
    public float rotationMultiplier = 10;
    public override void OnActionReceived(ActionBuffers actionBuffers) {
        if (alterMove) {
            Vector3 forward = transform.forward;
            switch (moveToggle) {
                case true:
                    transform.Rotate(Vector3.up, actionBuffers.ContinuousActions[0] * rotationMultiplier);
                    break;
                case false:
                    forward *= actionBuffers.ContinuousActions[1];
                    rBody.AddForce(forward * forceMultiplier);
                    break;
            }
            moveToggle = !moveToggle;
            
            
            
        } else {
            Vector3 forward = transform.forward;
            transform.Rotate(Vector3.up, actionBuffers.ContinuousActions[0] * rotationMultiplier);
            forward *= actionBuffers.ContinuousActions[1];
            rBody.AddForce(forward * forceMultiplier);
        }
        

        // allow agent to manipulate the ray sensor
        var raySensor = GetComponent<RayPerceptionSensorComponent3D>();
        if (raySensor && false) {
            raySensor.EndVerticalOffset += actionBuffers.ContinuousActions[2] * 0.001f;
        }
    }

    public override void CollectObservations(VectorSensor sensor) {
        sensor.AddObservation(transform.position);
    }

    public override void Heuristic(in ActionBuffers actionsOut) {
        var _ = actionsOut.ContinuousActions;

        _[0] = Input.GetAxis("Horizontal");
        _[1] = Input.GetAxis("Vertical");
        
    }
}
