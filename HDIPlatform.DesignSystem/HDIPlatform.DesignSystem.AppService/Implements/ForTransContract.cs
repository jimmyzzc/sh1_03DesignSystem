using HDIPlatform.DesignSystem.Domain.Entities.BaleContext;
using HDIPlatform.DesignSystem.Domain.Entities.FixedMealContext;
using HDIPlatform.DesignSystem.Domain.IRepositories;
using HDIPlatform.DesignSystem.Domain.Mediators;
using HDIPlatform.DesignSystem.IAppService.Interfaces;
using ShSoft.Infrastructure.Global.Transaction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using ShSoft.Common.PoweredByLee;

namespace HDIPlatform.DesignSystem.AppService.Implements
{
    /// <summary>
    /// For事务流转服务契约实现
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall, ConcurrencyMode = ConcurrencyMode.Multiple, ReleaseServiceInstanceOnTransactionComplete= false)]
    public class ForTransContract : IForTransContract
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

        /// <summary>
        /// 依赖注入构造器
        /// </summary>
        /// <param name="svcMediator">领域服务中介者</param>
        /// <param name="repMediator">仓储中介者</param>
        /// <param name="unitOfWork">单元事务</param>
        public ForTransContract(DomainServiceMediator svcMediator, RepositoryMediator repMediator, IUnitOfWorkDesign unitOfWork)
        {
            this._svcMediator = svcMediator;
            this._repMediator = repMediator;
            this._unitOfWork = unitOfWork;
        }

        #endregion

        #region # 下架套餐模板商品SKU —— void OffShelfDecorationPackSku(Guid skuId)
        /// <summary>
        /// 下架套餐模板商品SKU
        /// </summary>
        /// <param name="skuId">商品SKU Id</param>
        [OperationBehavior(TransactionScopeRequired = true)]
        public void OffShelfDecorationPackSku(Guid skuId)
        {
            IEnumerable<Guid> packItemIds = this._repMediator.DecorationPackSkuRep.FindPackItemIds(skuId);

            foreach (Guid packItemId in packItemIds)
            {
                DecorationPackItem currentPackItem = this._unitOfWork.Resolve<DecorationPackItem>(packItemId);
                DecorationPackSku currentPackSku = currentPackItem.GetPackSku(skuId);
                currentPackSku.OffShelf();

                this._unitOfWork.RegisterSave(currentPackItem);
            }

            this._unitOfWork.UnitedCommit();
        }
        #endregion

        #region # 重新上架套餐模板商品SKU —— void ReOnShelfDecorationPackSku(Guid skuId)
        /// <summary>
        /// 重新上架套餐模板商品SKU
        /// </summary>
        /// <param name="skuId">商品SKU Id</param>
        [OperationBehavior(TransactionScopeRequired = true)]
        public void ReOnShelfDecorationPackSku(Guid skuId)
        {
            IEnumerable<Guid> packItemIds = this._repMediator.DecorationPackSkuRep.FindPackItemIds(skuId);

            foreach (Guid packItemId in packItemIds)
            {
                DecorationPackItem currentPackItem = this._unitOfWork.Resolve<DecorationPackItem>(packItemId);
                DecorationPackSku currentPackSku = currentPackItem.GetPackSku(skuId);
                currentPackSku.ReOnShelf();

                this._unitOfWork.RegisterSave(currentPackItem);
            }

            this._unitOfWork.UnitedCommit();
        }
        #endregion

        #region # 下架套餐模板工艺 —— void OffShelfDecorationPackCraft(Guid craftEntityId)
        /// <summary>
        /// 下架套餐模板工艺
        /// </summary>
        /// <param name="craftEntityId">工艺实体Id</param>
        [OperationBehavior(TransactionScopeRequired = true)]
        public void OffShelfDecorationPackCraft(Guid craftEntityId)
        {
            IEnumerable<Guid> packItemIds = this._repMediator.DecorationPackCraftRep.FindPackItemIds(craftEntityId);

            foreach (Guid packItemId in packItemIds)
            {
                DecorationPackItem currentPackItem = this._unitOfWork.Resolve<DecorationPackItem>(packItemId);
                DecorationPackCraft currentPackCraft = currentPackItem.GetPackCraft(craftEntityId);
                currentPackCraft.OffShelf();

                this._unitOfWork.RegisterSave(currentPackItem);
            }

            this._unitOfWork.UnitedCommit();
        }
        #endregion

        #region # 重新上架套餐模板工艺 —— void ReOnShelfDecorationPackCraft(Guid craftEntityId)
        /// <summary>
        /// 重新上架套餐模板工艺
        /// </summary>
        /// <param name="craftEntityId">工艺实体Id</param>
        [OperationBehavior(TransactionScopeRequired = true)]
        public void ReOnShelfDecorationPackCraft(Guid craftEntityId)
        {
            IEnumerable<Guid> packItemIds = this._repMediator.DecorationPackCraftRep.FindPackItemIds(craftEntityId);

            foreach (Guid packItemId in packItemIds)
            {
                DecorationPackItem currentPackItem = this._unitOfWork.Resolve<DecorationPackItem>(packItemId);
                DecorationPackCraft currentPackCraft = currentPackItem.GetPackCraft(craftEntityId);
                currentPackCraft.ReOnShelf();

                this._unitOfWork.RegisterSave(currentPackItem);
            }

            this._unitOfWork.UnitedCommit();
        }
        #endregion

        #region # 下架定制套餐商品SKU —— void OffShelfBalePackSku(Guid skuId)
        /// <summary>
        /// 下架定制套餐商品SKU
        /// </summary>
        /// <param name="skuId">商品SKU Id</param>
        [OperationBehavior(TransactionScopeRequired = true)]
        public void OffShelfBalePackSku(Guid skuId)
        {
            //组集
            IEnumerable<Guid> groupIds = this._repMediator.BalePackProductRep.GetBalePackGroupIds(skuId);
            IEnumerable<BalePackGroup> groups = this._unitOfWork.ResolveBalePackGroup(groupIds);

            foreach (BalePackGroup group in groups)
            {
                IEnumerable<BalePackCategory> categorys = group.BalePackCategorys;

                foreach (BalePackCategory category in categorys)
                {
                    if (category.ExistsProduct(skuId))
                    {
                        category.ProductShelfOff(skuId);
                    }
                }

                this._unitOfWork.RegisterSave(group);
            }

            this._unitOfWork.UnitedCommit();
        }
        #endregion

        #region # 重新上架定制套餐商品SKU ——void ReOnShelfBalePackSku(Guid skuId)
        /// <summary>
        /// 重新上架定制套餐商品SKU
        /// </summary>
        /// <param name="skuId">商品SKU Id</param>
        [OperationBehavior(TransactionScopeRequired = true)]
        public void ReOnShelfBalePackSku(Guid skuId)
        {
            //组集
            IEnumerable<Guid> groupIds = this._repMediator.BalePackProductRep.GetBalePackGroupIds(skuId);
            IEnumerable<BalePackGroup> groups = this._unitOfWork.ResolveBalePackGroup(groupIds);

            foreach (BalePackGroup group in groups)
            {
                IEnumerable<BalePackCategory> categorys = group.BalePackCategorys;

                foreach (BalePackCategory category in categorys)
                {
                    if (category.ExistsProduct(skuId))
                    {
                        category.ProductReShelfed(skuId);
                    }
                }

                this._unitOfWork.RegisterSave(group);
            }

            this._unitOfWork.UnitedCommit();
        }
        #endregion

        //工艺端 工艺模板取消工法
        #region # 工艺是否在套餐中使用 —— bool IsCraftInPack(IEnumerable<Guid> craftEntityIds)
        /// <summary>
        /// 工艺是否在套餐中使用
        /// </summary>
        /// <param name="craftEntityIds">工艺实体Id集</param>
        /// <returns>true|使用</returns>
        public bool IsCraftInPack(IEnumerable<Guid> craftEntityIds)
        {
            craftEntityIds = craftEntityIds ?? new List<Guid>();
            if (!craftEntityIds.Any())
                return false;
            IEnumerable<Guid> packIds = this._repMediator.DecorationPackCraftRep.GetPackIdsByCrafts(craftEntityIds);
            //排除克隆版本套餐
            packIds = this._repMediator.DecorationPackRep.Find(packIds).Values.Where(s => !s.IsClone).Select(s => s.Id);


            return packIds.Any();
        }
        #endregion




        #region # 测试

        #endregion

    }
}