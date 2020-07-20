using HDIPlatform.DesignSystem.Domain.Entities.BaleContext;
using HDIPlatform.DesignSystem.Domain.Entities.FixedMealContext;
using ShSoft.Infrastructure.RepositoryBase;
using System;
using System.Collections.Generic;
using HDIPlatform.DesignSystem.Domain.Entities.ConfigContext;
using HDIPlatform.DesignSystem.Domain.Entities.HouseTypePackContext;

namespace HDIPlatform.DesignSystem.Domain.IRepositories
{
    /// <summary>
    /// 工作单元 - 设计系统
    /// </summary>
    public interface IUnitOfWorkDesign : IUnitOfWork
    {
        #region # 根据工艺Id集获取套餐模板项列表 —— IEnumerable<DecorationPackItem> ResolvePackItemsByCraftIds(IEnumerable<Guid> craftIds); 
        /// <summary>
        /// 根据工艺Id集获取套餐模板项列表
        /// </summary>
        /// <param name="craftIds">工艺Id集</param>
        /// <returns>套餐模板项列表</returns>
        IEnumerable<DecorationPackItem> ResolvePackItemsByCraftIds(IEnumerable<Guid> craftIds);
        #endregion


        #region # 根据套餐模板获取套餐模板项列表 —— IEnumerable<DecorationPackItem> ResolvePackItemsByPack(...
        /// <summary>
        /// 根据套餐模板获取套餐模板项列表
        /// </summary>
        /// <param name="packId">套餐模板Id</param>
        /// <returns>套餐模板项列表</returns>
        IEnumerable<DecorationPackItem> ResolvePackItemsByPack(Guid packId);
        #endregion

        #region # 根据套餐模板获取套餐模板项列表 ——     Dictionary<Guid, IList<DecorationPackItem>> ResolvePackItemsByPacks(IEnumerable<Guid> packIds);
        /// <summary>
        /// 根据套餐模板集获取对应的套餐模板项列表
        /// </summary>
        /// <param name="packIds">套餐模板Id集</param>
        /// <returns>套餐模板项列表</returns>
        Dictionary<Guid, IEnumerable<DecorationPackItem>> ResolvePackItemsByPacks(IEnumerable<Guid> packIds);
        #endregion

        #region # 根据套餐模板获取套餐模板项列表 ——      IEnumerable<DecorationPackItem> ResolvePackItemsByIds(IEnumerable<Guid> packItemIds);
        /// <summary>
        /// 根据套餐模板获取套餐模板项列表
        /// </summary>
        /// <param name="packItemIds">套餐模板项Id集</param>
        /// <returns>套餐模板项列表</returns>
        IEnumerable<DecorationPackItem> ResolvePackItemsByIds(IEnumerable<Guid> packItemIds);
        #endregion

        #region # 根据套餐模板获取套餐模板方案列表 —— IEnumerable<DecorationPackScheme> ResolvePackSchemesByPack(...
        /// <summary>
        /// 根据套餐模板获取套餐模板方案列表
        /// </summary>
        /// <param name="packId">套餐模板Id</param>
        /// <returns>套餐模板方案列表</returns>
        IEnumerable<DecorationPackScheme> ResolvePackSchemesByPack(Guid packId);
        #endregion

        #region # 根据套餐模板获取套餐模板默认方案 ——  DecorationPackScheme ResolveDefaultPackSchemeByPack(Guid packId);
        /// <summary>
        /// 根据套餐模板获取套餐模板默认方案
        /// </summary>
        /// <param name="packId">套餐模板Id</param>
        /// <returns>套餐模板默认方案</returns>
        DecorationPackScheme ResolveDefaultPackSchemeByPack(Guid packId);
        #endregion

        #region # 根据套餐Id获取选区列表 —— IEnumerable<BalePackChoiceArea> ResolveAreaByPack(Guid packId)
        /// <summary>
        /// 根据套餐Id获取选区列表
        /// </summary>
        /// <param name="packId">套餐Id</param>
        /// <returns>选区列表</returns>
        IEnumerable<BalePackChoiceArea> ResolveAreaByPack(Guid packId);
        #endregion

        #region # 根据选区集获取组列表 —— IEnumerable<BalePackGroup> ResolveBalePackGroupByArea(IEnumerable<Guid> choiceAreaIds)
        /// <summary>
        /// 根据选区集获取组列表
        /// </summary>
        /// <param name="choiceAreaIds">选区Id集</param>
        /// <returns>组列表</returns>
        IEnumerable<BalePackGroup> ResolveBalePackGroupByArea(IEnumerable<Guid> choiceAreaIds);
        #endregion

        #region # 根据组Id获取组列表 —— IEnumerable<BalePackGroup> ResolveBalePackGroup(IEnumerable<Guid> groupIds)
        /// <summary>
        /// 根据组Id获取组列表
        /// </summary>
        /// <param name="groupIds">组Id</param>
        /// <returns>组列表</returns>
        IEnumerable<BalePackGroup> ResolveBalePackGroup(IEnumerable<Guid> groupIds);
        #endregion

        #region # 根据楼盘Id户型定价列表 —— IEnumerable<HouseTypePrice> ResolveHouseTypePrice(Guid propertyId)
        /// <summary>
        /// 根据楼盘Id户型定价列表
        /// </summary>
        /// <param name="propertyId">楼盘Id</param>
        /// <returns>户型定价列表</returns>
        IEnumerable<HouseTypePrice> ResolveHouseTypePrice(Guid propertyId);
        #endregion

        #region # 根据Id获取固定套餐集 —— IEnumerable<DecorationPack> GetDecorationPacks(IEnumerable<Guid> packIds)
        /// <summary>
        /// 根据Id获取固定套餐集
        /// </summary>
        /// <param name="packIds">套餐Id集</param>
        /// <returns>固定套餐集</returns>
        IEnumerable<DecorationPack> GetDecorationPacks(IEnumerable<Guid> packIds);
        #endregion

        #region # 根据套餐模板获取套餐模板项及导航属性列表 —— IEnumerable<DecorationPackItem> GetItemsByPackSku(...

        /// <summary>
        /// 根据套餐模板获取套餐模板项及导航属性列表
        /// </summary>
        /// <param name="packId">套餐模板Id</param>
        /// <returns>套餐模板项列表</returns>
        IEnumerable<DecorationPackItem> GetItemsByPackSku(Guid packId);

        #endregion

        #region # 根据套餐模板获取套餐模板推荐项及导航属性列表 —— IEnumerable<DecorationPackRecommendedItem> GetRecommendedItemsByPack(Guid packId);

        /// <summary>
        /// 根据套餐模板获取套餐模板推荐项及导航属性列表
        /// </summary>
        /// <param name="packId">套餐模板Id</param>
        /// <returns>套餐模板推荐项列表</returns>
        IEnumerable<DecorationPackRecommendedItem> GetRecommendedItemsByPack(Guid packId);

        #endregion

        #region # 根据套餐系列Id集获取套餐系列集 —— Dictionary<Guid,PackSeries> GetPackSeriesByIds(IEnumerable<Guid> ids);

        /// <summary>
        /// 根据套餐系列Id集获取套餐系列集
        /// </summary>
        /// <param name="ids">套餐系列Id集</param>
        /// <returns>套餐系列集</returns>
        Dictionary<Guid,PackSeries> GetPackSeriesByIds(IEnumerable<Guid> ids);

        #endregion


        #region # 根据套餐选区Id集获取列表 ——IEnumerable<DecorationPackItem> FindPackItemsByIds(IEnumerable<Guid> packItemIds)

        /// <summary>
        /// 根据套餐选区Id集获取列表
        /// </summary>
        /// <param name="packItemIds">套餐选区Id集</param>
        /// <returns>套餐选区列表</returns>
        IEnumerable<DecorationPackItem> FindPackItemsByIds(IEnumerable<Guid> packItemIds);

        #endregion
    }

}
