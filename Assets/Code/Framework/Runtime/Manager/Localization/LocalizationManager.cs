using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Tables;
using UnityEngine.ResourceManagement.AsyncOperations;
using YRFramework.Runtime.Manager;

namespace YRFramework.Runtime.Localization
{
    /// <summary>
    /// 本地化管理器
    /// </summary>
    [DisallowMultipleComponent]
    [AddComponentMenu("YRFramework/LocalizationManager")]
    public class LocalizationManager : YRFrameworkManager, IInit
    {
        #region Base
        public override E_FrameworkManagerType ManagerType { get; protected set; } = E_FrameworkManagerType.Localization;
        #endregion

        /// <summary>
        /// 当前设置语言
        /// </summary>
        private E_Language curLanguage;
        /// <summary>
        /// 本地化字典
        /// </summary>
        private Dictionary<E_Language, Locale> dicLocale;
        /// <summary>
        /// 本地化表
        /// </summary>
        private StringTable stringTable;

        [LabelText("初始化语言")]
        [SerializeField]
        private E_Language initLanguage;

        /// <summary>
        /// 语言类型
        /// </summary>
        public E_Language Language
        {
            get { return curLanguage; }
            private set
            {
                curLanguage = value;
            }
        }

        public async UniTask OnInit()
        {
            dicLocale = new Dictionary<E_Language, Locale>();

            AsyncOperationHandle initOperation = LocalizationSettings.SelectedLocaleAsync;
            await initOperation;
            List<Locale> listLocale = LocalizationSettings.AvailableLocales.Locales;
            foreach (Locale locale in listLocale)
            {
                if (!Enum.TryParse(locale.Identifier.Code, true, out E_Language language))
                {
                    Debug.LogError($"没有匹配的语言类型：{locale.Identifier.Code}");
                    continue;
                }

                dicLocale.Add(language, locale);
            }

            SetLanguage(initLanguage);

            await UniTask.CompletedTask;
        }

        public void OnRelease()
        {
            if (null != dicLocale)
            {
                dicLocale.Clear();
                dicLocale = null;
            }

            stringTable = null;
        }

        /// <summary>
        /// 设置语言
        /// </summary>
        private void SetLanguage(E_Language language)
        {
            if (Language == language)
                return;

            if (!dicLocale.TryGetValue(language, out Locale locale))
            {
                Debug.Log($"没有该语言：{language}，设置失败");
                return;
            }

            Debug.Log($"设置语言：{language}");
            Language = language;
            LocalizationSettings.Instance.SetSelectedLocale(locale);

            SetStringTable();
        }

        /// <summary>
        /// 初始化StringTable
        /// </summary>
        public async void SetStringTable()
        {
            AsyncOperationHandle<StringTable> loadingOperation = LocalizationSettings.StringDatabase.GetTableAsync("Localization");
            await loadingOperation;
            if (AsyncOperationStatus.Succeeded != loadingOperation.Status)
            {
                Debug.LogError($"不能加载StringTable：{loadingOperation.OperationException}");
                return;
            }
            stringTable = loadingOperation.Result;
        }

        /// <summary>
        /// 通过key获取本地化语言
        /// </summary>
        /// <param name="key"></param>
        public void GetLocalization(string key)
        {
            stringTable.GetEntry(key).GetLocalizedString();
        }
    }
}