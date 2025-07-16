using System;
using System.Collections.Generic;
using Pryanik.UnityMediator.SceneBinding;
using Pryanik.UnityMediator.SignalHandlers;

namespace Pryanik.UnityMediator.Factory.Pool
{
    public class PoolValueSignalHandler<T> : 
        IValueSignalHandler<PoolValueSignal<T>,T>, 
        ISignalHandler<PoolReturnSignal<T>>
    {
        private readonly Stack<T> _stack = new();
        private readonly SceneMediatorContext _context;
        private readonly Func<PoolValueSignal<T>, T> _createFunc;

        public PoolValueSignalHandler(SceneMediatorContext context, Func<PoolValueSignal<T>, T> createFunc)
        {
            _context = context;
            _createFunc = createFunc;
        }

        public T HandleSignal(PoolValueSignal<T> signal)
        {
            if (_stack.TryPop(out var result) is false)
            {
                var instance = _createFunc.Invoke(signal);
                _context.AssignMediatorAttribute(instance);

                return instance;
            }

            return result;
        }

        public void HandleSignal(PoolReturnSignal<T> signal)
        {
            var item = signal.Item;
            
            signal.ResetAction?.Invoke(item);
            _stack.Push(item);
        }
    }
}