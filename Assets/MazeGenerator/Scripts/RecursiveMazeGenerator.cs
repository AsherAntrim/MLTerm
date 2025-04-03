using UnityEngine;
using System.Collections;

//<summary>
//Pure recursive maze generation.
//Use carefully for large mazes.
//</summary>
public class RecursiveMazeGenerator : BasicMazeGenerator {

	public RecursiveMazeGenerator(int rows, int columns) : base(rows, columns) {

	}

	public override void GenerateMaze() {
		VisitCell(0, 0, Direction.Start);
	}

	private void VisitCell(int row, int column, Direction moveMade) {
		Direction[] movesAvailable = new Direction[4];
		int movesAvailableCount = 0;

		do {
			movesAvailableCount = 0;

			var curCell = GetMazeCell(row, column);
			//check move right
			if (column + 1 < ColumnCount && !GetMazeCell(row, column + 1).IsVisited) {
				movesAvailable[movesAvailableCount] = Direction.Right;
				movesAvailableCount++;
				curCell.RewardRight = true;
			} else if (!curCell.IsVisited && moveMade != Direction.Left) {
				curCell.WallRight = true;
			}
			//check move forward
			if (row + 1 < RowCount && !GetMazeCell(row + 1, column).IsVisited) {
				movesAvailable[movesAvailableCount] = Direction.Front;
				movesAvailableCount++;
				curCell.RewardFront = true;
			} else if (!curCell.IsVisited && moveMade != Direction.Back) {
				curCell.WallFront = true;
			}
			//check move left
			if (column > 0 && column - 1 >= 0 && !GetMazeCell(row, column - 1).IsVisited) {
				movesAvailable[movesAvailableCount] = Direction.Left;
				movesAvailableCount++;
				curCell.RewardLeft = true;
			} else if (!curCell.IsVisited && moveMade != Direction.Right) {
				curCell.WallLeft = true;
			}
			//check move backward
			if (row > 0 && row - 1 >= 0 && !GetMazeCell(row - 1, column).IsVisited) {
				movesAvailable[movesAvailableCount] = Direction.Back;
				movesAvailableCount++;
				curCell.RewardBack = true;
			} else if (!curCell.IsVisited && moveMade != Direction.Front) {
				curCell.WallBack = true;
			}

			// Determine whether it's a straight corridor. If it is, do not generate reward walls.
			if ((curCell.WallBack && curCell.WallFront && !curCell.WallRight && !curCell.WallLeft) ||
				(curCell.WallRight && curCell.WallLeft && !curCell.WallBack && !curCell.WallFront)) {
				curCell.RewardRight = false;
				curCell.RewardFront = false;
				curCell.RewardLeft = false;
				curCell.RewardBack = false;
			}

			if (movesAvailableCount == 0 && !curCell.IsVisited) {
				curCell.IsGoal = true;
			}

			curCell.IsVisited = true;

			if (movesAvailableCount > 0) {
				switch (movesAvailable[Random.Range(0, movesAvailableCount)]) {
					case Direction.Start:
						break;
					case Direction.Right:
						VisitCell(row, column + 1, Direction.Right);
						break;
					case Direction.Front:
						VisitCell(row + 1, column, Direction.Front);
						break;
					case Direction.Left:
						VisitCell(row, column - 1, Direction.Left);
						break;
					case Direction.Back:
						VisitCell(row - 1, column, Direction.Back);
						break;
				}
			}

		} while (movesAvailableCount > 0);
	}
}
