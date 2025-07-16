using System;
using Pryanik.UnityMediator.Factory;
using Pryanik.UnityMediator.Factory.Pool;
using Pryanik.UnityMediator.SignalHandlers;
using Pryanik.UnityMediator.Signals;
using UnityEngine;

namespace Pryanik.UnityMediator.SceneBinding
{
    public abstract class MediatorMonoInstaller : MonoBehaviour
    {
        internal MediatorBuilder Builder { get; set; }
        public abstract void BindSignalHandlers();

        protected void RegisterSignal<T>(ISignalHandler<T> handler)
            where T : Signal
        {
            Builder.RegisterSignal(handler);
        }

        protected void RegisterValueSignal<T1, T2>(IValueSignalHandler<T1, T2> handler)
            where T1 : ValueSignal<T2>
        {
            Builder.RegisterValueSignal(handler);
        }

        protected void RegisterAllSignalsFrom<T>(T handler)
        {
            Builder.RegisterAllSignalsFrom(handler);
        }

        protected void RegisterFactory<T>(Func<FactoryValueSignal<T>, T> func)
        {
            Builder.RegisterFactory(func);
        }

        protected void RegisterPool<T>(Func<PoolValueSignal<T>, T> func)
        {
            Builder.RegisterPool(func);
        }
    }
}