using System;
using Pryanik.UnityMediator.Signals;

#nullable enable
namespace Pryanik.UnityMediator.Factory.Pool
{
    public abstract class PoolReturnSignal<T> : Signal
    {
        public T Item;
        public Action<T>? ResetAction;
    }
}