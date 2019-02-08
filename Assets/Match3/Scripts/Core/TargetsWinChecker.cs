using System.Collections.Generic;
using System.Linq;
using TestCompany.Common;

namespace TestCompany.Match3.Core
{
    public class TargetsWinChecker : IWinChecker
    {
        public event EventHandler OnWin;

        public event EventHandler<List<Dto.Item>> OnTargetsChange;

        public List<Dto.Item> Targets { get; }

        private readonly CellsField _cellsField;


        public TargetsWinChecker(CellsField cellsField, List<Dto.Item> targets)
        {
            _cellsField = cellsField;
            _cellsField.OnItemDestroyed += OnItemDestroyed;
            Targets = targets;
        }

        public void Clear()
        {
            if (_cellsField != null)
                _cellsField.OnItemDestroyed -= OnItemDestroyed;
        }


        private void OnItemDestroyed(Static.Item staticItem)
        {
            var itemIndex = Targets.FindIndex(i => i.StaticItem.Id == staticItem.Id);
            if (itemIndex < 0)
                return;

            var item = Targets[itemIndex];
            if (item.Amount <= 0)
                return;

            item.Amount--;
            Targets[itemIndex] = item;

            OnTargetsChange?.Invoke(Targets);

            var allItemsDestroyed = Targets.All(i => i.Amount <= 0);
            if (!allItemsDestroyed)
                return;

            Clear();
            OnWin?.Invoke();
        }
    }
}