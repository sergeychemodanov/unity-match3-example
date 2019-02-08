using TestCompany.Common;

namespace TestCompany.Match3.Core
{
    public interface IGameOverChecker
    {
        event EventHandler OnGameOver;

        event EventHandler<int> OnRemainingValueChanged;

        int RemainingValue { get; }

        void Clear();
    }
}