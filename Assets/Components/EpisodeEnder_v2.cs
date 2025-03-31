using UnityEngine;

public class EpisodeEnder_v2 : MonoBehaviour {
    public GameObject maze;
    void OnTriggerEnter(Collider other) {
        var agent = other.GetComponent<MazeAgent>();
        var mazeGen = maze.GetComponent<MazeSpawner>();
        if (agent) {
            agent.EndEpisode();
            mazeGen.GenerateMaze();
        }
    }
}
