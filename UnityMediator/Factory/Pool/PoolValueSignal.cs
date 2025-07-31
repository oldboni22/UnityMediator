using Pryanik.UnityMediator.Signals;

namespace Pryanik.UnityMediator.Factory.Pool
{
    #region Description
    /// <summary>
    /// Signal used for getting item from the pool.
    /// </summary>
    /// <typeparam name="T">Pool item type.</typeparam>
    #endregion
    public abstract class PoolValueSignal<T> : ValueSignal<T>
    {
        
    }
}