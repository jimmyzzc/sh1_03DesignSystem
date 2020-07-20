using HDIPlatform.DesignSystem.Domain.Entities.BaleContext;
using HDIPlatform.DesignSystem.Domain.Entities.FixedMealContext;
using HDIPlatform.DesignSystem.Domain.IRepositories;
using ShSoft.Infrastructure.Repository.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using HDIPlatform.DesignSystem.Domain.Entities.HouseTypePackContext;
using System.Data.Entity;
using System.Linq.Expressions;
using HDIPlatform.DesignSystem.Domain.Entities.ConfigContext;
using ShSoft.Common.PoweredByLee;

namespace HDIPlatform.DesignSystem.Repository.Base
{
    /// <summary>
    /// 工作单元 - 设计系统
    /// </summary>
    public sealed class UnitOfWork : EFUnitOfWorkProvider, IUnitOfWorkDesign
    {
        #region # 根据工艺Id集获取套餐模板项列表 —— IEnumerable<DecorationPackItem> ResolvePackItemsByCraftIds(IEnumerable<Guid> craftIds); 

        /// <summary>
        /// 根据工艺Id集获取套餐模板项列表
        /// </summary>
        /// <param name="craftIds">工艺Id集</param>
        /// <returns>套餐模板项列表</returns>
        public IEnumerable<DecorationPackItem> ResolvePackItemsByCraftIds(IEnumerable<Guid> craftIds)
        {
            return base.ResolveRange<DecorationPackItem>(x => x.PackCraftEntities.Select(s => s.CraftEntityId).Intersect(craftIds).Any())
                .Include(s => s.PackCraftEntities).AsEnumerable().DistinctBy(s => s.Id);
        }

        #endregion

        #region # 根据套餐模板获取套餐模板项列表 —— IEnumerable<DecorationPackItem> ResolvePackItemsByPack(...
        /// <summary>
        /// 根据套餐模板获取套餐模板项列表
        /// </summary>
        /// <param name="packId">套餐模板Id</param>
        /// <returns>套餐模板项列表</returns>
        public IEnumerable<DecorationPackItem> ResolvePackItemsByPack(Guid packId)
        {
            return base.ResolveRange<DecorationPackItem>(x => x.PackId == packId).AsEnumerable();
        }
        #endregion

        #region # 根据套餐模板获取套餐模板项列表 ——     Dictionary<Guid, IList<DecorationPackItem>> ResolvePackItemsByPacks(IEnumerable<Guid> packIds);

        /// <summary>
        /// 根据套餐模板集获取对应的套餐模板项列表
        /// </summary>
        /// <param name="packIds">套餐模板Id集</param>
        /// <returns>套餐模板项列表</returns>
        public Dictionary<Guid, IEnumerable<DecorationPackItem>> ResolvePackItemsByPacks(IEnumerable<Guid> packIds)
        {
            Dictionary < Guid, IEnumerable< DecorationPackItem >> result = new Dictionary<Guid, IEnumerable<DecorationPackItem>>();
            var items = base.ResolveRange<DecorationPackItem>(x => packIds.Contains(x.PackId));
            foreach (var packId in packIds)
            {
                result.Add(packId, items.Where(s => s.PackId == packId).AsEnumerable());
            }

            return result;
        }
        #endregion

        #region # 根据套餐模板获取套餐模板项列表 ——      IEnumerable<DecorationPackItem> ResolvePackItemsByIds(IEnumerable<Guid> packItemIds);

        /// <summary>
        /// 根据套餐模板获取套餐模板项列表
        /// </summary>
        /// <param name="packItemIds">套餐模板项Id集</param>
        /// <returns>套餐模板项列表</returns>
        public IEnumerable<DecorationPackItem> ResolvePackItemsByIds(IEnumerable<Guid> packItemIds)
        {
            return base.ResolveRange<DecorationPackItem>(x => packItemIds.Contains(x.Id)).Include(s => s.PackCraftEntities).Include(s => s.PackSkus).AsEnumerable();
        }
        #endregion


        #region # 根据套餐模板获取套餐模板方案列表 —— IEnumerable<DecorationPackScheme> ResolvePackSchemesByPack(...
        /// <summary>
        /// 根据套餐模板获取套餐模板方案列表
        /// </summary>
        /// <param name="packId">套餐模板Id</param>
        /// <returns>套餐模板方案列表</returns>
        public IEnumerable<DecorationPackScheme> ResolvePackSchemesByPack(Guid packId)
        {
            return base.ResolveRange<DecorationPackScheme>(x => x.PackId == packId).AsEnumerable();
        }
        #endregion

        #region # 根据套餐模板获取套餐模板默认方案 ——  DecorationPackScheme ResolveDefaultPackSchemeByPack(Guid packId);

        /// <summary>
        /// 根据套餐模板获取套餐模板默认方案
        /// </summary>
        /// <param name="packId">套餐模板Id</param>
        /// <returns>套餐模板默认方案</returns>
        public DecorationPackScheme ResolveDefaultPackSchemeByPack(Guid packId)
        {
            return base.ResolveRange<DecorationPackScheme>(x => x.PackId == packId && x.IsDefault).FirstOrDefault();
        }

        #endregion

        #region # 根据套餐Id获取选区列表 —— IEnumerable<BalePackChoiceArea> ResolveAreaByPack(Guid packId)
        /// <summary>
        /// 根据套餐Id获取选区列表
        /// </summary>
        /// <param name="packId">套餐Id</param>
        /// <returns>选区列表</returns>
        public IEnumerable<BalePackChoiceArea> ResolveAreaByPack(Guid packId)
        {
            return base.Resolve<BalePack>(packId).ChoiceAreas.AsEnumerable();
        }
        #endregion

        #region # 根据选区集获取组列表 —— IEnumerable<BalePackGroup> ResolveBalePackGroupByArea(IEnumerable<Guid> choiceAreaIds)
        /// <summary>
        /// 根据选区集获取组列表
        /// </summary>
        /// <param name="choiceAreaIds">选区Id集</param>
        /// <returns>组列表</returns>
        public IEnumerable<BalePackGroup> ResolveBalePackGroupByArea(IEnumerable<Guid> choiceAreaIds)
        {
            return base.ResolveRange<BalePackGroup>(s => choiceAreaIds.Contains(s.ChoiceAreaId)).AsEnumerable();
        }
        #endregion

        #region # 根据组Id获取组列表 —— IEnumerable<BalePackGroup> ResolveBalePackGroup(IEnumerable<Guid> groupIds)
        /// <summary>
        /// 根据组Id获取组列表
        /// </summary>
        /// <param name="groupIds">组Id</param>
        /// <returns>组列表</returns>
        public IEnumerable<BalePackGroup> ResolveBalePackGroup(IEnumerable<Guid> groupIds)
        {
            return base.ResolveRange<BalePackGroup>(s => groupIds.Contains(s.Id)).AsEnumerable();
        }
        #endregion

        #region # 根据楼盘Id户型定价列表 —— IEnumerable<HouseTypePrice> ResolveHouseTypePrice(Guid propertyId)
        /// <summary>
        /// 根据楼盘Id户型定价列表
        /// </summary>
        /// <param name="propertyId">楼盘Id</param>
        /// <returns>户型定价列表</returns>
        public IEnumerable<HouseTypePrice> ResolveHouseTypePrice(Guid propertyId)
        {
            return base.ResolveRange<HouseTypePrice>(s => s.PropertyId == propertyId).AsEnumerable();
        }

        #endregion

        #region # 根据Id获取固定套餐集 —— IEnumerable<DecorationPack> GetDecorationPacks(IEnumerable<Guid> packIds)

        /// <summary>
        /// 根据Id获取固定套餐集
        /// </summary>
        /// <param name="packIds">套餐Id集</param>
        /// <returns>固定套餐集</returns>
        public IEnumerable<DecorationPack> GetDecorationPacks(IEnumerable<Guid> packIds)
        {
            return base.ResolveRange<DecorationPack>(s => packIds.Contains(s.Id)).AsEnumerable();
        }

        #endregion

        #region # 根据套餐模板获取套餐模板项及导航属性列表 —— IEnumerable<DecorationPackItem> GetItemsByPackSku(...
        /// <summary>
        /// 根据套餐模板获取套餐模板项及导航属性列表
        /// </summary>
        /// <param name="packId">套餐模板Id</param>
        /// <returns>套餐模板项列表</returns>
        public IEnumerable<DecorationPackItem> GetItemsByPackSku(Guid packId)
        {
            return base.ResolveRange<DecorationPackItem>(x => x.PackId == packId).Include(x => x.PackSkus).Include(x => x.PackCraftEntities).AsEnumerable();
           
        }
        #endregion

        #region # 根据套餐模板获取套餐模板推荐项及导航属性列表 —— IEnumerable<DecorationPackRecommendedItem> GetRecommendedItemsByPack(Guid packId);

        /// <summary>
        /// 根据套餐模板获取套餐模板推荐项及导航属性列表
        /// </summary>
        /// <param name="packId">套餐模板Id</param>
        /// <returns>套餐模板推荐项列表</returns>
        public IEnumerable<DecorationPackRecommendedItem> GetRecommendedItemsByPack(Guid packId)
        {
            return base.ResolveRange<DecorationPackRecommendedItem>(x => x.PackId == packId).Include(x => x.PackRecommendedSkus).AsEnumerable();
        }

        #endregion

        #region # 根据套餐系列Id集获取套餐系列集 —— Dictionary<Guid,PackSeries> GetPackSeriesByIds(IEnumerable<Guid> ids);

        /// <summary>
        /// 根据套餐系列Id集获取套餐系列集
        /// </summary>
        /// <param name="ids">套餐系列Id集</param>
        /// <returns>套餐系列集</returns>
        public Dictionary<Guid, PackSeries> GetPackSeriesByIds(IEnumerable<Guid> ids)
        {
            if (ids.IsNullOrEmpty())
            {
                return new Dictionary<Guid, PackSeries>();
            }
            ids = ids.Distinct();
            Dictionary<Guid, PackSeries> result = new Dictionary<Guid, PackSeries>();
            Expression<Func<PackSeries, bool>> query = s => ids.Contains(s.Id);
            IEnumerable<PackSeries> packSeries = base.ResolveRange<PackSeries>(query);
            foreach (var id in ids)
            {
                result.Add(id, packSeries.SingleOrDefault(s=>s.Id == id));
            }

            return result;
        }

        #endregion


        #region # 根据套餐选区Id集获取列表 ——IEnumerable<DecorationPackItem> FindPackItemsByIds(IEnumerable<Guid> packItemIds)

        /// <summary>
        /// 根据套餐选区Id集获取列表
        /// </summary>
        /// <param name="packItemIds">套餐选区Id集</param>
        /// <returns>套餐选区列表</returns>
        public IEnumerable<DecorationPackItem> FindPackItemsByIds(IEnumerable<Guid> packItemIds)
        {
            if (packItemIds.IsNullOrEmpty())
            {
                packItemIds = new List<Guid>();
            }
            return base.ResolveRange<DecorationPackItem>(s => packItemIds.Contains(s.Id)).Include(s => s.PackSkus).Include(s => s.PackCraftEntities);
        }

        #endregion
    }
}
