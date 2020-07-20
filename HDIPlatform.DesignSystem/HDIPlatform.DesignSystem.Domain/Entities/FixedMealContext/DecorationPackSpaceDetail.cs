using System;
using ShSoft.Infrastructure.EntityBase;
using ShSoft.ValueObjects.Enums;

namespace HDIPlatform.DesignSystem.Domain.Entities.FixedMealContext
{
    /// <summary>
    /// 套餐模板空间详情
    /// </summary>
    [Serializable]
    public class DecorationPackSpaceDetail : PlainEntity
    {
        #region # 构造器

        #region 01.无参构造器
        /// <summary>
        /// 无参构造器
        /// </summary>
        protected DecorationPackSpaceDetail() { }
        #endregion

        #region 02.创建套餐模板空间详情构造器
        /// <summary>
        /// 创建套餐模板空间详情构造器
        /// </summary>
        /// <param name="numericalStandard">数值标准</param>
        /// <param name="groundLength">地面长度</param>
        /// <param name="groundWidth">地面宽度</param>
        /// <param name="spacePerimeter">空间周长</param>
        /// <param name="wallHigh">墙面高度</param>
        /// <param name="holeArea">洞口面积</param>
        /// <param name="facadeArea">立面面积</param>
        /// <param name="groundArea">地面面积</param>
        /// <param name="ceilingArea">棚面面积</param>
        public DecorationPackSpaceDetail(NumericalStandard numericalStandard, decimal groundLength, decimal groundWidth, decimal spacePerimeter, decimal wallHigh, decimal holeArea, decimal facadeArea, decimal groundArea, decimal ceilingArea)
        {
            NumericalStandard = numericalStandard;
            GroundLength = groundLength;
            GroundWidth = groundWidth;
            SpacePerimeter = spacePerimeter;
            WallHigh = wallHigh;
            HoleArea = holeArea;
            FacadeArea = facadeArea;
            GroundArea = groundArea;
            CeilingArea = ceilingArea;
        }
        #endregion

        #endregion

        #region # 属性

        #region 数值标准 —— NumericalStandard NumericalStandard

        /// <summary>
        /// 数值标准
        /// </summary>
        public NumericalStandard NumericalStandard { get; private set; }
        #endregion

        #region 地面长度 —— decimal GroundLength
        /// <summary>
        /// 地面长度
        /// </summary>
        public decimal GroundLength { get; private set; } 
        #endregion

        #region 地面宽度 —— decimal GroundWidth 
        /// <summary>
        /// 地面宽度
        /// </summary>
        public decimal GroundWidth { get; private set; } 
        #endregion

        #region 空间周长 —— decimal SpacePerimeter 
        /// <summary>
        /// 空间周长
        /// </summary>
        public decimal SpacePerimeter { get; private set; }
        #endregion

        #region 墙面高度 —— decimal WallHigh 
        /// <summary>
        /// 墙面高度
        /// </summary>
        public decimal WallHigh { get; private set; } 
        #endregion

        #region 洞口面积 —— decimal HoleArea 
        /// <summary>
        /// 洞口面积
        /// </summary>
        public decimal HoleArea { get; private set; } 
        #endregion

        #region 立面面积 —— decimal FacadeArea 
        /// <summary>
        /// 立面面积
        /// </summary>
        public decimal FacadeArea { get; private set; }
        #endregion

        #region 地面面积 —— decimal GroundArea 
        /// <summary>
        /// 地面面积
        /// </summary>
        public decimal GroundArea { get; private set; } 
        #endregion

        #region 棚面面积 —— decimal CeilingArea 
        /// <summary>
        /// 棚面面积
        /// </summary>
        public decimal CeilingArea { get; private set; } 
        #endregion

        #region 导航属性 - 套餐模板空间 —— DecorationPackSpace DecorationPackSpace
        /// <summary>
        /// 导航属性 - 套餐模板空间
        /// </summary>
        public virtual DecorationPackSpace DecorationPackSpace { get; private set; }
        #endregion

        #endregion

        #region # 方法

        #region 修改套餐模板空间详情套餐模板空间详情 —— void UpdateSpaceDetailInfo(NumericalStandard numericalStandard...
        /// <summary>
        /// 修改套餐模板空间详情套餐模板空间详情
        /// </summary>
        /// <param name="numericalStandard">数值标准</param>
        /// <param name="groundLength">地面长度</param>
        /// <param name="groundWidth">地面宽度</param>
        /// <param name="spacePerimeter">空间周长</param>
        /// <param name="wallHigh">墙面高度</param>
        /// <param name="holeArea">洞口面积</param>
        /// <param name="facadeArea">立面面积</param>
        /// <param name="groundArea">地面面积</param>
        /// <param name="ceilingArea">棚面面积</param>
        public void UpdateSpaceDetailInfo(NumericalStandard numericalStandard, decimal groundLength, decimal groundWidth, decimal spacePerimeter, decimal wallHigh, decimal holeArea, decimal facadeArea, decimal groundArea, decimal ceilingArea)
        {
            NumericalStandard = numericalStandard;
            GroundLength = groundLength;
            GroundWidth = groundWidth;
            SpacePerimeter = spacePerimeter;
            WallHigh = wallHigh;
            HoleArea = holeArea;
            FacadeArea = facadeArea;
            GroundArea = groundArea;
            CeilingArea = ceilingArea;
        } 
        #endregion



        #endregion
    }
}
