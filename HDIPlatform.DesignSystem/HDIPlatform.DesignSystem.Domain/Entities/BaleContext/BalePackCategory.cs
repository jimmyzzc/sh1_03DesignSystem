using ShSoft.Common.PoweredByLee;
using ShSoft.Infrastructure.EntityBase;
using System;
using System.Collections.Generic;
using System.Linq;
using HDIPlatform.DesignSystem.Domain.EventSources.BaleContext;
using ShSoft.Infrastructure.EventBase.Mediator;

namespace HDIPlatform.DesignSystem.Domain.Entities.BaleContext
{
    /// <summary>
    /// 套餐组内品类
    /// </summary>
    [Serializable]
    public class BalePackCategory : PlainEntity
    {
        #region # 构造器

        #region 01.无参构造器

        /// <summary>
        /// 无参构造器
        /// </summary>
        protected BalePackCategory()
        {
            this.BalePackProducts = new HashSet<BalePackProduct>();
        }
        #endregion


        #region 02.创建套餐组内品类构造器

        /// <summary>
        /// 创建套餐组内品类构造器
        /// </summary>

        /// <param name="categoryId">品类Id</param>
        public BalePackCategory(Guid categoryId)
            : this()
        {
            this.CategoryId = categoryId;
        }
        #endregion

        #endregion

        #region # 属性

        #region 三级品类Id —— Guid CategoryId
        /// <summary>
        /// 三级品类Id
        /// </summary>
        public Guid CategoryId { get; private set; }

        #endregion


        #region 导航属性 - 品类配置商品集 —— ICollection<BalePackProduct> BalePackProducts
        /// <summary>
        /// 导航属性 - 品类配置商品集
        /// </summary>
        public virtual ICollection<BalePackProduct> BalePackProducts { get; private set; }
        #endregion

        #region 导航属性 - 套餐选区内组 —— BalePackGroup BalePackGroup
        /// <summary>
        /// 导航属性 - 套餐选区内组
        /// </summary>
        public virtual BalePackGroup BalePackGroup { get; private set; }
        #endregion

        #endregion

        #region # 方法

        #region 新增品类下商品集 —— void AddBalePackProduct(IEnumerable<BalePackProduct> products)
        /// <summary>
        /// 新增品类下商品集
        /// </summary>
        /// <param name="products">商品集</param>
        public void AddBalePackProduct(IEnumerable<BalePackProduct> products)
        {
            #region # 验证

            products = products == null ? new BalePackProduct[0] : products.ToArray();

            if (!products.Any())
            {
                throw new ArgumentNullException("products", "套餐品类商品集不可为空！");
            }
            if (products.Count() != products.DistinctBy(x => x.ProductId).Count() || this.BalePackProducts.Select(s => s.ProductId).Intersect(products.Select(s => s.ProductId)).Any())
            {
                throw new InvalidOperationException("同一品类内，商品不可重复！");
            }
            #endregion
            this.BalePackProducts.AddRange(products);
        }
        #endregion

        #region 删除品类下已配置商品集 —— void DeleteBalePackProduct(IEnumerable<BalePackProduct> products)

        /// <summary>
        /// 删除品类下已配置商品集
        /// </summary>
        /// <param name="products"></param>
        public void DeleteBalePackProduct(IEnumerable<BalePackProduct> products)
        {
            foreach (BalePackProduct product in products.ToList())
            {
                this.BalePackProducts.Remove(product);
            }

            //挂起领域事件(类似上架商品)——修改套餐是否存在下架商品
            EventMediator.Suspend(new BalePackProductShelvedEvent(this.BalePackGroup.ChoiceAreaId, true));
        }
        #endregion

        #region 删除品类下已配置商品集 —— void RemoveBalePackProduct(IEnumerable<Guid> productIds)

        /// <summary>
        /// 删除品类下已配置商品集
        /// </summary>
        /// <param name="productIds">商品Id集</param>
        public void RemoveBalePackProduct(IEnumerable<Guid> productIds)
        {
            //验证
            Assert.IsNotNull(productIds, "品类内商品集不可为null！");
            IEnumerable<BalePackProduct> products = this.GetProductsInCategory(productIds).ToList();
            products.ForEach(product =>
            {
                this.BalePackProducts.Remove(product);
            });
        }
        #endregion

        #region 获取品类下商品 —— IEnumerable<BalePackProduct> GetProductsInCategory(IEnumerable<Guid> productIds)

        /// <summary>
        /// 获取品类下商品
        /// </summary>
        /// <param name="productIds">商品Id</param>
        /// <returns></returns>
        public IEnumerable<BalePackProduct> GetProductsInCategory(IEnumerable<Guid> productIds)
        {
            IEnumerable<BalePackProduct> products = this.BalePackProducts.Where(s => productIds.Contains(s.ProductId));

            return products;

        }
        #endregion

        #region 是否存在商品 —— bool ExistsProduct(Guid productId)

        /// <summary>
        /// 是否存在商品
        /// </summary>
        /// <param name="productId">商品Id</param>
        /// <returns></returns>
        public bool ExistsProduct(Guid productId)
        {
            return this.BalePackProducts.Any(s => s.ProductId == productId);
        }
        #endregion

        #region 是否存在商品 —— bool ExistsProducts(IEnumerable<Guid> productIds)

        /// <summary>
        /// 是否存在商品
        /// </summary>
        /// <param name="productIds">商品Id集</param>
        /// <returns></returns>
        public bool ExistsProducts(IEnumerable<Guid> productIds)
        {
            return this.BalePackProducts.Select(s => s.ProductId).Intersect(productIds).Any();
        }
        #endregion

        #region 获取指定商品 —— BalePackProduct GetBalePackProductById(Guid productId)

        /// <summary>
        /// 获取指定商品
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public BalePackProduct GetBalePackProductById(Guid productId)
        {
            BalePackProduct product = this.BalePackProducts.SingleOrDefault(s => s.ProductId == productId);

            #region # 验证
            if (product == null)
            {
                throw new ArgumentOutOfRangeException("productId", string.Format("套餐品类下不存在Id为\"{0}\"的商品！", productId));
            }

            #endregion

            return product;
        }
        #endregion

        #region 替换下架商品 —— void ReplaceProduct(Guid shelfOffProductId, Guid productId)

        /// <summary>
        /// 替换下架商品
        /// </summary>
        /// <param name="shelfOffProductId">下架商品Id</param>
        /// <param name="productId">商品Id</param>
        public void ReplaceProduct(Guid shelfOffProductId, Guid productId)
        {

            BalePackProduct product = this.GetBalePackProductById(shelfOffProductId);
            product.ReplaceProduct(productId);

        }
        #endregion


        #region 下架商品 —— void ProductShelfOff(Guid productId)

        /// <summary>
        /// 下架商品
        /// </summary>
        /// <param name="productId">商品Id</param>
        public void ProductShelfOff(Guid productId)
        {
            BalePackProduct product = this.GetBalePackProductById(productId);
            product.ProductShelfOff();
        }
        #endregion

        #region 重新上架商品 —— void ProductReShelfed(Guid productId)

        /// <summary>
        /// 重新上架商品
        /// </summary>
        /// <param name="productId">商品Id</param>
        public void ProductReShelfed(Guid productId)
        {

            BalePackProduct product = this.GetBalePackProductById(productId);
            product.ProductReShelfed();
        }
        #endregion


        #region 删除品类下已配置的所有商品集 —— void DeleteBalePackAllProduct()

        /// <summary>
        /// 删除品类下已配置的所有商品集
        /// </summary>
        public void DeleteBalePackAllProduct()
        {
            foreach (BalePackProduct product in this.BalePackProducts.ToList())
            {
                this.BalePackProducts.Remove(product);
            }

            //挂起领域事件(类似上架商品)——修改套餐是否存在下架商品
            EventMediator.Suspend(new BalePackProductShelvedEvent(this.BalePackGroup.ChoiceAreaId, true));
        }
        #endregion
        #endregion
    }
}
