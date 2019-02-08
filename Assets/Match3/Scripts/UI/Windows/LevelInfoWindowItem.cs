using TestCompany.Match3.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace TestCompany.Match3.UI
{
    public class LevelInfoWindowItem : MonoBehaviour
    {
        public Dto.Item Item { get; private set; }

        [SerializeField] private Image _icon;

        [SerializeField] private Text _text;

        public void Initialize(Dto.Item item)
        {
            Item = item;
            _icon.sprite = SpriteHelper.GetItemIcon(item.StaticItem);
            _text.text = Item.Amount.ToString();
        }
    }
}