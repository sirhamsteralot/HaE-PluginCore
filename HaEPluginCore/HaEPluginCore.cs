using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.IO;
using Sandbox;
using VRage.Plugins;
using VRage.Collections;
using VRage.FileSystem;


namespace HaEPluginCore
{
    public class HaEPluginCore : IPlugin
    {
        public static HaEInputHandler HaEInputHandler;
        public static HaEPluginLoader HaEPluginLoader;

        public static MySandboxGame instance;

        public static Action OnInit;
        public static Action OnUpdate;
        public static Action OnDispose;
        
        public void Init(object gameInstance)
        {
            instance = (MySandboxGame)gameInstance;
            HaEInputHandler = new HaEInputHandler();
            HaEPluginLoader = new HaEPluginLoader();

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
