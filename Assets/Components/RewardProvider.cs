using UnityEngine;

public class RewardProvider : MonoBehaviour {
    public enum RewardType {
        Simple,
        Complex,
        ComplexCycle
    }

    public RewardType type = RewardType.Simple;
    public float rewardSize = 1;
    private float rewardAmount;

    void Start() {
        rewardAmount = rewardSize;
    }

    void OnTriggerEnter(Collider other) {
        var agent = other.GetComponent<MazeAgent>();
        if (agent) {
            agent.SetReward(rewardAmount);
            switch (type) {
                case RewardType.Simple:
                    Destroy(gameObject);
                    break;
                case RewardType.Complex:
                    if (rewardAmount > 0) {
                        rewardAmount = 0;
                    } else if (rewardAmount == 0) {
                        rewardAmount = -rewardSize;
                    }
                    break;
                case RewardType.ComplexCycle:
                    rewardAmount = rewardAmount == 0 ? 0 : -rewardSize;
                    break;
            }
        }
    }
}
