using SingleElimDecisionAssist.Enums;
using SingleElimDecisionAssist.Extensions;
using SingleElimDecisionAssist.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SingleElimDecisionAssist.Models
{
#nullable enable
    public class SingleElim<T> : IElimination<T>
    {
        public bool HasWinner { get; private set; }
        public int Round { get; private set; }
        public bool Shuffle { get; set; }
        public Action<(T, T)>? NextCallback { get; set; } = null;
        public Action<T>? WinnerCallback { get; set; } = null;

        private List<T>? pool;
        private Stack<(T, T)> pairs = new Stack<(T, T)>();

        public void LoadNew(IEnumerable<T> items)
        {
            if (items == null || items.Count() == 0)
            {
                throw new ArgumentException("Empty or null item list");
            }
            if (items.Count() == 1)
            {
                HasWinner = true;
                WinnerCallback?.Invoke(items.First());
            }
            if (Shuffle)
            {
                pool = items.Shuffle().ToList();
            }
            else
            {
                pool = items.ToList();
            }
            HasWinner = false;
            Round = 0;
            pairs = new Stack<(T, T)>();
            NextPair();
        }

        public void Choose(ElimChoice choice)
        {
            if (pool == null)
            {
                throw new InvalidOperationException("Elimination must first be initialized");
            }
            if (HasWinner)
                return;
            var item = pairs.Pop();
            if (choice == ElimChoice.First)
            {
                pool.Add(item.Item1);
            }
            else
            {
                pool.Add(item.Item2);
            }
            Round++;
            NextPair();
        }

        private void NextPair()
        {
            if (pool!.Count == 1 && pairs.Count == 0)
            {
                HasWinner = true;
                WinnerCallback?.Invoke(pool.First());
                return;
            }
            if (pairs.Count == 0)
            {
                pairs = new Stack<(T, T)>(pool.SelectTwo((a, b) => (a, b)).Where((x, i) => i % 2 == 0));
                if (pool.Count % 2 != 0)
                {
                    var last = pool.Last();
                    pool.Clear();
                    pool.Add(last);
                }
                else
                {
                    pool.Clear();
                }
            }

            NextCallback?.Invoke(pairs.Peek());
        }
    }
}
