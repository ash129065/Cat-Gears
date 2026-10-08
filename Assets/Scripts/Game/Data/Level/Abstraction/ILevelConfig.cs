using Game.Data.Base;
using Game.Data.General;
using JetBrains.Annotations;

namespace Game.Data.Level.Abstraction
{
    public interface ILevelConfig
    {
        // ---- JSON props ----
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

        int CastleHp { get; }               // if 0, use EffectiveCastleHp
        Wave[] Waves { get; }               // exactly WavesPerLevel when !UseDefaultWaves

        float Difficulty { get; }           // 0 = derive from Id
        [CanBeNull] CatType[] UnlockableCats { get; }   // cats unlocked by finishing this level, or null

        // ---- not serialized ----
        bool UseDefaultWaves { get; }       // JSON "waves": "default"

        // ---- derived ----
        int Rows { get; }
        int Cols { get; }
        RoadLayout Layout { get; }
        int EffectiveCastleHp { get; }
        float EffectiveDifficulty { get; }
    }
}