using HDIPlatform.DesignSystem.Domain.Entities.FixedMealContext;
using HDIPlatform.DesignSystem.Domain.IRepositories.FixedMealContext;
using ShSoft.Infrastructure.Repository.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HDIPlatform.DesignSystem.Repository.Implements.FixedMealContext
{
    /// <summary>
    /// 套餐模板方案仓储实现
    /// </summary>
    public class DecorationPackSchemeRepository : EFRepositoryProvider<DecorationPackScheme>, IDecorationPackSchemeRepository
    {
        #region # 根据套餐模板获取套餐模板方案列表 —— IEnumerable<DecorationPackScheme> FindByPack(Guid packId)
        /// <summary>
        /// 根据套餐模板获取套餐模板方案列表
        /// </summary>
        /// <param name="packId">套餐模板Id</param>
        /// <returns>套餐模板方案列表</returns>
        public IEnumerable<DecorationPackScheme> FindByPack(Guid packId)
        {
            return base.Find(x => x.PackId == packId).AsEnumerable();
        }
        #endregion

        #region # 是否存在套餐模板方案 —— bool ExistsScheme(Guid packId)
        /// <summary>
        /// 是否存在套餐模板方案
        /// </summary>
        /// <param name="packId">套餐模板Id</param>
        /// <returns>是否存在</returns>
        public bool ExistsScheme(Guid packId)
        {
            return base.Exists(x => x.PackId == packId);
        }
        #endregion

        #region # 是否存在方案名称 —— bool ExistsName(Guid packId, Guid? packSchemeId...
        /// <summary>
        /// 是否存在方案名称
        /// </summary>
        /// <param name="packId">套餐模板Id</param>
        /// <param name="packSchemeId">套餐模板方案Id</param>
        /// <param name="schemeName">方案名称</param>
        /// <returns>是否存在</returns>
        public bool ExistsName(Guid packId, Guid? packSchemeId, string schemeName)
        {
            IQueryable<DecorationPackScheme> packSchemes = base.Find(x => x.PackId == packId);

            if (packSchemeId != null)
            {
                DecorationPackScheme current = this.Single(packSchemeId.Value);

                if (current.Name == schemeName)
                {
                    return false;
                }

                return packSchemes.Any(x => x.Name == schemeName);
            }

            return packSchemes.Any(x => x.Name == schemeName);
        }
        #endregion

        #region # 获取套餐模板的其他默认方案 —— DecorationPackScheme FirstOrDefault(Guid packId...
        /// <summary>
        /// 获取套餐模板的其他默认方案
        /// </summary>
        /// <param name="packId">套餐模板Id</param>
        /// <param name="packSchemeId">套餐模板方案Id</param>
        /// <returns>其他默认方案</returns>
        public DecorationPackScheme FirstOrDefault(Guid packId, Guid packSchemeId)
        {
            return base.Find(x => x.PackId == packId && x.Id != packSchemeId).OrderByDescending(x => x.AddedTime).FirstOrDefault();
        }
        #endregion


        #region # 根据套餐模板获取套餐模板默认方案列表 —— IEnumerable<DecorationPackScheme> FindDefaultByPack(List<Guid> packIds)

        /// <summary>
        /// 根据套餐模板获取套餐模板默认方案列表
        /// </summary>
        /// <param name="packIds">套餐模板Id</param>
        /// <returns>套餐模板默认方案列表</returns>
        public IEnumerable<DecorationPackScheme> FindDefaultByPack(List<Guid> packIds)
        {
            return base.Find(x => packIds.Contains(x.PackId) && x.IsDefault).AsEnumerable();
        }

        #endregion

        #region # 根据套餐模板获取套餐模板默认方案 —— DecorationPackScheme GetDefaultByPack(Guid packId);

        /// <summary>
        /// 根据套餐模板获取套餐模板默认方案列表
        /// </summary>
        /// <param name="packId">套餐模板Id</param>
        /// <returns>套餐模板默认方案</returns>
        public DecorationPackScheme GetDefaultByPack(Guid packId)
        {
            return base.Find(x => x.PackId == packId && x.IsDefault).SingleOrDefault(); 
        }

        #endregion
    }
}
