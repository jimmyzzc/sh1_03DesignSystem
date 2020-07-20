using ShSoft.Infrastructure.EntityBase;
using System;

namespace HDIPlatform.DesignSystem.Domain.Entities.FixedMealContext
{
    /// <summary>
    /// 套餐模板 - 楼盘
    /// </summary>
    [Serializable]
    public class DecorationPackProperty : AggregateRootEntity
    {
        #region # 构造器

        #region 01.无参构造器
        /// <summary>
        /// 无参构造器
        /// </summary>
        protected DecorationPackProperty() { }
        #endregion

        #region 02.创建套餐模板 - 楼盘构造器
        /// <summary>
        /// 创建套餐模板 - 楼盘构造器
        /// </summary>
        /// <param name="packId">套餐模板Id</param>
        /// <param name="propertyId">楼盘Id</param>
        public DecorationPackProperty(Guid packId, Guid propertyId)
            : this()
        {
            this.PackId = packId;
            this.PropertyId = propertyId;
        }
        #endregion

        #endregion

        #region # 属性

        #region 套餐模板Id —— Guid PackId
        /// <summary>
        /// 套餐模板Id
        /// </summary>
        public Guid PackId { get; private set; }
        #endregion

        #region 楼盘Id —— Guid PropertyId
        /// <summary>
        /// 楼盘Id
        /// </summary>
        public Guid PropertyId { get; private set; }
        #endregion

        #endregion

        #region # 方法

        #region 设置套餐模板Id —— void SetPackId(Guid packId)
        /// <summary>
        /// 设置套餐模板Id
        /// </summary>
        /// <param name="packId">套餐模板Id</param>
        /// <remarks>注意：克隆套餐模板时候用，勿乱用</remarks>
        public void SetPackId(Guid packId)
        {
            this.PackId = packId;
        }
        #endregion


        #endregion
    }
}
