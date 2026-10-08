using System;
using UnityEngine;

namespace Game.Data
{
    // TODO :: (1) :: can be used later down the road when we have a appropriate design for it (1)
    
    public enum Area { Desert, CityStreets, NightStreets }
    public enum SlotOrder { RowCol, ColRow }
    public enum Origin { TopLeft, BottomLeft }
    public enum Dir { Cw, Ccw } // clockwise, counter-clockwise
    public enum GearSize { Small, Big }
    public enum CatType { Club, Spear, Archer, Frost, Rider, Wizard }
    public enum SpecialType { Coin, Rusty, Broken }
    public enum RaiderType { Raider, Runner, Brute, Boss }
    
    // TODO :: (1)
    public enum RoadLayout { Loop, TopBottom } // Loop: up to 6 cols; TopBottom: 7-9 cols

    [Serializable]
    public struct Slot
    {
        public int Row;
        public int Col;
 
        public Slot(int row, int col) { Row = row; Col = col; }
        public override string ToString() => $"[{Row},{Col}]";
    }
    
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
    
    [Serializable]
    public struct RoadPolyline
    {
        public Vector2[] pointOffsetRelativeToBoard;
    }
    
    [Serializable]
    public struct WaveEntry
    {
        public RaiderType Type;
        public int Cave;        // 0 or 1, index into LevelConfig.Roads
    }
 
    [Serializable]
    public struct Wave
    {
        public WaveEntry[] Entries;
    }
    
    [Serializable]
    public struct LevelConfig
    {
        public const int DefaultCastleHp = 2000;
        public const int WavesPerLevel = 5;
        
        // TODO :: (1)
        public const int LoopRoadMaxCols = 6; 
        
        public int Id;
        public Area Area;
        
        public SlotOrder SlotOrder; // TODO :: can be used later down the road when the data and game's logic can be segregated even more
        public Origin Origin;
        
        // One string per row, first string = top row.
        // '.' empty, '#' hole, 'G' golden gear, 'I' frozen anchor.
        public string[] Board;
        
        public EngineDef[] Engines;
        public FrozenDef[] Frozen;
        public SpecialDef[] Special;
        public RoadPolyline[] Roads;    // 1 or 2 (one per cave)
        
        public CatType[] CatPool;
        public int StartCoins;
        public int StartPrice;
        
        public int CastleHp;            // if 0, use DefaultCastleHp
        public bool UseDefaultWaves;    // JSON "waves": "default"
        
        public Wave[] Waves;            // exactly WavesPerLevel when !UseDefaultWaves
        
        public float Difficulty;        // 0 = derive from Id
        public CatType? Unlock;         // cat unlocked by finishing this level, or null
        
        public int Rows => Board?.Length ?? 0;
        public int Cols => Rows > 0 ? Board[0].Length : 0;
        
        // TODO :: (1)
        public RoadLayout Layout => Cols <= LoopRoadMaxCols ? RoadLayout.Loop : RoadLayout.TopBottom;
        
        public int EffectiveCastleHp => CastleHp > 0 ? CastleHp : DefaultCastleHp;
        public float EffectiveDifficulty => Difficulty > 0f ? Difficulty : 1f + 0.08f * (Id - 1);
    }
    
    [CreateAssetMenu(fileName = "LevelData", menuName = "Scriptable Objects/LevelData")]
    public class LevelData : ScriptableObject
    {
        
    }
}
