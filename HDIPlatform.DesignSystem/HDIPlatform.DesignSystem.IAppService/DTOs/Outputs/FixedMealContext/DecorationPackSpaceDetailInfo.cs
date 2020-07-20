using System.Runtime.Serialization;
using ShSoft.Infrastructure.DTOBase;
using ShSoft.ValueObjects.Enums;

namespace HDIPlatform.DesignSystem.IAppService.DTOs.Outputs.FixedMealContext
{
    /// <summary>
    /// 套餐模板空间详情数据传输对象
    /// </summary>
    /// <remarks>勿忘[DataMember]</remarks>
    [DataContract(Namespace = "http://HDIPlatform.DesignSystem.IAppService.DTOs.Outputs.FixedMealContext")]
    public class DecorationPackSpaceDetailInfo : BaseDTO
    {

        #region 数值标准 —— NumericalStandard NumericalStandard

        /// <summary>
        /// 数值标准
        /// </summary>
        [DataMember]
        public NumericalStandard NumericalStandard { get; set; }
        #endregion

        #region 地面长度 —— decimal GroundLength
        /// <summary>
        /// 地面长度
        /// </summary>
        [DataMember]
        public decimal GroundLength { get; set; }
        #endregion

        #region 地面宽度 —— decimal GroundWidth
        /// <summary>
        /// 地面宽度
        /// </summary>
        [DataMember]
        public decimal GroundWidth { get; set; }
        #endregion

        #region 空间周长 —— decimal SpacePerimeter
        /// <summary>
        /// 空间周长
        /// </summary>
        [DataMember]
        public decimal SpacePerimeter { get; set; }
        #endregion

        #region 墙面高度 —— decimal WallHigh
        /// <summary>
        /// 墙面高度
        /// </summary>
        [DataMember]
        public decimal WallHigh { get; set; }
        #endregion

        #region 洞口面积 —— decimal HoleArea
        /// <summary>
        /// 洞口面积
        /// </summary>
        [DataMember]
        public decimal HoleArea { get; set; }
        #endregion

        #region 立面面积 —— decimal FacadeArea
        /// <summary>
        /// 立面面积
        /// </summary>
        [DataMember]
        public decimal FacadeArea { get; set; }
        #endregion

        #region 地面面积 —— decimal GroundArea
        /// <summary>
        /// 地面面积
        /// </summary>
        [DataMember]
        public decimal GroundArea { get; set; }
        #endregion

        #region 棚面面积 —— decimal CeilingArea
        /// <summary>
        /// 棚面面积
        /// </summary>
        [DataMember]
        public decimal CeilingArea { get; set; }
        #endregion
    }
}
