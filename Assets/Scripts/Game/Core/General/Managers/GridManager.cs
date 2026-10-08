using Game.Abstraction;
using Game.Core.Entities.Gear.Model;
using Game.Core.Views;
using Game.Data.Level.Abstraction;
using Game.Data.Level.SOClasses;
using Game.Interface;
using Game.Level;
using UnityEngine;

namespace Game.Core.Managers
{
    public interface IGridCreator : IInterfaceBase
    {
        void CreateGrid(ILevelConfig levelConfig);
        void InitGearHandler(IGearHandler gearHandler);
    }
    
    public class GridManager : MonoBehaviour, IBase, IGridCreator
    {
        // TODO :: Initialize all fields
        [SerializeField] private GearSpriteSo gearSpriteSo;
        [SerializeField] private GearView gearViewPrefab;
        [SerializeField] private Transform gearRoot;
        [SerializeField] private Vector2 cellSpanSize;
        
        private Vector2Int[] neighboringDirections = 
            { Vector2Int.up, Vector2Int.right, Vector2Int.down, Vector2Int.left };
        
        private IGearHandler gearHandler;
        
        public void InitBase()
        {
            BaseInterfaceManager.Instance.RegisterInterfaceInstance<IGridCreator>(this);
        }
        
        public void InitGearHandler(IGearHandler gearHandler)
        {
            this.gearHandler = gearHandler;
        }
        
        public void CreateGrid(ILevelConfig levelConfig)
        {
            // levelConfig.Board

            string boardRow;
            int rowIdx = -1, colIdx = -1;
            int totalRows = levelConfig.Rows, totalCols = levelConfig.Cols;
            string posString;
            
            for (int row = totalRows - 1; row >= 0; row++)
            {
                rowIdx++;
                colIdx = -1;
                boardRow = levelConfig.Board[row];
                
                for (int col = 0; col < totalCols; col++)
                {
                    colIdx++;
                    posString = GetPositionInStringForm(colIdx, rowIdx);
                    
                    GearModel gearModel = new();
                    gearModel.Init(posString);
                    
                    GearView gearView = Instantiate(gearViewPrefab, gearRoot);
                    gearView.transform.localPosition = new Vector3(
                        (colIdx - (totalCols - 1) / 2f) * cellSpanSize.x,
                        ((totalRows - 1) / 2f - rowIdx) * cellSpanSize.y,
                        0);
                    gearView.transform.rotation = Quaternion.identity;

                    gearView.InitModel();
                    gearView.Init(posString, GetSprite(boardRow[col]));
                    
                    gearHandler.AddGearData(gearModel);
                }
            }
        }

        public string GetPositionInStringForm(int col, int row)
        {
            return $"gear({col}, {row})";
        }

        public Sprite GetSprite(char identifier)
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
