using ShSoft.Infrastructure.EventBase;
using System;

namespace HDIPlatform.DesignSystem.Domain.EventSources.FixedMealContext
{
    /// <summary>
    /// 套餐模板商品SKU项上/下架事件
    /// </summary>
    public class PackSkuShelvedEvent : Event
    {
        #region # 构造器

        #region 01.无参构造器
        /// <summary>
        /// 无参构造器
        /// </summary>
        protected PackSkuShelvedEvent() { }
        #endregion

        #region 02.基础构造器
        /// <summary>
        /// 基础构造器
        /// </summary>
        /// <param name="packId">套餐模板Id</param>
        /// <param name="shelved">上/下架</param>
        public PackSkuShelvedEvent(Guid packId, bool shelved)
            : this()
        {
            this.PackId = packId;
            this.Shelved = shelved;
        }
        #endregion

        #endregion

        #region # 属性

        #region 套餐模板Id —— Guid PackId
        /// <summary>
        /// 套餐模板Id
        /// </summary>
        public Guid PackId { get; set; }
        #endregion

        #region 是否已上架 —— bool Shelved
        /// <summary>
        /// 是否已上架
        /// </summary>
        public bool Shelved { get; set; }
        #endregion

        #endregion
    }
}
