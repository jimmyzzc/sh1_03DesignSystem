using System;
using System.Collections.Generic;
using System.Linq;
using HDIPlatform.DesignSystem.Domain.Entities.BaleContext;
using HDIPlatform.DesignSystem.Domain.EventSources.BaleContext;
using HDIPlatform.DesignSystem.Domain.IRepositories;
using HDIPlatform.DesignSystem.Domain.Mediators;
using ShSoft.Infrastructure.EventBase;

namespace HDIPlatform.DesignSystem.DomainEventHandler.BaleContext
{
    /// <summary>
    ///套餐商品上/下架事件处理者
    /// </summary>
    public class BalePackProductShelvedEventHandler : IEventHandler<BalePackProductShelvedEvent>
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
        public BalePackProductShelvedEventHandler(DomainServiceMediator svcMediator, RepositoryMediator repMediator, IUnitOfWorkDesign unitOfWork)
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

        #region # 事件处理方法 —— void Handle(BalePackProductShelvedEvent eventSource)
        /// <summary>
        /// 事件处理方法
        /// </summary>
        /// <param name="eventSource">事件源</param>
        public void Handle(BalePackProductShelvedEvent eventSource)
        {

            BalePackChoiceArea area = this._repMediator.BalePackChoiceAreaRep.Single(eventSource.ChoiceAreaId);
            BalePack balePack = this._unitOfWork.Resolve<BalePack>(area.BalePack.Id);
            //下架
            if (!eventSource.Shelved)
            {
                balePack.SetHasOffShelvedSku(true);
                this._unitOfWork.RegisterSave(balePack);
            }
            //上架
            else
            {
                //查询套餐下所有商品均上架
                IEnumerable<Guid> choiceAreaIds = balePack.ChoiceAreas.Select(s => s.Id);
                IEnumerable<BalePackProduct> products = this._repMediator.BalePackProductRep.GetProductsByAreaIds(choiceAreaIds);

                if (products.All(s => s.Shelved))
                {
                    balePack.SetHasOffShelvedSku(false);
                    this._unitOfWork.RegisterSave(balePack);
                }
            }

            this._unitOfWork.Commit();
        }
        #endregion
    }
}
