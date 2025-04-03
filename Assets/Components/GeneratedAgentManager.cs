using Unity.MLAgents;
using UnityEngine;

public class GeneratedAgentManager : AgentManager {

    [Tooltip("Will add the current episode count to the seed.")]
    public bool addEpisodeCount;

    public override void OnEpisodeBegin() {
        var maze = FindFirstObjectByType<ModMazeSpawn>();
        if (maze) {
            // Remove current maze
            foreach (Transform child in maze.transform) {
                Destroy(child.gameObject);
            }
            maze.GenerateMaze();
            if (addEpisodeCount) {
                maze.RandomSeed += Academy.Instance.EpisodeCount;
            }
        }
    }
}
