using Cysharp.Threading.Tasks;
using UnityEngine;
using YRFramework.Runtime;
using YRFramework.Runtime.UI;

namespace GamePlay.Runtime.UI
{
    [UIFactory(YRConsts.UI.Main, E_UIGroupType.Normal, E_UIShowType.Full, true)]
    public class UIMainFactory : IUIFactory
    {
        public UIEntity Create(GameObject go, UIInfo uiInfo)
        {
            UIEntity uiEntity = UIEntity.Create(new UIMainController(), go, uiInfo);

            return uiEntity;
        }

        public static async UniTask Open()
        {
            UIEntity uiEntity = await Game.UI.Create(YRConsts.UI.Main);
            if (null == uiEntity)
                return;

            if (!uiEntity.TryGetUIController(out UIMainController uiMainController))
                return;

            uiMainController.Open();
        }

        public static void Close()
        {
            Game.UI.CloseUI(YRConsts.UI.Main);
        }
    }
}