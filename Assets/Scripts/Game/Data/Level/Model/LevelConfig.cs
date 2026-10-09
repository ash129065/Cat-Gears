using System;
using Game.Core.Economy.Shop;
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
    public class LevelConfig : ILevelConfig
    {
        public const int DefaultCastleHp = 2000;
        public const int WavesPerLevel = 5;
        public const int LoopRoadMaxCols = 6;     // up to 6 cols: loop road; 7-9: top-and-bottom road
        public const float DifficultyStep = 0.08f;

        // ---- JSON props ----
        [JsonProperty] [field: SerializeField] public int Id { get; private set; }
        [JsonProperty] [field: SerializeField] public Area Area { get; private set; }

        [JsonProperty] [field: SerializeField] public SlotOrder SlotOrder { get; private set; }
        [JsonProperty] [field: SerializeField] public Origin Origin { get; private set; }

// One string per row, first string = top row.
// '.' empty, '#' hole, 'G' golden gear, 'I' frozen anchor.
        [JsonProperty] [field: SerializeField] public string[] Board { get; private set; }

        [JsonProperty] [field: SerializeField] public EngineDef[] Engines { get; private set; }
        [JsonProperty] [field: SerializeField] public FrozenDef[] Frozen { get; private set; }
        [JsonProperty] [field: SerializeField] public SpecialDef[] Special { get; private set; }
        [JsonProperty] [field: SerializeField] public RoadPolyline[] Roads { get; private set; }   // 1 or 2 (one per cave)

        [JsonProperty] [field: SerializeField] public CatType[] CatPool { get; private set; }
        [JsonProperty] [field: SerializeField] public int StartCoins { get; private set; }
        [JsonProperty] [field: SerializeField] public int StartPrice { get; private set; }

        [JsonProperty] [field: SerializeField] public int CastleHp { get; private set; }           // 0 = use DefaultCastleHp
        [JsonProperty] [field: SerializeField] public Wave[] Waves { get; private set; }           // empty = use the default waves

        [JsonProperty] [field: SerializeField] public ShopOffer[] ShopPool { get; private set; }

        [JsonProperty] [field: SerializeField] public float Difficulty { get; private set; }       // 0 = derive from Id

// Cats unlocked by finishing this level. null or empty = none.
        [JsonProperty] [field: SerializeField] public CatType[] Unlock { get; private set; }

        [JsonProperty("UseDefaultWaves")]
        bool useDefaultWaves;

        // ---- derived ----
        [JsonIgnore] public bool UseDefaultWaves => useDefaultWaves || Waves == null || Waves.Length == 0;
        [JsonIgnore] public int Rows => Board?.Length ?? 0;
        [JsonIgnore] public int Cols => Rows > 0 ? Board[0].Length : 0;
        [JsonIgnore] public RoadLayout Layout => Cols <= LoopRoadMaxCols ? RoadLayout.Loop : RoadLayout.TopBottom;
        [JsonIgnore] public int EffectiveCastleHp => CastleHp > 0 ? CastleHp : DefaultCastleHp;
        [JsonIgnore] public float EffectiveDifficulty => Difficulty > 0f ? Difficulty : 1f + DifficultyStep * (Id - 1);
    }
}
