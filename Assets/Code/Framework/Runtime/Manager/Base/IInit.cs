using Cysharp.Threading.Tasks;

namespace YRFramework.Runtime
{
    /// <summary>
    /// 初始化接口
    /// </summary>
    public interface IInit
    {
        /// <summary>
        /// 和Release成对
        /// </summary>
        UniTask OnInit();

        /// <summary>
        /// 和Init成对
        /// </summary>
         void OnRelease();
    }
}