using System;
using System.Linq;
using Pryanik.UnityMediator.Exceptions;
using Pryanik.UnityMediator.SignalHandlers;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Pryanik.UnityMediator.Global.StorageBase
{
    #region Description
    /// <typeparam name="T1">Storable type.</typeparam>
    /// <typeparam name="T2">Storable item type.</typeparam>
    /// <typeparam name="T3">Storable key type.</typeparam>
    #endregion
    public abstract class StorageBase<T1,T2,T3> : ScriptableObject
        where T1 : Storable<T2,T3>
    {
        [SerializeField] private T1[] _items;

        protected T2 GetByKey(T3 key)
        {
            var item = _items.FirstOrDefault(i => i.Key.Equals(key));
           
            if (item == null)
                throw new NoStorableItemException<T3,T1>(key);

            return item.Item;
        }

        protected T2 GetRandom()
        {
            var item = _items.OrderBy(_ => Random.Range(-100,100)).First();
            return item.Item;
        }
    }
}