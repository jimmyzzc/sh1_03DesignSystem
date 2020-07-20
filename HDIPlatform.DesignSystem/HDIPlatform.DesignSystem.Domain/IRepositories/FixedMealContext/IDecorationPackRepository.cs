using System;
using HDIPlatform.DesignSystem.Domain.Entities.FixedMealContext;
using ShSoft.Infrastructure.RepositoryBase;
using ShSoft.ValueObjects.Enums;
using System.Collections.Generic;

namespace HDIPlatform.DesignSystem.Domain.IRepositories.FixedMealContext
{
    /// <summary>
    /// 套餐模板仓储接口
    /// </summary>
    public interface IDecorationPackRepository : IRepository<DecorationPack>
    {
        #region # 分页获取套餐模板列表 —— IEnumerable<DecorationPack> FindByPage(string keywords, decimal? minPrice...

        /// <summary>
        /// 分页获取套餐模板列表
        /// </summary>
        /// <param name="keywords">关键字</param>
        /// <param name="propertyId">楼盘Id</param>
        /// <param name="applicableSquare">适用面积</param>
        /// <param name="isNewHouse">是否适用新房</param>
        /// <param name="isBuildingPrice">是否是建筑价格搜索</param>
        /// <param name="minPrice">最小价格</param>
        /// <param name="maxPrice">最大价格</param>
        /// <param name="isBuildingSquare">是否是建筑面积搜索</param>
        /// <param name="minSquare">最小面积</param>
        /// <param name="maxSquare">最大面积</param>
        /// <param name="hasOffSku">是否有下架商品</param>
        /// <param name="hasChangedSku">是否有变价商品|工艺</param>
        /// <param name="color">颜色</param>
        /// <param name="packType">套餐类型</param>
        /// <param name="packKind">套餐类别</param>
        /// <param name="packMode">套餐模式</param>
        /// <param name="status">状态</param>
        /// <param name="spaceTypes">空间类型集</param>
        /// <param name="styleNos">设计风格集</param>
        /// <param name="layouts">居室集</param>
        /// <param name="propertys">楼盘集</param>
        /// <param name="hasPackSeries">是否有标签(全部,标签:true,其他:false)</param>
        /// <param name="packSeriesIds">套餐系列集</param>
        /// <param name="stores">门店集</param>
        /// <param name="hasStores">是否有门店（全部,标签:true,其他:false）</param>
        /// <param name="createrId">套餐创建人Id？（全部：null,我的：当前登录人员）</param>
        /// <param name="sort">排序条件 true为倒序</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页容量</param>
        /// <param name="rowCount">总记录条数</param>
        /// <param name="pageCount">总页数</param>
        /// <returns>套餐模板列表</returns>
        IEnumerable<DecorationPack> FindByPage(string keywords, Guid? propertyId, float? applicableSquare, bool? isNewHouse, bool? isBuildingPrice, decimal? minPrice,
            decimal? maxPrice, bool? isBuildingSquare, float? minSquare, float? maxSquare, bool? hasOffSku, bool? hasChangedSku, DecorationPackColor? color,
            DecorationPackType? packType, DecorationPackKind? packKind, DecorationPackMode? packMode, ShelfStatus? status, IList<string> spaceTypes,
            IList<string> styleNos, IList<string> layouts, IList<Guid> propertys, bool hasPackSeries, IList<Guid> packSeriesIds, IList<Guid> stores, bool hasStores, Guid? createrId, Dictionary<string, bool> sort, int pageIndex, int pageSize,
            out int rowCount, out int pageCount);
        #endregion

        #region # 分页获取套餐模板列表 —— IEnumerable<DecorationPack> FindByCondition(Guid propertyId, Guid? houseTypeId, decimal? minPrice, decimal? maxPrice, float? minSquare, float? maxSquare,

        /// <summary>
        /// 分页获取套餐模板列表(排除克隆版本套餐)
        /// </summary>
        /// <param name="propertyId">楼盘Id</param>
        /// <param name="houseTypeId">户型Id</param>
        /// <param name="minPrice">最小价格</param>
        /// <param name="maxPrice">最大价格</param>
        /// <param name="minSquare">最小面积</param>
        /// <param name="maxSquare">最大面积</param>
        /// <param name="sort">排序条件 true为倒序</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页容量</param>
        /// <param name="rowCount">总记录条数</param>
        /// <param name="pageCount">总页数</param>
        /// <returns>套餐模板列表</returns>
        IEnumerable<DecorationPack> FindByCondition(Guid propertyId, Guid? houseTypeId, decimal? minPrice, decimal? maxPrice, float? minSquare, float? maxSquare,
            Dictionary<string, bool> sort, int pageIndex, int pageSize, out int rowCount, out int pageCount);
        #endregion

        #region # 分页获取套餐模板列表 —— IEnumerable<DecorationPack> FindByCondition(IList<Guid> propertys, Guid? houseTypeId, bool hasPackSeries, IEnumerable<Guid> packSeriesIds, IEnumerable<string> styleNos,Dictionary<string, bool> sort, int pageIndex, int pageSize, out int rowCount, out int pageCount)

        /// <summary>
        /// 分页获取套餐模板列表(排除克隆版本套餐)
        /// </summary>
        /// <param name="keywords">关键字</param>
        /// <param name="containsNoProperty">是否包含无楼盘标签套餐（true查询）</param>
        /// <param name="propertys">楼盘集</param>
        /// <param name="propertyId">楼盘Id</param>
        /// <param name="houseTypeId">户型Id</param>
        /// <param name="hasPackSeries">是否有标签(全部,标签:true,其他:false)</param>
        /// <param name="packSeriesIds">套餐系列集</param>
        /// <param name="styleNos">设计风格集</param>
        /// <param name="isBuildSuqare">是否是建筑面积搜索</param>
        /// <param name="minSquare">最小面积</param>
        /// <param name="maxSquare">最大面积</param>
        /// <param name="sort">排序条件 true为倒序</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页容量</param>
        /// <param name="rowCount">总记录条数</param>
        /// <param name="pageCount">总页数</param>
        /// <returns>套餐模板列表</returns>
        IEnumerable<DecorationPack> FindByCondition(string keywords, bool containsNoProperty, IList<Guid> propertys, Guid? propertyId, Guid? houseTypeId, bool hasPackSeries, IEnumerable<Guid> packSeriesIds, IEnumerable<string> styleNos, bool? isBuildSuqare, float? minSquare, float? maxSquare, Dictionary<string, bool> sort, int pageIndex, int pageSize, out int rowCount, out int pageCount);
        #endregion


        #region # 获取套餐模板最小排序 —— float GetMinSort()
        /// <summary>
        /// 获取套餐模板最小排序
        /// </summary>
        /// <returns>最小排序</returns>
        float GetMinSort();
        #endregion

        #region # 获取套餐模板最大排序 —— float GetMaxSort()
        /// <summary>
        /// 获取套餐模板最大排序
        /// </summary>
        /// <returns>最大排序</returns>
        float GetMaxSort();
        #endregion

        #region # 根据源套餐Id获取套餐当前版本及Id ——  DecorationPack FindDecorationPackVersionNumber(Guid sourcePackId)
        /// <summary>
        /// 根据源套餐Id获取套餐当前版本及Id
        /// </summary>
        /// <param name="sourcePackId">源套餐Id</param>
        /// <returns></returns>
        DecorationPack FindDecorationPackVersionNumber(Guid sourcePackId);
        #endregion

        #region # 根据源套餐Id集获取套餐最新版本集 ——         IEnumerable<DecorationPack> FindDecorationPackVersionNumbers(IEnumerable<Guid> sourcePackIds);
        /// <summary>
        /// 根据源套餐Id集获取套餐最新版本集
        /// </summary>
        /// <param name="sourcePackIds">源套餐Id集</param>
        /// <returns></returns>
        IEnumerable<DecorationPack> FindDecorationPackVersionNumbers(IEnumerable<Guid> sourcePackIds);
        #endregion

        #region # 验证套餐名称是否重复 —— bool ExistsPackName(Guid? packId, string packName)
        /// <summary>
        /// 验证套餐名称是否重复(排除克隆套餐)
        /// </summary>
        /// <param name="packId">套餐Id</param>
        /// <param name="packName">套餐名称</param>
        /// <returns></returns>
        bool ExistsPackName(Guid? packId, string packName);
        #endregion

        #region # 获取所有源套餐标签(居室|设计风格|空间类型) —— Tuple<List<string>, List<string>, List<string>> FindSourceDecorationPackLabels()
        /// <summary>
        /// 获取所有源套餐标签(居室|设计风格|空间类型)
        /// </summary>
        /// <returns></returns>
        Tuple<List<string>, List<string>, List<string>> FindSourceDecorationPackLabels();
        #endregion


        #region # 通过套餐Id集获取套餐集 ——IEnumerable<DecorationPack> GetDecorationPacksByPackIds(IEnumerable<Guid> packIds);
        /// <summary>
        /// 通过套餐Id集获取套餐集
        /// </summary>
        /// <param name="packIds">套餐Id集</param>
        /// <param name="createrId">套餐创建人Id？（全部：null,我的：当前登录人员）</param>
        /// <returns></returns>
        IEnumerable<DecorationPack> GetDecorationPacksByPackIds(IEnumerable<Guid> packIds, Guid? createrId = null);
        #endregion


        #region # 获取套餐风格集 ——IEnumerable<string> GetDecorationPacksStyles();
        /// <summary>
        /// 获取套餐风格集
        /// </summary>
        /// <returns></returns>
        IEnumerable<string> GetDecorationPacksStyles();
        #endregion

        #region # 获取套餐居室集 —— IEnumerable<string> GetDecorationPacksLayouts();
        /// <summary>
        /// 获取套餐居室集
        /// </summary>
        /// <returns></returns>
        IEnumerable<string> GetDecorationPacksLayouts();
        #endregion

        #region # 获取套餐系列集 —— IEnumerable<Tuple<Guid, string,string>>  GetDecorationPackSeries();
        /// <summary>
        /// 获取套餐系列集
        /// </summary>
        /// <returns></returns>
        IEnumerable<Tuple<Guid, string, string>> GetDecorationPackSeries();
        #endregion

        #region # 获取套餐户型集 —— IEnumerable<Tuple<Guid, string, Guid, string, string>> GetDecorationPackHouseTypes()

        /// <summary>
        /// 获取套餐户型集
        /// </summary>
        /// <returns></returns>
        IEnumerable<Tuple<Guid, string, Guid, string, string>> GetDecorationPackHouseTypes();

        #endregion

        #region # 获取套餐楼盘集 —— Dictionary<Guid, string> GetDecorationPacksPropertys()

        /// <summary>
        /// 获取套餐楼盘集
        /// </summary>
        /// <returns></returns>
        Dictionary<Guid, string> GetDecorationPacksPropertys();

        #endregion


        #region # 获取套餐系列集 ——    IEnumerable<Tuple<string, int, IEnumerable<Tuple<Guid, string, string, int, string>>>> GetDecorationPackGroupSeries()
        /// <summary>
        /// 获取套餐系列集(分组名|排序|系列集（系列Id，名称，描述，排序，分组名）)
        /// </summary>
        /// <returns></returns>
        IEnumerable<Tuple<string, int, IEnumerable<Tuple<Guid, string, string, int, string>>>> GetDecorationPackGroupSeries();

        #endregion

        #region 山河2020年新需求(2020-05-08)

        #region # 根据项目Id分页获取套餐模板列表 —— IEnumerable<DecorationPack> FindByPage(string keywords, decimal? minPrice...
        /// <summary>
        /// 根据项目Id分页获取套餐模板列表
        /// </summary>
        /// <param name="projectId">项目Id</param>
        /// <param name="keywords">关键字</param>
        /// <param name="propertyId">楼盘Id</param>
        /// <param name="applicableSquare">适用面积</param>
        /// <param name="isNewHouse">是否适用新房</param>
        /// <param name="isBuildingPrice">是否是建筑价格搜索</param>
        /// <param name="minPrice">最小价格</param>
        /// <param name="maxPrice">最大价格</param>
        /// <param name="isBuildingSquare">是否是建筑面积搜索</param>
        /// <param name="minSquare">最小面积</param>
        /// <param name="maxSquare">最大面积</param>
        /// <param name="hasOffSku">是否有下架商品</param>
        /// <param name="hasChangedSku">是否有变价商品|工艺</param>
        /// <param name="color">颜色</param>
        /// <param name="packType">套餐类型</param>
        /// <param name="packKind">套餐类别</param>
        /// <param name="packMode">套餐模式</param>
        /// <param name="status">状态</param>
        /// <param name="spaceTypes">空间类型集</param>
        /// <param name="styleNos">设计风格集</param>
        /// <param name="layouts">居室集</param>
        /// <param name="propertys">楼盘集</param>
        /// <param name="hasPackSeries"></param>
        /// <param name="packSeriesIds">套餐系列集</param>
        /// <param name="stores">门店集</param>
        /// <param name="hasStores">是否有门店（全部,标签:true,其他:false）</param>
        /// <param name="createrId">套餐创建人Id？（全部：null,我的：当前登录人员）</param>
        /// <param name="sort">排序条件 false为倒序</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页容量</param>
        /// <param name="rowCount">总记录条数</param>
        /// <param name="pageCount">总页数</param>
        /// <returns>套餐模板列表</returns>
        IEnumerable<DecorationPack> FindByPage(Guid projectId, string keywords, Guid? propertyId, float? applicableSquare, bool? isNewHouse, bool? isBuildingPrice, decimal? minPrice, decimal? maxPrice, bool? isBuildingSquare,
            float? minSquare, float? maxSquare, bool? hasOffSku, bool? hasChangedSku, DecorationPackColor? color, DecorationPackType? packType,
            DecorationPackKind? packKind, DecorationPackMode? packMode, ShelfStatus? status, IList<string> spaceTypes, IList<string> styleNos, IList<string> layouts,
            IList<Guid> propertys, bool hasPackSeries, IList<Guid> packSeriesIds, IList<Guid> stores, bool hasStores, Guid? createrId, Dictionary<string, bool> sort, int pageIndex, int pageSize, out int rowCount, out int pageCount);
        #endregion

        #region # 根据项目Id获取已生效套餐列表——IEnumerable<DecorationPackInfo> GetDecorationPackByProjectId(Guid projectId)
        /// <summary>
        /// 根据项目Id获取已生效套餐列表
        /// </summary>
        /// <param name="projectId">项目Id</param>
        /// <returns>已生效套餐列表</returns>
        IEnumerable<DecorationPack> GetDecorationPackByProjectId(Guid projectId);
        #endregion

        #endregion
    }
}
