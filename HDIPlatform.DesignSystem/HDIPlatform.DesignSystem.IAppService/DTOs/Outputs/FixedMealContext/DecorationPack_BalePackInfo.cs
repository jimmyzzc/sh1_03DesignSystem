using HDIPlatform.DesignSystem.IAppService.DTOs.Outputs.BaleContext;
using ShSoft.Infrastructure.DTOBase;
using ShSoft.ValueObjects.Enums;
using System;
using System.Runtime.Serialization;

namespace HDIPlatform.DesignSystem.IAppService.DTOs.Outputs.FixedMealContext
{
    /// <summary>
    /// 套餐模板 - 大包/定制模板关系数据传输对象
    /// </summary>
    /// <remarks>勿忘[DataMember]</remarks>
    [DataContract(Namespace = "http://HDIPlatform.DesignSystem.IAppService.DTOs.Outputs.FixedMealContext")]
    public class DecorationPack_BalePackInfo : BaseDTO
    {
        #region 套餐模板Id —— Guid DecorationPackId
        /// <summary>
        /// 套餐模板Id
        /// </summary>
        [DataMember]
        public Guid DecorationPackId { get; set; }
        #endregion

        #region 大包/定制模板Id —— Guid BalePackId
        /// <summary>
        /// 大包/定制模板Id
        /// </summary>
        [DataMember]
        public Guid BalePackId { get; set; }
        #endregion

        #region 大包/定制模板类型 —— BalePackType BalePackType
        /// <summary>
        /// 大包/定制模板类型
        /// </summary>
        [DataMember]
        public BalePackType BalePackType { get; set; }
        #endregion


        #region 导航属性 - 套餐模板 —— DecorationPackInfo DecorationPackInfo
        /// <summary>
        /// 导航属性 - 套餐模板
        /// </summary>
        [DataMember]
        public DecorationPackInfo DecorationPackInfo { get; set; }
        #endregion

        #region 导航属性 - 大包/定制模板 —— BalePackInfo BalePackInfo
        /// <summary>
        /// 导航属性 - 大包/定制模板
        /// </summary>
        [DataMember]
        public BalePackInfo BalePackInfo { get; set; }
        #endregion
    }
}
