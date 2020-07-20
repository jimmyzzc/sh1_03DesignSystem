using HDIPlatform.DesignSystem.Domain.Entities.FixedMealContext;
using HDIPlatform.DesignSystem.Domain.IRepositories.FixedMealContext;
using ShSoft.Infrastructure.Repository.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using ShSoft.Common.PoweredByLee;

namespace HDIPlatform.DesignSystem.Repository.Implements.FixedMealContext
{
    /// <summary>
    /// 套餐模板项仓储实现
    /// </summary>
    public class DecorationPackItemRepository : EFRepositoryProvider<DecorationPackItem>, IDecorationPackItemRepository
    {
        #region # 获取实体对象集合 —— override IQueryable<DecorationPackItem> FindAllInner()
        /// <summary>
        /// 获取实体对象集合
        /// </summary>
        /// <returns>实体对象集合</returns>
        protected override IQueryable<DecorationPackItem> FindAllInner()
        {
            return base.FindAllInner().OrderBy(x => x.AddedTime);
        }
        #endregion

        #region # 根据套餐模板获取套餐模板项列表 —— IEnumerable<DecorationPackItem> FindByPack(Guid packId)
        /// <summary>
        /// 根据套餐模板获取套餐模板项列表
        /// </summary>
        /// <param name="packId">套餐模板Id</param>
        /// <param name="packSpaceId">套餐模板空间Id</param>
        /// <returns>套餐模板项列表</returns>
        public IEnumerable<DecorationPackItem> FindByPack(Guid packId, Guid? packSpaceId)
        {
            Expression<Func<DecorationPackItem, bool>> condition =
                x =>
                    x.PackId == packId &&
                    (packSpaceId == null || x.PackSpaceId == packSpaceId);

            return base.Find(condition).AsEnumerable();
        }
        #endregion

        #region # 根据套餐模板获取套餐模板项Id列表 —— IEnumerable<Guid> FindIdsByPack(Guid packId, Guid? packSpaceId)
        /// <summary>
        /// 根据套餐模板获取套餐模板项Id列表
        /// </summary>
        /// <param name="packId">套餐模板Id</param>
        /// <param name="packSpaceId">套餐模板空间Id</param>
        /// <returns>套餐模板项Id列表</returns>
        public IEnumerable<Guid> FindIdsByPack(Guid packId, Guid? packSpaceId)
        {
            Expression<Func<DecorationPackItem, bool>> condition =
                x =>
                    x.PackId == packId &&
                    (packSpaceId == null || x.PackSpaceId == packSpaceId);

            return base.FindIds(condition).AsEnumerable();
        }
        #endregion

        #region # 套餐模板项名称是否存在 —— bool ExistsName(Guid packId, Guid? packItemId...
        /// <summary>
        /// 套餐模板项名称是否存在
        /// </summary>
        /// <param name="packId">套餐模板Id</param>
        /// <param name="packItemId">套餐模板项Id</param>
        /// <param name="packItemName">套餐模板项名称</param>
        /// <returns>是否存在</returns>
        public bool ExistsName(Guid packId, Guid? packItemId, string packItemName)
        {
            IQueryable<DecorationPackItem> packItems = base.Find(x => x.PackId == packId);

            if (packItemId != null)
            {
                DecorationPackItem current = this.Single(packItemId.Value);

                if (current.Name == packItemName)
                {
                    return false;
                }

                return packItems.Any(x => x.Name == packItemName);
            }

            return packItems.Any(x => x.Name == packItemName);
        }
        #endregion

        #region # 套餐模板项名称是否存在（空间） —— bool ExistsName(Guid packId,Guid packSpaceId, Guid? packItemId...
        /// <summary>
        /// 套餐模板项名称是否存在（空间）
        /// </summary>
        /// <param name="packId">套餐模板Id</param>
        /// <param name="packSpaceId">空间Id</param>
        /// <param name="packItemId">套餐模板项Id</param>
        /// <param name="packItemName">套餐模板项名称</param>
        /// <returns>是否存在</returns>
        public bool ExistsName(Guid packId, Guid packSpaceId, Guid? packItemId, string packItemName)
        {
            IQueryable<DecorationPackItem> packItems = base.Find(x => x.PackId == packId && x.PackSpaceId == packSpaceId);

            if (packItemId != null)
            {
                DecorationPackItem current = this.Single(packItemId.Value);

                if (current.Name == packItemName)
                {
                    return false;
                }

                return packItems.Any(x => x.Name == packItemName);
            }

            return packItems.Any(x => x.Name == packItemName);
        }

        #endregion

        #region # 验证固定套餐是否可以上架 —— bool IsDecorationPackShelfed(Guid packId, IEnumerable<Guid> packSpaceIds)
        /// <summary>
        /// 验证固定套餐是否可以上架
        /// </summary>
        /// <param name="packId"></param>
        /// <param name="packSpaceIds"></param>
        /// <returns></returns>
        public bool IsDecorationPackShelfed(Guid packId, IEnumerable<Guid> packSpaceIds)
        {
            //套餐是否包含空间
            bool isShelfed = packSpaceIds.Any();
            if (isShelfed)
            {
                //空间是否包含选区
                IQueryable<DecorationPackItem> packItems = this.Find(s => s.PackId == packId && packSpaceIds.Contains(s.PackSpaceId));
                isShelfed = packSpaceIds.All(s => packItems.Select(t => t.PackSpaceId).Contains(s));
                if (isShelfed)
                {
                    //选区是否包含商品|工艺
                    isShelfed = packItems.All(x => x.PackSkus.Any() || x.PackCraftEntities.Any());
                }
            }
            return isShelfed;
        }
        #endregion


        //#region # 根据套餐模板获取套餐模板项列表 —— IEnumerable<DecorationPackItem> FindByPackIds(IEnumerable<Guid> packIds)
        ///// <summary>
        ///// 根据套餐模板获取套餐模板项列表
        ///// </summary>
        ///// <param name="packIds">套餐模板Id集</param>
        ///// <returns>套餐模板项列表</returns>
        //public IEnumerable<DecorationPackItem> FindByPackIds(IEnumerable<Guid> packIds)
        //{
        //    Expression<Func<DecorationPackItem, bool>> condition =
        //        x => packIds.Contains(x.PackId);

        //    return base.Find(condition).AsEnumerable();
        //}
        //#endregion


        #region # 根据套餐模板获取套餐模板项列表 —— Dictionary<Guid, IEnumerable<DecorationPackItem>> FindByPackIds(IEnumerable<Guid> packIds)

        /// <summary>
        /// 根据套餐模板获取套餐模板项列表
        /// </summary>
        /// <param name="packIds">套餐模板Id集</param>
        /// <returns>套餐Id|套餐模板项列表</returns>
        public Dictionary<Guid, IEnumerable<DecorationPackItem>> FindByPackIds(IEnumerable<Guid> packIds)
        {

            packIds = packIds ?? new Guid[0];
             Expression<Func<DecorationPackItem, bool>> condition =
                x => packIds.Contains(x.PackId);
            IEnumerable<DecorationPackItem> items= base.Find(condition).AsEnumerable();

            Dictionary<Guid, IEnumerable<DecorationPackItem>> result = new Dictionary<Guid, IEnumerable<DecorationPackItem>>();

            foreach (var packId in packIds)
            {
                result.Add(packId, items.Where(s => s.PackId == packId));
            }

            return result;
        }

        #endregion


        #region # 根据套餐选区Id集获取列表 ——IEnumerable<DecorationPackItem> FindByIds(IEnumerable<Guid> packItemIds)

        /// <summary>
        /// 根据套餐选区Id集获取列表
        /// </summary>
        /// <param name="packItemIds">套餐选区Id集</param>
        /// <returns>套餐选区列表</returns>
        public IEnumerable<DecorationPackItem> FindByIds(IEnumerable<Guid> packItemIds)
        {
            if (packItemIds.IsNullOrEmpty())
            {
                packItemIds = new List<Guid>();
            }
            return base.Find(s => packItemIds.Contains(s.Id)).Include(s=>s.PackSkus).Include(s => s.PackCraftEntities);
        }

        #endregion
    }
}
