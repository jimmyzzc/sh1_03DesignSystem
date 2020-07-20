using HDIPlatform.DesignSystem.AppService.Maps;
using HDIPlatform.DesignSystem.Domain.Entities.BaleContext;
using HDIPlatform.DesignSystem.Domain.Entities.FixedMealContext;
using HDIPlatform.DesignSystem.Domain.IRepositories;
using HDIPlatform.DesignSystem.Domain.Mediators;
using HDIPlatform.DesignSystem.IAppService.DTOs.Inputs;
using HDIPlatform.DesignSystem.IAppService.DTOs.Outputs.FixedMealContext;
using HDIPlatform.DesignSystem.IAppService.Interfaces;
using SD.AOP.Core.Aspects.ForMethod;
using ShSoft.Common.PoweredByLee;
using ShSoft.Infrastructure.DTOBase;
using ShSoft.Infrastructure.Global.Transaction;
using ShSoft.ValueObjects.Enums;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.ServiceModel;
using System.Transactions;
using HDIPlatform.DesignSystem.Domain.Entities.ArticleContext;
using HDIPlatform.DesignSystem.Domain.Entities.ConfigContext;
using HDIPlatform.MarketSystem.IAppService.Interfaces;
using HDIPlatform.ResourceService.IAppService.DTOs.Outputs.CraftContext;
using HDIPlatform.ResourceService.IAppService.DTOs.Outputs.ProductContext;
using HDIPlatform.ResourceService.IAppService.Interfaces;

namespace HDIPlatform.DesignSystem.AppService.Implements
{
    /// <summary>
    /// 套餐模板服务契约实现
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
    public class FixedMealContract : IFixedMealContract
    {
        #region # 字段及依赖注入构造器

        /// <summary>
        /// 同步锁
        /// </summary>
        private static readonly object _Sync = new object();

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

        private readonly ICustomerModelContract _customerModelContract;
        private readonly IProductContract _productContract;
        private readonly ICraftEntityContract _craftEntityContract;

        /// <summary>
        /// 依赖注入构造器
        /// </summary>
        /// <param name="svcMediator">领域服务中介者</param>
        /// <param name="repMediator">仓储中介者</param>
        /// <param name="unitOfWork">单元事务</param>
        public FixedMealContract(DomainServiceMediator svcMediator, RepositoryMediator repMediator, IUnitOfWorkDesign unitOfWork, ICustomerModelContract customerModelContract, IProductContract productContract, ICraftEntityContract craftEntityContract)
        {
            this._svcMediator = svcMediator;
            this._repMediator = repMediator;
            this._unitOfWork = unitOfWork;
            _customerModelContract = customerModelContract;
            this._productContract = productContract;
            this._craftEntityContract = craftEntityContract;
        }

        #endregion


        //命令部分

        #region # 创建套餐模板 —— Guid CreateDecorationPack(string packName...

        /// <summary>
        /// 创建套餐模板
        /// </summary>
        /// <param name="packName">套餐模板名称</param>
        /// <param name="packType">套餐类型</param>
        /// <param name="packKind">套餐类别</param>
        /// <param name="packMode">套餐模式</param>
        /// <param name="newHouse">适用新房</param>
        /// <param name="secondHandHouse">适用二手房</param>
        /// <param name="packSeriesIds">套餐系列Id集</param>
        /// <param name="stores">门店集</param>
        /// <param name="operatorId">操作人Id</param>
        /// <param name="operator">操作人名称</param>
        /// <returns>套餐模板Id</returns>
        public Guid CreateDecorationPack(string packName, DecorationPackType packType, DecorationPackKind packKind, DecorationPackMode packMode, bool newHouse,
            bool secondHandHouse, IEnumerable<Guid> packSeriesIds, Dictionary<Guid, string> stores, Guid operatorId, string @operator)
        {
            //验证
            Assert.IsFalse(this._repMediator.DecorationPackRep.ExistsPackName((Guid?)null, packName), "套餐模板名称已存在！");
            if (!newHouse && !secondHandHouse)
            {
                throw new InvalidOperationException("请设置套餐适用于哪一类房源 ! ");
            }

            packSeriesIds = packSeriesIds ?? new List<Guid>();

            IDictionary<Guid, PackSeries> packSeries = _unitOfWork.GetPackSeriesByIds(packSeriesIds);

            if (packSeries.Values.Contains(null))
            {
                throw new ArgumentException(string.Format("Id为{0}的套餐系列不存在 , 请联系管理员 ! ", packSeries.First(s => s.Value == null).Key));
            }


            DecorationPack pack = new DecorationPack(packName, packType, packKind, packMode, newHouse, secondHandHouse, packSeries.Values, stores, @operator, operatorId);

            lock (_Sync)
            {
                float maxSort = this._repMediator.DecorationPackRep.GetMaxSort();
                pack.SetSort(maxSort + 1);
            }

            this._unitOfWork.RegisterAdd(pack);
            this._unitOfWork.UnitedCommit();

            return pack.Id;
        }
        #endregion

        #region # 修改套餐模板名称 —— void UpdateDecorationPack(Guid packId...
        /// <summary>
        /// 修改套餐模板名称
        /// </summary>
        /// <param name="packId">套餐模板Id</param>
        /// <param name="packName">套餐模板名称</param>
        public void UpdateDecorationPack(Guid packId, string packName)
        {
            //验证
            Assert.IsFalse(this._repMediator.DecorationPackRep.ExistsPackName(packId, packName), "套餐模板名称已存在！");

            DecorationPack currentPack = this._unitOfWork.Resolve<DecorationPack>(packId);
            //Assert.IsFalse(currentPack.Status == ShelfStatus.Shelfed, "套餐已上架不可修改");
            currentPack.UpdateInfo(packName);

            this._unitOfWork.RegisterSave(currentPack);
            this._unitOfWork.UnitedCommit();
        }
        #endregion

        #region # 设置套餐模板排序 —— void SetDecorationPackSort(Guid packId, int sort)
        /// <summary>
        /// 设置套餐模板排序
        /// </summary>
        /// <param name="packId">套餐模板Id</param>
        /// <param name="sort">排序</param>
        [TransactionAspect(TransactionScopeOption.RequiresNew)]
        public void SetDecorationPackSort(Guid packId, int sort)
        {
            lock (_Sync)
            {
                DecorationPack currentPack = this._unitOfWork.Resolve<DecorationPack>(packId);
                currentPack.SetSort(sort - 0.5f);

                this._unitOfWork.RegisterSave(currentPack);
                this._unitOfWork.Commit();

                this._svcMediator.DecorationPackSvc.InitSorts();
            }
        }
        #endregion

        #region # 删除套餐模板 —— void RemoveDecorationPack(Guid packId)
        /// <summary>
        /// 删除套餐模板
        /// </summary>
        /// <param name="packId">套餐模板Id</param>
        [TransactionAspect(TransactionScopeOption.RequiresNew)]
        public void RemoveDecorationPack(Guid packId)
        {
            lock (_Sync)
            {
                DecorationPack currentPack = this._repMediator.DecorationPackRep.Single(packId);

                Assert.IsTrue(currentPack.Status == ShelfStatus.NotShelfed, "只有未上架的套餐可以删除！");
                //删除模板项
                IEnumerable<DecorationPackItem> packItems = this._unitOfWork.ResolvePackItemsByPack(packId);
                IEnumerable<Guid> packItemIds = packItems.Select(x => x.Id).ToArray();
                if (packItemIds.Any())
                    this._unitOfWork.RegisterRemoveRange<DecorationPackItem>(packItemIds);
                this._unitOfWork.RegisterRemove<DecorationPack>(currentPack.Id);
                this._unitOfWork.Commit();

                this._svcMediator.DecorationPackSvc.InitSorts();
            }
        }
        #endregion

        #region # 保存套餐模板方案空间集 —— void SaveDecorationPackSpaces(Guid packId...
        /// <summary>
        /// 保存套餐模板方案空间集
        /// </summary>
        /// <param name="packId">套餐模板Id</param>
        /// <param name="packSpaceParams">套餐模板方案空间参数模型集</param>
        public void SaveDecorationPackSpaces(Guid packId, IEnumerable<PackSpaceParam> packSpaceParams)
        {
            DecorationPack currentPack = this._unitOfWork.Resolve<DecorationPack>(packId);

            #region # 验证

            if (currentPack.PackType == DecorationPackType.SingleSpace && packSpaceParams.Count(x => !x.IsDeleted) > 1)
            {
                throw new ArgumentException("单空间套餐只允许添加一个空间！");
            }

            packSpaceParams = packSpaceParams == null ? new PackSpaceParam[0] : packSpaceParams.ToArray();
            Assert.IsTrue(packSpaceParams.Any(), "套餐模板方案空间集不可为空！");

            PackSpaceParam[] notDeleteds = packSpaceParams.Where(x => !x.IsDeleted).ToArray();

            if (notDeleteds.DistinctBy(x => x.SpaceName).Count() != notDeleteds.Count())
            {
                throw new ArgumentOutOfRangeException("packSpaceParams", "套餐模板空间参数模型集中存在名称重复！");
            }

            #endregion


            Assert.IsFalse(currentPack.Status == ShelfStatus.Shelfed, "套餐已上架不可修改");
            foreach (PackSpaceParam param in packSpaceParams)
            {
                //删除
                if (param.SpaceId != null && param.IsDeleted)
                {
                    DecorationPackSpace currentSpace = currentPack.GetPackSpace(param.SpaceId.Value);
                    currentPack.RemoveSpace(currentSpace);
                }
                //修改
                else if (param.SpaceId != null && !param.IsDeleted)
                {
                    //验证
                    Assert.IsFalse(this._repMediator.DecorationPackSpaceRep.ExistsName(packId, param.SpaceId, param.SpaceName), string.Format("套餐模板空间名称\"{0}\"已存在！", param.SpaceName));

                    DecorationPackSpace currentSpace = currentPack.GetPackSpace(param.SpaceId.Value);
                    //修改或创建
                    foreach (DecorationPackSpaceDetailParam spaceDetailParam in param.SpaceDetailParams)
                    {
                        currentSpace.UpdateSpaceDetail(spaceDetailParam.SpaceDetailId, spaceDetailParam.NumericalStandard,
                           spaceDetailParam.GroundLength, spaceDetailParam.GroundWidth, spaceDetailParam.SpacePerimeter,
                           spaceDetailParam.WallHigh,
                           spaceDetailParam.HoleArea, spaceDetailParam.FacadeArea, spaceDetailParam.GroundArea, spaceDetailParam.CeilingArea);
                    }
                    currentSpace.UpdateInfo(param.SpaceName, param.Square, param.Sort);
                }
                //创建
                else
                {
                    //验证
                    Assert.IsFalse(this._repMediator.DecorationPackSpaceRep.ExistsName(packId, null, param.SpaceName), string.Format("套餐模板空间名称\"{0}\"已存在！", param.SpaceName));

                    IEnumerable<DecorationPackSpaceDetail> spaceDetails =
                        param.SpaceDetailParams.Select(
                            x =>
                                new DecorationPackSpaceDetail(x.NumericalStandard, x.GroundLength, x.GroundWidth, x.SpacePerimeter, x.WallHigh,
                                    x.HoleArea, x.FacadeArea, x.GroundArea, x.CeilingArea));
                    DecorationPackSpace packSpace = new DecorationPackSpace(param.SpaceName, param.Square, param.SpaceType, spaceDetails, param.Sort);
                    currentPack.AddSpace(packSpace);
                }
            }

            this._unitOfWork.RegisterSave(currentPack);
            this._unitOfWork.UnitedCommit();
        }
        #endregion

        #region # 创建套餐模板项 —— Guid CreatePackItem(string packItemName...
        /// <summary>
        /// 创建套餐模板项
        /// </summary>
        /// <param name="packItemName">套餐模板项名称</param>
        /// <param name="packId">套餐模板Id</param>
        /// <param name="packSpaceId">套餐模板空间Id</param>
        /// <param name="categoryArea">品类区域</param>
        /// <param name="categoryIds">三级品类Id集</param>
        /// <returns>套餐模板项Id</returns>
        public Guid CreatePackItem(string packItemName, Guid packId, Guid packSpaceId, CategoryArea categoryArea, IEnumerable<Guid> categoryIds)
        {
            //验证
            Assert.IsTrue(this._repMediator.DecorationPackRep.Exists(packId), string.Format("Id为\"{0}\"的套餐模板不存在！", packId));
            Assert.IsTrue(this._repMediator.DecorationPackSpaceRep.Exists(packSpaceId), string.Format("Id为\"{0}\"的套餐模板空间不存在！", packSpaceId));
            Assert.IsFalse(this._repMediator.DecorationPackItemRep.ExistsName(packId, packSpaceId, null, packItemName), "选区名称已存在！");
            DecorationPack currentPack = this._unitOfWork.Resolve<DecorationPack>(packId);
            Assert.IsFalse(currentPack.Status == ShelfStatus.Shelfed, "套餐已上架不可修改");
            DecorationPackItem item = new DecorationPackItem(packItemName, packId, packSpaceId, categoryArea, categoryIds);

            this._unitOfWork.RegisterAdd(item);
            this._unitOfWork.UnitedCommit();

            return item.Id;
        }
        #endregion

        #region # 添加商品SKU集至套餐模板项 —— void AddSkuItems(Guid packItemId, IEnumerable<DecorationPackSkuParam>...
        /// <summary>
        /// 添加商品SKU集至套餐模板项
        /// </summary>
        /// <param name="packItemId">套餐模板项Id</param>
        /// <param name="skuItems">商品SKU字典</param>
        /// <param name="defaultSkuId">默认商品SKU Id</param>
        /// <param name="defaultSkuQuantity">默认商品SKU数量</param>
        /// <remarks>IDictionary[Guid, int]，[商品SKU Id, 排序]</remarks>
        public void AddSkuItems(Guid packItemId, IEnumerable<DecorationPackSkuParam> skuItems)
        {
            //验证
            Assert.IsNotNull(skuItems, "商品字典不可为null！");


            DecorationPackItem currentPackItem = this._unitOfWork.Resolve<DecorationPackItem>(packItemId);
            DecorationPack currentPack = this._unitOfWork.Resolve<DecorationPack>(currentPackItem.PackId);
            Assert.IsFalse(currentPack.Status == ShelfStatus.Shelfed, "套餐已上架不可修改");

            //商品SKU集Id与套餐模板项中所配置三级品类是否一致(套餐模板项应包含商品SKU的所有三级品类)
            IEnumerable<Guid> skuCategoryIds = skuItems.Select(x => x.CategoryId).Distinct().ToArray();
            IEnumerable<Guid> categoryIds = currentPackItem.CategoryIds;
            Assert.IsTrue(skuCategoryIds.All(b => categoryIds.Contains(b)), "商品SKU集与套餐模板项中所配置三级品类不一致！");

            //商品SKU项处理
            IEnumerable<DecorationPackSku> packSkuItems = skuItems.Select(keyValue => new DecorationPackSku(keyValue.SkuId, keyValue.Sort, keyValue.Shelved, keyValue.CategoryId, keyValue.BrandId, keyValue.Default, keyValue.SkuQuantity, keyValue.CostPrice, keyValue.Changed, keyValue.SkuCraftPositions));

            //添加商品SKU集至套餐模板项
            currentPackItem.AddPackSkus(packSkuItems);
            this._unitOfWork.RegisterSave(currentPackItem);
            this._unitOfWork.UnitedCommit();
        }
        #endregion

        #region # 添加工艺实体集至套餐模板项 —— void AddCraftItems(Guid packItemId, IEnumerable<DecorationPackCraftParam>...
        /// <summary>
        /// 添加工艺实体集至套餐模板项
        /// </summary>
        /// <param name="packItemId">套餐模板项Id</param>
        /// <param name="craftEntityItems">工艺实体字典</param>
        /// <param name="defaultCraftEntityId">默认工艺实体Id</param>
        /// <param name="defaultCraftQuantity">默认工艺工程量</param>
        /// <remarks>IDictionary[Guid, int]，[工艺实体Id, 排序]</remarks>
        public void AddCraftItems(Guid packItemId, IEnumerable<DecorationPackCraftParam> craftEntityItems)
        {
            //验证
            Assert.IsNotNull(craftEntityItems, "工艺实体字典不可为null！");
            DecorationPackItem currentPackItem = this._unitOfWork.Resolve<DecorationPackItem>(packItemId);
            DecorationPack currentPack = this._unitOfWork.Resolve<DecorationPack>(currentPackItem.PackId);
            Assert.IsFalse(currentPack.Status == ShelfStatus.Shelfed, "套餐已上架不可修改");

            //工艺实体处理
            IEnumerable<DecorationPackCraft> packCraftItems = craftEntityItems.Select(keyValue => new DecorationPackCraft(keyValue.CraftEntityId, keyValue.Sort, keyValue.Shelved, keyValue.Default, keyValue.CraftQuantity, keyValue.CostPrice, keyValue.Changed, keyValue.SkuCraftPositions));

            //添加工艺实体集至套餐模板项
            currentPackItem.AddPackCrafts(packCraftItems);
            this._unitOfWork.RegisterSave(currentPackItem);

            //#region # 处理套餐
            ////套餐下所有选区（选区1，选区2……）
            //IEnumerable<DecorationPackItem> items = this._repMediator.DecorationPackItemRep.FindByPack(currentPackItem.PackId, null);
            ////所有工艺和商品是否都没变价
            //if (items.All(x => x.AllSkuNoChanged && x.AllCraftNoChanged))
            //{
            //    currentPack.SetHasChangedSku(false);
            //}
            //else
            //{
            //    currentPack.SetHasChangedSku(true);
            //}
            ////所有工艺和商品是否都上架
            //if (items.All(x => x.AllSkuShelved && x.AllCraftShelved))
            //{
            //    currentPack.SetHasOffShelvedSku(false);
            //}
            //else
            //{
            //    currentPack.SetHasOffShelvedSku(true);
            //}
            //this._unitOfWork.RegisterSave(currentPack);
            //#endregion

            this._unitOfWork.UnitedCommit();
        }
        #endregion

        #region # 设置选区内默认商品的工程量 ——  void SetDefaultSkuQuantity(Guid packItemId, Guid defaultSkuId, decimal skuQuantity, Dictionary<SkuCraftPosition, decimal> skuCraftPositions)
        /// <summary>
        /// 设置选区内默认商品的工程量
        /// </summary>
        /// <param name="packItemId">选区Id</param>
        /// <param name="defaultSkuId">默认SkuId</param>
        /// <param name="skuQuantity">默认Sku工程量</param>
        /// <param name="skuCraftPositions">放置位置</param>
        public void SetDefaultSkuQuantity(Guid packItemId, Guid defaultSkuId, decimal skuQuantity, Dictionary<SkuCraftPosition, decimal> skuCraftPositions)
        {
            DecorationPackItem currentPackItem = this._unitOfWork.Resolve<DecorationPackItem>(packItemId);
            DecorationPack currentPack = this._unitOfWork.Resolve<DecorationPack>(currentPackItem.PackId);
            Assert.IsFalse(currentPack.Status == ShelfStatus.Shelfed, "套餐已上架不可修改");

            currentPackItem.SetDefaultSkuQuantity(defaultSkuId, skuQuantity, skuCraftPositions);

            this._unitOfWork.RegisterSave(currentPackItem);
            this._unitOfWork.UnitedCommit();
        }
        #endregion

        #region # 设置选区内默认工艺的工程量 ——  void SetDefaultCraftQuantity(Guid packItemId, Guid defaultCraftId, decimal craftQuantity, Dictionary<SkuCraftPosition, decimal> skuCraftPositions)
        /// <summary>
        /// 设置选区内默认工艺的工程量
        /// </summary>
        /// <param name="packItemId">选区Id</param>
        /// <param name="defaultCraftId">默认CraftId</param>
        /// <param name="craftQuantity">默认工艺工程量</param>
        /// <param name="skuCraftPositions">放置位置</param>
        public void SetDefaultCraftQuantity(Guid packItemId, Guid defaultCraftId, decimal craftQuantity, Dictionary<SkuCraftPosition, decimal> skuCraftPositions)
        {
            DecorationPackItem currentPackItem = this._unitOfWork.Resolve<DecorationPackItem>(packItemId);
            DecorationPack currentPack = this._unitOfWork.Resolve<DecorationPack>(currentPackItem.PackId);
            Assert.IsFalse(currentPack.Status == ShelfStatus.Shelfed, "套餐已上架不可修改");
            currentPackItem.SetDefaultCraftQuantity(defaultCraftId, craftQuantity, skuCraftPositions);

            this._unitOfWork.RegisterSave(currentPackItem);
            this._unitOfWork.UnitedCommit();
        }

        #endregion

        #region # 批量修改套餐模板项 —— void UpdatePackItems(IDictionary<Guid,PackItemParam> itemParams)
        /// <summary>
        /// 批量修改套餐模板项
        /// </summary>
        /// <param name="itemParams">套餐模板项参数模型字典</param>
        public void UpdatePackItems(IDictionary<Guid, PackItemParam> itemParams)
        {
            foreach (KeyValuePair<Guid, PackItemParam> itemParam in itemParams)
            {
                DecorationPackItem currentPackItem = this._unitOfWork.Resolve<DecorationPackItem>(itemParam.Key);
                DecorationPack currentPack = this._unitOfWork.Resolve<DecorationPack>(currentPackItem.PackId);
                Assert.IsFalse(currentPack.Status == ShelfStatus.Shelfed, "套餐已上架不可修改");

                //验证
                Assert.IsFalse(this._repMediator.DecorationPackItemRep.ExistsName(currentPackItem.PackId, currentPackItem.PackSpaceId, currentPackItem.Id, itemParam.Value.ItemName), "选区名称已存在！");

                currentPackItem.UpdateInfo(itemParam.Value.ItemName, itemParam.Value.CategoryArea);
                currentPackItem.SetDefaultQuantity(itemParam.Value.DefaultSkuQuantity, itemParam.Value.DefaultCraftQuantity);

                this._unitOfWork.RegisterSave(currentPackItem);
            }

            this._unitOfWork.UnitedCommit();
        }
        #endregion

        #region # 修改套餐模板项 —— void void UpdatePackItem(Guid itemId, PackItemParam itemParam, IEnumerable<Guid> categoryIds)

        /// <summary>
        /// 修改套餐模板项
        /// </summary>
        /// <param name="itemId">模板项Id</param>
        /// <param name="itemParam">套餐模板项参数模型字典</param>
        /// <param name="categoryIds">品类集</param>
        public void UpdatePackItem(Guid itemId, PackItemParam itemParam, IEnumerable<Guid> categoryIds)
        {
            DecorationPackItem currentPackItem = this._unitOfWork.Resolve<DecorationPackItem>(itemId);
            DecorationPack currentPack = this._unitOfWork.Resolve<DecorationPack>(currentPackItem.PackId);
            //验证
            Assert.IsFalse(currentPack.Status == ShelfStatus.Shelfed, "套餐已上架不可修改");
            Assert.IsNotNull(categoryIds, "三级品类集不可为null！");
            Assert.IsFalse(this._repMediator.DecorationPackItemRep.ExistsName(currentPackItem.PackId, currentPackItem.PackSpaceId, currentPackItem.Id, itemParam.ItemName), "选区名称已存在！");
            categoryIds = categoryIds == null ? new Guid[0] : categoryIds.ToArray();
            currentPackItem.UpdateInfo(itemParam.ItemName, itemParam.CategoryArea);
            currentPackItem.SetDefaultQuantity(itemParam.DefaultSkuQuantity, itemParam.DefaultCraftQuantity);
            currentPackItem.UpdateCategoryIds(categoryIds);

            this._unitOfWork.RegisterSave(currentPackItem);
            this._unitOfWork.UnitedCommit();
        }
        #endregion

        #region # 克隆套餐模板项 —— void ClonePackItem(Guid packItemId, IDictionary<Guid, string>...
        /// <summary>
        /// 克隆套餐模板项
        /// </summary>
        /// <param name="packItemId">套餐模板项Id</param>
        /// <param name="packSpaceIdNames">套餐模板空间Id/新套餐模板项名称字典</param>
        public void ClonePackItem(Guid packItemId, IDictionary<Guid, string> packSpaceIdNames)
        {
            #region # 验证

            packSpaceIdNames = packSpaceIdNames ?? new Dictionary<Guid, string>();

            //if (packSpaceIdNames.Values.Count != packSpaceIdNames.Values.Distinct().Count())
            //{
            //    throw new ArgumentOutOfRangeException("packSpaceIdNames", "给定的新套餐模板项名称中存在重复！");
            //}

            #endregion

            DecorationPackItem currentPackItem = this._repMediator.DecorationPackItemRep.Single(packItemId);
            DecorationPack currentPack = this._unitOfWork.Resolve<DecorationPack>(currentPackItem.PackId);
            Assert.IsFalse(currentPack.Status == ShelfStatus.Shelfed, "套餐已上架不可修改");


            //注：此处代码是为了将导航属性加载出来
            Trace.WriteLine(currentPackItem.PackSkus.Count);
            Trace.WriteLine(currentPackItem.PackCraftEntities.Count);

            foreach (KeyValuePair<Guid, string> keyValue in packSpaceIdNames)
            {
                //验证
                Assert.IsTrue(this._repMediator.DecorationPackSpaceRep.Exists(keyValue.Key), "套餐模板空间不存在！");
                Assert.IsFalse(this._repMediator.DecorationPackItemRep.ExistsName(currentPackItem.PackId, keyValue.Key, null, keyValue.Value), "选区名称重复！");

                DecorationPackItem clonedPackItem = currentPackItem.Clone<DecorationPackItem>();
                clonedPackItem.SetId(Guid.NewGuid());                                           //重新设置Id
                clonedPackItem.UpdateInfo(keyValue.Value, clonedPackItem.CategoryArea);         //重新设置套餐模板项名称
                clonedPackItem.SetPackSpaceId(keyValue.Key);                                    //重新设置套餐模板空间Id
                //空间
                DecorationPackSpace currentSpace = currentPack.GetPackSpace(keyValue.Key);
                //空间标准值
                DecorationPackSpaceDetail spaceDetail = currentSpace.SpaceDetails.Single(x => x.NumericalStandard == NumericalStandard.Standard);

                //重新为Id赋值
                foreach (DecorationPackSku packSku in clonedPackItem.PackSkus)
                {
                    packSku.SetId(Guid.NewGuid());

                    #region 默认商品的放置位置和工程量修改
                    if (packSku.Default)
                    {
                        IDictionary<SkuCraftPosition, decimal> skuCraftPositions = packSku.SkuCraftPositions;
                        Dictionary<SkuCraftPosition, decimal> clonedSkuCraftPositions = new Dictionary<SkuCraftPosition, decimal>();
                        skuCraftPositions.ForEach(p =>
                        {
                            decimal quantity = p.Value;
                            switch (p.Key)
                            {
                                case SkuCraftPosition.Ceiling:
                                    quantity = spaceDetail.CeilingArea;
                                    break;
                                case SkuCraftPosition.Ground:
                                    quantity = spaceDetail.GroundArea;
                                    break;
                                case SkuCraftPosition.Other:
                                    break;
                                case SkuCraftPosition.Wall:
                                    quantity = spaceDetail.FacadeArea;
                                    break;

                            }
                            clonedSkuCraftPositions.Add(p.Key, quantity);

                        });
                        packSku.SetDefaultSkuPositions(clonedSkuCraftPositions.Sum(x => x.Value), clonedSkuCraftPositions);
                    }
                    #endregion
                }
                foreach (DecorationPackCraft packCraft in clonedPackItem.PackCraftEntities)
                {
                    packCraft.SetId(Guid.NewGuid());

                    #region 默认工艺的放置位置和工程量修改
                    if (packCraft.Default)
                    {
                        IDictionary<SkuCraftPosition, decimal> skuCraftPositions = packCraft.SkuCraftPositions;
                        Dictionary<SkuCraftPosition, decimal> clonedSkuCraftPositions = new Dictionary<SkuCraftPosition, decimal>();
                        skuCraftPositions.ForEach(p =>
                        {
                            decimal quantity = p.Value;
                            switch (p.Key)
                            {
                                case SkuCraftPosition.Ceiling:
                                    quantity = spaceDetail.CeilingArea;
                                    break;
                                case SkuCraftPosition.Ground:
                                    quantity = spaceDetail.GroundArea;
                                    break;
                                case SkuCraftPosition.Other:
                                    break;
                                case SkuCraftPosition.Wall:
                                    quantity = spaceDetail.FacadeArea;
                                    break;
                            }
                            clonedSkuCraftPositions.Add(p.Key, quantity);

                        });
                        packCraft.SetDefaultCraftPositions(clonedSkuCraftPositions.Sum(x => x.Value), clonedSkuCraftPositions);
                    }
                    #endregion
                }
                this._unitOfWork.RegisterAdd(clonedPackItem);
            }
            this._unitOfWork.UnitedCommit();
        }
        #endregion

        #region # 删除套餐模板项 —— void RemovePackItem(Guid packItemId)
        /// <summary>
        /// 删除套餐模板项
        /// </summary>
        /// <param name="packItemId">套餐模板项Id</param>
        public void RemovePackItem(Guid packItemId)
        {
            DecorationPackItem currentPackItem = this._unitOfWork.Resolve<DecorationPackItem>(packItemId);
            DecorationPack currentPack = this._unitOfWork.Resolve<DecorationPack>(currentPackItem.PackId);
            Assert.IsFalse(currentPack.Status == ShelfStatus.Shelfed, "套餐已上架不可修改");
            this._unitOfWork.RegisterRemove<DecorationPackItem>(packItemId);

            #region # 处理套餐
            //套餐下所有选区（选区1，选区2……）
            //IEnumerable<DecorationPackItem> items = this._repMediator.DecorationPackItemRep.FindByPack(currentPackItem.PackId, null);
            IEnumerable<DecorationPackItem> items = this._unitOfWork.ResolvePackItemsByPack(currentPack.Id);
            //所有工艺和商品是否都没变价
            if (items.All(x => x.AllSkuNoChanged && x.AllCraftNoChanged))
            {
                currentPack.SetHasChangedSku(false);
            }
            else
            {
                currentPack.SetHasChangedSku(true);
            }
            //所有工艺和商品是否都上架
            if (items.All(x => x.AllSkuShelved && x.AllCraftShelved))
            {
                currentPack.SetHasOffShelvedSku(false);
            }
            else
            {
                currentPack.SetHasOffShelvedSku(true);
            }
            this._unitOfWork.RegisterSave(currentPack);
            #endregion

            this._unitOfWork.UnitedCommit();
        }
        #endregion

        #region # 按平方米定价（平米定价清空整体定价） —— void SetPriceByUnit(Guid packId, decimal unitPrice, decimal buildingUnitPrice)

        /// <summary>
        /// 按平方米定价（平米定价清空整体定价,商品|工艺成本价更新为最新价格）
        /// </summary>
        /// <param name="packId">套餐模板Id</param>
        /// <param name="unitPrice">按照使用面积单价</param>
        /// <param name="buildingUnitPrice">按建筑面积定价</param>
        /// <param name="premium">整体超出部分单价</param>
        /// <param name="spacePricingParams">空间定价参数集</param>
        /// <param name="isUnitActual">平米使用</param>
        /// <param name="unitSquare">平米定价最低购买使用面积</param>
        /// <param name="unitBuildingSquare">平米定价最低购买建筑面积</param>
        /// <param name="isWhole">是否整体定价超出</param>
        /// <param name="criterionSquare">标准面积（公式）</param>
        /// <param name="isUnitBuilding">平米建筑</param>
        /// <param name="minApplicableSquare">套餐适用的最小使用面积</param>
        /// <param name="maxApplicableSquare">套餐适用的最大使用面积</param>
        /// <param name="isManageFee">设置管理费参考价</param>
        /// <param name="manageFee">管理费参考价</param>
        /// <param name="isWaterElectricityFee">设置水电预收参考价</param>
        /// <param name="waterElectricityFee">水电预收参考价</param>
        public void SetPriceByUnit(Guid packId, decimal unitPrice, decimal buildingUnitPrice, bool isUnitBuilding, bool isUnitActual, float unitSquare, float unitBuildingSquare, bool isWhole, float criterionSquare, decimal premium, float minApplicableSquare, float maxApplicableSquare, bool isManageFee, decimal manageFee, bool isWaterElectricityFee, decimal waterElectricityFee, IEnumerable<PackSchemeSpacePricingParam> spacePricingParams)
        {
            DecorationPack currentPack = this._unitOfWork.Resolve<DecorationPack>(packId);

            IEnumerable<DecorationPackSpace> spaces = _repMediator.DecorationPackSpaceRep.FindByPack(packId);

            if (currentPack.PackType == DecorationPackType.SingleSpace && spaces.Count() > 1)
            {
                throw new ArgumentException("单空间套餐只允许添加一个空间！");
            }

            Assert.IsFalse(currentPack.Status == ShelfStatus.Shelfed, "套餐已上架不可修改");
            //TODO 校验 
            //建筑整体定价
            if (isUnitBuilding)
                Assert.IsFalse((buildingUnitPrice.IsZeroOrMinus() || unitBuildingSquare.IsZeroOrMinus()), "请至少完整填写一套定价！");
            if (isUnitActual) //使用面积整体定价
                Assert.IsFalse((unitPrice.IsZeroOrMinus() || unitSquare.IsZeroOrMinus()), "请至少完整填写一套定价！");
            //使用建筑都未勾选
            Assert.IsFalse((!isUnitBuilding && !isUnitActual), "请至少完整填写一套定价！");

            currentPack.SetPriceByUnit(unitPrice, buildingUnitPrice, isUnitBuilding, isUnitActual, unitSquare, unitBuildingSquare, isManageFee, manageFee, isWaterElectricityFee, waterElectricityFee);
            //清空 空间超出定价
            currentPack.ClearSpacesPrice();
            if (isWhole)
            {
                currentPack.SetWholeExceedPrice(criterionSquare, premium);
            }
            else
            {
                foreach (PackSchemeSpacePricingParam spacePricingParam in spacePricingParams)
                {
                    currentPack.SetSpaceExceedPrice(spacePricingParam.PackSpaceId, spacePricingParam.DecorationPackSpacePricingType,
                        spacePricingParam.CriterionSquare, spacePricingParam.SpaceRatio, spacePricingParam.UnitPrice);
                }
            }
            currentPack.SetApplicableSquare(minApplicableSquare, maxApplicableSquare);
            this._unitOfWork.RegisterSave(currentPack);
            this._unitOfWork.UnitedCommit();
        }
        #endregion

        #region # 整体定价（清空平米定价） —— void SetPriceTotally(Guid packId, float square...
        /// <summary>
        /// 整体定价（清空平米定价,商品|工艺成本价更新为最新价格）
        /// </summary>
        /// <param name="packId">套餐模板Id</param>
        /// <param name="buildingSquare">标准面积（建筑面积）</param>
        /// <param name="isBuilding">是否整体建筑面积定价</param>
        /// <param name="isActual">是否整体使用面积定价</param>
        /// <param name="isWhole">是否整体定价超出</param>
        /// <param name="criterionSquare">标准面积（公式）</param>
        /// <param name="totalPrice">使用面积总价</param>
        /// <param name="premium">超出部分单价</param>
        /// <param name="buildingTotalPrice">建筑面积总价</param>
        /// <param name="minApplicableSquare">套餐适用的最小使用面积</param>
        /// <param name="maxApplicableSquare">套餐适用的最大使用面积</param>
        /// <param name="spacePricingParams">空间定价参数集</param>
        /// <param name="square">标准面积（使用面积）</param>
        /// <param name="isManageFee">设置管理费参考价</param>
        /// <param name="manageFee">管理费参考价</param>
        /// <param name="isWaterElectricityFee">设置水电预收参考价</param>
        /// <param name="waterElectricityFee">水电预收参考价</param>
        public void SetPriceTotally(Guid packId, decimal totalPrice, decimal buildingTotalPrice, float square, float buildingSquare, bool isBuilding, bool isActual, bool isWhole, float criterionSquare, decimal premium, float minApplicableSquare, float maxApplicableSquare, bool isManageFee, decimal manageFee, bool isWaterElectricityFee, decimal waterElectricityFee, IEnumerable<PackSchemeSpacePricingParam> spacePricingParams)
        {
            DecorationPack currentPack = this._unitOfWork.Resolve<DecorationPack>(packId);

            IEnumerable<DecorationPackSpace> spaces = _repMediator.DecorationPackSpaceRep.FindByPack(packId);

            if (currentPack.PackType == DecorationPackType.SingleSpace && spaces.Count() > 1)
            {
                throw new ArgumentException("单空间套餐只允许添加一个空间！");
            }

            Assert.IsFalse(currentPack.Status == ShelfStatus.Shelfed, "套餐已上架不可修改");
            //TODO 校验 
            //建筑整体定价
            if (isBuilding)
                Assert.IsFalse((buildingTotalPrice.IsZeroOrMinus() || buildingSquare.IsZeroOrMinus()), "请至少完整填写一套定价！");
            if (isActual) //使用面积整体定价
                Assert.IsFalse((totalPrice.IsZeroOrMinus() || square.IsZeroOrMinus()), "请至少完整填写一套定价！");
            //使用建筑都未勾选
            Assert.IsFalse((!isBuilding && !isActual), "请至少完整填写一套定价！");


            spacePricingParams = spacePricingParams == null ? new PackSchemeSpacePricingParam[0] : spacePricingParams.ToArray();
            currentPack.SetPriceTotally(square, totalPrice, buildingSquare, buildingTotalPrice, isBuilding, isActual, isManageFee, manageFee, isWaterElectricityFee, waterElectricityFee);
            //清空 空间超出定价
            currentPack.ClearSpacesPrice();

            if (isWhole)
            {
                currentPack.SetWholeExceedPrice(criterionSquare, premium);
            }
            else
            {
                foreach (PackSchemeSpacePricingParam spacePricingParam in spacePricingParams)
                {
                    currentPack.SetSpaceExceedPrice(spacePricingParam.PackSpaceId, spacePricingParam.DecorationPackSpacePricingType,
                        spacePricingParam.CriterionSquare, spacePricingParam.SpaceRatio, spacePricingParam.UnitPrice);
                }
            }
            currentPack.SetApplicableSquare(minApplicableSquare, maxApplicableSquare);
            this._unitOfWork.RegisterSave(currentPack);
            this._unitOfWork.UnitedCommit();
        }
        #endregion

        #region # 上架套餐模板 —— void OnShelf(Guid packId)
        /// <summary>
        /// 上架套餐模板
        /// </summary>
        /// <param name="packId">套餐模板Id</param>
        public void OnShelf(Guid packId)
        {
            DecorationPack currentPack = this._unitOfWork.Resolve<DecorationPack>(packId);

            IEnumerable<DecorationPackSpace> packSpaces = _repMediator.DecorationPackSpaceRep.FindByPack(packId);

            if (currentPack.PackType == DecorationPackType.SingleSpace && packSpaces.Count() > 1)
            {
                throw new ArgumentException("单空间套餐只允许添加一个空间！");
            }
            List<Guid> packSpaceIds = packSpaces.Select(x => x.Id).ToList();
            //验证
            //Assert.IsFalse(!this._repMediator.DecorationPackSkuRep.ExistsByPack(packId) && !this._repMediator.DecorationPackCraftRep.ExistsByPack(packId), "套餐模板中未配置商品或者工艺，不可上架！");
            Assert.IsTrue(this._repMediator.DecorationPackItemRep.IsDecorationPackShelfed(packId, packSpaceIds), "当前套餐中存在未配置空间或选区或商品或工艺的项，不可上架！");

            #region # 校验 ：每个空间每个选区内都配置了商品或工艺定额(详细校验作废)
            //DecorationPack currentPack = this._unitOfWork.Resolve<DecorationPack>(packId);
            //////套餐空间集
            //IEnumerable<DecorationPackSpace> packSpaces = currentPack.Spaces;
            //List<Guid> packSpaceIds = packSpaces.Select(x => x.Id).ToList();
            ////1、套餐必须有空间
            //Assert.IsFalse(!packSpaces.Any(), "套餐模板中未配置空间，不可上架！");
            ////套餐空间集
            //packSpaces.ForEach(p =>
            //{
            //    //套餐空间所有选区
            //    IEnumerable<DecorationPackItem> packItems = this._repMediator.DecorationPackItemRep.FindByPack(packId, p.Id).ToArray();
            //    //2、空间内必须有选区
            //    Assert.IsFalse(!packItems.Any(), "套餐模板空间中未配置选区，不可上架！");
            //    packItems.ForEach(packItem =>
            //    {
            //        IEnumerable<DecorationPackSku> packSkus = this._repMediator.DecorationPackSkuRep.FindByPackItem(packItem.Id);
            //        IEnumerable<DecorationPackCraft> packCrafts = this._repMediator.DecorationPackCraftRep.FindByPackItem(packItem.Id);
            //        //3、选区内必须配置商品|工艺
            //        Assert.IsFalse(!packSkus.Any() && !packCrafts.Any(), "套餐模板空间选区中未配置商品或者工艺，不可上架！");
            //    });
            //});
            #endregion

            //4、套餐必须定价
            currentPack.OnShelf();

            this._unitOfWork.RegisterSave(currentPack);
            this._unitOfWork.UnitedCommit();
        }
        #endregion

        #region # 下架套餐模板 —— void OffShelf(Guid packId)
        /// <summary>
        /// 下架套餐模板
        /// </summary>
        /// <param name="packId">套餐模板Id</param>
        public void OffShelf(Guid packId)
        {
            DecorationPack currentPack = this._unitOfWork.Resolve<DecorationPack>(packId);
            currentPack.OffShelf();

            this._unitOfWork.RegisterSave(currentPack);
            this._unitOfWork.UnitedCommit();
        }
        #endregion

        #region # 克隆套餐模板 —— Guid CloneDecorationPack(Guid sourcePackId, string packName...

        /// <summary>
        /// 克隆套餐模板
        /// </summary>
        /// <param name="sourcePackId">源套餐模板Id</param>
        /// <param name="packName">套餐模板名称</param>
        /// <param name="packType">套餐类型</param>
        /// <returns>新套餐模板Id</returns>
        /// <param name="packKind">套餐类别</param>
        /// <param name="packMode">套餐模式</param>
        /// <param name="newHouse">适用新房</param>
        /// <param name="secondHandHouse">适用二手房</param>
        /// <param name="packSeriesIds">套餐系列Id集</param>
        /// <param name="stores">门店集</param>
        /// <param name="operatorId">操作人Id</param>
        /// <param name="operator">操作人名称</param>
        /// <returns>新套餐模板Id</returns>
        public Guid CloneDecorationPack(Guid sourcePackId, string packName, DecorationPackType packType, DecorationPackKind packKind, DecorationPackMode packMode, bool newHouse,
            bool secondHandHouse, IEnumerable<Guid> packSeriesIds, Dictionary<Guid, string> stores, Guid operatorId, string @operator)
        {
            //验证
            Assert.IsFalse(this._repMediator.DecorationPackRep.ExistsPackName((Guid?)null, packName), "套餐模板名称已存在！");

            DecorationPack sourcePack = this._repMediator.DecorationPackRep.Single(sourcePackId);

            //注：此处代码是为了将导航属性加载出来
            Trace.WriteLine(sourcePack.Spaces.Count);

            //注：此处代码是为了将导航属性加载出来
            foreach (DecorationPackSpace space in sourcePack.Spaces)
            {
                //注：此处代码是为了将导航属性加载出来
                Trace.WriteLine(space.SpaceDetails.Count);
            }

            //声明套餐模板副本Id
            Guid clonedPackId = Guid.NewGuid();

            //声明套餐模板空间Id映射关系字典
            IDictionary<Guid, Guid> spaceMaps = new Dictionary<Guid, Guid>();

            #region # 克隆套餐模板、套餐模板空间、空间详情

            DecorationPack clonedPack = sourcePack.Clone<DecorationPack>();
            clonedPack.SetId(clonedPackId);
            clonedPack.SetSourcePackIdAndVersion(null, string.Empty, false);
            clonedPack.UpdateInfo(packName);
            clonedPack.SetApplicableScope(newHouse, secondHandHouse);
            clonedPack.SetStores(stores);
            //清空户型楼盘
            clonedPack.SetHouseTypes(new List<Tuple<Guid, string, Guid, string, string>>());
            clonedPack.SetPropertys(new Dictionary<Guid, string>());

            IDictionary<Guid, PackSeries> packSeries = _unitOfWork.GetPackSeriesByIds(packSeriesIds);
            if (packSeries.Values.Contains(null))
            {
                throw new ArgumentException(string.Format("Id为{0}的套餐系列不存在 , 请联系管理员 ! ", packSeries.First(s => s.Value == null).Key));
            }
            clonedPack.SetPackSeries(packSeries.Values);
            //排序处理
            lock (_Sync)
            {
                float maxSort = this._repMediator.DecorationPackRep.GetMaxSort();
                clonedPack.SetSort(maxSort + 1);
            }
            var cloneArticles = new List<Article>();
            var packArticles = this._repMediator.ArticleRepository.GetArticlesByPackId(sourcePackId).ToList();
            foreach (var article in packArticles.Where(s=>s.SpaceId == null))
            {
                Article cloneArticle = article.Clone<Article>();
                cloneArticle.SetId(Guid.NewGuid());
                cloneArticle.SetPackId(clonedPackId);
                cloneArticles.Add(cloneArticle);
            }
            //套餐模板空间Id重新赋值
            foreach (DecorationPackSpace space in clonedPack.Spaces)
            {

                Guid newPackSpaceId = Guid.NewGuid();
                spaceMaps.Add(space.Id, newPackSpaceId);
                space.SetId(newPackSpaceId);

                foreach (DecorationPackSpaceDetail spaceDetail in space.SpaceDetails)
                {
                    spaceDetail.SetId(Guid.NewGuid());
                }

                var spaceArticles = packArticles.Where(s => s.SpaceId == space.Id);
                foreach (var spaceArticle in spaceArticles)
                {
                    Article cloneArticle = spaceArticle.Clone<Article>();
                    cloneArticle.SetId(Guid.NewGuid());
                    cloneArticle.SetPackId(clonedPackId);
                    cloneArticles.Add(cloneArticle);
                }
            }

            clonedPack.UpdatePackInfo(packType, packKind, packMode, operatorId, @operator);
            this._unitOfWork.RegisterAdd(clonedPack);
            if (cloneArticles.Any())
            {
                this._unitOfWork.RegisterAddRange(cloneArticles);
            }
          
            #endregion

            #region # 克隆套餐模板项

            IEnumerable<DecorationPackItem> packItems = this._repMediator.DecorationPackItemRep.FindByPack(sourcePackId, null);

            foreach (DecorationPackItem packItem in packItems)
            {
                //注：此处代码是为了将导航属性加载出来
                Trace.WriteLine(packItem.PackSkus.Count);
                Trace.WriteLine(packItem.PackCraftEntities.Count);

                //克隆套餐模板项
                DecorationPackItem clonedPackItem = packItem.Clone<DecorationPackItem>();
                clonedPackItem.SetId(Guid.NewGuid());
                clonedPackItem.SetPackId(clonedPackId);
                clonedPackItem.SetPackSpaceId(spaceMaps[clonedPackItem.PackSpaceId]);
                //重新为Id赋值
                foreach (DecorationPackSku packSku in clonedPackItem.PackSkus)
                {
                    packSku.SetId(Guid.NewGuid());
                }
                foreach (DecorationPackCraft packCraft in clonedPackItem.PackCraftEntities)
                {
                    packCraft.SetId(Guid.NewGuid());
                }

                this._unitOfWork.RegisterAdd(clonedPackItem);
            }

            #endregion

            #region # 克隆套餐模板方案

            IEnumerable<DecorationPackScheme> packSchemes = this._repMediator.DecorationPackSchemeRep.FindByPack(sourcePackId);

            foreach (DecorationPackScheme packScheme in packSchemes)
            {
                //注：此处代码是为了将导航属性加载出来
                Trace.WriteLine(packScheme.SchemeSpaces.Count);

                //克隆套餐模板方案
                DecorationPackScheme clonedPackScheme = packScheme.Clone<DecorationPackScheme>();
                clonedPackScheme.SetId(Guid.NewGuid());
                clonedPackScheme.SetPackId(clonedPackId);

                foreach (DecorationPackSchemeSpace schemeSpace in clonedPackScheme.SchemeSpaces)
                {
                    schemeSpace.SetId(Guid.NewGuid());
                    schemeSpace.SetPackSpaceId(spaceMaps[schemeSpace.PackSpaceId]);
                }

                this._unitOfWork.RegisterAdd(clonedPackScheme);
            }

            #endregion

            #region # 克隆套餐关联大包/定制

            IEnumerable<DecorationPack_BalePack> decorationPackBalePacks =
                this._repMediator.DecorationPack_BalePackRep.FindByDecorationPack(sourcePackId);

            foreach (DecorationPack_BalePack decorationPackBalePack in decorationPackBalePacks)
            {
                DecorationPack_BalePack clonedDecorationPackBalePack = decorationPackBalePack.Clone<DecorationPack_BalePack>();
                clonedDecorationPackBalePack.SetId(Guid.NewGuid());
                clonedDecorationPackBalePack.SetDecorationPackId(clonedPackId);
                this._unitOfWork.RegisterAdd(clonedDecorationPackBalePack);
            }
            #endregion

            this._unitOfWork.UnitedCommit();
            return clonedPackId;
        }
        #endregion

        #region # 创建套餐模板方案 —— void CreatePackScheme(Guid packId, string schemeName...
        /// <summary>
        /// 创建套餐模板方案
        /// </summary>
        /// <param name="packId">套餐模板Id</param>
        /// <param name="schemeName">套餐模板方案名称</param>
        /// <param name="cover">封面</param>
        /// <param name="description">描述</param>
        /// <param name="images">方案概况图片集</param>
        /// <param name="schemeDescriptions">方案概况描述</param>
        /// <param name="videoAudioLink">视频音频地址链接</param>
        /// <param name="videoAudioFileName">视频音频文件名称</param>
        /// <param name="videoAudioFileSize">视频音频文件大小</param>
        /// <param name="videoAudioFileId">视频音频文件Id</param>
        /// <param name="videoAudiogFileSuffix">视频音频文件扩展名</param>
        /// <param name="schemeSpaceParams">套餐模板方案空间参数模型集</param>
        public void CreatePackScheme(Guid packId, string schemeName, string cover, string description, IList<string> images, IList<string> schemeDescriptions, string videoAudioLink, string videoAudioFileSize, string videoAudioFileId, string videoAudiogFileSuffix, string videoAudioFileName, IEnumerable<PackSchemeSpaceParam> schemeSpaceParams)
        {
            //验证
            schemeSpaceParams = schemeSpaceParams == null ? new PackSchemeSpaceParam[0] : schemeSpaceParams.ToArray();
            Assert.IsTrue(schemeSpaceParams.Any(), "套餐模板方案空间集不可为空！");
            Assert.IsFalse(this._repMediator.DecorationPackSchemeRep.ExistsName(packId, null, schemeName), "方案名称已存在！");

            DecorationPack currentPack = this._unitOfWork.Resolve<DecorationPack>(packId);
            //Assert.IsFalse(currentPack.Status == ShelfStatus.Shelfed, "套餐已上架不可修改");

            #region # 验证套餐模板空间

            IEnumerable<Guid> sourceSpaceIds = currentPack.Spaces.Select(x => x.Id);
            IEnumerable<Guid> targetSpaceIds = schemeSpaceParams.Select(x => x.PackSpaceId);

            if (!sourceSpaceIds.EqualsTo(targetSpaceIds))
            {
                throw new InvalidOperationException("套餐模板方案空间与套餐模板空间不符！");
            }

            #endregion
            //首个方案设置成默认方案
            bool isDefault = !this._repMediator.DecorationPackSchemeRep.ExistsScheme(packId);



            IEnumerable<DecorationPackSchemeSpace> schemeSpaces = schemeSpaceParams.Select(x => new DecorationPackSchemeSpace(x.PackSpaceId, x.Descriptions, x.Sort, x.Images, x.VideoAudioLink, x.VideoAudioFileSize, x.VideoAudioFileId, x.VideoAudiogFileSuffix, x.VideoAudioFileName));
            DecorationPackScheme packScheme = new DecorationPackScheme(schemeName, packId, cover, description, images, schemeDescriptions, videoAudioLink, videoAudioFileSize, videoAudioFileId, videoAudiogFileSuffix, videoAudioFileName, isDefault, schemeSpaces);

            this._unitOfWork.RegisterAdd(packScheme);
            this._unitOfWork.UnitedCommit();
        }
        #endregion

        #region # 修改套餐模板方案 —— void UpdatePackScheme(Guid packSchemeId, string schemeName...
        /// <summary>
        /// 修改套餐模板方案
        /// </summary>
        /// <param name="packSchemeId">套餐模板方案Id</param>
        /// <param name="schemeName">套餐模板方案名称</param>
        /// <param name="cover">封面</param>
        /// <param name="description">描述</param>
        /// <param name="images">方案概况图片集</param>
        /// <param name="schemeDescriptions">方案概况描述</param>
        /// <param name="videoAudioLink">视频音频地址链接</param>
        /// <param name="videoAudioFileName">视频音频文件名称</param>
        /// <param name="videoAudioFileSize">视频音频文件大小</param>
        /// <param name="videoAudioFileId">视频音频文件Id</param>
        /// <param name="videoAudiogFileSuffix">视频音频文件扩展名</param>
        /// <param name="schemeSpaceParams">套餐模板方案空间参数模型集</param>
        public void UpdatePackScheme(Guid packSchemeId, string schemeName, string cover, string description, IList<string> images, IList<string> schemeDescriptions, string videoAudioLink, string videoAudioFileSize, string videoAudioFileId, string videoAudiogFileSuffix, string videoAudioFileName, IEnumerable<PackSchemeSpaceParam> schemeSpaceParams)
        {
            //验证
            schemeSpaceParams = schemeSpaceParams == null ? new PackSchemeSpaceParam[0] : schemeSpaceParams.ToArray();
            Assert.IsTrue(schemeSpaceParams.Any(), "套餐模板方案空间集不可为空！");

            DecorationPackScheme currentPackScheme = this._unitOfWork.Resolve<DecorationPackScheme>(packSchemeId);
            DecorationPack currentPack = this._unitOfWork.Resolve<DecorationPack>(currentPackScheme.PackId);
            //Assert.IsFalse(currentPack.Status == ShelfStatus.Shelfed, "套餐已上架不可修改");

            //验证名称重复
            Assert.IsFalse(this._repMediator.DecorationPackSchemeRep.ExistsName(currentPackScheme.PackId, currentPackScheme.Id, schemeName), "方案名称已存在！");

            #region # 验证套餐模板空间

            IEnumerable<Guid> sourceSpaceIds = currentPack.Spaces.Select(x => x.Id);
            IEnumerable<Guid> targetSpaceIds = schemeSpaceParams.Select(x => x.PackSpaceId);

            if (!sourceSpaceIds.EqualsTo(targetSpaceIds))
            {
                throw new InvalidOperationException("套餐模板方案空间与套餐模板空间不符！");
            }

            #endregion


            IEnumerable<DecorationPackSchemeSpace> schemeSpaces = schemeSpaceParams.Select(x => new DecorationPackSchemeSpace(x.PackSpaceId, x.Descriptions, x.Sort, x.Images, x.VideoAudioLink, x.VideoAudioFileSize, x.VideoAudioFileId, x.VideoAudiogFileSuffix, x.VideoAudioFileName));
            currentPackScheme.UpdateInfo(schemeName, cover, description, images, schemeDescriptions, videoAudioLink, videoAudioFileSize, videoAudioFileId, videoAudiogFileSuffix, videoAudioFileName, schemeSpaces);

            this._unitOfWork.RegisterSave(currentPackScheme);
            this._unitOfWork.UnitedCommit();
        }
        #endregion

        #region # 删除套餐模板方案 —— void RemovePackScheme(Guid packSchemeId)
        /// <summary>
        /// 删除套餐模板方案
        /// </summary>
        /// <param name="packSchemeId">套餐模板方案Id</param>
        public void RemovePackScheme(Guid packSchemeId)
        {
            DecorationPackScheme currentPackScheme = this._repMediator.DecorationPackSchemeRep.Single(packSchemeId);

            #region # 删除默认方案，则将其他方案设置成默认方案 处理

            if (currentPackScheme.IsDefault)
            {
                //获取套餐其他方案
                DecorationPackScheme bakScheme = this._repMediator.DecorationPackSchemeRep.FirstOrDefault(currentPackScheme.PackId, currentPackScheme.Id);
                if (bakScheme != null)
                {
                    bakScheme.SetDefault(true);
                    this._unitOfWork.RegisterSave(currentPackScheme);
                }
            }

            #endregion

            this._unitOfWork.RegisterRemove<DecorationPackScheme>(packSchemeId);
            this._unitOfWork.UnitedCommit();
        }
        #endregion

        #region # 设置套餐模板选购封面（20181102设置方案图为默认） —— void SetCover(Guid packId, Guid packSchemeId)

        /// <summary>
        /// 设置套餐模板选购封面（20181102设置方案图为默认）
        /// </summary>
        /// <param name="packId">套餐模板Id</param>
        /// <param name="packSchemeId">套餐模板方案Id</param>
        public void SetCover(Guid packId, Guid packSchemeId)
        {

            //获取原默认方案
            DecorationPackScheme defaultScheme = this._unitOfWork.ResolveDefaultPackSchemeByPack(packId);
            if (defaultScheme != null)
            {
                defaultScheme.SetDefault(false);
                this._unitOfWork.RegisterSave(defaultScheme);
            }

            DecorationPackScheme currentPackScheme = this._unitOfWork.Resolve<DecorationPackScheme>(packSchemeId);
            currentPackScheme.SetDefault(true);
            this._unitOfWork.RegisterSave(currentPackScheme);
            this._unitOfWork.UnitedCommit();
        }
        #endregion

        #region # 保存套餐模板标签 —— void SavePackLabels(Guid packId, DecorationPackColor color...

        /// <summary>
        /// 保存套餐模板标签
        /// </summary>
        /// <param name="packId">套餐模板Id</param>
        /// <param name="color">颜色</param>
        /// <param name="layouts">居室集</param>
        /// <param name="styleNos">设计风格集</param>
        /// <param name="houseTypes">户型集(户型Id|户型名称，楼盘Id，楼盘名称,楼盘地址)</param>
        /// <param name="newHouse">适用新房</param>
        /// <param name="secondHandHouse">适用二手房</param>
        /// <param name="packSeriesIds">套餐系列Id集</param>
        /// <param name="stores">门店集</param>
        public void SavePackLabels(Guid packId, DecorationPackColor color, IEnumerable<string> layouts, Dictionary<string, string> styleNos,
            IEnumerable<Tuple<Guid, string, Guid, string, string>> houseTypes, bool newHouse, bool secondHandHouse, IEnumerable<Guid> packSeriesIds, Dictionary<Guid, string> stores)
        {
            //验证
            if (!newHouse && !secondHandHouse)
            {
                throw new InvalidOperationException("请设置套餐适用于哪一类房源 ! ");
            }
            layouts = layouts ?? new string[0];
            styleNos = styleNos ?? new Dictionary<string, string>();
            houseTypes = houseTypes ?? new List<Tuple<Guid, string, Guid, string, string>>();
            packSeriesIds = packSeriesIds ?? new List<Guid>();
            stores = stores ?? new Dictionary<Guid, string>();
            DecorationPack currentPack = this._unitOfWork.Resolve<DecorationPack>(packId);
            //Assert.IsFalse(currentPack.Status == ShelfStatus.Shelfed, "套餐已上架不可修改");
            currentPack.SetColor(color);
            currentPack.SetLayouts(layouts);
            currentPack.SetStyleNos(styleNos);
            currentPack.SetHouseTypes(houseTypes);
            currentPack.SetApplicableScope(newHouse, secondHandHouse);
            IDictionary<Guid, PackSeries> packSeries = _unitOfWork.GetPackSeriesByIds(packSeriesIds);
            if (packSeries.Values.Contains(null))
            {
                throw new ArgumentException(string.Format("Id为{0}的套餐系列不存在 , 请联系管理员 ! ", packSeries.First(s => s.Value == null).Key));
            }
            currentPack.SetPackSeries(packSeries.Values);
            currentPack.SetStores(stores);

            this._unitOfWork.RegisterSave(currentPack);
            this._unitOfWork.UnitedCommit();
        }

        #endregion

        #region # 替换商品SKU —— void ReplaceSku(Guid sourceSkuId, Guid targetSkuId...

        /// <summary>
        /// 替换商品SKU（skuId修改|变价|上架处理）
        /// </summary>
        /// <param name="sourceSkuId">源商品SKU Id</param>
        /// <param name="targetSkuId">模板商品SKU Id</param>
        /// <param name="costPrice">新商品原价</param>
        /// <param name="packIds">套餐模板Id集</param>
        public void ReplaceSku(Guid sourceSkuId, Guid targetSkuId, decimal costPrice, IEnumerable<Guid> packIds)
        {
            packIds = packIds ?? new Guid[0];
            //所有套餐
            List<DecorationPack> currentPacks = this._unitOfWork.GetDecorationPacks(packIds).ToList();
            foreach (Guid packId in packIds)
            {
                //DecorationPack currentPack = this._unitOfWork.Resolve<DecorationPack>(packId);
                DecorationPack currentPack = currentPacks.Single(x => x.Id == packId);
                ShelfStatus oldStatus = currentPack.Status;
                //if (oldStatus == ShelfStatus.Shelfed)
                //    currentPack.OffShelf();
                bool isChanged = false;
                IEnumerable<DecorationPackItem> packItems = this._unitOfWork.ResolvePackItemsByPack(packId);
                foreach (DecorationPackItem packItem in packItems)
                {
                    if (packItem.ExistsSku(sourceSkuId))
                    {
                        isChanged = true;
                        packItem.ReplaceSku(sourceSkuId, targetSkuId, costPrice);
                        this._unitOfWork.RegisterSave(packItem);
                    }
                }

                #region # 处理套餐 下架|变价标识
                //所有工艺和商品是否都没变价
                if (packItems.All(x => x.AllSkuNoChanged && x.AllCraftNoChanged))
                {
                    currentPack.SetHasChangedSku(false);
                }
                else
                {
                    currentPack.SetHasChangedSku(true);
                }
                //所有工艺和商品是否都上架
                if (packItems.All(x => x.AllSkuShelved && x.AllCraftShelved))
                {
                    currentPack.SetHasOffShelvedSku(false);
                }
                else
                {
                    currentPack.SetHasOffShelvedSku(true);
                }
                #endregion
                if (oldStatus == ShelfStatus.Shelfed && isChanged)
                    currentPack.OnShelf();
                this._unitOfWork.RegisterSave(currentPack);
            }

            this._unitOfWork.UnitedCommit();
        }
        #endregion

        #region # 删除商品SKU —— void RemoveSku(Guid sourceSkuId...
        /// <summary>
        /// 删除商品SKU
        /// </summary>
        /// <param name="sourceSkuId">源商品SKU Id</param>
        /// <param name="packIds">套餐模板Id集</param>
        public void RemoveSku(Guid sourceSkuId, IEnumerable<Guid> packIds)
        {
            packIds = packIds ?? new Guid[0];
            //所有商品
            List<DecorationPackSku> currentPackSkus = this._repMediator.DecorationPackSkuRep.GetSkus(new List<Guid> { sourceSkuId }).ToList();

            //判断是否包含不默认商品
            if (currentPackSkus.Any(x => !x.Default))
            {
                //非默认商品涉及套餐 和勾选 取交集
                List<Guid> currentPackIds = currentPackSkus.Where(x => !x.Default).Select(x => x.PackItem.PackId).Distinct().ToList();
                List<Guid> changedPackIds = packIds.Intersect(currentPackIds).ToList();
                //所有套餐
                List<DecorationPack> currentPacks = this._unitOfWork.GetDecorationPacks(changedPackIds).ToList();
                foreach (Guid packId in changedPackIds)
                {
                    DecorationPack currentPack = currentPacks.Single(x => x.Id == packId);
                    ShelfStatus oldStatus = currentPack.Status;
                    bool isChanged = false;
                    IEnumerable<DecorationPackItem> packItems = this._unitOfWork.ResolvePackItemsByPack(packId);
                    foreach (DecorationPackItem packItem in packItems)
                    {
                        //存在并不是默认
                        if (packItem.ExistsNoDefaultSku(new List<Guid> { sourceSkuId }))
                        {
                            isChanged = true;
                            packItem.RemoveSku(sourceSkuId);
                            this._unitOfWork.RegisterSave(packItem);
                        }
                    }

                    #region # 处理套餐 下架|变价标识
                    //所有工艺和商品是否都没变价
                    if (packItems.All(x => x.AllSkuNoChanged && x.AllCraftNoChanged))
                    {
                        currentPack.SetHasChangedSku(false);
                    }
                    else
                    {
                        currentPack.SetHasChangedSku(true);
                    }
                    //所有工艺和商品是否都上架
                    if (packItems.All(x => x.AllSkuShelved && x.AllCraftShelved))
                    {
                        currentPack.SetHasOffShelvedSku(false);
                    }
                    else
                    {
                        currentPack.SetHasOffShelvedSku(true);
                    }

                    #endregion

                    if (oldStatus == ShelfStatus.Shelfed && isChanged)
                        currentPack.OnShelf();
                    this._unitOfWork.RegisterSave(currentPack);
                }

                this._unitOfWork.UnitedCommit();
            }
        }
        #endregion

        #region # 批量删除商品SKU —— void RemoveAllSku(Dictionary<Guid, List<Guid>> skuPackIds
        /// <summary>
        /// 批量删除商品SKU
        /// </summary>
        /// <param name="skuPackIds">商品SKU Id|套餐模板Id集</param>
        public void RemoveAllSku(Dictionary<Guid, List<Guid>> skuPackIds)
        {

            skuPackIds = skuPackIds ?? new Dictionary<Guid, List<Guid>>();
            //TODO 优化 
            //套餐去重集合 循环 再处理商品
            //下架商品勾选套餐
            List<Guid> allPackIds = skuPackIds.SelectMany(x => x.Value).Distinct().ToList();
            List<Guid> allSkuIds = skuPackIds.Keys.ToList();
            //所有商品
            List<DecorationPackSku> currentPackSkus = this._repMediator.DecorationPackSkuRep.GetSkus(allSkuIds).ToList();
            //判断是否包含不默认商品
            if (currentPackSkus.Any(x => !x.Default))
            {
                //非默认商品涉及套餐 和勾选 取交集
                List<Guid> currentPackIds = currentPackSkus.Where(x => !x.Default).Select(x => x.PackItem.PackId).Distinct().ToList();
                List<Guid> changedPackIds = allPackIds.Intersect(currentPackIds).ToList();

                //所有套餐
                List<DecorationPack> currentPacks = this._unitOfWork.GetDecorationPacks(changedPackIds).ToList();
                #region 套餐
                foreach (Guid packId in changedPackIds)
                {
                    //查找传参下架商品
                    List<Guid> skuIds = skuPackIds.Where(x => x.Value.Contains(packId)).Select(x => x.Key).ToList();
                    DecorationPack currentPack = currentPacks.Single(x => x.Id == packId);
                    ShelfStatus oldStatus = currentPack.Status;
                    bool isChanged = false;
                    IEnumerable<DecorationPackItem> packItems = this._unitOfWork.ResolvePackItemsByPack(packId);
                    foreach (DecorationPackItem packItem in packItems)
                    {
                        if (packItem.ExistsNoDefaultSku(skuIds))
                        {
                            isChanged = true;
                            packItem.RemoveSkus(skuIds);
                            this._unitOfWork.RegisterSave(packItem);
                        }
                    }

                    #region # 处理套餐 下架|变价标识

                    //套餐下所有选区（选区1，选区2……）
                    //所有工艺和商品是否都没变价
                    if (packItems.All(x => x.AllSkuNoChanged && x.AllCraftNoChanged))
                    {
                        currentPack.SetHasChangedSku(false);
                    }
                    else
                    {
                        currentPack.SetHasChangedSku(true);
                    }
                    //所有工艺和商品是否都上架
                    if (packItems.All(x => x.AllSkuShelved && x.AllCraftShelved))
                    {
                        currentPack.SetHasOffShelvedSku(false);
                    }
                    else
                    {
                        currentPack.SetHasOffShelvedSku(true);
                    }

                    #endregion


                    if (oldStatus == ShelfStatus.Shelfed && isChanged)
                    {
                        currentPack.OnShelf();
                    }

                    this._unitOfWork.RegisterSave(currentPack);
                }
                #endregion
                this._unitOfWork.UnitedCommit();

            }


        }
        #endregion

        #region # 替换工艺实体 —— void ReplaceCraft(Guid sourceCraftEntityId, Guid targetCraftEntityId...

        /// <summary>
        /// 替换工艺实体
        /// </summary>
        /// <param name="sourceCraftEntityId">源工艺实体Id</param>
        /// <param name="targetCraftEntityId">模板工艺实体Id</param>
        /// <param name="costPrice">新工艺成本价</param>
        /// <param name="packIds">套餐模板Id集</param>
        public void ReplaceCraft(Guid sourceCraftEntityId, Guid targetCraftEntityId, decimal costPrice, IEnumerable<Guid> packIds)
        {
            packIds = packIds ?? new Guid[0];
            //所有套餐
            List<DecorationPack> currentPacks = this._unitOfWork.GetDecorationPacks(packIds).ToList();
            foreach (Guid packId in packIds)
            {
                //DecorationPack currentPack = this._unitOfWork.Resolve<DecorationPack>(packId);
                DecorationPack currentPack = currentPacks.Single(x => x.Id == packId);
                ShelfStatus oldStatus = currentPack.Status;
                //if (oldStatus == ShelfStatus.Shelfed)
                //    currentPack.OffShelf();
                IEnumerable<DecorationPackItem> packItems = this._unitOfWork.ResolvePackItemsByPack(packId);
                bool isChanged = false;
                foreach (DecorationPackItem packItem in packItems)
                {
                    if (packItem.ExistsCraft(sourceCraftEntityId))
                    {
                        isChanged = true;
                        packItem.ReplaceCraft(sourceCraftEntityId, targetCraftEntityId, costPrice);
                        this._unitOfWork.RegisterSave(packItem);
                    }
                }

                #region # 处理套餐 下架|变价标识
                //所有工艺和商品是否都没变价
                if (packItems.All(x => x.AllSkuNoChanged && x.AllCraftNoChanged))
                {
                    currentPack.SetHasChangedSku(false);
                }
                else
                {
                    currentPack.SetHasChangedSku(true);
                }
                //所有工艺和商品是否都上架
                if (packItems.All(x => x.AllSkuShelved && x.AllCraftShelved))
                {
                    currentPack.SetHasOffShelvedSku(false);
                }
                else
                {
                    currentPack.SetHasOffShelvedSku(true);
                }
                #endregion
                if (oldStatus == ShelfStatus.Shelfed && isChanged)
                    currentPack.OnShelf();
                this._unitOfWork.RegisterSave(currentPack);
            }

            this._unitOfWork.UnitedCommit();
        }
        #endregion

        #region # 删除工艺实体 —— void RemoveCraft(Guid sourceCraftEntityId...
        /// <summary>
        /// 删除工艺实体
        /// </summary>
        /// <param name="sourceCraftEntityId">源工艺实体Id</param>
        /// <param name="packIds">套餐模板Id集</param>
        public void RemoveCraft(Guid sourceCraftEntityId, IEnumerable<Guid> packIds)
        {
            packIds = packIds ?? new Guid[0];
            //所有工艺
            List<DecorationPackCraft> currentPackCrafts = this._repMediator.DecorationPackCraftRep.GetCrafts(new List<Guid> { sourceCraftEntityId }).ToList();
            //判断是否包含不默认工艺
            if (currentPackCrafts.Any(x => !x.Default))
            {
                //所有套餐
                List<DecorationPack> currentPacks = this._unitOfWork.GetDecorationPacks(packIds).ToList();
                foreach (Guid packId in packIds)
                {
                    //DecorationPack currentPack = this._unitOfWork.Resolve<DecorationPack>(packId);
                    DecorationPack currentPack = currentPacks.Single(x => x.Id == packId);
                    ShelfStatus oldStatus = currentPack.Status;
                    //if (oldStatus == ShelfStatus.Shelfed)
                    //    currentPack.OffShelf();
                    IEnumerable<DecorationPackItem> packItems = this._unitOfWork.ResolvePackItemsByPack(packId);
                    bool isChanged = false;
                    foreach (DecorationPackItem packItem in packItems)
                    {
                        //存在并不是默认
                        if (packItem.ExistsNoDefaultCraft(new List<Guid> { sourceCraftEntityId }))
                        {
                            isChanged = true;
                            packItem.RemoveCraft(sourceCraftEntityId);
                            this._unitOfWork.RegisterSave(packItem);
                        }
                    }

                    #region # 处理套餐 下架|变价标识

                    //套餐下所有选区（选区1，选区2……）
                    //所有工艺和商品是否都没变价
                    if (packItems.All(x => x.AllSkuNoChanged && x.AllCraftNoChanged))
                    {
                        currentPack.SetHasChangedSku(false);
                    }
                    else
                    {
                        currentPack.SetHasChangedSku(true);
                    }
                    //所有工艺和商品是否都上架
                    if (packItems.All(x => x.AllSkuShelved && x.AllCraftShelved))
                    {
                        currentPack.SetHasOffShelvedSku(false);
                    }
                    else
                    {
                        currentPack.SetHasOffShelvedSku(true);
                    }

                    #endregion

                    if (oldStatus == ShelfStatus.Shelfed && isChanged)
                        currentPack.OnShelf();

                    this._unitOfWork.RegisterSave(currentPack);
                }
            }

            this._unitOfWork.UnitedCommit();
        }
        #endregion

        #region # 批量删除工艺实体 —— void RemoveCraft(Guid sourceCraftEntityId...
        /// <summary>
        /// 批量删除工艺实体
        /// </summary>
        /// <param name="craftPackIds">工艺实体Id|套餐模板Id集</param>
        public void RemoveAllCraft(Dictionary<Guid, List<Guid>> craftPackIds)
        {
            craftPackIds = craftPackIds ?? new Dictionary<Guid, List<Guid>>();
            //TODO 优化 
            //套餐去重集合 循环 再处理工艺
            //下架商品勾选套餐
            List<Guid> allPackIds = craftPackIds.SelectMany(x => x.Value).Distinct().ToList();
            List<Guid> allCraftIds = craftPackIds.Keys.ToList();
            //所有工艺
            List<DecorationPackCraft> currentPackCrafts = this._repMediator.DecorationPackCraftRep.GetCrafts(allCraftIds).ToList();
            //判断是否包含不默认工艺
            if (currentPackCrafts.Any(x => !x.Default))
            {
                //非默认商品涉及套餐 和勾选 取交集
                List<Guid> currentPackIds = currentPackCrafts.Where(x => !x.Default).Select(x => x.PackItem.PackId).Distinct().ToList();
                List<Guid> changedPackIds = allPackIds.Intersect(currentPackIds).ToList();

                //所有套餐
                List<DecorationPack> currentPacks = this._unitOfWork.GetDecorationPacks(changedPackIds).ToList();
                foreach (Guid packId in changedPackIds)
                {
                    //查找传参下架工艺
                    List<Guid> craftIds = craftPackIds.Where(x => x.Value.Contains(packId)).Select(x => x.Key).ToList();
                    DecorationPack currentPack = currentPacks.Single(x => x.Id == packId);
                    IEnumerable<DecorationPackItem> packItems = this._unitOfWork.ResolvePackItemsByPack(packId);
                    bool isChanged = false;
                    foreach (DecorationPackItem packItem in packItems)
                    {
                        if (packItem.ExistsNoDefaultCraft(craftIds))
                        {
                            isChanged = true;
                            packItem.RemoveCrafts(craftIds);
                            this._unitOfWork.RegisterSave(packItem);
                        }
                    }
                    #region # 处理套餐 下架|变价标识
                    //套餐下所有选区（选区1，选区2……）
                    //所有工艺和商品是否都没变价
                    if (packItems.All(x => x.AllSkuNoChanged && x.AllCraftNoChanged))
                    {
                        currentPack.SetHasChangedSku(false);
                    }
                    else
                    {
                        currentPack.SetHasChangedSku(true);
                    }
                    //所有工艺和商品是否都上架
                    if (packItems.All(x => x.AllSkuShelved && x.AllCraftShelved))
                    {
                        currentPack.SetHasOffShelvedSku(false);
                    }
                    else
                    {
                        currentPack.SetHasOffShelvedSku(true);
                    }
                    #endregion
                    ShelfStatus oldStatus = currentPack.Status;
                    if (oldStatus == ShelfStatus.Shelfed && isChanged)
                    {
                        currentPack.OnShelf();
                    }
                    this._unitOfWork.RegisterSave(currentPack);
                }
                this._unitOfWork.UnitedCommit();
            }

        }
        #endregion

        #region # 关联大包/定制模板 —— void RelateBalePack(Guid decorationPackId, Guid balePackId)
        /// <summary>
        /// 关联大包/定制模板
        /// </summary>
        /// <param name="decorationPackId">套餐模板Id</param>
        /// <param name="balePackId">大包/定制模板Id</param>
        public void RelateBalePack(Guid decorationPackId, Guid balePackId)
        {
            //验证
            Assert.IsTrue(this._repMediator.DecorationPackRep.Exists(decorationPackId), "套餐模板不存在！");
            Assert.IsFalse(this._repMediator.DecorationPack_BalePackRep.Exists(decorationPackId, balePackId), "关联关系已存在！");

            BalePack currentBalePack = this._repMediator.BalePackRep.Single(balePackId);
            DecorationPack_BalePack relation = new DecorationPack_BalePack(decorationPackId, currentBalePack.Id, currentBalePack.BalePackType);

            this._unitOfWork.RegisterAdd(relation);
            this._unitOfWork.UnitedCommit();
        }
        #endregion

        #region # 取消关联大包/定制模板 —— void CancelRelateBalePack(Guid decorationPackId...
        /// <summary>
        /// 取消关联大包/定制模板
        /// </summary>
        /// <param name="decorationPackId">套餐模板Id</param>
        /// <param name="balePackId">大包/定制模板Id</param>
        public void CancelRelateBalePack(Guid decorationPackId, Guid balePackId)
        {
            DecorationPack_BalePack currentRelation = this._repMediator.DecorationPack_BalePackRep.Single(decorationPackId, balePackId);

            this._unitOfWork.RegisterPhysicsRemove<DecorationPack_BalePack>(currentRelation.Id);
            this._unitOfWork.Commit();
        }
        #endregion

        #region # 设置套餐浏览量 —— void SetPackViews(Guid packId)
        /// <summary>
        /// 设置套餐浏览量
        /// </summary>
        public void SetPackViews(Guid packId)
        {
            DecorationPack currentPack = this._unitOfWork.Resolve<DecorationPack>(packId);
            currentPack.SetViews();

            this._unitOfWork.RegisterSave(currentPack);
            this._unitOfWork.UnitedCommit();
        }
        #endregion

        #region # 设置套餐销售量 —— void SetPackSales(Guid packId)
        /// <summary>
        /// 设置套餐销售量
        /// </summary>
        /// <param name="packId">套餐副本Id</param>
        public void SetPackSales(Guid packId)
        {
            DecorationPack currentPack = this._unitOfWork.Resolve<DecorationPack>(packId);
            if (currentPack.SourcePackId.HasValue)
            {
                DecorationPack sourceCurrentPack = this._unitOfWork.Resolve<DecorationPack>(currentPack.SourcePackId.Value);
                sourceCurrentPack.SetSales();
                this._unitOfWork.RegisterSave(sourceCurrentPack);
            }
            currentPack.SetSales();
            this._unitOfWork.RegisterSave(currentPack);
            this._unitOfWork.UnitedCommit();
        }
        #endregion

        #region # 添加套餐项三级品类范围 —— void AddPackItemCategoryIds(Guid packItemId, IEnumerable<Guid> categoryIds)
        /// <summary>
        /// 添加套餐项三级品类范围（原三级品类集累加）
        /// </summary>
        /// <param name="packItemId">套餐项ID</param>
        /// <param name="categoryIds">三级品类集</param>
        public void AddPackItemCategoryIds(Guid packItemId, IEnumerable<Guid> categoryIds)
        {
            //验证
            Assert.IsNotNull(categoryIds, "三级品类集不可为null！");
            DecorationPackItem currentPackItem = this._unitOfWork.Resolve<DecorationPackItem>(packItemId);
            DecorationPack currentPack = this._unitOfWork.Resolve<DecorationPack>(currentPackItem.PackId);
            Assert.IsFalse(currentPack.Status == ShelfStatus.Shelfed, "套餐已上架不可修改");
            currentPackItem.AddCategoryIds(categoryIds);

            this._unitOfWork.RegisterSave(currentPackItem);
            this._unitOfWork.UnitedCommit();
        }
        #endregion

        #region # 删除套餐项三级品类配置（删除商品|处理套餐下架|变价标识） —— void DeletePackItemCategoryIds(Guid packItemId, Guid categoryId)
        /// <summary>
        /// 删除套餐项三级品类配置（删除商品|处理套餐下架|变价标识）
        /// </summary>
        /// <param name="packItemId">套餐项ID</param>
        /// <param name="categoryId">三级品类</param>
        public void DeletePackItemCategoryIds(Guid packItemId, Guid categoryId)
        {
            DecorationPackItem currentPackItem = this._unitOfWork.Resolve<DecorationPackItem>(packItemId);
            DecorationPack currentPack = this._unitOfWork.Resolve<DecorationPack>(currentPackItem.PackId);
            Assert.IsFalse(currentPack.Status == ShelfStatus.Shelfed, "套餐已上架不可修改");
            currentPackItem.DeleteCategoryIds(new List<Guid> { categoryId });
            #region # 处理套餐 下架|变价标识
            //套餐下所有选区（选区1，选区2……）
            //IEnumerable<DecorationPackItem> items = this._repMediator.DecorationPackItemRep.FindByPack(currentPackItem.PackId, null);
            IEnumerable<DecorationPackItem> packItems = this._unitOfWork.ResolvePackItemsByPack(currentPack.Id);
            //所有工艺和商品是否都没变价
            if (packItems.All(x => x.AllSkuNoChanged && x.AllCraftNoChanged))
            {
                currentPack.SetHasChangedSku(false);
            }
            else
            {
                currentPack.SetHasChangedSku(true);
            }
            //所有工艺和商品是否都上架
            if (packItems.All(x => x.AllSkuShelved && x.AllCraftShelved))
            {
                currentPack.SetHasOffShelvedSku(false);
            }
            else
            {
                currentPack.SetHasOffShelvedSku(true);
            }
            #endregion
            this._unitOfWork.RegisterSave(currentPack);
            this._unitOfWork.RegisterSave(currentPackItem);
            this._unitOfWork.UnitedCommit();
        }
        #endregion

        #region # 上传产品说明书 —— void SetInstructions(Guid packId, string instructions, string instructionsName)
        /// <summary>
        /// 上传产品说明书
        /// </summary>
        /// <param name="packId">套餐Id</param>
        /// <param name="instructions">文件Id</param>
        /// <param name="instructionsName">产品说明书名称</param>
        public void SetInstructions(Guid packId, string instructions, string instructionsName)
        {
            DecorationPack currentPack = this._unitOfWork.Resolve<DecorationPack>(packId);
            currentPack.SetInstructions(instructions, instructionsName);

            this._unitOfWork.RegisterSave(currentPack);
            this._unitOfWork.UnitedCommit();
        }
        #endregion

        #region # 修改套餐模板项设计要求 —— void SetRequirement(Guid packItemId,string requirement)
        /// <summary>
        /// 修改套餐模板项设计要求
        /// </summary>
        /// <param name="packItemId">套餐项ID</param>
        /// <param name="requirement">设计要求</param>
        public void SetRequirement(Guid packItemId, string requirement)
        {
            DecorationPackItem currentPackItem = this._unitOfWork.Resolve<DecorationPackItem>(packItemId);
            currentPackItem.SetRequirement(requirement);
            DecorationPack currentPack = this._unitOfWork.Resolve<DecorationPack>(currentPackItem.PackId);
            Assert.IsFalse(currentPack.Status == ShelfStatus.Shelfed, "套餐已上架不可修改");

            this._unitOfWork.RegisterSave(currentPackItem);
            this._unitOfWork.UnitedCommit();
        }
        #endregion

        #region # 变价套餐模板商品SKU —— void ChangedDecorationPackSku(IDictionary<Guid, decimal> skuIdCostPrices)

        /// <summary>
        /// 变价套餐模板商品SKU
        /// </summary>
        /// <param name="skuIdCostPrices">商品SKU Id|商品工程量成本价</param>
        public void ChangedDecorationPackSku(IDictionary<Guid, decimal> skuIdCostPrices)
        {

            //批量变更商品（A|B|C）
            List<Guid> skuIds = skuIdCostPrices.Keys.ToList();

            //根据SkuId查询所在的选区Id集
            List<Guid> packAllItemIds = this._repMediator.DecorationPackSkuRep.GetPackItemIdBySkuIds(skuIds).ToList();

            //涉及选区（选区1）
            List<DecorationPackItem> currentPackItems = this._unitOfWork.FindPackItemsByIds(packAllItemIds).ToList();

            //涉及套餐（套餐a）
            List<Guid> packIds = currentPackItems.Select(x => x.PackId).Distinct().ToList();
            List<DecorationPack> packs = this._unitOfWork.GetDecorationPacks(packIds).ToList();

            #region # 涉及到的套餐 批量处理不用领域事件
            foreach (DecorationPack currentPack in packs)
            {
                //套餐下所有选区（选区1，选区2……）
                List<DecorationPackItem> packItems = currentPackItems.Where(x => x.PackId == currentPack.Id).ToList();

                foreach (DecorationPackItem packItem in packItems)
                {
                    //选区下商品Key集（A,B商品）

                    packItem.PackSkus.ForEach(x =>
                    {
                        if (skuIdCostPrices.ContainsKey(x.SkuId))
                            x.CostPriceChanged(decimal.Round(skuIdCostPrices[x.SkuId], 2));
                    });

                }

                #region # 处理套餐

                //所有工艺和商品是否都没变价
                if (packItems.All(x => x.AllSkuNoChanged && x.AllCraftNoChanged))
                {
                    currentPack.SetHasChangedSku(false);
                }
                else
                {
                    currentPack.SetHasChangedSku(true);
                }


                #endregion

            }
            #endregion
            this._unitOfWork.RegisterSaveRange(currentPackItems);
            this._unitOfWork.RegisterSaveRange(packs);
            this._unitOfWork.Commit();
        }
        #endregion

        #region # 变价套餐模板工艺 —— void ChangedDecorationPackCraft(IDictionary<Guid, decimal> craftEntityIdCostPrices)

        /// <summary>
        /// 变价套餐模板工艺
        /// </summary>
        /// <param name="craftEntityIdCostPrices">工艺实体Id|工艺成本价</param>
        public void ChangedDecorationPackCraft(IDictionary<Guid, decimal> craftEntityIdCostPrices)
        {
            //批量变更工艺（A|B|C）
            List<Guid> craftIds = craftEntityIdCostPrices.Keys.ToList();

            //根据工艺Id查询所在的选区Id集
            List<Guid> packAllItemIds = this._repMediator.DecorationPackCraftRep.GetPackItemIdByCraftIds(craftIds).ToList();

            //涉及选区（选区1）
            List<DecorationPackItem> currentPackItems = this._unitOfWork.FindPackItemsByIds(packAllItemIds).ToList();

            //涉及套餐（套餐a）
            List<Guid> packIds = currentPackItems.Select(x => x.PackId).Distinct().ToList();
            List<DecorationPack> packs = this._unitOfWork.GetDecorationPacks(packIds).ToList();

            #region # 涉及到的套餐 批量处理不用领域事件
            foreach (DecorationPack currentPack in packs)
            {
                //套餐下所有选区（选区1，选区2……）
                List<DecorationPackItem> packItems = currentPackItems.Where(x => x.PackId == currentPack.Id).ToList();

                foreach (DecorationPackItem packItem in packItems)
                {
                    //选区下商品Key集（A,B工艺）

                    packItem.PackCraftEntities.ForEach(x =>
                    {
                        if (craftEntityIdCostPrices.ContainsKey(x.CraftEntityId))
                            x.CostPriceChanged(decimal.Round(craftEntityIdCostPrices[x.CraftEntityId], 2));
                    });

                }

                #region # 处理套餐

                //所有工艺和商品是否都没变价
                if (packItems.All(x => x.AllSkuNoChanged && x.AllCraftNoChanged))
                {
                    currentPack.SetHasChangedSku(false);
                }
                else
                {
                    currentPack.SetHasChangedSku(true);
                }


                #endregion

            }
            #endregion
            this._unitOfWork.RegisterSaveRange(currentPackItems);
            this._unitOfWork.RegisterSaveRange(packs);
            this._unitOfWork.Commit();

        }
        #endregion

        #region # 导入套餐空间预算(DIM) ——  void ImportDecorationPack(Guid packId,IEnumerable<PackSpaceParam> packSpaceParams)

        /// <summary>
        /// 导入套餐空间预算(DIM)
        /// </summary>
        /// <param name="packId">套餐模板Id</param>
        /// <param name="packSpaceParams">套餐空间参数集</param>
        public void ImportDecorationPack(Guid packId,IEnumerable<PackSpaceParam> packSpaceParams)
        {
            var currentPack = this._unitOfWork.Resolve<DecorationPack>(packId);
            if (currentPack.Status == ShelfStatus.Shelfed)
            {
                throw new ArgumentException("已上架的套餐不可以进行修改 ! ");
            }
            //清空原有套餐空间
            while (true)
            {
                if (!currentPack.Spaces.Any())
                {
                    break;
                }
                currentPack.RemoveSpace(currentPack.Spaces.First());
            }

            var skus = new List<ProductSkuInfo>();
            var crafts = new List<CraftEntityInfo>();
            //skuId包括必须商品和自有内部推荐商品
            var skuIds = packSpaceParams.SelectMany(s => s.PackItemParams).SelectMany(s => s.DecorationPackSkuParams).Select(s => s.SkuId)
                .Union(packSpaceParams.SelectMany(s => s.PackRecommendedItemParams).SelectMany(s => s.DecorationPackRecommendedSkuParams).Where(s => s.SkuId.HasValue)
                    .Select(s => s.SkuId.Value)).Distinct().ToList();
            if (!skuIds.IsNullOrEmpty())
            {
                skus = this._productContract.GetProductSkusByIds(skuIds).ToList();
            }
            var craftIds = packSpaceParams.SelectMany(s => s.PackItemParams).SelectMany(s => s.DecorationPackCraftParams).Select(s => s.CraftEntityId).Distinct().ToList();
            if (!craftIds.IsNullOrEmpty())
            {
                crafts = this._craftEntityContract.GetCraftEntitiesByIds(craftIds).ToList();
            }

            //新增空间
            foreach (var packSpaceParam in packSpaceParams)
            {

                IEnumerable<DecorationPackSpaceDetail> spaceDetails =
                    packSpaceParam.SpaceDetailParams.Select(
                        x =>
                            new DecorationPackSpaceDetail(x.NumericalStandard, x.GroundLength, x.GroundWidth, x.SpacePerimeter, x.WallHigh,
                                x.HoleArea, x.FacadeArea, x.GroundArea, x.CeilingArea));
                DecorationPackSpace packSpace = new DecorationPackSpace(packSpaceParam.SpaceName, packSpaceParam.Square, packSpaceParam.SpaceType, spaceDetails, packSpaceParam.Sort);
                currentPack.AddSpace(packSpace);
                //新增必选选区
                foreach (var packItemParam in packSpaceParam.PackItemParams)
                {
                    DecorationPackItem item = new DecorationPackItem(packItemParam.ItemName, packId, packSpace.Id, CategoryArea.Basic, new List<Guid>());
                
                    //商品SKU项处理
                    IEnumerable<DecorationPackSku> packSkuItems = packItemParam.DecorationPackSkuParams.Select(keyValue =>
                    {
                        var currentSku = skus.SingleOrDefault(s => s.Id == keyValue.SkuId);
                        if (currentSku == null)
                        {
                            throw  new ArgumentException(string.Format("Id为{0}的商品不存在 ! ",keyValue.SkuId));
                        }

                        return new DecorationPackSku(keyValue.SkuId, keyValue.Sort, !currentSku.IsDisable, currentSku.CategoryId, currentSku.BrandId, keyValue.Default,
                            keyValue.SkuQuantity, currentSku.QuantitiesCostPrice, false, keyValue.SkuCraftPositions);
                    });
                    //添加商品SKU集至套餐模板项
                    if (packSkuItems.Any())
                    {
                        item.AddPackSkus(packSkuItems);
                    }
                    item.SetCategoryIds(packSkuItems.Select(s=>s.CategoryId).Distinct());

                    //工艺实体项处理
                    IEnumerable<DecorationPackCraft> packCraftItems = packItemParam.DecorationPackCraftParams.Select(keyValue =>
                    {
                        var currentCraft = crafts.SingleOrDefault(s => s.Id == keyValue.CraftEntityId);
                        if (currentCraft == null)
                        {
                            throw new ArgumentException(string.Format("Id为{0}的工艺实体不存在 ! ", keyValue.CraftEntityId));
                        }

                        return new DecorationPackCraft(keyValue.CraftEntityId, keyValue.Sort, !currentCraft.IsDisable, keyValue.Default, keyValue.CraftQuantity,
                            currentCraft.CostPrice, keyValue.Changed, keyValue.SkuCraftPositions);
                    });
                    //添加工艺实体至套餐模板项
                    if (packCraftItems.Any())
                    {
                        item.AddPackCrafts(packCraftItems);
                    }
                    this._unitOfWork.RegisterAdd(item);
                }   
                //新增推荐选区
                foreach (var packRecommendedItem in packSpaceParam.PackRecommendedItemParams)
                {
                    DecorationPackRecommendedItem item = new DecorationPackRecommendedItem(packRecommendedItem.ItemName, packId, packSpace.Id, packRecommendedItem.Remark);

                    //商品SKU项处理
                    IEnumerable<DecorationPackRecommendedSku> packSkuItems = packRecommendedItem.DecorationPackRecommendedSkuParams.Select(keyValue =>
                    {
                        DecorationPackRecommendedSku packRecommendedSku;

                        if (keyValue.SkuId.HasValue || keyValue.SkuId.Value != Guid.Empty)
                        {
                            var currentSku = skus.SingleOrDefault(s => s.Id == keyValue.SkuId);
                            if (currentSku == null)
                            {
                                throw new ArgumentException(string.Format("Id为{0}的商品不存在 ! ", keyValue.SkuId));
                            }

                            packRecommendedSku = new DecorationPackRecommendedSku(keyValue.SkuId, currentSku.Name,  !currentSku.IsDisable, currentSku.CategoryId, currentSku.CategoryName, currentSku.BrandId, currentSku.BrandName,keyValue.Default,
                                keyValue.SkuQuantity, currentSku.QuantitiesUnit,currentSku.QuantitiesCostPrice, currentSku.QuantitiesSalePrice, keyValue.Remark,keyValue.HyperLink);
                        }
                        else
                        {
                            packRecommendedSku = new DecorationPackRecommendedSku(null, keyValue.SkuName,null, null, keyValue.CategoryName, null, keyValue.BrandName, keyValue.Default,
                                keyValue.SkuQuantity,keyValue.UnitName, keyValue.CostPrice, keyValue.SalesPrice, keyValue.Remark, keyValue.HyperLink);
                        }
                        return packRecommendedSku;
                    });
                    //添加商品SKU集至套餐模板项
                    item.AddPackSkus(packSkuItems);
                    this._unitOfWork.RegisterAdd(item);
                }
            }
            this._unitOfWork.RegisterSave(currentPack);
            this._unitOfWork.UnitedCommit();
        }                                                                                                                     
        #endregion

        //查询部分

        #region # 分页获取套餐模板列表 —— PageModel<DecorationPackInfo> GetDecorationPacks(string keywords...

        /// <summary>
        /// 分页获取套餐模板列表
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
        /// <param name="hasChangedSku">是否有变价商品</param>
        /// <param name="color">颜色</param>
        /// <param name="packType">套餐类型(单空间|空间组|家)</param>
        /// <param name="packKind">套餐类别(套餐|造型)</param>
        /// <param name="packMode">套餐模式(基础|成品|基础+成品)</param>
        /// <param name="status">状态</param>
        /// <param name="spaceTypes">空间类型集</param>
        /// <param name="styleNos">设计风格集</param>
        /// <param name="layouts">居室集</param>
        /// <param name="propertys">楼盘集</param>
        /// <param name="packSeriesIds">套餐系列集</param>
        /// <param name="stores">门店集</param>
        /// <param name="createrId">套餐创建人Id？（全部：null,我的：当前登录人员）</param>
        /// <param name="sort">排序条件 true为倒序</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页容量</param>
        /// <returns>套餐模板列表</returns>
        public PageModel<DecorationPackInfo> GetDecorationPacks(string keywords, Guid? propertyId, float? applicableSquare, bool? isNewHouse, bool isBuildingPrice,
            decimal? minPrice, decimal? maxPrice, bool isBuildingSquare,
            float? minSquare, float? maxSquare, bool? hasOffSku, bool? hasChangedSku, DecorationPackColor? color, DecorationPackType? packType,
            DecorationPackKind? packKind, DecorationPackMode? packMode, ShelfStatus? status, IList<string> spaceTypes, IList<string> styleNos, IList<string> layouts,
            IList<Guid> propertys, IList<Guid> packSeriesIds, IList<Guid> stores, bool hasStores, Guid? createrId, Dictionary<string, bool> sort, int pageIndex, int pageSize)
        {
            int rowCount, pageCount;

            IEnumerable<DecorationPack> decorationPacks = this._repMediator.DecorationPackRep.FindByPage(keywords, propertyId, applicableSquare, isNewHouse, isBuildingPrice, minPrice, maxPrice, isBuildingSquare, minSquare, maxSquare, hasOffSku, hasChangedSku, color, packType, packKind, packMode, status, spaceTypes, styleNos, layouts, propertys, true, packSeriesIds, stores, hasStores, createrId, sort, pageIndex, pageSize, out rowCount, out pageCount);
            //查询套餐默认方案
            List<Guid> packIds = decorationPacks.Select(x => x.Id).ToList();
            List<DecorationPackScheme> packSchemes = this._repMediator.DecorationPackSchemeRep.FindDefaultByPack(packIds).ToList();
            IEnumerable<DecorationPackInfo> decorationPackInfos = decorationPacks.Select(x => x.ToDTO(packSchemes));

            return new PageModel<DecorationPackInfo>(decorationPackInfos, pageIndex, pageSize, pageCount, rowCount);
        }

        #endregion


        #region # 分页获取套餐模板列表 —— PageModel<DecorationPackInfo> GetDecorationPacksForSales(string keywords, Guid? propertyId, float? applicableSquare, bool? isNewHouse,

        /// <summary>
        /// 分页获取套餐模板列表
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
        /// <param name="packKind">套餐类别(套餐|造型)</param>
        /// <param name="packMode">套餐模式(基础|成品|基础+成品)</param>
        /// <param name="spaceTypes">空间类型集</param>
        /// <param name="styleNos">设计风格集</param>
        /// <param name="layouts">居室集</param>
        /// <param name="hasPackSeries">是否有标签(全部和标签:true,其他:false)</param>
        /// <param name="packSeriesIds">套餐系列集</param>
        /// <param name="stores">门店集</param>
        /// <param name="sort">排序条件 true为倒序</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页容量</param>
        /// <returns>套餐模板列表</returns>
        public PageModel<DecorationPackInfo> GetDecorationPacksForSales(string keywords, Guid? propertyId, float? applicableSquare, bool? isNewHouse,
            bool? isBuildingPrice, decimal? minPrice, decimal? maxPrice, bool? isBuildingSquare, float? minSquare, float? maxSquare, DecorationPackType? packType, DecorationPackKind? packKind,
            DecorationPackMode? packMode, IList<string> spaceTypes, IList<string> styleNos, IList<string> layouts, bool hasPackSeries, IList<Guid> packSeriesIds, IList<Guid> stores, bool hasStores,
            Dictionary<string, bool> sort, int pageIndex, int pageSize)
        {
            int rowCount, pageCount;

            IEnumerable<DecorationPack> decorationPacks = this._repMediator.DecorationPackRep.FindByPage(keywords, propertyId, applicableSquare, isNewHouse,
                isBuildingPrice, minPrice, maxPrice, isBuildingSquare,
                minSquare, maxSquare, null, null, null, packType, packKind, packMode, ShelfStatus.Shelfed, spaceTypes, styleNos, layouts, null, hasPackSeries, packSeriesIds, stores, hasStores, null,
                sort, pageIndex,
                pageSize, out rowCount, out pageCount);
            List<Guid> packIds = decorationPacks.Select(x => x.Id).ToList();
            List<DecorationPackScheme> packSchemes = this._repMediator.DecorationPackSchemeRep.FindDefaultByPack(packIds).ToList();
            IEnumerable<DecorationPackInfo> decorationPackInfos = decorationPacks.Select(x => x.ToDTO(packSchemes));

            return new PageModel<DecorationPackInfo>(decorationPackInfos, pageIndex, pageSize, pageCount, rowCount);
        }

        #endregion

        

        #region # 获取套餐模板空间列表 —— IEnumerable<DecorationPackSpaceInfo> GetPackSpaces(Guid packId)
        /// <summary>
        /// 获取套餐模板空间列表
        /// </summary>
        /// <param name="packId">套餐模板Id</param>
        /// <returns>套餐模板空间列表</returns>
        public IEnumerable<DecorationPackSpaceInfo> GetPackSpaces(Guid packId)
        {
            IEnumerable<DecorationPackSpace> packSpaces = this._repMediator.DecorationPackSpaceRep.FindByPack(packId).ToList();
            IDictionary<Guid, bool> offShelvedSkuDic = this._repMediator.DecorationPackSkuRep.ExistsOffShelved(packSpaces.Select(x => x.Id));
            IDictionary<Guid, bool> offShelvedCraftDic = this._repMediator.DecorationPackCraftRep.ExistsOffShelved(packSpaces.Select(x => x.Id));

            IDictionary<Guid, bool> changedSkuDic = this._repMediator.DecorationPackSkuRep.ExistsChanged(packSpaces.Select(x => x.Id));
            IDictionary<Guid, bool> changedCraftDic = this._repMediator.DecorationPackCraftRep.ExistsChanged(packSpaces.Select(x => x.Id));

            IEnumerable<DecorationPackSpaceInfo> packSpaceInfos = packSpaces.Select(x => x.ToDTO(offShelvedSkuDic, offShelvedCraftDic, changedSkuDic, changedCraftDic));

            return packSpaceInfos;
        }
        #endregion

        #region # 获取套餐模板项列表 —— IEnumerable<DecorationPackItemInfo> GetPackItems(Guid packId...
        /// <summary>
        /// 获取套餐模板项列表
        /// </summary>
        /// <param name="packId">套餐模板Id</param>
        /// <param name="packSpaceId">套餐模板空间Id</param>
        /// <returns>套餐模板项列表</returns>
        public IEnumerable<DecorationPackItemInfo> GetPackItems(Guid packId, Guid? packSpaceId)
        {
            IEnumerable<DecorationPackItem> packItems = this._repMediator.DecorationPackItemRep.FindByPack(packId, packSpaceId).ToArray();

            IEnumerable<Guid> packIds = packItems.Select(x => x.PackId).Distinct();

            IDictionary<Guid, DecorationPack> packs = this._repMediator.DecorationPackRep.Find(packIds);
            //IDictionary<Guid, DecorationPackInfo> packInfos = packs.ToDictionary(x => x.Key, x => x.Value.ToDTO());

            IEnumerable<DecorationPackItemInfo> packItemInfos = packItems.Select(x => x.ToDTO(packs));

            return packItemInfos;
        }
        #endregion

        #region # 获取套餐模板项 ——  DecorationPackItemInfo GetDecorationPackItemById(Guid packItemId)
        /// <summary>
        /// 获取套餐模板项
        /// </summary>
        /// <param name="packItemId">套餐模板项Id</param>
        /// <returns></returns>
        public DecorationPackItemInfo GetDecorationPackItemById(Guid packItemId)
        {
            DecorationPackItem packItem = this._repMediator.DecorationPackItemRep.Single(packItemId);
            IDictionary<Guid, DecorationPack> packs = this._repMediator.DecorationPackRep.Find(new List<Guid> { packItem.PackId });
            //IDictionary<Guid, DecorationPackInfo> packInfos = packs.ToDictionary(x => x.Key, x => x.Value.ToDTO());
            DecorationPackItemInfo packItemInfo = packItem.ToDTO(packs);
            return packItemInfo;
        }

        #endregion

        #region # 获取套餐模板商品SKU项列表 —— IEnumerable<DecorationPackSkuInfo> GetPackSkus(Guid packItemId)
        /// <summary>
        /// 获取套餐模板商品SKU项列表
        /// </summary>
        /// <param name="packItemId">套餐模板项Id</param>
        /// <returns>套餐模板商品SKU项列表</returns>
        public IEnumerable<DecorationPackSkuInfo> GetPackSkus(Guid packItemId)
        {
            IEnumerable<DecorationPackSku> packSkus = this._repMediator.DecorationPackSkuRep.FindByPackItem(packItemId);
            DecorationPackItem packItem = this._repMediator.DecorationPackItemRep.Single(packItemId);
            IEnumerable<Guid> packIds = new List<Guid> { packItem.PackId };
            IDictionary<Guid, DecorationPack> packs = this._repMediator.DecorationPackRep.Find(packIds);
            IEnumerable<DecorationPackSkuInfo> packSkuInfos = packSkus.Select(x => x.ToDTO(packs));

            return packSkuInfos;
        }
        #endregion

        #region # 批量获取套餐模板商品SKU项列表 —— IDictionary<Guid, IEnumerable<DecorationPackSkuInfo>> GetBulkPackSkus(Guid packId, IEnumerable<Guid> packItemIds

        /// <summary>
        /// 批量获取套餐模板商品SKU项列表
        /// </summary>
        /// <param name="packId">套餐Id</param>
        /// <param name="packItemIds">套餐模板项Id集</param>
        /// <returns>套餐模板商品SKU项字典</returns>
        /// IDictionary[Guid, IEnumerable[DecorationPackSkuInfo]]，[套餐模板项Id，套餐模板商品SKU项列表]
        public IDictionary<Guid, IEnumerable<DecorationPackSkuInfo>> GetBulkPackSkus(Guid packId, IEnumerable<Guid> packItemIds)
        {
            packItemIds = packItemIds == null ? new Guid[0] : packItemIds.ToArray().Distinct();

            IDictionary<Guid, IEnumerable<DecorationPackSkuInfo>> dictionary = new Dictionary<Guid, IEnumerable<DecorationPackSkuInfo>>();
            IEnumerable<Guid> packIds = new List<Guid> { packId };
            IDictionary<Guid, DecorationPack> packs = this._repMediator.DecorationPackRep.Find(packIds);
            foreach (Guid packItemId in packItemIds)
            {
                IEnumerable<DecorationPackSku> packSkus = this._repMediator.DecorationPackSkuRep.FindByPackItem(packItemId);
                IEnumerable<DecorationPackSkuInfo> packSkuInfos = packSkus.Select(x => x.ToDTO(packs));

                dictionary.Add(packItemId, packSkuInfos);
            }

            return dictionary;
        }
        #endregion

        #region # 批量获取套餐商品SKU项列表 —— IDictionary<Guid, IEnumerable<DecorationPackSkuInfo>> GetBulkPackSkusByPackId(Guid packId
        /// <summary>
        /// 批量获取套餐商品SKU项列表
        /// </summary>
        /// <param name="packId">套餐Id</param>
        /// <returns>套餐商品SKU项字典</returns>
        /// IDictionary[Guid, IEnumerable[DecorationPackSkuInfo]]，[套餐模板项Id，套餐模板商品SKU项列表]
        public IDictionary<Guid, IEnumerable<DecorationPackSkuInfo>> GetBulkPackSkusByPackId(Guid packId)
        {
            IEnumerable<DecorationPackItem> packItems = this._repMediator.DecorationPackItemRep.FindByPack(packId, null).ToArray();
            List<Guid> packItemIds = packItems.Select(x => x.Id).ToList();

            IDictionary<Guid, IEnumerable<DecorationPackSkuInfo>> dictionary = new Dictionary<Guid, IEnumerable<DecorationPackSkuInfo>>();
            IEnumerable<Guid> packIds = new List<Guid> { packId };
            IDictionary<Guid, DecorationPack> packs = this._repMediator.DecorationPackRep.Find(packIds);
            foreach (Guid packItemId in packItemIds)
            {
                IEnumerable<DecorationPackSku> packSkus = this._repMediator.DecorationPackSkuRep.FindByPackItem(packItemId);
                IEnumerable<DecorationPackSkuInfo> packSkuInfos = packSkus.Select(x => x.ToDTO(packs));

                dictionary.Add(packItemId, packSkuInfos);
            }

            return dictionary;
        }
        #endregion

        #region # 获取套餐模板工艺项列表 —— IEnumerable<DecorationPackCraftInfo> GetPackCrafts(Guid packItemId
        /// <summary>
        /// 获取套餐模板工艺项列表
        /// </summary>
        /// <param name="packItemId">套餐模板项Id</param>
        /// <returns>套餐模板工艺项列表</returns>
        public IEnumerable<DecorationPackCraftInfo> GetPackCrafts(Guid packItemId)
        {
            DecorationPackItem packItem = this._repMediator.DecorationPackItemRep.Single(packItemId);
            IEnumerable<Guid> packIds = new List<Guid> { packItem.PackId };
            IDictionary<Guid, DecorationPack> packs = this._repMediator.DecorationPackRep.Find(packIds);
            IEnumerable<DecorationPackCraft> packCrafts = this._repMediator.DecorationPackCraftRep.FindByPackItem(packItemId);
            IEnumerable<DecorationPackCraftInfo> packCraftInfos = packCrafts.Select(x => x.ToDTO(packs));

            return packCraftInfos;
        }
        #endregion

        #region # 批量获取套餐模板工艺项列表 —— IDictionary<Guid, IEnumerable<DecorationPackCraftInfo>> GetBulkPackCrafts(Guid packId, IEnumerable<Guid> packItemIds

        /// <summary>
        /// 批量获取套餐模板工艺项列表
        /// </summary>
        /// <param name="packId">套餐Id</param>
        /// <param name="packItemIds">套餐模板项Id</param>
        /// <returns>套餐模板工艺项字典</returns>
        /// IDictionary[Guid, IEnumerable[DecorationPackCraftInfo]]，[套餐模板项Id，套餐模板工艺项列表]
        public IDictionary<Guid, IEnumerable<DecorationPackCraftInfo>> GetBulkPackCrafts(Guid packId, IEnumerable<Guid> packItemIds)
        {
            packItemIds = packItemIds == null ? new Guid[0] : packItemIds.ToArray().Distinct();

            IDictionary<Guid, IEnumerable<DecorationPackCraftInfo>> dictionary = new Dictionary<Guid, IEnumerable<DecorationPackCraftInfo>>();
            IEnumerable<Guid> packIds = new List<Guid> { packId };
            IDictionary<Guid, DecorationPack> packs = this._repMediator.DecorationPackRep.Find(packIds);
            foreach (Guid packItemId in packItemIds)
            {
                IEnumerable<DecorationPackCraft> packCrafts = this._repMediator.DecorationPackCraftRep.FindByPackItem(packItemId);
                IEnumerable<DecorationPackCraftInfo> packCraftInfos = packCrafts.Select(x => x.ToDTO(packs));

                dictionary.Add(packItemId, packCraftInfos);
            }

            return dictionary;
        }
        #endregion

        #region # 批量获取套餐工艺项列表 —— IDictionary<Guid, IEnumerable<DecorationPackCraftInfo>> GetBulkPackCraftsByPackId(Guid packId
        /// <summary>
        /// 批量获取套餐工艺项列表
        /// </summary>
        /// <param name="packId">套餐Id</param>
        /// <returns>套餐工艺项字典</returns>
        /// IDictionary[Guid, IEnumerable[DecorationPackCraftInfo]]，[套餐模板项Id，套餐模板工艺项列表]
        public IDictionary<Guid, IEnumerable<DecorationPackCraftInfo>> GetBulkPackCraftsByPackId(Guid packId)
        {
            IEnumerable<DecorationPackItem> packItems = this._repMediator.DecorationPackItemRep.FindByPack(packId, null).ToArray();
            List<Guid> packItemIds = packItems.Select(x => x.Id).ToList();

            IDictionary<Guid, IEnumerable<DecorationPackCraftInfo>> dictionary = new Dictionary<Guid, IEnumerable<DecorationPackCraftInfo>>();
            IEnumerable<Guid> packIds = new List<Guid> { packId };
            IDictionary<Guid, DecorationPack> packs = this._repMediator.DecorationPackRep.Find(packIds);
            foreach (Guid packItemId in packItemIds)
            {
                IEnumerable<DecorationPackCraft> packCrafts = this._repMediator.DecorationPackCraftRep.FindByPackItem(packItemId);
                IEnumerable<DecorationPackCraftInfo> packCraftInfos = packCrafts.Select(x => x.ToDTO(packs));

                dictionary.Add(packItemId, packCraftInfos);
            }

            return dictionary;
        }

        #endregion

        #region # 获取套餐模板方案列表 —— IEnumerable<DecorationPackSchemeInfo> GetPackSchemes(Guid packId)
        /// <summary>
        /// 获取套餐模板方案列表
        /// </summary>
        /// <param name="packId">套餐模板Id</param>
        /// <returns>套餐模板方案列表</returns>
        public IEnumerable<DecorationPackSchemeInfo> GetPackSchemes(Guid packId)
        {
            IEnumerable<Guid> packIds = new List<Guid> { packId };
            IDictionary<Guid, DecorationPack> packs = this._repMediator.DecorationPackRep.Find(packIds);
            IEnumerable<DecorationPackScheme> packSchemes = this._repMediator.DecorationPackSchemeRep.FindByPack(packId);
            IEnumerable<DecorationPackSchemeInfo> packSchemeInfos = packSchemes.Select(x => x.ToDTO(packs));

            return packSchemeInfos;
        }
        #endregion

        #region # 获取套餐模板方案空间列表 —— IEnumerable<DecorationPackSchemeSpaceInfo> GetPackSchemeSpaces(Guid packSchemeId
        /// <summary>
        /// 获取套餐模板方案空间列表
        /// </summary>
        /// <param name="packSchemeId">套餐模板方案Id</param>
        /// <returns>方案空间列表</returns>
        public IEnumerable<DecorationPackSchemeSpaceInfo> GetPackSchemeSpaces(Guid packSchemeId)
        {
            IEnumerable<DecorationPackSchemeSpace> packSchemeSpaces = this._repMediator.DecorationPackSchemeSpaceRep.FindByScheme(packSchemeId);
            IEnumerable<DecorationPackSchemeSpaceInfo> packSchemeSpaceInfos = packSchemeSpaces.Select(x => x.ToDTO());

            return packSchemeSpaceInfos;
        }
        #endregion

        #region # 获取套餐模板内下架商品数量 —— IDictionary<Guid, int> GetOffShelvedCount(IEnumerable<Guid> packIds)
        /// <summary>
        /// 获取套餐模板内下架商品数量
        /// </summary>
        /// <param name="packIds">套餐模板Id集</param>
        /// <returns>下架商品数量字典</returns>
        /// <remarks>IDictionary[Guid, int]，[套餐模板Id, 下架商品数量]</remarks>
        public IDictionary<Guid, int> GetOffShelvedCount(IEnumerable<Guid> packIds)
        {
            return this._repMediator.DecorationPackSkuRep.GetOffShelvedCount(packIds);
        }
        #endregion

        #region # 获取总下架商品数量 —— int GetTotalOffShelvedCount(Guid? createrId)
        /// <summary>
        /// 获取总下架商品数量
        /// </summary>
        /// <param name="createrId">套餐创建人Id？（全部：null,我的：当前登录人员）</param>
        /// <returns>总下架商品数量</returns>
        public int GetTotalOffShelvedCount(Guid? createrId)
        {
            List<Guid> packIds = null;
            if (createrId.HasValue)
            {
                int rowCount, pageCount;
                IEnumerable<DecorationPack> decorationPacks = this._repMediator.DecorationPackRep.FindByPage("", null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, true, null, null, true, createrId, null, 1, int.MaxValue, out rowCount, out pageCount).ToList();

                packIds = decorationPacks.Select(x => x.Id).ToList();
            }
            return this._repMediator.DecorationPackSkuRep.GetTotalOffShelvedCount(packIds);
        }
        #endregion

        #region # 获取套餐模板 —— DecorationPackInfo GetDecorationPack(Guid packId)
        /// <summary>
        /// 获取套餐模板
        /// </summary>
        /// <param name="packId">套餐模板Id</param>
        /// <returns>套餐模板</returns>
        public DecorationPackInfo GetDecorationPack(Guid packId)
        {
            DecorationPack currentPack = this._repMediator.DecorationPackRep.Single(packId);
            List<DecorationPackScheme> packSchemes = this._repMediator.DecorationPackSchemeRep.FindDefaultByPack(new List<Guid> { packId }).ToList();
            DecorationPackInfo currentPackInfo = currentPack.ToDTO(packSchemes);

            return currentPackInfo;
        }
        #endregion

        #region # 获取套餐模板方案 —— DecorationPackSchemeInfo GetPackScheme(Guid packSchemeId)
        /// <summary>
        /// 获取套餐模板方案
        /// </summary>
        /// <param name="packSchemeId">套餐模板方案Id</param>
        /// <returns>套餐模板方案</returns>
        public DecorationPackSchemeInfo GetPackScheme(Guid packSchemeId)
        {
            DecorationPackScheme currentScheme = this._repMediator.DecorationPackSchemeRep.Single(packSchemeId);
            IEnumerable<Guid> packIds = new List<Guid> { currentScheme.PackId };
            IDictionary<Guid, DecorationPack> packs = this._repMediator.DecorationPackRep.Find(packIds);
            return currentScheme.ToDTO(packs);
        }
        #endregion

        #region # 获取总下架商品SKU Id列表 —— IEnumerable<Guid> GetTotalOffShelvedSkus()
        /// <summary>
        /// 获取总下架商品SKU Id集列表
        /// </summary>
        /// <returns>商品SKU Id列表</returns>
        public IEnumerable<Guid> GetTotalOffShelvedSkus()
        {
            return this._repMediator.DecorationPackSkuRep.GetTotalOffShelvedSkus();
        }
        #endregion

        #region # 获取总下架商品SKU列表 —— Dictionary<Guid, IEnumerable<DecorationPackInfo>> GetOffShelvedSkusPacks()
        /// <summary>
        /// 获取总下架商品SKU列表
        /// </summary>
        /// <returns>商品SKU列表</returns>
        public Dictionary<Guid, IEnumerable<DecorationPackInfo>> GetOffShelvedSkusPacks()
        {
            Dictionary<Guid, IEnumerable<DecorationPackInfo>> dic = new Dictionary<Guid, IEnumerable<DecorationPackInfo>>();
            //下架工艺集
            List<Guid> skus = this._repMediator.DecorationPackSkuRep.GetTotalOffShelvedSkus().ToList();
            IEnumerable<Guid> packIds = this._repMediator.DecorationPackSkuRep.GetPackIdsBySkus(skus);
            IEnumerable<DecorationPack> packs = this._repMediator.DecorationPackRep.GetDecorationPacksByPackIds(packIds);
            foreach (Guid sku in skus)
            {
                IEnumerable<Guid> skuPackIds = this._repMediator.DecorationPackSkuRep.GetPackIds(sku);
                IEnumerable<DecorationPack> skuPacks = packs.Where(x => skuPackIds.Contains(x.Id));
                IEnumerable<DecorationPackInfo> skuPackInfos = skuPacks.Select(x => x.ToDTO(new List<DecorationPackScheme>()));
                dic.Add(sku, skuPackInfos);

            }
            return dic;
        }
        #endregion

        #region # 获取总下架商品SKU列表 —— Dictionary<Guid, IEnumerable<DecorationPackInfo>> GetOffShelvedSkusPacksByPage(Guid? createrId,int pageIndex, int pageSize, out int rowCount, out int pageCount)
        /// <summary>
        /// 获取总下架商品SKU列表
        /// </summary>
        /// <param name="createrId">套餐创建人Id？（全部：null,我的：当前登录人员）</param>
        /// <returns>商品SKU列表</returns>
        public Dictionary<Guid, IEnumerable<DecorationPackInfo>> GetOffShelvedSkusPacksByPage(Guid? createrId, int pageIndex, int pageSize, out int rowCount, out int pageCount)
        {
            Dictionary<Guid, IEnumerable<DecorationPackInfo>> dic = new Dictionary<Guid, IEnumerable<DecorationPackInfo>>();
            //下架商品集
            List<Guid> skuIds = this._repMediator.DecorationPackSkuRep.GetTotalOffShelvedSkusByPage(pageIndex, pageSize, out rowCount, out pageCount).ToList();
            //IEnumerable<Guid> packIds = this._repMediator.DecorationPackSkuRep.GetPackIdsBySkus(skus);
            //IEnumerable<DecorationPack> packs = this._repMediator.DecorationPackRep.GetDecorationPacksByPackIds(packIds).Values;
            List<Tuple<Guid, Guid, Guid, decimal>> items = this._repMediator.DecorationPackSkuRep.GetSkusPackIdsById(skuIds).ToList();
            List<Guid> packIds = items.Select(x => x.Item3).Distinct().ToList();
            List<DecorationPack> packs = this._repMediator.DecorationPackRep.GetDecorationPacksByPackIds(packIds, createrId).ToList();


            foreach (Guid skuId in skuIds)
            {
                List<Guid> skuPackIds = items.Where(x => x.Item1 == skuId).Select(x => x.Item3).Distinct().ToList();
                List<DecorationPack> skuPacks = packs.Where(x => skuPackIds.Contains(x.Id)).ToList();
                List<DecorationPackInfo> skuPackInfos = skuPacks.Select(x => x.ToDTO(new List<DecorationPackScheme>())).ToList();
                if (skuPackInfos.Any())
                    dic.Add(skuId, skuPackInfos);
            }
            return dic;
        }
        #endregion

        #region # 根据商品SKU获取套餐模板列表 —— IEnumerable<DecorationPackInfo> GetDecorationPacksBySku(Guid skuId
        /// <summary>
        /// 根据商品SKU获取套餐模板列表(排除克隆套餐版本)
        /// </summary>
        /// <param name="skuId">商品SKU Id</param>
        /// <returns>套餐模板列表</returns>
        public IEnumerable<DecorationPackInfo> GetDecorationPacksBySku(Guid skuId)
        {
            IEnumerable<Guid> packIds = this._repMediator.DecorationPackSkuRep.GetPackIds(skuId);
            IEnumerable<DecorationPack> packs = this._repMediator.DecorationPackRep.GetDecorationPacksByPackIds(packIds);
            IEnumerable<DecorationPackInfo> packInfos = packs.Select(x => x.ToDTO(new List<DecorationPackScheme>()));

            return packInfos;
        }
        #endregion

        #region # 获取总下架工艺实体Id列表 —— IEnumerable<Guid> GetTotalOffShelvedCrafts()
        /// <summary>
        /// 获取总下架工艺实体Id列表
        /// </summary>
        /// <returns>工艺实体Id列表</returns>
        public IEnumerable<Guid> GetTotalOffShelvedCrafts()
        {
            return this._repMediator.DecorationPackCraftRep.GetTotalOffShelvedCrafts();
        }
        #endregion

        #region # 获取总下架工艺实体Id列表 —— Dictionary<Guid, IEnumerable<DecorationPackInfo>> GetOffShelvedCraftsPacks()
        /// <summary>
        /// 获取总下架工艺实体Id列表
        /// </summary>
        /// <returns>工艺实体Id列表</returns>
        public Dictionary<Guid, IEnumerable<DecorationPackInfo>> GetOffShelvedCraftsPacks()
        {
            Dictionary<Guid, IEnumerable<DecorationPackInfo>> dic = new Dictionary<Guid, IEnumerable<DecorationPackInfo>>();
            //下架工艺集
            List<Guid> craftIds = this._repMediator.DecorationPackCraftRep.GetTotalOffShelvedCrafts().ToList();
            IEnumerable<Tuple<Guid, Guid, Guid, decimal>> items = this._repMediator.DecorationPackCraftRep.GetCraftsPackIdsById(craftIds);
            IEnumerable<Guid> packIds = items.Select(x => x.Item3).Distinct();
            IEnumerable<DecorationPack> packs = this._repMediator.DecorationPackRep.GetDecorationPacksByPackIds(packIds);
            //IEnumerable<Guid> packIds = this._repMediator.DecorationPackCraftRep.GetPackIdsByCrafts(crafts);
            //IEnumerable<DecorationPack> packs = this._repMediator.DecorationPackRep.GetDecorationPacksByPackIds(packIds).Values;

            foreach (Guid craftId in craftIds)
            {
                IEnumerable<Guid> craftPackIds = items.Where(x => x.Item1 == craftId).Select(x => x.Item3).Distinct();
                IEnumerable<DecorationPack> craftPacks = packs.Where(x => craftPackIds.Contains(x.Id));
                IEnumerable<DecorationPackInfo> craftPackInfos = craftPacks.Select(x => x.ToDTO(new List<DecorationPackScheme>()));
                dic.Add(craftId, craftPackInfos);

            }
            return dic;
        }
        #endregion

        #region # 获取总下架工艺实体Id列表 —— Dictionary<Guid, IEnumerable<DecorationPackInfo>> GetOffShelvedCraftsPacksByPage(Guid? createrId, int pageIndex, int pageSize,out int rowCount, out int pageCount)
        /// <summary>
        /// 获取总下架工艺实体Id列表
        /// </summary>
        /// <param name="createrId">套餐创建人Id？（全部：null,我的：当前登录人员）</param>
        /// <returns>工艺实体Id列表</returns>
        public Dictionary<Guid, IEnumerable<DecorationPackInfo>> GetOffShelvedCraftsPacksByPage(Guid? createrId, int pageIndex, int pageSize, out int rowCount, out int pageCount)
        {
            Dictionary<Guid, IEnumerable<DecorationPackInfo>> dic = new Dictionary<Guid, IEnumerable<DecorationPackInfo>>();
            //下架工艺集
            List<Guid> craftIds = this._repMediator.DecorationPackCraftRep.GetTotalOffShelvedCraftsByPage(pageIndex, pageSize, out rowCount, out pageCount).ToList();
            List<Tuple<Guid, Guid, Guid, decimal>> items = this._repMediator.DecorationPackCraftRep.GetCraftsPackIdsById(craftIds).ToList();
            List<Guid> packIds = items.Select(x => x.Item3).Distinct().ToList();
            List<DecorationPack> packs = this._repMediator.DecorationPackRep.GetDecorationPacksByPackIds(packIds, createrId).ToList();

            foreach (Guid craftId in craftIds)
            {
                List<Guid> craftPackIds = items.Where(x => x.Item1 == craftId).Select(x => x.Item3).Distinct().ToList();
                List<DecorationPack> craftPacks = packs.Where(x => craftPackIds.Contains(x.Id)).ToList();
                List<DecorationPackInfo> craftPackInfos = craftPacks.Select(x => x.ToDTO(new List<DecorationPackScheme>())).ToList();
                if (craftPackInfos.Any())
                    dic.Add(craftId, craftPackInfos);

            }
            return dic;
        }
        #endregion

        #region # 获取总下架工艺数量 —— int GetTotalOffShelvedCraftCount(Guid? createrId)
        /// <summary>
        /// 获取总下架工艺数量
        /// </summary>
        /// <param name="createrId">套餐创建人Id？（全部：null,我的：当前登录人员）</param>
        /// <returns>总下架工艺数量</returns>
        public int GetTotalOffShelvedCraftCount(Guid? createrId)
        {
            List<Guid> packIds = null;
            if (createrId.HasValue)
            {
                int rowCount, pageCount;
                IEnumerable<DecorationPack> decorationPacks = this._repMediator.DecorationPackRep.FindByPage("", null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, true, null, null, true, createrId, null, 1, int.MaxValue, out rowCount, out pageCount);

                packIds = decorationPacks.Select(x => x.Id).ToList();
            }
            return this._repMediator.DecorationPackCraftRep.GetTotalOffShelvedCraftCount(packIds);
        }
        #endregion

        #region # 根据工艺实体获取套餐模板列表 —— IEnumerable<DecorationPackInfo> GetDecorationPacksByCraft(Guid craftEntityId
        /// <summary>
        /// 根据工艺实体获取套餐模板列表
        /// </summary>
        /// <param name="craftEntityId">工艺实体Id</param>
        /// <returns>套餐模板Id列表</returns>
        public IEnumerable<DecorationPackInfo> GetDecorationPacksByCraft(Guid craftEntityId)
        {
            IEnumerable<Guid> packIds = this._repMediator.DecorationPackCraftRep.GetPackIds(craftEntityId);
            IEnumerable<DecorationPack> packs = this._repMediator.DecorationPackRep.GetDecorationPacksByPackIds(packIds);
            IEnumerable<DecorationPackInfo> packInfos = packs.Select(x => x.ToDTO(new List<DecorationPackScheme>()));

            return packInfos;
        }
        #endregion

        #region # 获取套餐商品SKU数量字典 —— IDictionary<Guid, float> GetSkuQuantities(Guid packId)
        /// <summary>
        /// 获取套餐商品SKU数量字典
        /// </summary>
        /// <param name="packId">套餐模板Id</param>
        /// <returns>商品SKU数量字典</returns>
        public IDictionary<Guid, float> GetSkuQuantities(Guid packId)
        {
            return this._repMediator.DecorationPackSkuRep.GetSkuQuantities(packId);
        }
        #endregion

        #region # 获取套餐工艺工程量字典 —— IDictionary<Guid, float> GetCraftQuantities(Guid packId)
        /// <summary>
        /// 获取套餐工艺工程量字典
        /// </summary>
        /// <param name="packId">套餐模板Id</param>
        /// <returns>工艺工程量字典</returns>
        public IDictionary<Guid, float> GetCraftQuantities(Guid packId)
        {
            return this._repMediator.DecorationPackCraftRep.GetCraftQuantities(packId);
        }
        #endregion

        #region # 获取套餐模板内下架工艺数量 —— IDictionary<Guid, int> GetOffShelvedCraftCount(IEnumerable<Guid> packIds)
        /// <summary>
        /// 获取套餐模板内下架工艺数量
        /// </summary>
        /// <param name="packIds">套餐模板Id集</param>
        /// <returns>下架工艺数量字典</returns>
        /// <remarks>IDictionary[Guid, int]，[套餐模板Id, 下架工艺数量]</remarks>
        public IDictionary<Guid, int> GetOffShelvedCraftCount(IEnumerable<Guid> packIds)
        {
            return this._repMediator.DecorationPackCraftRep.GetOffShelvedCount(packIds);
        }
        #endregion

        #region # 根据源套餐Id获取套餐当前版本号—— DecorationPackInfo GetDecorationPackVersionNumber(Guid sourcePackId)
        /// <summary>
        /// 根据套餐Id获取套餐当前版本号
        /// </summary>
        /// <param name="sourcePackId">源套餐Id</param>
        /// <returns></returns>
        public DecorationPackInfo GetDecorationPackVersionNumber(Guid sourcePackId)
        {
            DecorationPack decorationPack = this._repMediator.DecorationPackRep.FindDecorationPackVersionNumber(sourcePackId);
            if (decorationPack == null)
            {
                return null;
            }
            List<DecorationPackScheme> packSchemes = this._repMediator.DecorationPackSchemeRep.FindDefaultByPack(new List<Guid> { decorationPack.Id }).ToList();
            DecorationPackInfo decorationPackInfo = decorationPack.ToDTO(packSchemes);
            return decorationPackInfo;
        }
        #endregion

        #region # 根据套餐Id字典获取是否是最新版本及最新版本套餐状态—— Tuple<Guid,ShelfStatus,bool> GetNewestStatusByPackIds(Dictionary<Guid,Guid>  packDic)

        /// <summary>
        /// 根据套餐Id字典获取是否是最新版本及最新版本套餐状态
        /// </summary>
        /// <param name="packDic">Key:套餐Id , Value:源套餐Id</param>
        /// <returns>Item1:套餐Id , Item2:最新版本套餐状态 , Item3:是否最新版本</returns>
        public IEnumerable<Tuple<Guid, ShelfStatus, bool>> GetNewestStatusByPackIds(Dictionary<Guid, Guid> packDic)
        {
            List<DecorationPack> balePacks = this._repMediator.DecorationPackRep.FindDecorationPackVersionNumbers(packDic.Values).ToList();

            List<Tuple<Guid, ShelfStatus, bool>> result = new List<Tuple<Guid, ShelfStatus, bool>>();

            foreach (KeyValuePair<Guid, Guid> item in packDic)
            {
                var source = balePacks.SingleOrDefault(s => s.Id == item.Value);
                var newVersion = balePacks.Where(s => s.SourcePackId == item.Value).OrderByDescending(x => x.AddedTime).ThenByDescending(x => x.VersionNumber).FirstOrDefault() ?? source;
                if (source != null)
                    result.Add(new Tuple<Guid, ShelfStatus, bool>(item.Key, source.Status, item.Key == newVersion.Id));
            }
            return result;
        }
        #endregion

        #region # 获取固定源套餐标签集(居室|（设计风格编号|设计风格名称）|空间类型)—— Tuple<List<string>, List<string>, List<string>> GetSourceDecorationPackLabels()
        /// <summary>
        /// 获取固定源套餐标签集(居室|（设计风格编号|设计风格名称）|空间类型)
        /// </summary>
        /// <returns></returns>
        public Tuple<List<string>, Dictionary<string, string>, List<string>> GetSourceDecorationPackLabels()
        {
            Tuple<List<string>, List<string>, List<string>> packLabels = this._repMediator.DecorationPackRep.FindSourceDecorationPackLabels();
            Dictionary<string, string> styles = this._customerModelContract.GetStyles().Where(x => packLabels.Item2.Contains(x.Number)).ToDictionary(x => x.Number, x => x.Name);
            Tuple<List<string>, Dictionary<string, string>, List<string>> labels =
                new Tuple<List<string>, Dictionary<string, string>, List<string>>(packLabels.Item1, styles, packLabels.Item3);
            return labels;
        }
        #endregion

        #region # 根据套餐Id集获取套餐模板集(包含空间) —— IEnumerable<DecorationPackInfo> GetDecorationPackByIds(IEnumerable<Guid> packIds)
        /// <summary>
        /// 根据套餐Id集获取套餐模板集(包含空间)
        /// </summary>
        /// <param name="packIds">套餐模板Id集合</param>
        /// <returns>套餐模板(包含空间)</returns>
        public IEnumerable<DecorationPackInfo> GetDecorationPackByIds(IEnumerable<Guid> packIds)
        {
            packIds = packIds ?? new Guid[0];
            List<DecorationPack> currentPacks = this._repMediator.DecorationPackRep.GetDecorationPacksByPackIds(packIds).ToList();
            List<DecorationPackScheme> packSchemes = this._repMediator.DecorationPackSchemeRep.FindDefaultByPack(packIds.ToList()).ToList();
            List<DecorationPackInfo> currentPackInfos = currentPacks.Select(x => x.ToDtoAndSpaces(packSchemes)).ToList();

            return currentPackInfos;
        }
        #endregion

        #region # 批量获取套餐选区集 —— IDictionary<Guid, IEnumerable<DecorationPackItemInfo>> GetDecorationPackItems(IEnumerable<Guid> packIds)
        /// <summary>
        /// 批量获取套餐选区集
        /// </summary>
        /// <param name="packIds">套餐Id集</param>
        /// <returns>套餐Id|选区集</returns>
        public IDictionary<Guid, IEnumerable<DecorationPackItemInfo>> GetDecorationPackItems(IEnumerable<Guid> packIds)
        {
            packIds = packIds ?? new Guid[0];
            Dictionary<Guid, IEnumerable<DecorationPackItemInfo>> dic = new Dictionary<Guid, IEnumerable<DecorationPackItemInfo>>();
            Dictionary<Guid, IEnumerable<DecorationPackItem>> packItems = this._repMediator.DecorationPackItemRep.FindByPackIds(packIds);
            dic = packItems.ToDictionary(k => k.Key, v => v.Value.OrderBy(x => x.AddedTime).Select(x => x.ToDTO()));
            return dic;
        }
        #endregion

        #region # 批量获取套餐工艺项列表 —— Dictionary<Guid, Dictionary<Guid, IEnumerable<DecorationPackCraftInfo>>> GetBulkPackCraftsByPackIds(IEnumerable<Guid> packIds)
        /// <summary>
        /// 批量获取套餐工艺项列表
        /// </summary>
        /// <param name="packIds">套餐Id集</param>
        /// <returns>套餐工艺项字典</returns>
        /// Dictionary[Guid, Dictionary[Guid,IEnumerable[DecorationPackCraftInfo]]]，[套餐Id|套餐模板项Id|套餐模板工艺项列表]
        public Dictionary<Guid, Dictionary<Guid, IEnumerable<DecorationPackCraftInfo>>> GetBulkPackCraftsByPackIds(IEnumerable<Guid> packIds)
        {
            packIds = packIds ?? new Guid[0];
            Dictionary<Guid, IEnumerable<DecorationPackItem>> packItems = this._repMediator.DecorationPackItemRep.FindByPackIds(packIds);
            IDictionary<Guid, DecorationPack> packs = this._repMediator.DecorationPackRep.Find(packIds);

            Dictionary<Guid, Dictionary<Guid, IEnumerable<DecorationPackCraftInfo>>> dic =
                   new Dictionary<Guid, Dictionary<Guid, IEnumerable<DecorationPackCraftInfo>>>();
            packItems.ForEach(pack =>
            {
                Dictionary<Guid, IEnumerable<DecorationPackCraftInfo>> dictionary = new Dictionary<Guid, IEnumerable<DecorationPackCraftInfo>>();
                pack.Value.ForEach(item =>
                {
                    List<DecorationPackCraft> packCrafts = item.PackCraftEntities.ToList();
                    IEnumerable<DecorationPackCraftInfo> packCraftInfos = packCrafts.Select(x => x.ToDTO(packs));
                    dictionary.Add(item.Id, packCraftInfos);
                });

                dic.Add(pack.Key, dictionary);
            });
            return dic;
        }

        #endregion

        #region # 批量获取套餐商品SKU项列表 —— IDictionary<Guid, IEnumerable<DecorationPackSkuInfo>> GetBulkPackSkusByPackId(...
        /// <summary>
        /// 批量获取套餐商品SKU项列表
        /// </summary>
        /// <param name="packIds">套餐Id集</param>
        /// <returns>套餐商品SKU项字典</returns>
        /// Dictionary[Guid, Dictionary[Guid,IEnumerable[DecorationPackSkuInfo]]]，[套餐Id|套餐模板项Id|套餐模板商品SKU项列表]
        public Dictionary<Guid, Dictionary<Guid, IEnumerable<DecorationPackSkuInfo>>> GetBulkPackSkusByPackIds(IEnumerable<Guid> packIds)
        {
            packIds = packIds ?? new Guid[0];
            Dictionary<Guid, IEnumerable<DecorationPackItem>> packItems = this._repMediator.DecorationPackItemRep.FindByPackIds(packIds);
            IDictionary<Guid, DecorationPack> packs = this._repMediator.DecorationPackRep.Find(packIds);

            Dictionary<Guid, Dictionary<Guid, IEnumerable<DecorationPackSkuInfo>>> dic =
                   new Dictionary<Guid, Dictionary<Guid, IEnumerable<DecorationPackSkuInfo>>>();
            packItems.ForEach(pack =>
            {
                Dictionary<Guid, IEnumerable<DecorationPackSkuInfo>> dictionary = new Dictionary<Guid, IEnumerable<DecorationPackSkuInfo>>();
                pack.Value.ForEach(item =>
                {
                    List<DecorationPackSku> packSkus = item.PackSkus.ToList();
                    IEnumerable<DecorationPackSkuInfo> packSkuInfos = packSkus.Select(x => x.ToDTO(packs));
                    dictionary.Add(item.Id, packSkuInfos);
                });

                dic.Add(pack.Key, dictionary);
            });
            return dic;
        }
        #endregion

        #region # 获取全部套餐商品SkuId集 —— IEnumerable<Guid> GetAllPackSkuIds()
        /// <summary>
        /// 获取全部套餐商品SkuId集
        /// </summary>
        /// <returns>商品SkuId集</returns>
        public IEnumerable<Guid> GetAllPackSkuIds()
        {
            return _repMediator.DecorationPackSkuRep.FindAll().Select(s => s.SkuId).Distinct();
        }
        #endregion

        #region # 批量获取套餐工艺实体Id集 —— IEnumerable<Guid> GetAllPackCraftIds()
        /// <summary>
        /// 获取全部套餐工艺实体Id集
        /// </summary>
        /// <returns>工艺实体Id集</returns>
        public IEnumerable<Guid> GetAllPackCraftIds()
        {
            return _repMediator.DecorationPackCraftRep.FindAll().Select(s => s.CraftEntityId).Distinct();
        }
        #endregion

        #region # 获取套餐模板推荐项列表 —— IEnumerable<DecorationPackRecommendedItemInfo> GetPackRecommendedItems(Guid packId, Guid? packSpaceId);

        /// <summary>
        /// 获取套餐模板推荐项列表
        /// </summary>
        /// <param name="packId">套餐模板Id</param>
        /// <param name="packSpaceId">套餐模板空间Id</param>
        /// <returns>套餐模板推荐项列表</returns>
        public IEnumerable<DecorationPackRecommendedItemInfo> GetPackRecommendedItems(Guid packId, Guid? packSpaceId)
        {
            IEnumerable<DecorationPackRecommendedItem> packItems = this._repMediator.DecorationPackRecommendedItemRep.FindByPack(packId, packSpaceId).ToList();

            IDictionary<Guid, DecorationPackSpace> packSpaces = this._repMediator.DecorationPackSpaceRep.Find(packItems.Select(s => s.PackSpaceId).Distinct());

            IEnumerable<DecorationPackRecommendedItemInfo> packItemInfos = packItems.Select(x => x.ToDTO(packSpaces));

            return packItemInfos;
        }
        #endregion

        //商品|工艺变价


        #region # 获取总变价商品数量 —— int GetTotalChangedCount(Guid? createrId)
        /// <summary>
        /// 获取总变价商品数量
        /// </summary>
        /// <param name="createrId">套餐创建人Id？（全部：null,我的：当前登录人员）</param>
        /// <returns>总变价商品数量</returns>
        public int GetTotalChangedCount(Guid? createrId)
        {
            List<Guid> packIds = null;
            if (createrId.HasValue)
            {
                int rowCount, pageCount;
                IEnumerable<DecorationPack> decorationPacks = this._repMediator.DecorationPackRep.FindByPage("", null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, true, null, null, true, createrId, null, 1, int.MaxValue, out rowCount, out pageCount);
               
                packIds = decorationPacks.Select(x => x.Id).ToList();
            }
            return this._repMediator.DecorationPackSkuRep.GetTotalChangedCount(packIds);
        }
        #endregion

        #region # 获取套餐模板内变价商品数量 —— IDictionary<Guid, int> GetChangedCount(IEnumerable<Guid> packIds)
        /// <summary>
        /// 获取套餐模板内变价商品数量
        /// </summary>
        /// <param name="packIds">套餐模板Id集</param>
        /// <returns>变价商品数量字典</returns>
        /// <remarks>IDictionary[Guid, int]，[套餐模板Id, 变价商品数量]</remarks>
        public IDictionary<Guid, int> GetChangedCount(IEnumerable<Guid> packIds)
        {
            return this._repMediator.DecorationPackSkuRep.GetChangedCount(packIds);
        }
        #endregion

        #region # 获取总变价商品SKU Id列表 —— IEnumerable<Guid> GetTotalChangedSkus()
        /// <summary>
        /// 获取总变价商品SKU Id集列表
        /// </summary>
        /// <returns>商品SKU Id列表</returns>
        public IEnumerable<Guid> GetTotalChangedSkus()
        {
            return this._repMediator.DecorationPackSkuRep.GetTotalChangedSkus();
        }
        #endregion

        #region # 获取总变价商品SKU列表 —— Dictionary<Guid, IEnumerable<DecorationPackInfo>> GetChangedSkusPacks()
        /// <summary>
        /// 获取总变价商品SKU列表
        /// </summary>
        /// <returns>商品SKU列表</returns>
        public Dictionary<Guid, IEnumerable<DecorationPackInfo>> GetChangedSkusPacks()
        {
            Dictionary<Guid, IEnumerable<DecorationPackInfo>> dic = new Dictionary<Guid, IEnumerable<DecorationPackInfo>>();
            //变价商品集
            List<Guid> skus = this._repMediator.DecorationPackSkuRep.GetTotalChangedSkus().ToList();
            IEnumerable<Guid> packIds = this._repMediator.DecorationPackSkuRep.GetPackIdsBySkus(skus);
            IEnumerable<DecorationPack> packs = this._repMediator.DecorationPackRep.GetDecorationPacksByPackIds(packIds);

            foreach (Guid sku in skus)
            {
                IEnumerable<Guid> skuPackIds = this._repMediator.DecorationPackSkuRep.GetPackIds(sku);
                IEnumerable<DecorationPack> skuPacks = packs.Where(x => skuPackIds.Contains(x.Id));
                IEnumerable<DecorationPackInfo> skuPackInfos = skuPacks.Select(x => x.ToDTO(new List<DecorationPackScheme>()));
                dic.Add(sku, skuPackInfos);

            }
            return dic;
        }
        #endregion

        #region # 获取总变价商品SKU列表 ——Dictionary<Guid, Dictionary<DecorationPackInfo, decimal>> GetChangedSkusPacksByPage(Guid? createrId,int pageIndex, int pageSize, out int rowCount, out int pageCount)
        /// <summary>
        /// 获取总变价商品SKU列表
        /// </summary>
        /// <param name="createrId">套餐创建人Id？（全部：null,我的：当前登录人员）</param>
        /// <returns>商品SKUId| 套餐|商品成本价</returns>
        public Dictionary<Guid, Dictionary<DecorationPackInfo, decimal>> GetChangedSkusPacksByPage(Guid? createrId, int pageIndex, int pageSize, out int rowCount, out int pageCount)
        {
            Dictionary<Guid, Dictionary<DecorationPackInfo, decimal>> dic = new Dictionary<Guid, Dictionary<DecorationPackInfo, decimal>>();
            //下架工艺集
            List<Guid> skuIds = this._repMediator.DecorationPackSkuRep.GetTotalChangedSkuIdsByPage(pageIndex, pageSize, out rowCount, out pageCount).ToList();
            //IEnumerable<Guid> packIds = this._repMediator.DecorationPackSkuRep.GetPackIdsBySkus(skuIds);
            //IEnumerable<DecorationPack> packs = this._repMediator.DecorationPackRep.GetDecorationPacksByPackIds(packIds).Values;
            //List<DecorationPackSku> skus = this._repMediator.DecorationPackSkuRep.GetSkus(skuIds).ToList();
            List<Tuple<Guid, Guid, Guid, decimal>> items = this._repMediator.DecorationPackSkuRep.GetSkusPackIdsById(skuIds).ToList();
            List<Guid> packIds = items.Select(x => x.Item3).Distinct().ToList();
            List<DecorationPack> packs = this._repMediator.DecorationPackRep.GetDecorationPacksByPackIds(packIds, createrId).ToList();

            //TODO　不考虑同套餐下商品价格不同问题，只取其中一个
            foreach (Guid skuId in skuIds)
            {
                List<Guid> skuPackIds = items.Where(x => x.Item1 == skuId).Select(x => x.Item3).Distinct().ToList();
                List<DecorationPack> skuPacks = packs.Where(x => skuPackIds.Contains(x.Id)).ToList();
                List<DecorationPackInfo> skuPackInfos = skuPacks.Select(x => x.ToDTO(new List<DecorationPackScheme>())).ToList();
                Dictionary<DecorationPackInfo, decimal> dicPack = new Dictionary<DecorationPackInfo, decimal>();
                foreach (DecorationPackInfo skuPackInfo in skuPackInfos)
                {
                    decimal costPrice = items.FirstOrDefault(x => x.Item1 == skuId && x.Item3 == skuPackInfo.Id).Item4;
                    dicPack.Add(skuPackInfo, costPrice);
                }
                if (dicPack.Any())
                    dic.Add(skuId, dicPack);
            }
            return dic;
        }
        #endregion

        #region # 给定套餐模板空间下是否存在变价商品 —— IDictionary<Guid, bool> ExistsChanged(...
        /// <summary>
        /// 给定套餐模板空间下是否存在变价商品
        /// </summary>
        /// <param name="packSpaceIds">套餐模板空间Id</param>
        /// <returns>是否存在</returns>
        public IDictionary<Guid, bool> ExistsChanged(IEnumerable<Guid> packSpaceIds)
        {
            return this._repMediator.DecorationPackSkuRep.ExistsChanged(packSpaceIds);
        }

        #endregion


        #region # 获取总变价工艺实体Id列表 —— IEnumerable<Guid> GetTotalChangedCrafts()
        /// <summary>
        /// 获取总变价工艺实体Id列表
        /// </summary>
        /// <returns>工艺实体Id列表</returns>
        public IEnumerable<Guid> GetTotalChangedCrafts()
        {
            return this._repMediator.DecorationPackCraftRep.GetTotalChangedCrafts();
        }
        #endregion

        #region # 获取总变价工艺实体Id列表 —— Dictionary<Guid, IEnumerable<DecorationPackInfo>> GetChangedCraftsPacks()
        /// <summary>
        /// 获取总变价工艺实体Id列表
        /// </summary>
        /// <returns>工艺实体Id列表</returns>
        public Dictionary<Guid, IEnumerable<DecorationPackInfo>> GetChangedCraftsPacks()
        {
            Dictionary<Guid, IEnumerable<DecorationPackInfo>> dic = new Dictionary<Guid, IEnumerable<DecorationPackInfo>>();
            //下架工艺集
            List<Guid> craftIds = this._repMediator.DecorationPackCraftRep.GetTotalChangedCrafts().ToList();
            IEnumerable<Tuple<Guid, Guid, Guid, decimal>> items = this._repMediator.DecorationPackCraftRep.GetCraftsPackIdsById(craftIds);
            IEnumerable<Guid> packIds = items.Select(x => x.Item3).Distinct();
            IEnumerable<DecorationPack> packs = this._repMediator.DecorationPackRep.GetDecorationPacksByPackIds(packIds);

            foreach (Guid craftId in craftIds)
            {
                IEnumerable<Guid> craftPackIds = items.Where(x => x.Item1 == craftId).Select(x => x.Item3).Distinct();
                IEnumerable<DecorationPack> craftPacks = packs.Where(x => craftPackIds.Contains(x.Id));
                IEnumerable<DecorationPackInfo> craftPackInfos = craftPacks.Select(x => x.ToDTO(new List<DecorationPackScheme>()));
                dic.Add(craftId, craftPackInfos);

            }
            return dic;
        }
        #endregion

        #region # 获取总变价工艺实体Id列表 —— Dictionary<Guid, Dictionary<DecorationPackInfo, decimal>> GetChangedCraftsPacksByPage(Guid? createrId,int pageIndex, int pageSize)
        /// <summary>
        /// 获取总变价工艺实体Id列表
        /// </summary>
        /// <param name="createrId">套餐创建人Id？（全部：null,我的：当前登录人员）</param>
        /// <returns>工艺实体Id列表</returns>
        public Dictionary<Guid, Dictionary<DecorationPackInfo, decimal>> GetChangedCraftsPacksByPage(Guid? createrId, int pageIndex, int pageSize, out int rowCount, out int pageCount)
        {
            Dictionary<Guid, Dictionary<DecorationPackInfo, decimal>> dic = new Dictionary<Guid, Dictionary<DecorationPackInfo, decimal>>();
            //下架工艺集
            List<Guid> craftIds = this._repMediator.DecorationPackCraftRep.GetTotalChangedCraftsByPage(pageIndex, pageSize, out rowCount, out pageCount).ToList();
            List<Tuple<Guid, Guid, Guid, decimal>> items = this._repMediator.DecorationPackCraftRep.GetCraftsPackIdsById(craftIds).ToList();
            List<Guid> packIds = items.Select(x => x.Item3).Distinct().ToList();
            List<DecorationPack> packs = this._repMediator.DecorationPackRep.GetDecorationPacksByPackIds(packIds, createrId).ToList();
            //工艺价格只取一个
            foreach (Guid craftId in craftIds)
            {
                List<Guid> craftPackIds = items.Where(x => x.Item1 == craftId).Select(x => x.Item3).Distinct().ToList();
                List<DecorationPack> craftPacks = packs.Where(x => craftPackIds.Contains(x.Id)).ToList();
                List<DecorationPackInfo> craftPackInfos = craftPacks.Select(x => x.ToDTO(new List<DecorationPackScheme>())).ToList();
                Dictionary<DecorationPackInfo, decimal> dicPack = new Dictionary<DecorationPackInfo, decimal>();
                foreach (DecorationPackInfo craftPackInfo in craftPackInfos)
                {
                    decimal costPrice = items.FirstOrDefault(x => x.Item1 == craftId && x.Item3 == craftPackInfo.Id).Item4;
                    dicPack.Add(craftPackInfo, costPrice);
                }
                if (dicPack.Any())
                    dic.Add(craftId, dicPack);

            }
            return dic;
        }
        #endregion

        #region # 获取总变价工艺数量 —— int GetTotalChangedCraftCount(Guid? createrId)
        /// <summary>
        /// 获取总变价工艺数量
        /// </summary>
        /// <param name="createrId">套餐创建人Id？（全部：null,我的：当前登录人员）</param>
        /// <returns>总变价工艺数量</returns>
        public int GetTotalChangedCraftCount(Guid? createrId)
        {
            List<Guid> packIds = null;
            if (createrId.HasValue)
            {
                int rowCount, pageCount;
                IEnumerable<DecorationPack> decorationPacks = this._repMediator.DecorationPackRep.FindByPage("", null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, true, null, null, true, createrId, null, 1, int.MaxValue, out rowCount, out pageCount);

                packIds = decorationPacks.Select(x => x.Id).ToList();
            }
            return this._repMediator.DecorationPackCraftRep.GetTotalChangedCraftCount(packIds);
        }
        #endregion

        #region # 获取套餐模板内变价工艺数量 —— IDictionary<Guid, int> GetChangedCraftCount(IEnumerable<Guid> packIds)
        /// <summary>
        /// 获取套餐模板内变价工艺数量
        /// </summary>
        /// <param name="packIds">套餐模板Id集</param>
        /// <returns>变价工艺数量字典</returns>
        /// <remarks>IDictionary[Guid, int]，[套餐模板Id, 变价工艺数量]</remarks>
        public IDictionary<Guid, int> GetChangedCraftCount(IEnumerable<Guid> packIds)
        {
            return this._repMediator.DecorationPackCraftRep.GetChangedCount(packIds);
        }
        #endregion

        #region # 给定套餐模板空间下是否存在变价工艺 —— IDictionary<Guid, bool> ExistsChangedCraft(...
        /// <summary>
        /// 给定套餐模板空间下是否存在变价工艺
        /// </summary>
        /// <param name="packSpaceIds">套餐模板空间Id</param>
        /// <returns>是否存在</returns>
        public IDictionary<Guid, bool> ExistsChangedCraft(IEnumerable<Guid> packSpaceIds)
        {
            return this._repMediator.DecorationPackCraftRep.ExistsChanged(packSpaceIds);
        }
        #endregion

        #region # 获取下架|变价 商品+工艺数量 ——Tuple<int, int> GetTotalOffShelvedChangedCount(Guid? createrId)

        /// <summary>
        /// 获取下架|变价 商品+工艺数量
        /// </summary>
        /// <param name="createrId">套餐创建人Id？（全部：null,我的：当前登录人员）</param>
        /// <returns>下架商品工艺数量合计|变价商品工艺合计</returns>
        public Tuple<int, int> GetTotalOffShelvedChangedCount(Guid? createrId)
        {
            List<Guid> packIds = null;
            if (createrId.HasValue)
            {
                int rowCount, pageCount;
                IEnumerable<DecorationPack> decorationPacks = this._repMediator.DecorationPackRep.FindByPage("", null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, true, null, null, true, createrId, null, 1, int.MaxValue, out rowCount, out pageCount);

                packIds = decorationPacks.Select(x => x.Id).ToList();
            }
            int skuChanged = this._repMediator.DecorationPackSkuRep.GetTotalChangedCount(packIds);
            int craftChanged = this._repMediator.DecorationPackCraftRep.GetTotalChangedCraftCount(packIds);
            int skuOffShelved = this._repMediator.DecorationPackSkuRep.GetTotalOffShelvedCount(packIds);
            int craftOffShelved = this._repMediator.DecorationPackCraftRep.GetTotalOffShelvedCraftCount(packIds);
            return new Tuple<int, int>((skuOffShelved + craftOffShelved), (skuChanged + craftChanged));

        }
        #endregion


        #region # 获取套餐模板内下架|变价的商品|工艺数量 —— IDictionary<Guid, Tuple<int, int, int, int>> GetOffShelvedChangedCountByIds(IEnumerable<Guid> packIds)

        /// <summary>
        /// 获取套餐模板内下架|变价的商品|工艺数量
        /// </summary>
        /// <param name="packIds">套餐Id集</param>
        /// <returns>套餐Id|下架商品数量|下架工艺数量|变价商品数量|变价工艺数量</returns>
        public IDictionary<Guid, Tuple<int, int, int, int>> GetOffShelvedChangedCountByIds(IEnumerable<Guid> packIds)
        {

            IDictionary<Guid, int> craftOffShelved = this._repMediator.DecorationPackCraftRep.GetOffShelvedCount(packIds);

            IDictionary<Guid, int> skuOffShelved = this._repMediator.DecorationPackSkuRep.GetOffShelvedCount(packIds);

            IDictionary<Guid, int> skuChanged = this._repMediator.DecorationPackSkuRep.GetChangedCount(packIds);

            IDictionary<Guid, int> craftChanged = this._repMediator.DecorationPackCraftRep.GetChangedCount(packIds);

            Dictionary<Guid, Tuple<int, int, int, int>> count = new Dictionary<Guid, Tuple<int, int, int, int>>();

            foreach (Guid packId in packIds)
            {
                count.Add(packId, new Tuple<int, int, int, int>(skuOffShelved[packId], craftOffShelved[packId], skuChanged[packId], craftChanged[packId]));
            }
            return count;
        }
        #endregion


        #region # 验证套餐模板名称是否存在 —— bool ExistsPackName(Guid? packId, string packName)
        /// <summary>
        /// 验证套餐模板名称是否存在
        /// </summary>
        /// <param name="packId">套餐模板Id</param>
        /// <param name="packName">套餐模板名称</param>
        /// <returns>是否存在</returns>
        public bool ExistsPackName(Guid? packId, string packName)
        {
            return this._repMediator.DecorationPackRep.ExistsPackName(packId, packName);
        }
        #endregion

        #region # 验证套餐模板空间名称是否存在 —— bool ExistsPackSpaceName(Guid packId, Guid? packSpaceId...
        /// <summary>
        /// 验证套餐模板空间名称是否存在
        /// </summary>
        /// <param name="packId">套餐模板Id</param>
        /// <param name="packSpaceId">套餐模板空间Id</param>
        /// <param name="packSpaceName">套餐模板空间名称</param>
        /// <returns>是否存在</returns>
        public bool ExistsPackSpaceName(Guid packId, Guid? packSpaceId, string packSpaceName)
        {
            return this._repMediator.DecorationPackSpaceRep.ExistsName(packId, packSpaceId, packSpaceName);
        }
        #endregion

        #region # 验证套餐模板项名称是否存在 —— bool ExistsPackItemName(Guid packId, Guid? packItemId...

        /// <summary>
        /// 验证套餐模板项名称是否存在
        /// </summary>
        /// <param name="packId">套餐模板Id</param>
        /// <param name="packSpaceId">空间Id</param>
        /// <param name="packItemId">套餐模板项Id</param>
        /// <param name="packItemName">套餐模板项名称</param>
        /// <returns>是否存在</returns>
        public bool ExistsPackItemName(Guid packId, Guid packSpaceId, Guid? packItemId, string packItemName)
        {
            return this._repMediator.DecorationPackItemRep.ExistsName(packId, packSpaceId, packItemId, packItemName);
        }
        #endregion

        #region # 验证套餐模板方案名称是否存在 —— bool ExistsPackSchemeName(Guid packId, Guid? packSchemeId...
        /// <summary>
        /// 验证套餐模板方案名称是否存在
        /// </summary>
        /// <param name="packId">套餐模板Id</param>
        /// <param name="packSchemeId">套餐模板方案Id</param>
        /// <param name="packSchemeName">套餐模板方案名称</param>
        /// <returns>是否存在</returns>
        public bool ExistsPackSchemeName(Guid packId, Guid? packSchemeId, string packSchemeName)
        {
            return this._repMediator.DecorationPackSchemeRep.ExistsName(packId, packSchemeId, packSchemeName);
        }
        #endregion

        #region # 验证套餐模板下是否配置商品SKU —— bool ExistsPackSku(Guid packId)
        /// <summary>
        /// 验证套餐模板下是否配置商品SKU
        /// </summary>
        /// <param name="packId">套餐模板Id</param>
        /// <returns>是否配置</returns>
        public bool ExistsPackSku(Guid packId)
        {
            return this._repMediator.DecorationPackSkuRep.ExistsByPack(packId);
        }
        #endregion

        #region # 给定套餐模板空间下是否存在下架商品 —— IDictionary<Guid, bool> ExistsOffShelved(...
        /// <summary>
        /// 给定套餐模板空间下是否存在下架商品
        /// </summary>
        /// <param name="packSpaceIds">套餐模板空间Id</param>
        /// <returns>是否存在</returns>
        public IDictionary<Guid, bool> ExistsOffShelved(IEnumerable<Guid> packSpaceIds)
        {
            return this._repMediator.DecorationPackSkuRep.ExistsOffShelved(packSpaceIds);
        }
        #endregion

        #region # 给定套餐模板空间下是否存在下架工艺 —— IDictionary<Guid, bool> ExistsOffShelvedCraft(...
        /// <summary>
        /// 给定套餐模板空间下是否存在下架工艺
        /// </summary>
        /// <param name="packSpaceIds">套餐模板空间Id</param>
        /// <returns>是否存在</returns>
        public IDictionary<Guid, bool> ExistsOffShelvedCraft(IEnumerable<Guid> packSpaceIds)
        {
            return this._repMediator.DecorationPackCraftRep.ExistsOffShelved(packSpaceIds);
        }
        #endregion

        #region  # 验证套餐模板下是否存在下架|变价的商品|工艺 —— IDictionary<Guid, Tuple<bool, bool, bool, bool>> ExistsOffShelvedChanged(IEnumerable<Guid> packSpaceIds)

        /// <summary>
        /// 验证套餐模板下是否存在下架|变价的商品|工艺
        /// </summary>
        /// <param name="packSpaceIds">套餐模板空间Id</param>
        /// <returns>套餐模板空间Id|存在下架商品|下架工艺|变价商品|变价工艺</returns>
        public IDictionary<Guid, Tuple<bool, bool, bool, bool>> ExistsOffShelvedChanged(IEnumerable<Guid> packSpaceIds)
        {
            IDictionary<Guid, bool> craftOffShelved = this._repMediator.DecorationPackCraftRep.ExistsOffShelved(packSpaceIds);

            IDictionary<Guid, bool> skuOffShelved = this._repMediator.DecorationPackSkuRep.ExistsOffShelved(packSpaceIds);

            IDictionary<Guid, bool> skuChanged = this._repMediator.DecorationPackSkuRep.ExistsChanged(packSpaceIds);

            IDictionary<Guid, bool> craftChanged = this._repMediator.DecorationPackCraftRep.ExistsChanged(packSpaceIds);

            Dictionary<Guid, Tuple<bool, bool, bool, bool>> count = new Dictionary<Guid, Tuple<bool, bool, bool, bool>>();

            foreach (Guid packSpaceId in packSpaceIds)
            {
                count.Add(packSpaceId, new Tuple<bool, bool, bool, bool>(skuOffShelved[packSpaceId], craftOffShelved[packSpaceId], skuChanged[packSpaceId], craftChanged[packSpaceId]));
            }
            return count;
        }
        #endregion

        #region # 山河2020年新需求(张智超2020-05-08)

        #region # 根据项目Id分页获取关联套餐模板列表 —— PageModel<DecorationPackInfo> GetDecorationPacksByProjectId(guid projectId,string keywords, Guid? propertyId, float? applicableSquare, bool? isNewHouse,
        /// <summary>
        /// 根据项目Id分页获取关联套餐模板列表
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
        /// <param name="packType">套餐类型</param>
        /// <param name="packKind">套餐类别(套餐|造型)</param>
        /// <param name="packMode">套餐模式(基础|成品|基础+成品)</param>
        /// <param name="spaceTypes">空间类型集</param>
        /// <param name="styleNos">设计风格集</param>
        /// <param name="layouts">居室集</param>
        /// <param name="hasPackSeries">是否有标签(全部和标签:true,其他:false)</param>
        /// <param name="packSeriesIds">套餐系列集</param>
        /// <param name="stores">门店集</param>
        /// <param name="hasStores">是否有门店</param>
        /// <param name="sort">排序条件 true为倒序</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页容量</param>
        /// <returns>套餐模板列表</returns>
        public PageModel<DecorationPackInfo> GetDecorationPacksByProjectId(Guid projectId, string keywords, Guid? propertyId, float? applicableSquare, bool? isNewHouse,
            bool? isBuildingPrice, decimal? minPrice, decimal? maxPrice, bool? isBuildingSquare, float? minSquare, float? maxSquare, DecorationPackType? packType, DecorationPackKind? packKind,
            DecorationPackMode? packMode, IList<string> spaceTypes, IList<string> styleNos, IList<string> layouts, bool hasPackSeries, IList<Guid> packSeriesIds, IList<Guid> stores, bool hasStores,
            Dictionary<string, bool> sort, int pageIndex, int pageSize)
        {
            int rowCount, pageCount;

            IEnumerable<DecorationPack> decorationPacks = this._repMediator.DecorationPackRep.FindByPage(projectId, keywords, propertyId, applicableSquare, isNewHouse,
                isBuildingPrice, minPrice, maxPrice, isBuildingSquare,
                minSquare, maxSquare, null, null, null, packType, packKind, packMode, ShelfStatus.Shelfed, spaceTypes, styleNos, layouts, null, hasPackSeries, packSeriesIds, stores, hasStores, null,
                sort, pageIndex,
                pageSize, out rowCount, out pageCount);
            List<Guid> packIds = decorationPacks.Select(x => x.Id).ToList();
            List<DecorationPackScheme> packSchemes = this._repMediator.DecorationPackSchemeRep.FindDefaultByPack(packIds).ToList();
            IEnumerable<DecorationPackInfo> decorationPackInfos = decorationPacks.Select(x => x.ToDTO(packSchemes));

            return new PageModel<DecorationPackInfo>(decorationPackInfos, pageIndex, pageSize, pageCount, rowCount);
        }
        #endregion

        #endregion

        #region # 测试 Tuple —— void Test(Guid id)
        /// <summary>
        /// 测试 Tuple
        /// </summary>
        /// <param name="id"></param>
        public void Test(Guid id)
        {
            int rowCount;
            int pageCount;
            //Dictionary<Guid, IEnumerable<DecorationPackInfo>> dic = this.GetOffShelvedSkusPacksByPage(1, 10, out rowCount, out pageCount);
            //Dictionary<Guid, IEnumerable<DecorationPackInfo>> dic1 = this.GetOffShelvedCraftsPacksByPage(1, 10, out rowCount, out pageCount);
            Dictionary<Guid, Dictionary<DecorationPackInfo, decimal>> dic3 = this.GetChangedSkusPacksByPage(null, 1, 10, out rowCount, out pageCount);
            //Dictionary<Guid, Dictionary<DecorationPackInfo, decimal>> dic4 = this.GetChangedCraftsPacksByPage(1, 10, out rowCount, out pageCount);

            //Dictionary<Guid, List<Guid>> skuPackIds = new Dictionary<Guid, List<Guid>>();
            //dic.ForEach(item =>
            //{

            //    skuPackIds.Add(item.Key, item.Value.Select(x => x.Id).ToList());
            //});
            //this.RemoveAllSku(skuPackIds);

            //IEnumerable<DecorationPackItem> packItem4444s = this._repMediator.DecorationPackItemRep.FindByPack(Guid.Parse("2e93c061-fd35-409a-9ab6-fa3231ef2820"), null);

            //IEnumerable<DecorationPackSpaceInfo> packSpaceInfos = this.GetPackSpaces(id);
            //批量获取套餐选区工艺商品
            //List<Guid> packIds = new List<Guid> { Guid.Parse("ECAA7AAA-8819-4435-9B67-DDDEC60459A6") };
            ////, Guid.Parse("B1DD24D8-118C-41DA-8642-46A2A28BEEA7"), Guid.Parse("6925E646-005C-4660-BF5C-1644FDB14BDC") };
            //Dictionary<Guid, Dictionary<Guid, IEnumerable<DecorationPackCraftInfo>>> dicCraft = this.GetBulkPackCraftsByPackIds(packIds);
            //Dictionary<Guid, Dictionary<Guid, IEnumerable<DecorationPackSkuInfo>>> dicSku = this.GetBulkPackSkusByPackIds(packIds);

            //IEnumerable<DecorationPackInfo> currentPackInfos = this.GetDecorationPackByIds(packIds);

            //var n = this.GetDecorationPacksForSales(null, Guid.Parse("4a510b9d-1479-47f9-a86f-5ad4deab146d"), 0f, true,
            //     false, 1100, 10000, null, null, null, null, null,
            //     null, null, null, null, true, null, null,
            //     null, 1, 10);

            //var mm = this.GetDecorationPacksForSales(null, Guid.Parse("4a510b9d-1479-47f9-a86f-5ad4deab146d"), 0f, true,
            //  false, 1100, 10000, null, null, null, null, null,
            //  null, null, null, null, true, null, null,
            //  null, 1, 10);

            //Dictionary<Guid, IEnumerable<DecorationPackInfo>> dic = GetChangedSkusPacks();


            //var tot = this.GetDecorationPacks("复制", Guid.Empty, null, null, false, null, null, false, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, true, null, null, 1, 10);

            //DecorationPackInfo currentPackInfo = this.GetDecorationPack(id);
            this.GetDecorationPacksByProjectId(Guid.Parse("B38F5E58-2B23-4C25-BE2B-E388FA820F1F"), null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, true, null, new List<Guid>() { Guid.Parse("867a141a-eba8-4a44-b4d1-008cd70a2132") }, true, null, 1, 2);

        }
        #endregion

    }
}
