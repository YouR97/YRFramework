using cfg.Condition;
using System.Collections.Generic;
using YRFramework.Runtime;

namespace GamePlay.Runtime.Condition
{
    /// <summary>
    /// (框架)复合条件
    /// </summary>
    public sealed class Condition_CompositeLogic : ConditionLogicBase
    {
        #region 私有变量
        /// <summary>
        /// 子条件列表
        /// </summary>
        private readonly List<ConditionLogicBase> listChildren = new(2);
        /// <summary>
        /// 条件操作类型
        /// </summary>
        private E_ConditionOperator conditionOperator;
        #endregion

        #region 属性
        /// <summary>
        /// 触发事件类型数组
        /// </summary>
        public override E_EventType[] EventTypes => new E_EventType[0] { };
        #endregion

        public Condition_CompositeLogic() : base(E_ConditionType.Composite)
        {
        }

        public void InitData(E_ConditionOperator cOperator, int conditionExecuterId, ConditionLogicBase parentNode, ConditionLogicBase left, ConditionLogicBase right)
        {
            InitData(conditionExecuterId, parentNode, null);

            conditionOperator = cOperator;
            listChildren.Add(left);
            listChildren.Add(right);
        }

        #region API
        public override void SetIsRecheck()
        {
            base.SetIsRecheck();

            foreach (ConditionLogicBase child in listChildren) 
            {
                child.SetIsRecheck();
            }
        }

        public override void Recycle()
        {
            if (null != listChildren)
            {
                foreach (ConditionLogicBase conditionLogic in listChildren)
                {
                    conditionLogic.Recycle();
                }
            }

            Game.ReferencePool.Release(this);
        }
        #endregion

        protected override void OnCheck()
        {
            if (listChildren.Count <= 0)
            {
                cacheResult = true;
                cacheProgress = 1f;
                cacheLocalization = string.Empty;

                return;
            }

            if (E_ConditionOperator.AND == conditionOperator) // 与
            {
                foreach (ConditionLogicBase child in listChildren)
                {
                    if (!child.Check())
                    {
                        cacheResult = false;
                        return;
                    }
                }

                cacheResult = true;
                return;
            }
            else // 或
            {
                foreach (ConditionLogicBase child in listChildren)
                {
                    if (child.Check())
                    {
                        cacheResult = true;
                        return;
                    }
                }

                cacheResult = false;
                return;
            }
        }

        protected override void Dispose(bool disposing)
        {
            listChildren?.Clear();

            base.Dispose(disposing);
        }
    }
}