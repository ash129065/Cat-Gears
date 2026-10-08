// Cat Gears - cat data shapes (GDD F06, F09). No numbers live here: every value
// comes from Assets/Resources/cat-defaults.json via CatDefaults.
// Distances are in px, 1 slot = 48 px. Times are in seconds at x1 speed.

using System;
using System.Collections.Generic;
using Game.Data.Base;
using Game.Utils;
using Game.Data.Level;
using UnityEngine;

namespace Game.Data.Cat.Model
{
    public enum CatAbility { None, Pierce, Slow, FirstHitBonus, Splash }

    /// <summary>Base (Lv1) definition of one cat type, including combat stats.</summary>
    [Serializable]
    public struct CatDef
    {
        public CatType Type;
        public GearSize Size;
        public int Teeth;             // Small 10, Big 20 (turn time = engine turn x teeth / 10)
        public int UnlockLevel;

        // Lv1 combat stats (scaled by LevelScaling for Lv2/Lv3)
        public int Hp;
        public float Damage;
        public float Reach;           // px
        public float Cooldown;        // s between attacks
        public float MoveSpeed;       // px/s along the road
        public bool Ranged;           // true = fires a projectile that hits on arrival

        // Special ability and its parameters (only the matching ones are used)
        public CatAbility Ability;
        public int PierceCount;       // Spear: targets hit per thrust (target + next behind)
        public float PierceRange;     // Spear: px behind the target
        public float SlowFactor;      // Frost: 0.45 = 45% slower
        public float SlowDuration;    // Frost: s, does not stack, refreshes
        public float FirstHitMultiplier; // Knight Rider: first hit after reaching a raider
        public float SplashRadius;    // Wizard: px around the target, full damage

        public float TurnTime(float engineTurn) => engineTurn * Teeth / 10f;
    }
}
