using HDIPlatform.DesignSystem.Domain.Entities.FixedMealContext;
using HDIPlatform.DesignSystem.Domain.IRepositories.FixedMealContext;
using SD.Toolkits.EntityFramework.Extensions;
using ShSoft.Infrastructure.Repository.EntityFramework;
using ShSoft.Infrastructure.RepositoryBase;
using ShSoft.ValueObjects.Enums;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using ShSoft.Common.PoweredByLee;
using HDIPlatform.DesignSystem.Repository.Base;
using System.Data.SqlClient;

namespace HDIPlatform.DesignSystem.Repository.Implements.FixedMealContext
{
    /// <summary>
    /// 套餐模板仓储实现
    /// </summary>
    public class DecorationPackRepository : EFRepositoryProvider<DecorationPack>, IDecorationPackRepository
    {
        /// <summary>
        /// EF上下文对象
        /// </summary>
        private readonly DbContext _dbContext;

        public DecorationPackRepository()
        {
            this._dbContext = DbSession.CommandInstance;
        }

        #region # 获取实体对象集合 —— override IQueryable<DecorationPack> FindAllInner()
        /// <summary>
        /// 获取实体对象集合
        /// </summary>
        /// <returns>实体对象集合</returns>
        protected override IQueryable<DecorationPack> FindAllInner()
        {
            return base.FindAllInner().OrderBy(x => x.Sort);
        }
        #endregion

        #region # 分页获取套餐模板列表 —— IEnumerable<DecorationPack> FindByPage(string keywords, decimal? minPrice...

        /// <summary>
        /// 分页获取套餐模板列表(排除克隆版本套餐)
        /// </summary>
        /// <param name="keywords">关键字</param>
        /// <param name="propertyId">楼盘Id</param>
        /// <param name="applicableSquare">适用面积</param>
        /// <param name="isNewHouse">是否适用新房</param>
        /// <param name="isBuildingPrice">是否是建筑价格搜索</param>
        /// <param name="minPrice">最小价格</param>
        /// <param name="maxPrice">最大价格</param>
        /// <param name="isBuildingSquare">是否是建筑面积搜索</param>
        /// <param name="minSquare">最小面积</param>
        /// <param name="maxSquare">最大面积</param>
        /// <param name="hasOffSku">是否有下架商品</param>
        /// <param name="hasChangedSku">是否有变价商品|工艺</param>
        /// <param name="color">颜色</param>
        /// <param name="packType">套餐类型</param>
        /// <param name="packKind">套餐类别</param>
        /// <param name="packMode">套餐模式</param>
        /// <param name="status">状态</param>
        /// <param name="spaceTypes">空间类型集</param>
        /// <param name="styleNos">设计风格集</param>
        /// <param name="layouts">居室集</param>
        /// <param name="propertys">楼盘集</param>
        /// <param name="hasPackSeries"></param>
        /// <param name="packSeriesIds">套餐系列集</param>
        /// <param name="stores">门店集</param>
        /// <param name="hasStores">是否有门店（全部,标签:true,其他:false）</param>
        /// <param name="createrId">套餐创建人Id？（全部：null,我的：当前登录人员）</param>
        /// <param name="sort">排序条件 false为倒序</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页容量</param>
        /// <param name="rowCount">总记录条数</param>
        /// <param name="pageCount">总页数</param>
        /// <returns>套餐模板列表</returns>
        public IEnumerable<DecorationPack> FindByPage(string keywords, Guid? propertyId, float? applicableSquare, bool? isNewHouse, bool? isBuildingPrice, decimal? minPrice, decimal? maxPrice, bool? isBuildingSquare,
            float? minSquare, float? maxSquare, bool? hasOffSku, bool? hasChangedSku, DecorationPackColor? color, DecorationPackType? packType,
            DecorationPackKind? packKind, DecorationPackMode? packMode, ShelfStatus? status, IList<string> spaceTypes, IList<string> styleNos, IList<string> layouts,
            IList<Guid> propertys, bool hasPackSeries, IList<Guid> packSeriesIds, IList<Guid> stores, bool hasStores, Guid? createrId, Dictionary<string, bool> sort, int pageIndex, int pageSize, out int rowCount, out int pageCount)
        {

            IQueryable<DecorationPack> packs = this.FindAllInner();
            packs = packs.Where(x => (status == null || x.Status == status) && x.SourcePackId == null);
            //需求文档要求,如果楼盘条件查询不到,则过滤此条件
            var hasProperties = packs.Any(s => propertyId == null || !string.IsNullOrEmpty(s.PropertysStr) && s.PropertysStr.Contains(propertyId.ToString()));

            #region # 标签处理


            //空间类型标签
            if (spaceTypes != null && spaceTypes.Any())
            {
                PredicateBuilder<DecorationPack> builder = new PredicateBuilder<DecorationPack>(x => false);
                foreach (string spaceType in spaceTypes)
                {
                    string sType = spaceType;

                    builder.Or(x => x.SpaceTypesStr.Contains(sType));
                }

                packs = packs.Where(builder.Build());
            }

            //设计风格标签
            if (styleNos != null && styleNos.Any())
            {
                PredicateBuilder<DecorationPack> builder = new PredicateBuilder<DecorationPack>(x => false);
                foreach (string styleNo in styleNos)
                {
                    string sStyleNo = styleNo;

                    builder.Or(x => x.StyleNosStr.Contains(sStyleNo));
                }

                packs = packs.Where(builder.Build());
            }

            //居室标签
            if (layouts != null && layouts.Any())
            {
                PredicateBuilder<DecorationPack> builder = new PredicateBuilder<DecorationPack>(x => false);
                foreach (string layout in layouts)
                {
                    string sLayout = layout;

                    builder.Or(x => x.LayoutsStr.Contains(sLayout));
                }

                packs = packs.Where(builder.Build());
            }

            //楼盘标签
            if (propertys != null && propertys.Any())
            {
                PredicateBuilder<DecorationPack> builder = new PredicateBuilder<DecorationPack>(x => false);
                foreach (Guid property in propertys)
                {
                    string sProperty = property.ToString();

                    builder.Or(x => x.PropertysStr.Contains(sProperty));
                }

                packs = packs.Where(builder.Build());
            }

            if (hasStores)
            {
                //门店标签
                if (stores != null && stores.Any())
                {
                    PredicateBuilder<DecorationPack> builder = new PredicateBuilder<DecorationPack>(x => false);
                    foreach (Guid store in stores)
                    {
                        string sStore = store.ToString();

                        builder.Or(x => x.StoresStr.Contains(sStore));
                    }

                    packs = packs.Where(builder.Build());
                }
            }
            else
            {
                packs = packs.Where(x => string.IsNullOrEmpty(x.StoresStr));
            }

            if (hasPackSeries)
            {
                if (packSeriesIds != null && packSeriesIds.Any())
                {
                    packs = packs.Where(s => s.PackSeries.Select(t => t.Id).Intersect(packSeriesIds).Any());
                }
            }
            else
            {
                packs = packs.Where(s => !s.PackSeries.Any());
            }
            #endregion


            #region  建筑面积  || 使用面积  价格搜索
            //2018-07-23  学梅确认, 价格搜索按照定价方式里面的建筑面积|使用面积勾选进行查询；建筑面积-（是否整体建筑定价|是否平米整体建筑定价为true）
            if (minPrice != null || maxPrice != null)
            {
                if (isBuildingPrice.Value)
                {

                    if (minPrice != null && maxPrice == null)
                    {
                        packs = packs.Where(x => (x.BuildingTotalPrice >= minPrice && x.IsBuilding) || (x.UnitBuildingTotalPrice >= minPrice && x.IsUnitBuilding));

                    }
                    else if (minPrice == null && maxPrice != null)
                    {
                        packs = packs.Where(x => (x.BuildingTotalPrice <= maxPrice && x.IsBuilding) || (x.UnitBuildingTotalPrice <= maxPrice && x.IsUnitBuilding));

                    }
                    else if (minPrice != null && maxPrice != null)
                    {
                        packs = packs.Where(x =>
                            (x.BuildingTotalPrice >= minPrice && x.BuildingTotalPrice <= maxPrice && x.IsBuilding) ||
                            (x.UnitBuildingTotalPrice >= minPrice && x.UnitBuildingTotalPrice <= maxPrice && x.IsUnitBuilding));

                    }

                }
                else
                {

                    if (minPrice != null && maxPrice == null)
                    {
                        packs = packs.Where(x => (x.TotalPrice >= minPrice && x.IsActual) || (x.UnitTotalPrice >= minPrice && x.IsUnitActual));

                    }
                    else if (minPrice == null && maxPrice != null)
                    {
                        packs = packs.Where(x => (x.TotalPrice <= maxPrice && x.IsActual) || (x.UnitTotalPrice <= maxPrice && x.IsUnitActual));

                    }
                    else if (minPrice != null && maxPrice != null)
                    {
                        packs = packs.Where(x => (x.TotalPrice >= minPrice && x.TotalPrice <= maxPrice && x.IsActual) ||
                            (x.UnitTotalPrice >= minPrice && x.UnitTotalPrice <= maxPrice && x.IsUnitActual));

                    }

                }
            }

            #endregion

            #region 建筑面积 || 使用面积 面积搜索

            if (minSquare != null && maxSquare != null)
            {
                if (isBuildingSquare.Value)
                {
                    //2018-07-05  学梅确认, 平米定价-最低购买面积判断  ,   整体定价-使用面积
                    packs = packs.Where(x =>
                        !x.PricingType.HasValue ||
                        (x.PricingType == DecorationPackPricingType.Total && (minSquare == null || x.BuildingSquare >= minSquare) && (maxSquare == null || x.BuildingSquare <= maxSquare)
                        || x.PricingType == DecorationPackPricingType.Unit && (minSquare == null || x.UnitBuildingSquare >= minSquare) && (maxSquare == null || x.UnitBuildingSquare <= maxSquare)));
                }
                else
                {
                    //2018-07-05  学梅确认, 平米定价-最低购买面积判断  ,   整体定价-使用面积
                    packs = packs.Where(x =>
                        !x.PricingType.HasValue ||
                       (x.PricingType == DecorationPackPricingType.Total && (minSquare == null || x.Square >= minSquare) && (maxSquare == null || x.Square <= maxSquare)
                        || x.PricingType == DecorationPackPricingType.Unit && (minSquare == null || x.UnitSquare >= minSquare) && (maxSquare == null || x.UnitSquare <= maxSquare)));
                }
            }

            #endregion




            Expression<Func<DecorationPack, bool>> condition =
                x =>
                    (string.IsNullOrEmpty(keywords) || x.Keywords.Contains(keywords)) &&
                    (createrId == null || x.CreaterId == createrId) &&
                    (!hasProperties || propertyId == null || !string.IsNullOrEmpty(x.PropertysStr) && x.PropertysStr.Contains(propertyId.ToString())) &&
                    (applicableSquare == null || x.MinApplicableSquare <= applicableSquare && x.MaxApplicableSquare >= applicableSquare) &&
                    (isNewHouse == null || (isNewHouse.Value ? x.NewHouse : x.SecondHandHouse)) &&
                    (hasOffSku == null || x.HasOffShelvedSku == hasOffSku) &&
                    (hasChangedSku == null || x.HasChangedSku == hasChangedSku) &&
                    (color == null || x.Color == color) &&
                    (packType == null || x.PackType == packType) &&
                    (packKind == null || x.PackKind == packKind) &&
                    (packMode == null || x.PackMode == packMode);

            packs = packs.Where(condition);
            //排序
            if (sort != null && sort.Any() && packs.Any())
            {
                packs = packs.OrderBy(sort);
            }

            rowCount = packs.Count();
            pageCount = (int)Math.Ceiling(rowCount * 1.0 / pageSize);
            return packs.Skip((pageIndex - 1) * pageSize).Take(pageSize);
            //return packs.ToPage(x => true, pageIndex, pageSize, out rowCount, out pageCount);
        }

        #endregion

        #region # 分页获取套餐模板列表 —— IEnumerable<DecorationPack> FindByCondition(Guid propertyId, Guid? houseTypeId, decimal? minPrice, decimal? maxPrice, float? minSquare, float? maxSquare,

        /// <summary>
        /// 分页获取套餐模板列表(排除克隆版本套餐)
        /// </summary>
        /// <param name="propertyId">楼盘Id</param>
        /// <param name="houseTypeId">户型Id</param>
        /// <param name="minPrice">最小价格</param>
        /// <param name="maxPrice">最大价格</param>
        /// <param name="minSquare">最小面积</param>
        /// <param name="maxSquare">最大面积</param>
        /// <param name="sort">排序条件 false为倒序</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页容量</param>
        /// <param name="rowCount">总记录条数</param>
        /// <param name="pageCount">总页数</param>
        /// <returns>套餐模板列表</returns>
        public IEnumerable<DecorationPack> FindByCondition(Guid propertyId, Guid? houseTypeId, decimal? minPrice, decimal? maxPrice, float? minSquare, float? maxSquare,
            Dictionary<string, bool> sort, int pageIndex, int pageSize, out int rowCount, out int pageCount)
        {

            IQueryable<DecorationPack> packs = this.FindAllInner();
            packs = packs.Where(x => x.Status == ShelfStatus.Shelfed && x.SourcePackId == null);

            //如果户型可以查询到数据 , 条件为 户型+预算
            //否则如果楼盘可以查询到数据 ,  条件为楼盘+面积+预算
            //楼盘户型都无匹配数据 , 条件为面积+预算

            var hasHouseType = houseTypeId.HasValue && packs.Any(s => !string.IsNullOrEmpty(s.HouseTypesStr) && s.HouseTypesStr.Contains(houseTypeId.ToString()));
            var hasProperties = packs.Any(s => propertyId == null || !string.IsNullOrEmpty(s.PropertysStr) && s.PropertysStr.Contains(propertyId.ToString()));
            packs = packs.Where(x =>
                x.PricingType.HasValue &&
                (hasHouseType || x.PricingType == DecorationPackPricingType.Total && (minSquare == null || x.Square >= minSquare) &&
                 (maxSquare == null || x.Square <= maxSquare) || x.PricingType == DecorationPackPricingType.Unit && (minSquare == null || x.UnitSquare >= minSquare) &&
                 (maxSquare == null || x.UnitSquare <= maxSquare)) &&
                (x.PricingType == DecorationPackPricingType.Total && (minPrice == null || x.TotalPrice >= minPrice) && (maxPrice == null || x.TotalPrice <= maxPrice) ||
                 x.PricingType == DecorationPackPricingType.Unit && (minPrice == null || x.UnitTotalPrice >= minPrice) && (maxPrice == null || x.UnitTotalPrice <= maxPrice)) &&
                (!hasHouseType || !string.IsNullOrEmpty(x.HouseTypesStr) && x.HouseTypesStr.Contains(houseTypeId.ToString())) &&
                (!hasProperties || !hasProperties || !string.IsNullOrEmpty(x.PropertysStr) && x.PropertysStr.Contains(propertyId.ToString())));



            //排序
            if (sort != null && sort.Any() && packs.Any())
            {
                packs = packs.OrderBy(sort);
            }

            rowCount = packs.Count();
            pageCount = (int)Math.Ceiling(rowCount * 1.0 / pageSize);
            return packs.Skip((pageIndex - 1) * pageSize).Take(pageSize);


        }

        #endregion

        #region # 分页获取套餐模板列表 —— IEnumerable<DecorationPack> FindByCondition(IList<Guid> propertys, Guid? houseTypeId, bool hasPackSeries, IEnumerable<Guid> packSeriesIds, IEnumerable<string> styleNos,Dictionary<string, bool> sort, int pageIndex, int pageSize, out int rowCount, out int pageCount)

        /// <summary>
        /// 分页获取套餐模板列表(排除克隆版本套餐)
        /// </summary>
        /// <param name="keywords">关键字</param>
        /// <param name="containsNoProperty">是否包含无楼盘标签套餐（true查询）</param>
        /// <param name="propertys">楼盘集（查询包含此集合标签）</param>
        /// <param name="propertyId">楼盘Id（查询包含此楼盘标签）</param>
        /// <param name="houseTypeId">户型Id（查询包含此户型标签）</param>
        /// <param name="hasPackSeries">是否有标签(全部,标签:true,其他:false)</param>
        /// <param name="packSeriesIds">套餐系列集</param>
        /// <param name="styleNos">设计风格集</param>
        /// <param name="isBuildSuqare">是否是建筑面积搜索</param>
        /// <param name="minSquare">最小面积</param>
        /// <param name="maxSquare">最大面积</param>
        /// <param name="sort">排序条件 false为倒序</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页容量</param>
        /// <param name="rowCount">总记录条数</param>
        /// <param name="pageCount">总页数</param>
        /// <returns>套餐模板列表</returns>
        public IEnumerable<DecorationPack> FindByCondition(string keywords, bool containsNoProperty, IList<Guid> propertys, Guid? propertyId, Guid? houseTypeId, bool hasPackSeries, IEnumerable<Guid> packSeriesIds, IEnumerable<string> styleNos, bool? isBuildSuqare, float? minSquare, float? maxSquare, Dictionary<string, bool> sort, int pageIndex, int pageSize, out int rowCount, out int pageCount)
        {
            IQueryable<DecorationPack> packs = this.FindAllInner();
            packs = packs.Where(x => x.Status == ShelfStatus.Shelfed && x.SourcePackId == null);

            //筑家帮需求： 1、containsNoProperty false 楼盘集 只查询包含楼盘标签的套餐；2、containsNoProperty true 楼盘集 查询包含楼盘标签和无楼盘标签的套餐；
            //3、propertyId有值 只查询包含该楼盘标签的套餐；


            #region 标签处理
            //系列
            if (hasPackSeries)
            {
                if (packSeriesIds != null && packSeriesIds.Any())
                {
                    packs = packs.Where(s => s.PackSeries.Select(t => t.Id).Intersect(packSeriesIds).Any());
                }
            }
            else
            {
                packs = packs.Where(s => !s.PackSeries.Any());
            }
            //设计风格标签
            if (styleNos != null && styleNos.Any())
            {
                PredicateBuilder<DecorationPack> builder = new PredicateBuilder<DecorationPack>(x => false);
                foreach (string styleNo in styleNos)
                {
                    string sStyleNo = styleNo;

                    builder.Or(x => x.StyleNosStr.Contains(sStyleNo));
                }

                packs = packs.Where(builder.Build());
            }
            //楼盘标签
            if (propertys != null && propertys.Any())
            {
                PredicateBuilder<DecorationPack> builder = new PredicateBuilder<DecorationPack>(x => false);
                if (containsNoProperty)
                {
                    foreach (Guid property in propertys)
                    {
                        string sProperty = property.ToString();

                        builder.Or(x => string.IsNullOrEmpty(x.PropertysStr) || x.PropertysStr.Contains(sProperty));
                    }
                }
                else
                {

                    foreach (Guid property in propertys)
                    {
                        string sProperty = property.ToString();

                        builder.Or(x => x.PropertysStr.Contains(sProperty));
                    }

                }
                packs = packs.Where(builder.Build());
            }

            #endregion




            #region 建筑面积 || 使用面积 面积搜索

            if (minSquare != null && maxSquare != null)
            {
                if (isBuildSuqare.Value)
                {
                    //2018-07-05  学梅确认, 平米定价-最低购买面积判断  ,   整体定价-使用面积
                    packs = packs.Where(x =>
                        !x.PricingType.HasValue ||
                        (x.PricingType == DecorationPackPricingType.Total && (minSquare == null || x.BuildingSquare >= minSquare) && (maxSquare == null || x.BuildingSquare <= maxSquare)
                        || x.PricingType == DecorationPackPricingType.Unit && (minSquare == null || x.UnitBuildingSquare >= minSquare) && (maxSquare == null || x.UnitBuildingSquare <= maxSquare)));
                }
                else
                {
                    //2018-07-05  学梅确认, 平米定价-最低购买面积判断  ,   整体定价-使用面积
                    packs = packs.Where(x =>
                        !x.PricingType.HasValue ||
                       (x.PricingType == DecorationPackPricingType.Total && (minSquare == null || x.Square >= minSquare) && (maxSquare == null || x.Square <= maxSquare)
                        || x.PricingType == DecorationPackPricingType.Unit && (minSquare == null || x.UnitSquare >= minSquare) && (maxSquare == null || x.UnitSquare <= maxSquare)));
                }
            }

            #endregion


            Expression<Func<DecorationPack, bool>> condition =
                x => (string.IsNullOrEmpty(keywords) || x.Keywords.Contains(keywords)) &&
                    (houseTypeId == null || !string.IsNullOrEmpty(x.HouseTypesStr) && x.HouseTypesStr.Contains(houseTypeId.ToString()))
                && (propertyId == null || !string.IsNullOrEmpty(x.PropertysStr) && x.PropertysStr.Contains(propertyId.ToString()))
                //&& (!containsNoProperty || string.IsNullOrEmpty(x.PropertysStr))
                ;

            packs = packs.Where(condition);

            //排序
            if (sort != null && sort.Any() && packs.Any())
            {
                packs = packs.OrderBy(sort);
            }

            rowCount = packs.Count();
            pageCount = (int)Math.Ceiling(rowCount * 1.0 / pageSize);
            return packs.Skip((pageIndex - 1) * pageSize).Take(pageSize);
        }

        #endregion

        #region # 获取套餐模板最小排序 —— float GetMinSort()
        /// <summary>
        /// 获取套餐模板最小排序
        /// </summary>
        /// <returns>最小排序</returns>
        public float GetMinSort()
        {
            IQueryable<float> sorts = base.FindAllInner().Where(x => x.SourcePackId == null).Select(x => x.Sort).OrderBy(x => x);

            return sorts.Any() ? sorts.First() : 0;
        }
        #endregion

        #region # 获取套餐模板最大排序 —— float GetMaxSort()
        /// <summary>
        /// 获取套餐模板最大排序
        /// </summary>
        /// <returns>最大排序</returns>
        public float GetMaxSort()
        {
            IQueryable<float> sorts = base.FindAllInner().Where(x => x.SourcePackId == null).Select(x => x.Sort).OrderByDescending(x => x);

            return sorts.Any() ? sorts.First() : 0;
        }
        #endregion

        #region # 根据源套餐Id获取套餐当前版本及Id ——  DecorationPack FindDecorationPackVersionNumber(Guid sourcePackId)
        /// <summary>
        /// 根据源套餐Id获取套餐当前版本及Id
        /// </summary>
        /// <param name="sourcePackId">源套餐Id</param>
        /// <returns></returns>
        public DecorationPack FindDecorationPackVersionNumber(Guid sourcePackId)
        {
            IQueryable<DecorationPack> balePacks = base.FindAllInner();
            Expression<Func<DecorationPack, bool>> condition =
             s => s.SourcePackId == sourcePackId;
            balePacks = balePacks.Where(condition).OrderByDescending(x => x.AddedTime).ThenByDescending(x => x.VersionNumber); //修改时间不准确购买量浏览量修改；
            return balePacks.FirstOrDefault();
        }

        #endregion

        #region # 根据源套餐Id集获取套餐最新版本集 ——         IEnumerable<DecorationPack> FindDecorationPackVersionNumbers(IEnumerable<Guid> sourcePackIds);

        /// <summary>
        /// 根据源套餐Id集获取套餐最新版本集
        /// </summary>
        /// <param name="sourcePackIds">源套餐Id集</param>
        /// <returns></returns>
        public IEnumerable<DecorationPack> FindDecorationPackVersionNumbers(IEnumerable<Guid> sourcePackIds)
        {
            sourcePackIds = sourcePackIds.IsNullOrEmpty() ? new List<Guid>() : sourcePackIds.Distinct();

            Expression<Func<DecorationPack, bool>> condition = s => s.SourcePackId.HasValue && sourcePackIds.Contains(s.SourcePackId.Value) || sourcePackIds.Contains(s.Id);
            return base.Find(condition);
        }
        #endregion

        #region # 验证套餐名称是否重复 —— bool ExistsPackName(Guid? packId, string packName)
        /// <summary>
        /// 验证套餐名称是否重复
        /// </summary>
        /// <param name="packId">套餐Id</param>
        /// <param name="packName">套餐名称</param>
        /// <returns></returns>
        public bool ExistsPackName(Guid? packId, string packName)
        {
            return base.Exists(s => (packId == null || s.Id != packId) && s.Name == packName && !s.IsClone);
        }
        #endregion

        #region # 获取所有源套餐标签(居室|设计风格|空间类型)—— Tuple<List<string>, List<string>, List<string>> FindSourceDecorationPackLabels()
        /// <summary>
        /// 获取所有源套餐标签(居室|设计风格|空间类型)
        /// </summary>
        /// <returns></returns>
        public Tuple<List<string>, List<string>, List<string>> FindSourceDecorationPackLabels()
        {
            List<DecorationPack> sourceDecorationPacks = this.FindAllInner().Where(x => x.SourcePackId == null && x.Status == ShelfStatus.Shelfed).ToList();
            List<string> layouts = sourceDecorationPacks.SelectMany(x => x.Layouts).Distinct().ToList();
            List<string> styleNos = sourceDecorationPacks.SelectMany(x => x.StyleNos).Distinct().ToList();
            List<string> spaceTypes = sourceDecorationPacks.SelectMany(x => x.SpaceTypes).Distinct().ToList();
            Tuple<List<string>, List<string>, List<string>> labels = new Tuple<List<string>, List<string>, List<string>>(layouts, styleNos, spaceTypes);
            return labels;
        }
        #endregion

        #region # 通过套餐Id集获取套餐集 ——IEnumerable<DecorationPack> GetDecorationPacksByPackIds(IEnumerable<Guid> packIds, Guid? createrId = null);

        /// <summary>
        /// 通过套餐Id集获取套餐集
        /// </summary>
        /// <param name="createrId">套餐创建人Id？（全部：null,我的：当前登录人员）</param>
        /// <returns></returns>
        public IEnumerable<DecorationPack> GetDecorationPacksByPackIds(IEnumerable<Guid> packIds, Guid? createrId = null)
        {
            if (packIds.IsNullOrEmpty())
            {
                packIds = new List<Guid>();
            }

            Expression<Func<DecorationPack, bool>> query = q => packIds.Contains(q.Id) && (createrId == null || q.CreaterId == createrId);
            return base.Find(query);
        }

        #endregion


        #region # 获取套餐风格集 ——IEnumerable<string> GetDecorationPacksStyles();

        /// <summary>
        /// 获取套餐风格集
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> GetDecorationPacksStyles()
        {
            return base.FindAllInner().Where(x => x.SourcePackId == null && x.Status == ShelfStatus.Shelfed).Select(s => s.StyleNosStr).Where(s => !string.IsNullOrEmpty(s)).Distinct().ToList();
        }
        #endregion

        #region # 获取套餐居室集 —— IEnumerable<string> GetDecorationPacksLayouts()

        /// <summary>
        /// 获取套餐居室集
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> GetDecorationPacksLayouts()
        {
            return base.FindAllInner().Where(x => x.SourcePackId == null && x.Status == ShelfStatus.Shelfed).Select(s => s.LayoutsStr).Where(s => !string.IsNullOrEmpty(s)).Distinct().ToList().Select(s => s.JsonToObject<List<string>>()).SelectMany(s => s).Distinct();
        }
        #endregion

        #region # 获取套餐系列集 ——     IEnumerable<Tuple<Guid, string, string>> GetDecorationPackSeries();

        /// <summary>
        /// 获取套餐系列集
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Tuple<Guid, string, string>> GetDecorationPackSeries()
        {
            return base.FindAllInner().Where(x => x.SourcePackId == null && x.Status == ShelfStatus.Shelfed).SelectMany(s => s.PackSeries).DistinctBy(s => s.Id).Select(s => new Tuple<Guid, string, string>(s.Id, s.Name, s.Describe));
        }
        #endregion

        #region # 获取套餐户型集 ——     IEnumerable<Tuple<Guid, string, Guid, string, string>> GetDecorationPackHouseTypes()

        /// <summary>
        /// 获取套餐户型集
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Tuple<Guid, string, Guid, string, string>> GetDecorationPackHouseTypes()
        {
            return base.FindAllInner().Where(x => x.SourcePackId == null && x.Status == ShelfStatus.Shelfed).Select(s => s.HouseTypesStr).Where(s => !string.IsNullOrEmpty(s)).Distinct().ToList().Select(s => s.JsonToObject<List<Tuple<Guid, string, Guid, string, string>>>()).SelectMany(s => s).DistinctBy(x => x.Item1);
        }
        #endregion


        #region # 获取套餐楼盘集 —— Dictionary<Guid, string> GetDecorationPacksPropertys()

        /// <summary>
        /// 获取套餐楼盘集
        /// </summary>
        /// <returns></returns>
        public Dictionary<Guid, string> GetDecorationPacksPropertys()
        {
            List<DecorationPack> sourceDecorationPacks = this.FindAllInner().Where(x => x.SourcePackId == null && x.Status == ShelfStatus.Shelfed).ToList();
            return sourceDecorationPacks.SelectMany(x => x.Propertys).DistinctBy(x => x.Key).ToDictionary(x => x.Key, x => x.Value);
        }
        #endregion

        #region # 获取套餐系列集 ——     IEnumerable<Tuple<string, int, IEnumerable<Tuple<Guid, string, string, int, string>>>> GetDecorationPackGroupSeries()

        /// <summary>
        /// 获取套餐系列集(分组名|排序|系列集（系列Id，名称，描述，排序，分组名）)
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Tuple<string, int, IEnumerable<Tuple<Guid, string, string, int, string>>>> GetDecorationPackGroupSeries()
        {
            return
                base.FindAllInner()
                    .Where(x => x.SourcePackId == null && x.Status == ShelfStatus.Shelfed)
                    .SelectMany(s => s.PackSeries)
                    .DistinctBy(s => s.Id)
                    .GroupBy(x => x.GroupName)
                    .Select(s => new Tuple<string, int, IEnumerable<Tuple<Guid, string, string, int, string>>>(s.Key, s.OrderBy(x => x.Sort).First().Sort, s.OrderBy(x => x.Sort).Select(x => new Tuple<Guid, string, string, int, string>(x.Id, x.Name, x.Describe, x.Sort, x.GroupName)))).AsEnumerable();
        }
        #endregion


        #region # 根据项目Id分页获取套餐模板列表 —— IEnumerable<DecorationPack> FindByPage(string keywords, decimal? minPrice...
        /// <summary>
        /// 根据项目Id分页获取套餐模板列表
        /// </summary>
        /// <param name="projectId">项目Id</param>
        /// <param name="keywords">关键字</param>
        /// <param name="propertyId">楼盘Id</param>
        /// <param name="applicableSquare">适用面积</param>
        /// <param name="isNewHouse">是否适用新房</param>
        /// <param name="isBuildingPrice">是否是建筑价格搜索</param>
        /// <param name="minPrice">最小价格</param>
        /// <param name="maxPrice">最大价格</param>
        /// <param name="isBuildingSquare">是否是建筑面积搜索</param>
        /// <param name="minSquare">最小面积</param>
        /// <param name="maxSquare">最大面积</param>
        /// <param name="hasOffSku">是否有下架商品</param>
        /// <param name="hasChangedSku">是否有变价商品|工艺</param>
        /// <param name="color">颜色</param>
        /// <param name="packType">套餐类型</param>
        /// <param name="packKind">套餐类别</param>
        /// <param name="packMode">套餐模式</param>
        /// <param name="status">状态</param>
        /// <param name="spaceTypes">空间类型集</param>
        /// <param name="styleNos">设计风格集</param>
        /// <param name="layouts">居室集</param>
        /// <param name="propertys">楼盘集</param>
        /// <param name="hasPackSeries"></param>
        /// <param name="packSeriesIds">套餐系列集</param>
        /// <param name="stores">门店集</param>
        /// <param name="hasStores">是否有门店（全部,标签:true,其他:false）</param>
        /// <param name="createrId">套餐创建人Id？（全部：null,我的：当前登录人员）</param>
        /// <param name="sort">排序条件 false为倒序</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页容量</param>
        /// <param name="rowCount">总记录条数</param>
        /// <param name="pageCount">总页数</param>
        /// <returns>套餐模板列表</returns>
        public IEnumerable<DecorationPack> FindByPage(Guid projectId, string keywords, Guid? propertyId, float? applicableSquare, bool? isNewHouse, bool? isBuildingPrice, decimal? minPrice, decimal? maxPrice, bool? isBuildingSquare,
            float? minSquare, float? maxSquare, bool? hasOffSku, bool? hasChangedSku, DecorationPackColor? color, DecorationPackType? packType,
            DecorationPackKind? packKind, DecorationPackMode? packMode, ShelfStatus? status, IList<string> spaceTypes, IList<string> styleNos, IList<string> layouts,
            IList<Guid> propertys, bool hasPackSeries, IList<Guid> packSeriesIds, IList<Guid> stores, bool hasStores, Guid? createrId, Dictionary<string, bool> sort, int pageIndex, int pageSize, out int rowCount, out int pageCount)
        {
            IQueryable<DecorationPack> packs = this.FindAllInner();
            packs = packs.Where(x => (status == null || x.Status == status) && x.SourcePackId == null);
            //需求文档要求,如果楼盘条件查询不到,则过滤此条件
            var hasProperties = packs.Any(s => propertyId == null || !string.IsNullOrEmpty(s.PropertysStr) && s.PropertysStr.Contains(propertyId.ToString()));

            #region # 标签处理


            //空间类型标签
            if (spaceTypes != null && spaceTypes.Any())
            {
                PredicateBuilder<DecorationPack> builder = new PredicateBuilder<DecorationPack>(x => false);
                foreach (string spaceType in spaceTypes)
                {
                    string sType = spaceType;

                    builder.Or(x => x.SpaceTypesStr.Contains(sType));
                }

                packs = packs.Where(builder.Build());
            }

            //设计风格标签
            if (styleNos != null && styleNos.Any())
            {
                PredicateBuilder<DecorationPack> builder = new PredicateBuilder<DecorationPack>(x => false);
                foreach (string styleNo in styleNos)
                {
                    string sStyleNo = styleNo;

                    builder.Or(x => x.StyleNosStr.Contains(sStyleNo));
                }

                packs = packs.Where(builder.Build());
            }

            //居室标签
            if (layouts != null && layouts.Any())
            {
                PredicateBuilder<DecorationPack> builder = new PredicateBuilder<DecorationPack>(x => false);
                foreach (string layout in layouts)
                {
                    string sLayout = layout;

                    builder.Or(x => x.LayoutsStr.Contains(sLayout));
                }

                packs = packs.Where(builder.Build());
            }

            //楼盘标签
            if (propertys != null && propertys.Any())
            {
                PredicateBuilder<DecorationPack> builder = new PredicateBuilder<DecorationPack>(x => false);
                foreach (Guid property in propertys)
                {
                    string sProperty = property.ToString();

                    builder.Or(x => x.PropertysStr.Contains(sProperty));
                }

                packs = packs.Where(builder.Build());
            }

            if (hasStores)
            {
                //门店标签
                if (stores != null && stores.Any())
                {
                    PredicateBuilder<DecorationPack> builder = new PredicateBuilder<DecorationPack>(x => false);
                    foreach (Guid store in stores)
                    {
                        string sStore = store.ToString();

                        builder.Or(x => x.StoresStr.Contains(sStore));
                    }

                    packs = packs.Where(builder.Build());
                }
            }
            else
            {
                packs = packs.Where(x => string.IsNullOrEmpty(x.StoresStr));
            }

            if (hasPackSeries)
            {
                if (packSeriesIds != null && packSeriesIds.Any())
                {
                    packs = packs.Where(s => s.PackSeries.Select(t => t.Id).Intersect(packSeriesIds).Any());
                }
            }
            else
            {
                packs = packs.Where(s => !s.PackSeries.Any());
            }
            #endregion


            #region  建筑面积  || 使用面积  价格搜索
            //2018-07-23  学梅确认, 价格搜索按照定价方式里面的建筑面积|使用面积勾选进行查询；建筑面积-（是否整体建筑定价|是否平米整体建筑定价为true）
            if (minPrice != null || maxPrice != null)
            {
                if (isBuildingPrice.Value)
                {

                    if (minPrice != null && maxPrice == null)
                    {
                        packs = packs.Where(x => (x.BuildingTotalPrice >= minPrice && x.IsBuilding) || (x.UnitBuildingTotalPrice >= minPrice && x.IsUnitBuilding));

                    }
                    else if (minPrice == null && maxPrice != null)
                    {
                        packs = packs.Where(x => (x.BuildingTotalPrice <= maxPrice && x.IsBuilding) || (x.UnitBuildingTotalPrice <= maxPrice && x.IsUnitBuilding));

                    }
                    else if (minPrice != null && maxPrice != null)
                    {
                        packs = packs.Where(x =>
                            (x.BuildingTotalPrice >= minPrice && x.BuildingTotalPrice <= maxPrice && x.IsBuilding) ||
                            (x.UnitBuildingTotalPrice >= minPrice && x.UnitBuildingTotalPrice <= maxPrice && x.IsUnitBuilding));

                    }

                }
                else
                {

                    if (minPrice != null && maxPrice == null)
                    {
                        packs = packs.Where(x => (x.TotalPrice >= minPrice && x.IsActual) || (x.UnitTotalPrice >= minPrice && x.IsUnitActual));

                    }
                    else if (minPrice == null && maxPrice != null)
                    {
                        packs = packs.Where(x => (x.TotalPrice <= maxPrice && x.IsActual) || (x.UnitTotalPrice <= maxPrice && x.IsUnitActual));

                    }
                    else if (minPrice != null && maxPrice != null)
                    {
                        packs = packs.Where(x => (x.TotalPrice >= minPrice && x.TotalPrice <= maxPrice && x.IsActual) ||
                            (x.UnitTotalPrice >= minPrice && x.UnitTotalPrice <= maxPrice && x.IsUnitActual));

                    }

                }
            }

            #endregion

            #region 建筑面积 || 使用面积 面积搜索

            if (minSquare != null && maxSquare != null)
            {
                if (isBuildingSquare.Value)
                {
                    //2018-07-05  学梅确认, 平米定价-最低购买面积判断  ,   整体定价-使用面积
                    packs = packs.Where(x =>
                        !x.PricingType.HasValue ||
                        (x.PricingType == DecorationPackPricingType.Total && (minSquare == null || x.BuildingSquare >= minSquare) && (maxSquare == null || x.BuildingSquare <= maxSquare)
                        || x.PricingType == DecorationPackPricingType.Unit && (minSquare == null || x.UnitBuildingSquare >= minSquare) && (maxSquare == null || x.UnitBuildingSquare <= maxSquare)));
                }
                else
                {
                    //2018-07-05  学梅确认, 平米定价-最低购买面积判断  ,   整体定价-使用面积
                    packs = packs.Where(x =>
                        !x.PricingType.HasValue ||
                       (x.PricingType == DecorationPackPricingType.Total && (minSquare == null || x.Square >= minSquare) && (maxSquare == null || x.Square <= maxSquare)
                        || x.PricingType == DecorationPackPricingType.Unit && (minSquare == null || x.UnitSquare >= minSquare) && (maxSquare == null || x.UnitSquare <= maxSquare)));
                }
            }

            #endregion

            #region 关联套餐Id集
            //SqlParameter _packageId = new SqlParameter("@in_Pid",Guid.Parse("B38F5E58-2B23-4C25-BE2B-E388FA820F1F"));
            SqlParameter _packageId = new SqlParameter("@in_Pid", projectId);
            SqlParameter[] paraArray = new SqlParameter[1] { _packageId };
            var dt = this._dbContext.Database.SqlQuery<AssociatedPackage>("exec [dbo].[sh_Fill_GetDecorationPack] @in_Pid", paraArray);
            List<Guid> Ids = dt.Select(p => p.Id).ToList();
            #endregion


            Expression<Func<DecorationPack, bool>> condition =
                x =>//Ids.Contains(x.Id)&&
                    (string.IsNullOrEmpty(keywords) || x.Keywords.Contains(keywords)) &&
                    (createrId == null || x.CreaterId == createrId) &&
                    (!hasProperties || propertyId == null || !string.IsNullOrEmpty(x.PropertysStr) && x.PropertysStr.Contains(propertyId.ToString())) &&
                    (applicableSquare == null || x.MinApplicableSquare <= applicableSquare && x.MaxApplicableSquare >= applicableSquare) &&
                    (isNewHouse == null || (isNewHouse.Value ? x.NewHouse : x.SecondHandHouse)) &&
                    (hasOffSku == null || x.HasOffShelvedSku == hasOffSku) &&
                    (hasChangedSku == null || x.HasChangedSku == hasChangedSku) &&
                    (color == null || x.Color == color) &&
                    (packType == null || x.PackType == packType) &&
                    (packKind == null || x.PackKind == packKind) &&
                    (packMode == null || x.PackMode == packMode);

            packs = packs.Where(condition);
            //排序
            if (sort != null && sort.Any() && packs.Any())
            {
                packs = packs.OrderBy(sort);
            }

            rowCount = packs.Count();
            pageCount = (int)Math.Ceiling(rowCount * 1.0 / pageSize);
            return packs.Skip((pageIndex - 1) * pageSize).Take(pageSize);
        }
        #endregion

        #region # 根据项目Id获取已生效套餐列表——IEnumerable<DecorationPackInfo> GetDecorationPackByProjectId(Guid projectId)
        /// <summary>
        /// 根据项目Id获取已生效套餐列表
        /// </summary>
        /// <param name="projectId">项目Id</param>
        /// <returns>已生效套餐列表</returns>
        public IEnumerable<DecorationPack> GetDecorationPackByProjectId(Guid projectId)
        {
            return null;
        }
        #endregion
    }

    /// <summary>
    /// 续买套餐
    /// </summary>
    public  class AssociatedPackage
	{
        #region 套餐模板Id—Guid Id
        /// <summary>
        /// 套餐模板Id
        /// </summary>
        public Guid Id { get; set; } 
        #endregion
	}
}
