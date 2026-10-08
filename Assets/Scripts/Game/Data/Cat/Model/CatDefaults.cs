using System;
using System.Collections.Generic;
using CatGears.Levels;
using UnityEngine;

namespace Game.Data.Cat.Model
{
    /// <summary>Contents of cat-defaults.json.</summary>
    [Serializable]
    public class CatDefaults : IDefaultsData
    {
        public const string ResourceName = "cat-defaults";

        public float EngineTurn;      // s per turn of a 10-tooth gear at x1
        public CatDef[] Cats;
        public LevelScalingDef LevelScaling;

        static CatDefaults current;
        public static CatDefaults Current => current ??= DefaultsLoader.Load<CatDefaults>(ResourceName);
        public static CatDefaults Parse(string json) => DefaultsLoader.Parse<CatDefaults>(json, ResourceName);
        public static CatDefaults Load() => DefaultsLoader.Load<CatDefaults>(ResourceName);
        public static void Reload() => current = null;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        static void ResetStatics() => current = null;

        public List<string> Validate()
        {
            var e = new List<string>();
            if (EngineTurn <= 0f) e.Add("engineTurn must be > 0.");
            DefaultsLoader.CheckEachOnce(e, "cats", Cats == null ? null : Array.ConvertAll(Cats, c => (int)c.Type), typeof(CatType));

            int n = Model.LevelScaling.MaxLevel;
            if (LevelScaling.DamageMult == null || LevelScaling.DamageMult.Length != n) e.Add($"levelScaling.damageMult needs {n} values.");
            if (LevelScaling.HpMult == null || LevelScaling.HpMult.Length != n) e.Add($"levelScaling.hpMult needs {n} values.");
            if (LevelScaling.CatCap == null || LevelScaling.CatCap.Length != n) e.Add($"levelScaling.catCap needs {n} values.");
            return e;
        }
    }
}
