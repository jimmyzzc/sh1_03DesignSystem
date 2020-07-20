using System;
using System.Runtime.Serialization;

namespace HDIPlatform.DesignSystem.IAppService.DTOs.Outputs.HouseTypePackContext
{
    /// <summary>
    /// 户型定价模型
    /// </summary>
    /// <remarks>勿忘[DataMember]</remarks>
    [DataContract(Namespace = "http://HDIPlatform.DesignSystem.IAppService.DTOs.Outputs.HouseTypePackContext")]
    public struct HouseTypePriceInfo
    {
        #region 楼盘Id —— Guid PropertyId
        /// <summary>
        /// 楼盘Id
        /// </summary>
        [DataMember]
        public Guid PropertyId { get; set; }
        #endregion

        #region 楼盘名称 —— string PropertyName
        /// <summary>
        /// 楼盘名称
        /// </summary>
        [DataMember]
        public string PropertyName { get; set; }
        #endregion

        #region 楼盘地址 —— string PropertyAddress
        /// <summary>
        /// 楼盘地址
        /// </summary>
        [DataMember]
        public string PropertyAddress { get; set; }
        #endregion

        #region 户型Id —— Guid HouseTypeId
        /// <summary>
        /// 户型Id
        /// </summary>
        [DataMember]
        public Guid HouseTypeId { get; set; }
        #endregion

        #region 户型名称 —— string HouseTypeName
        /// <summary>
        /// 户型名称
        /// </summary>
        [DataMember]
        public string HouseTName { get; set; }
        #endregion

        #region 使用面积单价 —— decimal UnitPrice
        /// <summary>
        /// 使用面积单价
        /// </summary>
        [DataMember]
        public decimal UnitPrice { get; set; }
        #endregion
    }
}
