using HDIPlatform.DesignSystem.Domain.Entities.FixedMealContext;
using HDIPlatform.DesignSystem.Domain.IRepositories.FixedMealContext;
using ShSoft.Infrastructure.Repository.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using ShSoft.Common.PoweredByLee;

namespace HDIPlatform.DesignSystem.Repository.Implements.FixedMealContext
{
    /// <summary>
    /// 套餐模板推荐项仓储实现
    /// </summary>
    public class DecorationPackRecommendedItemRepository : EFRepositoryProvider<DecorationPackRecommendedItem>, IDecorationPackRecommendedItemRepository
    {
        #region # 获取实体对象集合 —— override IQueryable<DecorationPackRecommendedItem> FindAllInner()
        /// <summary>
        /// 获取实体对象集合
        /// </summary>
        /// <returns>实体对象集合</returns>
        protected override IQueryable<DecorationPackRecommendedItem> FindAllInner()
        {
            return base.FindAllInner().OrderBy(x => x.AddedTime);
        }
        #endregion

        #region # 根据套餐模板获取套餐模板推荐项列表 —— IEnumerable<DecorationPackItem> FindByPack(Guid packId)
        /// <summary>
        /// 根据套餐模板获取套餐模板推荐项列表
        /// </summary>
        /// <param name="packId">套餐模板Id</param>
        /// <param name="packSpaceId">套餐模板空间Id</param>
        /// <returns>套餐模板推荐项列表</returns>
        public IEnumerable<DecorationPackRecommendedItem> FindByPack(Guid packId, Guid? packSpaceId)
        {
            Expression<Func<DecorationPackRecommendedItem, bool>> condition =
                x =>
                    x.PackId == packId &&
                    (packSpaceId == null || x.PackSpaceId == packSpaceId);

            return base.Find(condition).AsEnumerable();
        }
        #endregion
    }
}
