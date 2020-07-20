using HDIPlatform.DesignSystem.AppService.Maps;
using HDIPlatform.DesignSystem.Domain.Entities.BaleContext;
using HDIPlatform.DesignSystem.Domain.IRepositories;
using HDIPlatform.DesignSystem.Domain.Mediators;
using HDIPlatform.DesignSystem.IAppService.DTOs.Outputs.BaleContext;
using HDIPlatform.DesignSystem.IAppService.Interfaces;
using ShSoft.Common.PoweredByLee;
using ShSoft.Infrastructure.DTOBase;
using ShSoft.Infrastructure.Global.Transaction;
using ShSoft.ValueObjects.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using HDIPlatform.SupplierSystem.IAppService.DTOs.Outputs;
using HDIPlatform.SupplierSystem.IAppService.Interfaces;
using IProductContract = HDIPlatform.ResourceService.IAppService.Interfaces.IProductContract;

namespace HDIPlatform.DesignSystem.AppService.Implements
{
    /// <summary>
    /// 大包服务契约实现
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class BaleContract : IBaleContract
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

        private readonly ICategoryContract _categoryContract;

        private readonly HDIPlatform.ResourceService.IAppService.Interfaces.IProductContract _productContract;
        /// <summary>
        /// 依赖注入构造器
        /// </summary>
        /// <param name="svcMediator">领域服务中介者</param>
        /// <param name="repMediator">仓储中介者</param>
        /// <param name="unitOfWork">单元事务</param>
        public BaleContract(DomainServiceMediator svcMediator, RepositoryMediator repMediator, IUnitOfWorkDesign unitOfWork, ICategoryContract categoryContract, IProductContract productContract)
        {
            this._svcMediator = svcMediator;
            this._repMediator = repMediator;
            this._unitOfWork = unitOfWork;
            _categoryContract = categoryContract;
            _productContract = productContract;
        }

        #endregion


        //命令部分

        #region # 创建大包套餐 —— Guid CreateBalePack(string packName, Guid operatorId, string @operator)
        /// <summary>
        /// 创建大包套餐
        /// </summary>
        /// <param name="packName">大包套餐名称</param>
        /// <param name="operatorId">操作人Id</param>
        /// <param name="operator">操作人名称</param>
        /// <returns>套餐Id</returns>
        public Guid CreateBalePack(string packName, Guid operatorId, string @operator)
        {
            //套餐名称重复验证
            Assert.IsFalse(this._repMediator.BalePackRep.ExistsPackName((Guid?)null, packName, BalePackType.Bale), "大包套餐名称重复，请重试！");

            BalePack balePack = new BalePack(packName, BalePackType.Bale, @operator, operatorId);

            this._unitOfWork.RegisterAdd(balePack);
            this._unitOfWork.UnitedCommit();

            return balePack.Id;
        }
        #endregion

        #region # 创建定制套餐 —— Guid CrateCustomizedPack(string packName, Guid operatorId, string @operator)

        /// <summary>
        /// 创建定制套餐
        /// </summary>
        /// <param name="packName">定制套餐名称</param>
        /// <param name="operatorId">操作人Id</param>
        /// <param name="operator">操作人名称</param>
        /// <returns>套餐Id</returns>
        public Guid CrateCustomizedPack(string packName, Guid operatorId, string @operator)
        {
            //套餐名称重复验证
            Assert.IsFalse(this._repMediator.BalePackRep.ExistsPackName((Guid?)null, packName, BalePackType.Customized), "定制套餐名称重复，请重试！");

            BalePack custPack = new BalePack(packName, BalePackType.Customized, @operator, operatorId);

            this._unitOfWork.RegisterAdd(custPack);
            this._unitOfWork.UnitedCommit();

            return custPack.Id;
        }
        #endregion

        #region # 设置套餐比例 —— void SetPackDiscountRatio(Guid packId, float discountRatio)

        /// <summary>
        /// 设置套餐比例
        /// </summary>
        /// <param name="packId">套餐Id</param>
        /// <param name="discountRatio">套餐比例</param>
        public void SetPackDiscountRatio(Guid packId, float discountRatio)
        {
            BalePack pack = this._unitOfWork.Resolve<BalePack>(packId);
            Assert.IsFalse(pack.PackShelfStatus == ShelfStatus.Shelfed, "套餐已上架不可修改");
            pack.SetDiscountRatio(discountRatio);
            this._unitOfWork.RegisterSave(pack);
            this._unitOfWork.UnitedCommit();
        }
        #endregion

        #region # 设置套餐标签 —— void SetPackLabel(Guid packId, IEnumerable<string> labels)
        /// <summary>
        /// 设置套餐标签
        /// </summary>
        /// <param name="packId">套餐Id</param>
        /// <param name="labels">标签集</param>
        public void SetPackLabel(Guid packId, IEnumerable<string> labels)
        {
            //验证
            labels = labels ?? new string[0];

            BalePack pack = this._unitOfWork.Resolve<BalePack>(packId);
            //Assert.IsFalse(pack.PackShelfStatus == ShelfStatus.Shelfed, "套餐已上架不可修改");
            pack.SetLabelStr(labels);

            this._unitOfWork.RegisterSave(pack);
            this._unitOfWork.UnitedCommit();
        }
        #endregion

        #region # 设置套餐封面 —— void SetPackCoverDrawing(Guid packId, string coverDrawing)
        /// <summary>
        /// 设置套餐封面
        /// </summary>
        /// <param name="packId">套餐Id</param>
        /// <param name="coverDrawing">封面图</param>
        public void SetPackCoverDrawing(Guid packId, string coverDrawing)
        {
            BalePack pack = this._unitOfWork.Resolve<BalePack>(packId);
            //Assert.IsFalse(pack.PackShelfStatus == ShelfStatus.Shelfed, "套餐已上架不可修改");
            pack.SetCoverDrawing(coverDrawing);

            this._unitOfWork.RegisterSave(pack);
            this._unitOfWork.UnitedCommit();
        }
        #endregion

        #region # 设置套餐下架 —— void SetPackShelfOff(Guid packId)
        /// <summary>
        /// 下架套餐
        /// </summary>
        /// <param name="packId">套餐Id</param>
        public void SetPackShelfOff(Guid packId)
        {
            BalePack pack = this._unitOfWork.Resolve<BalePack>(packId);
            pack.SetPackShelfOff();

            this._unitOfWork.RegisterSave(pack);
            this._unitOfWork.UnitedCommit();
        }
        #endregion

        #region # 设置套餐上架 —— void SetPackShelfed(Guid packId)
        /// <summary>
        /// 上架套餐
        /// </summary>
        /// <param name="packId">套餐Id</param>
        public void SetPackShelfed(Guid packId)
        {
            BalePack pack = this._unitOfWork.Resolve<BalePack>(packId);
            //折扣比例必须大于0
            IEnumerable<Guid> choiceAreaIds = this._repMediator.BalePackRep.GetChoiceAreaIdsByPackId(packId);
            if (pack.BalePackType == BalePackType.Bale)
                Assert.IsTrue(this._repMediator.BalePackGroupRep.IsBalePackShelfed(choiceAreaIds), "当前套餐存在未添加选区或子选区或品类的项，不可上架");
            if (pack.BalePackType == BalePackType.Customized)
                Assert.IsTrue(this._repMediator.BalePackGroupRep.IsCustomizedPackShelfed(choiceAreaIds), "当前套餐存在未添加选区或子选区或品类或商品的项，不可上架");

            ////验证套餐内是否有选区
            //Assert.IsTrue(choiceAreaIds.Any(), "该套餐内未配置选区,不可上架");
            ////验证组内是否都已配置品类
            //Assert.IsTrue(this.ExistsCategoryByPackId(packId), "该套餐子选区存在未配置品类,不可上架");
            ////定制套餐验证品类下是否都已配置商品
            //if (pack.BalePackType == BalePackType.Customized)
            //{
            //    Assert.IsTrue(this.ExistsProductByPackId(packId), "该套餐品类存在未配置商品,不可上架");
            //}

            pack.SetPackShelfed();

            this._unitOfWork.RegisterSave(pack);
            this._unitOfWork.UnitedCommit();
        }
        #endregion

        #region # 修改大包套餐名称及折扣比例 —— void ModifyBalePackNameAndRatio(Guid packId,string packName...
        /// <summary>
        /// 修改大包套餐名称及折扣比例
        /// </summary>
        /// <param name="packId">套餐Id</param>
        /// <param name="packName">套餐名称</param>
        /// <param name="discountRatio">折扣比例</param>
        public void ModifyBalePackNameAndRatio(Guid packId, string packName, float discountRatio)
        {
            //套餐名称重复验证
            Assert.IsFalse(this._repMediator.BalePackRep.ExistsPackName(packId, packName, BalePackType.Bale), "大包套餐名称重复，请重试！");

            BalePack pack = this._unitOfWork.Resolve<BalePack>(packId);
            Assert.IsFalse(pack.PackShelfStatus == ShelfStatus.Shelfed, "套餐已上架不可修改");
            Assert.IsTrue(discountRatio.IsPercentage(), "折扣比例必须大于0");
            pack.ModifyPackNameAndRatio(packName, discountRatio);

            this._unitOfWork.RegisterSave(pack);
            this._unitOfWork.UnitedCommit();
        }
        #endregion

        #region # 修改定制套餐名称及折扣比例 —— void ModifyCustPackNameAndRatio(Guid packId,string packName...
        /// <summary>
        /// 修改定制套餐名称及折扣比例
        /// </summary>
        /// <param name="packId">套餐Id</param>
        /// <param name="packName">套餐名称</param>
        /// <param name="discountRatio">折扣比例</param>
        public void ModifyCustPackNameAndRatio(Guid packId, string packName, float discountRatio)
        {
            //套餐名称重复验证
            Assert.IsFalse(this._repMediator.BalePackRep.ExistsPackName(packId, packName, BalePackType.Customized), "定制套餐名称重复，请重试！");

            BalePack pack = this._unitOfWork.Resolve<BalePack>(packId);
            Assert.IsFalse(pack.PackShelfStatus == ShelfStatus.Shelfed, "套餐已上架不可修改");
            Assert.IsTrue(discountRatio.IsPercentage(), "折扣比例必须大于0");
            pack.ModifyPackNameAndRatio(packName, discountRatio);

            this._unitOfWork.RegisterSave(pack);
            this._unitOfWork.UnitedCommit();
        }
        #endregion

        #region # 删除套餐 —— void RemovePack(Guid packId)
        /// <summary>
        /// 删除套餐
        /// </summary>
        /// <param name="packId">套餐Id</param>
        public void RemovePack(Guid packId)
        {
            BalePack pack = this._repMediator.BalePackRep.Single(packId);
            Assert.IsTrue(pack.PackShelfStatus == ShelfStatus.NotShelfed, "只有【未上架】状态套餐可以删除");
            //删除组
            IEnumerable<Guid> choiceAreaIds = this._unitOfWork.ResolveAreaByPack(packId).Select(s => s.Id).ToList();
            IEnumerable<BalePackGroup> groups = this._unitOfWork.ResolveBalePackGroupByArea(choiceAreaIds);
            IEnumerable<Guid> groupIds = groups.Select(x => x.Id);
            if (groupIds.Any())
                this._unitOfWork.RegisterRemoveRange<BalePackGroup>(groupIds);

            this._unitOfWork.RegisterRemove<BalePack>(packId);
            this._unitOfWork.UnitedCommit();
        }
        #endregion

        #region # 新增选区 —— Guid CreateChoiceArea(Guid packId, string choiceAreaName)
        /// <summary>
        /// 新增选区
        /// </summary>
        /// <param name="packId">套餐Id</param>
        /// <param name="choiceAreaName">选区名称</param>
        public Guid CreateChoiceArea(Guid packId, string choiceAreaName)
        {
            //选区名称重复验证
            Assert.IsFalse(this._repMediator.BalePackChoiceAreaRep.ExistsChoiceAreaName(packId, (Guid?)null, choiceAreaName), "套餐内选区名称重复，请重试！");

            BalePack pack = this._unitOfWork.Resolve<BalePack>(packId);
            Assert.IsFalse(pack.PackShelfStatus == ShelfStatus.Shelfed, "套餐已上架不可修改");
            BalePackChoiceArea choiceArea = new BalePackChoiceArea(choiceAreaName);
            pack.AddChoiceArea(choiceArea);

            this._unitOfWork.RegisterSave(pack);
            this._unitOfWork.UnitedCommit();

            return choiceArea.Id;
        }
        #endregion

        #region # 修改选区名称 —— void UpdateChoiceAreaName(Guid packId,Guid choiceAreaId, string choiceAreaName)
        /// <summary>
        /// 修改选区名称
        /// </summary>
        /// <param name="packId">套餐Id</param>
        /// <param name="choiceAreaId">选区Id</param>
        /// <param name="choiceAreaName">选区名称</param>
        public void UpdateChoiceAreaName(Guid packId, Guid choiceAreaId, string choiceAreaName)
        {
            //选区名称重复验证
            Assert.IsTrue(this._repMediator.BalePackRep.Exists(packId));
            Assert.IsFalse(this._repMediator.BalePackChoiceAreaRep.ExistsChoiceAreaName(packId, choiceAreaId, choiceAreaName), "套餐内选区名称重复，请重试！");

            BalePack pack = this._unitOfWork.Resolve<BalePack>(packId);
            Assert.IsFalse(pack.PackShelfStatus == ShelfStatus.Shelfed, "套餐已上架不可修改");
            BalePackChoiceArea choiceArea = pack.GetChoiceAreaById(choiceAreaId);
            choiceArea.ModifyAreaName(choiceAreaName);

            this._unitOfWork.RegisterSave(pack);
            this._unitOfWork.UnitedCommit();
        }
        #endregion

        #region # 删除选区 —— void RemoveChoiceArea(Guid choiceAreaId)
        /// <summary>
        /// 删除选区
        /// </summary>
        /// <param name="packId">套餐Id</param>
        /// <param name="choiceAreaId">选区Id</param>
        public void RemoveChoiceArea(Guid packId, Guid choiceAreaId)
        {
            BalePack pack = this._unitOfWork.Resolve<BalePack>(packId);
            Assert.IsFalse(pack.PackShelfStatus == ShelfStatus.Shelfed, "套餐已上架不可修改");
            BalePackChoiceArea choiceArea = pack.GetChoiceAreaById(choiceAreaId);
            pack.DeleteChoiceArea(choiceArea);
            //删除组
            IEnumerable<BalePackGroup> groups = this._unitOfWork.ResolveBalePackGroupByArea(new List<Guid> { choiceAreaId });
            IEnumerable<Guid> groupIds = groups.Select(x => x.Id);
            if (groupIds.Any())
                this._unitOfWork.RegisterRemoveRange<BalePackGroup>(groupIds);

            this._unitOfWork.RegisterSave(pack);
            this._unitOfWork.UnitedCommit();
        }
        #endregion

        #region # 新增组 —— Guid CreateGroupInChoiceArea(Guid choiceAreaId, string groupName...
        /// <summary>
        /// 新增组
        /// </summary>
        /// <param name="choiceAreaId">选区Id</param>
        /// <param name="groupName">组名</param>
        /// <param name="required">是否必选</param>
        /// <param name="categoryIds">品类Id集</param>
        public Guid CreateGroupInChoiceArea(Guid choiceAreaId, string groupName, bool required)
        {

            // 组名称重复验证
            Assert.IsFalse(this._repMediator.BalePackGroupRep.ExistsGroupName(choiceAreaId, (Guid?)null, groupName), "名称重复，请重试！");

            BalePackChoiceArea choiceArea = this._repMediator.BalePackChoiceAreaRep.Single(choiceAreaId);
            Assert.IsFalse(choiceArea.BalePack.PackShelfStatus == ShelfStatus.Shelfed, "套餐已上架不可修改");
            BalePackGroup group = new BalePackGroup(choiceAreaId, groupName, required);

            this._unitOfWork.RegisterAdd(group);
            this._unitOfWork.UnitedCommit();

            return group.Id;
        }
        #endregion

        #region # 修改组名称及选购方式 —— void UpdateGroupNameAndIsRequired(Guid groupId, string groupName, bool isRequired)

        /// <summary>
        /// 修改组名称及选购方式
        /// </summary>
        /// <param name="groupId">组Id</param>
        /// <param name="groupName">组名称</param>
        /// <param name="isRequired">选购方式true|必选，false|推荐</param>
        public void UpdateGroupNameAndIsRequired(Guid groupId, string groupName, bool isRequired)
        {
            BalePackGroup group = this._unitOfWork.Resolve<BalePackGroup>(groupId);
            // 组名称重复验证
            Assert.IsFalse(this._repMediator.BalePackGroupRep.ExistsGroupName(group.ChoiceAreaId, groupId, groupName), "名称重复，请重试！");
            BalePackChoiceArea choiceArea = this._repMediator.BalePackChoiceAreaRep.Single(group.ChoiceAreaId);
            Assert.IsFalse(choiceArea.BalePack.PackShelfStatus == ShelfStatus.Shelfed, "套餐已上架不可修改");
            //组选购方式修改
            if (isRequired)
                group.GroupIsRequired();
            else
                group.GroupIsNotRequired();
            group.ModifyName(groupName);
            this._unitOfWork.RegisterSave(group);
            this._unitOfWork.UnitedCommit();
        }
        #endregion


        #region # 删除组 —— void RemoveGroup(Guid groupId)

        /// <summary>
        /// 删除组
        /// </summary>
        /// <param name="groupId">组Id</param>
        public void RemoveGroup(Guid groupId)
        {

            BalePackGroup group = this._unitOfWork.Resolve<BalePackGroup>(groupId);
            BalePackChoiceArea choiceArea = this._repMediator.BalePackChoiceAreaRep.Single(group.ChoiceAreaId);
            Assert.IsFalse(choiceArea.BalePack.PackShelfStatus == ShelfStatus.Shelfed, "套餐已上架不可修改");
            group.DeleteAllCategoryInGroup();
            this._unitOfWork.RegisterRemove<BalePackGroup>(groupId);
            this._unitOfWork.UnitedCommit();
        }
        #endregion

        #region # 新增品类 —— void CreateCategory(Guid groupId, IEnumerable<Guid> categoryIds)
        /// <summary>
        /// 新增品类
        /// </summary>
        /// <param name="groupId">组Id</param>
        /// <param name="categoryIds">品类Id集</param>
        public void CreateCategory(Guid groupId, IEnumerable<Guid> categoryIds)
        {
            //验证
            categoryIds = categoryIds != null ? categoryIds.ToArray() : new Guid[0];
            Assert.IsTrue(categoryIds.Any(), "组内品类集不可为null！");

            BalePackGroup group = this._unitOfWork.Resolve<BalePackGroup>(groupId);
            BalePackChoiceArea choiceArea = this._repMediator.BalePackChoiceAreaRep.Single(group.ChoiceAreaId);
            Assert.IsFalse(choiceArea.BalePack.PackShelfStatus == ShelfStatus.Shelfed, "套餐已上架不可修改");

            IEnumerable<BalePackCategory> categoryInGroups = categoryIds.Select(categoryId => new BalePackCategory(categoryId));
            //TODO　20180102变更 验证改为子选区|组内三级品类不可重复
            //BalePackChoiceArea area = this._repMediator.BalePackChoiceAreaRep.Single(group.ChoiceAreaId);
            ////验证套餐内不可有重复品类
            ////套餐内所有品类
            //IEnumerable<Guid> choiceAreaIds = this._repMediator.BalePackRep.GetChoiceAreaIdsByPackId(area.BalePack.Id);
            //IEnumerable<Guid> packCategoryIds = this._repMediator.BalePackGroupRep.GetCategoryIds(choiceAreaIds);
            //if (categoryInGroups.Count() != categoryInGroups.DistinctBy(x => x.CategoryId).Count() || packCategoryIds.Intersect(categoryInGroups.Select(s => s.CategoryId)).Any())
            //{
            //    throw new InvalidOperationException("同一套餐内，品类不可重复！");
            //}
            group.AddCategoryInGroup(categoryInGroups);

            this._unitOfWork.RegisterSave(group);
            this._unitOfWork.UnitedCommit();
        }
        #endregion

        #region # 删除品类 —— void RemoveCategory(Guid groupId, Guid categoryId)
        /// <summary>
        /// 删除品类
        /// </summary>
        /// <param name="groupId">组Id</param>
        /// <param name="categoryId">品类Id</param>
        public void RemoveCategory(Guid groupId, Guid categoryId)
        {
            BalePackGroup group = this._unitOfWork.Resolve<BalePackGroup>(groupId);
            BalePackChoiceArea choiceArea = this._repMediator.BalePackChoiceAreaRep.Single(group.ChoiceAreaId);
            Assert.IsFalse(choiceArea.BalePack.PackShelfStatus == ShelfStatus.Shelfed, "套餐已上架不可修改");
            BalePackCategory category = group.GetCategoryInGroup(categoryId);
            group.DeleteCategoryInGroup(category);

            this._unitOfWork.RegisterSave(group);
            this._unitOfWork.UnitedCommit();
        }
        #endregion

        #region # 新增品类下商品 —— void CreateBalePackProduct(Guid groupId, Guid categoryId...
        /// <summary>
        /// 新增品类下商品集
        /// </summary>
        /// <param name="groupId">组Id</param>
        /// <param name="categoryId">品类Id</param>
        /// <param name="productIds">商品Id集</param>
        public void CreateBalePackProduct(Guid groupId, Guid categoryId, IEnumerable<Guid> productIds)
        {
            //验证
            productIds = productIds == null ? new Guid[0] : productIds.ToArray();
            Assert.IsTrue(productIds.Any(), "品类内商品集不可为空！");

            BalePackGroup group = this._unitOfWork.Resolve<BalePackGroup>(groupId);
            BalePackChoiceArea choiceArea = this._repMediator.BalePackChoiceAreaRep.Single(group.ChoiceAreaId);
            Assert.IsFalse(choiceArea.BalePack.PackShelfStatus == ShelfStatus.Shelfed, "套餐已上架不可修改");
            BalePackCategory category = group.GetCategoryInGroup(categoryId);
            IEnumerable<BalePackProduct> products = productIds.Select(productId => new BalePackProduct(productId));
            category.AddBalePackProduct(products);

            this._unitOfWork.RegisterSave(group);
            this._unitOfWork.UnitedCommit();
        }
        #endregion

        #region # 移除品类下已配置商品 —— void RemoveBalePackProduct(Guid groupId...
        /// <summary>
        /// 移除品类下已配置商品
        /// </summary>
        /// <param name="groupId">组Id</param>
        /// <param name="categoryId">品类Id</param>
        /// <param name="productIds">商品Id集</param>
        public void RemoveBalePackProduct(Guid groupId, Guid categoryId, IEnumerable<Guid> productIds)
        {
            //验证
            Assert.IsNotNull(productIds, "品类内商品集不可为null！");

            BalePackGroup group = this._unitOfWork.Resolve<BalePackGroup>(groupId);
            BalePackChoiceArea choiceArea = this._repMediator.BalePackChoiceAreaRep.Single(group.ChoiceAreaId);
            Assert.IsFalse(choiceArea.BalePack.PackShelfStatus == ShelfStatus.Shelfed, "套餐已上架不可修改");
            BalePackCategory category = group.GetCategoryInGroup(categoryId);
            IEnumerable<BalePackProduct> products = category.GetProductsInCategory(productIds);
            category.DeleteBalePackProduct(products);

            this._unitOfWork.RegisterSave(group);
            this._unitOfWork.UnitedCommit();
        }
        #endregion

        #region # 替换下架商品 —— void ChangedProduct(Guid shelfOffProductId, IEnumerable<Guid> packIds, Guid productId)
        /// <summary>
        /// 替换下架商品
        /// </summary>
        /// <param name="shelfOffProductId">下架商品Id</param>
        /// <param name="packIds">套餐Id集</param>
        /// <param name="productId">商品Id</param>
        public void ChangedProduct(Guid shelfOffProductId, IEnumerable<Guid> packIds, Guid productId)
        {
            packIds = packIds ?? new Guid[0];

            foreach (Guid packId in packIds)
            {
                BalePack pack = this._unitOfWork.Resolve<BalePack>(packId);
                ShelfStatus oldStatus = pack.PackShelfStatus;
                if (oldStatus == ShelfStatus.Shelfed)
                    pack.SetPackShelfOff();
                //Assert.IsFalse(pack.PackShelfStatus == ShelfStatus.Shelfed, "套餐已上架不可修改");
                IEnumerable<Guid> choiceAreaIds = this._unitOfWork.ResolveAreaByPack(packId).Select(s => s.Id).ToList();
                IEnumerable<BalePackGroup> groups = this._unitOfWork.ResolveBalePackGroupByArea(choiceAreaIds);

                foreach (BalePackGroup group in groups)
                {
                    IEnumerable<BalePackCategory> categorys = group.BalePackCategorys;
                    foreach (BalePackCategory category in categorys)
                    {

                        if (category.ExistsProduct(shelfOffProductId))
                        {
                            category.ReplaceProduct(shelfOffProductId, productId);
                        }
                    }
                    this._unitOfWork.RegisterSave(group);
                }
                if (oldStatus == ShelfStatus.Shelfed)
                    pack.SetPackShelfed();
                this._unitOfWork.RegisterSave(pack);
            }

            this._unitOfWork.UnitedCommit();
        }
        #endregion

        #region # 设置套餐浏览量 —— void SetPackViews(Guid packId)
        /// <summary>
        /// 设置套餐浏览量
        /// </summary>
        public void SetPackViews(Guid packId)
        {
            BalePack pack = this._unitOfWork.Resolve<BalePack>(packId);
            pack.SetViews();

            this._unitOfWork.RegisterSave(pack);
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
            BalePack pack = this._unitOfWork.Resolve<BalePack>(packId);
            if (pack.SourcePackId.HasValue)
            {
                BalePack sourcePack = this._unitOfWork.Resolve<BalePack>(pack.SourcePackId.Value);
                sourcePack.SetSales();
                this._unitOfWork.RegisterSave(sourcePack);
            }

            pack.SetSales();

            this._unitOfWork.RegisterSave(pack);
            this._unitOfWork.UnitedCommit();
        }
        #endregion


        //查询部分

        #region # 获取下架商品Id列表 —— IEnumerable<Guid> GetShelfOffProducts()

        /// <summary>
        /// 获取下架商品
        /// </summary>
        /// <returns>下架商品Id集</returns>
        public IEnumerable<Guid> GetShelfOffProducts()
        {
            return this._repMediator.BalePackProductRep.GetShelfOffProducts();
        }
        #endregion

        #region # 获取下架商品所在套餐列表 —— IDictionary<Guid, string> GetPackIdAndName(Guid productId)
        /// <summary>
        /// 获取下架商品所在套餐列表
        /// </summary>
        /// <param name="productId">下架商品Id</param>
        /// <returns>IDictionary[Guid, string]，[套餐Id, 套餐名称]</returns>
        public IDictionary<Guid, string> GetPackIdAndName(Guid productId)
        {
            IEnumerable<BalePackGroup> groups = this._repMediator.BalePackProductRep.GetBalePackGroupsByProId(productId);
            IEnumerable<Guid> choiceAreaIds = groups.Select(s => s.ChoiceAreaId);

            return this._repMediator.BalePackChoiceAreaRep.GetPackByChoiceAreaIds(choiceAreaIds);
        }
        #endregion

        #region # 根据组Id品类Id获取品类已配置商品列表 —— IEnumerable<BalePackProductInfo> GetProductsByCategoryId(...
        /// <summary>
        /// 根据组Id品类Id获取品类已配置商品列表
        /// </summary>
        /// <param name="groupId">组Id</param>
        /// <param name="categoryId">品类Id</param>
        /// <returns>品类已配置商品列表</returns>
        public IEnumerable<BalePackProductInfo> GetProductsByCategoryId(Guid groupId, Guid categoryId)
        {
            IEnumerable<BalePackProduct> products = this._repMediator.BalePackProductRep.GetProductsByCategoryId(groupId, categoryId);
            IEnumerable<BalePackProductInfo> productInfos = products.Select(x => x.ToDTO());

            return productInfos;
        }
        #endregion

        #region # 根据组Id品类Id获取品类已配置商品列表 —— IDictionary<Guid, bool> GetProducts(Guid groupId, Guid categoryId)
        /// <summary>
        /// 根据组Id品类Id获取品类已配置商品列表
        /// </summary>
        /// <param name="groupId">组Id</param>
        /// <param name="categoryId">品类Id</param>
        /// <returns>IDictionary[Guid, bool]，[商品Id, 是否已上架]</returns>
        public IDictionary<Guid, bool> GetProducts(Guid groupId, Guid categoryId)
        {
            return this._repMediator.BalePackProductRep.GetProducts(groupId, categoryId);
        }
        #endregion

        #region # 根据组Id获取组内三级品类Id列表及是否含下架商品及是否包含商品 —— Tuple<Guid, bool,bool>  GetCategoryIdsByGroup(Guid groupId)
        /// <summary>
        ///根据组Id获取组内三级品类Id列表及是否含下架商品及是否包含商品
        /// </summary>
        /// <param name="groupId">组Id</param>
        /// <returns>IEnumerable<Tuple<Guid, bool, bool>>，[组内三级品类Id列表, 品类内是否含下架商品true|有，false|无,品类下是否含有商品]</returns>
        public IEnumerable<Tuple<Guid, bool, bool>> GetCategoryIdsByGroup(Guid groupId)
        {
            BalePackGroup group = this._repMediator.BalePackGroupRep.Single(groupId);

            return group.GetCategoryIds();
        }
        #endregion

        #region # 根据套餐Id获取套餐内选区列表 —— IEnumerable<ChoiceAreaInfo> GetChoiceAreas(Guid packId)
        /// <summary>
        /// 根据套餐Id获取套餐内选区列表
        /// </summary>
        /// <param name="packId">套餐Id</param>
        /// <returns>套餐内选区列表</returns>
        public IEnumerable<BalePackChoiceAreaInfo> GetChoiceAreas(Guid packId)
        {
            BalePack pack = this._repMediator.BalePackRep.Single(packId);

            return pack.ChoiceAreas.OrderByDescending(s => s.AddedTime).Select(s => s.ToDTO(this._repMediator));
        }
        #endregion

        #region # 根据套餐Id获取套餐内选区列表 —— IDictionary<Guid, string> GetChoiceAreasByPackId(Guid packId)
        /// <summary>
        /// 根据套餐Id获取套餐内选区列表
        /// </summary>
        /// <param name="packId">套餐Id</param>
        /// <returns>IDictionary[Guid, string]，[选区Id, 选区名称]</returns>
        public IDictionary<Guid, string> GetChoiceAreasByPackId(Guid packId)
        {
            BalePack pack = this._repMediator.BalePackRep.Single(packId);

            return pack.ChoiceAreas.OrderByDescending(s => s.AddedTime).ToDictionary(s => s.Id, s => s.Name);
        }
        #endregion

        #region # 根据套餐状态标签分页获取大包套餐列表 —— PageModel<BalePackInfo> GetBalePackByPage(string keywords...
        /// <summary>
        /// 根据套餐状态标签分页获取大包套餐列表
        /// </summary>
        /// <param name="keywords">关键字</param>
        /// <param name="status">套餐状态</param>
        /// <param name="labels">套餐标签</param>
        /// <param name="sort">排序条件 true为倒序</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页容量</param>
        /// <returns>大包套餐列表</returns>
        public PageModel<BalePackInfo> GetBalePackByPage(string keywords, ShelfStatus? status, IList<string> labels, Dictionary<string, bool> sort, int pageIndex, int pageSize)
        {
            int rowCount, pageCount;

            IEnumerable<BalePack> balePacks = this._repMediator.BalePackRep.FindBalePackByPage(keywords, status, labels, sort, pageIndex, pageSize, out rowCount, out pageCount);
            IEnumerable<BalePackInfo> balePackInfos = balePacks.Select(s => s.ToDTO());

            return new PageModel<BalePackInfo>(balePackInfos, pageIndex, pageSize, pageCount, rowCount);
        }
        #endregion

        #region # 根据套餐状态标签分页获取定制套餐列表 —— PageModel<BalePackInfo> GetCustPackByPage(string keywords...
        /// <summary>
        /// 根据套餐状态标签分页获取定制套餐列表
        /// </summary>
        /// <param name="keywords">关键字</param>
        /// <param name="status">套餐状态</param>
        /// <param name="labels">套餐标签</param>
        /// <param name="sort">排序条件 true为倒序</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页容量</param>
        /// <returns>定制套餐列表</returns>
        public PageModel<BalePackInfo> GetCustPackByPage(string keywords, ShelfStatus? status, IList<string> labels, Dictionary<string, bool> sort, int pageIndex, int pageSize)
        {
            int rowCount, pageCount;

            IEnumerable<BalePack> custPacks = this._repMediator.BalePackRep.FindCustPackByPage(keywords, status, labels, sort, pageIndex, pageSize, out rowCount, out pageCount);
            IEnumerable<BalePackInfo> custPackInfos = custPacks.Select(s => s.ToDTO());

            return new PageModel<BalePackInfo>(custPackInfos, pageIndex, pageSize, pageCount, rowCount);
        }
        #endregion

        #region # 根据选区Id获取选区内组列表 —— IEnumerable<GroupInChoiceAreaInfo> GetGroupsByChoiceAreaId(Guid choiceAreaId)
        /// <summary>
        /// 根据选区Id获取选区内组列表
        /// </summary>
        /// <param name="choiceAreaId">选区Id</param>
        /// <returns>选区内组列表</returns>
        public IEnumerable<BalePackGroupInfo> GetGroupsByChoiceAreaId(Guid choiceAreaId)
        {
            IEnumerable<BalePackGroup> groups = this._repMediator.BalePackGroupRep.FindByChoiceAreaId(choiceAreaId);

            return groups.Select(s => s.ToDTO());
        }
        #endregion

        #region # 根据套餐Id获取套餐内组列表 —— IEnumerable<GroupInChoiceAreaInfo> GetGroupsByPackId(Guid packId)
        /// <summary>
        /// 根据套餐Id获取套餐内组列表
        /// </summary>
        /// <param name="packId">套餐Id</param>
        /// <returns>套餐内组列表</returns>
        public IEnumerable<BalePackGroupInfo> GetGroupsByPackId(Guid packId)
        {
            List<Guid> choiceAreaIds = this._repMediator.BalePackRep.GetChoiceAreaIdsByPackId(packId).ToList();
            IEnumerable<BalePackGroup> groups = this._repMediator.BalePackGroupRep.GetGroupByChoiceAreaIds(choiceAreaIds);

            return groups.Select(s => s.ToDTO());
        }
        #endregion

        #region # 根据套餐获取大包|定制套餐选区树（选区-组-品类|定制商品集-品牌） —— IEnumerable<BalePackAreaTreeInfo> GetBalePackAreaTreeInfo(Guid packId)
        /// <summary>
        /// 根据套餐获取大包|定制套餐选区树（选区-组-品类|定制商品集-品牌）
        /// </summary>
        /// <param name="packId"></param>
        /// <returns></returns>
        public IEnumerable<BalePackAreaTreeInfo> GetBalePackAreaTreeInfo(Guid packId)
        {

            //套餐
            BalePack pack = this._repMediator.BalePackRep.Single(packId);
            //选区集
            List<BalePackChoiceArea> packAreas = pack.ChoiceAreas.OrderByDescending(s => s.AddedTime).ToList();
            //子选区集
            List<Guid> choiceAreaIds = packAreas.Select(x => x.Id).ToList();
            List<BalePackGroup> packGroups = this._repMediator.BalePackGroupRep.GetGroupByChoiceAreaIds(choiceAreaIds).ToList();
            //品类集
            List<BalePackCategory> packCategorys = packGroups.SelectMany(x => x.BalePackCategorys).ToList();
            //商品集
            List<BalePackProduct> packProducts = packCategorys.SelectMany(x => x.BalePackProducts).ToList();
            //三级品类集
            List<Guid> categoryIds = packCategorys.Select(x => x.CategoryId).Distinct().ToList();
            //商品Id集
            List<Guid> skuIds = packProducts.Select(x => x.ProductId).Distinct().ToList();
            //品类 品牌基本信息
            IDictionary<Guid, CategoryInfo> categoryInfos = new Dictionary<Guid, CategoryInfo>();
            if (categoryIds.Any())
                categoryInfos = this._categoryContract.GetCategoryByIds(categoryIds);
            IDictionary<Guid, IEnumerable<BrandInfo>> cbrands = new Dictionary<Guid, IEnumerable<BrandInfo>>();
            if (categoryIds.Any())
                cbrands = this._categoryContract.GetBrandsByCategoryIds(categoryIds);
            //商品基本信息
            List<HDIPlatform.ResourceService.IAppService.DTOs.Outputs.ProductContext.ProductSkuInfo> listProductInfos =
                new List<ResourceService.IAppService.DTOs.Outputs.ProductContext.ProductSkuInfo>();
            if (skuIds.Any())
                listProductInfos = this._productContract.GetProductSkusByIds(skuIds).ToList();
            List<BalePackAreaTreeInfo> packAreaInfos = new List<BalePackAreaTreeInfo>();

            packAreas.ForEach(area =>
            {
                #region # 选区
                List<BalePackGroup> areaGroups = packGroups.Where(x => x.ChoiceAreaId == area.Id).ToList();
                List<BalePackAreaTreeInfo> packAreaGroupInfos = new List<BalePackAreaTreeInfo>();
                //选区
                BalePackAreaTreeInfo packArea = new BalePackAreaTreeInfo
                {
                    IsRequired = areaGroups.Any(x => x.Required),
                    ItemId = area.Id,
                    ItemName = area.Name,
                    ChildNodes = new List<BalePackAreaTreeInfo>()
                };
                areaGroups.ForEach(areaGroup =>
                {
                    #region # 组
                    List<BalePackAreaTreeInfo> packAreaGroupCategoryInfos = new List<BalePackAreaTreeInfo>();
                    List<BalePackCategory> packAreaGroupCategorys = areaGroup.BalePackCategorys.ToList();
                    //组
                    BalePackAreaTreeInfo packAreaGroup = new BalePackAreaTreeInfo
                    {
                        IsRequired = areaGroup.Required,
                        ItemId = areaGroup.Id,
                        ItemName = areaGroup.Name,
                        ChildNodes = new List<BalePackAreaTreeInfo>()

                    };
                    packAreaGroupCategorys.ForEach(packCategory =>
                    {
                        #region # 品类
                        List<BalePackAreaTreeInfo> packAreaGroupCategoryBrandInfos = new List<BalePackAreaTreeInfo>();
                        if (categoryInfos.ContainsKey(packCategory.CategoryId))
                        {
                            //品类
                            BalePackAreaTreeInfo packAreaGroupCategory = new BalePackAreaTreeInfo
                            {

                                ItemId = packCategory.CategoryId,
                                ItemName = categoryInfos[packCategory.CategoryId].Name,
                                ChildNodes = new List<BalePackAreaTreeInfo>(),
                                SkuIds = new List<Guid>()
                            };
                            //大包 获取所有该品类下的品牌集
                            if (pack.BalePackType == BalePackType.Bale)
                            {
                                if (cbrands.ContainsKey(packCategory.CategoryId))
                                {
                                    packAreaGroupCategoryBrandInfos = cbrands[packCategory.CategoryId].Select(
                                        x =>
                                            new BalePackAreaTreeInfo { ItemId = x.Id, ItemName = x.Name, ChildNodes = new List<BalePackAreaTreeInfo>() })
                                        .ToList();
                                }
                            }
                            else //定制：获取商品后，根据商品的品牌属性获取品牌集
                            {
                                //品类下商品Id集
                                List<Guid> packCategorySkuIds = packCategory.BalePackProducts.Select(s => s.ProductId).ToList();
                                packAreaGroupCategoryBrandInfos = listProductInfos.Where(x => packCategorySkuIds.Contains(x.Id)).Select(x =>
                                            new BalePackAreaTreeInfo { ItemId = x.BrandId, ItemName = x.BrandName, ChildNodes = new List<BalePackAreaTreeInfo>() }).DistinctBy(x => x.ItemId)
                                        .ToList();
                                packAreaGroupCategory.SkuIds = packCategorySkuIds;
                            }

                            packAreaGroupCategory.ChildNodes = packAreaGroupCategoryBrandInfos;
                            packAreaGroupCategoryInfos.Add(packAreaGroupCategory);
                        }

                        #endregion
                    });

                    packAreaGroup.ChildNodes = packAreaGroupCategoryInfos;
                    packAreaGroupInfos.Add(packAreaGroup);
                    #endregion

                });

                packArea.ChildNodes = packAreaGroupInfos;
                packAreaInfos.Add(packArea);
                #endregion
            });

            return packAreaInfos;
        }

        #endregion

        #region # 根据固定套餐Id分页获取关联大包套餐列表 —— PageModel<BalePackInfo> GetRelateBalePackByDecoId(Guid decoId, BalePackType? ...
        /// <summary>
        /// 根据固定套餐Id分页获取关联大包套餐列表
        /// </summary>
        /// <param name="decoId">固定套餐Id</param>
        /// <param name="type">大包套餐类型</param>
        /// <param name="keywords">关键字</param>
        /// <param name="status">套餐状态</param>
        /// <param name="labels">套餐标签</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页容量</param>
        /// <returns></returns>
        public PageModel<BalePackInfo> GetRelateBalePackByDecoId(Guid decoId, BalePackType? type, string keywords, ShelfStatus? status, IList<string> labels, int pageIndex, int pageSize)
        {
            int rowCount, pageCount;

            IEnumerable<Guid> relateBalePackIds = this._repMediator.DecorationPack_BalePackRep.FindBalePackIdsByDecorationPack(decoId);
            IEnumerable<BalePack> balePacks = this._repMediator.BalePackRep.FindRelateBalePackByPage(type, keywords, status, labels, relateBalePackIds, pageIndex, pageSize, out rowCount, out pageCount);
            IEnumerable<BalePackInfo> balePackInfos = balePacks.Select(s => s.ToDTO());

            return new PageModel<BalePackInfo>(balePackInfos, pageIndex, pageSize, pageCount, rowCount);
        }
        #endregion

        #region # 根据固定套餐Id分页获取未关联大包套餐列表 —— PageModel<BalePackInfo> GetNoRelateBalePackByDecoId(Guid decoId, BalePackType? ...
        /// <summary>
        /// 根据固定套餐Id分页获取未关联大包套餐列表
        /// </summary>
        /// <param name="decoId">固定套餐Id</param>
        /// <param name="type">大包套餐类型</param>
        /// <param name="keywords">关键字</param>
        /// <param name="status">套餐状态</param>
        /// <param name="labels">套餐标签</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页容量</param>
        /// <returns></returns>
        public PageModel<BalePackInfo> GetNoRelateBalePackByDecoId(Guid decoId, BalePackType? type, string keywords, ShelfStatus? status, IList<string> labels, int pageIndex, int pageSize)
        {
            int rowCount, pageCount;

            IEnumerable<Guid> relateBalePackIds = this._repMediator.DecorationPack_BalePackRep.FindBalePackIdsByDecorationPack(decoId);
            IEnumerable<BalePack> balePacks = this._repMediator.BalePackRep.FindNoRelateBalePackByPage(type, keywords, status, labels, relateBalePackIds, pageIndex, pageSize, out rowCount, out pageCount);
            IEnumerable<BalePackInfo> balePackInfos = balePacks.Select(s => s.ToDTO());

            return new PageModel<BalePackInfo>(balePackInfos, pageIndex, pageSize, pageCount, rowCount);
        }
        #endregion

        #region # 根据套餐Id获取套餐 —— BalePackInfo GetBalePackInfoById(Guid packId)
        /// <summary>
        /// 根据套餐Id获取套餐
        /// </summary>
        /// <param name="packId">套餐Id</param>
        /// <returns></returns>
        public BalePackInfo GetBalePackInfoById(Guid packId)
        {
            BalePack currentPack = this._repMediator.BalePackRep.Single(packId);
            BalePackInfo currentPackInfo = currentPack.ToDTO();

            return currentPackInfo;
        }
        #endregion


        #region # 根据套餐Id批量获取套餐 ——  IEnumerable<BalePackInfo> GetBalePackInfoByIds(IEnumerable<Guid> packIds)
        /// <summary>
        /// 根据套餐Id批量获取套餐
        /// </summary>
        /// <param name="packIds">套餐Ids</param>
        /// <returns></returns>
        public IEnumerable<BalePackInfo> GetBalePackInfoByIds(IEnumerable<Guid> packIds)
        {
            packIds = packIds ?? new Guid[0];
            IEnumerable<BalePack> currentPacks = this._repMediator.BalePackRep.FindAll().Where(x => packIds.Contains(x.Id));
            IEnumerable<BalePackInfo> currentPackInfos = currentPacks.Select(x => x.ToDTO());
            return currentPackInfos;
        }
        #endregion

        #region # 根据套餐Id获取套餐下架商品 —— IDictionary<Guid, int> GetPackShelfOffProCount(IEnumerable<Guid> packIds)
        /// <summary>
        /// 根据套餐Id获取套餐下架商品
        /// </summary>
        /// <returns>IDictionary[Guid, string]，[套餐Id, 下架商品个数]</returns>
        public IDictionary<Guid, int> GetPackShelfOffProCount(IEnumerable<Guid> packIds)
        {
            packIds = packIds ?? new Guid[0];

            IDictionary<Guid, int> shelfOffProDic = new Dictionary<Guid, int>();

            foreach (Guid packId in packIds)
            {
                IEnumerable<Guid> choiceAreaIds = this._repMediator.BalePackRep.GetChoiceAreaIdsByPackId(packId);
                int shelfOffProCount = this._repMediator.BalePackProductRep.GetShelfOffProCountByAreaIds(choiceAreaIds);
                shelfOffProDic.Add(packId, shelfOffProCount);
            }

            return shelfOffProDic;
        }
        #endregion

        #region # 根据套餐选区组三级品类获取套餐下商品列表 —— IEnumerable<Guid> GetProductsByPackId(Guid packId, Guid? choiceAreaId, Guid? groupId, Guid? categoryId)

        /// <summary>
        /// 获取套餐下商品
        /// </summary>
        /// <param name="packId">套餐Id</param>
        /// <param name="choiceAreaId">选区Id</param>
        /// <param name="groupId">组Id</param>
        /// <param name="categoryId">三级品类Id</param>
        /// <returns></returns>
        public IEnumerable<Guid> GetProductsByPackId(Guid packId, Guid? choiceAreaId, Guid? groupId, Guid? categoryId)
        {
            List<Guid> choiceAreaIds = new List<Guid>();
            if (choiceAreaId.HasValue)
                choiceAreaIds.Add(choiceAreaId.Value);
            else
                choiceAreaIds = this._repMediator.BalePackRep.GetChoiceAreaIdsByPackId(packId).ToList();
            IEnumerable<Guid> productIds = this._repMediator.BalePackProductRep.GetProductIdsByPackId(choiceAreaIds, groupId, categoryId);
            return productIds;
        }
        #endregion

        #region # 根据源套餐Id获取套餐当前版本号 —— BalePackInfo GetPackVersionNumber(Guid sourcePackId)
        /// <summary>
        /// 根据源套餐Id获取套餐当前版本号及套餐副本Id
        /// </summary>
        /// <param name="sourcePackId">源套餐Id</param>
        /// <returns></returns>
        public BalePackInfo GetPackVersionNumber(Guid sourcePackId)
        {
            BalePack balePack = this._repMediator.BalePackRep.FindBalePackVersionNumber(sourcePackId);
            BalePackInfo balePackInfo = balePack.ToDTO();
            return balePackInfo;
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
            List<BalePack> balePacks = this._repMediator.BalePackRep.FindBaleVersionNumbers(packDic.Values).ToList();

            List<Tuple<Guid, ShelfStatus, bool>> result = new List<Tuple<Guid, ShelfStatus, bool>>();

            foreach (KeyValuePair<Guid, Guid> item in packDic)
            {
                var source = balePacks.SingleOrDefault(s => s.Id == item.Value);
                var newVersion = balePacks.Where(s=>s.SourcePackId == item.Value).OrderByDescending(x => x.AddedTime).ThenByDescending(x => x.VersionNumber).FirstOrDefault() ?? source;
                if (source != null)
                    result.Add(new Tuple<Guid, ShelfStatus, bool>(item.Key, source.PackShelfStatus, item.Key == newVersion.Id));
            }
            return result;
        }

        #endregion


        #region # 验证大包套餐名称是否存在 —— bool ExistsBalePackName(Guid? packId, string packName
        /// <summary>
        /// 验证大包套餐名称是否存在
        /// </summary>
        /// <param name="packId">套餐Id</param>
        /// <param name="packName">套餐名称</param>
        /// <returns></returns>
        public bool ExistsBalePackName(Guid? packId, string packName)
        {
            return this._repMediator.BalePackRep.ExistsPackName(packId, packName, BalePackType.Bale);
        }
        #endregion

        #region # 验证定制套餐名称是否存在 —— bool ExistsCustPackName(Guid? packId, string packName)
        /// <summary>
        /// 验证定制套餐名称是否存在
        /// </summary>
        /// <param name="packId">套餐Id</param>
        /// <param name="packName">套餐名称</param>
        /// <returns></returns>
        public bool ExistsCustPackName(Guid? packId, string packName)
        {
            return this._repMediator.BalePackRep.ExistsPackName(packId, packName, BalePackType.Customized);
        }
        #endregion

        #region # 验证选区名称是否存在 —— bool ExistsChoiceAreaName(Guid packId, Guid? choiceAreaId, string choiceAreaName)

        /// <summary>
        /// 验证选区名称是否存在
        /// </summary>
        /// <param name="packId">套餐Id</param>
        /// <param name="choiceAreaId">选区Id</param>
        /// <param name="choiceAreaName">选区名称</param>
        /// <returns></returns>
        public bool ExistsChoiceAreaName(Guid packId, Guid? choiceAreaId, string choiceAreaName)
        {
            return this._repMediator.BalePackChoiceAreaRep.ExistsChoiceAreaName(packId, choiceAreaId, choiceAreaName);
        }
        #endregion

        #region  # 验证大包套餐组内是否有品类 —— bool ExistsCategoryByPackId(Guid packId)
        /// <summary>
        /// 验证大包套餐组内是否有品类
        /// </summary>
        /// <param name="packId">套餐Id</param>
        /// <returns></returns>
        public bool ExistsCategoryByPackId(Guid packId)
        {
            IEnumerable<Guid> choiceAreaIds = this._repMediator.BalePackRep.GetChoiceAreaIdsByPackId(packId);

            return this._repMediator.BalePackGroupRep.ExistsCategory(choiceAreaIds);
        }
        #endregion

        #region # 验证定制套餐组内品类下是否有商品 —— bool ExistsProductByPackId(Guid packId)
        /// <summary>
        /// 验证定制套餐组内下是否有商品
        /// </summary>
        /// <param name="packId">套餐Id</param>
        /// <returns></returns>
        public bool ExistsProductByPackId(Guid packId)
        {
            IEnumerable<Guid> choiceAreaIds = this._repMediator.BalePackRep.GetChoiceAreaIdsByPackId(packId);

            return this._repMediator.BalePackGroupRep.ExistsProduct(choiceAreaIds);
        }
        #endregion


        #region # 验证分组名称是否存在 —— bool ExistsGroupName(Guid choiceAreaId, Guid? groupId, string groupName)

        /// <summary>
        /// 验证分组名称是否存在
        /// </summary>
        /// <param name="choiceAreaId">选区Id</param>
        /// <param name="groupId">组Id</param>
        /// <param name="groupName">组名称</param>
        /// <returns></returns>
        public bool ExistsGroupName(Guid choiceAreaId, Guid? groupId, string groupName)
        {
            return this._repMediator.BalePackGroupRep.ExistsGroupName(choiceAreaId, groupId, groupName);
        }

        #endregion




        #region # 批量获取大包|定制套餐选区树（选区-组-品类|定制商品集-品牌） —— Dictionary<Guid, IEnumerable<BalePackAreaTreeInfo>> GetBalePackAreaTreeInfos(IEnumerable<Guid> packIds)
        /// <summary>
        /// 批量获取大包|定制套餐选区树（选区-组-品类|定制商品集-品牌）
        /// </summary>
        /// <param name="packIds">套餐Id集</param>
        /// <returns>套餐id|（选区-组-品类|定制商品集-品牌）</returns>
        public Dictionary<Guid, IEnumerable<BalePackAreaTreeInfo>> GetBalePackAreaTreeInfos(IEnumerable<Guid> packIds)
        {
            packIds = packIds ?? new Guid[0];
            Dictionary<Guid, IEnumerable<BalePackAreaTreeInfo>> dic = new Dictionary<Guid, IEnumerable<BalePackAreaTreeInfo>>();
            //套餐
            IDictionary<Guid, BalePack> packs = this._repMediator.BalePackRep.Find(packIds);
            //选区集
            List<BalePackChoiceArea> packAreaAlls = packs.Values.SelectMany(x=>x.ChoiceAreas).OrderByDescending(s => s.AddedTime).ToList();
            //子选区集
            List<Guid> choiceAreaIds = packAreaAlls.Select(x => x.Id).ToList();
            List<BalePackGroup> packGroups = this._repMediator.BalePackGroupRep.GetGroupByChoiceAreaIds(choiceAreaIds).ToList();
            //品类集
            List<BalePackCategory> packCategorys = packGroups.SelectMany(x => x.BalePackCategorys).ToList();
            //商品集
            List<BalePackProduct> packProducts = packCategorys.SelectMany(x => x.BalePackProducts).ToList();
            //三级品类集
            List<Guid> categoryIds = packCategorys.Select(x => x.CategoryId).Distinct().ToList();
            //商品Id集
            List<Guid> skuIds = packProducts.Select(x => x.ProductId).Distinct().ToList();
            //品类 品牌基本信息
            IDictionary<Guid, CategoryInfo> categoryInfos = new Dictionary<Guid, CategoryInfo>();
            if (categoryIds.Any())
                categoryInfos = this._categoryContract.GetCategoryByIds(categoryIds);
            IDictionary<Guid, IEnumerable<BrandInfo>> cbrands = new Dictionary<Guid, IEnumerable<BrandInfo>>();
            if (categoryIds.Any())
                cbrands = this._categoryContract.GetBrandsByCategoryIds(categoryIds);
            //商品基本信息
            List<HDIPlatform.ResourceService.IAppService.DTOs.Outputs.ProductContext.ProductSkuInfo> listProductInfos =
                new List<ResourceService.IAppService.DTOs.Outputs.ProductContext.ProductSkuInfo>();
            if (skuIds.Any())
                listProductInfos = this._productContract.GetProductSkusByIds(skuIds).ToList();
            
            packs.ForEach(pack =>
            {
                List<BalePackAreaTreeInfo> packAreaInfos = new List<BalePackAreaTreeInfo>();
                List<BalePackChoiceArea> packAreas = packAreaAlls.Where(x => x.BalePack.Id == pack.Key).ToList();
                packAreas.ForEach(area =>
                {
                    #region # 选区
                    List<BalePackGroup> areaGroups = packGroups.Where(x => x.ChoiceAreaId == area.Id).ToList();
                    List<BalePackAreaTreeInfo> packAreaGroupInfos = new List<BalePackAreaTreeInfo>();
                    //选区
                    BalePackAreaTreeInfo packArea = new BalePackAreaTreeInfo
                    {
                        IsRequired = areaGroups.Any(x => x.Required),
                        ItemId = area.Id,
                        ItemName = area.Name,
                        ChildNodes = new List<BalePackAreaTreeInfo>()
                    };
                    areaGroups.ForEach(areaGroup =>
                    {
                        #region # 组
                        List<BalePackAreaTreeInfo> packAreaGroupCategoryInfos = new List<BalePackAreaTreeInfo>();
                        List<BalePackCategory> packAreaGroupCategorys = areaGroup.BalePackCategorys.ToList();
                        //组
                        BalePackAreaTreeInfo packAreaGroup = new BalePackAreaTreeInfo
                        {
                            IsRequired = areaGroup.Required,
                            ItemId = areaGroup.Id,
                            ItemName = areaGroup.Name,
                            ChildNodes = new List<BalePackAreaTreeInfo>()

                        };
                        packAreaGroupCategorys.ForEach(packCategory =>
                        {
                            #region # 品类
                            List<BalePackAreaTreeInfo> packAreaGroupCategoryBrandInfos = new List<BalePackAreaTreeInfo>();
                            if (categoryInfos.ContainsKey(packCategory.CategoryId))
                            {
                                //品类
                                BalePackAreaTreeInfo packAreaGroupCategory = new BalePackAreaTreeInfo
                                {

                                    ItemId = packCategory.CategoryId,
                                    ItemName = categoryInfos[packCategory.CategoryId].Name,
                                    ChildNodes = new List<BalePackAreaTreeInfo>(),
                                    SkuIds = new List<Guid>()
                                };
                                //大包 获取所有该品类下的品牌集
                                if (pack.Value.BalePackType == BalePackType.Bale)
                                {
                                    if (cbrands.ContainsKey(packCategory.CategoryId))
                                    {
                                        packAreaGroupCategoryBrandInfos = cbrands[packCategory.CategoryId].Select(
                                            x =>
                                                new BalePackAreaTreeInfo { ItemId = x.Id, ItemName = x.Name, ChildNodes = new List<BalePackAreaTreeInfo>() })
                                            .ToList();
                                    }
                                }
                                else //定制：获取商品后，根据商品的品牌属性获取品牌集
                                {
                                    //品类下商品Id集
                                    List<Guid> packCategorySkuIds = packCategory.BalePackProducts.Select(s => s.ProductId).ToList();
                                    packAreaGroupCategoryBrandInfos = listProductInfos.Where(x => packCategorySkuIds.Contains(x.Id)).Select(x =>
                                                new BalePackAreaTreeInfo { ItemId = x.BrandId, ItemName = x.BrandName, ChildNodes = new List<BalePackAreaTreeInfo>() }).DistinctBy(x => x.ItemId)
                                            .ToList();
                                    packAreaGroupCategory.SkuIds = packCategorySkuIds;
                                }

                                packAreaGroupCategory.ChildNodes = packAreaGroupCategoryBrandInfos;
                                packAreaGroupCategoryInfos.Add(packAreaGroupCategory);
                            }

                            #endregion
                        });

                        packAreaGroup.ChildNodes = packAreaGroupCategoryInfos;
                        packAreaGroupInfos.Add(packAreaGroup);
                        #endregion

                    });

                    packArea.ChildNodes = packAreaGroupInfos;
                    packAreaInfos.Add(packArea);
                    #endregion
                });

                dic.Add(pack.Key, packAreaInfos);
            });

            return dic;
        }

        #endregion

        #region # 测试 Tuple —— void Test(Guid id)
        /// <summary>
        /// 测试 Tuple
        /// </summary>
        /// <param name="id"></param>
        public void Test(Guid id)
        {

            //IEnumerable<Tuple<Guid, bool, bool>> r = this.GetCategoryIdsByGroup(id);
            //r.ForEach(i =>
            //{
            //    Guid cateId = i.Item1;
            //    bool isM = i.Item2;
            //    bool isE = i.Item3;
            //});

            //List<Guid> packIds = new List<Guid> { Guid.Parse("260A6F62-1EED-4F58-BF20-E5AD96733E74"), Guid.Parse("BE401028-CCF5-434F-B9A9-89D822E767BE"), Guid.Parse("96138A48-3D9F-4D6B-A725-69C3A2A2C758"), Guid.Parse("D7450E13-B9A3-4C4F-A604-FD9EDEB24A06") };
            //Dictionary<Guid, IEnumerable<BalePackAreaTreeInfo>> dic = this.GetBalePackAreaTreeInfos(packIds);
            var skuList = this.GetShelfOffProducts().Distinct().ToList();
            var pList = this._productContract.GetProductSkusByIds(skuList).OrderBy(x=>x.ProductId).ToList();
            var pIdList = pList.Select(x => x.ProductId).ToList();
            var pdIdList = pList.Select(x => x.ProductId).Distinct().ToList();
            var e = skuList.Except(pIdList).ToList();
            var i = pIdList.Except(skuList).ToList();

        }
        #endregion

    }
}
