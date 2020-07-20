using HDIPlatform.DesignSystem.Domain.Entities.FixedMealContext;
using HDIPlatform.DesignSystem.Domain.IRepositories.FixedMealContext;
using ShSoft.Infrastructure.Repository.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HDIPlatform.DesignSystem.Repository.Implements.FixedMealContext
{
    /// <summary>
    /// 套餐模板方案空间仓储实现
    /// </summary>
    public class DecorationPackSchemeSpaceRepository : EFRepositoryProvider<DecorationPackSchemeSpace>, IDecorationPackSchemeSpaceRepository
    {
        #region # 获取实体对象集合 —— override IQueryable<DecorationPackSchemeSpace> FindAllInner()
        /// <summary>
        /// 获取实体对象集合
        /// </summary>
        /// <returns>实体对象集合</returns>
        protected override IQueryable<DecorationPackSchemeSpace> FindAllInner()
        {
            return base.FindAllInner().Where(x => x.PackScheme != null && !x.PackScheme.Deleted);
        }
        #endregion

        #region # 根据套餐模板方案获取方案空间列表 —— IEnumerable<DecorationPackSchemeSpace> FindByScheme(...
        /// <summary>
        /// 根据套餐模板方案获取方案空间列表
        /// </summary>
        /// <param name="packSchemeId">套餐模板方案Id</param>
        /// <returns>方案空间列表</returns>
        public IEnumerable<DecorationPackSchemeSpace> FindByScheme(Guid packSchemeId)
        {
            return this.Find(x => x.PackScheme.Id == packSchemeId).AsEnumerable();
        }
        #endregion


        #region # 根据套餐模板方案获取方案空间列表 —— IEnumerable<DecorationPackSchemeSpace> FindByScheme(...

        /// <summary>
        /// 根据套餐模板方案获取方案空间列表
        /// </summary>
        /// <param name="packSchemeIds">套餐模板方案Id集</param>
        /// <returns>方案空间列表</returns>
        public IEnumerable<DecorationPackSchemeSpace> FindBySchemeIds(List<Guid> packSchemeIds)
        {
            return this.Find(x => packSchemeIds.Contains(x.PackScheme.Id)).AsEnumerable();

        }

        #endregion
    }
}
