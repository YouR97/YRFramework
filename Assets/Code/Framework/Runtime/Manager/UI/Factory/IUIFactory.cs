using UnityEngine;

namespace YRFramework.Runtime.UI
{
    public interface IUIFactory
    {
        public UIEntity Create(GameObject go, UIInfo uiInfo);
    }
}