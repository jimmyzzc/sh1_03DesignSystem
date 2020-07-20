using HDIPlatform.DesignSystem.Domain.Entities.FixedMealContext;
using ShSoft.Infrastructure.RepositoryBase;
using System;
using System.Collections.Generic;

namespace HDIPlatform.DesignSystem.Domain.IRepositories.FixedMealContext
{
    /// <summary>
    /// 套餐模板商品SKU项仓储接口
    /// </summary>
    public interface IDecorationPackSkuRepository : IRepository<DecorationPackSku>
    {
        #region # 根据套餐模板项获取商品SKU项列表 —— IEnumerable<DecorationPackSku> FindByPackItem(...
        /// <summary>
        /// 根据套餐模板项获取商品SKU项列表
        /// </summary>
        /// <param name="packItemId">套餐模板项Id</param>
        /// <returns>商品SKU项列表</returns>
        IEnumerable<DecorationPackSku> FindByPackItem(Guid packItemId);
        #endregion

        #region # 套餐模板下是否配置套餐模板商品SKU项 —— bool ExistsByPack(Guid packId)
        /// <summary>
        /// 套餐模板下是否配置套餐模板商品SKU项
        /// </summary>
        /// <param name="packId">套餐模板Id</param>
        /// <returns>是否配置</returns>
        bool ExistsByPack(Guid packId);
        #endregion

        #region # 获取套餐模板内下架商品数量 —— IDictionary<Guid, int> GetOffShelvedCount(IEnumerable<Guid> packIds)
        /// <summary>
        /// 获取套餐模板内下架商品数量
        /// </summary>
        /// <param name="packIds">套餐模板Id集</param>
        /// <returns>下架商品数量字典</returns>
        /// <remarks>IDictionary[Guid, int]，[套餐模板Id, 下架商品数量]</remarks>
        IDictionary<Guid, int> GetOffShelvedCount(IEnumerable<Guid> packIds);
        #endregion

        #region # 获取套餐模板内变价商品数量 —— IDictionary<Guid, int> GetChangedCount(IEnumerable<Guid> packIds)
        /// <summary>
        /// 获取套餐模板内变价商品数量
        /// </summary>
        /// <param name="packIds">套餐模板Id集</param>
        /// <returns>变价商品数量字典</returns>
        /// <remarks>IDictionary[Guid, int]，[套餐模板Id, 变价商品数量]</remarks>
        IDictionary<Guid, int> GetChangedCount(IEnumerable<Guid> packIds);
        #endregion

        #region # 获取总下架商品数量 —— int GetTotalOffShelvedCount(IList<Guid> packIds)
        /// <summary>
        /// 获取总下架商品数量
        /// </summary>
        /// <param name="packIds">套餐范围Id集（我的套餐范围内:集合|null：全部套餐）</param>
        /// <returns>总下架商品数量</returns>
        int GetTotalOffShelvedCount(IList<Guid> packIds);
        #endregion

        #region # 获取总变价商品数量 —— int GetTotalChangedCount(IList<Guid> packIds)
        /// <summary>
        /// 获取总变价商品数量
        /// </summary>
        /// <param name="packIds">套餐范围Id集（我的套餐范围内:集合|null：全部套餐）</param>
        /// <returns>总变价商品数量</returns>
        int GetTotalChangedCount(IList<Guid> packIds);
        #endregion


        #region # 获取总下架商品SKU Id列表 —— IEnumerable<Guid> GetTotalOffShelvedSkus()
        /// <summary>
        /// 获取总下架商品SKU Id集列表
        /// </summary>
        /// <returns>商品SKU Id列表</returns>
        IEnumerable<Guid> GetTotalOffShelvedSkus();
        #endregion

        #region # 获取总变价商品SKU Id列表 —— IEnumerable<Guid> GetTotalChangedSkus()
        /// <summary>
        /// 获取总变价商品SKU Id集列表
        /// </summary>
        /// <returns>商品SKU Id列表</returns>
        IEnumerable<Guid> GetTotalChangedSkus();
        #endregion

        #region # 获取套餐模板Id列表 —— IEnumerable<Guid> GetPackIds(Guid skuId)
        /// <summary>
        /// 获取套餐模板Id列表
        /// </summary>
        /// <param name="skuId">商品SKU Id</param>
        /// <returns>套餐模板Id列表</returns>
        IEnumerable<Guid> GetPackIds(Guid skuId);
        #endregion

        #region # 给定套餐模板空间下是否存在下架商品 —— IDictionary<Guid, bool> ExistsOffShelved(...
        /// <summary>
        /// 给定套餐模板空间下是否存在下架商品
        /// </summary>
        /// <param name="packSpaceIds">套餐模板空间Id</param>
        /// <returns>是否存在</returns>
        IDictionary<Guid, bool> ExistsOffShelved(IEnumerable<Guid> packSpaceIds);
        #endregion

        #region # 给定套餐模板空间下是否存在变价商品 —— IDictionary<Guid, bool> ExistsChanged(...
        /// <summary>
        /// 给定套餐模板空间下是否存在变价商品
        /// </summary>
        /// <param name="packSpaceIds">套餐模板空间Id</param>
        /// <returns>是否存在</returns>
        IDictionary<Guid, bool> ExistsChanged(IEnumerable<Guid> packSpaceIds);
        #endregion

        #region # 获取套餐商品SKU数量字典 —— IDictionary<Guid, float> GetSkuQuantities(Guid packId)
        /// <summary>
        /// 获取套餐商品SKU数量字典
        /// </summary>
        /// <param name="packId">套餐模板Id</param>
        /// <returns>商品SKU数量字典</returns>
        IDictionary<Guid, float> GetSkuQuantities(Guid packId);
        #endregion

        #region # 根据商品SKU获取套餐模板项Id列表 —— IEnumerable<Guid> FindPackItemIds(Guid skuId)
        /// <summary>
        /// 根据商品SKU获取套餐模板项Id列表
        /// </summary>
        /// <param name="skuId">商品SKU Id</param>
        /// <returns></returns>
        IEnumerable<Guid> FindPackItemIds(Guid skuId);
        #endregion

        #region # 根据套餐获取商品SKU项列表 —— IDictionary<Guid,IEnumerable<DecorationPackSku>>  FindByPackId(...
        /// <summary>
        /// 根据套餐获取商品SKU项列表
        /// </summary>
        /// <param name="packId">套餐Id</param>
        /// <returns>选区|商品SKU项列表</returns>
        IDictionary<Guid, IEnumerable<DecorationPackSku>> FindByPackId(Guid packId);
        #endregion

        #region # 获取套餐选区Id列表 —— IEnumerable<Guid> GetPackItemIdsBySkus(IEnumerable<Guid> skuIds)

        /// <summary>
        /// 获取套餐选区Id列表
        /// </summary>
        /// <param name="skuIds">SkuId集</param>
        /// <returns>套餐选区Id列表</returns>
        IEnumerable<Guid> GetPackItemIdsBySkus(IEnumerable<Guid> skuIds);

        #endregion

        #region # 获取套餐模板Id列表 —— IEnumerable<Guid> GetPackIdsBySkus(IEnumerable<Guid> skuIds)
        /// <summary>
        /// 获取套餐模板Id列表
        /// </summary>
        /// <param name="skuIds">SkuId集</param>
        /// <returns>套餐模板Id列表</returns>
        IEnumerable<Guid> GetPackIdsBySkus(IEnumerable<Guid> skuIds);
        #endregion

        #region # 获取总下架商品SKU Id列表 —— IEnumerable<Guid> GetTotalOffShelvedSkusByPage(int pageIndex, int pageSize, out int rowCount, out int pageCount);
        /// <summary>
        /// 获取总下架商品SKU Id集列表
        /// </summary>
        /// <returns>商品SKU Id列表</returns>
        IEnumerable<Guid> GetTotalOffShelvedSkusByPage(int pageIndex, int pageSize, out int rowCount, out int pageCount);
        #endregion


        #region # 获取总变价商品SKU Id列表 —— IEnumerable<Guid> GetTotalChangedSkuIdsByPage(int pageIndex, int pageSize, out int rowCount, out int pageCount);
        /// <summary>
        /// 获取总变价商品SKU Id集列表
        /// </summary>
        /// <returns>商品SKU Id列表</returns>
        IEnumerable<Guid> GetTotalChangedSkuIdsByPage(int pageIndex, int pageSize, out int rowCount, out int pageCount);
        #endregion


        #region # 通过SkuId集获取套餐选区Id ——IEnumerable<Guid> GetPackItemIdBySkuIds(IEnumerable<Guid> skuIds)

        /// <summary>
        /// 通过SkuId集获取套餐选区Id
        /// </summary>
        /// <param name="skuIds">SkuId集</param>
        /// <returns>套餐选区Id集</returns>
        IEnumerable<Guid> GetPackItemIdBySkuIds(IEnumerable<Guid> skuIds);
        #endregion


        #region # 获取总变价商品SKU 列表 ——  IEnumerable<DecorationPackSku> GetSkus(IEnumerable<Guid> skuIds)
        /// <summary>
        /// 获取总变价商品SKU Id集列表
        /// </summary>
        /// <returns>商品SKU 列表</returns>
        IEnumerable<DecorationPackSku> GetSkus(IEnumerable<Guid> skuIds);
        #endregion


        #region # 获取总变价商品SKUId|选区Id|套餐Id列表 —— IEnumerable<Tuple<Guid, Guid, Guid>> GetTotalChangedSkuIdsByPage(int pageIndex, int pageSize, out int rowCount, out int pageCount);

        /// <summary>
        /// 获取总变价商品SKUId|选区Id|套餐Id列表
        /// </summary>
        /// <returns>商品SKUId|选区Id|套餐Id</returns>
        IEnumerable<Tuple<Guid, Guid, Guid>> GetTotalChangedByPage(int pageIndex, int pageSize, out int rowCount, out int pageCount);

        #endregion

        #region # 获取套餐模板Id列表(商品实体Id|选区Id|套餐Id|商品实体成本价) —— IEnumerable<Tuple<Guid, Guid, Guid,decimal>>   GetSkusPackIdsById(IEnumerable<Guid> skuIds)

        /// <summary>
        /// 获取套餐模板Id列表(商品实体Id|选区Id|套餐Id|商品实体成本价)
        /// </summary>
        /// <param name="skuIds">商品实体Id集</param>
        /// <returns>商品实体Id|选区Id|套餐Id|商品实体成本价</returns>
        IEnumerable<Tuple<Guid, Guid, Guid, decimal>> GetSkusPackIdsById(IEnumerable<Guid> skuIds);

        #endregion
    }
}
