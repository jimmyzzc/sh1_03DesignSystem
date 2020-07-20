using HDIPlatform.DesignSystem.Domain.Entities.HouseTypePackContext;
using HDIPlatform.DesignSystem.IAppService.DTOs.Outputs.HouseTypePackContext;
using ShSoft.Common.PoweredByLee;

namespace HDIPlatform.DesignSystem.AppService.Maps
{
    /// <summary>
    /// XXX相关映射工具类
    /// </summary>
    public static class HouseTypePackMap
    {
        #region # 示例 —— static OrderInfo ToDTO(this Order order)
        /// <summary>
        /// 户型定价映射
        /// </summary>
        /// <param name="houseTypePrice">户型定价领域模型</param>
        /// <returns>户型定价数据传输对象</returns>
        public static HouseTypePriceInfo ToDTO(this HouseTypePrice houseTypePrice)
        {
            HouseTypePriceInfo houseTypePriceInfo = Transform<HouseTypePrice, HouseTypePriceInfo>.Map(houseTypePrice);

            return houseTypePriceInfo;
        }
        #endregion
    }
}
