using System;
using UnityEngine;
using System.Collections.Generic;
using NUnit.Framework.Constraints;

namespace Game.Core.Entities.Gear.Model
{
    // TODO :: (1) :: double check list requirement and usage
    // TODO :: (̐2) :: inject gear data
    
    public class GearModel : MonoBehaviour
    {
        public string GearPosId { get; private set; }

        private string powerSourcerGearId; // the one who rotates this or the one who gave power to this gear for turning
        private List<string> powerProvidedToGearIds; // TODO :: (1)

        public Action OnModelStateChanged;
        
        public float RotationSpeed { get; private set; }
        public bool IsPowered { get; private set; }
        
        public GearModel()
        {
            RotationSpeed = 1f; // TODO :: (̐2)
        }

        public void Init(string posId)
        {
            GearPosId = posId;
        }
        
        public void SetIsPowered(bool isPowered)
        {
            IsPowered = isPowered;
        }
    }
}
