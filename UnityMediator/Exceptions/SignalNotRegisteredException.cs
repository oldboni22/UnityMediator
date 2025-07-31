using System;
using Pryanik.UnityMediator.Signals;

namespace Pryanik.UnityMediator.Exceptions
{
    public class SignalNotRegisteredException<T> : Exception
        where T : Signal
    {
        public SignalNotRegisteredException() :
            base($"A signal of type {typeof(T).FullName} was not registered!")
        {
            
        }
    }
}