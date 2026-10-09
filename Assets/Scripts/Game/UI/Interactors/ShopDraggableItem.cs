using UnityEngine;
using Game.Data.Gear.SO;
using UnityEngine.EventSystems;

namespace Game.UI.Interactors
{
    public class ShopDraggableItem : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        [SerializeField] private GearDragHandlerSo dragHandlerSo;
        
        private UnityEngine.Camera camera;
        private Vector2 mouseScreenPosition;

        public void InitShopItemData()
        {
            
        }
        
        private void Awake()
        {
            camera = UnityEngine.Camera.main;
        }
        
        public void OnBeginDrag(PointerEventData eventData)
        {
            Debug.Log("DragFunc :: OnBeginDrag");
            dragHandlerSo.RaiseOnDragBegun("affordablePriceGear", camera.ScreenToWorldPoint(Input.mousePosition));
        }

        public void OnDrag(PointerEventData eventData)
        {
            Debug.Log("DragFunc :: Dragging");
            dragHandlerSo.RaiseOnDragInProgress(camera.ScreenToWorldPoint(eventData.position));
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            Debug.Log("DragFunc :: OnEndDrag");   
            
            dragHandlerSo.RaiseOnDragEnded(camera.ScreenToWorldPoint(eventData.position));
        }
    }
}
