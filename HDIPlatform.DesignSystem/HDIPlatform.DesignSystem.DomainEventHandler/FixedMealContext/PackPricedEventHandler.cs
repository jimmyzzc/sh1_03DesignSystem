using System;
using HDIPlatform.DesignSystem.Domain.Entities.FixedMealContext;
using HDIPlatform.DesignSystem.Domain.EventSources.FixedMealContext;
using HDIPlatform.DesignSystem.Domain.IRepositories;
using HDIPlatform.DesignSystem.Domain.Mediators;
using ShSoft.Infrastructure.EventBase;
using System.Collections.Generic;
using System.Linq;
using HDIPlatform.ResourceService.IAppService.Interfaces;
using SD.AOP.Core.Aspects.ForMethod;
using System.Transactions;
using ShSoft.Common.PoweredByLee;

namespace HDIPlatform.DesignSystem.DomainEventHandler.FixedMealContext
{
    /// <summary>
    /// 套餐模板商品SKU项|工艺 变价 事件处理者
    /// </summary>
    public class PackPricedEventHandler : IEventHandler<PackPricedEvent>
    {
        #region # 字段及依赖注入构造器

        /// <summary>
        /// 领域服务中介者
        /// </summary>
        private readonly DomainServiceMediator _svcMediator;

        /// <summary>
        /// 仓储中介者
        /// </summary>
        private readonly RepositoryMediator _repMediator;

        /// <summary>
        /// 单元事务
        /// </summary>
        private readonly IUnitOfWorkDesign _unitOfWork;

        private readonly IProductContract _productContract;
        private readonly ICraftEntityContract _craftEntityContract;

        /// <summary>
        /// 依赖注入构造器
        /// </summary>
        /// <param name="svcMediator">领域服务中介者</param>
        /// <param name="repMediator">仓储中介者</param>
        /// <param name="unitOfWork">单元事务</param>
        /// <param name="productContract"></param>
        /// <param name="craftEntityContract"></param>
        public PackPricedEventHandler(DomainServiceMediator svcMediator, RepositoryMediator repMediator, IUnitOfWorkDesign unitOfWork, IProductContract productContract, ICraftEntityContract craftEntityContract)
        {
            this._svcMediator = svcMediator;
            this._repMediator = repMediator;
            this._unitOfWork = unitOfWork;
            _productContract = productContract;
            _craftEntityContract = craftEntityContract;
            this.Sort = uint.MaxValue;
        }

        #endregion

        #region # 执行顺序，倒序排列 —— uint Sort
        /// <summary>
        /// 执行顺序，倒序排列
        /// </summary>
        public uint Sort { get; private set; }
        #endregion

        #region # 事件处理方法 —— void Handle(PackPricedEvent eventSource)
        /// <summary>
        /// 事件处理方法
        /// </summary>
        /// <param name="eventSource">事件源</param>
        public void Handle(PackPricedEvent eventSource)
        {

            IEnumerable<DecorationPackItem> packItems = this._unitOfWork.ResolvePackItemsByPack(eventSource.PackId);
            //工艺
            List<Guid> craftAllIds = packItems.SelectMany(x => x.PackCraftEntities).Select(x => x.CraftEntityId).Distinct().ToList();
            Dictionary<Guid, decimal> craftEntityAllInfos = this._craftEntityContract.GetCraftEntitiesCostPriceByIds(craftAllIds);
            //商品
            List<Guid> skuAllIds = packItems.SelectMany(x => x.PackSkus).Select(x => x.SkuId).Distinct().ToList();
            Dictionary<Guid, decimal> skuAllInfos = this._productContract.GetProductSkusCostPriceByIds(skuAllIds);

            foreach (DecorationPackItem packItem in packItems)
            {
                //DecorationPackItem currentPackItem = this._unitOfWork.Resolve<DecorationPackItem>(packItem.Id);
                //List<DecorationPackCraft> currentPackCrafts = packItem.PackCraftEntities.ToList();
                //currentPackCrafts.ForEach(x =>
                //{
                //    if (craftEntityAllInfos.ContainsKey(x.CraftEntityId))
                //        x.SetCostPrice(decimal.Round(craftEntityAllInfos[x.CraftEntityId], 2, MidpointRounding.AwayFromZero));

                //});
                //List<DecorationPackSku> currentPackSkus = packItem.PackSkus.ToList();
                //currentPackSkus.ForEach(x =>
                //{
                //    if (skuAllInfos.ContainsKey(x.SkuId))
                //        x.SetCostPrice(decimal.Round(skuAllInfos[x.SkuId], 2, MidpointRounding.AwayFromZero));
                //});

                packItem.SaveCostPrice(skuAllInfos, craftEntityAllInfos);
                this._unitOfWork.RegisterSave(packItem);
            }
            this._unitOfWork.Commit();
        }
        #endregion
    }
}
