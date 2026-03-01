using System;
using UnityEngine;
using YRFramework.Runtime;

namespace GamePlay.Runtime.Condition
{
    /// <summary>
    /// 条件执行者
    /// </summary>
    public sealed class ConditionExecuter : IDisposable
    {
        /// <summary>
        /// 是否释放
        /// </summary>
        private bool isDisposed;
        /// <summary>
        /// 回调
        /// </summary>
        public ConditionCallBack successAction;

        #region 属性
        /// <summary>
        /// id
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// 条件
        /// </summary>
        public ConditionLogicBase ConditionLogic { get; private set; }
        #endregion

        ~ConditionExecuter()
        {
            Dispose(false);
        }

        #region API
        /// <summary>
        /// 初始化数据
        /// </summary>
        /// <param name="id"></param>
        /// <param name="conditionLogic"></param>
        /// <param name="success"></param>
        public void InitData(int id, ConditionLogicBase conditionLogic, ConditionCallBack success)
        {
            Id = id;
            ConditionLogic = conditionLogic;
            successAction = success;

            isDisposed = false;
        }

        /// <summary>
        /// 执行
        /// </summary>
        public void Execute(bool isRecheck)
        {
            bool isSuccess;
            try
            {
                if (isRecheck)
                    ConditionLogic.SetIsRecheck();

                isSuccess = ConditionLogic.Check();
            }
            catch (Exception e)
            {
                Debug.LogError($"[{nameof(ConditionExecuter)}]条件非法！ {e.Message}\r\n{e.StackTrace}");
                return;
            }

            successAction?.Invoke(isSuccess, Id);
        }
        #endregion

        #region IDisposable
        private void Dispose(bool isDisposing)
        {
            if (!isDisposed)
            {
                if (isDisposing)
                {
                    // TODO: 释放托管状态(托管对象)
                }

                // TODO: 释放未托管的资源(未托管的对象)并重写终结器
                // TODO: 将大型字段设置为 null
                if (null != ConditionLogic)
                {
                    Id = YRConsts.INVALID_INT;
                    successAction = null;
                    if (null != ConditionLogic)
                    {
                        ConditionLogic.Recycle();
                        ConditionLogic = null;
                    }
                }

                isDisposed = true;
            }
        }

        // // TODO: 仅当“Dispose(bool disposing)”拥有用于释放未托管资源的代码时才替代终结器
        // ~ConditionExecuter()
        // {
        //     // 不要更改此代码。请将清理代码放入“Dispose(bool disposing)”方法中
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // 不要更改此代码。请将清理代码放入“Dispose(bool disposing)”方法中
            Dispose(isDisposing: true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}