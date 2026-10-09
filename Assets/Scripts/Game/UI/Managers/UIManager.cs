using Game.Abstraction;
using Game.Interface;
using Game.Level;
using UnityEngine;

namespace Game.UI.Managers
{
    public interface IUIHandler : IInterfaceBase {}
    
    public class UIManager : MonoBehaviour,  IUIHandler, IBase
    {
        public void InitBase()
        {
            InterfaceManagerMain.Instance.RegisterInterfaceInstance<UIManager>(this);
            BaseInterfaceManager.Instance.RegisterInterfaceInstance<IUIHandler>(this);
        }

        public void InitShopData()
        {
            
        }
    }
}
