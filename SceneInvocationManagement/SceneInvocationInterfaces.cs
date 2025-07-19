namespace Pryanik.UnityMediator.SceneInvocationManagement
{
    public interface IUnitySceneInvoke {}
    
    public interface IAwake : IUnitySceneInvoke
    {
        void OnAwake();
    }

    public interface IStart : IUnitySceneInvoke
    {
        void OnStart();
    }

    public interface IUpdate : IUnitySceneInvoke
    {
        void OnUpdate();
    }

    public interface ILateUpdate : IUnitySceneInvoke
    {
        void OnLateUpdate();
    }

    public interface IFixedUpdate : IUnitySceneInvoke
    {
        void OnFixedUpdate();
    }
}