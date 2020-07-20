using System.Collections.Generic;
using ShSoft.Infrastructure.DTOBase;
using ShSoft.ValueObjects.Enums;
using System.Runtime.Serialization;

namespace HDIPlatform.DesignSystem.IAppService.DTOs.Outputs.FixedMealContext
{
    /// <summary>
    /// 套餐模板空间数据传输对象
    /// </summary>
    /// <remarks>勿忘[DataMember]</remarks>
    [DataContract(Namespace = "http://HDIPlatform.DesignSystem.IAppService.DTOs.Outputs.FixedMealContext")]
    public class DecorationPackSpaceInfo : BaseDTO
    {
        #region 空间面积 —— float Square
        /// <summary>
        /// 空间面积
        /// </summary>
        [DataMember]
        public float Square { get; set; }
        #endregion

        #region 空间类型 —— SpaceType SpaceType
        /// <summary>
        /// 空间类型
        /// </summary>
        [DataMember]
        public SpaceType SpaceType { get; set; }
        #endregion

        #region 标准面积 —— float CriterionSquare
        /// <summary>
        /// 标准面积(公式)
        /// </summary>
        [DataMember]
        public float CriterionSquare { get; set; }
        #endregion

        #region 空间占比 —— decimal SpaceRatio
        /// <summary>
        /// 空间占比（标准空间面积/标准整体面积|5/60）
        /// </summary>
        [DataMember]
        public decimal SpaceRatio { get; set; }
        #endregion

        #region 单价 —— decimal UnitPrice
        /// <summary>
        /// 超出面积单价
        /// </summary>
        [DataMember]
        public decimal UnitPrice { get; set; }
        #endregion

        #region 套餐空间定价类型 ——  DecorationPackSpacePricingType? DecorationPackSpacePricingType
        /// <summary>
        /// 套餐空间定价类型
        /// </summary>
        [DataMember]
        public DecorationPackSpacePricingType? DecorationPackSpacePricingType { get; set; }
        #endregion

        #region 导航属性 - 套餐模板 —— DecorationPackInfo DecorationPackInfo
        /// <summary>
        /// 导航属性 - 套餐模板
        /// </summary>
        [DataMember]
        public DecorationPackInfo DecorationPackInfo { get; set; }
        #endregion

        #region 导航属性 - 套餐模板空间详情 —— IEnumerable<DecorationPackSpaceDetailInfo> DecorationPackSpaceDetailInfos
        /// <summary>
        /// 导航属性 - 套餐模板空间详情
        /// </summary>
        [DataMember]
        public IEnumerable<DecorationPackSpaceDetailInfo> DecorationPackSpaceDetailInfos { get; set; }
        #endregion

        #region 存在下架商品|工艺
        /// <summary>
        /// 存在下架商品|工艺
        /// </summary>
        [DataMember]
        public bool ExistsOffShelvedSkuCraft { get; set; } 
        #endregion

        #region 存在变价商品|工艺
        /// <summary>
        /// 存在变价商品|工艺
        /// </summary>
        [DataMember]
        public bool ExistsChangedSkuCraft { get; set; }
        #endregion
    }
}
