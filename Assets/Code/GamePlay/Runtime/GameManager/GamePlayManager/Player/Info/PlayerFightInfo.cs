using UnityEngine;

namespace GamePlay.Runtime.Player
{
    /// <summary>
    /// 玩家战斗信息
    /// </summary>
    public sealed class PlayerFightInfo
    {
        #region 私有字段
        /// <summary>
        /// 最大生命点数
        /// </summary>
        private int maxHpPoint;
        /// <summary>
        /// 当前生命点数
        /// </summary>
        private int curHpPoint;
        #endregion

        #region 属性
        /// <summary>
        /// 最大生命点数
        /// </summary>
        public int MaxHpPoint { get { return maxHpPoint; } }
        /// <summary>
        /// 当前生命点数
        /// </summary>
        public int CurHpPoint { get{ return curHpPoint; } }
        #endregion

        public PlayerFightInfo()
        {
            maxHpPoint = 1;
            curHpPoint = 1;
        }
    }
}