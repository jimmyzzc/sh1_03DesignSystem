using System.Collections.Generic;
using ShSoft.ValueObjects.Enums;
using System.Runtime.Serialization;

namespace HDIPlatform.DesignSystem.IAppService.DTOs.Inputs
{
    /// <summary>
    /// 套餐模板项参数模型
    /// </summary>
    /// <remarks>勿忘[DataMember]</remarks>
    [DataContract(Namespace = "http://HDIPlatform.DesignSystem.IAppService.DTOs.Inputs")]
    public struct PackItemParam
    {
        /// <summary>
        /// 模板项名称
        /// </summary>
        [DataMember]
        public string ItemName;

        /// <summary>
        /// 默认商品SKU数量
        /// </summary>
        [DataMember]
        public float DefaultSkuQuantity;

        /// <summary>
        /// 默认工艺工程量
        /// </summary>
        [DataMember]
        public float DefaultCraftQuantity;

        /// <summary>
        /// 品类区域
        /// </summary>
        [DataMember]
        public CategoryArea CategoryArea;

        /// <summary>
        /// 套餐选区工艺集
        /// </summary>
        [DataMember] public IEnumerable<DecorationPackCraftParam> DecorationPackCraftParams;

        /// <summary>
        /// 套餐选区商品集
        /// </summary>
        [DataMember] public IEnumerable<DecorationPackSkuParam> DecorationPackSkuParams;
    }
}
