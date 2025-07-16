using Pryanik.UnityMediator.Signals;

namespace Pryanik.UnityMediator.SignalHandlers
{
    public interface ISignalHandler<in T> 
        where T : Signal
    {
        void HandleSignal(T signal);
    }
}