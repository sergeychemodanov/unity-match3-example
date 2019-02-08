using System.Collections.Generic;
using TestCompany.Common;

namespace TestCompany.Match3.Core
{
    public interface IWinChecker
    {
        event EventHandler OnWin;

        event EventHandler<List<Dto.Item>> OnTargetsChange;

        List<Dto.Item> Targets { get; }

        void Clear();
    }
}