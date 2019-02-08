using UnityEngine;

namespace TestCompany.Common
{
    public static class Extensions
    {
        public static Vector3 ToVector3(this Vector2Int vector2Int)
        {
            return new Vector3(vector2Int.x, vector2Int.y);
        }

        public static Transform SetXPosition(this Transform transform, float val)
        {
            var position = transform.position;
            position.x = val;
            transform.position = position;
            return transform;
        }
        
        public static Transform SetYPosition(this Transform transform, float val)
        {
            var position = transform.position;
            position.y = val;
            transform.position = position;
            return transform;
        }
        
        public static Transform SetZPosition(this Transform transform, float val)
        {
            var position = transform.position;
            position.z = val;
            transform.position = position;
            return transform;
        }

        public static void ClearChildObjects(this Transform transform)
        {
            foreach (Transform child in transform)
                Object.Destroy(child.gameObject);
        }
    }
}