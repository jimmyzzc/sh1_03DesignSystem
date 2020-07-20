using HDIPlatform.DesignSystem.Domain.Entities.FixedMealContext;
using HDIPlatform.DesignSystem.Domain.IRepositories.FixedMealContext;
using ShSoft.Infrastructure.Repository.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HDIPlatform.DesignSystem.Repository.Implements.FixedMealContext
{
    /// <summary>
    /// 套餐模板空间仓储实现
    /// </summary>
    public class DecorationPackSpaceRepository : EFRepositoryProvider<DecorationPackSpace>, IDecorationPackSpaceRepository
    {
        #region # 获取实体对象集合 —— override IQueryable<DecorationPackSpace> FindAllInner()
        /// <summary>
        /// 获取实体对象集合
        /// </summary>
        /// <returns>实体对象集合</returns>
        protected override IQueryable<DecorationPackSpace> FindAllInner()
        {
            return base.FindAllInner().Where(x => x.DecorationPack != null && !x.DecorationPack.Deleted).OrderBy(x => x.AddedTime);
        }
        #endregion

        #region # 根据套餐模板获取套餐空间列表 —— IEnumerable<DecorationPackSpace> FindByPack(...
        /// <summary>
        /// 根据套餐模板获取套餐空间列表
        /// </summary>
        /// <param name="packId">套餐模板Id</param>
        /// <returns>套餐空间列表</returns>
        public IEnumerable<DecorationPackSpace> FindByPack(Guid packId)
        {
            return this.Find(x => x.DecorationPack.Id == packId).OrderBy(x => x.Sort).AsEnumerable();
        }
        #endregion

        #region # 套餐模板空间名称是否存在 —— bool ExistsName(Guid packId, Guid? packSpaceId...
        /// <summary>
        /// 套餐模板空间名称是否存在
        /// </summary>
        /// <param name="packId">套餐模板Id</param>
        /// <param name="packSpaceId">套餐模板空间Id</param>
        /// <param name="packSpaceName">套餐模板空间名称</param>
        /// <returns>是否存在</returns>
        public bool ExistsName(Guid packId, Guid? packSpaceId, string packSpaceName)
        {
            IQueryable<DecorationPackSpace> packSpaces = base.Find(x => x.DecorationPack.Id == packId);

            if (packSpaceId != null)
            {
                DecorationPackSpace current = this.Single(packSpaceId.Value);

                if (current.Name == packSpaceName)
                {
                    return false;
                }

                return packSpaces.Any(x => x.Name == packSpaceName);
            }

            return packSpaces.Any(x => x.Name == packSpaceName);
        }
        #endregion
    }
}
