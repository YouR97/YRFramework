using Cysharp.Threading.Tasks;
using UnityEngine;
using YRFramework.Runtime;
using YRFramework.Runtime.UI;

namespace GamePlay.Runtime.UI
{
    [UIFactory(YRConsts.UI.Home, E_UIGroupType.Normal, E_UIShowType.Full, true)]
    public sealed class UI_HomeFactory : IUIFactory
    {
        #region 接口方法
        UIEntity IUIFactory.Create(GameObject go, UIInfo uiInfo)
        {
            UIEntity uiEntity = UIEntity.Create(new UI_HomeController(), go, uiInfo);

            return uiEntity;
        }
        #endregion

        #region 公共方法
        public static async UniTask Open()
        {
            UIEntity uiEntity = await Game.UI.Create(YRConsts.UI.Home);
            if (null == uiEntity)
                return;

            if (!uiEntity.TryGetUIController(out UI_HomeController ui))
                return;

            ui.Open();
        }

        public static void Close()
        {
            Game.UI.CloseUI(YRConsts.UI.Home);
        }
        #endregion
    }
}