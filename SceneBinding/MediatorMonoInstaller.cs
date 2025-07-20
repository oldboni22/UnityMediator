using System;
using Pryanik.UnityMediator.Factory;
using Pryanik.UnityMediator.Factory.Pool;
using Pryanik.UnityMediator.SceneInvocationManagement;
using Pryanik.UnityMediator.SignalHandlers;
using Pryanik.UnityMediator.Signals;
using UnityEngine;

namespace Pryanik.UnityMediator.SceneBinding
{
    public enum SceneInvocationEvent
    {
        Awake,
        Start,
        Update,
        LateUpdate,
        FixedUpdate
    }
    
    public abstract class MediatorMonoInstaller : MonoBehaviour
    {
        internal MediatorBuilder MediatorBuilder { get; set; }
        internal SceneInvokerBuilder InvokerBuilder { get; set; }
        

        #region SignalBinding

        public virtual void BindSignalHandlers()
        {
            
        }
        
        protected void RegisterSignal<T>(ISignalHandler<T> handler)
            where T : Signal
        {
            MediatorBuilder.RegisterSignal(handler);
        }

        protected void RegisterValueSignal<T1, T2>(IValueSignalHandler<T1, T2> handler)
            where T1 : ValueSignal<T2>
        {
            MediatorBuilder.RegisterValueSignal(handler);
        }

        protected void RegisterAllSignalsFrom<T>(T handler)
        {
            MediatorBuilder.RegisterAllSignalsFrom(handler);
        }

        protected void RegisterFactory<T>(Func<FactoryValueSignal<T>, T> func)
        {
            MediatorBuilder.RegisterFactory(func);
        }

        protected void RegisterPool<T>(Func<PoolValueSignal<T>, T> func)
        {
            MediatorBuilder.RegisterPool(func);
        }
        #endregion


        #region InvocationBinding
        public virtual void BindInvocationOrder()
        {
            
        }

        protected void BindObjectInvocation<T>(T item, SceneInvocationEvent @event, ushort priority = 100)
            where T : IUnitySceneInvoke

        {
            switch (@event)
            {
                case SceneInvocationEvent.Awake : 
                {
                    InvokerBuilder.AddAwake(item as IAwake, priority);
                    break;
                }
                case SceneInvocationEvent.Start:
                {
                    InvokerBuilder.AddStart(item as IStart, priority);
                    break;
                }
                case SceneInvocationEvent.Update:
                {
                    InvokerBuilder.AddUpdate(item as IUpdate, priority);
                    break;
                }
                case SceneInvocationEvent.LateUpdate:
                {
                    InvokerBuilder.AddFixedUpdate(item as IFixedUpdate, priority);
                    break;
                }
                case SceneInvocationEvent.FixedUpdate:
                {
                    InvokerBuilder.AddFixedUpdate(item as IFixedUpdate, priority);
                    break;
                }
            }
        }
        #endregion
    }
}