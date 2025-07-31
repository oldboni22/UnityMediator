using System;
using Pryanik.UnityMediator.Signals;

namespace Pryanik.UnityMediator.Exceptions
{
    public class ValueSignalNotRegisteredException<T1,T2> : Exception
        where T1 : ValueSignal<T2>
    {
        public ValueSignalNotRegisteredException() :
            base($"A value signal of type {typeof(T1).FullName} was not registered!")
        {
            
        }
    }
}