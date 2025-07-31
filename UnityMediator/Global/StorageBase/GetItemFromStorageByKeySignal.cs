using Pryanik.UnityMediator.Signals;

namespace Pryanik.UnityMediator.Global.StorageBase
{
    #region Description
    ///<summary>Value signal used to get item from storage by its key.</summary>
    /// <typeparam name="T1">Storable type.</typeparam>
    /// <typeparam name="T2">Storable item type.</typeparam>
    /// <typeparam name="T3">Storable key type.</typeparam>
    #endregion
    public abstract class GetItemFromStorageByKeySignal<T1,T2,T3> : ValueSignal<T2>
        where T1 : Storable<T2,T3>
    {
        public T3 KeyValue;
    }
}