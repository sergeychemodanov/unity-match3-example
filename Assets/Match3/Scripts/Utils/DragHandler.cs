using TestCompany.Common;
using UnityEngine;
using UnityEngine.EventSystems;

namespace TestCompany.Match3.Utils
{
    public class DragHandler : MonoBehaviour, IDragHandler
    {
        public event EventHandler<Vector2Int> OnMove;

        private const float DragThreshold = 5;

        public void OnDrag(PointerEventData eventData)
        {
            var direction = GetDirection(eventData.delta);
            if (direction != Vector2Int.zero)
                OnMove?.Invoke(direction);
        }

        private Vector2Int GetDirection(Vector2 delta)
        {
            if (delta.x > DragThreshold)
                return Vector2Int.right;

            if (delta.x < -DragThreshold)
                return Vector2Int.left;

            if (delta.y > DragThreshold)
                return Vector2Int.up;

            if (delta.y < -DragThreshold)
                return Vector2Int.down;

            return Vector2Int.zero;
        }
    }
}