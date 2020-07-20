using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using HDIPlatform.DesignSystem.Domain.Entities.ArticleContext;
using HDIPlatform.DesignSystem.Domain.Entities.FixedMealContext;
using HDIPlatform.DesignSystem.Domain.EventSources.FixedMealContext;
using HDIPlatform.DesignSystem.Domain.IRepositories;
using HDIPlatform.DesignSystem.Domain.Mediators;
using ShSoft.Common.PoweredByLee;
using ShSoft.Infrastructure.EventBase;
using ShSoft.ValueObjects.Enums;

namespace HDIPlatform.DesignSystem.DomainEventHandler.FixedMealContext
{
    /// <summary>
    /// 固定套餐上架事件——克隆套餐副本事件处理者
    /// </summary>
    public class OnShelfDecorationPackEventHandler : IEventHandler<OnShelfDecorationPackEvent>
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
        public OnShelfDecorationPackEventHandler(DomainServiceMediator svcMediator, RepositoryMediator repMediator, IUnitOfWorkDesign unitOfWork)
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

        #region # 事件处理方法 —— void Handle(OnShelfDecorationPackEvent eventSource)
        /// <summary>
        /// 事件处理方法
        /// </summary>
        /// <param name="eventSource">事件源</param>
        public void Handle(OnShelfDecorationPackEvent eventSource)
        {
            this.CloneDecorationPack(eventSource.PackId);
        }
        #endregion

        #region # 克隆套餐模板 —— Guid CloneDecorationPack(Guid sourcePackId)
        /// <summary>
        /// 克隆套餐模板
        /// </summary>
        /// <param name="sourcePackId">源套餐模板Id</param>
        /// <returns>新套餐模板Id</returns>
        private void CloneDecorationPack(Guid sourcePackId)
        {
            DecorationPack sourcePack = this._repMediator.DecorationPackRep.Single(sourcePackId);
            //套餐模板名称,版本号
            string version = this._svcMediator.NumberSvc.GeneratePackVersionNo(sourcePack.Id, "Decoration");
            //string packName = "备份套餐【" + sourcePack.Name + "】" + version;
            //验证
            //Assert.IsFalse(this._repMediator.DecorationPackRep.ExistsName(packName), "套餐模板名称已存在！");

            //注：此处代码是为了将导航属性加载出来
            Trace.WriteLine(sourcePack.Spaces.Count);

            //注：此处代码是为了将导航属性加载出来
            foreach (DecorationPackSpace space in sourcePack.Spaces)
            {
                //注：此处代码是为了将导航属性加载出来
                Trace.WriteLine(space.SpaceDetails.Count);
            }

            //声明套餐模板副本Id
            Guid clonedPackId = Guid.NewGuid();

            //声明套餐模板空间Id映射关系字典
            IDictionary<Guid, Guid> spaceMaps = new Dictionary<Guid, Guid>();

            #region # 克隆套餐模板、套餐模板空间；设置源套餐Id，版本号；修改套餐名称；

            DecorationPack clonedPack = sourcePack.Clone<DecorationPack>();
            clonedPack.SetId(clonedPackId);
            //clonedPack.UpdateInfo(sourcePack.Name);
            clonedPack.SetSourcePackIdAndVersion(sourcePackId, version,true);
            var cloneArticles = new List<Article>();
            var packArticles = this._repMediator.ArticleRepository.GetArticlesByPackId(sourcePackId).ToList();
            foreach (var article in packArticles.Where(s => s.SpaceId == null))
            {
                Article cloneArticle = article.Clone<Article>();
                cloneArticle.SetId(Guid.NewGuid());
                cloneArticle.SetPackId(clonedPackId);
                cloneArticles.Add(cloneArticle);
            }
            //套餐模板空间Id重新赋值
            foreach (DecorationPackSpace space in clonedPack.Spaces)
            {
                Guid newPackSpaceId = Guid.NewGuid();
                spaceMaps.Add(space.Id, newPackSpaceId);
                space.SetId(newPackSpaceId);
                foreach (DecorationPackSpaceDetail spaceDetail in space.SpaceDetails)
                {
                    spaceDetail.SetId(Guid.NewGuid());
                }
                var spaceArticles = packArticles.Where(s => s.SpaceId == space.Id);
                foreach (var spaceArticle in spaceArticles)
                {
                    Article cloneArticle = spaceArticle.Clone<Article>();
                    cloneArticle.SetId(Guid.NewGuid());
                    cloneArticle.SetPackId(clonedPackId);
                    cloneArticles.Add(cloneArticle);
                }
            }

            this._unitOfWork.RegisterAdd(clonedPack);
            if (cloneArticles.Any())
            {
                this._unitOfWork.RegisterAddRange(cloneArticles);
            }

            #endregion

            #region # 克隆套餐模板项

            //IEnumerable<DecorationPackItem> packItems = this._repMediator.DecorationPackItemRep.FindByPack(sourcePackId, null);
            IEnumerable<DecorationPackItem> packItems = this._unitOfWork.GetItemsByPackSku(sourcePackId);

            foreach (DecorationPackItem packItem in packItems)
            {
                //注：此处代码是为了将导航属性加载出来
                //Trace.WriteLine(packItem.PackSkus.Count);
                //Trace.WriteLine(packItem.PackCraftEntities.Count);

                //克隆套餐模板项
                DecorationPackItem clonedPackItem = packItem.Clone<DecorationPackItem>();
                clonedPackItem.SetId(Guid.NewGuid());
                clonedPackItem.SetPackId(clonedPackId);
                clonedPackItem.SetPackSpaceId(spaceMaps[clonedPackItem.PackSpaceId]);
                clonedPackItem.SetIsClone();

                //重新为Id赋值
                foreach (DecorationPackSku packSku in clonedPackItem.PackSkus)
                {
                    packSku.SetId(Guid.NewGuid());
                }
                foreach (DecorationPackCraft packCraft in clonedPackItem.PackCraftEntities)
                {
                    packCraft.SetId(Guid.NewGuid());
                }

                this._unitOfWork.RegisterAdd(clonedPackItem);
            }

            #endregion

            #region # 克隆套餐模板推荐项

            //IEnumerable<DecorationPackItem> packItems = this._repMediator.DecorationPackItemRep.FindByPack(sourcePackId, null);
            IEnumerable<DecorationPackRecommendedItem> packRecommendedItems = this._unitOfWork.GetRecommendedItemsByPack(sourcePackId);

            foreach (DecorationPackRecommendedItem packRecommendedItem in packRecommendedItems)
            {
                //注：此处代码是为了将导航属性加载出来
                //Trace.WriteLine(packItem.PackSkus.Count);
                //Trace.WriteLine(packItem.PackCraftEntities.Count);

                //克隆套餐模板项
                DecorationPackRecommendedItem clonedPackRecommendedItem = packRecommendedItem.Clone<DecorationPackRecommendedItem>();
                clonedPackRecommendedItem.SetId(Guid.NewGuid());
                clonedPackRecommendedItem.SetPackId(clonedPackId);
                clonedPackRecommendedItem.SetPackSpaceId(spaceMaps[clonedPackRecommendedItem.PackSpaceId]);
                clonedPackRecommendedItem.SetIsClone();

                //重新为Id赋值
                foreach (DecorationPackRecommendedSku packSku in clonedPackRecommendedItem.PackRecommendedSkus)
                {
                    packSku.SetId(Guid.NewGuid());
                }
                this._unitOfWork.RegisterAdd(clonedPackRecommendedItem);
            }

            #endregion

            #region # 克隆套餐模板方案

            IEnumerable<DecorationPackScheme> packSchemes = this._repMediator.DecorationPackSchemeRep.FindByPack(sourcePackId);

            foreach (DecorationPackScheme packScheme in packSchemes)
            {
                //注：此处代码是为了将导航属性加载出来
                Trace.WriteLine(packScheme.SchemeSpaces.Count);

                //克隆套餐模板方案
                DecorationPackScheme clonedPackScheme = packScheme.Clone<DecorationPackScheme>();
                clonedPackScheme.SetId(Guid.NewGuid());
                clonedPackScheme.SetPackId(clonedPackId);

                foreach (DecorationPackSchemeSpace schemeSpace in clonedPackScheme.SchemeSpaces)
                {
                    schemeSpace.SetId(Guid.NewGuid());
                    schemeSpace.SetPackSpaceId(spaceMaps[schemeSpace.PackSpaceId]);
                }

                this._unitOfWork.RegisterAdd(clonedPackScheme);
            }

            #endregion

            #region # 克隆套餐关联大包/定制

            IEnumerable<DecorationPack_BalePack> decorationPackBalePacks =
                this._repMediator.DecorationPack_BalePackRep.FindByDecorationPack(sourcePackId);

            foreach (DecorationPack_BalePack decorationPackBalePack in decorationPackBalePacks)
            {
                DecorationPack_BalePack clonedDecorationPackBalePack = decorationPackBalePack.Clone<DecorationPack_BalePack>();
                clonedDecorationPackBalePack.SetId(Guid.NewGuid());
                clonedDecorationPackBalePack.SetDecorationPackId(clonedPackId);
                this._unitOfWork.RegisterAdd(clonedDecorationPackBalePack);
            }
            #endregion

            this._unitOfWork.Commit();
        }
        #endregion
    }
}
