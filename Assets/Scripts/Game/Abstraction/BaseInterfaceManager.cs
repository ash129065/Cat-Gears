using System.Collections.Generic;
using Game.Interface;
using Game.Level;

namespace Game.Abstraction
{
    public class BaseInterfaceManager
    {
        private static Dictionary<string, IInterfaceBase> interfacesDictionary = new();
    
        #region Singleton
        public static BaseInterfaceManager Instance { get; private set; }
    
        public static void InitMainInstance()
        {
            if (Instance == null)
            {
                Instance = new BaseInterfaceManager();
            }
        }
    
        public BaseInterfaceManager()
        {
            interfacesDictionary = new();
        }
        #endregion
    
        public void RegisterInterfaceInstance<T>(IInterfaceBase interfaceInst) where T : IInterfaceBase
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
    
        public T GetInterfaceInstance<T>() where T : IInterfaceBase
        {
            string interfaceType = typeof(T).ToString();
    
            if (interfacesDictionary.ContainsKey(interfaceType))
                return (T)interfacesDictionary[interfaceType];
    
            return default;
        }
    }

}