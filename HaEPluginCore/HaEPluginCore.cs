using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sandbox;
using VRage.Plugins;


namespace HaEPluginCore
{
    public class HaEPluginCore : IPlugin
    {
        public static MySandboxGame instance;

        public static Action OnInit;
        public static Action OnUpdate;
        public static Action OnDispose;
        
        public void Init(object gameInstance)
        {
            instance = (MySandboxGame)gameInstance;

            OnInit?.Invoke();
        }

        public void Update()
        {
            OnUpdate?.Invoke();
        }

        public void Dispose()
        {
            OnDispose?.Invoke();
        }
    }
}
