using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using ShSoft.Infrastructure.DTOBase;
using ShSoft.ValueObjects.Enums;

namespace HDIPlatform.DesignSystem.IAppService.DTOs.Outputs.BaleContext
{
    /// <summary>
    /// 大包/定制套餐数据传输对象
    /// </summary>
    /// <remarks>勿忘[DataMember]</remarks>
    [DataContract(Namespace = "http://HDIPlatform.DesignSystem.IAppService.DTOs.Outputs.BaleContext")]
    public class BalePackInfo : BaseDTO
    {

        #region  折扣比例 —— float DiscountRatio
        /// <summary>
        /// 折扣比例
        /// </summary>
        [DataMember]
        public float DiscountRatio { get; set; }

        #endregion

        #region 套餐状态 —— ShelfStatus PackShelfStatus

        /// <summary>
        /// 套餐状态
        /// </summary>
        [DataMember]
        public ShelfStatus PackShelfStatus { get; set; }
        #endregion

        #region 套餐类型 ——BalePackType BalePackType

        /// <summary>
        /// 套餐类型
        /// </summary>
        [DataMember]
        public BalePackType BalePackType { get; set; }
        #endregion

        #region  封面 —— Guid CoverDrawing

        /// <summary>
        /// 套餐封面
        /// </summary>
        [DataMember]
        public Guid CoverDrawing { get; set; }
        #endregion

        #region 是否有下架商品 —— bool HasOffShelvedSku
        /// <summary>
        /// 是否有下架商品
        /// </summary>
        [DataMember]
        public bool HasOffShelvedSku { get; set; }
        #endregion

        #region 标签集 —— HashSet<string> Labels
        /// <summary>
        /// 标签集
        /// </summary>
        [DataMember]
        public HashSet<string> Labels { get; set; }
        #endregion

        #region 套餐浏览量 —— int Views
        /// <summary>
        /// 套餐浏览量
        /// </summary>
        [DataMember]
        public int Views { get; set; }
        #endregion

        #region 套餐销售量 —— int Sales
        /// <summary>
        /// 套餐销售量
        /// </summary>
        [DataMember]
        public int Sales { get; set; }
        #endregion

        #region 创建人 —— string Creater
        /// <summary>
        /// 创建人
        /// </summary>
        [DataMember]
        public string Creater { get; set; }
        #endregion

        #region 创建人Id —— Guid CreaterId
        /// <summary>
        /// 创建人Id
        /// </summary>
        [DataMember]
        public Guid CreaterId { get; set; }
        #endregion

        #region 源套餐Id —— Guid? SourcePackId
        /// <summary>
        /// 源套餐Id(源套餐一直为空)
        /// </summary>
        [DataMember]
        public Guid? SourcePackId { get; set; }
        #endregion

        #region 版本号 —— string VersionNumber
        /// <summary>
        /// 版本号(源套餐一直为空)（套餐名称首拼套餐类型首拼两位流水日期）
        /// </summary>
        [DataMember]
        public string VersionNumber { get; set; }
        #endregion

        #region 是否克隆 —— bool IsClone
        /// <summary>
        /// 是否克隆
        /// </summary>
        [DataMember]
        public bool IsClone { get; set; }
        #endregion
    }
}
