using System;
using System.Runtime.Serialization;
using ShSoft.Infrastructure.DTOBase;

namespace HDIPlatform.DesignSystem.IAppService.DTOs.Outputs.BaleContext
{
    /// <summary>
    /// 套餐选区内组数据传输对象
    /// </summary>
    /// <remarks>勿忘[DataMember]</remarks>
    [DataContract(Namespace = "http://HDIPlatform.DesignSystem.IAppService.DTOs.Outputs.BaleContext")]
    public class BalePackGroupInfo : BaseDTO
    {
        #region 选区Id —— Guid ChoiceAreaId
        /// <summary>
        /// 选区Id
        /// </summary>
        [DataMember]
        public Guid ChoiceAreaId { get; set; }
        #endregion


        #region 是否必选 —— bool Required
        /// <summary>
        /// 是否必选
        /// </summary>
        [DataMember]
        public bool Required { get; set; }
        #endregion

        #region 是否包含商品（定制套餐） —— bool ExistsSku
        /// <summary>
        /// 是否包含商品(定制套餐)
        /// </summary>
        [DataMember]
        public bool ExistsSku { get; set; }
        #endregion
    }
}
