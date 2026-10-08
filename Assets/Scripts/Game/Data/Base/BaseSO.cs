using UnityEngine;

namespace Game.Data.Base
{
    [CreateAssetMenu(fileName = "BaseSO", menuName = "Scriptable Objects/BaseSO")]
    public abstract class BaseSo : ScriptableObject
    {
        public abstract void InitData();
    }
}
