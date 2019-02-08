using UnityEngine;
using TestCompany.Common;
using TestCompany.Match3.Utils;

namespace TestCompany.Match3.Core
{
    public class Item : MonoBehaviour
    {
        public event EventHandler<Item, Vector2Int> OnMove;

        public Static.Item StaticItem { get; private set; }

        public Cell Cell { get; private set; }

        [SerializeField] private SpriteRenderer _icon;

        private DragHandler _dragHandler;

        public void Initialize(Static.Item staticItem)
        {
            StaticItem = staticItem;
            _icon.sprite = SpriteHelper.GetItemIcon(staticItem);
        }

        public void SetCell(Cell cell)
        {
            Cell = cell;
            transform.SetParent(Cell.transform);
        }

        private void Start()
        {
            _dragHandler = gameObject.AddComponent<DragHandler>();
            _dragHandler.OnMove += direction => { OnMove?.Invoke(this, direction); };
        }
    }
}