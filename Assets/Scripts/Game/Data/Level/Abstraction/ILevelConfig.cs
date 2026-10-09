using Game.Core.Economy.Shop;
using Game.Data.Base;
using Game.Data.General;
using JetBrains.Annotations;

namespace Game.Data.Level.Abstraction
{
    public interface ILevelConfig
    {
        // ---- JSON props (same order as the level JSON) ----
        int Id { get; }
        Area Area { get; }
 
        SlotOrder SlotOrder { get; }
        Origin Origin { get; }
 
        // One string per row, first string = top row.
        // '.' empty, '#' hole, 'G' golden gear, 'I' frozen anchor.
        string[] Board { get; }
 
        EngineDef[] Engines { get; }
        FrozenDef[] Frozen { get; }
        SpecialDef[] Special { get; }
        RoadPolyline[] Roads { get; }       // 1 or 2 (one per cave)
 
        CatType[] CatPool { get; }
        int StartCoins { get; }
        int StartPrice { get; }
 
        ShopOffer[] ShopPool { get; }       // what the shop tray may offer in this level
 
        int CastleHp { get; }               // if 0, use EffectiveCastleHp
        bool UseDefaultWaves { get; }       // true = ignore Waves and use the default waves
        Wave[] Waves { get; }               // exactly WavesPerLevel when !UseDefaultWaves
 
        float Difficulty { get; }           // 0 = derive from Id
 
        // ---- derived (not in the JSON) ----
        int Rows { get; }
        int Cols { get; }
        RoadLayout Layout { get; }
        int EffectiveCastleHp { get; }
        float EffectiveDifficulty { get; }
    }
}