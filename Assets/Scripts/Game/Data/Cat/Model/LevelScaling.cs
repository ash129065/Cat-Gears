using System;

namespace Game.Data.Cat.Model
{
    [Serializable]
    public struct LevelScalingDef
    {
        public float[] DamageMult;    // per merge level Lv1..Lv3
        public float[] HpMult;
        public int[] CatCap;          // cats on the road per gear, Lv1..Lv3
    }

    /// <summary>Merge-level scaling shared by all cats (GDD F06, F04). Values from cat-defaults.json.</summary>
    public static class LevelScaling
    {
        public const int MaxLevel = 3;

        static LevelScalingDef Def => CatDefaults.Current.LevelScaling;
        static int Idx(int level) => Math.Max(1, Math.Min(MaxLevel, level)) - 1;

        public static float Damage(in CatDef c, int level) => c.Damage * Def.DamageMult[Idx(level)];
        public static int Hp(in CatDef c, int level) => (int)Math.Round(c.Hp * Def.HpMult[Idx(level)]);
        public static int Cap(int level) => Def.CatCap[Idx(level)];
    }
}