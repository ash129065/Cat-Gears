using System.Collections.Generic;
using Game.Data.Base;
using UnityEditor;
using UnityEngine;

namespace Game.Data.Level
{
    [CreateAssetMenu(fileName = "LevelDatas", menuName = "Scriptable Objects/LevelDatas")]
    public class LevelDataCollectionSo : BaseSo
    {
        [SerializeField] private List<LevelDataSo> levelDataList;
        
        private Dictionary<int, LevelConfig> levelDataDict = new();

        [ContextMenu("LoadAllScriptableLevelData")]
        public void CreateScriptableForEachLevel()
        {
            DeleteLevelConfigsMenu();
            levelDataList.Clear();
            
            TextAsset[] textAssets = Resources.LoadAll<TextAsset>("LevelsJson");

            string folder = "Assets/Data/SO/LevelData/Levels";
            if (!AssetDatabase.IsValidFolder(folder))
                AssetDatabase.CreateFolder("Assets", "Data");

            LevelDataSo levelDataSo;
                
            for (int idx = 0; idx < textAssets.Length; idx++)
            {
                string path = folder + $"/LevelData{idx + 1}.asset";
            
                ScriptableObject asset = CreateInstance(typeof(LevelDataSo));
                AssetDatabase.CreateAsset(asset, path);

                levelDataSo = ((LevelDataSo)asset);
                Debug.Log($"textAsset[{idx}] = {textAssets[idx].text}");
                levelDataSo.PopulateData(textAssets[idx].text);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
                
                levelDataList.Add(levelDataSo);
            }
        }
        
        // folderPath is project-relative, e.g. "Assets/Data/Levels"
        public static int DeleteAll<T>(string folderPath, bool toTrash = true) where T : ScriptableObject
        {
            if (!AssetDatabase.IsValidFolder(folderPath)) return 0;

            string[] guids = AssetDatabase.FindAssets($"t:{typeof(T).Name}", new[] { folderPath });
            var paths = new List<string>(guids.Length);
            foreach (string guid in guids)
                paths.Add(AssetDatabase.GUIDToAssetPath(guid));

            int deleted = 0;
            AssetDatabase.StartAssetEditing();
            try
            {
                foreach (string path in paths)
                {
                    bool ok = toTrash
                        ? AssetDatabase.MoveAssetToTrash(path)   // recoverable
                        : AssetDatabase.DeleteAsset(path);       // permanent
                    if (ok) deleted++;
                    else Debug.LogWarning($"Could not delete {path}");
                }
            }
            finally
            {
                AssetDatabase.StopAssetEditing();
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
            return deleted;
        }
        
        [ContextMenu("Delete All Level Configs")]
        static void DeleteLevelConfigsMenu()
        {
            const string folder = "Assets/Data/SO/LevelData/Levels";
            if (!EditorUtility.DisplayDialog("Delete level configs",
                    $"Delete every LevelConfigSO in {folder}?", "Delete", "Cancel")) return;

            int n = DeleteAll<LevelDataSo>(folder);
            Debug.Log($"Deleted {n} level configs.");
        }
        
        public override void InitData()
        {
            foreach (var config in levelDataList)
                levelDataDict.Add(config.LevelConfig.Id, config.LevelConfig);
        }
        
        public LevelConfig GetLevelConfig(int id) => levelDataDict[id];
    }
}
