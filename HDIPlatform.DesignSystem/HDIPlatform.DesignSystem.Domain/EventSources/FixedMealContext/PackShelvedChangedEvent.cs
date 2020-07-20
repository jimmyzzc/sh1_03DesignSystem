using ShSoft.Infrastructure.EventBase;
using System;

namespace HDIPlatform.DesignSystem.Domain.EventSources.FixedMealContext
{
    /// <summary>
    /// 套餐是否包含下架|变价商品|工艺事件
    /// </summary>
    public class PackShelvedChangedEvent : Event
    {
        #region # 构造器

        #region 01.无参构造器
        /// <summary>
        /// 无参构造器
        /// </summary>
        protected PackShelvedChangedEvent() { }
        #endregion

        #region 02.基础构造器
        /// <summary>
        /// 基础构造器
        /// </summary>
        /// <param name="packId">套餐模板Id</param>
        public PackShelvedChangedEvent(Guid packId)
            : this()
        {
            this.PackId = packId;
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

        #endregion
    }
}
