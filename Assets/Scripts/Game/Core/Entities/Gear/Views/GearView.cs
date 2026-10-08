using UnityEngine;

namespace Game.Core.Views
{
    public class GearView : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer gear;        
        
        private string posId;
        
        public void Init(string posId, Sprite sprite)
        {
            this.posId = posId;
            gear.sprite = sprite;
        }

        public void InitModel()
        {
            
        }
    }
}
