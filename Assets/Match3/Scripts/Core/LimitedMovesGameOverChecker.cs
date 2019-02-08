using TestCompany.Common;

namespace TestCompany.Match3.Core
{
    public class LimitedMovesGameOverChecker : IGameOverChecker
    {
        public event EventHandler OnGameOver;

        public event EventHandler<int> OnRemainingValueChanged;

        public int RemainingValue { get; private set; }

        private readonly CellsField _cellsField;


        public LimitedMovesGameOverChecker(CellsField cellsField, int startMovesCount)
        {
            _cellsField = cellsField;
            _cellsField.OnPlayerMadeMove += OnPlayerMadeMove;
            RemainingValue = startMovesCount;
        }

        public void Clear()
        {
            if (_cellsField != null)
                _cellsField.OnPlayerMadeMove -= OnPlayerMadeMove;
        }


        private void OnPlayerMadeMove()
        {
            if (RemainingValue <= 0)
                return;

            RemainingValue--;
            OnRemainingValueChanged?.Invoke(RemainingValue);

            if (RemainingValue > 0)
                return;

            OnGameOver?.Invoke();
            Clear();
        }
    }
}