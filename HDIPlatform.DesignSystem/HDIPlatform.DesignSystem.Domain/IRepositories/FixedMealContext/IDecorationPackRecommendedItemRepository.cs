using HDIPlatform.DesignSystem.Domain.Entities.FixedMealContext;
using ShSoft.Infrastructure.RepositoryBase;
using System;
using System.Collections.Generic;

namespace HDIPlatform.DesignSystem.Domain.IRepositories.FixedMealContext
{
    /// <summary>
    /// 套餐模板推荐项仓储接口
    /// </summary>
    public interface IDecorationPackRecommendedItemRepository : IRepository<DecorationPackRecommendedItem>
    {
        #region # 根据套餐模板获取套餐模板推荐项列表 ——   IEnumerable<DecorationPackRecommendedItem> FindByPack(Guid packId, Guid? packSpaceId);
        /// <summary>
        /// 根据套餐模板获取套餐模板推荐项列表
        /// </summary>
        /// <param name="packId">套餐模板Id</param>
        /// <param name="packSpaceId">套餐模板空间Id</param>
        /// <returns>套餐模板项推荐列表</returns>
        IEnumerable<DecorationPackRecommendedItem> FindByPack(Guid packId, Guid? packSpaceId);
        #endregion

    }
}
