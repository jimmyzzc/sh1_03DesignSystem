using HDIPlatform.DesignSystem.Domain.Entities.FixedMealContext;
using HDIPlatform.DesignSystem.Domain.EventSources.FixedMealContext;
using HDIPlatform.DesignSystem.Domain.IRepositories;
using HDIPlatform.DesignSystem.Domain.Mediators;
using ShSoft.Infrastructure.EventBase;
using System.Collections.Generic;
using System.Linq;

namespace HDIPlatform.DesignSystem.DomainEventHandler.FixedMealContext
{
    /// <summary>
    /// 套餐模板商品SKU项|工艺 变价 事件处理者
    /// </summary>
    public class PackShelvedChangedEventHandler : IEventHandler<PackShelvedChangedEvent>
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
        public PackShelvedChangedEventHandler(DomainServiceMediator svcMediator, RepositoryMediator repMediator, IUnitOfWorkDesign unitOfWork)
        {
            this._svcMediator = svcMediator;
            this._repMediator = repMediator;
            this._unitOfWork = unitOfWork;
            this.Sort = uint.MaxValue;
        }

        #endregion

        #region # 执行顺序，倒序排列 —— uint Sort
        /// <summary>
        /// 执行顺序，倒序排列
        /// </summary>
        public uint Sort { get; private set; }
        #endregion

        #region # 事件处理方法 —— void Handle(PackShelvedChangedEvent eventSource)
        /// <summary>
        /// 事件处理方法
        /// </summary>
        /// <param name="eventSource">事件源</param>
        public void Handle(PackShelvedChangedEvent eventSource)
        {
            DecorationPack currentPack = this._unitOfWork.Resolve<DecorationPack>(eventSource.PackId);

            #region # 处理套餐
            //套餐下所有选区（选区1，选区2……）
            IEnumerable<DecorationPackItem> items = this._repMediator.DecorationPackItemRep.FindByPack(eventSource.PackId, null);
                //this._unitOfWork.ResolvePackItemsByPack(eventSource.PackId);
            //所有工艺和商品是否都没变价
            if (items.All(x => x.AllSkuNoChanged && x.AllCraftNoChanged))
            {
                currentPack.SetHasChangedSku(false);
            }
            else
            {
                currentPack.SetHasChangedSku(true);
            }
            //所有工艺和商品是否都上架
            if (items.All(x => x.AllSkuShelved && x.AllCraftShelved))
            {
                currentPack.SetHasOffShelvedSku(false);
            }
            else
            {
                currentPack.SetHasOffShelvedSku(true);
            }
            this._unitOfWork.RegisterSave(currentPack);
            #endregion
            this._unitOfWork.Commit();
        }
        #endregion
    }
}
