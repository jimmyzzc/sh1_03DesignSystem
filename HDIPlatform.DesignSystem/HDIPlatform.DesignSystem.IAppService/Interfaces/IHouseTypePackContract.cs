using HDIPlatform.DesignSystem.IAppService.DTOs.Outputs.BaleContext;
using ShSoft.Infrastructure;
using ShSoft.Infrastructure.DTOBase;
using ShSoft.ValueObjects.Enums;
using System;
using System.Collections.Generic;
using System.ServiceModel;
using HDIPlatform.DesignSystem.IAppService.DTOs.Inputs;
using HDIPlatform.DesignSystem.IAppService.DTOs.Outputs.HouseTypePackContext;

namespace HDIPlatform.DesignSystem.IAppService.Interfaces
{
    /// <summary>
    /// 户型套餐服务契约接口
    /// </summary>
    [ServiceContract(Namespace = "http://HDIPlatform.DesignSystem.IAppService.Interfaces")]
    public interface IHouseTypePackContract : IApplicationService
    {
        //命令部分

       
        #region # 新增户型定价 —— void AddHouseTypePrice(IEnumerable<HouseTypePriceParam> houseTypePriceParams, Guid operatorId, string @operator)
        /// <summary>
        /// 新增户型定价
        /// </summary>
        /// <param name="houseTypePriceParams">户型定价模型</param>
        /// <param name="operatorId">操作人</param>
        /// <param name="operator">操作人</param>
        [OperationContract]
        void AddHouseTypePrice(IEnumerable<HouseTypePriceParam> houseTypePriceParams, Guid operatorId, string @operator);

        #endregion

        #region  # 编辑户型定价 ——  void UpdateHouseTypePrice(Dictionary<Guid, decimal> houseTypePrices, Guid operatorId, string @operator)

        /// <summary>
        /// 编辑户型定价
        /// </summary>
        /// <param name="houseTypePrices">户型定价集（户型定价Id|单价）</param>
        /// <param name="operatorId">操作人</param>
        /// <param name="operator">操作人</param>
        [OperationContract]
        void UpdateHouseTypePrice(Dictionary<Guid, decimal> houseTypePrices, Guid operatorId, string @operator);

        #endregion

        #region # 删除户型定价 —— void DeleteHouseTypePrice(Guid houseTypeId)

        /// <summary>
        /// 删除户型定价
        /// </summary>
        /// <param name="houseTypeId">户型定价Id</param>
        [OperationContract]
        void DeleteHouseTypePrice(Guid houseTypeId);

        #endregion

        #region # 删除楼盘下所有户型定价 —— void DeleteHouseTypePriceByPId(Guid propertyId)

        /// <summary>
        /// 删除楼盘下所有户型定价
        /// </summary>
        /// <param name="propertyId">楼盘Id</param>
        [OperationContract]
        void DeleteHouseTypePriceByPId(Guid propertyId);

        #endregion

        //查询部分

        #region # 分页获取楼盘下户型定价 —— PageModel<HouseTypePriceInfo> GetHouseTypePrices(Guid propertyId, Dictionary<string, bool> sort, int pageIndex, int pageSize)

        /// <summary>
        /// 分页获取楼盘下户型定价
        /// </summary>
        /// <param name="propertyId"></param>
        /// <param name="sort"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [OperationContract]
        PageModel<HouseTypePriceInfo> GetHouseTypePrices(Guid propertyId, Dictionary<string, bool> sort, int pageIndex, int pageSize);

        #endregion

        #region  # 分页获取定价户型楼盘列表 —— PageModel<HouseTypePriceInfo> GetPropertys(string keywords, Dictionary<string, bool> sort, int pageIndex, int pageSize)

        /// <summary>
        /// 分页获取定价户型楼盘列表
        /// </summary>
        /// <param name="keywords">关键字</param>
        /// <param name="sort"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [OperationContract]
        PageModel<HouseTypePriceInfo> GetPropertys(string keywords, Dictionary<string, bool> sort, int pageIndex, int pageSize);

        #endregion



    }
}
