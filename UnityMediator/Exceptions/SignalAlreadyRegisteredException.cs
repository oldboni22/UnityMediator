using System;

namespace Pryanik.UnityMediator.Exceptions
{
    public class SignalAlreadyRegisteredException : Exception
    {
        public SignalAlreadyRegisteredException(Type type) : base ($"The signal of type {type.FullName} was already registered.")
{}
    }
}