using System.Linq;
using System.Runtime.CompilerServices;
using Unity.MLAgents;
using UnityEngine;
using UnityEngine.Scripting;

public class ProgressionAgentManager : AgentManager {
    public enum MixStrategy {
        Random,
        InOrder
    }

    [Tooltip("Will add the current episode count to the seed.")]
    public bool addEpisodeCount;
    [Tooltip("List of custom mazes to mix into procedurally generated mazes.")]
    public GameObject[] staticMazes;
    [Tooltip("Reference to the procedural maze generator.")]
    public ModMazeSpawn mazeGenerator;
    [Tooltip(@"How new static maze should be selected. Random will select a 
               random element from the list seeded by the current episode. 
               InOrder will keep track of the last index used and increment,
               cycling back to the beginning after reaching the end.")]
    public MixStrategy strategy;

    private int index = 0;

    public override void OnEpisodeBegin() {
        var episode = Academy.Instance.EpisodeCount;

        Debug.Log("Rows: " + mazeGenerator.Rows);
        Debug.Log("Columns: " + mazeGenerator.Columns);
        Debug.Log("EpisodeNum: " + episode);
        if (episode == 1) {
            mazeGenerator.Rows = 2;
            mazeGenerator.Columns = 2;
            Debug.Log("Rows: " + mazeGenerator.Rows);
            Debug.Log("Columns: " + mazeGenerator.Columns);
        } else {
            mazeGenerator.Rows = 3;
            mazeGenerator.Columns = 3;
            Debug.Log("Rows: " + mazeGenerator.Rows);
            Debug.Log("Columns: " + mazeGenerator.Columns);
        }
        
        // Remove current maze
        foreach (Transform child in mazeGenerator.transform) {
            Destroy(child.gameObject);
        }

        if (staticMazes is null || staticMazes.Count() == 0) {
            if (addEpisodeCount) {
                mazeGenerator.RandomSeed += episode;
            }
            mazeGenerator.GenerateMaze();
        } else {
            Random.InitState(episode);
            if (Random.Range(0f, 1f) < 0.5f) {
                if (addEpisodeCount) {
                    mazeGenerator.RandomSeed += episode;
                }
                mazeGenerator.GenerateMaze();
            } else {
                switch (strategy) {
                    case MixStrategy.InOrder:
                        Instantiate(staticMazes[index++], mazeGenerator.transform);
                        break;
                    case MixStrategy.Random:
                        var randIndex = Random.Range(0, staticMazes.Count());
                        Instantiate(staticMazes[randIndex], mazeGenerator.transform);
                        break;
                }
            }
        }
    }
}
