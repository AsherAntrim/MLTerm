using UnityEngine;

public class EpisodeEnder : MonoBehaviour {
    void OnTriggerEnter(Collider other) {
        var agent = other.GetComponent<MazeAgent>();
        if (agent) {
            agent.EndEpisode();
        }
    }
}
