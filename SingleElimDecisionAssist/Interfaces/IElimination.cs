using SingleElimDecisionAssist.Enums;
using System;
using System.Collections.Generic;

namespace SingleElimDecisionAssist.Interfaces
{
#nullable enable
    public interface IElimination<T>
    {
        bool HasWinner { get; }
        int Round { get; }
        bool Shuffle { get; set; }
        Action<(T, T)>? NextCallback { get; set; }
        Action<T>? WinnerCallback { get; set; }
        void LoadNew(IEnumerable<T> items);
        void Choose(ElimChoice choice);
    }

}
