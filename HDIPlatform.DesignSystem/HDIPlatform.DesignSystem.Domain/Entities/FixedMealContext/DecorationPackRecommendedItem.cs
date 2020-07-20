using ShSoft.Common.PoweredByLee;
using ShSoft.Infrastructure.EntityBase;
using ShSoft.ValueObjects.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using HDIPlatform.DesignSystem.Domain.EventSources.FixedMealContext;
using ShSoft.Infrastructure.EventBase.Mediator;

namespace HDIPlatform.DesignSystem.Domain.Entities.FixedMealContext
{
    /// <summary>
    /// 套餐模板推荐项
    /// </summary>
    [Serializable]
    public class DecorationPackRecommendedItem : AggregateRootEntity
    {
        #region # 构造器

        #region 01.无参构造器
        /// <summary>
        /// 无参构造器
        /// </summary> 
        protected DecorationPackRecommendedItem()
        { 
            //初始化导航属性
            this.PackRecommendedSkus = new HashSet<DecorationPackRecommendedSku>();
            IsClone = false;
        }
        #endregion

        #region 02.创建套餐模板项构造器

        /// <summary>
        /// 创建套餐模板项构造器
        /// </summary>
        /// <param name="itemName">模板项名称</param>
        /// <param name="packId">套餐模板Id</param>
        /// <param name="packSpaceId">套餐模板空间Id</param>
        /// <param name="remark">备注</param>
        public DecorationPackRecommendedItem(string itemName, Guid packId, Guid packSpaceId,string remark)
            : this()
        {
            base.Name = itemName;
            this.PackId = packId;
            this.PackSpaceId = packSpaceId;
            this.Remark = remark;

        }
        #endregion 
        #endregion

        #region # 属性

        #region 是否克隆 —— bool IsClone
        /// <summary>
        /// 是否克隆
        /// </summary>
        public bool IsClone { get; private set; }
        #endregion

        #region 套餐模板Id —— Guid PackId
        /// <summary>
        /// 套餐模板Id
        /// </summary>
        public Guid PackId { get; private set; }
        #endregion

        #region 套餐模板空间Id —— Guid PackSpaceId
        /// <summary>
        /// 套餐模板空间Id
        /// </summary>
        public Guid PackSpaceId { get; private set; }
        #endregion

        #region 备注 —— string Remark
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; private set; }
        #endregion

        #region 导航属性 - 套餐模板推荐商品SKU项集 ——  ICollection<DecorationPackRecommendedSku> PackRecommendedSkus
        /// <summary>
        /// 导航属性 - 套餐模板推荐商品SKU项集
        /// </summary>
        public virtual ICollection<DecorationPackRecommendedSku> PackRecommendedSkus { get; private set; }
        #endregion

        #endregion

        #region # 方法

        #region 添加商品SKU项集 —— void AddPackSkus(IEnumerable<DecorationPackSku> packSkus)
        /// <summary>
        /// 添加商品SKU项集
        /// </summary>
        /// <param name="packRecommendedSkus">商品SKU项集</param>
        public void AddPackSkus(IEnumerable<DecorationPackRecommendedSku> packRecommendedSkus)
        {
            //先清空
            foreach (DecorationPackRecommendedSku skuItem in this.PackRecommendedSkus.ToArray())
            {
                this.PackRecommendedSkus.Remove(skuItem);
            }

            //再添加
            this.PackRecommendedSkus.AddRange(packRecommendedSkus);
            foreach (DecorationPackRecommendedSku packSku in packRecommendedSkus)
            {
                packSku.PackRecommendedItem = this;
            }

            //挂起领域事件 处理套餐上是否包含下架|变价商品|工艺属性
            EventMediator.Suspend(new PackShelvedChangedEvent(this.PackId));

        }
        #endregion

        #endregion

        #region 方法

        #region 设置套餐模板Id —— void SetPackId(Guid packId)
        /// <summary>
        /// 设置套餐模板Id
        /// </summary>
        /// <param name="packId">套餐模板Id</param>
        /// <remarks>注意：克隆套餐模板时候用，勿乱用</remarks>
        public void SetPackId(Guid packId)
        {
            this.PackId = packId;
        }
        #endregion

        #region 设置套餐模板空间Id —— void SetPackSpaceId(Guid packSpaceId)
        /// <summary>
        /// 设置套餐模板空间Id
        /// </summary>
        /// <param name="packSpaceId">套餐模板空间Id</param>
        /// <remarks>注意：克隆套餐模板时候用，勿乱用</remarks>
        public void SetPackSpaceId(Guid packSpaceId)
        {
            this.PackSpaceId = packSpaceId;
        }
        #endregion

        #region 克隆套餐时设置是否克隆 —— void SetIsClone()
        /// <summary>
        /// 克隆套餐时设置是否克隆
        /// </summary>
        public void SetIsClone()
        {
            IsClone = true;
        }
        #endregion

        #endregion
    }
}
