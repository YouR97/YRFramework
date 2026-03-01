using System.Collections.Generic;
using UnityEngine;
using YRFramework.Runtime;

namespace GamePlay.Runtime.Fight
{
    /// <summary>
    /// 战斗管理器
    /// </summary>
    public sealed partial class FightManager
    {
        /// <summary>
        /// 战斗点位字典
        /// </summary>
        private Dictionary<E_FightPos, List<Transform>> dicFightListPos;

        /// <summary>
        /// 初始化点位
        /// </summary>
        private void InitPoint()
        {
            dicFightListPos = new Dictionary<E_FightPos, List<Transform>>
            {
                [E_FightPos.PlayerPos] = new List<Transform>(1),
                [E_FightPos.EnemyPos] = new List<Transform>(10),
            };
        }

        /// <summary>
        /// 加载点位
        /// </summary>
        private void LoadPoint(ReferenceCollector rc)
        {
            List<Transform> listPoint = new();
            listPoint.Add(rc.RcGetGameObject($"{E_FightPos.PlayerPos}").transform);
            dicFightListPos[E_FightPos.PlayerPos] = listPoint;
            
            //dicFightListPos[E_FightPos.EnemyPos] = rc.RcGetGameObject($"{E_FightPos.EnemyPos}").transform;
        }

        private void UnloadPoint()
        {
            dicFightListPos.Clear();
        }

        /// <summary>
        /// 设置点位
        /// </summary>
        private void SetPos()
        {
            
        }
    }
}