using System.Collections.Generic;
using Game.Abstraction;
using Game.Core.Controller;
using Game.Core.Entities.Gear.Model;
using Game.Core.Entities.Gear.Views;
using Game.Data.General;
using Game.Data.Level;
using Game.Data.Level.Abstraction;
using Game.Data.Level.SOClasses;
using Game.Interface;
using Game.Level;
using UnityEngine;

namespace Game.Core.Managers
{
    // TODO :: (1) :: set power source gear Ids using bfs
    // TODO :: (2) :: set the engine defs, frozen defs, special defs, priced defs
    // TODO :: (3) :: convert to a variable
    
    public interface IGridCreator : IInterfaceBase
    {
        void CreateGrid(ILevelConfig levelConfig);
        void InitGearHandler(IGearHandler gearHandler);
    }

    public interface IGridSpawner
    {
        void SpawnNewGearInstance(string gearValType, Vector2 slot);
        void UpdateGearView(Vector2 worldPos);
        void DestroyGearView(Vector2 worldPos);
    }

    [System.Serializable]
    public class BFSTracker
    {
        private List<string> poweredNodes = new();
        
        public void AddToPoweredNodes(string nodeName)
        {
            poweredNodes.Add(nodeName);
        }
    }

    public class GridManager : MonoBehaviour, IBase, IGridCreator, IGridSpawner
    {
        // TODO :: Initialize all fields
        [SerializeField] private GearPlacementController gearPlacementController;
        [SerializeField] private GearSpriteSo gearSpriteSo;
        [SerializeField] private GearView gearViewPrefab;
        [SerializeField] private Transform gearRoot;
        [SerializeField] private Vector2 cellSpanSize;
        [SerializeField] private LayerMask slotMask;   // only the Slot layer
        
        private Vector2Int[] neighboringDirections = 
            { Vector2Int.up, Vector2Int.right, Vector2Int.down, Vector2Int.left };
        
        private int totalRows, totalCols;
        private IGearHandler gearHandler;
        private GearView draggedGearView;
        private BFSTracker bfsTracker;
        
        private Dictionary<string, Slot> gearSlotDict = new();
        private Dictionary<string, GearView> gearViewDict = new();

        public Slot GetSlotAt(Vector3 worldPos)
        {
            Collider2D hit = Physics2D.OverlapPoint(worldPos, slotMask);
            if (hit == null)
                Debug.Log($"Hit :: hit object is null");
            else
                Debug.Log($"Hit :: {hit.name}, worldPos: {worldPos}");
            
            if (hit != null && gearSlotDict.ContainsKey(hit.name))
                return gearSlotDict[hit.name];

            return new(-1, -1);
        }
        
        public void InitBase()
        {
            InterfaceManagerMain.Instance.RegisterInterfaceInstance<GridManager>(this);
            BaseInterfaceManager.Instance.RegisterInterfaceInstance<IGridCreator>(this);
        }
        
        public void InitGearHandler(IGearHandler gearHandler)
        {
            this.gearHandler = gearHandler;
            bfsTracker = new BFSTracker();
            gearPlacementController.InitGridSpawner(this);
        }
        
        public void CreateGrid(ILevelConfig levelConfig)
        {
            string boardRow;
            totalRows = levelConfig.Rows;
            totalCols = levelConfig.Cols;

            Debug.Log($"CreateGrid: {levelConfig.Id}, {levelConfig.Rows}, {levelConfig.Cols}");
            
            for (int row = 0; row < totalRows; row++)
            {
                boardRow = levelConfig.Board[row];
                for (int col = 0; col < totalCols; col++)
                {
                    if (!boardRow[col].Equals('.')) continue;
                    
                    Debug.Log($"boardRow[col]: {boardRow[col]}");
                    SetGearModelAndView(col, row, GetSprite(boardRow[col]));
                }
            }
            
            // EngineDef, FrozenDef, SpecialDef 
            foreach (EngineDef def in levelConfig.Engines)
            {
                SetGearModelAndView(def.Slot.Col, def.Slot.Row, gearSpriteSo.GetSprite("engineGear"));
            }

            // TODO :: (2)

            // TODO :: (1)

        }

        private void SetGearModelAndView(int col, int row, Sprite sprite)
        {
            string posId = GetPositionInStringForm(col, row);
            Debug.Log($"PosString: {posId}");
                    
            GearModel gearModel = new();
            gearModel.Init(posId);
            
            GearView gearView = Instantiate(gearViewPrefab, gearRoot);
            
            gearView.name = posId; 
            gearView.transform.rotation = Quaternion.identity;
            gearView.transform.localPosition = new Vector3(
                (col - (totalCols - 1) / 2f) * cellSpanSize.x,
                ((totalRows - 1) / 2f - row) * cellSpanSize.y,
                0);

            gearView.InitModel(gearModel);
            gearView.Init(col, row, posId);
            gearView.InitSprite(sprite);
            
            gearHandler.UpdateGearData(gearModel);

            Slot slot = new(row, col);
            UpdateGearSlotDict(gearView.name, slot);
            UpdateGearViewDict(posId, gearView);
        }

        private void UpdateGearViewDict(string posId, GearView gearView)
        {
            gearViewDict[posId] = gearView;
        }
        
        public void SpawnNewGearInstance(string gearValType, Vector2 worldPos)
        {
            // gearValType can be used for getting the right gearView sprite 
            draggedGearView = Instantiate(gearViewPrefab, gearRoot);
            draggedGearView.transform.position = worldPos;
            draggedGearView.transform.rotation = Quaternion.identity;
            draggedGearView.BoxCollider2D.enabled = false;
            
            draggedGearView.SetSortingOrder(4); // TODO :: (3)
            
            draggedGearView.InitSprite(gearSpriteSo.GetSprite(gearValType));
        }

        public void UpdateGearView(Vector2 worldPos)
        {
            draggedGearView.transform.position = worldPos;
        }

        public void DestroyGearView(Vector2 worldPos)
        {
            Slot slot = GetSlotAt(worldPos);
            Debug.Log($"DestroyGearView: worldPos: {worldPos}, col: {slot.Col}, row: {slot.Row}");
            if (slot.Col != -1 && slot.Row != -1)
            {
                // found the overlap point
                
                string posId = GetPositionInStringForm(slot.Col, slot.Row);
                draggedGearView.name = posId;
                
                Debug.Log($"PosString: {posId}");
                    
                GearModel gearModel = new();
                gearModel.Init(posId);
                
                draggedGearView.BoxCollider2D.enabled = true;
                draggedGearView.transform.rotation = Quaternion.identity;
                draggedGearView.transform.localPosition = new Vector3(
                    (slot.Col - (totalCols - 1) / 2f) * cellSpanSize.x,
                    ((totalRows - 1) / 2f - slot.Row) * cellSpanSize.y,
                    0);
                
                draggedGearView.InitModel(gearModel);
                draggedGearView.Init(slot.Col, slot.Row, posId);
                    
                gearHandler.UpdateGearData(gearModel);
                UpdateGearSlotDict(draggedGearView.name, slot);
                if (gearViewDict.ContainsKey(draggedGearView.name))
                    Destroy(gearViewDict[posId].gameObject);

                int sortingOrder = gearViewDict[posId].SpriteRenderer.sortingOrder;
                UpdateGearViewDict(draggedGearView.name, draggedGearView);
                draggedGearView.SetSortingOrder(sortingOrder);
            }
            else
            {
                Destroy(draggedGearView.gameObject);
            }
        }

        private void UpdateGearSlotDict(string name, Slot slot)
        {
            gearSlotDict[name] = slot;
        }

        private string GetPositionInStringForm(int col, int row)
        {
            return $"gear({col}, {row})";
        }

        private Sprite GetSprite(char identifier)
        {
            switch (identifier)
            {
                case '.': // place where any gear type can go
                    return gearSpriteSo.GetSprite("emptyGear");
                case '#': // hole: nothing can be placed
                    return gearSpriteSo.GetSprite("hole");
                case 'G': // golden gear
                    return gearSpriteSo.GetSprite("goldenGear");
                case 'I': // larger gear
                    return gearSpriteSo.GetSprite("largerGear");
            }
            
            return null;
        }
    }
}
