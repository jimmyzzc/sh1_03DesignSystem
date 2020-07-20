using HDIPlatform.DesignSystem.IAppService.DTOs.Outputs.BaleContext;
using ShSoft.Infrastructure;
using ShSoft.Infrastructure.DTOBase;
using ShSoft.ValueObjects.Enums;
using System;
using System.Collections.Generic;
using System.ServiceModel;
using HDIPlatform.DesignSystem.IAppService.DTOs.Outputs.ConfigContext;

namespace HDIPlatform.DesignSystem.IAppService.Interfaces
{
    /// <summary>
    /// 套餐配置服务契约接口
    /// </summary>
    [ServiceContract(Namespace = "http://HDIPlatform.DesignSystem.IAppService.Interfaces")]
    public interface IConfigContract : IApplicationService
    {
        //命令部分

        #region 套餐系列

        #region 创建套餐系列 -- void CreatePackSeries(string name, string describe, string groupName);

        /// <summary>
        /// 创建套餐系列
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="describe">描述</param>
        /// <param name="groupName">分组名</param>
        [OperationContract]
        PackSeriesInfo CreatePackSeries(string name, string describe, string groupName);

        #endregion

        #region 修改套餐系列 --  void UpdatePackSeries(string name,string describe, int sort, string groupName)

        /// <summary>
        /// 修改套餐系列
        /// </summary>
        /// <param name="packSeriesId">套餐系列Id</param>
        /// <param name="name">名称</param>
        /// <param name="describe">描述</param>
        /// <param name="sort">序号</param>
        /// <param name="groupName">分组名</param>
        [OperationContract]
        void UpdatePackSeries(Guid packSeriesId, string name, string describe, int sort, string groupName);

        #endregion

        #region 删除套餐系列 --    void DeletePackSeries(Guid packSeriesId);

        /// <summary>
        /// 删除套餐系列
        /// </summary>
        /// <param name="packSeriesId">套餐系列Id</param>
        [OperationContract]
        void DeletePackSeries(Guid packSeriesId);

        #endregion

        #endregion

        //查询部分

        #region 套餐系列

        #region 分页查询套餐系列集 -- PageModel<PackSeriesInfo> GetPackSeries(string keywords, int pageIndex, int pageSize);

        /// <summary>
        /// 分页查询套餐系列集
        /// </summary>
        /// <param name="keywords">关键字</param>
        /// <param name="pageIndex">页号</param>
        /// <param name="pageSize">页容量</param>
        /// <returns>套餐系列集</returns>
        [OperationContract]
        PageModel<PackSeriesInfo> GetPackSeries(string keywords, int pageIndex, int pageSize);

        #endregion

        #region 查询套餐系列标签及套餐数量集 -- Dictionary<PackSeriesInfo,int> GetPackSeriesCounts();

        /// <summary>
        /// 查询套餐系列标签及套餐数量集
        /// </summary>
        /// <param name="propertyId">关键字</param>
        /// <param name="applicableSquare">页号</param>
        /// <param name="isNewHouse">页容量</param>
        /// <returns>套餐系列标签及套餐数量集</returns>
        [OperationContract]
        Dictionary<PackSeriesInfo,int> GetPackSeriesCounts(Guid? propertyId, float? applicableSquare, bool? isNewHouse);

        #endregion


        #region 查询套餐分组系列标签及套餐数量集 --IEnumerable<Tuple<string, int, Dictionary<PackSeriesInfo, int>>> GetPackGroupSeriesCounts(Guid? propertyId, float? applicableSquare, bool? isNewHouse)

        /// <summary>
        /// 查询套餐分组系列标签及套餐数量集
        /// </summary>
        /// <param name="propertyId">关键字</param>
        /// <param name="applicableSquare">页号</param>
        /// <param name="isNewHouse">页容量</param>
        /// <returns>套餐分组系列标签及套餐数量集（分组名|排序|系列集）</returns>
        [OperationContract]
        IEnumerable<Tuple<string, int, Dictionary<PackSeriesInfo, int>>> GetPackGroupSeriesCounts(Guid? propertyId, float? applicableSquare,
            bool? isNewHouse);

        #endregion

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
