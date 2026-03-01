using Cysharp.Threading.Tasks;
using UnityEngine;
using YRFramework.Runtime;
using YRFramework.Runtime.UI;

namespace GamePlay.Runtime.UI
{
    [UIFactory(YRConsts.UI.BlackLoading, E_UIGroupType.Loading, E_UIShowType.Pop, true)]
    public sealed class UI_BlackLoadingFactory : IUIFactory
    {
        public UIEntity Create(GameObject go, UIInfo uiInfo)
        {
            UIEntity uiEntity = UIEntity.Create(new UI_BlackLoadingController(), go, uiInfo);

            return uiEntity;
        }

        public static async UniTask Open()
        {
            await Utility.ScreenShot.CaptureScreenshot();
            
            UIEntity uiEntity = await Game.UI.Create(YRConsts.UI.BlackLoading);
            if (null == uiEntity)
                return;

            if (!uiEntity.TryGetUIController(out UI_BlackLoadingController uiLoadingController))
                return;

            uiLoadingController.Open();
        }

        public static void CloseByAnim()
        {
            if (!Game.UI.TryGetUIByTyoe(YRConsts.UI.BlackLoading, out UIEntity uiEntity))
                return;

            if (!uiEntity.TryGetUIController(out UI_BlackLoadingController uiLoadingController))
                return;

            uiLoadingController.CloseByAnim();
        }

        public static void Close()
        {
            Game.UI.CloseUI(YRConsts.UI.BlackLoading);
        }
    }
}