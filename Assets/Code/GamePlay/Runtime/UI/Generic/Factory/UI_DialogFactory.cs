using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using YRFramework.Runtime;
using YRFramework.Runtime.UI;

namespace GamePlay.Runtime.UI
{
    [UIFactory(YRConsts.UI.Dialog, E_UIGroupType.Tip, E_UIShowType.Pop, true)]
    public class UI_DialogFactory : IUIFactory
    {
        public UIEntity Create(GameObject go, UIInfo uiInfo)
        {
            UIEntity uiEntity = UIEntity.Create(new UI_DialogController(), go, uiInfo);

            return uiEntity;
        }

        public static async UniTask Open(string title, string content, Action close = null,
            string btn1 = "", Action action1 = null,
            string btn2 = "", Action action2 = null,
            string btn3 = "", Action action3 = null)
        {
            UIEntity uiEntity = await Game.UI.Create(YRConsts.UI.Dialog);
            if (null == uiEntity)
                return;

            if (!uiEntity.TryGetUIController(out UI_DialogController ui))
                return;

            ui.Open(title, content, close, btn1, action1, btn2, action2, btn3, action3);
        }

        public static void Close()
        {
            Game.UI.CloseUI(YRConsts.UI.Dialog);
        }
    }
}