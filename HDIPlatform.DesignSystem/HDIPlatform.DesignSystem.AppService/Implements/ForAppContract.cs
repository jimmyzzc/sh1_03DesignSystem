
using HDIPlatform.DesignSystem.Domain.IRepositories;
using HDIPlatform.DesignSystem.Domain.Mediators;
using HDIPlatform.DesignSystem.IAppService.Interfaces;
using ShSoft.Common.PoweredByLee;
using ShSoft.Infrastructure.Global.Transaction;
using ShSoft.ValueObjects.Enums;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.ServiceModel;
using HDIPlatform.DesignSystem.AppService.Maps;
using HDIPlatform.DesignSystem.Domain.Entities.FixedMealContext;
using HDIPlatform.DesignSystem.Domain.Entities.HouseTypePackContext;
using HDIPlatform.DesignSystem.IAppService.DTOs.Inputs;
using HDIPlatform.DesignSystem.IAppService.DTOs.Outputs.FixedMealContext;
using HDIPlatform.DesignSystem.IAppService.DTOs.Outputs.ForAppContext;
using HDIPlatform.DesignSystem.IAppService.DTOs.Outputs.HouseTypePackContext;
using HDIPlatform.MarketSystem.IAppService.Interfaces;
using HDIPlatform.ResourceService.IAppService.Interfaces;
using ShSoft.Infrastructure.DTOBase;

namespace HDIPlatform.DesignSystem.AppService.Implements
{
    /// <summary>
    /// App服务契约实现
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class ForAppContract : IForAppContract
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
        /// 客户模型服务契约
        /// </summary>
        private readonly ICustomerModelContract _customerModelContract;
        /// <summary>
        /// 商品资源服务契约
        /// </summary>
        private readonly IProductContract _productContract;
        /// <summary>
        /// 工艺资源服务契约
        /// </summary>
        private readonly ICraftEntityContract _craftEntityContract;

        /// <summary>
        /// 依赖注入构造器
        /// </summary>
        /// <param name="svcMediator">领域服务中介者</param>
        /// <param name="repMediator">仓储中介者</param>
        /// <param name="unitOfWork">单元事务</param>
        /// <param name="customerModelContract">客户模型服务契约</param>
        /// <param name="productContract">商品资源服务契约</param>
        /// <param name="craftEntityContract">工艺资源服务契约</param>
        public ForAppContract(DomainServiceMediator svcMediator, RepositoryMediator repMediator, IUnitOfWorkDesign unitOfWork, ICustomerModelContract customerModelContract, IProductContract productContract, ICraftEntityContract craftEntityContract)
        {
            this._svcMediator = svcMediator;
            this._repMediator = repMediator;
            this._unitOfWork = unitOfWork;
            _customerModelContract = customerModelContract;
            _productContract = productContract;
            _craftEntityContract = craftEntityContract;
        }

        #endregion

        #region 获取已使用的套餐标签 --     IEnumerable<App_LableInfo> GetDecorationPackLables();

        /// <summary>
        /// 获取已使用的套餐标签
        /// </summary>
        public App_LableInfo GetDecorationPackLables()
        {
            App_LableInfo lableInfo = new App_LableInfo();
            lableInfo.Layouts = _repMediator.DecorationPackRep.GetDecorationPacksLayouts().ToList();
            var styleNos = _repMediator.DecorationPackRep.GetDecorationPacksStyles().ToList();
            lableInfo.Styles = styleNos.Any()
                ? _customerModelContract.GetStyles().Where(s => styleNos.Any(t => t.Contains(s.Number))).ToDictionary(k => k.Number, v => v.Name)
                : new Dictionary<string, string>();
            lableInfo.PackSeries = _repMediator.DecorationPackRep.GetDecorationPackSeries();
            lableInfo.PackGroupSeries = _repMediator.DecorationPackRep.GetDecorationPackGroupSeries().OrderBy(x => x.Item2).ToList();

            //楼盘-户型集
            var houseTypes = this._repMediator.DecorationPackRep.GetDecorationPackHouseTypes().ToList();
            //packs.SelectMany(x => x.HouseTypes).DistinctBy(x => x.Item1).ToList();
            ////楼盘Id-户型集（户型Id|户型名称）
            //Dictionary<Guid, Dictionary<Guid, string>> dic = houseTypes.GroupBy(x => x.Item3).ToDictionary(x => x.Key, x => x.ToDictionary(v => v.Item1, v => v.Item2));
            ////楼盘Id|楼盘名称
            //Dictionary<Guid, string> propertys = houseTypes.GroupBy(x => x.Item3).ToDictionary(x => x.Key, x => x.First().Item4);

            lableInfo.HouseTypes = houseTypes;
            lableInfo.PHouseTypes = houseTypes.Any()
                ? houseTypes.GroupBy(x => x.Item3).ToDictionary(x => x.Key, x => x.ToDictionary(v => v.Item1, v => v.Item2))
                : new Dictionary<Guid, Dictionary<Guid, string>>();
            var propertys = this._repMediator.DecorationPackRep.GetDecorationPacksPropertys();
            lableInfo.Propertys = propertys.Any() ? propertys : new Dictionary<Guid, string>();

            //houseTypes.Any()
            //? houseTypes.GroupBy(x => x.Item3).ToDictionary(x => x.Key, x => x.First().Item4)
            //: new Dictionary<Guid, string>();
            return lableInfo;
        }

        #endregion

        #region 根据条件分页获取套餐 --  PageModel<App_DecorationPackInfo> GetDecorationPacks(string keywords, bool hasPackSeries,bool isBuildSuqare, IEnumerable<Guid> packSeriesIds, IEnumerable<string> layouts, IEnumerable<string> styleNos,float? minSquare,float? maxSquare,int pageIndex,int PageSize);

        /// <summary>
        /// 根据条件分页获取套餐
        /// </summary>
        public PageModel<App_DecorationPackInfo> GetDecorationPacks(string keywords, bool hasPackSeries, bool isBuildSuqare, IEnumerable<Guid> packSeriesIds, IEnumerable<string> layouts,
            IEnumerable<string> styleNos,
            float? minSquare, float? maxSquare, Dictionary<string, bool> sort, int pageIndex, int pageSize)
        {
            int rowCount, pageCount;
            var decorationPacks = _repMediator.DecorationPackRep.FindByPage(keywords, null, null, null, null, null, null, isBuildSuqare, minSquare, maxSquare, null, null, null, null, null, null,
                ShelfStatus.Shelfed, null, styleNos.ToList(), layouts.ToList(), null, hasPackSeries, packSeriesIds.ToList(), null, true, null, sort, pageIndex,
                pageSize, out rowCount, out pageCount);
            //查询套餐默认方案
            List<Guid> packIds = decorationPacks.Select(x => x.Id).ToList();
            List<DecorationPackScheme> packSchemes = this._repMediator.DecorationPackSchemeRep.FindDefaultByPack(packIds).ToList();
            IEnumerable<App_DecorationPackInfo> decorationPackInfos = decorationPacks.Select(x => x.ToAppDTO(packSchemes));
            return new PageModel<App_DecorationPackInfo>(decorationPackInfos, pageIndex, pageSize, pageCount, rowCount);
        }

        #endregion

        #region 分页获取推荐套餐 --  PageModel<App_DecorationPackInfo> GetRecommendDecorationPacks(Guid propertyId, Guid? houseTypeId, float? minSquare, float? maxSquare, decimal? minPrice, decimal? maxPrice, int pageIndex, int pageSize);

        /// <summary>
        /// 分页获取推荐套餐
        /// </summary>
        public PageModel<App_DecorationPackInfo> GetRecommendDecorationPacks(Guid propertyId, Guid? houseTypeId, float? minSquare, float? maxSquare, decimal? minPrice,
            decimal? maxPrice, int pageIndex, int pageSize)
        {
            int rowCount, pageCount;
            var decorationPacks = _repMediator.DecorationPackRep.FindByCondition(propertyId, houseTypeId, minPrice, maxPrice, minSquare, maxSquare, null, pageIndex,
                pageSize, out rowCount, out pageCount);
            //查询套餐默认方案
            List<Guid> packIds = decorationPacks.Select(x => x.Id).ToList();
            List<DecorationPackScheme> packSchemes = this._repMediator.DecorationPackSchemeRep.FindDefaultByPack(packIds).ToList();
            IEnumerable<App_DecorationPackInfo> decorationPackInfos = decorationPacks.Select(x => x.ToAppDTO(packSchemes));
            return new PageModel<App_DecorationPackInfo>(decorationPackInfos, pageIndex, pageSize, pageCount, rowCount);
        }
        #endregion

        #region 获取套餐明细 --  IEnumerable<App_DecorationPackItemGroupInfo> GetPackDetails(Guid packId);

        /// <summary>
        /// 获取套餐明细
        /// </summary>
        public IEnumerable<App_DecorationPackItemGroupInfo> GetPackDetails(Guid packId)
        {
            var result = new List<App_DecorationPackItemGroupInfo>();
            var packItems = this._repMediator.DecorationPackItemRep.FindByPack(packId, null).ToList();
            var packRecommendedItems = this._repMediator.DecorationPackRecommendedItemRep.FindByPack(packId, null).ToList();
            var packSpaces = this._repMediator.DecorationPackSpaceRep.FindByPack(packId).ToList();

            var craftEntities = this._craftEntityContract.GetCraftEntitiesByIds(packItems.SelectMany(s => s.PackCraftEntities).Where(x => x.Default).Select(s => s.CraftEntityId).Distinct()).ToList();
            var productSkus = this._productContract.GetProductSkusByIds(packItems.SelectMany(s => s.PackSkus).Where(x => x.Default).Select(s => s.SkuId)
                .Union(packRecommendedItems.SelectMany(s => s.PackRecommendedSkus).Where(s => s.Default && s.SkuId.HasValue).Select(s => s.SkuId.Value)).Distinct()
                .ToList()).ToList();


            foreach (var packItem in packItems)
            {
                var group = result.SingleOrDefault(s => s.Name == packItem.Name && !s.IsRecommended);
                if (group == null)
                {
                    group = new App_DecorationPackItemGroupInfo(packItem.Name);
                    result.Add(group);
                }

                foreach (var decorationPackSku in packItem.PackSkus.Where(x => x.Default).OrderBy(s => s.Sort))
                {
                    var skuInfo = productSkus.SingleOrDefault(s => s.Id == decorationPackSku.SkuId);
                    if (skuInfo == null)
                    {
                        throw new DataException("套餐中存在异常商品数据 , 请与管理员联系 ! "); 
                    }
                    group.SkuInfos.Add(decorationPackSku.ToAppDTO(skuInfo, packSpaces));
                }

                foreach (var packCraftEntity in packItem.PackCraftEntities.Where(x => x.Default).OrderBy(s => s.Sort))
                {
                    var craftEntityInfo = craftEntities.SingleOrDefault(s => s.Id == packCraftEntity.CraftEntityId);
                    if (craftEntityInfo == null)
                    {
                        throw new DataException("套餐中存在异常工艺数据 , 请与管理员联系 ! ");
                    }
                    group.CraftInfos.Add(packCraftEntity.ToAppDTO(craftEntityInfo, packSpaces));
                }

                group.SkuInfos = group.SkuInfos.OrderBy(s => s.Sort).ThenByDescending(s => s.Name).ToList();
                group.CraftInfos = group.CraftInfos.OrderBy(s => s.Sort).ThenByDescending(s => s.Name).ToList();
            }


            foreach (var packRecommendedItem in packRecommendedItems)
            {
                var group = result.SingleOrDefault(s => s.Name == packRecommendedItem.Name && s.IsRecommended);
                if (group == null)
                {
                    group = new App_DecorationPackItemGroupInfo(packRecommendedItem.Name);
                    group.IsRecommended = true;
                    result.Add(group);
                }

                foreach (var decorationPackSku in packRecommendedItem.PackRecommendedSkus.Where(x => x.Default).OrderBy(s => s.Sort))
                {
                    var skuInfo = productSkus.SingleOrDefault(s => s.Id == decorationPackSku.SkuId);
                    if (skuInfo == null)
                    {
                        throw new DataException("套餐中存在异常商品数据 , 请与管理员联系 ! ");
                    }
                    group.SkuInfos.Add(decorationPackSku.ToAppDTO(skuInfo, packSpaces));
                }
                group.SkuInfos = group.SkuInfos.OrderBy(s => s.Sort).ThenByDescending(s => s.Name).ToList();
            }

            return result;
        }

        #endregion

        #region # 获取套餐空间效果图（默认方案） —— IEnumerable<App_DecorationPackSpaceImageInfo> GetPackSpaceImageInfos(Guid packId)

        /// <summary>
        /// 获取套餐空间效果图（默认方案）
        /// </summary>
        /// <param name="packId">套餐Id</param>
        /// <returns>套餐空间默认方案图册集</returns>
        public IEnumerable<App_DecorationPackSpaceImageInfo> GetPackSpaceImageInfos(Guid packId)
        {
            //空间
            IEnumerable<DecorationPackSpace> packSpaces = this._repMediator.DecorationPackSpaceRep.FindByPack(packId).ToList();
            //套餐默认方案
            DecorationPackScheme packScheme = this._repMediator.DecorationPackSchemeRep.GetDefaultByPack(packId);
            List<DecorationPackSchemeSpace> packSchemeSpaces = packScheme != null ? packScheme.SchemeSpaces.ToList() : new List<DecorationPackSchemeSpace>();
            //转DTO
            IEnumerable<App_DecorationPackSpaceImageInfo> result = packSpaces.Select(x => x.ToAppDTO(packSchemeSpaces));
            return result;
        }

        #endregion
        #region # 根据条件分页获取套餐 --  PageModel<App_DecorationPackInfo> GetDecorationPacksForMarketApp(string keywords, bool containsNoProperty, IList<Guid> propertys, Guid? propertyId, Guid? houseTypeId, bool hasPackSeries, IEnumerable<Guid> packSeriesIds, IEnumerable<string> styleNos, bool? isBuildSuqare, float? minSquare, float? maxSquare, Dictionary<string, bool> sort, int pageIndex, int pageSize)

        /// <summary>
        /// 根据条件分页获取套餐
        /// </summary>
        /// <param name="keywords">关键字</param>
        /// <param name="propertys">楼盘标签集（查询包含此集合标签）</param>
        /// <param name="containsNoProperty">是否包含无楼盘标签套餐（true查询）</param>
        /// <param name="propertyId">楼盘标签（查询包含此楼盘标签）</param>
        /// <param name="houseTypeId">户型标签</param>
        /// <param name="hasPackSeries">是否包含系列标签(全部,标签:true,其他:false)</param>
        /// <param name="packSeriesIds">系列标签集</param>
        /// <param name="styleNos">风格标签集</param>
        /// <param name="isBuildSuqare">是否是建筑面积搜索</param>
        /// <param name="minSquare">最小面积</param>
        /// <param name="maxSquare">最大面积</param>
        /// <param name="sort">排序</param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public PageModel<App_DecorationPackInfo> GetDecorationPacksForMarketApp(string keywords, bool containsNoProperty, IList<Guid> propertys, Guid? propertyId, Guid? houseTypeId, bool hasPackSeries, IEnumerable<Guid> packSeriesIds, IEnumerable<string> styleNos, bool? isBuildSuqare, float? minSquare, float? maxSquare, Dictionary<string, bool> sort, int pageIndex, int pageSize)
        {
            int rowCount, pageCount;
            var decorationPacks = _repMediator.DecorationPackRep.FindByCondition(keywords, containsNoProperty, propertys, propertyId, houseTypeId, hasPackSeries, packSeriesIds, styleNos, isBuildSuqare, minSquare, maxSquare, sort, pageIndex,
                pageSize, out rowCount, out pageCount);
            //查询套餐默认方案
            List<Guid> packIds = decorationPacks.Select(x => x.Id).ToList();
            List<DecorationPackScheme> packSchemes = this._repMediator.DecorationPackSchemeRep.FindDefaultByPack(packIds).ToList();
            IEnumerable<App_DecorationPackInfo> decorationPackInfos = decorationPacks.Select(x => x.ToAppDTO(packSchemes));
            return new PageModel<App_DecorationPackInfo>(decorationPackInfos, pageIndex, pageSize, pageCount, rowCount);
        }

        #endregion

        #region # 根据Id获取固定套餐集 —— IEnumerable<App_DecorationPackInfo> GetDecorationPacksByIds(List<Guid> packIds)
        /// <summary>
        /// 根据Id获取固定套餐集
        /// </summary>
        /// <param name="packIds">固定套餐Id集</param>
        /// <returns></returns>
        public IEnumerable<App_DecorationPackInfo> GetDecorationPacksByIds(List<Guid> packIds)
        {
            IEnumerable<DecorationPack> decorationPacks = _repMediator.DecorationPackRep.GetDecorationPacksByPackIds(packIds);
            //查询套餐默认方案
            List<DecorationPackScheme> packSchemes = this._repMediator.DecorationPackSchemeRep.FindDefaultByPack(packIds).ToList();
            IEnumerable<App_DecorationPackInfo> decorationPackInfos = decorationPacks.Select(x => x.ToAppDTO(packSchemes));
            return decorationPackInfos;
        }

        #endregion



        #region # 测试 Tuple —— void Test(Guid id)
        /// <summary>

        /// 测试 Tuple
        /// </summary>
        /// <param name="id"></param>
        public void Test(Guid id)
        {

            //App_LableInfo lableInfo = this.GetDecorationPackLables();

            IEnumerable<App_DecorationPackSpaceImageInfo> p = GetPackSpaceImageInfos(id);
        }
        #endregion
    }
}
