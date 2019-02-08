using TestCompany.Match3.Core;
using UnityEngine;

namespace TestCompany.Match3.Utils
{
    public interface IItemCreator
    {
        Item Create(Transform container, Static.Item staticItem);
        Item CreateRandom(Transform container);
    }
}