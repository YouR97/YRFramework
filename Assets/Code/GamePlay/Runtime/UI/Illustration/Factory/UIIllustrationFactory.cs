using Cysharp.Threading.Tasks;
using UnityEngine;
using YRFramework.Runtime;
using YRFramework.Runtime.UI;

namespace GamePlay.Runtime.UI
{
    [UIFactory(YRConsts.UI.Illustration, E_UIGroupType.Normal, E_UIShowType.Full, true)]
    public class UIIllustrationFactory : IUIFactory
    {
        public UIEntity Create(GameObject go, UIInfo uiInfo)
        {
            UIEntity uiEntity = UIEntity.Create(new UIIllustrationController(), go, uiInfo);

            return uiEntity;
        }

        public static async UniTask Open()
        {
            UIEntity uiEntity = await Game.UI.Create(YRConsts.UI.Illustration);
            if (null == uiEntity)
                return;

            if (!uiEntity.TryGetUIController(out UIIllustrationController uiIllustrationController))
                return;

            uiIllustrationController.Open();
        }

        public static void Close()
        {
            Game.UI.CloseUI(YRConsts.UI.Illustration);
        }
    }
}