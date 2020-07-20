using System;
using ShSoft.Infrastructure.EventBase;

namespace HDIPlatform.DesignSystem.Domain.EventSources.BaleContext
{
    /// <summary>
    /// 大包定制套餐上架事件——克隆套餐副本
    /// </summary>
    public class OnShelfBalePackEvent : Event
    {
        #region # 构造器

        #region 01.无参构造器
        /// <summary>
        /// 无参构造器
        /// </summary>
        protected OnShelfBalePackEvent() { }
        #endregion

        #region 02.基础构造器
        /// <summary>
        /// 基础构造器
        /// </summary>
        /// <param name="packId">套餐Id</param>
        public OnShelfBalePackEvent(Guid packId)
            : this()
        {
            this.PackId = packId;
        }
        #endregion

        #endregion

        #region # 属性

        #region 套餐Id —— Guid PackId
        /// <summary>
        /// 套餐Id
        /// </summary>
        public Guid PackId { get; set; }
        #endregion

        #endregion
    }
}
