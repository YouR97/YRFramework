using System;

namespace YRFramework.Runtime.UI
{
    /// <summary>
    /// UI工厂特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class UIFactoryAttribute : Attribute
    {
        /// <summary>
        /// UI名
        /// </summary>
        public string UIName { get; private set; }

        /// <summary>
        /// UI组类型
        /// </summary>
        public E_UIGroupType UIGroupType { get; private set; }

        /// <summary>
        /// 窗口类型
        /// </summary>
        public E_UIShowType WindowType { get; private set; }

        /// <summary>
        /// 是否焦点界面（焦点界面打开会抢夺焦点）
        /// </summary>
        public bool IsCentre { get; private set; }

        private UIFactoryAttribute()
        { }

        /// <summary>
        /// UI工厂特性
        /// </summary>
        /// <param name="uiName">UI名</param>
        /// <param name="uiGroupType">UI组</param>
        /// <param name="windowType">窗口类型，全屏界面会暂停(隐藏)被遮挡的UI</param>
        /// <param name="isCentre">是否焦点界面，焦点界面会抢夺焦点</param>
        public UIFactoryAttribute(string uiName, E_UIGroupType uiGroupType, E_UIShowType windowType, bool isCentre)
        {
            UIName = uiName;
            UIGroupType = uiGroupType;
            WindowType = windowType;
            IsCentre = isCentre;
        }
    }
}