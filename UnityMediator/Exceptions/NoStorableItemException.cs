using System;

namespace Pryanik.UnityMediator.Exceptions
{
    #region Description
    /// <typeparam name="TKey">Key type.</typeparam>
    /// <typeparam name="TValue">Value type.</typeparam>
    #endregion
    public class NoStorableItemException<TKey,TValue> : Exception
    {
        public NoStorableItemException(TKey key) : 
            base($"No storable item of type {typeof(TValue).FullName} with key = {key.ToString()} was found.")
        {
            
        }
    }
}