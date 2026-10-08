using System;
using Game.Abstraction;
using Game.Core;
using Game.Core.Managers;
using Game.Interface;
using Game.Level;
using UnityEngine;

namespace Game.State
{
    public enum GameState { GameStart, GameInProgress, GameEnd }
    
    public class GameManager : MonoBehaviour, IBase, IDataLoader
    {
        private IGridCreator gridCreator; 
        private IGearHandler gearHandler;
        private ILevelDataHandler levelDataHandler;
        
        public void InitBase()
        {
            InterfaceManagerMain.Instance.RegisterInterfaceInstance<GameManager>(this);
            
            
            levelDataHandler = BaseInterfaceManager.Instance.GetInterfaceInstance<ILevelDataHandler>();
            gridCreator = BaseInterfaceManager.Instance.GetInterfaceInstance<IGridCreator>();
            gearHandler = BaseInterfaceManager.Instance.GetInterfaceInstance<IGearHandler>();
            OnGameStateChanged(GameState.GameStart);
        }
        
        public void InitDataAndDependencies()
        {
            // levelDataHandler = BaseInterfaceManager.Instance.GetInterfaceInstance<ILevelDataHandler>();
            // gridCreator = BaseInterfaceManager.Instance.GetInterfaceInstance<IGridCreator>();
            // gearHandler = BaseInterfaceManager.Instance.GetInterfaceInstance<IGearHandler>();
            //
            // Debug.Log($"InitDataAndDependencies called: " +
            //           $"levelDataHandler: {levelDataHandler}, " +
            //           $"gridCreator: {gridCreator}, " +
            //           $"gearHandler: {gearHandler}");
        }
        
        public void OnGameStateChanged(GameState newState)
        {
            switch (newState)
            {
                case GameState.GameStart:
                {
                    gridCreator.InitGearHandler(gearHandler);
                    gridCreator.CreateGrid(levelDataHandler.GetLevelConfig());
                    break;
                }
            }
        }
    }
}
