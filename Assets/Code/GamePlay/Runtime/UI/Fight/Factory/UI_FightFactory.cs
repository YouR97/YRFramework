using Cysharp.Threading.Tasks;
using UnityEngine;
using YRFramework.Runtime;
using YRFramework.Runtime.UI;

namespace GamePlay.Runtime.UI
{
    [UIFactory(YRConsts.UI.Fight, E_UIGroupType.Normal, E_UIShowType.Full, true)]
    public sealed class UI_FightFactory : IUIFactory
    {
        #region 接口方法
        UIEntity IUIFactory.Create(GameObject go, UIInfo uiInfo)
        {
            UIEntity uiEntity = UIEntity.Create(new UI_FightController(), go, uiInfo);

            return uiEntity;
        }
        #endregion

        #region 公共方法
        public static async UniTask Open()
        {
            UIEntity uiEntity = await Game.UI.Create(YRConsts.UI.Fight);
            if (null == uiEntity)
                return;

            if (!uiEntity.TryGetUIController(out UI_FightController ui))
                return;

            ui.Open();
        }

        public static void Close()
        {
            Game.UI.CloseUI(YRConsts.UI.Fight);
        }
        #endregion
    }
}