using UnityEngine;
using System.Collections.Generic;

namespace Game.Data.Level.SOClasses
{
    
    [System.Serializable]
    public struct GearSpriteData
    {
        public string gearType; 
        public Sprite sprite;
    }
    
    [CreateAssetMenu(fileName = "GearSpriteSO", menuName = "Scriptable Objects/GearSpriteSO")]
    public class GearSpriteSo : ScriptableObject
    {
        [SerializeField] private GearSpriteData[] spriteDatas;
        
        private readonly Dictionary<string, Sprite> gearSprites = new Dictionary<string, Sprite>();

        public void InitSpriteData()
        {
            foreach (var sprite in spriteDatas)
                gearSprites.Add(sprite.gearType, sprite.sprite);
        }
        
        public Sprite GetSprite(string gearType) => gearSprites[gearType];
    }
}
