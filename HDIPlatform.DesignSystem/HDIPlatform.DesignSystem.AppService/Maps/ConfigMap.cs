using HDIPlatform.DesignSystem.Domain.Entities.ConfigContext;
using HDIPlatform.DesignSystem.IAppService.DTOs.Outputs.ConfigContext;
using ShSoft.Common.PoweredByLee;

namespace HDIPlatform.DesignSystem.AppService.Maps
{
    /// <summary>
    /// 配置相关映射工具类
    /// </summary>
    public static class ConfigMap
    {

        #region 套餐系列映射 —— static PackSeriesInfo ToDTO(this PackSeries packSeries)

        /// <summary>
        ///  套餐系列映射
        /// </summary>
        /// <param name="packSeries">套餐系列领域模型</param>
        /// <returns>套餐系列DTO</returns>
        public static PackSeriesInfo ToDTO(this PackSeries packSeries)
        {

            PackSeriesInfo packSeriesInfo = Transform<PackSeries, PackSeriesInfo>.Map(packSeries);
            //PackSeriesInfo packSeriesInfo = new PackSeriesInfo();
            //packSeriesInfo.Id = packSeries.Id;
            //packSeriesInfo.Name = packSeries.Name;
            //packSeriesInfo.AddedTime = packSeries.AddedTime;
            //packSeriesInfo.Describe = packSeries.Describe;
            //packSeriesInfo.Sort = packSeries.Sort;
            return packSeriesInfo;
        }

        #endregion

    }
}
