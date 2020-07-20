using HDIPlatform.DesignSystem.Domain.Entities.FixedMealContext;
using HDIPlatform.DesignSystem.Domain.IDomainServices;
using HDIPlatform.DesignSystem.Domain.IRepositories;
using HDIPlatform.DesignSystem.Domain.Mediators;
using System.Collections.Generic;
using System.Linq;

namespace HDIPlatform.DesignSystem.DomainService.Implements
{
    /// <summary>
    /// 套餐模板领域服务实现
    /// </summary>
    public class DecorationPackService : IDecorationPackService
    {
        #region # 字段及依赖注入构造器

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
        /// <param name="repMediator">仓储中介者</param>
        /// <param name="unitOfWork">单元事务</param>
        public DecorationPackService(RepositoryMediator repMediator, IUnitOfWorkDesign unitOfWork)
        {
            this._repMediator = repMediator;
            this._unitOfWork = unitOfWork;
        }

        #endregion

        #region # 重置排序 —— void InitSorts()
        /// <summary>
        /// 重置排序
        /// </summary>
        public void InitSorts()
        {
            //修改源套餐排序
            IEnumerable<DecorationPack> decorationPacks = this._unitOfWork.ResolveAll<DecorationPack>().Where(x => x.SourcePackId == null).OrderBy(x => x.Sort);

            int sort = 1;

            foreach (DecorationPack pack in decorationPacks)
            {
                pack.SetSort(sort);
                sort++;

                this._unitOfWork.RegisterSave(pack);
            }

            this._unitOfWork.Commit();
        }
        #endregion

        #region # 获取聚合根实体关键字 —— string GetKeywords(DecorationPack entity)
        /// <summary>
        /// 获取聚合根实体关键字
        /// </summary>
        /// <param name="entity">聚合根实体对象</param>
        /// <returns>关键字</returns>
        public string GetKeywords(DecorationPack entity)
        {
            return entity.Keywords;
        }
        #endregion
    }
}
