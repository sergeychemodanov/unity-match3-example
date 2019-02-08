using UnityEngine;
using TestCompany.Match3.Core;
using TestCompany.Match3.Static;

namespace TestCompany.Match3.Utils
{
    public class DefaultCellCreator : ICellCreator
    {
        public Cell Create(CellType cellType, Transform container, Vector2Int position)
        {
            var prefab = Resources.Load<Cell>($"Cells/{cellType}");
            var cell = Object.Instantiate(prefab);

            cell.gameObject.name = $"{cellType}Cell [{position.x}, {position.y}]";
            cell.transform.SetParent(container, false);
            cell.Initialize(position, cellType);

            return cell;
        }
    }
}