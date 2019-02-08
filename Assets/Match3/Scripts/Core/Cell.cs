using UnityEngine;
using TestCompany.Common;
using TestCompany.Match3.Static;

namespace TestCompany.Match3.Core
{
    public class Cell : MonoBehaviour
    {
        public Vector2Int Position { get; private set; }

        public CellType CellType { get; private set; }

        public Item Item { get; private set; }
        
        public void Initialize(Vector2Int position, CellType cellType)
        {
            Position = position;
            CellType = cellType;
            transform.localPosition = Position.ToVector3();
        }

        public void SetItem(Item item)
        {
            if (CellType == CellType.Obstacle)
                return;

            Item = item;
            
            if (Item != null)
                Item.SetCell(this);
        }

        public void DestroyItem()
        {
            if (Item == null)
                return;

            Destroy(Item.gameObject);
            Item = null;
        }
    }
}