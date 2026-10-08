using UnityEngine;
using Game.Data.Cat.Model;
using System.Collections.Generic;

namespace Game.Data.Cat
{
    [CreateAssetMenu(fileName = "CatTypeSO", menuName = "Scriptable Objects/CatTypeSO")]
    public class CatTypeSo : ScriptableObject
    {
        [SerializeField] private List<CatDef> catDefs;
    }
}