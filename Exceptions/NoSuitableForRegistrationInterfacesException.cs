using System;

namespace Pryanik.UnityMediator.Exceptions
{
    public class NoSuitableForRegistrationInterfacesException : Exception
    {
        public NoSuitableForRegistrationInterfacesException(Type type) : base($"No suitable for registration interfaces were found while registering {type.FullName}") {}
    }
}