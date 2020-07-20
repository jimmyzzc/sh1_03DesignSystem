using System;
using System.Collections.Generic;
using ShSoft.Infrastructure.EntityBase;

namespace HDIPlatform.DesignSystem.Domain.Entities.DimPackContext
{
    /// <summary>
    /// DIM户型套餐树（DCU|DCT）
    /// </summary>
    [Serializable]
    public class DimPackTree : AggregateRootEntity
    {
        #region # 构造器

        #region 01.无参构造器
        /// <summary>
        /// 无参构造器
        /// </summary>
        protected DimPackTree() { }
        #endregion


        #endregion

        #region # 属性

        #region DIM套餐空间Id —— Guid PackSpaceId
        /// <summary>
        /// DIM套餐空间Id
        /// </summary>
        public Guid PackSpaceId { get; private set; }
        #endregion

        #region DIM套餐空间名称 —— string PackSpaceName
        /// <summary>
        /// DIM套餐空间名称
        /// </summary>
        public string PackSpaceName { get; private set; }
        #endregion

        #region DCU|DCT 预算Id —— Guid ItemId
        /// <summary>
        /// DCU|DCT 预算Id
        /// </summary>
        public Guid ItemId { get; private set; }
        #endregion

        #region DCU|DCT 预算名称 —— string ItemName
        /// <summary>
        /// DCU|DCT 预算名称
        /// </summary>
        public string ItemName { get; private set; }
        #endregion

        #region DCU（默认true）—— bool IsDcu
        /// <summary>
        /// DCU（默认true）
        /// </summary>
        public bool IsDcu { get; private set; }
        #endregion

        #region 子节点集（DCU|DCT） —— ICollection<DimPackTree> SubNodes
        /// <summary>
        /// 子节点集（DCU|DCT）
        /// </summary>
        public virtual ICollection<DimPackTree> SubNodes { get; private set; }
        #endregion

        #region 导航属性 - DIM套餐 —— DimPack DimPack
        /// <summary>
        /// 导航属性 - DIM套餐
        /// </summary>
        public virtual DimPack DimPack { get; private set; }
        #endregion

        #endregion

        #region # 方法




        #endregion
    }
}
