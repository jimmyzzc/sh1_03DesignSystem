using HDIPlatform.DesignSystem.Domain.EventSources.FixedMealContext;
using ShSoft.Infrastructure.EntityBase;
using ShSoft.Infrastructure.EventBase.Mediator;
using System;
using System.Collections.Generic;
using System.Linq;
using ShSoft.Common.PoweredByLee;
using ShSoft.ValueObjects.Enums;

namespace HDIPlatform.DesignSystem.Domain.Entities.FixedMealContext
{
    /// <summary>
    /// 套餐模板商品SKU项
    /// </summary>
    [Serializable]
    public class DecorationPackSku : PlainEntity
    {
        #region # 构造器

        /// <summary>
        /// 同步锁
        /// </summary>
        private static readonly object _Sync = new object();

        #region 01.无参构造器
        /// <summary>
        /// 无参构造器
        /// </summary>
        protected DecorationPackSku()
        {
            //默认值
            //this.Default = false;
            //this.Shelved = true;
        }
        #endregion

        #region 02.创建套餐模板商品SKU项构造器

        /// <summary>
        /// 创建套餐模板商品SKU项构造器
        /// </summary>
        /// <param name="skuId">商品SKU Id</param>
        /// <param name="sort">排序</param>
        /// <param name="shelved">是否已上架</param>
        /// <param name="categoryId">三级品类Id</param>
        /// <param name="brandId">品牌Id</param>
        /// <param name="isDefault">是否是默认商品</param>
        /// <param name="skuQuantity">默认商品工程量(手动填写)</param>
        /// <param name="costPrice">商品工程量成本单价</param>
        /// <param name="changed">是否变价</param>
        /// <param name="skuCraftPositions">放置位置</param>
        public DecorationPackSku(Guid skuId, int sort, bool shelved, Guid categoryId, Guid brandId, bool isDefault, decimal skuQuantity, decimal costPrice, bool changed, Dictionary<SkuCraftPosition, decimal> skuCraftPositions)
            : this()
        {
            this.SkuId = skuId;
            base.Sort = sort;
            this.Shelved = shelved;
            this.CategoryId = categoryId;
            this.BrandId = brandId;
            this.Default = isDefault;
            this.CostPrice = costPrice;
            this.Changed = changed;
            if (isDefault)
                this.SetDefaultSkuPositions(skuQuantity, skuCraftPositions);

            //挂起领域事件
            //EventMediator.Suspend(new PackSkuShelvedEvent(this.PackItem.PackId, this.Shelved));
        }
        #endregion

        #endregion

        #region # 属性

        #region 商品SKU Id —— Guid SkuId
        /// <summary>
        /// 商品SKU Id
        /// </summary>
        public Guid SkuId { get; private set; }
        #endregion

        #region 是否默认 —— bool Default
        /// <summary>
        /// 是否默认
        /// </summary>
        public bool Default { get; private set; }
        #endregion

        #region 是否已上架 —— bool Shelved
        /// <summary>
        /// 是否已上架
        /// </summary>
        public bool Shelved { get; private set; }
        #endregion

        #region 三级品类Id —— Guid CategoryId
        /// <summary>
        /// 三级品类Id
        /// </summary>
        public Guid CategoryId { get; private set; }

        #endregion

        #region 品牌Id —— Guid BrandId
        /// <summary>
        /// 品牌Id
        /// </summary>
        public Guid BrandId { get; private set; }
        #endregion

        #region  默认商品SKU数量 ——  decimal SkuQuantity
        /// <summary>
        ///   默认商品SKU数量(手动填写)
        /// </summary>
        public decimal SkuQuantity { get; private set; }
        #endregion

        #region 是否已变价 —— bool Changed
        /// <summary>
        /// 是否已变价
        /// </summary>
        public bool Changed { get; private set; }
        #endregion

        #region 商品工程量成本价 ——  decimal CostPrice
        /// <summary>
        ///   商品工程量成本价
        /// </summary>
        public decimal CostPrice { get; private set; }
        #endregion


        #region 只读属性 - 默认商品SKU数量 ——  decimal DefaultSkuQuantity
        /// <summary>
        /// 只读属性 - 默认商品SKU数量
        /// </summary>
        public decimal DefaultSkuQuantity
        {
            get { return this.SkuCraftPositions.Values.Sum(); }
        }
        #endregion

        #region 只读属性 - 放置位置集 —— IDictionary<SkuCraftPosition, decimal> SkuCraftPositions
        /// <summary>
        /// 只读属性 - 放置位置集
        /// </summary>
        public IDictionary<SkuCraftPosition, decimal> SkuCraftPositions
        {
            get
            {
                return string.IsNullOrWhiteSpace(this.SkuCraftPositionsStr)
                    ? new Dictionary<SkuCraftPosition, decimal>()
                    : this.SkuCraftPositionsStr.JsonToObject<Dictionary<SkuCraftPosition, decimal>>();
            }
        }
        #endregion

        #region 内部属性 - 放置位置集序列化字符串 —— string SkuCraftPositionsStr
        /// <summary>
        /// 内部属性 - 放置位置集序列化字符串
        /// </summary>
        public string SkuCraftPositionsStr { get; private set; }
        #endregion

        #region 导航属性 - 套餐模板项 —— DecorationPackItem PackItem
        /// <summary>
        /// 导航属性 - 套餐模板项
        /// </summary>
        public virtual DecorationPackItem PackItem { get; internal set; }
        #endregion

        #endregion

        #region # 方法

        #region 设置默认 —— void SetDefault()
        /// <summary>
        /// 设置默认
        /// </summary>
        public void SetDefault()
        {
            lock (_Sync)
            {
                //将所有都初始化为非默认
                foreach (DecorationPackSku packSku in this.PackItem.PackSkus)
                {
                    packSku.Default = false;
                }

                this.Default = true;
            }
        }
        #endregion

        #region 重新上架 —— void ReOnShelf()
        /// <summary>
        /// 重新上架
        /// </summary>
        public void ReOnShelf()
        {
            if (this.Shelved) return;

            this.Shelved = true;

            //挂起领域事件
            EventMediator.Suspend(new PackSkuShelvedEvent(this.PackItem.PackId, this.Shelved));
        }
        #endregion

        #region 下架 —— void OffShelf()
        /// <summary>
        /// 下架
        /// </summary>
        public void OffShelf()
        {
            if (!this.Shelved) return;

            this.Shelved = false;

            //挂起领域事件
            EventMediator.Suspend(new PackSkuShelvedEvent(this.PackItem.PackId, this.Shelved));
        }
        #endregion

        #region 替换商品SKU —— void ReplaceSku(Guid skuId)

        /// <summary>
        /// 替换商品SKU
        /// </summary>
        /// <param name="skuId">商品SKU Id</param>
        /// <param name="costPrice">新商品成本价</param>
        public void ReplaceSku(Guid skuId, decimal costPrice)
        {
            #region # 验证

            if (this.SkuId == skuId)
            {
                return;
            }
            if (this.PackItem.ExistsSku(skuId))
            {
                throw new InvalidOperationException(string.Format("{0} 套餐模板项中已存在该商品SKU！", this.PackItem.Name));
            }

            #endregion

            this.SkuId = skuId;
            this.SetCostPrice(costPrice);
            //ReOnShelf();
            this.Shelved = true;
        }
        #endregion

        #region 设置默认商品工程量 —— void SetDefaultSkuPositions(decimal skuQuantity, Dictionary<SkuCraftPosition, decimal> skuCraftPositions)
        /// <summary>
        /// 设置默认商品工程量
        /// </summary>
        /// <param name="skuQuantity">默认商品SKU数量(手动填写)</param>
        /// <param name="skuCraftPositions">放置位置集</param>
        public void SetDefaultSkuPositions(decimal skuQuantity, Dictionary<SkuCraftPosition, decimal> skuCraftPositions)
        {
            this.SkuQuantity = skuQuantity;

            #region # 验证
            skuCraftPositions = skuCraftPositions ?? new Dictionary<SkuCraftPosition, decimal>();
            #endregion

            this.SkuCraftPositionsStr = skuCraftPositions.ToJson();
        }
        #endregion

        #region  变价处理 —— void CostPriceChanged(decimal newCostPrice)
        /// <summary>
        /// 变价处理
        /// </summary>
        /// <param name="newCostPrice">最新工程量成本价</param>
        public void CostPriceChanged(decimal newCostPrice)
        {
            //新价格！=原价格
            if (decimal.Compare(newCostPrice, this.CostPrice) != 0m)
            {
                if (this.Changed) return;
                this.Changed = true;
            }
            else
            {
                //现价格=原价格
                if (!this.Changed) return;
                this.Changed = false;
            }
            //挂起领域事件（批量处理 在外面）
            //EventMediator.Suspend(new PackSkuChangedEvent(this.PackItem.PackId, this.Changed));
        }

        #endregion

        #region 成本价更新为最新 —— void SetCostPrice(decimal newCostPrice)
        /// <summary>
        /// 套餐定价|替换商品 成本价更新为最新
        /// </summary>
        /// <param name="newCostPrice">成本价</param>
        public void SetCostPrice(decimal newCostPrice)
        {
            this.CostPrice = newCostPrice;
            this.Changed = false;
        }
        #endregion

        //#region # 换新商品取消变价

        ///// <summary>
        ///// 换新商品取消变价
        ///// </summary>
        ///// <param name="costPrice">成本价</param>
        //public void ReChanged(decimal costPrice)
        //{
        //    this.SetCostPrice(costPrice);
        //    this.Changed = false;
        //}
        //#endregion

        #endregion
    }
}
