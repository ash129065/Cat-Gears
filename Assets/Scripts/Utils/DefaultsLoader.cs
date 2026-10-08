// Cat Gears - shared JSON loading for the defaults files in Assets/Resources:
//   cat-defaults.json      -> CatDefaults     (cats, engine turn, merge scaling)
//   raider-defaults.json   -> RaiderDefaults  (raiders, per-wave scaling, spawn)
//   balance-defaults.json  -> BalanceDefaults (castle, economy, flow values)
//
// Requires Newtonsoft Json (Package Manager > Add package by name >
// com.unity.nuget.newtonsoft-json). Enum values are written by name in the JSON
// ("Club", "Small", "Pierce"). Unknown keys are an error, so typos are caught on load.

using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using UnityEngine;

namespace CatGears.Levels
{
    public interface IDefaultsData
    {
        /// <summary>Returns a list of problems; empty means valid.</summary>
        List<string> Validate();
    }

    public static class DefaultsLoader
    {
        public static T Load<T>(string resourceName) where T : class, IDefaultsData
        {
            var asset = Resources.Load<TextAsset>(resourceName);
            if (asset == null)
                throw new FileNotFoundException($"Missing Assets/Resources/{resourceName}.json.");
            return Parse<T>(asset.text, resourceName);
        }

        public static T Parse<T>(string json, string label) where T : class, IDefaultsData
        {
            var settings = new JsonSerializerSettings
            {
                MissingMemberHandling = MissingMemberHandling.Error,
                Converters = { new StringEnumConverter() }
            };
            
            var data = JsonConvert.DeserializeObject<T>(json, settings)
                       ?? throw new InvalidDataException($"{label}.json is empty.");

            var errors = data.Validate();
            if (errors.Count > 0)
                throw new InvalidDataException($"Invalid {label}.json:\n - " + string.Join("\n - ", errors));
            return data;
        }

        /// <summary>Adds an error for each enum value that is missing or listed twice.</summary>
        public static void CheckEachOnce(List<string> errors, string label, int[] values, System.Type enumType)
        {
            if (values == null || values.Length == 0) { errors.Add($"{label} is missing or empty."); return; }
            var seen = new HashSet<int>();
            foreach (var v in values)
                if (!seen.Add(v)) errors.Add($"{label} lists {System.Enum.GetName(enumType, v)} more than once.");
            foreach (var v in System.Enum.GetValues(enumType))
                if (!seen.Contains((int)v)) errors.Add($"{label} has no entry for {v}.");
        }
    }
}