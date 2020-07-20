using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using ShSoft.Infrastructure.DTOBase;

namespace HDIPlatform.DesignSystem.IAppService.DTOs.Outputs.BaleContext
{
    /// <summary>
    /// 套餐组内品类数据传输对象
    /// </summary>
    /// <remarks>勿忘[DataMember]</remarks>
    [DataContract(Namespace = "http://HDIPlatform.DesignSystem.IAppService.DTOs.Outputs.BaleContext")]
    public class BalePackCategoryInfo : BaseDTO
    {

        #region 品类Id —— Guid CategoryId
        /// <summary>
        /// 品类Id
        /// </summary>
        [DataMember]
        public Guid CategoryId { get; set; }

        #endregion

        #region 导航属性 - 套餐选区内组 —— BalePackGroupInfo BalePackGroupInfo
        /// <summary>
        /// 导航属性 - 套餐选区内组
        /// </summary>
        [DataMember]
        public BalePackGroupInfo BalePackGroupInfo { get; set; }
        #endregion

        #region 是否包含商品 —— bool ExistsSku
        /// <summary>
        /// 是否包含商品
        /// </summary>
        [DataMember]
        public bool ExistsSku { get; set; }
        #endregion
    }
}
