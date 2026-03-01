using System;
using System.Collections.Generic;
using UnityEngine.Profiling;

namespace YRFramework.Runtime.Profiling
{
    public struct YRProfiler : IDisposable
    {
#if UNITY_EDITOR
        private static Dictionary<Type, string> dicCacheTypeName = new();
#endif

        public YRProfiler(string tag)
        {
#if UNITY_EDITOR
            Profiler.BeginSample(string.IsNullOrEmpty(tag) ? "unknown" : tag);
#endif
        }

        public YRProfiler(object obj)
        {
#if UNITY_EDITOR
            var type = obj?.GetType();
            var tag = "unknown";
            if (type != null && !dicCacheTypeName.TryGetValue(type, out tag))
            {
                tag = type.Name;
                dicCacheTypeName.Add(type, tag);
            }

            Profiler.BeginSample(tag);
#endif
        }

        public void Dispose()
        {
#if UNITY_EDITOR
            Profiler.EndSample();
#endif
        }
    }
}