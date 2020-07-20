using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using HDIPlatform.DesignSystem.Domain.Entities.BaleContext;
using HDIPlatform.DesignSystem.Domain.EventSources.BaleContext;
using HDIPlatform.DesignSystem.Domain.IRepositories;
using HDIPlatform.DesignSystem.Domain.Mediators;
using ShSoft.Common.PoweredByLee;
using ShSoft.Infrastructure.EventBase;

namespace HDIPlatform.DesignSystem.DomainEventHandler.BaleContext
{
    /// <summary>
    /// 大包定制套餐上架事件——克隆套餐副本事件处理者
    /// </summary>
    public class OnShelfBalePackEventHandler : IEventHandler<OnShelfBalePackEvent>
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
        public OnShelfBalePackEventHandler(DomainServiceMediator svcMediator, RepositoryMediator repMediator, IUnitOfWorkDesign unitOfWork)
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

        #region # 事件处理方法 —— void Handle(OnShelfBalePackEvent eventSource)
        /// <summary>
        /// 事件处理方法
        /// </summary>
        /// <param name="eventSource">事件源</param>
        public void Handle(OnShelfBalePackEvent eventSource)
        {
            this.CloneBalePack(eventSource.PackId);
        }
        #endregion

        #region # 克隆套餐模板 —— Guid CloneDecorationPack(Guid sourcePackId)
        /// <summary>
        /// 克隆套餐模板
        /// </summary>
        /// <param name="sourcePackId">源套餐模板Id</param>
        /// <returns>新套餐模板Id</returns>
        private void CloneBalePack(Guid sourcePackId)
        {
            BalePack sourcePack = this._repMediator.BalePackRep.Single(sourcePackId);
            //套餐模板名称,版本号
            string version = this._svcMediator.NumberSvc.GeneratePackVersionNo(sourcePack.Id, sourcePack.BalePackType.ToString());
            //string packName = "备份套餐【" + sourcePack.Name + "】" + version;
            //验证
            //Assert.IsFalse(this._repMediator.DecorationPackRep.ExistsName(packName), "套餐模板名称已存在！");

            //注：此处代码是为了将导航属性加载出来
            Trace.WriteLine(sourcePack.ChoiceAreas.Count);

            //声明套餐副本Id
            Guid clonedPackId = Guid.NewGuid();

            //声明套餐选区Id映射关系字典
            IDictionary<Guid, Guid> choiceAreaMaps = new Dictionary<Guid, Guid>();

            #region # 克隆套餐、套餐选区；设置源套餐Id，版本号；修改套餐名称；

            BalePack clonedPack = sourcePack.Clone<BalePack>();
            clonedPack.SetId(clonedPackId);
            clonedPack.ModifyPackNameAndRatio(sourcePack.Name, sourcePack.DiscountRatio);
            clonedPack.SetSourcePackIdAndVersion(sourcePackId, version);

            //套餐模板空间Id重新赋值
            foreach (BalePackChoiceArea choiceArea in clonedPack.ChoiceAreas)
            {
                Guid newPackSpaceId = Guid.NewGuid();
                choiceAreaMaps.Add(choiceArea.Id, newPackSpaceId);
                choiceArea.SetId(newPackSpaceId);
            }

            this._unitOfWork.RegisterAdd(clonedPack);

            #endregion

            #region # 克隆套餐组、品类、商品；设置是否克隆；
            IEnumerable<Guid> choiceAreaIds = this._repMediator.BalePackRep.GetChoiceAreaIdsByPackId(sourcePackId);
            IEnumerable<BalePackGroup> groups = this._repMediator.BalePackGroupRep.GetGroupByChoiceAreaIds(choiceAreaIds);


            foreach (BalePackGroup group in groups)
            {
                //注：此处代码是为了将导航属性加载出来
                Trace.WriteLine(group.BalePackCategorys.Count);

                foreach (BalePackCategory category in group.BalePackCategorys)
                {
                    //注：此处代码是为了将导航属性加载出来
                    Trace.WriteLine(category.BalePackProducts.Count);
                }

                //克隆套餐组
                BalePackGroup clonedGroup = group.Clone<BalePackGroup>();
                clonedGroup.SetId(Guid.NewGuid());
                clonedGroup.SetChoiceAreaId(choiceAreaMaps[clonedGroup.ChoiceAreaId]);
                clonedGroup.SetIsClone();

                //重新为Id赋值
                foreach (BalePackCategory clonedCategory in clonedGroup.BalePackCategorys)
                {
                    clonedCategory.SetId(Guid.NewGuid());

                    foreach (BalePackProduct clonedProduct in clonedCategory.BalePackProducts)
                    {
                        clonedProduct.SetId(Guid.NewGuid());
                    }
                }



                this._unitOfWork.RegisterAdd(clonedGroup);
            }

            #endregion

            this._unitOfWork.Commit();
        }
        #endregion
    }
}
