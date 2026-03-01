using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;
using YRFramework.Runtime.Utility;

namespace YRFramework.Runtime.Storage
{
    /// <summary>
    /// 本地存储实现
    /// </summary>
    public class Storage_PlayerPrefs : IStorage
    {
        private bool isDirty;

        /// <summary>
        /// Json序列化设置
        /// </summary>
        private readonly JsonSerializerSettings settings;

        #region IStorage实现
        /// <summary>
        ///  脏标记
        /// </summary>
        public bool IsDirty
        {
            get { return isDirty; }
            protected set { isDirty = value; }
        }

        public Storage_PlayerPrefs()
        {
            settings = new()
            {
                NullValueHandling = NullValueHandling.Ignore,
            };
        }

        string IStorage.Load(string key)
        {
            string encodeKey = YRUtility.Encode.Base64Encode(key); // 编码后的key

            string jsonData;
            if (PlayerPrefs.HasKey(encodeKey))
            {
                try
                {
                    byte[] encryptData = Convert.FromBase64String(PlayerPrefs.GetString(encodeKey));
                    jsonData = YRUtility.Rijndael.Decrypt(encryptData);
                }
                catch (Exception e)
                {
                    Debug.LogError($"[{nameof(Storage_PlayerPrefs)}]:{key} : {e.Message}");
                    // 如果解析失败，删除本地数据
                    PlayerPrefs.DeleteKey(encodeKey);

                    return null;
                }
            }
            else
            {
                Debug.Log($"[{nameof(Storage_PlayerPrefs)}]:无本地存储数据:{key}！");

                return null;
            }

            return jsonData;
        }

        void IStorage.LoadToDic(string key, string jsonData, ref Dictionary<string, IStorageData> dicStorage)
        {
            try
            {
                if (string.IsNullOrEmpty(jsonData))
                    return;

                JObject jObj = JObject.Parse(jsonData);

                if (dicStorage.TryGetValue(key, out IStorageData storageData))
                    JsonConvert.PopulateObject(jObj.ToString(), storageData, settings);
            }
            catch (JsonReaderException ex)
            {
                Debug.LogError($"[{nameof(Storage_PlayerPrefs)}]:JSON 解析异常: {ex.Message}");
            }
            catch (Exception ex)
            {
                Debug.LogError($"[{nameof(Storage_PlayerPrefs)}:发生异常: {ex.Message}");
            }
        }

        void IStorage.Set<T>(string key, T t)
        {
            string encodeKey = YRUtility.Encode.Base64Encode(key); // 编码后的key
            string saveStr = Convert.ToBase64String(EncryptData(t));
            PlayerPrefs.SetString(encodeKey, saveStr);
        }

        public void Save()
        {
            PlayerPrefs.Save();
        }

        void IStorage.Clear()
        {
            PlayerPrefs.DeleteAll();
            Save();
        }
        #endregion

        /// <summary>
        /// 数据加密
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objectdata"></param>
        /// <returns></returns>
        private byte[] EncryptData<T>(T objectdata)
        {
            string jsonData = JsonConvert.SerializeObject(objectdata, settings);
            byte[] encryptData = YRUtility.Rijndael.Encrypt(jsonData);

            return encryptData;
        }
    }
}