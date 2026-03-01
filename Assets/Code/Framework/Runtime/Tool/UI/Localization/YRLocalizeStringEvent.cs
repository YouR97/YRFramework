using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;

namespace YRFramework.Runtime.Tool.UI
{
    /// <summary>
    /// 本地化脚本
    /// </summary>
    [DisallowMultipleComponent]
    [RequireComponent(typeof(TMP_Text))]
    public class YRLocalizeStringEvent : LocalizeStringEvent
    {
        private TMP_Text text;
        
        private void Awake()
        {
            text = gameObject.GetComponent<TMP_Text>();

            OnUpdateString.AddListener(UpdateText);
        }

        private void UpdateText(string value)
        {
            text.text = value;
        }

        public void SetLocalization(string key, params object[] param)
        {
            if (null != StringReference)
                StringReference.Arguments = param;

            SetEntry(key);
        }
    }
}