using cfg.Condition;
using System;
using System.Collections.Generic;
using YRFramework.Runtime;

namespace GamePlay.Runtime.Condition
{
    /// <summary>
    /// 条件逻辑基类
    /// </summary>
    public abstract class ConditionLogicBase : IDisposable
    {
        /// <summary>
        /// 是否释放
        /// </summary>
        private bool isDisposed;

        /// <summary>
        /// 是否重新检查
        /// </summary>
        protected bool isRecheck;
        /// <summary>
        /// 缓存的结果
        /// </summary>
        protected bool cacheResult;
        /// <summary>
        /// 缓存的本地化文本
        /// </summary>
        protected string cacheLocalization;
        /// <summary>
        /// 缓存的进度
        /// </summary>
        protected float cacheProgress;

        #region 属性
        /// <summary>
        /// 条件执行者id
        /// </summary>
        public int ConditionExecuterId { get; private set; }

        /// <summary>
        /// 条件类型
        /// </summary>
        public E_ConditionType ConditionType { get; private set; }

        /// <summary>
        /// 父节点
        /// </summary>
        public ConditionLogicBase ParentNode { get; private set; }

        /// <summary>
        /// 是否根条件
        /// </summary>
        public bool IsRootCondition { get { return null == ParentNode; } }

        /// <summary>
        /// 触发事件类型数组
        /// </summary>
        public abstract E_EventType[] EventTypes { get; }
        #endregion

        public ConditionLogicBase(E_ConditionType conditionType)
        {
            ConditionType = conditionType;
        }

        public virtual void InitData(int conditionExecuterId, ConditionLogicBase parentNode, IList<string> listArg)
        {
            ConditionExecuterId = conditionExecuterId;
            ParentNode = parentNode;

            Game.Condition.RegisterConditionEvent(EventTypes, this);

            isRecheck = true;
            isDisposed = false;
        }

        /// <summary>
        /// 设置是否重新检查状态
        /// </summary>
        /// <param name="isRecheck"></param>
        public virtual void SetIsRecheck()
        {
            isRecheck = true;
        }

        ~ConditionLogicBase()
        {
            Dispose(false);
        }

        /// <summary>
        /// 检查校验
        /// </summary>
        /// <returns></returns>
        public bool Check()
        {
            if (isRecheck)
            {
                OnCheck();

                isRecheck = false;
            }

            return cacheResult;
        }

        /// <summary>
        /// 获取条件本地化
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string GetLocalization()
        {
            Check();

            return cacheLocalization;
        }

        /// <summary>
        /// 获取进度
        /// </summary>
        /// <returns></returns>
        public float GetProgress()
        {
            Check();

            return cacheProgress;
        }

        /// <summary>
        /// 回收
        /// </summary>
        public abstract void Recycle();

        /// <summary>
        /// 检查
        /// </summary>
        /// <returns></returns>
        protected abstract void OnCheck();

        #region IDispose
        protected virtual void Dispose(bool isDisposing)
        {
            if (!isDisposed)
            {
                if (isDisposing)
                {
                }

                // TODO: 释放未托管的资源(未托管的对象)并重写终结器
                // TODO: 将大型字段设置为 null

                ConditionExecuterId = YRConsts.INVALID_INT;
                ConditionType = E_ConditionType.None;
                ParentNode = null;
                isRecheck = false;
                cacheResult = false;
                cacheLocalization = string.Empty;
                cacheProgress = 0f;
                Game.Condition.UnRegisterConditionEvent(EventTypes, this);

                isDisposed = true;
            }
        }

        // // TODO: 仅当“Dispose(bool disposing)”拥有用于释放未托管资源的代码时才替代终结器
        // ~ConditionLogicBase()
        // {
        //     // 不要更改此代码。请将清理代码放入“Dispose(bool disposing)”方法中
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // 不要更改此代码。请将清理代码放入“Dispose(bool disposing)”方法中
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}