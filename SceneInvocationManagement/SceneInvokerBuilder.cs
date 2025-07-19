using System.Collections.Concurrent;
using System.Linq;

namespace Pryanik.UnityMediator.SceneInvocationManagement
{
    class InvocationEntry
    {
        internal object Value;
        internal ushort Priority;
    }
    
    internal class SceneInvokerBuilder
    {
        private SceneInvokerBuilder(SceneInvocationController controller) => _controller = controller;
        
        private readonly SceneInvocationController _controller;
        
        private readonly ConcurrentBag<InvocationEntry> _awakes = new ConcurrentBag<InvocationEntry>();
        private readonly ConcurrentBag<InvocationEntry> _starts = new ConcurrentBag<InvocationEntry>();
        private readonly ConcurrentBag<InvocationEntry> _updates = new ConcurrentBag<InvocationEntry>();
        private readonly ConcurrentBag<InvocationEntry> _lateUpdates = new ConcurrentBag<InvocationEntry>();
        private readonly ConcurrentBag<InvocationEntry> _fixedUpdates = new ConcurrentBag<InvocationEntry>();

        internal static SceneInvokerBuilder GetBuilder(SceneInvocationController controller)
            => new SceneInvokerBuilder(controller);
        
        internal void AddAwake(IAwake item, ushort priority) => _awakes.Add( new InvocationEntry()
        {
            Value = item,
            Priority = priority
        });
        internal void AddStart(IStart item, ushort priority) => _starts.Add(new InvocationEntry()
        {
            Value = item,
            Priority = priority
        });
        internal void AddUpdate(IUpdate item, ushort priority) => _updates.Add(new InvocationEntry()
        {
            Value = item,
            Priority = priority
        });
        internal void AddLateUpdate(ILateUpdate item, ushort priority) => _lateUpdates.Add(new InvocationEntry()
        {
            Value = item,
            Priority = priority
        });
        internal void AddFixedUpdate(IFixedUpdate item, ushort priority) => _fixedUpdates.Add(new InvocationEntry()
        {
            Value = item,
            Priority = priority
        });

        internal void SetCollections()
        {
            _controller.SetCollections
            (
                _awakes.OrderBy(entry => entry.Priority).Select(a => a.Value as IAwake),
                _starts.OrderBy(entry => entry.Priority).Select(a => a.Value as IStart),
                _updates.OrderBy(entry => entry.Priority).Select(a => a.Value as IUpdate),
                _lateUpdates.OrderBy(entry => entry.Priority).Select(a => a.Value as ILateUpdate),
                _fixedUpdates.OrderBy(entry => entry.Priority).Select(a => a.Value as IFixedUpdate)
            );
        }
    }
}