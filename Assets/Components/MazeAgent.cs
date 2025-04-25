using System;
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
        seenGoalTimes = 0;
        if (agentManager) {
            agentManager.OnEpisodeBegin();
        }

        rBody.angularVelocity = Vector3.zero;
        rBody.linearVelocity = Vector3.zero;
        var spawnPos = FindAnyObjectByType<AgentSpawner>();
        if (spawnPos) {
            UnityEngine.Random.InitState(Academy.Instance.EpisodeCount);
            transform.position = spawnPos.transform.position + new Vector3(0, transform.localScale.y, 0);
            transform.Rotate(Vector3.up, UnityEngine.Random.Range(0, 360));
        }
    }

    public float forceMultiplier = 10;
    public float rotationMultiplier = 10;
    [Tooltip("The number of steps the agent can take before ending the episode automatically.")]
    public int resetStepCount = 6000;
    int seenGoalTimes;

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
            Vector3 forward = transform.forward * Math.Abs(actionBuffers.ContinuousActions[1]);
            rBody.AddForce(forward * forceMultiplier);
        }

        // var distToGoal = Vector3.Distance(transform.position, GameObject.FindGameObjectWithTag("Finish").transform.position);
        // if (distToGoal < lastDistance) {
        //     AddReward(0.0001f);
        // } else {
        //     AddReward(-0.0001f);
        // }

        // var rayPerceptor = GetComponent<RayPerceptionSensorComponent3D>();
        // var outputs = rayPerceptor.RaySensor.RayPerceptionOutput.RayOutputs;
        // if (outputs is not null) {
        //     foreach (var output in outputs) {
        //         if (output.HitTaggedObject && output.HitTagIndex == 1) { // 0 is wall, 1 is finish, 2 is rward
        //             seenGoalTimes++;
        //             AddReward(0.01f / seenGoalTimes);
        //         }
        //     }
        // }

        AddReward(-0.0001f);
    }

    // void OnCollisionEnter(Collision col) {
    //     if (col.gameObject.CompareTag("Wall")) {
    //         AddReward(-0.1f);
    //     }
    // }

    // void OnCollisionStay(Collision col) {
    //     if (col.gameObject.CompareTag("Wall")) {
    //         AddReward(-0.1f);
    //     }
    // }

    public override void CollectObservations(VectorSensor sensor) {
        sensor.AddObservation(transform.position);
        // sensor.AddObservation(GameObject.FindGameObjectWithTag("Finish").transform.position);
    }

    public override void Heuristic(in ActionBuffers actionsOut) {
        var _ = actionsOut.ContinuousActions;

        _[0] = Input.GetAxis("Horizontal");
        _[1] = Input.GetAxis("Vertical");
    }
}
