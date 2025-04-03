using UnityEngine;

public class RewardProvider : MonoBehaviour {
    public float rewardSize = 1;
    private float rewardAmount;

    void Start() {
        rewardAmount = rewardSize;
    }

    void OnTriggerEnter(Collider other) {
        var agent = other.GetComponent<MazeAgent>();
        if (agent) {
            agent.SetReward(rewardAmount);
            if (rewardAmount > 0) {
                rewardAmount = 0;
            }
            if (rewardAmount == 0) {
                rewardAmount = -rewardSize;
            }
        }
    }
}
