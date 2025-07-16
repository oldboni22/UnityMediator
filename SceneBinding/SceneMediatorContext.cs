using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Pryanik.UnityMediator.Global;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Pryanik.UnityMediator.SceneBinding
{
    public sealed class SceneMediatorContext : MonoBehaviour
    {
        [SerializeField] private MediatorMonoInstaller[] _installers;

        private Mediator _mediator = new Mediator();
        
        private void Awake()
        {
            var builder = MediatorBuilder.GetBuilder(_mediator,this);


            var allInstallers = new List<MediatorMonoInstaller>();
            
            allInstallers.AddRange(GlobalMediatorContext.Installers);
            allInstallers.AddRange(_installers);

            Parallel.ForEach(allInstallers, installer =>
            {
                installer.Builder = builder;
                installer.BindSignalHandlers();
            });
            
            _mediator.SetDictionary(builder.GetDictionary());
            

            Parallel.ForEach(GameObject.FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None),
                AssignMediatorAttribute);

        }
        
        internal void AssignMediatorAttribute<T>(T instance)
        {
            var type = instance.GetType();

            var field = type.GetFields(
                BindingFlags.Instance |
                BindingFlags.Public |
                BindingFlags.NonPublic).
                FirstOrDefault(f =>
                Attribute.IsDefined(f, typeof(MediatorAttribute)) &&
                f.FieldType == typeof(Mediator) &&
                f.GetValue(instance) == null);
            
            if (field == null)
                return;
            
            field.SetValue(instance,_mediator);
        }
    }
}