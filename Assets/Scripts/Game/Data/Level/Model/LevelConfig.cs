using System;
using Game.Data.Base;
using Game.Data.General;
using Game.Data.Level.Abstraction;
using JetBrains.Annotations;
using Newtonsoft.Json;
using UnityEngine;

namespace Game.Data.Level
{
    [Serializable]
    public struct Slot
    {
        public int Row;
        public int Col;

        public Slot(int row, int col)
        {
            Row = row;
            Col = col;
        }

        public override string ToString() => $"[{Row},{Col}]";
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
        public int Cave; // 0 or 1, index into LevelConfig.Roads
    }

    [Serializable]
    public struct Wave
    {
        public WaveEntry[] Entries;
    }

    [Serializable]
    public struct LevelConfig : ILevelConfig
    {
        public const int DefaultCastleHp = 2000;
        public const int WavesPerLevel = 5;

        // TODO :: (1)
        public const int LoopRoadMaxCols = 6;

        #region JSON PROPS

        [JsonIgnore] [field: SerializeField] public bool UseDefaultWaves { get; set; } // JSON "waves": "default"

        [field: SerializeField] public int Id { get; set; }
        [field: SerializeField] public Area Area { get; set; }

        // TODO :: can be used later down the road when the data and game's logic can be segregated even more
        [field: SerializeField] public SlotOrder SlotOrder { get; set; }
        [field: SerializeField] public Origin Origin { get; set; }

        // One string per row, first string = top row.
        // '.' empty, '#' hole, 'G' golden gear, 'I' frozen anchor.
        [field: SerializeField] public string[] Board { get; set; }

        [field: SerializeField] public EngineDef[] Engines { get; set; }
        [field: SerializeField] public FrozenDef[] Frozen { get; set; }
        [field: SerializeField] public SpecialDef[] Special { get; set; }
        [field: SerializeField] public RoadPolyline[] Roads { get; set; } // 1 or 2 (one per cave)

        [field: SerializeField] public CatType[] CatPool { get; set; }
        [field: SerializeField] public int StartCoins { get; set; }
        [field: SerializeField] public int StartPrice { get; set; }

        [field: SerializeField] public int CastleHp { get; set; } // if 0, use DefaultCastleHp

        [field: SerializeField] public Wave[] Waves { get; set; } // exactly WavesPerLevel when !UseDefaultWaves

        [field: SerializeField] public float Difficulty { get; set; } // 0 = derive from Id

        [CanBeNull]
        [field: SerializeField]
        public CatType[] UnlockableCats { get; set; } // cats unlocked by finishing this level, or null

        #endregion

        // Derived (not serialized)
        public int Rows => Board?.Length ?? 0;
        public int Cols => Rows > 0 ? Board[0].Length : 0;

        // TODO :: (1)
        public RoadLayout Layout => Cols <= LoopRoadMaxCols ? RoadLayout.Loop : RoadLayout.TopBottom;

        public int EffectiveCastleHp => CastleHp > 0 ? CastleHp : DefaultCastleHp;
        public float EffectiveDifficulty => Difficulty > 0f ? Difficulty : 1f + 0.08f * (Id - 1);
    }
}