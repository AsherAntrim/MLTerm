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
            transform.SetPositionAndRotation(
                spawnPos.transform.position + new Vector3(0, this.transform.localScale.y, 0),
                Quaternion.Euler(spawnPos.transform.rotation.eulerAngles + new Vector3(0, Random.Range(0, 360), 0)));
        }
    }

    public float forceMultiplier = 10;
    public float rotationMultiplier = 10;
	private int stepCount;

    float lastDistance;
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
            transform.Rotate(Vector3.up, actionBuffers.ContinuousActions[0] * rotationMultiplier);
            Vector3 forward = transform.forward * actionBuffers.ContinuousActions[1];
            rBody.AddForce(forward * forceMultiplier);
        }

        var goal = GameObject.FindGameObjectWithTag("Finish");
        if (Vector3.Distance(transform.position, goal.transform.position) < lastDistance) {
            AddReward(0.01f);
        } else {
            AddReward(-0.01f);
        }

        if (Academy.Instance.StepCount > stepCount+6000) {
			stepCount += 6000;
            SetReward(-1);
            EndEpisode();
        }
    }
	
	void OnCollisionEnter(Collision col) {
		if (col.gameObject.CompareTag("Wall")) {
			AddReward(-0.1f);
		}
	}
	
	void OnCollisionStay(Collision col) {
		if (col.gameObject.CompareTag("Wall")) {
			AddReward(-0.01f);
		}
	}
	
	void OnCollisionExit(Collision col) {
		if (col.gameObject.CompareTag("Wall")) {
			AddReward(0.1f);
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
