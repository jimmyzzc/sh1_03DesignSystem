using HDIPlatform.DesignSystem.Domain.Entities.FixedMealContext;
using HDIPlatform.DesignSystem.Domain.IRepositories.FixedMealContext;
using ShSoft.Infrastructure.Repository.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HDIPlatform.DesignSystem.Repository.Implements.FixedMealContext
{
    /// <summary>
    /// 套餐模板 - 大包/定制模板仓储实现
    /// </summary>
    public class DecorationPack_BalePackRepository : EFRepositoryProvider<DecorationPack_BalePack>, IDecorationPack_BalePackRepository
    {
        #region # 获取唯一关联关系 —— DecorationPack_BalePack Single(Guid decorationPackId, Guid balePackId)
        /// <summary>
        /// 获取唯一关联关系
        /// </summary>
        /// <param name="decorationPackId">套餐模板Id</param>
        /// <param name="balePackId">大包/定制模板Id</param>
        /// <returns>套餐模板 - 大包/定制模板关联关系</returns>
        public DecorationPack_BalePack Single(Guid decorationPackId, Guid balePackId)
        {
            DecorationPack_BalePack currentRelation = base.SingleOrDefault(x => x.DecorationPackId == decorationPackId && x.BalePackId == balePackId);

            #region # 验证

            if (currentRelation == null)
            {
                throw new InvalidOperationException("关联关系不存在！");
            }

            #endregion

            return currentRelation;
        }
        #endregion

        #region # 是否存在关联关系 —— bool Exists(Guid decorationPackId, Guid balePackId)
        /// <summary>
        /// 是否存在关联关系
        /// </summary>
        /// <param name="decorationPackId">套餐模板Id</param>
        /// <param name="balePackId">大包/定制模板Id</param>
        /// <returns>是否存在</returns>
        public bool Exists(Guid decorationPackId, Guid balePackId)
        {
            return base.Exists(x => x.DecorationPackId == decorationPackId && x.BalePackId == balePackId);
        }
        #endregion

        #region # 根据套餐模板获取关联关系列表 —— IEnumerable<DecorationPack_BalePack> FindByDecorationPack(...
        /// <summary>
        /// 根据套餐模板获取关联关系列表
        /// </summary>
        /// <param name="decorationPackId">套餐模板Id</param>
        /// <returns>关联关系列表</returns>
        public IEnumerable<DecorationPack_BalePack> FindByDecorationPack(Guid decorationPackId)
        {
            return base.Find(x => x.DecorationPackId == decorationPackId).AsEnumerable();
        }
        #endregion

        #region # 根据套餐模板获取大包/定制模板Id列表 —— IEnumerable<Guid> FindBalePackIdsByDecorationPack(...
        /// <summary>
        /// 根据套餐模板获取大包/定制模板Id列表
        /// </summary>
        /// <param name="decorationPackId">套餐模板Id</param>
        /// <returns>大包/定制模板Id列表</returns>
        public IEnumerable<Guid> FindBalePackIdsByDecorationPack(Guid decorationPackId)
        {
            IQueryable<DecorationPack_BalePack> relations = base.Find(x => x.DecorationPackId == decorationPackId);

            IEnumerable<Guid> balePackIds = relations.Select(x => x.BalePackId).AsEnumerable();

            return balePackIds;
        }
        #endregion
    }
}
