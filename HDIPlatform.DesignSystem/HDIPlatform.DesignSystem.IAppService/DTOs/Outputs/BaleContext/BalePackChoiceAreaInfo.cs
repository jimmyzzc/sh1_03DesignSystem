using System.Runtime.Serialization;
using ShSoft.Infrastructure.DTOBase;

namespace HDIPlatform.DesignSystem.IAppService.DTOs.Outputs.BaleContext
{
    /// <summary>
    /// 套餐选区数据传输对象
    /// </summary>
    /// <remarks>勿忘[DataMember]</remarks>
    [DataContract(Namespace = "http://HDIPlatform.DesignSystem.IAppService.DTOs.Outputs.BaleContext")]
    public class BalePackChoiceAreaInfo : BaseDTO
    {

        #region 导航属性 - 大包/定制套餐 —— BalePackInfo BalePackInfo
        /// <summary>
        /// 导航属性 - 大包/定制套餐
        /// </summary>
        [DataMember]
        public BalePackInfo BalePackInfo { get; set; }
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
