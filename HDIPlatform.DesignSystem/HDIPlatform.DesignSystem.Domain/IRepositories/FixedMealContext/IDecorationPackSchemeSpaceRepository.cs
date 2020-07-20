using HDIPlatform.DesignSystem.Domain.Entities.FixedMealContext;
using ShSoft.Infrastructure.RepositoryBase;
using System;
using System.Collections.Generic;

namespace HDIPlatform.DesignSystem.Domain.IRepositories.FixedMealContext
{
    /// <summary>
    /// 套餐模板方案空间仓储接口
    /// </summary>
    public interface IDecorationPackSchemeSpaceRepository : IRepository<DecorationPackSchemeSpace>
    {
        #region # 根据套餐模板方案获取方案空间列表 —— IEnumerable<DecorationPackSchemeSpace> FindByScheme(...
        /// <summary>
        /// 根据套餐模板方案获取方案空间列表
        /// </summary>
        /// <param name="packSchemeId">套餐模板方案Id</param>
        /// <returns>方案空间列表</returns>
        IEnumerable<DecorationPackSchemeSpace> FindByScheme(Guid packSchemeId);
        #endregion


        #region # 根据套餐模板方案获取方案空间列表 —— IEnumerable<DecorationPackSchemeSpace> FindByScheme(...
        /// <summary>
        /// 根据套餐模板方案获取方案空间列表
        /// </summary>
        /// <param name="packSchemeIds">套餐模板方案Id集</param>
        /// <returns>方案空间列表</returns>
        IEnumerable<DecorationPackSchemeSpace> FindBySchemeIds(List<Guid> packSchemeIds);
        #endregion
    }
}
