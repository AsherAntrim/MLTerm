using UnityEngine;

public class StaticAgentManager : AgentManager {

    [Tooltip("Locations and reward wall prefabs to generate/regenerate when an episode begins. Good for static mazes.")]
    public RewardWallTemplate[] templates;
    private GameObject rewardWallsParent;

    void Start() {
        rewardWallsParent = Instantiate(new GameObject());
    }

    public override void OnEpisodeBegin() {
        while (rewardWallsParent.transform.childCount > 0) {
            DestroyImmediate(rewardWallsParent.transform.GetChild(0).gameObject);
        }

        foreach (var t in templates) {
            Instantiate(t, t.transform.position, t.transform.rotation, rewardWallsParent.transform);
        }
    }
}
