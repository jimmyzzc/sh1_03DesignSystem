using ShSoft.Infrastructure.EntityBase;
using ShSoft.ValueObjects.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using ShSoft.Common.PoweredByLee;

namespace HDIPlatform.DesignSystem.Domain.Entities.FixedMealContext
{
    /// <summary>
    /// 套餐模板空间
    /// </summary>
    [Serializable]
    public class DecorationPackSpace : PlainEntity
    {
        #region # 构造器

        #region 01.无参构造器

        /// <summary>
        /// 无参构造器
        /// </summary>
        protected DecorationPackSpace()
        {
            this.SpaceDetails = new HashSet<DecorationPackSpaceDetail>();
        }
        #endregion

        #region 02.创建套餐模板空间构造器

        /// <summary>
        /// 创建套餐模板空间构造器
        /// </summary>
        /// <param name="spaceName">空间名称</param>
        /// <param name="square">面积</param>
        /// <param name="spaceType">类型</param>
        /// <param name="spaceDetails">套餐模板空间详情集</param>
        /// <param name="sort">排序</param>
        public DecorationPackSpace(string spaceName, float square, SpaceType spaceType, IEnumerable<DecorationPackSpaceDetail> spaceDetails, int sort)
            : this()
        {
            base.Name = spaceName;
            this.Square = square;
            this.SpaceType = spaceType;
            base.Sort = sort;
            this.SpaceDetails.AddRange(spaceDetails);
        }
        #endregion

        #endregion

        #region # 属性

        #region 空间面积 —— float Square
        /// <summary>
        /// 空间面积
        /// </summary>
        public float Square { get; private set; }
        #endregion

        #region 空间类型 —— SpaceType SpaceType
        /// <summary>
        /// 空间类型
        /// </summary>
        public SpaceType SpaceType { get; private set; }
        #endregion

        #region 标准面积 —— float CriterionSquare
        /// <summary>
        /// 标准面积（公式）
        /// </summary>
        public float CriterionSquare { get; private set; }
        #endregion

        #region 空间占比 —— decimal SpaceRatio
        /// <summary>
        /// 空间占比（标准空间面积/标准整体面积|5/60）
        /// </summary>
        public decimal SpaceRatio { get; private set; }
        #endregion

        #region 单价 —— decimal UnitPrice
        /// <summary>
        /// 超出面积单价
        /// </summary>
        public decimal UnitPrice { get; private set; }
        #endregion

        #region 套餐空间定价类型 ——  DecorationPackSpacePricingType DecorationPackSpacePricingType
        /// <summary>
        /// 套餐空间定价类型
        /// </summary>
        public DecorationPackSpacePricingType? DecorationPackSpacePricingType { get; private set; }
        #endregion

        #region 导航属性 - 套餐模板 —— DecorationPack DecorationPack
        /// <summary>
        /// 导航属性 - 套餐模板
        /// </summary>
        public virtual DecorationPack DecorationPack { get; private set; }
        #endregion

        #region 导航属性 - 套餐模板空间详情集 —— ICollection<DecorationPackSpaceDetail> SpaceDetails
        /// <summary>
        /// 导航属性 - 套餐模板空间详情集
        /// </summary>
        public virtual ICollection<DecorationPackSpaceDetail> SpaceDetails { get; private set; }
        #endregion


        #endregion

        #region # 方法

        #region 修改套餐模板空间信息 —— void UpdateInfo(string spaceName, float square)

        /// <summary>
        /// 修改套餐模板空间信息
        /// </summary>
        /// <param name="spaceName">空间名称</param>
        /// <param name="square">空间面积</param>
        /// <param name="sort">排序</param>
        public void UpdateInfo(string spaceName, float square, int sort)
        {
            base.Name = spaceName;
            this.Square = square;
            base.Sort = sort;

            //设置套餐模板总面积
            //this.DecorationPack.Square = this.DecorationPack.Spaces.Any() ? this.DecorationPack.Spaces.Sum(x => x.Square) : 0;
        }
        #endregion

        #region 空间定价 —— void SetPrice(DecorationPackSpacePricingType spacePricingType, float criterionSquare, decimal spaceRatio, decimal unitPrice)
        /// <summary>
        /// 空间定价
        /// </summary>
        /// <param name="spacePricingType">套餐空间定价类型</param>
        /// <param name="criterionSquare">标准面积</param>
        /// <param name="spaceRatio">空间占比</param>
        /// <param name="unitPrice">超出面积单价</param>
        public void SetPrice(DecorationPackSpacePricingType spacePricingType, float criterionSquare, decimal spaceRatio, decimal unitPrice)
        {

            Assert.IsTrue(unitPrice >= 0, "超出面积单价不可小于0！");
            this.UnitPrice = unitPrice;
            this.DecorationPackSpacePricingType = spacePricingType;
            if (spacePricingType == ShSoft.ValueObjects.Enums.DecorationPackSpacePricingType.FormulaFirst)
            {
                //验证
                Assert.IsTrue(criterionSquare >= 0, "标准面积不可小于0！");
                this.CriterionSquare = criterionSquare;
            }
            else
            {
                //验证
                Assert.IsTrue(spaceRatio >= 0, "空间面积占比不可小于0！");
                this.SpaceRatio = spaceRatio;
            }

        }
        #endregion

        #region 修改套餐模板空间详情 —— void UpdateSpaceDetail(Guid? spaceDetailId, NumericalStandard numericalStandard...
        /// <summary>
        /// 修改套餐模板空间详情
        /// </summary>
        /// <param name="spaceDetailId">套餐模板空间详情Id</param>
        /// <param name="numericalStandard">数值标准</param>
        /// <param name="groundLength">地面长度</param>
        /// <param name="groundWidth">地面宽度</param>
        /// <param name="spacePerimeter">空间周长</param>
        /// <param name="wallHigh">墙面高度</param>
        /// <param name="holeArea">洞口面积</param>
        /// <param name="facadeArea">立面面积</param>
        /// <param name="groundArea">地面面积</param>
        /// <param name="ceilingArea">棚面面积</param>
        public void UpdateSpaceDetail(Guid? spaceDetailId, NumericalStandard numericalStandard, decimal groundLength, decimal groundWidth, decimal spacePerimeter, decimal wallHigh, decimal holeArea, decimal facadeArea, decimal groundArea, decimal ceilingArea)
        {
            if (spaceDetailId.HasValue)
            {
                DecorationPackSpaceDetail spaceDetail = this.SpaceDetails.SingleOrDefault(x => x.Id == spaceDetailId);
                Assert.IsFalse(spaceDetail == null, "套餐模板空间详情不存在！");
                spaceDetail.UpdateSpaceDetailInfo(numericalStandard, groundLength, groundWidth, spacePerimeter, wallHigh, holeArea, facadeArea, groundArea,
                    ceilingArea);
            }
            else
            {
                DecorationPackSpaceDetail spaceDetail = new DecorationPackSpaceDetail(numericalStandard, groundLength, groundWidth, spacePerimeter, wallHigh, holeArea, facadeArea, groundArea,
                    ceilingArea);
                this.AddSpaceDetail(spaceDetail);
            }
        }
        #endregion


        #region 添加套餐模板空间详情 —— void AddSpaceDetail(DecorationPackSpaceDetail spaceDetail)
        /// <summary>
        /// 添加套餐模板空间详情
        /// </summary>
        /// <param name="spaceDetail"></param>
        public void AddSpaceDetail(DecorationPackSpaceDetail spaceDetail)
        {
            Assert.IsFalse(this.SpaceDetails.Any(x => x.NumericalStandard == spaceDetail.NumericalStandard), "已存在该数值标准的套餐模板空间详情，请重新操作！");
            this.SpaceDetails.Add(spaceDetail);
        }
        #endregion

        #region 清空空间超出定价 —— void ClearSpacePrice()

        /// <summary>
        /// 清空空间超出定价
        /// </summary>
        public void ClearSpacePrice()
        {
            this.DecorationPackSpacePricingType = null;
            this.UnitPrice = 0m;
            this.CriterionSquare = 0f;
            this.SpaceRatio = 0m;
        }

        #endregion

        #endregion
    }
}
