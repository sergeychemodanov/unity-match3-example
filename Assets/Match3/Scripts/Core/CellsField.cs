using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TestCompany.Common;
using TestCompany.Match3.Static;
using TestCompany.Match3.Utils;

namespace TestCompany.Match3.Core
{
    public class CellsField : MonoBehaviourFiniteStateMachine<CellsFieldState>
    {
        public event EventHandler<Static.Item> OnItemDestroyed;

        public event EventHandler OnPlayerMadeMove;

        public Cell[,] Cells { get; private set; }

        public LevelData LevelData { get; private set; }


        public void Play(LevelData levelData)
        {
            LevelData = levelData;
            SetState(CellsFieldState.Initialization);
        }

        public void Clear()
        {
            SetState(CellsFieldState.None);
            StopAllCoroutines();
            transform.ClearChildObjects();
            Cells = null;
        }


        private void Start()
        {
            AddTransition(CellsFieldState.Initialization, OnInitializationStateEnter);
            AddTransition(CellsFieldState.DestroyingMatches, OnDestroyingMatchesStateEnter);
            AddTransition(CellsFieldState.ItemsFalling, OnItemsFallingStateEnter);
        }

        private void OnInitializationStateEnter()
        {
            var cellCreator = new DefaultCellCreator();
            var itemCreator = new DefaultItemCreator(App.Instance.StaticDataProvider);
            var xLength = LevelData.Cells.GetLength(0);
            var yLength = LevelData.Cells.GetLength(1);

            Cells = new Cell[xLength, yLength];

            for (var x = 0; x < xLength; x++)
            {
                for (var y = 0; y < yLength; y++)
                {
                    var cellType = LevelData.Cells[x, y];
                    var position = new Vector2Int(x, y);
                    var cell = cellCreator.Create(cellType, transform, position);

                    if (cellType != CellType.Obstacle)
                    {
                        var randomItem = itemCreator.CreateRandom(cell.transform);
                        randomItem.OnMove += OnItemDragged;
                        cell.SetItem(randomItem);
                    }

                    Cells[x, y] = cell;
                }
            }

            Camera.main.transform
                .SetXPosition((xLength - 1) / 2f)
                .SetYPosition((yLength - 1) / 2f);

            SetState(CellsFieldState.DestroyingMatches);
        }

        private void OnDestroyingMatchesStateEnter()
        {
            var matches = Cells.GetMatches();
            if (matches.Count > 0)
            {
                foreach (var matchedCell in matches)
                {
                    OnItemDestroyed?.Invoke(matchedCell.Item.StaticItem);
                    matchedCell.DestroyItem();
                }

                SetState(CellsFieldState.ItemsFalling);
                return;
            }

            SetState(CellsFieldState.Idle);
        }

        private void OnItemsFallingStateEnter()
        {
            var emptyCells = Cells.GetEmpty();
            StartCoroutine(MoveItemsDown(emptyCells));
        }

        private IEnumerator MoveItemsDown(List<List<Cell>> emptyCells)
        {
            var itemsToMove = new Dictionary<Item, Cell>();

            // get items to move
            foreach (var emptyCellsRow in emptyCells)
            {
                // check items from upper cells
                foreach (var emptyCell in emptyCellsRow.ToList())
                {
                    var upperCell = Cells.GetNeighborCell(emptyCell, Vector2Int.up);
                    if (upperCell != null && upperCell.Item != null)
                    {
                        itemsToMove.Add(upperCell.Item, emptyCell);
                        emptyCellsRow.Remove(emptyCell);
                    }
                }

                // check items from upper right & upper left cells
                foreach (var emptyCell in emptyCellsRow)
                {
                    Item item = null;

                    var upperRightCell = Cells.GetNeighborCell(emptyCell, Vector2Int.up + Vector2Int.right);
                    if (upperRightCell != null && upperRightCell.Item != null)
                        item = upperRightCell.Item;

                    if (item == null)
                    {
                        var upperLeftCell = Cells.GetNeighborCell(emptyCell, Vector2Int.up + Vector2Int.left);
                        if (upperLeftCell != null && upperLeftCell.Item != null)
                            item = upperLeftCell.Item;
                    }

                    if (item != null && !itemsToMove.ContainsKey(item))
                        itemsToMove.Add(item, emptyCell);
                }
            }

            // move items
            if (itemsToMove.Count > 0)
            {
                foreach (var itemToMoveData in itemsToMove)
                {
                    var startingCell = itemToMoveData.Key.Cell;
                    itemToMoveData.Value.SetItem(itemToMoveData.Key);
                    startingCell.SetItem(null);

                    Tweener.Instance.MoveTo(
                        itemToMoveData.Key.transform,
                        itemToMoveData.Value.transform.position,
                        Constants.ItemMoveTime);
                }

                SpawnNewItems();
                yield return new WaitForSeconds(Constants.ItemMoveTime);

                var nextEmptyCells = Cells.GetEmpty();
                if (nextEmptyCells.Count > 0)
                {
                    StartCoroutine(MoveItemsDown(nextEmptyCells));
                    yield break;
                }
            }

            // all items moved
            SetState(CellsFieldState.DestroyingMatches);
        }

        private void OnItemDragged(Item item, Vector2Int direction)
        {
            if (CurrentState != CellsFieldState.Idle)
                return;

            var startingCell = item.Cell;
            var destinationCell = Cells.GetNeighborCell(startingCell, direction);
            if (destinationCell == null || destinationCell.Item == null || destinationCell.CellType != CellType.Default)
                return;

            SetState(CellsFieldState.HandlingInput);

            Tweener.Instance.SwapPositions(
                item.transform,
                destinationCell.Item.transform,
                Constants.ItemMoveTime,
                () => {
                    startingCell.SetItem(destinationCell.Item);
                    destinationCell.SetItem(item);

                    var matches = Cells.GetMatches();
                    if (matches.Count > 0)
                    {
                        SetState(CellsFieldState.DestroyingMatches);
                        OnPlayerMadeMove?.Invoke();
                    }
                    else
                    {
                        Tweener.Instance.SwapPositions(
                            item.transform,
                            startingCell.Item.transform,
                            Constants.ItemMoveTime,
                            () => {
                                destinationCell.SetItem(startingCell.Item);
                                startingCell.SetItem(item);
                                SetState(CellsFieldState.Idle);
                            });
                    }
                });
        }

        private void SpawnNewItems()
        {
            var itemCreator = new DefaultItemCreator(App.Instance.StaticDataProvider);
            var spawnCells = Cells.GetByType(CellType.SpawnPoint);
            foreach (var spawnCell in spawnCells)
            {
                if (spawnCell.Item != null)
                    continue;

                var item = itemCreator.CreateRandom(spawnCell.transform);
                item.OnMove += OnItemDragged;
                spawnCell.SetItem(item);
            }
        }
    }

    public enum CellsFieldState
    {
        None,
        Initialization,
        HandlingInput,
        DestroyingMatches,
        ItemsFalling,
        Idle
    }
}