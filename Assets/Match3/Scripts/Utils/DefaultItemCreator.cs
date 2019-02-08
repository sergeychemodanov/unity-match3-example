using UnityEngine;

namespace TestCompany.Match3.Utils
{
    public class DefaultItemCreator : IItemCreator
    {
        private readonly Static.IStaticDataProvider _staticDataProvider;

        public DefaultItemCreator(Static.IStaticDataProvider staticDataProvider)
        {
            _staticDataProvider = staticDataProvider;
        }

        public Core.Item Create(Transform container, Static.Item staticItem)
        {
            var prefab = Resources.Load<Core.Item>($"Items/{staticItem.Type}");
            var item = Object.Instantiate(prefab, container, false);
            item.gameObject.name = staticItem.Id;
            item.Initialize(staticItem);
            return item;
        }

        public Core.Item CreateRandom(Transform container)
        {
            var randomStaticItemIndex = Random.Range(0, _staticDataProvider.Items.Count);
            var randomStaticItem = _staticDataProvider.Items[randomStaticItemIndex];
            return Create(container, randomStaticItem);
        }
    }
}