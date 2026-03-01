using System.Collections.Generic;
using UnityEngine;

namespace YRFramework.Runtime
{
    /// <summary>
    /// 容器扩展类
    /// </summary>
    public static class CollectionExtension
    {
        /// <summary>
        /// 确保List容量
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="capacit"></param>
        public static void EnsureCapacity<T>(this List<T> list, int capacit)
        {
            if (list.Capacity < capacit)
                list.Capacity = capacit;
        }

        /// <summary>
        /// 确保List大小并填充默认值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="size"></param>
        public static void EnsureSize<T>(this List<T> list, int size)
        {
            if (list.Capacity < size)
                list.Capacity = size;

            for (int i = list.Count; i < size; ++i)
                list.Add(default);
        }

        /// <summary>
        /// 确保List大小并填充指定值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="size"></param>
        /// <param name="defaultValue"></param>
        public static void EnsureSize<T>(this List<T> list, int size, T defaultValue)
        {
            if (list.Capacity < size)
                list.Capacity = size;

            for (int i = list.Count; i < size; ++i)
                list.Add(defaultValue);
        }

        /// <summary>
        /// 重置容量并填充默认值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="capacit"></param>
        /// <returns></returns>
        public static List<T> ReCapacity<T>(this List<T> list, int capacit)
        {
            if (list.Capacity < capacit)
                list.Capacity = capacit;

            if (capacit < list.Count)
                list.RemoveRange(capacit, list.Count - capacit);

            for (int i = list.Count; i < capacit; ++i)
                list.Add(default);

            return list;
        }

        /// <summary>
        /// 重置容量并填充指定值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="size"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static List<T> ReCapacity<T>(this List<T> list, int capacit, T defaultValue)
        {
            if (list.Capacity < capacit)
                list.Capacity = capacit;

            if (capacit < list.Count)
                list.RemoveRange(capacit, list.Count - capacit);

            for (int i = list.Count - 1; i >= 0; --i)
                list[i] = defaultValue;

            for (int i = list.Count; i < capacit; ++i)
                list.Add(defaultValue);

            return list;
        }

        public static T Peek<T>(this List<T> list)
        {
            return list[^1];
        }

        public static bool TryPeek<T>(this List<T> list, out T value)
        {
            if (list.Count > 0)
            {
                value = list[^1];
                return true;
            }

            value = default;
            return false;
        }

        public static T Pop<T>(this List<T> list)
        {
            int tail = list.Count - 1;
            T value = list[tail];
            list.RemoveAt(tail);

            return value;
        }

        public static bool TryPop<T>(this List<T> list, out T value)
        {
            if (list.Count > 0)
            {
                int tail = list.Count - 1;
                value = list[tail];
                list.RemoveAt(tail);

                return true;
            }

            value = default;
            return false;
        }

        /// <summary>
        /// 先交换再删除指定位置
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="index"></param>
        public static void RemoveAtSwapBack<T>(this List<T> list, int index)
        {
            int tail = list.Count - 1;
            if (index != tail)
                list[index] = list[tail];

            list.RemoveAt(tail);
        }

        /// <summary>
        /// 先交换再删除指定位置
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static T RemoveAtSwapBack<T>(this List<T> list, ref int index)
        {
            T removed = list[index];
            int tail = list.Count - 1;
            if (index != tail)
                list[index] = list[tail];
            else
                index = -1;

            list.RemoveAt(tail);

            return removed;
        }

        /// <summary>
        /// 先交换再删除指定元素
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool RemoveSwapBack<T>(this List<T> list, T value)
        {
            int index = list.IndexOf(value);
            if (index != -1)
            {
                list.RemoveAtSwapBack(index);
                return true;
            }

            return false;
        }

        /// <summary>
        /// 把指定位置元素移动多少位置
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="index"></param>
        /// <param name="step"></param>
        /// <returns></returns>
        public static int MoveAt<T>(this List<T> list, int index, int step)
        {
            if (index < 0 || index >= list.Count)
                return -1;

            int goal = index + step;
            if (goal < 0 || goal >= list.Count)
                return -1;

            T value = list[index];
            list.RemoveAt(index);
            list.Insert(goal, value);

            return goal;
        }

        /// <summary>
        /// 把指定元素移动多少位置
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="value"></param>
        /// <param name="step"></param>
        /// <returns></returns>
        public static int Move<T>(this List<T> list, T value, int step)
        {
            return list.MoveAt(list.IndexOf(value), step);
        }
    }
}