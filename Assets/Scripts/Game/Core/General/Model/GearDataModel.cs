using System.Collections.Generic;
using Game.Core.Entities.Gear.Model;

namespace Game.Core.General.Model
{
    public class GearDataModel
    {
        private Dictionary<string, GearModel> gearModelDict = new();

        public void Add(GearModel gearModel)
        {
            gearModelDict[gearModel.GearPosId] = gearModel;
        }
        
        public void Remove(string gearId)
        {
            
        }
    }
}
