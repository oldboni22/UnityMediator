using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Pryanik.UnityMediator.Exceptions;
using Pryanik.UnityMediator.Factory;
using Pryanik.UnityMediator.Factory.Pool;
using Pryanik.UnityMediator.SceneBinding;
using Pryanik.UnityMediator.SignalHandlers;
using Pryanik.UnityMediator.Signals;
using UnityEngine;

namespace Pryanik.UnityMediator
{
    internal sealed class MediatorBuilder
    {
        private MediatorBuilder(Mediator mediator,SceneMediatorContext context)
        {
            _mediator = mediator;
            _context = context;
        }
        
        private readonly ConcurrentDictionary<Type, object> _dictionary = new();
        private readonly Mediator _mediator;
        private readonly SceneMediatorContext _context;
        
        public static MediatorBuilder GetBuilder(Mediator mediator,SceneMediatorContext context) =>
            new MediatorBuilder(mediator,context);
        
        internal Dictionary<Type,object> GetDictionary()
        {
            var dict = _dictionary.ToDictionary
            (
                pair => pair.Key,
                pair => pair.Value
            );
            return dict;
        }
        
        public void RegisterSignal<T>(ISignalHandler<T> handler)
            where T : Signal
        {
            if (_dictionary.ContainsKey(typeof(T)))
            {
                throw new SignalAlreadyRegisteredException(typeof(T));
            }
            
            _dictionary.TryAdd(typeof(T), handler);
            _context.AssignMediatorAttribute(handler);
        }
        
        public void RegisterValueSignal<T1,T2>(IValueSignalHandler<T1,T2> handler)
            where T1 : ValueSignal<T2>
        {
            if (_dictionary.ContainsKey(typeof(T1)))
            {
                throw new SignalAlreadyRegisteredException(typeof(T1));
            }

            _dictionary.TryAdd(typeof(T1), handler);
            _context.AssignMediatorAttribute(handler);
        }

        public void RegisterFactory<T>(Func<FactoryValueSignal<T>,T> func)
        {
            RegisterValueSignal(new FactoryValueSignalHandler<T>(_context,func));
        }
        
        public void RegisterPool<T>(Func<PoolValueSignal<T>,T> func)
        {
            RegisterAllSignalsFrom(new PoolValueSignalHandler<T>(_context,func));
        }
        
        public void RegisterAllSignalsFrom(object handler)
        {
            var validGenerics = new[] { typeof(ISignalHandler<>), typeof(IValueSignalHandler<,>) };
            
            var interfaces = handler.GetType().GetInterfaces().
                    Where(i => 
                        _dictionary.ContainsKey(i) is false &&
                        i.IsGenericType).
                    Where(i =>
                    {
                        var type = i;
                        while(type != null && type != typeof(object))
                        {
                            if (validGenerics.Contains(type.GetGenericTypeDefinition()))
                                return true;
                            
                            type = type.BaseType;
                        }
                        
                        return false;
                    }).
                    AsParallel().
                    ToArray();
            
            if (interfaces.Length == 0)
            {
                throw new NoSuitableForRegistrationInterfacesException(handler.GetType());
            }

            var registerSignalMethodInfo = typeof(MediatorBuilder).GetMethod(nameof(RegisterSignal));
            var registerValueSignalMethodInfo = typeof(MediatorBuilder).GetMethod(nameof(RegisterValueSignal));
            
            Parallel.ForEach(interfaces, @interface =>
            {
                var genericArgs = @interface.GetGenericArguments();
                if (genericArgs.Length == 1)
                {
                    var registerMethodWithGeneric = registerSignalMethodInfo!.MakeGenericMethod(genericArgs);
                    registerMethodWithGeneric.Invoke(this, new object[] { handler });
                }
                else if (genericArgs.Length == 2)
                {
                    var registerMethodWithGeneric = registerValueSignalMethodInfo!.MakeGenericMethod(genericArgs);
                    registerMethodWithGeneric.Invoke(this, new object[] { handler });
                }
            });
        }
    }
}