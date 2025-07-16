using Pryanik.UnityMediator.Signals;

namespace Pryanik.UnityMediator.Global.StorageBase
{
    public abstract class GetRandomItemFromStorageSignal<T1,T2,T3> : ValueSignal<T2>
        where T1 : Storable<T2,T3>
    {
        
    }
}