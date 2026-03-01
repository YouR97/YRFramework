using Cysharp.Threading.Tasks;
using UnityEngine;
using YRFramework.Runtime;
using YRFramework.Runtime.UI;

namespace GamePlay.Runtime.UI
{
    [UIFactory(YRConsts.UI.Loading, E_UIGroupType.Loading, E_UIShowType.Pop, true)]
    public sealed class UI_LoadingFactory : IUIFactory
    {
        public UIEntity Create(GameObject go, UIInfo uiInfo)
        {
            UIEntity uiEntity = UIEntity.Create(new UI_LoadingController(), go, uiInfo);

            return uiEntity;
        }

        public static async UniTask Open()
        {
            UIEntity uiEntity = await Game.UI.Create(YRConsts.UI.Loading);
            if (null == uiEntity)
                return;

            if (!uiEntity.TryGetUIController(out UI_LoadingController uiLoadingController))
                return;

            uiLoadingController.Open();
        }

        public static void Close()
        {
            Game.UI.CloseUI(YRConsts.UI.Loading);
        }
    }
}