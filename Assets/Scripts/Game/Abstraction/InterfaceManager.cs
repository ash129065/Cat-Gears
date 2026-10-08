using UnityEngine;
using System.Collections.Generic;

namespace Game.Interface
{
    public class InterfaceManagerMain
    {
        private static Dictionary<string, IBase> interfacesDictionary = new Dictionary<string, IBase>();
    
        #region Singleton
        public static InterfaceManagerMain Instance { get; private set; }
    
        public static void InitMainInstance()
        {
            if (Instance == null)
            {
                Instance = new InterfaceManagerMain();
            }
        }
    
        public InterfaceManagerMain()
        {
            interfacesDictionary = new Dictionary<string, IBase>();
        }
        #endregion
    
        public void RegisterInterfaceInstance<T>(IBase interfaceInst) where T : IBase
        {
            string interfaceType = typeof(T).ToString();
    
            if (!interfacesDictionary.ContainsKey(interfaceType))
            {
                interfacesDictionary.Add(interfaceType, interfaceInst);
            }
            else
            {
                interfacesDictionary[interfaceType] = interfaceInst;
            }
        }
    
        public T GetInterfaceInstance<T>() where T : IBase
        {
            string interfaceType = typeof(T).ToString();
    
            if (interfacesDictionary.ContainsKey(interfaceType))
                return (T)interfacesDictionary[interfaceType];
    
            return default;
        }
    }

}