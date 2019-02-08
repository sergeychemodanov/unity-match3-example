using TestCompany.Common;

namespace TestCompany.Match3.Core
{
    public class LimitedTimeGameOverChecker : IGameOverChecker
    {
        public event EventHandler OnGameOver;

        public event EventHandler<int> OnRemainingValueChanged;

        public int RemainingValue { get; private set; }

        public LimitedTimeGameOverChecker(int startTime)
        {
            RemainingValue = startTime;
            Timer.Instance.OnOneSecondPassed += OnOneSecondPassed;
        }

        public void Clear()
        {
            if (Timer.Instance != null)
                Timer.Instance.OnOneSecondPassed -= OnOneSecondPassed;
        }

        private void OnOneSecondPassed()
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