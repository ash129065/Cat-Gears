using Newtonsoft.Json;
using UnityEngine;

namespace Game.Data.Level
{
    // TODO :: (1) :: can be used later down the road when we have a appropriate design for it (1)
    
    [CreateAssetMenu(fileName = "LevelData", menuName = "Scriptable Objects/LevelData")]
    public class LevelDataSo : ScriptableObject
    {
        [SerializeField] private LevelConfig levelConfig;

        public LevelConfig LevelConfig => levelConfig;
        
        public void PopulateData(string data)
        {
            levelConfig = JsonConvert.DeserializeObject<LevelConfig>(data);
        }
    }
}
