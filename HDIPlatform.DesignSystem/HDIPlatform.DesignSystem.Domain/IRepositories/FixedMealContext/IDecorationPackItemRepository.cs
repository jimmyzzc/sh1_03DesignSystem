using HDIPlatform.DesignSystem.Domain.Entities.FixedMealContext;
using ShSoft.Infrastructure.RepositoryBase;
using System;
using System.Collections.Generic;

namespace HDIPlatform.DesignSystem.Domain.IRepositories.FixedMealContext
{
    /// <summary>
    /// 套餐模板项仓储接口
    /// </summary>
    public interface IDecorationPackItemRepository : IRepository<DecorationPackItem>
    {
        #region # 根据套餐模板获取套餐模板项列表 —— IEnumerable<DecorationPackItem> FindByPack(Guid packId...
        /// <summary>
        /// 根据套餐模板获取套餐模板项列表
        /// </summary>
        /// <param name="packId">套餐模板Id</param>
        /// <param name="packSpaceId">套餐模板空间Id</param>
        /// <returns>套餐模板项列表</returns>
        IEnumerable<DecorationPackItem> FindByPack(Guid packId, Guid? packSpaceId);
        #endregion

        #region # 根据套餐模板获取套餐模板项Id列表 —— IEnumerable<Guid> FindIdsByPack(Guid packId, Guid? packSpaceId)
        /// <summary>
        /// 根据套餐模板获取套餐模板项Id列表
        /// </summary>
        /// <param name="packId">套餐模板Id</param>
        /// <param name="packSpaceId">套餐模板空间Id</param>
        /// <returns>套餐模板项Id列表</returns>
        IEnumerable<Guid> FindIdsByPack(Guid packId, Guid? packSpaceId);
        #endregion

        #region # 套餐模板项名称是否存在 （套餐）—— bool ExistsName(Guid packId, Guid? packItemId...
        /// <summary>
        /// 套餐模板项名称是否存在（套餐）
        /// </summary>
        /// <param name="packId">套餐模板Id</param>
        /// <param name="packItemId">套餐模板项Id</param>
        /// <param name="packItemName">套餐模板项名称</param>
        /// <returns>是否存在</returns>
        bool ExistsName(Guid packId, Guid? packItemId, string packItemName);
        #endregion


        #region # 套餐模板项名称是否存在（空间） —— bool ExistsName(Guid packId,Guid packSpaceId, Guid? packItemId...

        /// <summary>
        /// 套餐模板项名称是否存在（空间）
        /// </summary>
        /// <param name="packId">套餐模板Id</param>
        /// <param name="packSpaceId">空间Id</param>
        /// <param name="packItemId">套餐模板项Id</param>
        /// <param name="packItemName">套餐模板项名称</param>
        /// <returns>是否存在</returns>
        bool ExistsName(Guid packId, Guid packSpaceId, Guid? packItemId, string packItemName);
        #endregion

        #region # 验证固定套餐是否可以上架 —— bool IsDecorationPackShelfed(Guid packId, IEnumerable<Guid> packSpaceIds)
        /// <summary>
        /// 验证固定套餐是否可以上架
        /// </summary>
        /// <param name="packId"></param>
        /// <param name="packSpaceIds"></param>
        /// <returns></returns>
        bool IsDecorationPackShelfed(Guid packId, IEnumerable<Guid> packSpaceIds);
        #endregion


        #region # 根据套餐模板获取套餐模板项列表 —— Dictionary<Guid, IEnumerable<DecorationPackItem>> FindByPackIds(IEnumerable<Guid> packIds)

        /// <summary>
        /// 根据套餐模板获取套餐模板项列表
        /// </summary>
        /// <param name="packIds">套餐模板Id集</param>
        /// <returns>套餐Id|套餐模板项列表</returns>
        Dictionary<Guid, IEnumerable<DecorationPackItem>> FindByPackIds(IEnumerable<Guid> packIds);

        #endregion

        #region # 根据套餐选区Id集获取列表 ——IEnumerable<DecorationPackItem> FindByIds(IEnumerable<Guid> packItemIds)

        /// <summary>
        /// 根据套餐选区Id集获取列表
        /// </summary>
        /// <param name="packItemIds">套餐选区Id集</param>
        /// <returns>套餐选区列表</returns>
        IEnumerable<DecorationPackItem> FindByIds(IEnumerable<Guid> packItemIds);

        #endregion

    }
}
