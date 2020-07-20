using HDIPlatform.DesignSystem.Domain.Entities.FixedMealContext;
using ShSoft.Infrastructure.RepositoryBase;
using System;
using System.Collections.Generic;

namespace HDIPlatform.DesignSystem.Domain.IRepositories.FixedMealContext
{
    /// <summary>
    /// 套餐模板 - 大包/定制模板仓储接口
    /// </summary>
    public interface IDecorationPack_BalePackRepository : IRepository<DecorationPack_BalePack>
    {
        #region # 获取唯一关联关系 —— DecorationPack_BalePack Single(Guid decorationPackId, Guid balePackId)
        /// <summary>
        /// 获取唯一关联关系
        /// </summary>
        /// <param name="decorationPackId">套餐模板Id</param>
        /// <param name="balePackId">大包/定制模板Id</param>
        /// <returns>套餐模板 - 大包/定制模板关联关系</returns>
        DecorationPack_BalePack Single(Guid decorationPackId, Guid balePackId);
        #endregion

        #region # 是否存在关联关系 —— bool Exists(Guid decorationPackId, Guid balePackId)
        /// <summary>
        /// 是否存在关联关系
        /// </summary>
        /// <param name="decorationPackId">套餐模板Id</param>
        /// <param name="balePackId">大包/定制模板Id</param>
        /// <returns>是否存在</returns>
        bool Exists(Guid decorationPackId, Guid balePackId);
        #endregion

        #region # 根据套餐模板获取关联关系列表 —— IEnumerable<DecorationPack_BalePack> FindByDecorationPack(...
        /// <summary>
        /// 根据套餐模板获取关联关系列表
        /// </summary>
        /// <param name="decorationPackId">套餐模板Id</param>
        /// <returns>关联关系列表</returns>
        IEnumerable<DecorationPack_BalePack> FindByDecorationPack(Guid decorationPackId);
        #endregion

        #region # 根据套餐模板获取大包/定制模板Id列表 —— IEnumerable<Guid> FindBalePackIdsByDecorationPack(...
        /// <summary>
        /// 根据套餐模板获取大包/定制模板Id列表
        /// </summary>
        /// <param name="decorationPackId">套餐模板Id</param>
        /// <returns>大包/定制模板Id列表</returns>
        IEnumerable<Guid> FindBalePackIdsByDecorationPack(Guid decorationPackId);
        #endregion
    }
}
