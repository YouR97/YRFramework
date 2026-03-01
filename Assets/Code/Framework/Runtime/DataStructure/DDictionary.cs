using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YRFramework.Runtime.Collections
{
    /// <summary>
    /// 二维字典
    /// </summary>
    public class DDictionary<T, K, V> : IEnumerable
    {
        private readonly Dictionary<T, Dictionary<K, V>> dicTKV = new();

        public void Add(T t, K k, V v)
        {
            if (!dicTKV.TryGetValue(t, out Dictionary<K, V> dicKV))
            {
                dicKV = new();
                dicTKV.Add(t, dicKV);
            }

            if (!dicKV.TryAdd(k, v))
                Debug.LogWarning($"[{nameof(DDictionary<T, K, V>)}]:存在K：{typeof(K)}");
        }

        public Dictionary<K, V> RemoveKey(T t)
        {
            return dicTKV.Remove(t, out Dictionary<K, V> dicKV) ? dicKV : null;
        }

        public V RemoveSubKey(T t, K k)
        {
            if (!dicTKV.TryGetValue(t, out Dictionary<K, V> dicKV) || !dicKV.Remove(k, out V value))
                return default;

            return value;
        }

        public bool TryGetValue(T t, K k, out V value)
        {
            if (!dicTKV.TryGetValue(t, out Dictionary<K, V> dicKV) || !dicKV.TryGetValue(k, out value))
            {
                value = default;
                return false;
            }

            return true;
        }

        public bool TryGetDic(T t, out Dictionary<K, V> dicKV)
        {
            if (!dicTKV.TryGetValue(t, out dicKV))
                return false;

            return true;
        }

        public bool ContainsKey(T t)
        {
            return dicTKV.ContainsKey(t);
        }

        public bool ContainsKeyAndSubKey(T t, K k)
        {
            if (!dicTKV.TryGetValue(t, out Dictionary<K, V> dicKV) || !dicKV.ContainsKey(k))
                return false;

            return true;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<V> GetEnumerator()
        {
            foreach (Dictionary<K, V> dicKV in dicTKV.Values)
            {
                foreach (V value in dicKV.Values)
                {
                    yield return value;
                }
            }
        }
    }
}