using cfg;
using cfg.Audio;
using cfg.Enemy;
using cfg.Entity;
using cfg.Level;
using SimpleJSON;
using UnityEngine;
using YRFramework.Runtime.Manager;

namespace YRFramework.Runtime.DataTable
{
    /// <summary>
    /// 配置管理器
    /// </summary>
    [DisallowMultipleComponent]
    [AddComponentMenu("YRFramework/DataTableManager")]
    public sealed partial class DataTableManager : YRFrameworkManager
    {
        #region Base
        public override E_FrameworkManagerType ManagerType { get; protected set; } = E_FrameworkManagerType.DataTable;
        #endregion

        /// <summary>
        /// 配置表
        /// </summary>
        private Tables tables;

        #region 属性
        /// <summary>
        /// 音效表
        /// </summary>
        public AudioConfig AudioConfig { get { return tables.AudioConfig; } }
        /// <summary>
        /// 关卡表
        /// </summary>
        public LevelConfig LevelConfig { get { return tables.LevelConfig; } }
        /// <summary>
        /// 实体表
        /// </summary>
        public EntityConfig EntityConfig { get { return tables.EntityConfig; } }
        /// <summary>
        /// 怪物表
        /// </summary>
        public EnemyConfig EnemyConfig { get { return tables.EnemyConfig; } }
        #endregion

        /// <summary>
        /// 加载数据表
        /// </summary>
        public void LoadDataTable()
        {
            tables = new Tables(LoadTableFile);
        }

        /// <summary>
        /// 加载配置
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        private JSONNode LoadTableFile(string file)
        {
            TextAsset textAsset = FrameworkGameEnter.Asset.LoadAsset<TextAsset>(file);
            if (null == textAsset)
            {
                Debug.LogError($"加载配置错误：{file}");
                return null;
            }
            JSONNode jsonNode = JSON.Parse(textAsset.text);
            FrameworkGameEnter.Asset.Unload(file);

            return jsonNode;
        }
    }
}