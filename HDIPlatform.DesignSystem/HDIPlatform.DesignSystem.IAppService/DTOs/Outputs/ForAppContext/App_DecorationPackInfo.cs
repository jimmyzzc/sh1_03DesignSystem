using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using HDIPlatform.DesignSystem.IAppService.DTOs.Outputs.ConfigContext;
using ShSoft.Infrastructure.DTOBase;
using ShSoft.ValueObjects.Enums;

namespace HDIPlatform.DesignSystem.IAppService.DTOs.Outputs.ForAppContext
{
    /// <summary>
    /// 套餐模板数据传输对象
    /// </summary>
    /// <remarks>勿忘[DataMember]</remarks>
    [DataContract(Namespace = "http://HDIPlatform.DesignSystem.IAppService.DTOs.Outputs.ForAppContext")]
    public class App_DecorationPackInfo : BaseDTO
    {
        #region 套餐状态 —— ShelfStatus Status
        /// <summary>
        /// 套餐状态
        /// </summary>
        [DataMember]
        public ShelfStatus Status { get; set; }
        #endregion

        #region 套餐类型 —— DecorationPackType PackType
        /// <summary>
        /// 套餐类型
        /// </summary>
        [DataMember]
        public DecorationPackType PackType { get; set; }
        #endregion

        #region 套餐类别 —— DecorationPackKind PackKind
        /// <summary>
        /// 套餐类别
        /// </summary>
        [DataMember]
        public DecorationPackKind PackKind { get; set; }
        #endregion

        #region 套餐模式 —— DecorationPackMode PackMode
        /// <summary>
        /// 套餐模式
        /// </summary>
        [DataMember]
        public DecorationPackMode PackMode { get; set; }
        #endregion

        #region 套餐颜色 —— DecorationPackColor Color
        /// <summary>
        /// 套餐颜色
        /// </summary>
        [DataMember]
        public DecorationPackColor Color { get; set; }
        #endregion

        #region 是否平米使用面积定价 —— bool IsUnitActual
        /// <summary>
        /// 是否平米使用面积定价
        /// </summary>
        [DataMember]
        public bool IsUnitActual { get; set; }
        #endregion

        #region 是否整体使用面积定价 —— bool IsActual
        /// <summary>
        /// 是否整体使用面积定价
        /// </summary>
        [DataMember]
        public bool IsActual { get; set; }
        #endregion

        #region  套餐总面积 实际面积（使用面积） —— float Square
        /// <summary>
        ///  套餐总面积 实际面积（使用面积）
        /// </summary>
        [DataMember]
        public float Square { get; set; }
        #endregion

        #region 实际面积（使用面积）单价 —— decimal UnitPrice
        /// <summary>
        /// 实际面积（使用面积）单价
        /// </summary>
        [DataMember]
        public decimal UnitPrice { get; set; }
        #endregion

        #region 整体定价：使用面积总价 —— decimal TotalPrice
        /// <summary>
        /// 整体定价：使用面积总价
        /// </summary>
        [DataMember]
        public decimal TotalPrice { get; set; }
        #endregion

        #region 平米定价方式： 使用面积总价=最低购买面积*单价 —— decimal UnitTotalPrice
        /// <summary>
        /// 平米定价方式： 使用面积总价=最低购买面积*单价
        /// </summary>
        [DataMember]
        public decimal UnitTotalPrice { get; set; }
        #endregion

        #region 是否平米整体建筑定价 —— bool IsUnitBuilding
        /// <summary>
        /// 是否平米整体建筑定价
        /// </summary>
        [DataMember]
        public bool IsUnitBuilding { get; set; }
        #endregion

        #region 是否整体建筑定价 —— bool IsBuilding
        /// <summary>
        /// 是否整体建筑定价
        /// </summary>
        [DataMember]
        public bool IsBuilding { get; set; }
        #endregion

        #region 套餐总面积 实际面积（建筑面积）—— float BuildingSquare
        /// <summary>
        /// 套餐总面积 实际面积（建筑面积）
        /// </summary>
        [DataMember]
        public float BuildingSquare { get; set; }
        #endregion

        #region 建筑面积单价 —— decimal BuildingUnitPrice
        /// <summary>
        /// 建筑面积单价
        /// </summary>
        [DataMember]
        public decimal BuildingUnitPrice { get; set; }
        #endregion

        #region 整体定价：建筑面积总价 —— decimal BuildingTotalPrice
        /// <summary>
        /// 整体定价：建筑面积总价
        /// </summary>
        [DataMember]
        public decimal BuildingTotalPrice { get; set; }
        #endregion

        #region 平米定价方式： 建筑面积总价=最低购买面积*单价 —— decimal UnitBuildingTotalPrice
        /// <summary>
        /// 平米定价方式： 建筑面积总价=最低购买面积*单价
        /// </summary>
        [DataMember]
        public decimal UnitBuildingTotalPrice { get; set; }
        #endregion

        #region 适用新房 —— bool NewHouse
        /// <summary>
        /// 适用新房
        /// </summary>
        [DataMember]
        public bool NewHouse { get; set; }
        #endregion

        #region 适用二手房 —— bool SecondHandHouse
        /// <summary>
        /// 适用二手房
        /// </summary>
        [DataMember]
        public bool SecondHandHouse { get; set; }
        #endregion


        #region 套餐系列标签 ——IEnumerable<string> PackSeries
        /// <summary>
        /// 套餐系列标签
        /// </summary>
        [DataMember]
        public IEnumerable<string> PackSeries { get; set; }
        #endregion

        #region 套餐系列标签名称|说明 ——Dictionary<string,string> PackSerieInfos
        /// <summary>
        /// 套餐系列标签名称|说明
        /// </summary>
        [DataMember]
        public Dictionary<string,string> PackSerieInfos { get; set; }
        #endregion

        #region 套餐适用的最小使用面积 —— float Square
        /// <summary>
        /// 套餐适用的最小使用面积
        /// </summary>
        [DataMember]
        public float MinApplicableSquare { get; set; }
        #endregion

        #region 套餐适用的最大使用面积—— float MaxApplicableSquare
        /// <summary>
        /// 套餐适用的最大使用面积
        /// </summary>
        [DataMember]
        public float MaxApplicableSquare { get; set; }
        #endregion


        #region 超出是否整体定价 —— bool IsWhole
        /// <summary>
        /// 超出是否整体定价
        /// </summary>
        [DataMember]
        public bool IsWhole { get; set; }
        #endregion

        #region 整体 超出补单价 —— decimal Premium
        /// <summary>
        /// 整体 超出补单价
        /// </summary>
        [DataMember]
        public decimal Premium { get; set; }
        #endregion

        #region 标准面积 —— float CriterionSquare
        /// <summary>
        /// 标准面积（公式）
        /// </summary>
        [DataMember]
        public float CriterionSquare { get; set; }
        #endregion

        #region 是否已定价 —— bool Priced
        /// <summary>
        /// 是否已定价
        /// </summary>
        [DataMember]
        public bool Priced { get; set; }
        #endregion

        #region 定价类型 —— DecorationPackPricingType? PricingType
        /// <summary>
        /// 定价类型
        /// </summary>
        [DataMember]
        public DecorationPackPricingType? PricingType { get; set; }
        #endregion


        //默认方案

        #region 方案选购封面 —— string Cover
        /// <summary>
        /// 方案选购封面
        /// </summary>
        [DataMember]
        public string Cover { get; set; }
        #endregion

        #region 方案选购封面描述 —— string CoverDescription
        /// <summary>
        /// 方案选购封面描述
        /// </summary>
        [DataMember]
        public string CoverDescription { get; set; }
        #endregion

        #region 方案概况图片集 —— IList<string> SchemeImages
        /// <summary>
        /// 方案概况图片集
        /// </summary>
        [DataMember]
        public IList<string> SchemeImages { get; set; }
        #endregion

        #region 方案概况描述 ——  List<string> SchemeDescriptions
        /// <summary>
        /// 方案概况描述
        /// </summary>
        [DataMember]
        public List<string> SchemeDescriptions { get; set; }
        #endregion

        #region 方案视频音频地址链接 —— string SchemeVideoAudioLink
        /// <summary>
        /// 方案视频音频地址链接
        /// </summary>
        [DataMember]
        public string SchemeVideoAudioLink { get; set; }
        #endregion

        #region 方案视频音频地址链接 —— string SchemeVideoAudioLinkFileId
        /// <summary>
        /// 方案视频音频地址链接文件Id
        /// </summary>
        [DataMember]
        public string SchemeVideoAudioLinkFileId { get; set; }
        #endregion

        #region 空间信息集 —— Dictionary<Guid, string> SpaceInfos
        /// <summary>
        /// 空间信息集
        /// </summary>
        [DataMember]
        public Dictionary<Guid, string> SpaceInfos { get; set; }
        #endregion

        #region 居室集 —— IList<string> Layouts
        /// <summary>
        /// 居室集
        /// </summary>
        [DataMember]
        public IList<string> Layouts { get; set; }
        #endregion

        #region 设计风格集 —— IList<string> StyleNos
        /// <summary>
        /// 设计风格集
        /// </summary>
        [DataMember]
        public IList<string> StyleNos { get; set; }
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


        #region 平米定价最低购买使用面积—— float UnitSquare
        /// <summary>
        /// 平米定价最低购买使用面积
        /// </summary>
        [DataMember]
        public float UnitSquare { get; set; }
        #endregion

        #region 平米定价最低购买建筑面积—— float UnitBuildingSquare
        /// <summary>
        /// 平米定价最低购买建筑面积
        /// </summary>
        [DataMember]
        public float UnitBuildingSquare { get; set; }
        #endregion

        #region 产品说明书Id —— string Instructions
        /// <summary>
        /// 产品说明书(文件Id)
        /// </summary>
        [DataMember]
        public string Instructions { get; set; }
        #endregion

        #region 产品说明书名称 —— string InstructionsName
        /// <summary>
        /// 产品说明书名称
        /// </summary>
        [DataMember]
        public string InstructionsName { get; set; }
        #endregion


        #region 楼盘集 —— Dictionary<Guid, string> Propertys
        /// <summary>
        ///  楼盘集
        /// </summary>
        [DataMember]
        public Dictionary<Guid, string> Propertys { get; set; }
        #endregion

        #region 设置管理费参考价 —— bool IsManageFee
        /// <summary>
        /// 设置管理费参考价
        /// </summary>
        [DataMember]
        public bool IsManageFee { get; set; }
        #endregion

        #region 设置水电预收参考价 —— bool IsWaterElectricityFee
        /// <summary>
        /// 设置水电预收参考价
        /// </summary>
        [DataMember]
        public bool IsWaterElectricityFee { get; set; }
        #endregion

        #region 管理费参考价 —— decimal ManageFee
        /// <summary>
        /// 管理费参考价
        /// </summary>
        [DataMember]
        public decimal ManageFee { get; set; }
        #endregion

        #region 水电预收参考价 —— decimal WaterElectricityFee
        /// <summary>
        /// 水电预收参考价
        /// </summary>
        [DataMember]
        public decimal WaterElectricityFee { get; set; }
        #endregion
    }
}
