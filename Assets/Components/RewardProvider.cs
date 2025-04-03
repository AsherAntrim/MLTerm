using UnityEngine;

public class RewardProvider : MonoBehaviour {
    public string type = "Simple";
    public float rewardSize = 1;
    private float rewardAmount;

    void Start() {
        rewardAmount = rewardSize;
    }

    void OnTriggerEnter(Collider other) {
        var agent = other.GetComponent<MazeAgent>();
        if (agent) {
            agent.SetReward(rewardAmount);
            if (type == "Simple") {
                Destroy(gameObject);
            } else if (type == "Complex") {
                if (rewardAmount > 0) {
                    rewardAmount = 0;
                } else if (rewardAmount == 0) {
                    rewardAmount = -rewardSize;
                }
            } else if (type == "ComplexCycle") {
                if (rewardAmount != 0) {
                    rewardAmount = 0;
                } else if (rewardAmount == 0) {
                    rewardAmount = -rewardSize;
                }
            }
        }
    }
}
