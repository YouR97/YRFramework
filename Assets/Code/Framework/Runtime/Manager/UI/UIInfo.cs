namespace YRFramework.Runtime.UI
{
    /// <summary>
    /// UI信息
    /// </summary>
    public struct UIInfo
    {
        /// <summary>
        /// UI类型（UI名）
        /// </summary>
        public string Type { get; private set; }

        /// <summary>
        /// UI组类型
        /// </summary>
        public E_UIGroupType GroupType { get; private set; }

        /// <summary>
        /// 是否暂停被覆盖的界面
        /// </summary>
        public E_UIShowType WindowType { get; private set; }

        /// <summary>
        /// 是否焦点界面（焦点界面抢夺输入控制权）
        /// </summary>
        public bool IsCentre { get; private set; }

        /// <summary>
        /// UI工厂
        /// </summary>
        internal IUIFactory UIFactory { get; private set; }

        internal UIInfo(string type, E_UIGroupType groupType, E_UIShowType windowType, bool isCentre, IUIFactory uiFactory)
        {
            Type = type;
            GroupType = groupType;
            WindowType = windowType;
            IsCentre = isCentre;
            UIFactory = uiFactory;
        }
    }
}