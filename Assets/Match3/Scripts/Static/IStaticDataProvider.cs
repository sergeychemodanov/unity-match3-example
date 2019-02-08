using System.Collections.Generic;

namespace TestCompany.Match3.Static
{
    public interface IStaticDataProvider
    {
        List<LevelData> Levels { get; }

        List<Item> Items { get; }
    }
}