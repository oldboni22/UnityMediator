using System;
using Pryanik.UnityMediator.Signals;

#nullable enable
namespace Pryanik.UnityMediator.Factory.Pool
{
    public abstract class PoolReturnSignal<T> : Signal
    {
        public T Item { get; }
        public Action<T>? ResetAction { get; }

        protected PoolReturnSignal(T item, Action<T>? resetAction = null)
        {
            Item = item;
            ResetAction = resetAction;
        }
    }
}