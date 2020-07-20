using System;
using System.Collections.Generic;
using ShSoft.Common.PoweredByLee;
using ShSoft.Infrastructure.EntityBase;

namespace HDIPlatform.DesignSystem.Domain.Entities.DimPackContext
{
    /// <summary>
    /// DIM户型套餐树（DCU）
    /// </summary>
    [Serializable]
    public class DimPackTreeDcu : DimPackTree
    {
        #region # 构造器

        #region 01.无参构造器
        /// <summary>
        /// 无参构造器
        /// </summary>
        protected DimPackTreeDcu() { }
        #endregion


        #endregion

        #region # 属性

        #region DCU中SKU是否可甲供 —— bool IsSkuOwnerSupply
        /// <summary>
        /// DCU中SKU是否可甲供
        /// </summary>
        public bool IsSkuOwnerSupply { get; private set; }
        #endregion

        #region DCU中SKU是否可外选 —— bool IsSkuExternal
        /// <summary>
        /// DCU中SKU是否可外选
        /// </summary>
        public bool IsSkuExternal { get; private set; }
        #endregion

        #region DCU中工艺是否可甲供 —— bool IsCraftOwnerSupply
        /// <summary>
        /// DCU中工艺是否可甲供
        /// </summary>
        public bool IsCraftOwnerSupply { get; private set; }
        #endregion

        #region DCU中工艺是否可外选 —— bool IsCraftExternal
        /// <summary>
        /// DCU中工艺是否可外选
        /// </summary>
        public bool IsCraftExternal { get; private set; }
        #endregion

        #region 只读—内选商品Id集 —— List<Guid> SkuIds
        /// <summary>
        /// 内选商品Id集
        /// </summary>
        public List<Guid> SkuIds
        {
            get
            {

                return !string.IsNullOrEmpty(this.SkuIdsStr)
                                ? this.SkuIdsStr.JsonToObject<List<Guid>>()
                                : new List<Guid>();
            }
        }
        #endregion

        #region  内选商品Id集合字符串 —— string SkuIdsStr

        /// <summary>
        /// 内选商品Id集合字符串
        /// </summary>
        public string SkuIdsStr { get; private set; }

        #endregion

        #region  内选工艺Id集合字符串 —— string CraftIdsStr

        /// <summary>
        /// 内选工艺Id集合字符串
        /// </summary>
        public string CraftIdsStr { get; private set; }

        #endregion

        #region 只读—内选工艺Id集 —— List<Guid> CraftIds
        /// <summary>
        /// 内选工艺Id集
        /// </summary>
        public List<Guid> CraftIds
        {
            get
            {

                return !string.IsNullOrEmpty(this.CraftIdsStr)
                                ? this.CraftIdsStr.JsonToObject<List<Guid>>()
                                : new List<Guid>();
            }
        }
        #endregion

        #region DCU 单位 —— string DcuUnit
        /// <summary>
        /// DCU 单位
        /// </summary>
        public string DcuUnit { get; private set; }
        #endregion

        #region DCU 工程量（商品|工艺一致） —— decimal DcuQuantity
        /// <summary>
        /// DCU 工程量（商品|工艺一致）
        /// </summary>
        public decimal DcuQuantity { get; private set; }
        #endregion

        #region DCU Id(分组) —— Guid? DcuId
        /// <summary>
        /// DCU Id(分组)
        /// </summary>
        public Guid? DcuId { get; private set; }
        #endregion

        #region DCU名称 (分组)—— string DcuName
        /// <summary>
        /// DCU名称(分组)
        /// </summary>
        public string DcuName { get; private set; }
        #endregion

        #region DCU已选商品Id —— Guid? DcuSkuId
        public Guid? DcuSkuId { get; private set; }
        #endregion

        #region DCU已选工艺Id —— Guid? DcuCraftId
        public Guid? DcuCraftId { get; private set; }
        #endregion

        #region DCU已选商品内选 —— bool SkuInternal
        public bool SkuInternal { get; private set; }
        #endregion

        #region DCU已选工艺内选 —— bool CraftInternal
        public bool CraftInternal { get; private set; }
        #endregion

        #region DCU品类集

        #endregion

        #endregion

        #region # 方法




        #endregion
    }
}
