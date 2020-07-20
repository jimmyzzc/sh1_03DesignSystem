using ShSoft.Infrastructure.EventBase;
using System;

namespace HDIPlatform.DesignSystem.Domain.EventSources.FixedMealContext
{
    /// <summary>
    /// 套餐模板空间已删除事件
    /// </summary>
    public class PackSpaceDeletedEvent : Event
    {
        #region # 构造器

        #region 01.无参构造器
        /// <summary>
        /// 无参构造器
        /// </summary>
        protected PackSpaceDeletedEvent() { }
        #endregion

        #region 02.基础构造器
        /// <summary>
        /// 基础构造器
        /// </summary>
        public PackSpaceDeletedEvent(Guid packId, Guid packSpaceId)
            : this()
        {
            this.PackId = packId;
            this.PackSpaceId = packSpaceId;
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

        #region 套餐模板空间Id —— Guid PackSpaceId
        /// <summary>
        /// 套餐模板空间Id
        /// </summary>
        public Guid PackSpaceId { get; set; }
        #endregion

        #endregion
    }
}
