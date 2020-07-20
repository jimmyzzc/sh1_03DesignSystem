using HDIPlatform.DesignSystem.Domain.Entities.BaleContext;
using HDIPlatform.DesignSystem.Domain.IRepositories.BaleContext;
using ShSoft.Infrastructure.Repository.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HDIPlatform.DesignSystem.Repository.Implements.BaleContext
{
    /// <summary>
    /// 大包套餐选区仓储实现
    /// </summary>
    public class BalePackChoiceAreaRepository : EFRepositoryProvider<BalePackChoiceArea>, IBalePackChoiceAreaRepository
    {

        #region # 获取实体对象集合 —— override IQueryable<DecorationPackCraft> FindAllInner()
        /// <summary>
        /// 获取实体对象集合
        /// </summary>
        /// <returns>实体对象集合</returns>
        protected override IQueryable<BalePackChoiceArea> FindAllInner()
        {
            return base.FindAllInner().Where(x => x.BalePack != null && !x.BalePack.Deleted).OrderBy(x => x.Sort);
        }
        #endregion
        #region 根据选区Id集获取套餐列表 —— IDictionary<Guid, string> GetPackByChoiceAreaIds(IEnumerable<Guid> choiceAreaIds)
        /// <summary>
        /// 根据选区Id集获取套餐列表
        /// </summary>
        /// <param name="choiceAreaIds">选区Id集</param>
        /// <returns>IDictionary[Guid, string]，[套餐Id, 套餐名称]</returns>
        public IDictionary<Guid, string> GetPackByChoiceAreaIds(IEnumerable<Guid> choiceAreaIds)
        {
            IQueryable<BalePackChoiceArea> choiceAreas = this.FindAllInner().Where(s => choiceAreaIds.Contains(s.Id));
            IQueryable<BalePack> packs = choiceAreas.Select(s => s.BalePack).Distinct();

            return packs.ToDictionary(s => s.Id, s => s.Name);
        }
        #endregion


        #region 验证套餐内选区名称是否重复 —— bool ExistsChoiceAreaName(Guid packId, Guid? choiceAreaId, string choiceAreaName)

        /// <summary>
        /// 验证套餐内选区名称是否重复
        /// </summary>
        /// <param name="packId">套餐Id</param>
        /// <param name="choiceAreaId">选区Id</param>
        /// <param name="choiceAreaName">选区名称</param>
        /// <returns></returns>
        public bool ExistsChoiceAreaName(Guid packId, Guid? choiceAreaId, string choiceAreaName)
        {
            return base.Exists(x => x.BalePack.Id == packId && (choiceAreaId == null || x.Id != choiceAreaId) && x.Name == choiceAreaName);
        }
        #endregion

    }
}
