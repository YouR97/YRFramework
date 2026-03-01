using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YRFramework.Runtime;
using YRFramework.Runtime.UI;

namespace GamePlay.Runtime.UI
{
    /// <summary>
    /// 战斗界面
    /// </summary>
    public class UI_FightController : UIControllerBase
    {
        /// <summary>
        /// 桃心
        /// </summary>
        private sealed class UI_SelfLife : UIGameObjectNode
        {
            /// <summary>
            /// 桃心
            /// </summary>
            private Image imageLifeIcon;

            public UI_SelfLife(GameObject root) : base(root)
            {
                imageLifeIcon = rc.RcGetComponent<Image>("ImageLifeIcon");
            }

            public void SetInfo(bool isHealth)
            {
                imageLifeIcon.gameObject.IsScaleShow(isHealth);

                IsScaleShow = true;
            }
        }

        #region UI
        /// <summary>
        /// 怪物名
        /// </summary>
        private TMP_Text textEnemyName;
        /// <summary>
        /// 怪物生命值
        /// </summary>
        private TMP_Text textEnemyHp;
        /// <summary>
        /// 怪物血量百分比
        /// </summary>
        private TMP_Text textEnemyHpPercentage;
        /// <summary>
        /// 累计造成伤害
        /// </summary>
        private TMP_Text textTotalDamage;
        /// <summary>
        /// 当前伤害
        /// </summary>
        private TMP_Text textCurDamage;
        /// <summary>
        /// 已经行动次数
        /// </summary>
        private TMP_Text textActionCount;
        /// <summary>
        /// 剩余交换次数
        /// </summary>
        private TMP_Text textExchangeCount;
        /// <summary>
        /// 怪物立绘
        /// </summary>
        private Image imageEnemyIcon;
        /// <summary>
        /// 攻击
        /// </summary>
        private Button btnAttack;
        /// <summary>
        /// 交换
        /// </summary>
        private Button btnExchange;
        /// <summary>
        /// 技能
        /// </summary>
        private Button btnSkill;
        /// <summary>
        /// 生命值布局
        /// </summary>
        private GridLayoutGroup gridSelfLife;
        #endregion

        /// <summary>
        /// 玩家生命数量对应大小数组
        /// </summary>
        private readonly int[] LIFE_SIZES = new int[10] { 150, 150, 150, 110, 80, 80, 80, 80, 80, 80 };
        /// <summary>
        /// 玩家生命列表
        /// </summary>
        private List<UI_SelfLife> listSelfLife;

        protected override void Awake(ReferenceCollector rc)
        {
            textEnemyName = rc.RcGetComponent<TMP_Text>("TextEnemyName");
            textEnemyHp = rc.RcGetComponent<TMP_Text>("TextEnemyHp");
            textEnemyHpPercentage = rc.RcGetComponent<TMP_Text>("TextEnemyHpPercentage");
            textTotalDamage = rc.RcGetComponent<TMP_Text>("TextTotalDamageValue");
            textCurDamage = rc.RcGetComponent<TMP_Text>("TextCurDamage");
            textActionCount = rc.RcGetComponent<TMP_Text>("TextActionCount");
            textExchangeCount = rc.RcGetComponent<TMP_Text>("TextExchangeCount");
            textEnemyName = rc.RcGetComponent<TMP_Text>("TextEnemyName");
            imageEnemyIcon = rc.RcGetComponent<Image>("ImageEnemyIcon");
            btnAttack = rc.RcGetComponent<Button>("ButtonAttack");
            btnExchange = rc.RcGetComponent<Button>("ButtonExchange");
            btnSkill = rc.RcGetComponent<Button>("ButtonSkill");
            gridSelfLife = rc.RcGetComponent<GridLayoutGroup>("SelfLifeRoot");

            Transform tsSelfLifeRoot = gridSelfLife.transform;
            listSelfLife = new List<UI_SelfLife>(tsSelfLifeRoot.childCount);
            for (int i = 0; i < tsSelfLifeRoot.childCount; ++i)
            {
                listSelfLife.Add(new UI_SelfLife(tsSelfLifeRoot.GetChild(i).gameObject));
            }

            Utility.UI.BindButton(btnAttack, OnAttack);
            Utility.UI.BindButton(btnExchange, OnExchange);
            Utility.UI.BindButton(btnSkill, OnSkill);
        }

        public void Open()
        {
            int selfLifeSize = LIFE_SIZES[Game.Fight.MaxSelfHp];
            gridSelfLife.cellSize = new Vector2(selfLifeSize, selfLifeSize);
            for (int i = 0; i < listSelfLife.Count; ++i)
            {
                UI_SelfLife ui_SelfLife = listSelfLife[i];
                ui_SelfLife.SetInfo(i < Game.Fight.SelfHp);
                ui_SelfLife.IsActive = i < Game.Fight.MaxSelfHp;
            }
        }

        public override void Close()
        {
        }

        public override void Dispose()
        {
            if (null != listSelfLife)
            {
                foreach (var i in listSelfLife)
                {
                    i.Dispose();
                }
                listSelfLife.Clear();
                listSelfLife = null;
            }

            base.Dispose();
        }

        #region 回调
        /// <summary>
        /// 攻击
        /// </summary>
        private void OnAttack()
        {
            // TODO 攻击
        }

        /// <summary>
        /// 交换
        /// </summary>
        private void OnExchange()
        { 

        }

        /// <summary>
        /// 技能
        /// </summary>
        private void OnSkill()
        {

        }
        #endregion
    }
}