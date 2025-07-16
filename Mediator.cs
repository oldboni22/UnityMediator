using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Pryanik.UnityMediator.Exceptions;
using Pryanik.UnityMediator.SignalHandlers;
using Pryanik.UnityMediator.Signals;
using UnityEngine;

namespace Pryanik.UnityMediator
{
    public sealed class Mediator
    {
        private Dictionary<Type, object> _dictionary;
        internal void SetDictionary(Dictionary<Type, object> dictionary) => _dictionary = dictionary;
        
        internal Mediator()
        {
        }

        public void SendSignal<T>(T signal) 
            where T : Signal
        {
            if (_dictionary.TryGetValue(typeof(T), out var value) is false)
            {
                throw new SignalNotRegisteredException<T>();
            }
            
            (value as ISignalHandler<T>)!.HandleSignal(signal);
        }

        public T2 SendValueSignal<T1, T2>(T1 signal)
            where T1 : ValueSignal<T2>
        {
            if (_dictionary.TryGetValue(typeof(T1), out var value) is false)
            {
                throw new ValueSignalNotRegisteredException<T1, T2>();
            }
            
            return (value as IValueSignalHandler<T1, T2>)!.HandleSignal(signal);
        }


    }
}