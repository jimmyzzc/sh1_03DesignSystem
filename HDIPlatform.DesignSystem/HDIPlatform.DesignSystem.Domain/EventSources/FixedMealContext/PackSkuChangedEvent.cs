using ShSoft.Infrastructure.EventBase;
using System;

namespace HDIPlatform.DesignSystem.Domain.EventSources.FixedMealContext
{
    /// <summary>
    /// 套餐模板商品SKU|工艺项变价事件
    /// </summary>
    public class PackSkuChangedEvent : Event
    {
        #region # 构造器

        #region 01.无参构造器
        /// <summary>
        /// 无参构造器
        /// </summary>
        protected PackSkuChangedEvent() { }
        #endregion

        #region 02.基础构造器
        /// <summary>
        /// 基础构造器
        /// </summary>
        /// <param name="packId">套餐模板Id</param>
        /// <param name="changed">变价|未变价</param>
        public PackSkuChangedEvent(Guid packId, bool changed)
            : this()
        {
            this.PackId = packId;
            this.Changed = changed;
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

       #region 是否已变价 —— bool Changed
        /// <summary>
        /// 是否已变价
        /// </summary>
        public bool Changed { get;  set; }
        #endregion

        #endregion
    }
}
