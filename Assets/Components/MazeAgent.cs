using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class MazeAgent : Agent {

    Rigidbody rBody;
    void Start() {
        rBody = GetComponent<Rigidbody>();
    }
	
	public Transform[] transforms;
	public GameObject edibleWall;

    public override void OnEpisodeBegin() {
		foreach(var t in transforms) {
			var wall = Instantiate(edibleWall);
		}
		
        rBody.angularVelocity = Vector3.zero;
        rBody.linearVelocity = Vector3.zero;
        var spawnPos = FindAnyObjectByType<AgentSpawner>();
        transform.SetPositionAndRotation(spawnPos.transform.position + new Vector3(0, this.transform.localScale.y, 0), spawnPos.transform.rotation);
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
