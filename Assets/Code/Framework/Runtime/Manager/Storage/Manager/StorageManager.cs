using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using YRFramework.Runtime.Manager;
using YRFramework.Runtime.Utility;

namespace YRFramework.Runtime.Storage
{
    /// <summary>
    /// 本地缓存管理器
    /// </summary>
    [DisallowMultipleComponent]
    [AddComponentMenu("YRFramework/StorageManager")]
    public sealed partial class StorageManager : YRFrameworkManager, IInit, IUpdate
    {
        #region Base
        public override E_FrameworkManagerType ManagerType { get; protected set; } = E_FrameworkManagerType.Storage;
        #endregion

        #region 私有字段
        /// <summary>
        /// 本地缓存数据工厂字典
        /// </summary>
        private Dictionary<string, IStorageDataFactory> dicAllStorageDataFactory;
        /// <summary>
        /// 缓存的数据字典
        /// </summary>
        private Dictionary<string, IStorageData> dicStorageData;
        /// <summary>
        /// 缓存数据接口
        /// </summary>
        private IStorage storage;

        [SerializeField]
        [BoxGroup("本地缓存管理器"), LabelText("脏标记检查频率(单位:秒)"), ShowInInspector, PropertyOrder(0)]
        private float checkFrequency = 60f;

        [BoxGroup("本地缓存管理器"), LabelText("脏标记计时器(单位:秒)"), ShowInInspector, PropertyOrder(0), ReadOnly]
        private float checkTimer;
        #endregion

        #region 接口实现
        async UniTask IInit.OnInit()
        {
            dicAllStorageDataFactory = new();
            dicStorageData = new();
            storage = new Storage_PlayerPrefs();
            checkTimer = 0f;

            #region 获取所有本地缓存数据工厂
            List<Type> listTypes = YRUtility.Assembly.GetTypes();
            foreach (Type type in listTypes)
            {
                StorageDataFactoryAttribute storageDataAttribute = type.GetCustomAttribute<StorageDataFactoryAttribute>(false);
                if (null == storageDataAttribute)
                    continue;

                if (dicAllStorageDataFactory.ContainsKey(storageDataAttribute.Key))
                {
                    Debug.LogError($"[{nameof(StorageManager)}]:已添加同类的本地缓存数据类型:{storageDataAttribute.Key}");
                    continue;
                }

                if (Activator.CreateInstance(type) is not IStorageDataFactory storageDataFactory)
                {
                    Debug.LogError($"[{nameof(StorageManager)}]:{storageDataAttribute.GetType().FullName}没有实现{nameof(IStorageDataFactory)}");
                    continue;
                }

                dicAllStorageDataFactory[storageDataAttribute.Key] = storageDataFactory;
            }
            #endregion

            await UniTask.CompletedTask;
        }

        void IUpdate.OnUpdate(float deltaTime, float realtimeSinceStartup)
        {
            checkTimer += deltaTime;

            if (checkTimer < checkFrequency)
                return;

            Save();
        }

        void IInit.OnRelease()
        {
            Save();

            if (null != dicAllStorageDataFactory)
            {
                dicAllStorageDataFactory.Clear();
                dicAllStorageDataFactory = null;
            }

            if (null != dicStorageData)
            {
                dicStorageData.Clear();
                dicStorageData = null;
            }
        }
        #endregion

        #region API
        /// <summary>
        /// 立即保存
        /// </summary>
        public void Save()
        {
            checkTimer = 0f;

            if (SetData())
                storage.Save();

            Debug.Log($"[{nameof(StorageManager)}]保存");
        }

        /// <summary>
        /// 清除所有缓存
        /// </summary>
        public void ClearAll()
        {
            storage.Clear();
            dicStorageData.Clear();
        }
        #endregion

        /// <summary>
        /// 获取本地缓存数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        internal T GetStorage<T>(string key) where T : IStorageData
        {
            if (!dicStorageData.TryGetValue(key, out IStorageData storageData))
            {
                dicStorageData.Add(key, dicAllStorageDataFactory[key].Create());

                string jsonData = storage.Load(key);
                if (!string.IsNullOrEmpty(jsonData))
                    storage.LoadToDic(key, jsonData, ref dicStorageData);

                storageData = dicStorageData[key];
            }

            return (T)storageData;
        }

        /// <summary>
        /// 设置数据
        /// </summary>
        private bool SetData()
        {
            bool isDirty = false;
            foreach (var kv in dicStorageData)
            {
                if (kv.Value is not IStorageData storageData)
                    continue;

                if (!storageData.IsDirty)
                    continue;

                isDirty = true;
                storage.Set(kv.Key, kv.Value);
            }

            return isDirty;
        }
    }
}