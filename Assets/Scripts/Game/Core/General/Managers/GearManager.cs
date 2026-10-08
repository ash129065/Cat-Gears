using Game.Abstraction;
using Game.Core.Entities.Gear.Model;
using Game.Core.General.Model;
using Game.Interface;
using Game.Level;
using UnityEngine;

namespace Game.Core.Managers
{
    public interface IGearHandler : IInterfaceBase
    {
        void AddGearData(GearModel gearModel);
        void RemoveGearData(string gearId);
    }
    
    public class GearManager : MonoBehaviour, IGearHandler
    {
        public GearDataModel GearDataModel { get; private set; }

        public void InitBase()
        {
            GearDataModel = new GearDataModel();
            
            BaseInterfaceManager.Instance.RegisterInterfaceInstance<IGearHandler>(this);
        }

        public void AddGearData(GearModel gearModel)
        {
            GearDataModel.Add(gearModel);
        }

        public void RemoveGearData(string gearId)
        {
            GearDataModel.Remove(gearId);
        }
    }
}
