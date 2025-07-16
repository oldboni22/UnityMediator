using UnityEngine;

namespace Pryanik.UnityMediator.Global.StorageBase
{
    #region Description 
/// <typeparam name="T1">Item type. Must be Serializable.</typeparam>
/// <typeparam name="T2">Key type. Must be Serializable.</typeparam>
    #endregion
    public abstract class Storable<T1,T2> : ScriptableObject
    {
        [SerializeField] private T1 _item;
        [SerializeField] private T2 _key;

        public T1 Item => _item;
        public T2 Key => _key;
    }
}