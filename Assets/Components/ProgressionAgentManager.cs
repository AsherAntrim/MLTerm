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
    private int r = 0;
    private int c = 0;
    private bool first = true;

    public override void OnEpisodeBegin() {
        var episode = Academy.Instance.EpisodeCount;

        if (mazeGenerator.UseProgression) {
            if (first) {
                if (mazeGenerator.Rows >= mazeGenerator.StartRows && mazeGenerator.Columns >= mazeGenerator.StartColumns) {
                    r = mazeGenerator.Rows;
                    c = mazeGenerator.Columns;
                    mazeGenerator.Rows = mazeGenerator.StartRows;
                    mazeGenerator.Columns = mazeGenerator.StartColumns;
                }
                first = false;
            } else {
                if (mazeGenerator.Rows < r) {
                    mazeGenerator.Rows++;
                }
                if (mazeGenerator.Columns < c) {
                    mazeGenerator.Columns++;
                }
                
            }
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
