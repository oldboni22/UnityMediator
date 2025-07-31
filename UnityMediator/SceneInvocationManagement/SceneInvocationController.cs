using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace Pryanik.UnityMediator.SceneInvocationManagement
{
    internal sealed class SceneInvocationController : MonoBehaviour
    {
        private IAwake[] _awakes;
        private IStart[] _starts;
        
        private IUpdate[] _updates;
        private ILateUpdate[] _lateUpdates;
        private IFixedUpdate[] _fixedUpdates;

        internal void SetCollections(
            IEnumerable<IAwake> awakes,
            IEnumerable<IStart> starts,
            IEnumerable<IUpdate> updates,
            IEnumerable<ILateUpdate> lateUpdates,
            IEnumerable<IFixedUpdate> fixedUpdates)
        {
            _awakes = awakes.ToArray();
            _starts = starts.ToArray();
            _updates = updates.ToArray();
            _lateUpdates = lateUpdates.ToArray();
            _fixedUpdates = fixedUpdates.ToArray();
        }

        public void InvokeAwake()
        {
            foreach (var awake in _awakes)
            {
                awake.OnAwake();
            }
            _awakes = Array.Empty<IAwake>();
        }

        private async void Start()
        {
            while (_starts == null)
                await Task.Delay(100);
            
            foreach (var start in _starts)
            {
                start.OnStart();
            }
            _starts = Array.Empty<IStart>();
        }

        private void Update()
        {
            foreach (var upd in _updates)
            {
                upd.OnUpdate();
            }
        }

        private void LateUpdate()
        {
            foreach (var upd in _lateUpdates)
            {
                upd.OnLateUpdate();
            }
        }

        private void FixedUpdate()
        {
            foreach (var upd in _fixedUpdates)
            {
                upd.OnFixedUpdate();
            }
        }
    }
}