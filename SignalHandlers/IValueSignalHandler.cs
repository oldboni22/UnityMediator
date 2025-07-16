using Pryanik.UnityMediator.Signals;

namespace Pryanik.UnityMediator.SignalHandlers
{
    public interface IValueSignalHandler<in T1, out T2> 
        where T1 : ValueSignal<T2>
    {
        T2 HandleSignal(T1 signal);
    }
}