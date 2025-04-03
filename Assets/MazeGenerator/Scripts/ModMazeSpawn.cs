using UnityEngine;
using System.Collections;
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
    public int NumOfTestObjects = 1;
    public GameObject GoalPrefab = null;
    public GameObject TestObjPrefab = null;

    private BasicMazeGenerator mMazeGenerator = null;
    private List<int> sortedRandNumbers = new List<int>();

    void Start() {
        GenerateMaze();
    }

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
        if (NumOfTestObjects > 0) {
            sortedRandNumbers = SortRandNum(1, Rows * Columns, NumOfTestObjects);
            //testObjRand = Random.Range(1, Rows * Columns / NumOfTestObjects);
            //testObjCount = NumOfTestObjects;
        }

        for (int row = 0; row < Rows; row++) {
            for (int column = 0; column < Columns; column++) {
                float x = column * (CellWidth + (AddGaps ? .2f : 0));
                float z = row * (CellHeight + (AddGaps ? .2f : 0));
                MazeCell cell = mMazeGenerator.GetMazeCell(row, column);
                GameObject tmp;
                tmp = Instantiate(Floor, new Vector3(x, 0, z), Quaternion.Euler(0, 0, 0));
                tmp.transform.parent = transform;
                if (cell.WallRight) {
                    tmp = Instantiate(Wall, new Vector3(x + CellWidth / 2, 0, z) + Wall.transform.position, Quaternion.Euler(0, 90, 0));// right
                    tmp.transform.parent = transform;
                }
                if (cell.WallFront) {
                    tmp = Instantiate(Wall, new Vector3(x, 0, z + CellHeight / 2) + Wall.transform.position, Quaternion.Euler(0, 0, 0));// front
                    tmp.transform.parent = transform;
                }
                if (cell.WallLeft) {
                    tmp = Instantiate(Wall, new Vector3(x - CellWidth / 2, 0, z) + Wall.transform.position, Quaternion.Euler(0, 270, 0));// left
                    tmp.transform.parent = transform;
                }
                if (cell.WallBack) {
                    tmp = Instantiate(Wall, new Vector3(x, 0, z - CellHeight / 2) + Wall.transform.position, Quaternion.Euler(0, 180, 0));// back
                    tmp.transform.parent = transform;
                }
                if (cell.IsGoal && GoalPrefab != null) {
                    tmp = Instantiate(GoalPrefab, new Vector3(x, 1, z), Quaternion.Euler(0, 0, 0));
                    tmp.transform.parent = transform;
                }
                if (sortedRandNumbers.Count > 0 && sortedRandNumbers[0] == (row * Columns + column) && TestObjPrefab != null) {
                    tmp = Instantiate(TestObjPrefab, new Vector3(x, 1, z), Quaternion.Euler(0, 0, 0));
                    tmp.transform.parent = transform;
                    sortedRandNumbers.RemoveAt(0);
                }
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
