using Game.Abstraction;
using Game.Data.Base;
using Game.Interface;
using UnityEngine;

namespace Game.Loader
{
    public class BootLoader : MonoBehaviour
    {
        [Tooltip("Turn off to keep the board dormant until Load() is called, e.g. from a button.")]
        [SerializeField] private bool loadOnStart = true;
        
        [SerializeField] private GameObject[] baseManagers;
        [SerializeField] private BaseSo[] scriptableObjects;
        
        private void Start()
        {
            if (loadOnStart)
                Load();
        }
        
        public void Load()
        {
            BaseInterfaceManager.InitMainInstance();
            InterfaceManagerMain.InitMainInstance();
            
            InitializeScriptables();
            InitializeBaseManagers();
        }
        
        public void InitializeBaseManagers()
        {
            foreach (GameObject baseManager in baseManagers)
            {
                if (baseManager.TryGetComponent(out IBase foundManager))
                    foundManager.InitBase();
            }
        }
        
        private void InitializeScriptables()
        {
            foreach (BaseSo scriptableObject in scriptableObjects)
                scriptableObject.InitData();
        }
    }
}
