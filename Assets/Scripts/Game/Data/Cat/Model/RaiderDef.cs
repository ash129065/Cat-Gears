// Cat Gears - raider data shapes (GDD F10). No numbers live here: every value
// comes from Assets/Resources/raider-defaults.json via RaiderDefaults.

using System;
using System.Collections.Generic;
using CatGears.Levels;
using UnityEngine;

namespace Game.Data.Cat.Model
{
    /// <summary>Base (wave 1) raider definition. Level difficulty scales these, not the cats.</summary>
    [Serializable]
    public struct RaiderDef
    {
        public RaiderType Type;
        public int Hp;
        public float Speed;           // px/s toward the castle
        public float HitDamage;       // damage to a cat per hit
        public float HitCooldown;     // s between hits
        public int CastleDamage;      // taken off castle HP on reaching the gate
        public int Coins;             // paw coins dropped

        // Wave 1 = base values. Wave N multiplies by per-wave^(N-1), then by the level
        // difficulty. (GDD: "x1.22 / x1.12 per wave number, then x the level's difficulty".)
        // Assumption: difficulty applies to HP and hit damage; castle damage is not scaled.
        public int ScaledHp(int wave, float difficulty) =>
            (int)Math.Round(Hp * Math.Pow(RaiderDefaults.Current.Scaling.HpPerWave, wave - 1) * difficulty);

        public float ScaledHit(int wave, float difficulty) =>
            (float)(HitDamage * Math.Pow(RaiderDefaults.Current.Scaling.HitPerWave, wave - 1) * difficulty);
    }

    [Serializable]
    public struct RaiderScalingDef
    {
        public float HpPerWave;       // raider HP multiplier per wave number
        public float HitPerWave;      // raider hit damage multiplier per wave number
        public float BlockDistance;   // px, a raider stops when a cat is this close
        public float SpawnInterval;   // s between raiders leaving the cave
    }
    
    public static class RaiderTable
    {
        public static RaiderDef Get(RaiderType type)
        {
            var all = RaiderDefaults.Current.Raiders;
            for (int i = 0; i < all.Length; i++) if (all[i].Type == type) return all[i];
            throw new ArgumentOutOfRangeException(nameof(type), type, "Unknown raider type.");
        }
    }
}