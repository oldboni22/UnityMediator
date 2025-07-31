using Pryanik.UnityMediator.Signals;

namespace Pryanik.UnityMediator.Factory
{
    #region Description
    /// <summary>
    /// Value signal used for getting item from factory.
    /// </summary>
    /// <typeparam name="T">Factory item type.</typeparam>
    #endregion
    public abstract class FactoryValueSignal<T> : ValueSignal<T>
    {
        
    }
}