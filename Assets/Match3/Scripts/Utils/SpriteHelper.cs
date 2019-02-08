using UnityEngine;

namespace TestCompany.Match3.Utils
{
    public static class SpriteHelper
    {
        public static Sprite GetItemIcon(Static.Item staticItem)
        {
            return Resources.Load<Sprite>($"Icons/{staticItem.Id}");
        }
    }
}