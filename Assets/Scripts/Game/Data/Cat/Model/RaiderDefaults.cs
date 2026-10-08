using System;
using System.Collections.Generic;
using CatGears.Levels;
using Game.Data;
using Game.Data.Cat.Model;
using UnityEngine;

/// <summary>Contents of raider-defaults.json.</summary>
[Serializable]
public class RaiderDefaults : IDefaultsData
{
    public const string ResourceName = "raider-defaults";

    public RaiderDef[] Raiders;
    public RaiderScalingDef Scaling;

    static RaiderDefaults current;
    public static RaiderDefaults Current => current ??= DefaultsLoader.Load<RaiderDefaults>(ResourceName);
    public static RaiderDefaults Parse(string json) => DefaultsLoader.Parse<RaiderDefaults>(json, ResourceName);
    public static void Reload() => current = null;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    static void ResetStatics() => current = null;

    public List<string> Validate()
    {
        var e = new List<string>();
        DefaultsLoader.CheckEachOnce(e, "raiders", Raiders == null ? null : Array.ConvertAll(Raiders, r => (int)r.Type), typeof(RaiderType));
        if (Scaling.HpPerWave <= 0f) e.Add("scaling.hpPerWave must be > 0.");
        if (Scaling.HitPerWave <= 0f) e.Add("scaling.hitPerWave must be > 0.");
        if (Scaling.SpawnInterval <= 0f) e.Add("scaling.spawnInterval must be > 0.");
        return e;
    }
}