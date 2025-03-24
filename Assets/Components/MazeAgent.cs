using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using UnityEngine;

public class MazeAgent : Agent {

    Rigidbody rBody;
    void Start() {
        rBody = GetComponent<Rigidbody>();
    }

    public override void OnEpisodeBegin() {
        rBody.angularVelocity = Vector3.zero;
        rBody.linearVelocity = Vector3.zero;
    }

    public float forceMultiplier = 10;
    public float rotationMultiplier = 10;
    public override void OnActionReceived(ActionBuffers actionBuffers) {
        Vector3 forward = transform.forward;
        transform.Rotate(Vector3.up, actionBuffers.ContinuousActions[0] * rotationMultiplier);
        forward *= actionBuffers.ContinuousActions[1];
        rBody.AddForce(forward * forceMultiplier);
    }

    public override void Heuristic(in ActionBuffers actionsOut) {
        var _ = actionsOut.ContinuousActions;
        _[0] = Input.GetAxis("Horizontal");
        _[1] = Input.GetAxis("Vertical");
    }
}
