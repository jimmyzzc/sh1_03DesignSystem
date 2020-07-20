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
    /// 套餐模板商品SKU项上/下架事件处理者
    /// </summary>
    public class PackSkuShelvedEventHandler : IEventHandler<PackSkuShelvedEvent>
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
        public PackSkuShelvedEventHandler(DomainServiceMediator svcMediator, RepositoryMediator repMediator, IUnitOfWorkDesign unitOfWork)
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

        #region # 事件处理方法 —— void Handle(PackSkuShelvedEvent eventSource)
        /// <summary>
        /// 事件处理方法
        /// </summary>
        /// <param name="eventSource">事件源</param>
        public void Handle(PackSkuShelvedEvent eventSource)
        {
            DecorationPack currentPack = this._unitOfWork.Resolve<DecorationPack>(eventSource.PackId);

            //下架
            if (!eventSource.Shelved)
            {
                currentPack.SetHasOffShelvedSku(true);
                this._unitOfWork.RegisterSave(currentPack);
            }
            //上架
            else
            {
                IEnumerable<DecorationPackItem> packItems = this._repMediator.DecorationPackItemRep.FindByPack(eventSource.PackId, null);
                //所有工艺和商品是否都上架
                if (packItems.All(x => x.AllSkuShelved && x.AllCraftShelved))
                {
                    currentPack.SetHasOffShelvedSku(false);
                    this._unitOfWork.RegisterSave(currentPack);
                }
            }

            this._unitOfWork.Commit();
        }
        #endregion
    }
}
