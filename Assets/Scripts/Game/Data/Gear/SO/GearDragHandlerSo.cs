using System;
using Game.Core.Entities.Gear.Views;
using UnityEngine;

namespace Game.Data.Gear.SO
{
    [CreateAssetMenu(fileName = "GearDragHandler", menuName = "Scriptable Objects/Gear/GearDragHandler")]
    public class GearDragHandlerSo : ScriptableObject
    {
        public Action<string, Vector2> OnDragBegun;
        public Action<Vector2> OnDragInProgress;
        public Action<Vector2> OnDragEnded;
        
        public void RaiseOnDragBegun(string gearVal, Vector2 worldPos) => OnDragBegun?.Invoke(gearVal, worldPos);
        public void RaiseOnDragInProgress(Vector2 worldPos) => OnDragInProgress?.Invoke(worldPos);
        public void RaiseOnDragEnded(Vector2 worldPos) => OnDragEnded?.Invoke(worldPos);
    }
}
