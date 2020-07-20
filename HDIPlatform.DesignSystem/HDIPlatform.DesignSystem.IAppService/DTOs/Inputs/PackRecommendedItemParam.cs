using System.Collections.Generic;
using ShSoft.ValueObjects.Enums;
using System.Runtime.Serialization;

namespace HDIPlatform.DesignSystem.IAppService.DTOs.Inputs
{
    /// <summary>
    /// 套餐模板推荐项参数模型
    /// </summary>
    /// <remarks>勿忘[DataMember]</remarks>
    [DataContract(Namespace = "http://HDIPlatform.DesignSystem.IAppService.DTOs.Inputs")]
    public struct PackRecommendedItemParam
    {
        /// <summary>
        /// 模板项名称
        /// </summary>
        [DataMember]
        public string ItemName;

        /// <summary>
        /// 备注
        /// </summary>
        [DataMember]
        public string Remark;

        /// <summary>
        /// 套餐选区商品集
        /// </summary>
        [DataMember] public IEnumerable<DecorationPackRecommendedSkuParam> DecorationPackRecommendedSkuParams;
    }
}
