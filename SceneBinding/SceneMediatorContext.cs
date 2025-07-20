using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Pryanik.UnityMediator.Global;
using Pryanik.UnityMediator.SceneInvocationManagement;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Pryanik.UnityMediator.SceneBinding
{
    public sealed class SceneMediatorContext : MonoBehaviour
    {
        private static readonly Dictionary<Type, FieldInfo> _cachedFieldInfo = new(); 
        
        [SerializeField] private MediatorMonoInstaller[] _installers;
        
        [SerializeField] private bool _addInvocationController;
        
        private Mediator _mediator = new Mediator();
        
        private void Awake()
        { 
            var allInstallers = new List<MediatorMonoInstaller>();
            allInstallers.AddRange(GlobalMediatorContext.Installers);
            allInstallers.AddRange(_installers);
            
            BindSignals(allInstallers);
            
            if(_addInvocationController)
                BindInvocation(allInstallers);
        }

        private void BindInvocation(IEnumerable<MediatorMonoInstaller> installers)
        {
            var gameObj = new GameObject("InvocationController");
           
            gameObj.transform.SetParent(transform);
            var controller = gameObj.AddComponent<SceneInvocationController>();

            var builder = SceneInvokerBuilder.GetBuilder(controller,this);
            
            Parallel.ForEach(installers, installer =>
            {
                installer.InvokerBuilder = builder;
                installer.BindInvocationOrder();
            });
            
            builder.SetCollections();
            controller.InvokeAwake();
        }
        
        private void BindSignals(IEnumerable<MediatorMonoInstaller> installers)
        {
            var builder = MediatorBuilder.GetBuilder(_mediator,this);
            
            Parallel.ForEach(installers, installer =>
            {
                installer.MediatorBuilder = builder;
                installer.BindSignalHandlers();
            });
            
            _mediator.SetDictionary(builder.GetDictionary());
            
            Parallel.ForEach(GameObject.FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None),
                AssignMediatorAttribute);
        }
        
        internal void AssignMediatorAttribute<T>(T instance)
        {
            if(instance == null)
                return;
            
            var type = instance.GetType();

            if (_cachedFieldInfo.TryGetValue(type, out var field) is false)
            {
                field = type.GetFields(
                        BindingFlags.Instance |
                        BindingFlags.Public |
                        BindingFlags.NonPublic).
                        FirstOrDefault(f =>
                        Attribute.IsDefined(f, typeof(MediatorAttribute)) &&
                        f.FieldType == typeof(Mediator));


                _cachedFieldInfo.Add(type, field);
            }
            
            if (field == null || field.GetValue(instance) == null)
                return;
            
            field.SetValue(instance,_mediator);
        }
    }
}