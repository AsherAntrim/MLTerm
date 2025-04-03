using UnityEngine;

public class SimpleRewardProvider : MonoBehaviour {
    public float rewardAmount = 1;

    void OnTriggerEnter(Collider other) {
        var agent = other.GetComponent<MazeAgent>();
        if (agent) {
            agent.SetReward(ProvideReward());
            PostReward();
        }
    }

    public virtual float ProvideReward() {
        return rewardAmount;
    }

    public virtual void PostReward() {
        Destroy(gameObject);
    }
}
