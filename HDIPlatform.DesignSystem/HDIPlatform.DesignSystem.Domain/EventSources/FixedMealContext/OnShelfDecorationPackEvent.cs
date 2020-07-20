using System;
using ShSoft.Infrastructure.EventBase;

namespace HDIPlatform.DesignSystem.Domain.EventSources.FixedMealContext
{
    /// <summary>
    /// 固定套餐上架事件——克隆套餐副本
    /// </summary>
    public class OnShelfDecorationPackEvent : Event
    {
        #region # 构造器

        #region 01.无参构造器
        /// <summary>
        /// 无参构造器
        /// </summary>
        protected OnShelfDecorationPackEvent() { }
        #endregion

        #region 02.基础构造器
        /// <summary>
        /// 基础构造器
        /// </summary>
        /// <param name="packId">套餐Id</param>
        public OnShelfDecorationPackEvent(Guid packId)
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
