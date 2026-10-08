using System;
using Game.Data.Base;
using Game.Data.Level;

namespace Game.Data.General
{
    // TODO :: (1) :: lookup on how to use it
    
    [Serializable]
    public struct EngineDef
    {
        public Slot Slot;
        public Dir Dir;
    }
 
    [Serializable]
    public struct FrozenDef
    {
        public Slot Slot;       // anchor = top-left slot of the ice
        public GearSize Size;   // must match the held cat's size
        public CatType Cat;
        public int Level;       // 1-3 (default 1)
        public int Waves;       // thaw count, 1-4
    }
 
    [Serializable]
    public struct SpecialDef
    {
        public Slot Slot;
        public SpecialType Type;
        public int Repair;      // broken gears only; -1 = default (2 x current small price)
    }

    // TODO :: (1) 
    [Serializable]
    public struct PricedDef
    {
        public Slot Slot;
        public int Price;
    }
}
