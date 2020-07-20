using HDIPlatform.DesignSystem.Domain.Entities.ConfigContext;
using HDIPlatform.DesignSystem.Domain.Entities.FixedMealContext;
using ShSoft.Infrastructure.DomainServiceBase;

namespace HDIPlatform.DesignSystem.Domain.IDomainServices
{
    /// <summary>
    /// 套餐系列领域服务接口
    /// </summary>
    public interface IPackSeriesService : IDomainService<PackSeries>
    {
        #region # 重置排序 —— void ReSort()
        /// <summary>
        /// 重置排序
        /// </summary>
        void ReSort();
        #endregion
    }
}
