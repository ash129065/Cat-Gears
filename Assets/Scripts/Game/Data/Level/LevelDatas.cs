using System.Collections.Generic;
using UnityEngine;

namespace Game.Data
{
    [CreateAssetMenu(fileName = "LevelDatas", menuName = "Scriptable Objects/LevelDatas")]
    public class LevelDatas : ScriptableObject
    {
        public List<LevelData> levelDataList;
    }
}
