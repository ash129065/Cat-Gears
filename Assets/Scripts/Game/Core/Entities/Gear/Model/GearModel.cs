using UnityEngine;
using System.Collections.Generic;

namespace Game.Core.Entities.Gear.Model
{
    public class GearModel : MonoBehaviour
    {
        private string posId;

        private string powerSourceGearId;
        private List<string> powerRecievedGearIds;
        
        public bool IsPowered { get; private set; }
        
        public GearModel()
        {
            
        }

        public void Init(string posId)
        {
            this.posId = posId;
        }
        
        public void SetIsPowered(bool isPowered)
        {
            IsPowered = isPowered;
        }
    }
}
