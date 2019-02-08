using System.Collections.Generic;

namespace TestCompany.Match3.Static
{
    public struct LevelData
    {
        public int Number;

        public CellType[,] Cells;

        public LoseConditionType LoseConditionType;

        public int LoseConditionValue;

        public List<Dto.Item> Targets;

        public LevelData(int number, CellType[,] cells, LoseConditionType loseConditionType, int loseConditionValue, List<Dto.Item> targets)
        {
            Number = number;
            Cells = cells;
            LoseConditionType = loseConditionType;
            LoseConditionValue = loseConditionValue;
            Targets = targets;
        }
    }
}