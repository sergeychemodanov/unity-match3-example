using System.Collections.Generic;
using System.Linq;

namespace TestCompany.Match3.Static
{
    public class DefaultStaticDataProvider : IStaticDataProvider
    {
        public List<LevelData> Levels { get; }

        public List<Item> Items { get; private set; }

        public DefaultStaticDataProvider()
        {
            Levels = new List<LevelData>();
            Items = new List<Item>();

            CreateTestItems();
            CreateTestLevel();
        }

        private void CreateTestLevel()
        {
            const int xSize = 5;
            const int ySize = 6;
            const int xObstacleIndex = 2;
            const int yObstacleIndex = 2;

            var cells = new CellType[xSize, ySize];

            for (var x = 0; x < xSize; x++)
            {
                for (var y = 0; y < ySize; y++)
                {
                    var cellType = y == ySize - 1
                        ? CellType.SpawnPoint
                        : x == xObstacleIndex && y == yObstacleIndex
                            ? CellType.Obstacle
                            : CellType.Default;

                    cells[x, y] = cellType;
                }
            }

            var firstTarget = new Dto.Item(Items.First(), 10);
            var targets = new List<Dto.Item> { firstTarget };

            var limitedMovesLevel = new LevelData(1, cells, LoseConditionType.LimitedMoves, 15, targets);
            var limitedTimeLevel = new LevelData(2, cells, LoseConditionType.LimitedTime, 30, targets);

            Levels.Add(limitedMovesLevel);
            Levels.Add(limitedTimeLevel);
        }

        private void CreateTestItems()
        {
            Items = new List<Item>
            {
                new Item("bean_blue", ItemType.Default),
                new Item("bean_green", ItemType.Default),
                new Item("bean_orange", ItemType.Default),
                new Item("bean_pink", ItemType.Default),
                new Item("bean_purple", ItemType.Default)
            };
        }
    }
}