// Cat Gears - global cat table as a ScriptableObject, editable in the Inspector.
// Create one via: Assets > Create > Cat Gears > Cat Table
//
// No stat values are hardcoded here. New (empty) assets import their values from
// cat-defaults.json (Assets/Resources, or the file assigned to "Defaults Json").
// Use the component's context menu (the three dots) > "Import from defaults JSON"
// to overwrite the asset with the file's values again.

using System;
using System.Collections.Generic;
using Game.Data.Base;
using Game.Data.Cat.Model;
using Game.Data.Level;
using UnityEngine;

namespace Game.Data.Cat.SO
{
    [CreateAssetMenu(fileName = "CatTable", menuName = "Cat Gears/Cat Table")]
    public class CatTableAsset : ScriptableObject
    {
        [Tooltip("Optional. Leave empty to use Assets/Resources/cat-defaults.json.")]
        [SerializeField] TextAsset defaultsJson;

        [Tooltip("Seconds for one turn of a 10-tooth gear at x1 speed (GDD F02/F03).")]
        [SerializeField] float engineTurn;

        [Tooltip("One entry per CatType. Stats are Lv1 values; merge levels scale them (LevelScaling).")]
        [SerializeField] CatDef[] catDefinitions;

        Dictionary<CatType, CatDef> catDefLookupDict;

        public float EngineTurn => engineTurn;
        public IReadOnlyList<CatDef> All => catDefinitions;

        public CatDef Get(CatType type)
        {
            if (catDefLookupDict == null) BuildLookup();
            if (catDefLookupDict.TryGetValue(type, out var def)) return def;
            throw new ArgumentOutOfRangeException(nameof(type), type, "Cat type is missing from the cat table asset.");
        }

        public bool TryGet(CatType type, out CatDef def)
        {
            if (catDefLookupDict == null) BuildLookup();
            return catDefLookupDict.TryGetValue(type, out def);
        }

        /// <summary>Seconds per gear turn for a cat type (3.2 s small, 6.4 s big at x1).</summary>
        public float TurnTime(CatType type) => Get(type).TurnTime(engineTurn);

        void OnEnable()
        {
            catDefLookupDict = null;
#if UNITY_EDITOR
            // A freshly created asset is empty: fill it from the JSON. Never overwrites existing data.
            if (catDefinitions == null || catDefinitions.Length == 0) ImportDefaults(logIfMissing: false);
#endif
        }

        void BuildLookup()
        {
            catDefLookupDict = new Dictionary<CatType, CatDef>();
            foreach (var c in catDefinitions) catDefLookupDict[c.Type] = c; // last entry wins; OnValidate warns about duplicates
        }

        [ContextMenu("Import from defaults JSON")]
        void ImportDefaultsFromMenu() => ImportDefaults(logIfMissing: true);

        void ImportDefaults(bool logIfMissing)
        {
            try
            {
                var d = defaultsJson != null ? CatDefaults.Parse(defaultsJson.text) : CatDefaults.Load();
                engineTurn = d.EngineTurn;
                catDefinitions = d.Cats;
                catDefLookupDict = null;
#if UNITY_EDITOR
                UnityEditor.EditorUtility.SetDirty(this);
#endif
            }
            catch (Exception e)
            {
                // Resources can be unavailable while Unity is loading assets; the menu command retries.
                if (logIfMissing) Debug.LogError($"[CatTable] Could not import defaults: {e.Message}", this);
                else Debug.LogWarning($"[CatTable] Could not auto-import defaults ({e.Message}). Use the context menu > Import from defaults JSON.", this);
            }
        }

        void OnValidate()
        {
            catDefLookupDict = null;
            if (catDefinitions == null) return;

            var seen = new HashSet<CatType>();
            foreach (var c in catDefinitions)
            {
                if (!seen.Add(c.Type))
                    Debug.LogWarning($"[CatTable] Duplicate entry for {c.Type}.", this);

                int expectedTeeth = c.Size == GearSize.Big ? 20 : 10;
                if (c.Teeth != expectedTeeth)
                    Debug.LogWarning($"[CatTable] {c.Type} is {c.Size} but has {c.Teeth} teeth (expected {expectedTeeth}).", this);

                if (c.Hp <= 0 || c.Damage <= 0f || c.Cooldown <= 0f || c.MoveSpeed <= 0f)
                    Debug.LogWarning($"[CatTable] {c.Type} has a non-positive Hp, Damage, Cooldown or MoveSpeed.", this);
            }
            
            foreach (CatType t in Enum.GetValues(typeof(CatType)))
                if (!seen.Contains(t))
                    Debug.LogWarning($"[CatTable] No entry for {t}.", this);
        }
    }
}