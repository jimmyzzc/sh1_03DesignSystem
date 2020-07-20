using System;
using ShSoft.Infrastructure.EntityBase;
using ShSoft.ValueObjects.Enums;

namespace HDIPlatform.DesignSystem.Domain.Entities.DimPackContext
{
    /// <summary>
    /// DIM户型套餐空间
    /// </summary>
    [Serializable]
    public class DimPackSpace : AggregateRootEntity
    {
        #region # 构造器

        #region 01.无参构造器
        /// <summary>
        /// 无参构造器
        /// </summary>
        protected DimPackSpace() { }
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

        #region DIM套餐空间Id ——   Guid PackSpaceId
        /// <summary>
        /// DIM套餐空间Id
        /// </summary>
        public Guid PackSpaceId { get; private set; }

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
