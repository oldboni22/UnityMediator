using System;
using Pryanik.UnityMediator.SceneBinding;
using Pryanik.UnityMediator.SignalHandlers;

namespace Pryanik.UnityMediator.Factory
{
    internal class FactoryValueSignalHandler<T> : IValueSignalHandler<FactoryValueSignal<T>,T>
    {
        private readonly SceneMediatorContext _context;
        private readonly Func<FactoryValueSignal<T>, T> _createFunc;

        public FactoryValueSignalHandler(SceneMediatorContext context, Func<FactoryValueSignal<T>, T> createFunc)
        {
            _context = context;
            _createFunc = createFunc;
        }

        public T HandleSignal(FactoryValueSignal<T> signal)
        {
            var instance = _createFunc.Invoke(signal);
            _context.AssignMediatorAttribute(instance);

            return instance;
        }
    }
}