using Game.Abstraction;
using Game.Data.Level;
using Game.Data.Level.Abstraction;
using Game.Interface;
using UnityEngine;

namespace Game.Level
{
    public interface IInterfaceBase {}
    
    public interface ILevelDataHandler : IInterfaceBase
    {
        ILevelConfig GetLevelConfig();
    }
    
    public class LevelManager : MonoBehaviour, IBase, IDataLoader, ILevelDataHandler
    {
        [SerializeField] private LevelDataCollectionSo levelDataCollectionSo;


        public void InitBase()
        {
            InterfaceManagerMain.Instance.RegisterInterfaceInstance<LevelManager>(this);
            BaseInterfaceManager.Instance.RegisterInterfaceInstance<ILevelDataHandler>(this);
        }

        public void InitDataAndDependencies()
        {
            throw new System.NotImplementedException();
        }

        public ILevelConfig GetLevelConfig()
        {
            return levelDataCollectionSo.GetLevelConfig(1);
        }
    }
}
