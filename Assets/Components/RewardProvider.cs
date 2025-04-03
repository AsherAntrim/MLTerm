using UnityEngine;

public class RewardProvider : MonoBehaviour {
    public float rewardAmount = 1;
    public bool diesOnCollision = true;

    void OnTriggerEnter(Collider other) {
        var agent = other.GetComponent<MazeAgent>();
        if (agent) {
            agent.SetReward(rewardAmount);
            if (diesOnCollision) {
                Destroy(this.gameObject);
            }
        }
    }
}
