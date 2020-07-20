using HDIPlatform.DesignSystem.Domain.Entities.FixedMealContext;
using ShSoft.Infrastructure.DomainServiceBase;

namespace HDIPlatform.DesignSystem.Domain.IDomainServices
{
    /// <summary>
    /// 套餐模板领域服务接口
    /// </summary>
    public interface IDecorationPackService : IDomainService<DecorationPack>
    {
        #region # 重置排序 —— void InitSorts()
        /// <summary>
        /// 重置排序
        /// </summary>
        void InitSorts();
        #endregion
    }
}
