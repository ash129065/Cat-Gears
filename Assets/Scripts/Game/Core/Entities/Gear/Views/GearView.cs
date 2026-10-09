using System;
using Game.Core.Entities.Gear.Model;
using Game.Data.Level;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Core.Entities.Gear.Views
{
    [System.Serializable]
    public struct GearProps
    {
        public Slot slot;

        public GearProps(Slot slot)
        {
            this.slot = slot;
        }
    }
    
    public class GearView : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer gear;

        private string posId;
        private GearModel model;
        private GearProps gearProps;

        public SpriteRenderer SpriteRenderer => gear;
        public BoxCollider2D BoxCollider2D { get; private set; }

        private void Awake()
        {
            BoxCollider2D = BoxCollider2D ?? GetComponent<BoxCollider2D>();
        }
        
        public void SetSortingOrder(int val)
        {
            SpriteRenderer.sortingOrder = val;
        }

        public void Init(int col, int row, string posId)
        {
            gearProps = new(new Slot(row, col));
            
            this.posId = posId;
        }

        public void InitSprite(Sprite sprite)
        {
            gear.sprite = sprite;
        }

        public void InitModel(GearModel model)
        {
            this.model = model;
            model.OnModelStateChanged = null;
            model.OnModelStateChanged += RefreshFromModel;
        }

        private void Update()
        {
            if (model == null || !model.IsPowered) return;

            Rotate();
        }

        private void Rotate()
        {
            transform.Rotate(Vector3.forward * model.RotationSpeed);
        }
        
        private void RefreshFromModel()
        {
            // update necessary view elements here
        }

        private void OnDisable()
        {
            if (model != null)
                model.OnModelStateChanged -= RefreshFromModel;
        }
    }
}
