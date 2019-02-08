using System.Collections.Generic;
using UnityEngine;
using TestCompany.Match3.Core;
using TestCompany.Match3.Static;

namespace TestCompany.Match3.Utils
{
    public static class Extensions
    {
        public static bool IsPositionValid(this Cell[,] cells, Vector2Int position)
        {
            var xLength = cells.GetLength(0);
            var yLength = cells.GetLength(1);

            var xValid = position.x >= 0 && position.x < xLength;
            var yValid = position.y >= 0 && position.y < yLength;

            return xValid && yValid;
        }

        public static Cell GetNeighborCell(this Cell[,] cells, Cell startCell, Vector2Int direction)
        {
            var position = startCell.Position + direction;
            var isPositionValid = cells.IsPositionValid(position);
            return isPositionValid ? cells[position.x, position.y] : null;
        }

        public static List<List<Cell>> GetEmpty(this Cell[,] cells)
        {
            var emptyCells = new List<List<Cell>>();

            var xLength = cells.GetLength(0);
            var yLength = cells.GetLength(1);

            for (var y = 0; y < yLength; y++)
            {
                var rowCells = new List<Cell>();

                for (var x = xLength - 1; x >= 0; x--)
                {
                    var cell = cells[x, y];
                    if (cell.Item == null && cell.CellType == CellType.Default)
                        rowCells.Add(cell);
                }

                if (rowCells.Count > 0)
                    emptyCells.Add(rowCells);
            }

            return emptyCells;
        }

        public static List<Cell> GetByType(this Cell[,] cells, CellType cellType)
        {
            var cellsByType = new List<Cell>();

            var xLength = cells.GetLength(0);
            var yLength = cells.GetLength(1);

            for (var y = 0; y < yLength; y++)
            {
                for (var x = xLength - 1; x >= 0; x--)
                {
                    var cell = cells[x, y];
                    if (cell.CellType == cellType)
                        cellsByType.Add(cell);
                }
            }

            return cellsByType;
        }

        public static List<Cell> GetMatches(this Cell[,] cells)
        {
            var matches = new List<Cell>();

            var xLength = cells.GetLength(0);
            var yLength = cells.GetLength(1);

            // check columns matches
            for (var x = xLength - 1; x >= 0; x--)
            {
                var column = new List<Cell>();

                for (var y = 0; y < yLength; y++)
                {
                    var cell = cells[x, y];
                    column.Add(cell);
                }

                var matchesForColumn = GetMatches(column);
                if (matchesForColumn.Count > 0)
                    matches.AddRange(matchesForColumn);
            }

            // check rows matches
            for (var y = 0; y < yLength; y++)
            {
                var row = new List<Cell>();

                for (var x = xLength - 1; x >= 0; x--)
                {
                    var cell = cells[x, y];
                    row.Add(cell);
                }

                var matchesForRow = GetMatches(row);
                foreach (var cell in matchesForRow)
                {
                    if (!matches.Contains(cell))   
                        matches.Add(cell);
                }
            }

            return matches;
        }


        private static List<Cell> GetMatches(List<Cell> cells)
        {
            const int minCellsCount = 3;
            var matches = new List<Cell>();

            var lastItemId = string.Empty;
            var cellsChain = new List<Cell>();

            for (var i = 0; i < cells.Count; i++)
            {
                var cell = cells[i];
                if (cell.Item != null && cell.CellType == CellType.Default)
                {
                    // if items equal - add to chain
                    if (i == 0 || lastItemId == cell.Item.StaticItem.Id)
                    {
                        cellsChain.Add(cell);
                        lastItemId = cell.Item.StaticItem.Id;

                        // if this is the last cell
                        if (i >= cells.Count - 1 && cellsChain.Count >= minCellsCount)
                            matches.AddRange(cellsChain);
                    }
                    // if items not equal - start new chain
                    else
                    {
                        if (cellsChain.Count >= minCellsCount)
                            matches.AddRange(cellsChain);

                        cellsChain.Clear();
                        cellsChain.Add(cell);
                        lastItemId = cell.Item.StaticItem.Id;
                    }
                }
                else
                {
                    if (cellsChain.Count >= minCellsCount)
                        matches.AddRange(cellsChain);

                    lastItemId = string.Empty;
                    cellsChain.Clear();
                }
            }

            return matches;
        }
    }
}