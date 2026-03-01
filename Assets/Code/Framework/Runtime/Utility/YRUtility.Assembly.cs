using System;
using System.Collections.Generic;

namespace YRFramework.Runtime.Utility
{
    public static partial class YRUtility
    {
        /// <summary>
        /// 程序集实用函数
        /// </summary>
        public static class Assembly
        {
            /// <summary>
            /// 程序集数组
            /// </summary>
            private static readonly System.Reflection.Assembly[] assemblies;
            /// <summary>
            /// 类型字典
            /// </summary>
            private static readonly Dictionary<string, Type> dicCachedType = new(StringComparer.Ordinal);

            static Assembly()
            {
                assemblies = AppDomain.CurrentDomain.GetAssemblies();
            }

            /// <summary>
            /// 获取所有程序集
            /// </summary>
            /// <returns></returns>
            public static System.Reflection.Assembly[] GetAssemblies()
            {
                return assemblies;
            }

            /// <summary>
            /// 获取已加载程序集中的所有类型
            /// </summary>
            /// <returns></returns>
            public static List<Type> GetTypes()
            {
                List<Type> listType = new();
                foreach (System.Reflection.Assembly assembly in assemblies)
                {
                    listType.AddRange(assembly.GetTypes());
                }
                
                return listType;
            }

            /// <summary>
            /// 获取已加载的程序集中的所有类型。
            /// </summary>
            /// <param name="results">已加载的程序集中的所有类型。</param>
            public static void GetTypes(ref List<Type> listType)
            {
                listType ??= new List<Type>();
                listType.Clear();

                foreach (System.Reflection.Assembly assembly in assemblies)
                {
                    listType.AddRange(assembly.GetTypes());
                }
            }

            /// <summary>
            /// 获取已加载的程序集中的指定类型。
            /// </summary>
            /// <param name="typeName">要获取的类型名。</param>
            /// <returns>已加载的程序集中的指定类型。</returns>
            public static Type GetType(string typeName)
            {
                if (string.IsNullOrWhiteSpace(typeName))
                    throw new Exception("类型名无效");

                if (dicCachedType.TryGetValue(typeName, out Type type))
                    return type;

                type = Type.GetType(typeName);
                if (null != type)
                {
                    dicCachedType.Add(typeName, type);
                    return type;
                }

                foreach (System.Reflection.Assembly assembly in assemblies)
                {
                    type = Type.GetType(YRUtility.Text.Format("{0}, {1}", typeName, assembly.FullName));
                    if (null != type)
                    {
                        dicCachedType.Add(typeName, type);
                        return type;
                    }
                }

                return null;
            }
        }
    }
}