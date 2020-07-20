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
    public class PackSkuChangedEventHandler : IEventHandler<PackSkuChangedEvent>
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
        public PackSkuChangedEventHandler(DomainServiceMediator svcMediator, RepositoryMediator repMediator, IUnitOfWorkDesign unitOfWork)
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

        #region # 事件处理方法 —— void Handle(PackSkuChangedEvent eventSource)
        /// <summary>
        /// 事件处理方法
        /// </summary>
        /// <param name="eventSource">事件源</param>
        public void Handle(PackSkuChangedEvent eventSource)
        {
            DecorationPack currentPack = this._unitOfWork.Resolve<DecorationPack>(eventSource.PackId);

            //变价
            if (eventSource.Changed)
            {
                currentPack.SetHasChangedSku(true);
                this._unitOfWork.RegisterSave(currentPack);
            }
            else
            {
                //未变价
                IEnumerable<DecorationPackItem> packItems = 
                    //this._unitOfWork.ResolvePackItemsByPack(eventSource.PackId);
                    this._repMediator.DecorationPackItemRep.FindByPack(eventSource.PackId, null);
                //所有工艺和商品是否都没变价
                if (packItems.All(x => x.AllSkuNoChanged && x.AllCraftNoChanged))
                {
                    currentPack.SetHasChangedSku(false);
                    this._unitOfWork.RegisterSave(currentPack);
                }

            }

            this._unitOfWork.Commit();
        }
        #endregion
    }
}
