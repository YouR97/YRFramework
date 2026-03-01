using System;
using System.Collections;
using System.Collections.Generic;

namespace YRFramework.Runtime.Collections
{
    public sealed class StrongList<T> : IEnumerator<T>, IEnumerable<T>
    {
        /// <summary>
        /// 数据列表
        /// </summary>
        private readonly List<T> listData;
        /// <summary>
        /// 当前索引
        /// </summary>
        private int curIndex;
        /// <summary>
        /// 当前数据
        /// </summary>
        private T curData;
        /// <summary>
        /// 是否保持顺序
        /// </summary>
        private bool isKeepOrder;
#if UNITY_EDITOR
        /// <summary>
        /// 遍历添加的数量
        /// </summary>
        private int foreachAddCount;
#endif

        /// <summary>
        /// 是否锁住
        /// </summary>
        private bool IsLock { get { return curIndex >= 0; } }

        /// <summary>
        /// 当前数据
        /// </summary>
        public T Current { get { return curData; } }

        public IReadOnlyList<T> ListData { get { return listData; } }

        object IEnumerator.Current { get { return curData; } }

        public int Count { get { return listData.Count; } }

        public StrongList(int capacity = 0, bool isKeepOrder = false)
        {
            listData = new List<T>(capacity);
            curIndex = -1;
            curData = default;
            this.isKeepOrder = isKeepOrder;
        }

        #region API
        public void Add(T value)
        {
#if UNITY_EDITOR
            if (-1 != curIndex)
            {
                if (100 == ++foreachAddCount)
                {
                    foreachAddCount = 0;
                    throw new Exception($"[{nameof(StrongList<T>)}]:在循环中不断地加入，死循环");
                }
            }
#endif

            listData.Add(value);
        }

        public bool Remove(T value)
        {
            if (!IsLock)
                return isKeepOrder ? listData.Remove(value) : listData.RemoveSwapBack(value);

            int index = listData.IndexOf(value);
            if (-1 == index)
                return false;

            if (index > curIndex - 1)
                return isKeepOrder ? listData.Remove(value) : listData.RemoveSwapBack(value);
            else if (index < curIndex - 1)
            {
                if (isKeepOrder)
                    listData.RemoveAt(index);
                else
                {
                    listData[index] = listData[curIndex - 1];
                    listData.RemoveAtSwapBack(curIndex - 1);
                }

                --curIndex;
            }
            else
            {
                if (isKeepOrder)
                    listData.Remove(value);
                else
                    listData.RemoveSwapBack(value);

                --curIndex;
            }

            return true;
        }

        public bool Contains(T value)
        {
            return listData.Contains(value);
        }

        public bool MoveNext()
        {
            if (listData.Count == curIndex)
            {
                curData = default;
                curIndex = -1;
                return false;
            }
            else
            {
                curData = listData[curIndex++];
                return true;
            }
        }

        public void Reset()
        {
            if (IsLock)
                throw new Exception($"[{nameof(StrongList<T>)}]:Enumerator被锁住");

            curIndex = 0;
            curData = default;
        }

        public void Clear()
        {
            curIndex = -1;
            curData = default;
            listData.Clear();
        }

        public IEnumerator<T> GetEnumerator()
        {
            Reset();
            return this;
        }

        public void Dispose()
        {
            curIndex = -1;
            curData = default;
        }
        #endregion

        IEnumerator IEnumerable.GetEnumerator()
        {
            Reset();
            return this;
        }
    }
}