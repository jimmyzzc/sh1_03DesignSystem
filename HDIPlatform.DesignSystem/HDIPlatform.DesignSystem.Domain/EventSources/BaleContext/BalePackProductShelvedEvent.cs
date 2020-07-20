using System;
using ShSoft.Infrastructure.EventBase;

namespace HDIPlatform.DesignSystem.Domain.EventSources.BaleContext
{
    /// <summary>
    /// 套餐商品上/下架事件
    /// </summary>
    public class BalePackProductShelvedEvent : Event
    {
        #region # 构造器

        #region 01.无参构造器
        /// <summary>
        /// 无参构造器
        /// </summary>
        protected BalePackProductShelvedEvent() { }
        #endregion

        #region 02.基础构造器

        /// <summary>
        /// 基础构造器
        /// </summary>
        /// <param name="choiceAreaId">选区Id</param>
        /// <param name="shelved">是否上架</param>
        public BalePackProductShelvedEvent(Guid choiceAreaId, bool shelved)
        {
            this.ChoiceAreaId = choiceAreaId;
            this.Shelved = shelved;
        }

        #endregion

        #endregion

        #region # 属性

        #region 商品是否已上架 —— bool Shelved
        /// <summary>
        /// 商品是否已上架
        /// </summary>
        public bool Shelved { get; set; }
        #endregion


        #region 商品所在选区Id —— Guid ChoiceAreaId

        /// <summary>
        /// 商品所在选区Id
        /// </summary>
        public Guid ChoiceAreaId { get; set; }
        #endregion

        #endregion
    }
}
