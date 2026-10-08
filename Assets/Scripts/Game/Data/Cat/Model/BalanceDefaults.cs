// Cat Gears - castle, economy and flow defaults (GDD Appendix A, F11, F12, F16).
// Values come from Assets/Resources/balance-defaults.json.

using System;
using UnityEngine;
using Game.Utils;
using System.Collections.Generic;

namespace Game.Data.Cat.Model
{
    [Serializable]
    public class BalanceDefaults : IDefaultsData
    {
        public const string ResourceName = "Json/balance-defaults";

        public int CastleHp;
        public int WavesPerLevel;
        public float DifficultyStep;          // level difficulty = 1 + step x (id - 1)
        public float SpawnStartProgressMin;   // gear progress at wave start, random range
        public float SpawnStartProgressMax;
        public float BigPriceMultiplier;      // big price = multiplier x small price
        public int BigPurchaseAddsToSmall;
        public float ExistingCatWeight;       // buy roll weight for types already on the board
        public float SellRefund;              // fraction of invested coins
        public int CoinGearPayout;            // coins per full turn during a wave
        public float BrokenRepairMultiplier;  // default repair = multiplier x current small price
        public int WaveClearBonusBase;        // bonus = base + wave number
        public float AutoStartCountdown;      // s between waves
        public int GemsPerStar;

        static BalanceDefaults current;
        public static BalanceDefaults Current => current ??= DefaultsLoader.Load<BalanceDefaults>(ResourceName);
        public static BalanceDefaults Parse(string json) => DefaultsLoader.Parse<BalanceDefaults>(json, ResourceName);
        public static void Reload() => current = null;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        static void ResetStatics() => current = null;

        public List<string> Validate()
        {
            var e = new List<string>();
            if (CastleHp <= 0) e.Add("castleHp must be > 0.");
            if (WavesPerLevel <= 0) e.Add("wavesPerLevel must be > 0.");
            if (SpawnStartProgressMin > SpawnStartProgressMax) e.Add("spawnStartProgressMin must be <= spawnStartProgressMax.");
            if (SellRefund < 0f || SellRefund > 1f) e.Add("sellRefund must be between 0 and 1.");
            return e;
        }
    }
}
