using HDIPlatform.DesignSystem.Domain.Entities.FixedMealContext;
using ShSoft.Infrastructure.RepositoryBase;
using System;
using System.Collections.Generic;

namespace HDIPlatform.DesignSystem.Domain.IRepositories.FixedMealContext
{
    /// <summary>
    /// 套餐模板空间仓储接口
    /// </summary>
    public interface IDecorationPackSpaceRepository : IRepository<DecorationPackSpace>
    {
        #region # 根据套餐模板获取套餐空间列表 —— IEnumerable<DecorationPackSpace> FindByPack(...
        /// <summary>
        /// 根据套餐模板获取套餐空间列表
        /// </summary>
        /// <param name="packId">套餐模板Id</param>
        /// <returns>套餐空间列表</returns>
        IEnumerable<DecorationPackSpace> FindByPack(Guid packId);
        #endregion

        #region # 套餐模板空间名称是否存在 —— bool ExistsName(Guid packId, Guid? packSpaceId...
        /// <summary>
        /// 套餐模板空间名称是否存在
        /// </summary>
        /// <param name="packId">套餐模板Id</param>
        /// <param name="packSpaceId">套餐模板空间Id</param>
        /// <param name="packSpaceName">套餐模板空间名称</param>
        /// <returns>是否存在</returns>
        bool ExistsName(Guid packId, Guid? packSpaceId, string packSpaceName);
        #endregion
    }
}
