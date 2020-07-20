using HDIPlatform.DesignSystem.Domain.Entities.FixedMealContext;
using ShSoft.Infrastructure.RepositoryBase;
using System;
using System.Collections.Generic;

namespace HDIPlatform.DesignSystem.Domain.IRepositories.FixedMealContext
{
    /// <summary>
    /// 套餐模板工艺项仓储接口
    /// </summary>
    public interface IDecorationPackCraftRepository : IRepository<DecorationPackCraft>
    {
        #region # 根据套餐模板项获取工艺项列表 —— IEnumerable<DecorationPackCraft> FindByPackItem(...
        /// <summary>
        /// 根据套餐模板项获取工艺项列表
        /// </summary>
        /// <param name="packItemId">套餐模板项Id</param>
        /// <returns>工艺项列表</returns>
        IEnumerable<DecorationPackCraft> FindByPackItem(Guid packItemId);
        #endregion

        #region # 获取总下架工艺实体Id列表 —— IEnumerable<Guid> GetTotalOffShelvedCrafts()
        /// <summary>
        /// 获取总下架工艺实体Id列表
        /// </summary>
        /// <returns>工艺实体Id列表</returns>
        IEnumerable<Guid> GetTotalOffShelvedCrafts();
        #endregion

        #region # 获取套餐模板Id列表 —— IEnumerable<Guid> GetPackIds(Guid craftEntityId)
        /// <summary>
        /// 获取套餐模板Id列表
        /// </summary>
        /// <param name="craftEntityId">工艺实体Id</param>
        /// <returns>套餐模板Id列表</returns>
        IEnumerable<Guid> GetPackIds(Guid craftEntityId);
        #endregion

        #region # 获取套餐工艺工程量字典 —— IDictionary<Guid, float> GetCraftQuantities(Guid packId)
        /// <summary>
        /// 获取套餐工艺工程量字典
        /// </summary>
        /// <param name="packId">套餐模板Id</param>
        /// <returns>工艺工程量字典</returns>
        IDictionary<Guid, float> GetCraftQuantities(Guid packId);
        #endregion

        #region # 根据工艺实体获取套餐模板项Id列表 —— IEnumerable<Guid> FindPackItemIds(Guid craftEntityId)
        /// <summary>
        /// 根据工艺实体获取套餐模板项Id列表
        /// </summary>
        /// <param name="craftEntityId">工艺实体Id</param>
        /// <returns></returns>
        IEnumerable<Guid> FindPackItemIds(Guid craftEntityId);
        #endregion

        #region # 获取套餐模板内下架工艺数量 —— IDictionary<Guid, int> GetOffShelvedCount(IEnumerable<Guid> packIds)
        /// <summary>
        /// 获取套餐模板内下架工艺数量
        /// </summary>
        /// <param name="packIds">套餐模板Id集</param>
        /// <returns>下架工艺数量字典</returns>
        /// <remarks>IDictionary[Guid, int]，[套餐模板Id, 下架工艺数量]</remarks>
        IDictionary<Guid, int> GetOffShelvedCount(IEnumerable<Guid> packIds);
        #endregion

        #region # 获取总下架工艺实体Id数量 —— int GetTotalOffShelvedCraftCount(IList<Guid> packIds)
        /// <summary>
        /// 获取总下架工艺实体Id数量
        /// </summary>
        /// <param name="packIds">套餐范围Id集（我的套餐范围内:集合|null：全部套餐）</param>
        /// <returns>工艺实体Id数量</returns>
        int GetTotalOffShelvedCraftCount(IList<Guid> packIds);
        #endregion

        #region # 给定套餐模板空间下是否存在下架工艺 —— IDictionary<Guid, bool> ExistsOffShelved(...
        /// <summary>
        /// 给定套餐模板空间下是否存在下架工艺
        /// </summary>
        /// <param name="packSpaceIds">套餐模板空间Id</param>
        /// <returns>是否存在</returns>
        IDictionary<Guid, bool> ExistsOffShelved(IEnumerable<Guid> packSpaceIds);
        #endregion

        #region # 获取套餐模板Id列表 —— IEnumerable<Guid> GetPackIdsByCrafts(IEnumerable<Guid> craftEntityIds)
        /// <summary>
        /// 获取套餐模板Id列表
        /// </summary>
        /// <param name="craftEntityIds">工艺实体Id集</param>
        /// <returns>套餐模板Id列表</returns>
        IEnumerable<Guid> GetPackIdsByCrafts(IEnumerable<Guid> craftEntityIds);
        #endregion

        #region # 套餐模板下是否配置套餐模板工艺 —— bool ExistsByPack(Guid packId)
        /// <summary>
        /// 套餐模板下是否配置套餐模板工艺
        /// </summary>
        /// <param name="packId">套餐模板Id</param>
        /// <returns>是否配置</returns>
        bool ExistsByPack(Guid packId);
        #endregion

        #region # 获取套餐选区Id列表 —— IEnumerable<Guid> GetPackItemIdsByCrafts(IEnumerable<Guid> craftEntityIds)

        /// <summary>
        /// 获取套餐选区Id列表
        /// </summary>
        /// <param name="craftEntityIds">工艺实体Id集</param>
        /// <returns>获取套餐选区Id列表</returns>
        IEnumerable<Guid> GetPackItemIdsByCrafts(IEnumerable<Guid> craftEntityIds);

        #endregion

        #region # 获取总下架工艺实体Id列表 —— IEnumerable<Guid> GetTotalOffShelvedCrafts()
        /// <summary>
        /// 获取总下架工艺实体Id列表
        /// </summary>
        /// <returns>工艺实体Id列表</returns>
        IEnumerable<Guid> GetTotalOffShelvedCraftsByPage(int pageIndex, int pageSize, out int rowCount, out int pageCount);
        #endregion

        #region # 获取总变价工艺实体Id列表 ——  IEnumerable<Guid> GetTotalChangedCraftsByPage(int pageIndex, int pageSize, out int rowCount, out int pageCount)
        /// <summary>
        /// 获取总变价工艺实体Id列表
        /// </summary>
        /// <returns>工艺实体Id列表</returns>
        IEnumerable<Guid> GetTotalChangedCraftsByPage(int pageIndex, int pageSize, out int rowCount, out int pageCount);
        #endregion

        #region # 获取总变价工艺实体Id列表 —— IEnumerable<Guid> GetTotalOffShelvedCrafts()
        /// <summary>
        /// 获取总变价工艺实体Id列表
        /// </summary>
        /// <returns>工艺实体Id列表</returns>
        IEnumerable<Guid> GetTotalChangedCrafts();
        #endregion

        #region # 获取套餐模板内变价工艺数量 —— IDictionary<Guid, int> GetOffShelvedCount(IEnumerable<Guid> packIds)
        /// <summary>
        /// 获取套餐模板内变价工艺数量
        /// </summary>
        /// <param name="packIds">套餐模板Id集</param>
        /// <returns>变价工艺数量字典</returns>
        /// <remarks>IDictionary[Guid, int]，[套餐模板Id, 变价工艺数量]</remarks>
        IDictionary<Guid, int> GetChangedCount(IEnumerable<Guid> packIds);
        #endregion

        #region # 获取总变价工艺实体Id数量 —— int GetTotalOffShelvedCraftCount(IList<Guid> packIds)
        /// <summary>
        /// 获取总变价工艺实体Id数量
        /// </summary>
        /// <param name="packIds">套餐范围Id集（我的套餐范围内:集合|null：全部套餐）</param>
        /// <returns>工艺实体Id数量</returns>
        int GetTotalChangedCraftCount(IList<Guid> packIds);
        #endregion

        #region # 给定套餐模板空间下是否存在变价工艺 —— IDictionary<Guid, bool> ExistsOffShelved(...
        /// <summary>
        /// 给定套餐模板空间下是否存在变价工艺
        /// </summary>
        /// <param name="packSpaceIds">套餐模板空间Id</param>
        /// <returns>是否存在</returns>
        IDictionary<Guid, bool> ExistsChanged(IEnumerable<Guid> packSpaceIds);
        #endregion

        #region # 获取套餐选区Id|CraftKey集 —— Dictionary<Guid, List<Guid>> GetPackItemIdCraftKeyIds(IEnumerable<Guid> craftEntityIds)

        /// <summary>
        /// 获取套餐选区Id|CraftKey集
        /// </summary>
        /// <param name="craftEntityIds">craftEntityId 集</param>
        /// <returns>套餐选区Id|CraftKey集</returns>
        Dictionary<Guid, List<Guid>> GetPackItemIdCraftKeyIds(IEnumerable<Guid> craftEntityIds);

        #endregion

        #region # 获取工艺项列表 —— IEnumerable<DecorationPackCraft> GetCrafts(IEnumerable<Guid> craftEntityIds)
        /// <summary>
        /// 获取工艺项列表
        /// </summary>
        /// <param name="craftEntityIds">craftEntityId 集</param>
        /// <returns>工艺项列表</returns>
        IEnumerable<DecorationPackCraft> GetCrafts(IEnumerable<Guid> craftEntityIds);
        #endregion

        #region # 获取总变价工艺实体Id|选区Id|套餐Id列表 ——  IEnumerable<Tuple<Guid, Guid, Guid>> GetTotalChangedCraftsByPage(int pageIndex, int pageSize, out int rowCount, out int pageCount)

        /// <summary>
        /// 获取总变价工艺实体Id|选区Id|套餐Id列表
        /// </summary>
        /// <returns>工艺实体Id|选区Id|套餐Id</returns>
        IEnumerable<Tuple<Guid, Guid, Guid>> GetTotalChangedByPage(int pageIndex, int pageSize, out int rowCount, out int pageCount);

        #endregion

        #region # 获取套餐模板Id列表(工艺实体Id|选区Id|套餐Id|工艺实体成本价) —— IEnumerable<Tuple<Guid, Guid, Guid,decimal>>   GetCraftsPackIdsById(IEnumerable<Guid> craftEntityIds)

        /// <summary>
        /// 获取套餐模板Id列表(工艺实体Id|选区Id|套餐Id|工艺实体成本价)
        /// </summary>
        /// <param name="craftEntityIds">工艺实体Id集</param>
        /// <returns>工艺实体Id|选区Id|套餐Id|工艺实体成本价</returns>
        IEnumerable<Tuple<Guid, Guid, Guid, decimal>> GetCraftsPackIdsById(IEnumerable<Guid> craftEntityIds);

        #endregion


        #region # 通过工艺Id集获取套餐选区Id ——IEnumerable<Guid> GetPackItemIdByCraftIds(IEnumerable<Guid> craftIds);

        /// <summary>
        /// 通过工艺Id集获取套餐选区Id
        /// </summary>
        /// <param name="craftIds">SkuId集</param>
        /// <returns>套餐选区Id集</returns>
        IEnumerable<Guid> GetPackItemIdByCraftIds(IEnumerable<Guid> craftIds);
        #endregion
    }
}
