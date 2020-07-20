using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using ShSoft.Infrastructure.DTOBase;

namespace HDIPlatform.DesignSystem.IAppService.DTOs.Outputs.BaleContext
{
    /// <summary>
    /// 大包|定制套餐选区树数据传输对象
    /// </summary>
    /// <remarks>勿忘[DataMember]</remarks>
    [DataContract(Namespace = "http://HDIPlatform.DesignSystem.IAppService.DTOs.Outputs.BaleContext")]
    public class BalePackAreaTreeInfo
    {

        #region 选区|组|品类|品牌Id —— Guid ItemId
        /// <summary>
        /// 选区|组|品类|品牌Id 
        /// </summary>
        [DataMember]
        public Guid ItemId { get; set; }
        #endregion

        #region 选区|组|品类|品牌名称 —— string ItemName
        /// <summary>
        /// 选区|组|品类|品牌名称
        /// </summary>

        [DataMember]
        public string ItemName { get; set; }
        #endregion

        #region 是否必选 （组）—— bool IsRequired
        /// <summary>
        /// 是否必选（组）
        /// </summary>
        [DataMember]
        public bool IsRequired { get; set; }
        #endregion


        #region 子集 ——IEnumerable<BalePackAreaTreeInfo> ChildNodes
        /// <summary>
        /// 子集
        /// </summary>
        [DataMember]
        public IEnumerable<BalePackAreaTreeInfo> ChildNodes { get; set; }
        #endregion

        #region 定制套餐-品类配置商品集 ——IEnumerable<Guid> SkuIds
        /// <summary>
        /// 定制套餐-品类配置商品集集
        /// </summary>
        [DataMember]
        public IEnumerable<Guid> SkuIds { get; set; }
        #endregion

    }
}
