using UnityEngine;

public class GeneratedAgentManager : AgentManager {

    [Tooltip("Will increment the maze generator random seed every time a new episode begins.")]
    public bool autoIncrementSeed;

    public override void OnEpisodeBegin() {
        var maze = FindFirstObjectByType<ModMazeSpawn>();
        // Remove current maze
        while (maze.transform.childCount > 0) {
            DestroyImmediate(maze.transform.GetChild(0).gameObject);
        }
        maze.GenerateMaze();
        if (autoIncrementSeed) {
            maze.RandomSeed += 1;
        }
    }
}
