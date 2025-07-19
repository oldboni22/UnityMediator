UnityMediator is a framework that implements the Mediator pattern in a manner that will be very familliar for ZenJect users.

<br>

<B><I>Development Notes:</I></B>
<ul>
  <li>This project was started as a learning exercise.</li>
  <li>It does not represent production-level architecture but is aimed at learning and experimentation.</li>
  <li>Contributions and feedback are welcome, but the project may remain simple by design, or even be discontinued.</li>
</ul>

<br>

<B><I>What advantages and disadvantages does mediator introduce ? </I></B>>
<ul>
  <li><B>Advantages</B>
    <ul>
      <li>Removes any class coupling.</li>
      <li>Eliminates circular dependencies.</li>
    </ul>
  </li>
  <li><B>Disadvantages</B>
    <ul>
      <li>Every action/function call requires a pair of signal/handler classes that represent/handle that call.</li>
      <li>The mediator becomes the "god object". While direct couplig is eliminated completely, any objects, that has any dependencies must rely on the mediator object.</li>
    </ul>
  </li>
</ul>    

![smilingbucktooth-3094559865](https://github.com/user-attachments/assets/de3f141b-985c-459a-ba60-1ceb983cc67f)

<B>How to use?</B>
<ul>
  <li>
    <B>1. PREPARATION </B>
      <ul>
        <li>
          Start by creating an empty object on your scene. Attach a <B>SceneMediatorContext</B> to it. <br><br>
          <img width="361" height="297" alt="image" src="https://github.com/user-attachments/assets/afb57e84-e71a-4527-bf31-95877283744d" /><br><br>
          The <B>SceneMediatorContext</B> is simmilar to Zenject's <B>Scene context</B>. 
          It Executes all the provided <B>MediatorMonoInstallers</B> as well as installers thar are stored in <B>GlobalMediatorContext</B>. <br>
          <B>This part is the most crucial one. Without it, the mediator won't be instatiated.</B>
        </li>
        <li>
          Then you'll need to register desired signals with their handlers.<br>
          Let's start with creating the signal : <br>
         <code>public class ExampleSignal : Signal{}</code> <br>
          Any signal has to inhert from the <B>Signal</B> class.<br>
          If you need to <B>pass some parameters</B> to the handler, list them in the signal class : <br><br>
          <code>public class ExampleSignal : Signal
    {                                 
        public int IntParameter;      
        public string StringParameter;
    }                                 </code> <br><br>
          You can use any method of organizing the signal class, such as <B>Properties, Constructors</B>, etc. The code below will work just as fine: <br><br>
          <code>public class ExampleSignal : Signal
    {                                          
        public int IntParameter { get; }       
        public string StringParameter { get; } 
        public ExampleSignal(int intParameter, string stringParameter)
        {                                      
            IntParameter = intParameter;       
            StringParameter = stringParameter; 
        }                                      
    }                                         </code> <br><br>
        </li>
        <li>
          Great! Now, let's create a handler for our signal : <br><br>
          <code>public class ExampleSignalHandler : ISignalHandler&lt;ExampleSignal&gt;
    {
        public void HandleSignal(ExampleSignal signal)
        {                                                                
            Debug.Log($"int parameter = {signal.IntParameter}.");        
            Debug.Log($"string parameter = {signal.StringParameter}.");  
        }                                                                
    }                                                                   </code><br><br>
          As you can see, we can <B>access all the required parameters from the signal instance</B>.
          Also note, that if you need one object to handle multiple signals, you can implement this by inheriting <B>ISignalHandler interface</B> multiple times : <br><br>
            <code>{
    public class ExampleSignalHandler : ISignalHandler&lt;ExampleSignal&gt;, ISignalHandler&lt;ExampleSignal2&gt;
    {                                                       
        public void HandleSignal(ExampleSignal signal)
        {                                                              
            Debug.Log($"int parameter = {signal.IntParameter}."); 
            Debug.Log($"string parameter = {signal.StringParameter}."); 
        }                                                                
        public void HandleSignal(ExampleSignal2 signal) {}             
    }                                                                 
}             </code><br><br>
        </li>
        <li>
          Now we have the signal and the handler, nice. <br>
          Let's <B>Register</B> the signal and the handler. As mentioned above, we will need a <B>MediatorInstaller</B> class.
          First, create a class that implements the <B>MediatorInstaller</B>. Then, <B>using the protected methods of MediatorInstaller</B>, register all your signals and their handlers. <Br>
          The registration methods are: <br>
          <ul>
            <li><code>protected void RegisterSignal&lt;T&gt;(ISignalHandler&lt;T&gt; handler) where T : Signal</code><br>
            Registers the signal of type T.</li>
            <li><code>protected void RegisterValueSignal&lt;T1&gt,&lt;T2&gt;(IValueSignalHandler&lt;T1&gt;&lt;T2&gt handler) where T1 : ValueSignal&lt;T2&gt</code><br>
            Registers the value signal of type T1 with return value type of T2.</li>
            <li><code>protected void RegisterAllSignalsFrom&lt;T&gt;(&lt;T&gt; handler)</code><br>
            Registers all the signals (both Signal and ValueSignal), from the interfaces <B>of the provided generic T</B>.</li>
            <li><code>protected void RegisterFactory&lt;T&gt;(Func&lt;FactoryValueSignal&lt;T&gt;, T&gt; func)</code><br>
            Registers a factory as a IValueSignalHandler. The main use is when you need to provide the created objects with thr Mediator object.</li>
            <li><code>protected void RegisterPool&lt;T&gt;(Func&lt;PoolValueSignal&lt;T&gt;, T&gt; func)</code><br>
            Registers a pool as a IValueSignalHandler. Works exactly as the factory, while also introducing reusability of the created objects.</li>
          </ul>
          We will talk about factories and pools later. For now, let's continue our example registration : <br><br>
          <code>public class ExampleInstaller : MediatorMonoInstaller
    {
        public override void BindSignalHandlers()
        {
            var handlerInstance = new ExampleSignalHandler();
            RegisterSignal<ExampleSignal>(handlerInstance);
        }
    }</code><br><br>
          Note, that all of the registration logic should be invoked in <B>BindSignalHandlers</B>.<br> 
          If you want to organise the binding process, you can use diffrent <B>MediatorInstaller</B> classes. 
          But you can also split the binding within a single installer with different registration methods, then you should call all of them from the <B>BindSignalHandlers method : </B> <br><br>
          <code>public class ExampleInstaller : MediatorMonoInstaller
    {
        public override void BindSignalHandlers()
        {
            BindGroupA();
            BindGroupB();
        }
        private void BindGroupA()
        {
            var handlerInstance = new ExampleSignalHandler();
            RegisterSignal<ExampleSignal>(handlerInstance);
        }
        private void BindGroupB()
        {
            var handlerInstance2 = new ExampleSignalHandler2();
            RegisterSignal<ExampleSignal2>(handlerInstance2);
        }
    }</code><br><br>
    The final step is creating the object with an instance of our Example installer.
    We need to attach the installer script to our object and then add the object to the list of Installers in our <B>SceneMediatorContext</B> : <br><br>
    <img width="364" height="237" alt="image" src="https://github.com/user-attachments/assets/868736bf-8b45-4615-8c23-cfc3c3d107fa" /> <br><br>
    <img width="363" height="194" alt="image" src="https://github.com/user-attachments/assets/20a2487c-35a5-4741-b731-1d562c3fa95c" /> <br><br>
    Nice! the preparation is complete. Now the context will register our signal and handler on Awake(). 
              <B>Please note, that all the objects, that rely on the mediator, should have less priority in the execution order, than the MediatorSceneContext.</B>
        </li>
      </ul>
  </li>
  <li>
    <B>2. USING THE MEDIATOR</B><br>
    <ul>
      <li>
    To use the mediator, our objects have to have a reference to the Mediator object. 
    To get the reference, you need to use the <B>[Mediator]</B> attribute. 
    It works exactly like the <B>[Inject]</B> attribute in Zenject.
    All the MonoBehaviour sctipts that have a field like <code>[Mediator] private _mediator;</code>, will get the reference to the Mediator object writen in that filed.
    Also, any object(not necessarily MonoBehaviour), that goes through the registration, automatically gets the [Mediator] field filled, if it has one.  
    As mentioned earlier, any object, created with built-in factory or pool, also gets the reference to the Mediator object.
      </li>
      <li>
        And knowing that, let's implement our example user:<br><br>
        <code>public class ExampleUser : MonoBehaviour
    {
        [Mediator] private Mediator _mediator;
        private void Update()
        {
            _mediator.SendSignal&lt;ExampleSignal&gt;(new ExampleSignal()
                {
                    IntParameter = 1, 
                    StringParameter = "123"
                });
        }
    }</code><br><br>
        And now, let's check it this works : <br><br>
        <img width="357" height="265" alt="image" src="https://github.com/user-attachments/assets/5d271aa3-0416-4ce8-b160-8be10d1509cb" /> <br><br>
        <img width="1155" height="285" alt="image" src="https://github.com/user-attachments/assets/a17fcd29-a401-47c4-960f-45197eada9c4" /> <br><br>
      And that's it. We now have a system, that allows objects to communicate between each other without any direct coupling.
      </li>
    <ul>
  </li>
</ul>

<br><br><br>
<b>To be continued : </b>
<ul>
  <li>ValueSignals</li>
  <li>Factories/Pools</li>
  <li>Global context</li>
  <li>Resource storages</li>
  <li>Scene invocation controll</li>
</ul>
