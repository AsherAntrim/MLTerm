using UnityEngine;

public class StaticAgentManager : AgentManager {

    [Tooltip("Locations and reward wall prefabs to generate/regenerate when an episode begins. Good for static mazes.")]
    public RewardWallTemplate[] templates;
    private GameObject rewardWallsParent;

    void Start() {
        rewardWallsParent = Instantiate(new GameObject());
    }

    public override void OnEpisodeBegin() {
        foreach (Transform t in rewardWallsParent.transform) {
            Destroy(t.gameObject);
        }

        foreach (var t in templates) {
            Instantiate(t.rewardWall, t.transform.position, t.transform.rotation, rewardWallsParent.transform);
        }
    }
}
