using UnityEngine;
using System.Collections.Generic;
using System.Linq;

//<summary>
//Game object, that creates maze and instantiates it in scene
//</summary>
public class ModMazeSpawn : MonoBehaviour {
    public enum MazeGenerationAlgorithm {
        PureRecursive,
        RecursiveTree,
        RandomTree,
        OldestTree,
        RecursiveDivision,
    }

    public MazeGenerationAlgorithm Algorithm = MazeGenerationAlgorithm.PureRecursive;
    public bool FullRandom = false;
    public int RandomSeed = 12345;
    public GameObject Floor = null;
    public GameObject Wall = null;
    public GameObject Pillar = null;
    public int Rows = 5;
    public int Columns = 5;
    public float CellWidth = 5;
    public float CellHeight = 5;
    public bool AddGaps = true;
    public int NumRandPositions = 1;
    public GameObject Goal = null;
    public GameObject RewardWall;
    public GameObject Spawn = null;

    private BasicMazeGenerator mMazeGenerator = null;
    private List<int> sortedRandNumbers = new List<int>();

    public void GenerateMaze() {
        if (!FullRandom) {
            Random.InitState(RandomSeed);
        }
        switch (Algorithm) {
            case MazeGenerationAlgorithm.PureRecursive:
                mMazeGenerator = new RecursiveMazeGenerator(Rows, Columns);
                break;
            case MazeGenerationAlgorithm.RecursiveTree:
                mMazeGenerator = new RecursiveTreeMazeGenerator(Rows, Columns);
                break;
            case MazeGenerationAlgorithm.RandomTree:
                mMazeGenerator = new RandomTreeMazeGenerator(Rows, Columns);
                break;
            case MazeGenerationAlgorithm.OldestTree:
                mMazeGenerator = new OldestTreeMazeGenerator(Rows, Columns);
                break;
            case MazeGenerationAlgorithm.RecursiveDivision:
                mMazeGenerator = new DivisionMazeGenerator(Rows, Columns);
                break;
        }
        mMazeGenerator.GenerateMaze();
        if (NumRandPositions > 0) {
            sortedRandNumbers = SortRandNum(1, Rows * Columns, NumRandPositions);
        }

        bool goalGenerated = false;
        int spawnPos = sortedRandNumbers[0]; // not super random, but should be fine. just grabbing the first position every time which will be sorted to be near begin.
        sortedRandNumbers.RemoveAt(0);

        for (int row = 0; row < Rows; row++) {
            for (int column = 0; column < Columns; column++) {
                float x = column * (CellWidth + (AddGaps ? .2f : 0));
                float z = row * (CellHeight + (AddGaps ? .2f : 0));
                float rot = 0;
                MazeCell cell = mMazeGenerator.GetMazeCell(row, column);
                GameObject tmp;
                if (Floor) {
                    tmp = Instantiate(Floor, new Vector3(x, 0, z), Quaternion.Euler(0, 0, 0));
                    tmp.transform.parent = transform;
                }
                if (cell.WallRight) {
                    rot = 90;
                    tmp = Instantiate(Wall, new Vector3(x + CellWidth / 2, 0, z) + Wall.transform.position, Quaternion.Euler(0, rot, 0));// right
                    tmp.transform.parent = transform;
                }
                if (cell.WallFront) {
                    rot = 0;
                    tmp = Instantiate(Wall, new Vector3(x, 0, z + CellHeight / 2) + Wall.transform.position, Quaternion.Euler(0, rot, 0));// front
                    tmp.transform.parent = transform;
                }
                if (cell.WallLeft) {
                    rot = 270;
                    tmp = Instantiate(Wall, new Vector3(x - CellWidth / 2, 0, z) + Wall.transform.position, Quaternion.Euler(0, rot, 0));// left
                    tmp.transform.parent = transform;
                }
                if (cell.WallBack) {
                    rot = 180;
                    tmp = Instantiate(Wall, new Vector3(x, 0, z - CellHeight / 2) + Wall.transform.position, Quaternion.Euler(0, rot, 0));// back
                    tmp.transform.parent = transform;
                }
                if (cell.IsGoal && !goalGenerated && Goal != null) {
                    if (cell.WallFront) {
                        rot += 90;
                    }
                    tmp = Instantiate(Goal, new Vector3(x, 0, z), Quaternion.Euler(0, rot, 0));
                    tmp.transform.parent = transform;
                    goalGenerated = true;
                }

                if (cell.RewardRight) {
                    tmp = Instantiate(RewardWall, new Vector3(x + CellWidth / 2, 0, z) + Wall.transform.position, Quaternion.Euler(0, 90, 0));
                    tmp.transform.parent = transform;
                }
                if (cell.RewardFront) {
                    tmp = Instantiate(RewardWall, new Vector3(x, 0, z + CellHeight / 2) + Wall.transform.position, Quaternion.Euler(0, 0, 0));
                    tmp.transform.parent = transform;
                }
                if (cell.RewardLeft) {
                    tmp = Instantiate(RewardWall, new Vector3(x - CellWidth / 2, 0, z) + Wall.transform.position, Quaternion.Euler(0, 270, 0));
                    tmp.transform.parent = transform;
                }
                if (cell.RewardBack) {
                    tmp = Instantiate(RewardWall, new Vector3(x, 0, z - CellHeight / 2) + Wall.transform.position, Quaternion.Euler(0, 180, 0));
                    tmp.transform.parent = transform;
                }

                if ((row * Columns + column) == spawnPos && Spawn != null) {
                    tmp = Instantiate(Spawn, new Vector3(x, 0.1f, z), Quaternion.Euler(0, 0, 0));
                    tmp.transform.parent = transform;
                }

                // if (sortedRandNumbers.Count > 0 && sortedRandNumbers[0] == (row * Columns + column) && Spawn != null) {
                //     tmp = Instantiate(Spawn, new Vector3(x, 1, z), Quaternion.Euler(0, 0, 0));
                //     tmp.transform.parent = transform;
                //     sortedRandNumbers.RemoveAt(0);
                // }
            }
        }
        if (Pillar != null) {
            for (int row = 0; row < Rows + 1; row++) {
                for (int column = 0; column < Columns + 1; column++) {
                    float x = column * (CellWidth + (AddGaps ? .2f : 0));
                    float z = row * (CellHeight + (AddGaps ? .2f : 0));
                    GameObject tmp = Instantiate(Pillar, new Vector3(x - CellWidth / 2, 0, z - CellHeight / 2), Quaternion.identity) as GameObject;
                    tmp.transform.parent = transform;
                }
            }
        }
    }

    List<int> SortRandNum(int min, int max, int count) {
        HashSet<int> uniqueNumbers = new HashSet<int>();

        // Generate unique numbers
        while (uniqueNumbers.Count < count) {
            uniqueNumbers.Add(Random.Range(min, max + 1));
        }

        // Convert to list and sort
        return uniqueNumbers.ToList().OrderBy(num => num).ToList();
    }
}
