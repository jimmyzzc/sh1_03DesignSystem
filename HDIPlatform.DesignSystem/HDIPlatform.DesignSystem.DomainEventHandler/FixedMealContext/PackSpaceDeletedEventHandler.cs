using HDIPlatform.DesignSystem.Domain.Entities.FixedMealContext;
using HDIPlatform.DesignSystem.Domain.EventSources.FixedMealContext;
using HDIPlatform.DesignSystem.Domain.IRepositories;
using HDIPlatform.DesignSystem.Domain.Mediators;
using ShSoft.Infrastructure.EventBase;
using System;
using System.Collections.Generic;

namespace HDIPlatform.DesignSystem.DomainEventHandler.FixedMealContext
{
    /// <summary>
    /// 套餐模板空间已删除事件处理者
    /// </summary>
    public class PackSpaceDeletedEventHandler : IEventHandler<PackSpaceDeletedEvent>
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
        public PackSpaceDeletedEventHandler(DomainServiceMediator svcMediator, RepositoryMediator repMediator, IUnitOfWorkDesign unitOfWork)
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

        #region # 事件处理方法 —— void Handle(PackSpaceDeletedEventHandler eventSource)
        /// <summary>
        /// 事件处理方法
        /// </summary>
        /// <param name="eventSource">事件源</param>
        public void Handle(PackSpaceDeletedEvent eventSource)
        {
            //删除相关套餐模板项
            IEnumerable<Guid> packItemIds = this._repMediator.DecorationPackItemRep.FindIdsByPack(eventSource.PackId, eventSource.PackSpaceId);
            this.RemovePackItems(packItemIds);

            //删除相关套餐模板方案空间
            this.RemovePackSchemeSpaces(eventSource.PackId, eventSource.PackSpaceId);

            this._unitOfWork.Commit();
        }
        #endregion

        #region # 删除相关套餐模板项 —— void RemovePackItems(IEnumerable<Guid> packItemIds)
        /// <summary>
        /// 删除相关套餐模板项
        /// </summary>
        /// <param name="packItemIds">套餐模板项Id集</param>
        private void RemovePackItems(IEnumerable<Guid> packItemIds)
        {
            foreach (Guid packItemId in packItemIds)
            {
                this._unitOfWork.RegisterRemove<DecorationPackItem>(packItemId);
            }
        }
        #endregion

        #region # 删除相关套餐模板方案空间 —— void RemovePackSchemeSpaces(Guid packId, Guid packSpaceId)
        /// <summary>
        /// 删除相关套餐模板方案空间
        /// </summary>
        /// <param name="packId">套餐模板Id</param>
        /// <param name="packSpaceId">套餐模板空间Id</param>
        private void RemovePackSchemeSpaces(Guid packId, Guid packSpaceId)
        {
            IEnumerable<DecorationPackScheme> packSchemes = this._unitOfWork.ResolvePackSchemesByPack(packId);

            foreach (DecorationPackScheme packScheme in packSchemes)
            {
                DecorationPackSchemeSpace currentSchemeSpace = packScheme.GetSchemeSpace(packSpaceId);
                if (currentSchemeSpace != null)
                    packScheme.RemoveSchemeSpace(currentSchemeSpace);

                this._unitOfWork.RegisterSave(packScheme);
            }
        }
        #endregion
    }
}
