using TestCompany.Match3.Core;
using TestCompany.Match3.Static;
using UnityEngine;

namespace TestCompany.Match3.Utils
{
    public interface ICellCreator
    {
        Cell Create(CellType cellType, Transform container, Vector2Int position);
    }
}