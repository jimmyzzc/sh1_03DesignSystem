using System;
using HDIPlatform.DesignSystem.Domain.EventSources.BaleContext;
using HDIPlatform.DesignSystem.Domain.EventSources.FixedMealContext;
using ShSoft.Infrastructure.EntityBase;
using ShSoft.Infrastructure.EventBase.Mediator;

namespace HDIPlatform.DesignSystem.Domain.Entities.BaleContext
{
    /// <summary>
    /// 套餐品类配置商品
    /// </summary>
    [Serializable]
    public class BalePackProduct : AggregateRootEntity
    {
        #region # 构造器

        #region 01.无参构造器

        /// <summary>
        /// 无参构造器
        /// </summary>
        protected BalePackProduct()
        {
            //默认参数 商品已上架
            this.Shelved = true;
        }
        #endregion

        #region 02.创建套餐品类配置商品构造器
        /// <summary>
        /// 创建套餐品类配置商品构造器
        /// </summary>
        /// <param name="productId">商品Id</param>
        public BalePackProduct(Guid productId)
            : this()
        {
            this.ProductId = productId;

        }
        #endregion

        #endregion

        #region # 属性

        #region 商品Id —— Guid ProductId
        /// <summary>
        /// 商品Id
        /// </summary>
        public Guid ProductId { get; private set; }

        #endregion

        #region 商品是否已上架 —— bool Shelved
        /// <summary>
        /// 商品是否已上架
        /// </summary>
        public bool Shelved { get; private set; }
        #endregion

        #region 导航属性 - 套餐组内品类 —— BalePackCategory BalePackCategory
        /// <summary>
        /// 导航属性 - 套餐组内品类
        /// </summary>
        public virtual BalePackCategory BalePackCategory { get; private set; }
        #endregion

        #endregion

        #region # 方法

        #region 商品重新上架 —— void ProductReShelfed()
        /// <summary>
        /// 商品上架
        /// </summary>
        public void ProductReShelfed()
        {
            if (this.Shelved) return;
            this.Shelved = true;
            //挂起领域事件
            EventMediator.Suspend(new BalePackProductShelvedEvent(this.BalePackCategory.BalePackGroup.ChoiceAreaId, this.Shelved));

        }
        #endregion

        #region 商品下架 —— void ProductShelfOff()
        /// <summary>
        /// 商品下架
        /// </summary>
        public void ProductShelfOff()
        {
            if (!this.Shelved) return;
            this.Shelved = false;
            //挂起领域事件
            EventMediator.Suspend(new BalePackProductShelvedEvent(this.BalePackCategory.BalePackGroup.ChoiceAreaId, this.Shelved));

        }
        #endregion

        #region 替换下架商品 —— void ReplaceProduct(Guid productId)
        /// <summary>
        /// 替换下架商品
        /// </summary>
        /// <param name="productId">商品Id</param>
        public void ReplaceProduct(Guid productId)
        {
            #region # 验证

            if (this.ProductId == productId)
            {
                return;
            }
            if (this.BalePackCategory.ExistsProduct(productId))
            {
                throw new InvalidOperationException("套餐品类中已存在该商品！");
            }
            #endregion
            this.ProductId = productId;
            ProductReShelfed();
        }
        #endregion

        #endregion
    }
}
