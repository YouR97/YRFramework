using Cysharp.Threading.Tasks;
using UnityEngine;
using YRFramework.Runtime;
using YRFramework.Runtime.UI;

namespace GamePlay.Runtime.UI
{
    [UIFactory(YRConsts.UI.Setting, E_UIGroupType.Normal, E_UIShowType.Pop, true)]
    public class UI_SettingFactory : IUIFactory
    {
        public UIEntity Create(GameObject go, UIInfo uiInfo)
        {
            UIEntity uiEntity = UIEntity.Create(new UI_SettingController(), go, uiInfo);

            return uiEntity;
        }

        public static async UniTask Open()
        {
            UIEntity uiEntity = await Game.UI.Create(YRConsts.UI.Setting);
            if (null == uiEntity)
                return;

            if (!uiEntity.TryGetUIController(out UI_SettingController uiSettingController))
                return;

            uiSettingController.Open();
        }

        public static void Close()
        {
            Game.UI.CloseUI(YRConsts.UI.Setting);
        }
    }
}