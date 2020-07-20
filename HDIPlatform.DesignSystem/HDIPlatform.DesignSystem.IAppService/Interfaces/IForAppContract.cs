using HDIPlatform.DesignSystem.IAppService.DTOs.Outputs.BaleContext;
using ShSoft.Infrastructure;
using ShSoft.Infrastructure.DTOBase;
using ShSoft.ValueObjects.Enums;
using System;
using System.Collections.Generic;
using System.ServiceModel;
using HDIPlatform.DesignSystem.IAppService.DTOs.Outputs.ConfigContext;
using HDIPlatform.DesignSystem.IAppService.DTOs.Outputs.ForAppContext;

namespace HDIPlatform.DesignSystem.IAppService.Interfaces
{
    /// <summary>
    /// APP服务契约接口
    /// </summary>
    [ServiceContract(Namespace = "http://HDIPlatform.DesignSystem.IAppService.Interfaces")]
    public interface IForAppContract : IApplicationService
    {


        #region 获取已使用的套餐标签 --     IEnumerable<App_LableInfo> GetDecorationPackLables();

        /// <summary>
        /// 获取已使用的套餐标签
        /// </summary>
        [OperationContract]
        App_LableInfo GetDecorationPackLables();

        #endregion


        #region 根据条件分页获取套餐 --  PageModel<App_DecorationPackInfo> GetDecorationPacks(string keywords, IEnumerable<Guid> packSeriesIds, IEnumerable<string> layouts, IEnumerable<string> styleNos,float? minSquare,float? maxSquare,int pageIndex,int PageSize);

        /// <summary>
        /// 根据条件分页获取套餐
        /// </summary>
        [OperationContract]
        PageModel<App_DecorationPackInfo> GetDecorationPacks(string keywords, bool hasPackSeries, bool isBuildSuqare, IEnumerable<Guid> packSeriesIds, IEnumerable<string> layouts, IEnumerable<string> styleNos,
            float? minSquare, float? maxSquare, Dictionary<string, bool> sort, int pageIndex, int pageSize);

        #endregion

        #region 分页获取推荐套餐 --  PageModel<App_DecorationPackInfo> GetRecommendDecorationPacks(Guid propertyId, Guid? houseTypeId, float? minSquare, float? maxSquare, decimal? minPrice, decimal? maxPrice, int pageIndex, int pageSize);

        /// <summary>
        /// 分页获取推荐套餐
        /// </summary>
        [OperationContract]
        PageModel<App_DecorationPackInfo> GetRecommendDecorationPacks(Guid propertyId, Guid? houseTypeId, float? minSquare, float? maxSquare, decimal? minPrice,
            decimal? maxPrice, int pageIndex, int pageSize);

        #endregion


        #region 获取套餐明细 --  IEnumerable<App_DecorationPackItemGroupInfo> GetPackDetails(Guid packId);

        /// <summary>
        /// 获取套餐明细
        /// </summary>
        [OperationContract]
        IEnumerable<App_DecorationPackItemGroupInfo> GetPackDetails(Guid packId);

        #endregion


        #region 根据条件分页获取套餐 --  PageModel<App_DecorationPackInfo> GetDecorationPacksForMarketApp(string keywords, bool containsNoProperty,IList<Guid> propertys, Guid? propertyId, Guid? houseTypeId, bool hasPackSeries, IEnumerable<Guid> packSeriesIds,IEnumerable<string> styleNos, bool? isBuildSuqare, float? minSquare, float? maxSquare, Dictionary<string, bool> sort,int pageIndex, int pageSize)

        /// <summary>
        /// 根据条件分页获取套餐
        /// </summary>
        /// <param name="keywords">关键字</param>
        /// <param name="propertys">楼盘标签集（查询包含此集合标签）</param>
        /// <param name="containsNoProperty">是否包含无楼盘标签套餐（true查询）</param>
        /// <param name="propertyId">楼盘标签（查询包含此楼盘标签）</param>
        /// <param name="houseTypeId">户型标签</param>
        /// <param name="hasPackSeries">是否包含系列标签(全部,标签:true,其他:false)</param>
        /// <param name="packSeriesIds">系列标签集</param>
        /// <param name="styleNos">风格标签集</param>
        /// <param name="isBuildSuqare">是否是建筑面积搜索</param>
        /// <param name="minSquare">最小面积</param>
        /// <param name="maxSquare">最大面积</param>
        /// <param name="sort">排序</param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [OperationContract]
        PageModel<App_DecorationPackInfo> GetDecorationPacksForMarketApp(string keywords, bool containsNoProperty, IList<Guid> propertys,
            Guid? propertyId, Guid? houseTypeId, bool hasPackSeries, IEnumerable<Guid> packSeriesIds, IEnumerable<string> styleNos,
            bool? isBuildSuqare, float? minSquare, float? maxSquare, Dictionary<string, bool> sort, int pageIndex, int pageSize);

        #endregion

        #region # 获取套餐空间效果图 —— IEnumerable<App_DecorationPackSpaceImageInfo> GetPackSpaceImageInfos(Guid packId)

        /// <summary>
        /// 获取套餐空间效果图
        /// </summary>
        /// <param name="packId">套餐Id</param>
        /// <returns>套餐空间图册集</returns>
        [OperationContract]
        IEnumerable<App_DecorationPackSpaceImageInfo> GetPackSpaceImageInfos(Guid packId);

        #endregion

        #region # 根据Id获取固定套餐集 —— IEnumerable<App_DecorationPackInfo> GetDecorationPacksByIds(List<Guid> packIds)

        /// <summary>
        /// 根据Id获取固定套餐集
        /// </summary>
        /// <param name="packIds">固定套餐Id集</param>
        /// <returns></returns>
        [OperationContract]
        IEnumerable<App_DecorationPackInfo> GetDecorationPacksByIds(List<Guid> packIds);

        #endregion


        #region # 测试 Tuple —— void Test(Guid id)

        /// <summary>

        /// 测试 Tuple
        /// </summary>
        /// <param name="id"></param>
        [OperationContract]
        void Test(Guid id);

        #endregion

    }
}
