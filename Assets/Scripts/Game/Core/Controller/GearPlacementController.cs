using Game.Core.Managers;
using Game.Data.Gear.SO;
using Game.Data.Level;
using UnityEngine;

namespace Game.Core.Controller
{
    public class GearPlacementController : MonoBehaviour
    {
        [SerializeField] private GearDragHandlerSo gearDragHandlerSo;

        private IGridSpawner gridSpawner;
        
        public void InitGridSpawner(IGridSpawner gridSpawner)
        {
            this.gridSpawner = gridSpawner;
        }
        
        private void OnEnable()
        {
            gearDragHandlerSo.OnDragBegun += OnPreviewGear;
            gearDragHandlerSo.OnDragInProgress += OnMoved;
            gearDragHandlerSo.OnDragEnded += OnPreviewCancelled;
        }
    
        private void OnDisable()
        {
            gearDragHandlerSo.OnDragBegun -= OnPreviewGear;
            gearDragHandlerSo.OnDragInProgress -= OnMoved;
            gearDragHandlerSo.OnDragEnded -= OnPreviewCancelled;
        }

        private void OnPreviewGear(string gearVal, Vector2 worldPos)
        {
            gridSpawner.SpawnNewGearInstance(gearVal, worldPos);
        }
    
        private void OnMoved(Vector2 worldPos)
        {
            gridSpawner.UpdateGearView(worldPos);
        }

        private void OnPreviewCancelled(Vector2 worldPos)
        {
            Debug.Log("OnPreviewCancelled");
            gridSpawner.DestroyGearView(worldPos);
        }
    }
}
