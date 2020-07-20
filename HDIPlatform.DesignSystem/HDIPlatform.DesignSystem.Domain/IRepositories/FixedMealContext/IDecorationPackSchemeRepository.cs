using HDIPlatform.DesignSystem.Domain.Entities.FixedMealContext;
using ShSoft.Infrastructure.RepositoryBase;
using System;
using System.Collections.Generic;

namespace HDIPlatform.DesignSystem.Domain.IRepositories.FixedMealContext
{
    /// <summary>
    /// 套餐模板方案仓储接口
    /// </summary>
    public interface IDecorationPackSchemeRepository : IRepository<DecorationPackScheme>
    {
        #region # 根据套餐模板获取套餐模板方案列表 —— IEnumerable<DecorationPackScheme> FindByPack(Guid packId)
        /// <summary>
        /// 根据套餐模板获取套餐模板方案列表
        /// </summary>
        /// <param name="packId">套餐模板Id</param>
        /// <returns>套餐模板方案列表</returns>
        IEnumerable<DecorationPackScheme> FindByPack(Guid packId);
        #endregion

        #region # 是否存在套餐模板方案 —— bool ExistsScheme(Guid packId)
        /// <summary>
        /// 是否存在套餐模板方案
        /// </summary>
        /// <param name="packId">套餐模板Id</param>
        /// <returns>是否存在</returns>
        bool ExistsScheme(Guid packId);
        #endregion

        #region # 是否存在方案名称 —— bool ExistsName(Guid packId, Guid? packSchemeId...
        /// <summary>
        /// 是否存在方案名称
        /// </summary>
        /// <param name="packId">套餐模板Id</param>
        /// <param name="packSchemeId">套餐模板方案Id</param>
        /// <param name="schemeName">方案名称</param>
        /// <returns>是否存在</returns>
        bool ExistsName(Guid packId, Guid? packSchemeId, string schemeName);
        #endregion

        #region # 获取套餐模板的其他默认方案 —— DecorationPackScheme FirstOrDefault(Guid packId...
        /// <summary>
        /// 获取套餐模板的其他默认方案
        /// </summary>
        /// <param name="packId">套餐模板Id</param>
        /// <param name="packSchemeId">套餐模板方案Id</param>
        /// <returns>其他默认方案</returns>
        DecorationPackScheme FirstOrDefault(Guid packId, Guid packSchemeId);
        #endregion

        #region # 根据套餐模板获取套餐模板默认方案列表 —— IEnumerable<DecorationPackScheme> FindDefaultByPack(List<Guid> packIds)
        /// <summary>
        /// 根据套餐模板获取套餐模板默认方案列表
        /// </summary>
        /// <param name="packIds">套餐模板Id</param>
        /// <returns>套餐模板默认方案列表</returns>
        IEnumerable<DecorationPackScheme> FindDefaultByPack(List<Guid> packIds);
        #endregion


        #region # 根据套餐模板获取套餐模板默认方案 —— DecorationPackScheme GetDefaultByPack(Guid packId);
        /// <summary>
        /// 根据套餐模板获取套餐模板默认方案
        /// </summary>
        /// <param name="packId">套餐模板Id</param>
        /// <returns>套餐模板默认方案|null</returns>
        DecorationPackScheme GetDefaultByPack(Guid packId);
        #endregion
    }
}
