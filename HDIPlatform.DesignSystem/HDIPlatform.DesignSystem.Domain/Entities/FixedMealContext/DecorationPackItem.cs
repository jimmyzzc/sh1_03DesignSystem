using ShSoft.Common.PoweredByLee;
using ShSoft.Infrastructure.EntityBase;
using ShSoft.ValueObjects.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using HDIPlatform.DesignSystem.Domain.EventSources.FixedMealContext;
using ShSoft.Infrastructure.EventBase.Mediator;

namespace HDIPlatform.DesignSystem.Domain.Entities.FixedMealContext
{
    /// <summary>
    /// 套餐模板项
    /// </summary>
    [Serializable]
    public class DecorationPackItem : AggregateRootEntity
    {
        #region # 构造器

        #region 01.无参构造器
        /// <summary>
        /// 无参构造器
        /// </summary>
        protected DecorationPackItem()
        {
            //初始化导航属性
            this.PackCraftEntities = new HashSet<DecorationPackCraft>();
            this.PackSkus = new HashSet<DecorationPackSku>();
            IsClone = false;
        }
        #endregion

        #region 02.创建套餐模板项构造器

        /// <summary>
        /// 创建套餐模板项构造器
        /// </summary>
        /// <param name="itemName">模板项名称</param>
        /// <param name="packId">套餐模板Id</param>
        /// <param name="packSpaceId">套餐模板空间Id</param>
        /// <param name="categoryArea">品类区域</param>
        /// <param name="categoryIds">三级品类Id集</param>
        public DecorationPackItem(string itemName, Guid packId, Guid packSpaceId, CategoryArea categoryArea, IEnumerable<Guid> categoryIds)
            : this()
        {
            base.Name = itemName;
            this.PackId = packId;
            this.PackSpaceId = packSpaceId;
            this.CategoryArea = categoryArea;
            this.SetCategoryIds(categoryIds);
        }
        #endregion

        #endregion

        #region # 属性

        #region 套餐模板Id —— Guid PackId
        /// <summary>
        /// 套餐模板Id
        /// </summary>
        public Guid PackId { get; private set; }
        #endregion

        #region 套餐模板空间Id —— Guid PackSpaceId
        /// <summary>
        /// 套餐模板空间Id
        /// </summary>
        public Guid PackSpaceId { get; private set; }
        #endregion

        #region 默认商品SKU数量 —— float? DefaultSkuQuantity
        /// <summary>
        /// 默认商品SKU数量
        /// </summary>
        public float? DefaultSkuQuantity { get; private set; }
        #endregion

        #region 默认工艺工程量 —— float? DefaultCraftQuantity
        /// <summary>
        /// 默认工艺工程量
        /// </summary>
        public float? DefaultCraftQuantity { get; private set; }
        #endregion

        #region 品类区域 —— CategoryArea CategoryArea
        /// <summary>
        /// 品类区域
        /// </summary>
        public CategoryArea CategoryArea { get; private set; }
        #endregion

        #region 是否克隆 —— bool IsClone
        /// <summary>
        /// 是否克隆
        /// </summary>
        public bool IsClone { get; private set; }
        #endregion

        #region 设计要求 —— string Requirement
        /// <summary>
        /// 设计要求
        /// </summary>
        public string Requirement { get; private set; }
        #endregion


        #region 只读属性 - 品类集 —— IList<Guid> CategoryIds
        /// <summary>
        /// 只读属性 - 品类集
        /// </summary>
        public IList<Guid> CategoryIds
        {
            get
            {
                return string.IsNullOrWhiteSpace(this.CategoryIdsStr)
                    ? new Guid[0]
                    : this.CategoryIdsStr.JsonToObject<IList<Guid>>();
            }
        }
        #endregion

        #region 内部属性 - 品类集序列化字符串 —— string  CategoryIdsStr
        /// <summary>
        /// 内部属性 - 品类集序列化字符串
        /// </summary>
        public string CategoryIdsStr { get; private set; }
        #endregion

        #region 只读属性 - 品牌Id集 —— IList<Guid> BrandIds
        /// <summary>
        /// 只读属性 - 品牌Id集
        /// </summary>
        public IList<Guid> BrandIds
        {
            get { return this.PackSkus.Select(x => x.BrandId).Distinct().ToList(); }
        }
        #endregion

        #region 只读属性 - 是否所有商品SKU都已上架 —— bool AllSkuShelved
        /// <summary>
        /// 只读属性 - 是否所有商品SKU都已上架
        /// </summary>
        public bool AllSkuShelved
        {
            get { return this.PackSkus.All(x => x.Shelved); }
        }
        #endregion

        #region 只读属性 - 是否所有工艺都已上架 —— bool AllCraftShelved
        /// <summary>
        /// 只读属性 - 是否所有工艺都已上架
        /// </summary>
        public bool AllCraftShelved
        {
            get { return this.PackCraftEntities.All(x => x.Shelved); }
        }
        #endregion


        #region 只读属性 - 是否所有商品SKU都没变价 —— bool AllSkuNoChanged
        /// <summary>
        /// 只读属性 - 是否所有商品SKU都没变价
        /// </summary>
        public bool AllSkuNoChanged
        {
            get { return this.PackSkus.All(x => !x.Changed); }
        }
        #endregion

        #region 只读属性 - 是否所有工艺都没变价 —— bool AllCraftNoChanged
        /// <summary>
        /// 只读属性 - 是否所有工艺都没变价
        /// </summary>
        public bool AllCraftNoChanged
        {
            get { return this.PackCraftEntities.All(x => !x.Changed); }
        }
        #endregion



        #region 导航属性 - 套餐模板工艺项集 —— ICollection<DecorationPackCraftEntity> PackCraftEntities
        /// <summary>
        /// 导航属性 - 套餐模板工艺项集
        /// </summary>
        public virtual ICollection<DecorationPackCraft> PackCraftEntities { get; private set; }
        #endregion

        #region 导航属性 - 套餐模板商品SKU项集 —— ICollection<DecorationPackSku> PackSkus
        /// <summary>
        /// 导航属性 - 套餐模板商品SKU项集
        /// </summary>
        public virtual ICollection<DecorationPackSku> PackSkus { get; private set; }
        #endregion

        #endregion

        #region # 方法

        #region 修改套餐模板项 —— void UpdateInfo(string packItemName)
        /// <summary>
        /// 修改套餐模板项名称
        /// </summary>
        /// <param name="packItemName">套餐模板项名称</param>
        /// <param name="categoryArea">品类区域</param>
        public void UpdateInfo(string packItemName, CategoryArea categoryArea)
        {
            base.Name = packItemName;
            this.CategoryArea = categoryArea;
        }
        #endregion

        #region 添加商品SKU项集 —— void AddPackSkus(IEnumerable<DecorationPackSku> packSkus)
        /// <summary>
        /// 添加商品SKU项集
        /// </summary>
        /// <param name="packSkus">商品SKU项集</param>
        public void AddPackSkus(IEnumerable<DecorationPackSku> packSkus)
        {
            #region # 验证

            packSkus = packSkus == null ? new DecorationPackSku[0] : packSkus.ToArray();

            if (!packSkus.Any())
            {
                throw new ArgumentNullException("packSkus", "套餐模板商品SKU项集不可为空！");
            }
            if (packSkus.Count() != packSkus.DistinctBy(x => x.SkuId).Count())
            {
                throw new InvalidOperationException("同一选区内，商品SKU不可重复！");
            }

            #endregion

            //先清空
            foreach (DecorationPackSku skuItem in this.PackSkus.ToArray())
            {
                this.PackSkus.Remove(skuItem);
            }

            //再添加
            this.PackSkus.AddRange(packSkus);
            foreach (DecorationPackSku packSku in packSkus)
            {
                packSku.PackItem = this;
            }

            //挂起领域事件 处理套餐上是否包含下架|变价商品|工艺属性
            EventMediator.Suspend(new PackShelvedChangedEvent(this.PackId));

        }
        #endregion

        #region 添加工艺项集 —— void AddPackCrafts(IEnumerable<DecorationPackCraft> packCrafts)
        /// <summary>
        /// 添加工艺项集
        /// </summary>
        /// <param name="packCrafts">工艺项集</param>
        public void AddPackCrafts(IEnumerable<DecorationPackCraft> packCrafts)
        {
            #region # 验证

            packCrafts = packCrafts == null ? new DecorationPackCraft[0] : packCrafts.ToArray();

            if (!packCrafts.Any())
            {
                throw new ArgumentNullException("packCrafts", "套餐模板商品工艺项集不可为空！");
            }
            if (packCrafts.Count() != packCrafts.DistinctBy(x => x.CraftEntityId).Count())
            {
                throw new InvalidOperationException("同一选区内，工艺实体不可重复！");
            }

            #endregion

            //先清空
            foreach (DecorationPackCraft craftItem in this.PackCraftEntities.ToArray())
            {
                this.PackCraftEntities.Remove(craftItem);
            }

            //再添加
            this.PackCraftEntities.AddRange(packCrafts);
            foreach (DecorationPackCraft packCraft in packCrafts)
            {
                packCraft.PackItem = this;
            }

            //挂起领域事件 处理套餐上是否包含下架|变价商品|工艺属性
            EventMediator.Suspend(new PackShelvedChangedEvent(this.PackId));
        }
        #endregion

        #region 设置默认商品数量 —— void SetDefaultSkuQuantity(float defaultSkuQuantity)
        /// <summary>
        /// 设置默认商品数量
        /// </summary>
        /// <param name="defaultSkuQuantity">默认商品SKU数量</param>
        public void SetDefaultSkuQuantity(float defaultSkuQuantity)
        {
            //this.DefaultSkuQuantity = defaultSkuQuantity;
        }
        #endregion

        #region 设置默认工艺工程量 —— void SetDefaultCraftQuantity(float defaultCraftQuantity)
        /// <summary>
        /// 设置默认工艺工程量
        /// </summary>
        /// <param name="defaultCraftQuantity">默认工艺工程量</param>
        public void SetDefaultCraftQuantity(float defaultCraftQuantity)
        {
            //this.DefaultCraftQuantity = defaultCraftQuantity;
        }
        #endregion

        #region 设置默认数量/工程量 —— void SetDefaultQuantity(float defaultSkuQuantity...
        /// <summary>
        /// 设置默认数量/工程量
        /// </summary>
        /// <param name="defaultSkuQuantity">默认商品SKU数量</param>
        /// <param name="defaultCraftQuantity">默认工艺工程量</param>
        public void SetDefaultQuantity(float defaultSkuQuantity, float defaultCraftQuantity)
        {
            //this.DefaultSkuQuantity = defaultSkuQuantity;
            //this.DefaultCraftQuantity = defaultCraftQuantity;
        }
        #endregion

        #region 设置套餐模板Id —— void SetPackId(Guid packId)
        /// <summary>
        /// 设置套餐模板Id
        /// </summary>
        /// <param name="packId">套餐模板Id</param>
        /// <remarks>注意：克隆套餐模板时候用，勿乱用</remarks>
        public void SetPackId(Guid packId)
        {
            this.PackId = packId;
        }
        #endregion

        #region 设置套餐模板空间Id —— void SetPackSpaceId(Guid packSpaceId)
        /// <summary>
        /// 设置套餐模板空间Id
        /// </summary>
        /// <param name="packSpaceId">套餐模板空间Id</param>
        /// <remarks>注意：克隆套餐模板时候用，勿乱用</remarks>
        public void SetPackSpaceId(Guid packSpaceId)
        {
            this.PackSpaceId = packSpaceId;
        }
        #endregion

        #region 替换商品SKU —— void ReplaceSku(Guid sourceSkuId, Guid targetSkuId)

        /// <summary>
        /// 替换商品SKU
        /// </summary>
        /// <param name="sourceSkuId">源商品SKU Id</param>
        /// <param name="targetSkuId">模板商品SKU Id</param>
        /// <param name="costPrice">新商品原价</param>
        public void ReplaceSku(Guid sourceSkuId, Guid targetSkuId, decimal costPrice)
        {
            DecorationPackSku currentPackSku = this.GetPackSku(sourceSkuId);
            currentPackSku.ReplaceSku(targetSkuId, costPrice);
            //挂起领域事件 处理套餐上是否包含下架|变价商品|工艺属性
            //EventMediator.Suspend(new PackShelvedChangedEvent(this.PackId));
        }
        #endregion

        #region 替换工艺实体 —— void ReplaceCraft(Guid sourceCraftEntityId, Guid targetCraftEntityId, decimal costPrice)

        /// <summary>
        /// 替换工艺实体
        /// </summary>
        /// <param name="sourceCraftEntityId">源工艺实体Id</param>
        /// <param name="targetCraftEntityId">模板工艺实体Id</param>
        /// <param name="costPrice">新工艺成本价</param>
        public void ReplaceCraft(Guid sourceCraftEntityId, Guid targetCraftEntityId, decimal costPrice)
        {
            DecorationPackCraft currentPackCraft = this.GetPackCraft(sourceCraftEntityId);
            currentPackCraft.ReplaceCraft(targetCraftEntityId, costPrice);
            //挂起领域事件 处理套餐上是否包含下架|变价商品|工艺属性
            //EventMediator.Suspend(new PackShelvedChangedEvent(this.PackId));
        }
        #endregion

        #region 获取套餐模板SKU项 —— DecorationPackSku GetPackSku(Guid skuId)
        /// <summary>
        /// 获取套餐模板SKU项
        /// </summary>
        /// <param name="skuId">商品SKU Id</param>
        /// <returns>套餐模板SKU项</returns>
        public DecorationPackSku GetPackSku(Guid skuId)
        {
            DecorationPackSku packSku = this.PackSkus.SingleOrDefault(x => x.SkuId == skuId);

            #region # 验证

            if (packSku == null)
            {
                throw new ArgumentOutOfRangeException("skuId", string.Format("选区下不存在Id为\"{0}\"的商品SKU！", skuId));
            }

            #endregion

            return packSku;
        }
        #endregion

        #region 获取套餐模板SKU项 ——  List<DecorationPackSku> GetPackSkus(List<Guid> skuIds)
        /// <summary>
        /// 获取套餐模板SKU项
        /// </summary>
        /// <param name="skuIds">SkuKey集</param>
        /// <returns>套餐模板SKU项</returns>
        public List<DecorationPackSku> GetPackSkus(List<Guid> skuIds)
        {
            List<DecorationPackSku> packSkus = this.PackSkus.Where(x => skuIds.Contains(x.Id)).ToList();
            return packSkus;
        }
        #endregion

        #region 获取套餐模板工艺项 —— DecorationPackCraft GetPackCraft(Guid craftEntityId)
        /// <summary>
        /// 获取套餐模板工艺项
        /// </summary>
        /// <param name="craftEntityId">工艺实体Id</param>
        /// <returns>套餐模板工艺项</returns>
        public DecorationPackCraft GetPackCraft(Guid craftEntityId)
        {
            DecorationPackCraft packCraft = this.PackCraftEntities.SingleOrDefault(x => x.CraftEntityId == craftEntityId);

            #region # 验证

            if (packCraft == null)
            {
                throw new ArgumentOutOfRangeException("craftEntityId", string.Format("选区下不存在Id为\"{0}\"的工艺实体！", craftEntityId));
            }

            #endregion

            return packCraft;
        }
        #endregion

        #region 获取套餐模板工艺项——  List<DecorationPackCraft> GetPackCrafts(List<Guid> craftEntityIds)
        /// <summary>
        /// 获取套餐模板工艺项
        /// </summary>
        /// <param name="craftEntityIds">CraftKey集</param>
        /// <returns>套餐模板工艺项</returns>
        public List<DecorationPackCraft> GetPackCrafts(List<Guid> craftEntityIds)
        {
            List<DecorationPackCraft> packSkus = this.PackCraftEntities.Where(x => craftEntityIds.Contains(x.Id)).ToList();
            return packSkus;
        }
        #endregion

        #region 是否存在商品SKU —— bool ExistsSku(Guid skuId)
        /// <summary>
        /// 是否存在商品SKU
        /// </summary>
        /// <param name="skuId">商品SKU Id</param>
        /// <returns>是否存在</returns>
        public bool ExistsSku(Guid skuId)
        {
            return this.PackSkus.Any(x => x.SkuId == skuId);
        }
        #endregion

        #region 是否存在工艺实体 —— bool ExistsCraft(Guid craftEntityId)
        /// <summary>
        /// 是否存在工艺实体
        /// </summary>
        /// <param name="craftEntityId">工艺实体Id</param>
        /// <returns>是否存在</returns>
        public bool ExistsCraft(Guid craftEntityId)
        {
            return this.PackCraftEntities.Any(x => x.CraftEntityId == craftEntityId);
        }
        #endregion

        #region 克隆套餐时设置是否克隆 —— void SetIsClone()
        /// <summary>
        /// 克隆套餐时设置是否克隆
        /// </summary>
        public void SetIsClone()
        {
            IsClone = true;
        }
        #endregion

        #region 设置品类集（原品类集清空）—— void SetCategoryIds(IEnumerable<Guid> categoryIds)
        /// <summary>
        /// 设置品类集（原品类集清空）
        /// </summary>
        /// <param name="categoryIds">三级品类集</param>
        public void SetCategoryIds(IEnumerable<Guid> categoryIds)
        {
            #region # 验证
            categoryIds = categoryIds == null ? new Guid[0] : categoryIds.ToArray();

            //if (!categoryIds.Any())
            //{
            //    throw new ArgumentNullException("categoryIds", "三级品类集不可为空！");
            //}
            //if (categoryIds.Count() != categoryIds.Distinct().Count())
            //{
            //    throw new InvalidOperationException("同一选区内，三级品类不可重复！");
            //}
            #endregion

            this.CategoryIdsStr = categoryIds.ToJson();
        }
        #endregion

        #region 添加品类集（原品类集进行累加） —— void AddCategoryIds(IEnumerable<Guid> categoryIds)
        /// <summary>
        /// 添加品类集（原品类集进行累加）
        /// </summary>
        /// <param name="categoryIds">三级品类集</param>
        public void AddCategoryIds(IEnumerable<Guid> categoryIds)
        {
            #region # 验证
            categoryIds = categoryIds == null ? new Guid[0] : categoryIds.ToArray();

            //if (!categoryIds.Any())
            //{
            //    throw new ArgumentNullException("categoryIds", "三级品类集不可为空！");
            //}
            if (categoryIds.Count() != categoryIds.Distinct().Count())
            {
                throw new InvalidOperationException("同一选区内，三级品类不可重复！");
            }
            #endregion

            //TODO 累加
            List<Guid> sourceCategoryIds = new List<Guid>();
            sourceCategoryIds.AddRange(this.CategoryIds);
            sourceCategoryIds.AddRange(categoryIds);
            if (sourceCategoryIds.Count() != sourceCategoryIds.Distinct().Count())
            {
                throw new InvalidOperationException("同一选区内，三级品类不可重复！");
            }
            this.CategoryIdsStr = sourceCategoryIds.ToJson();
        }
        #endregion

        #region 删除品类集 —— void DeleteCategoryIds(IEnumerable<Guid> categoryIds)
        /// <summary>
        /// 删除品类集 （删除三级品类下商品集）
        /// </summary>
        /// <param name="categoryIds">三级品类集</param>
        public void DeleteCategoryIds(IEnumerable<Guid> categoryIds)
        {
            #region # 验证
            categoryIds = categoryIds == null ? new Guid[0] : categoryIds.ToArray();

            //if (!categoryIds.Any())
            //{
            //    throw new ArgumentNullException("categoryIds", "三级品类集不可为空！");
            //}
            #endregion

            List<Guid> sourceCategoryIds = new List<Guid>();
            sourceCategoryIds.AddRange(this.CategoryIds);
            foreach (Guid categoryId in categoryIds.ToList())
            {
                if (sourceCategoryIds.Contains(categoryId))
                    sourceCategoryIds.Remove(categoryId);
            }

            if (sourceCategoryIds.Count() != sourceCategoryIds.Distinct().Count())
            {
                throw new InvalidOperationException("同一选区内，三级品类不可重复！");
            }
            //TODO　验证已配置商品是否在此三级品类下

            IEnumerable<DecorationPackSku> packSkus = this.GetCategotySkus(categoryIds);
            foreach (DecorationPackSku packSku in packSkus.ToList())
            {
                if (this.PackSkus.Contains(packSku))
                    this.PackSkus.Remove(packSku);
            }

            this.CategoryIdsStr = sourceCategoryIds.ToJson();
        }
        #endregion

        #region 修改品类集（与原品类集比对处理）—— void UpdateCategoryIds(IEnumerable<Guid> categoryIds)
        /// <summary>
        /// 修改品类集（与原品类集比对处理）
        /// </summary>
        /// <param name="categoryIds">三级品类集</param>
        public void UpdateCategoryIds(IEnumerable<Guid> categoryIds)
        {
            #region # 验证
            categoryIds = categoryIds == null ? new Guid[0] : categoryIds.ToArray();
            if (categoryIds.Count() != categoryIds.Distinct().Count())
            {
                throw new InvalidOperationException("同一选区内，三级品类不可重复！");
            }
            #endregion
            //sourceCategoryIds 原三级品类集(1,2,3,4),categoryIds 新（2,3,4,5）
            List<Guid> sourceCategoryIds = new List<Guid>();
            sourceCategoryIds.AddRange(this.CategoryIds);
            //不同（1,5）
            //List<Guid> differences = this.Compare(sourceCategoryIds, categoryIds.ToList());

            List<Guid> differences = sourceCategoryIds.Except(categoryIds).ToList();

            differences.AddRange(categoryIds.Except(sourceCategoryIds).ToList());

            foreach (Guid categoryId in differences)
            {
                if (sourceCategoryIds.Contains(categoryId))
                {
                    //移除品类 移除商品
                    sourceCategoryIds.Remove(categoryId);
                    IEnumerable<DecorationPackSku> packSkus = this.GetCategotySkus(new List<Guid> { categoryId });
                    foreach (DecorationPackSku packSku in packSkus.ToList())
                    {
                        if (this.PackSkus.Contains(packSku))
                            this.PackSkus.Remove(packSku);
                    }
                }
                else
                {
                    //添加品类
                    sourceCategoryIds.Add(categoryId);
                }
            }


            this.CategoryIdsStr = sourceCategoryIds.ToJson();
        }
        #endregion

        #region 获取三级品类下商品集 —— IEnumerable<DecorationPackSku> GetCategotySkus(IEnumerable<Guid> categoryIds)
        /// <summary>
        /// 获取三级品类下商品集
        /// </summary>
        /// <param name="categoryIds">三级品类Id集</param>
        /// <returns></returns>
        public IEnumerable<DecorationPackSku> GetCategotySkus(IEnumerable<Guid> categoryIds)
        {

            IEnumerable<DecorationPackSku> packSkus = this.PackSkus.Where(x => categoryIds.Contains(x.CategoryId));
            return packSkus;
        }
        #endregion

        #region 设置默认商品工程量放置位置 —— void SetDefaultSkuQuantity(Guid defaultSkuId, decimal skuQuantity, Dictionary<SkuCraftPosition, decimal> skuCraftPositions)
        /// <summary>
        /// 设置默认商品工程量放置位置
        /// </summary>
        /// <param name="defaultSkuId">默认SkuId</param>
        /// <param name="skuQuantity">默认Sku工程量</param>
        /// <param name="skuCraftPositions">放置位置</param>
        public void SetDefaultSkuQuantity(Guid defaultSkuId, decimal skuQuantity, Dictionary<SkuCraftPosition, decimal> skuCraftPositions)
        {
            DecorationPackSku packSku = this.GetPackSku(defaultSkuId);
            packSku.SetDefaultSkuPositions(skuQuantity, skuCraftPositions);
        }
        #endregion

        #region 设置默认工艺工程量放置位置 —— void SetDefaultCraftQuantity(Guid defaultCraftId, decimal craftQuantity, Dictionary<SkuCraftPosition, decimal> skuCraftPositions)
        /// <summary>
        /// 设置默认工艺工程量放置位置
        /// </summary>
        /// <param name="defaultCraftId">默认CraftId</param>
        /// <param name="craftQuantity">默认工艺工程量</param>
        /// <param name="skuCraftPositions">放置位置</param>
        public void SetDefaultCraftQuantity(Guid defaultCraftId, decimal craftQuantity, Dictionary<SkuCraftPosition, decimal> skuCraftPositions)
        {
            DecorationPackCraft packCraft = this.GetPackCraft(defaultCraftId);
            packCraft.SetDefaultCraftPositions(craftQuantity, skuCraftPositions);
        }


        #endregion

        #region 修改套餐模板项设计要求 —— void SetRequirement(string requirement)
        /// <summary>
        /// 修改套餐模板项设计要求
        /// </summary>
        /// <param name="requirement">设计要求</param>
        public void SetRequirement(string requirement)
        {
            Assert.IsFalse(requirement.Length > 200, "产品要求多于100字，请重新配置");
            this.Requirement = requirement;
        }
        #endregion

        #region 删除套餐商品 ——  void RemoveSku(Guid skuId)
        /// <summary>
        /// 删除套餐商品
        /// </summary>
        /// <param name="skuId"></param>
        public void RemoveSku(Guid skuId)
        {
            DecorationPackSku currentPackSku = this.GetPackSku(skuId);
            //非默认移除
            if (!currentPackSku.Default)
                this.PackSkus.Remove(currentPackSku);
        }
        #endregion

        #region 删除套餐下架商品集 ——  void RemoveSku(Guid skuId)
        /// <summary>
        /// 删除套餐下架商品集
        /// </summary>
        public void RemoveAllSku()
        {
            //下架非默认移除
            IEnumerable<DecorationPackSku> packSkus = this.PackSkus.Where(x => !x.Shelved && !x.Default);
            foreach (DecorationPackSku packSku in packSkus.ToList())
            {
                if (this.PackSkus.Contains(packSku))
                    this.PackSkus.Remove(packSku);
            }
        }
        #endregion

        #region 删除套餐工艺 ——  void RemoveSku(Guid skuId)
        /// <summary>
        /// 删除套餐工艺
        /// </summary>
        /// <param name="craftId"></param>
        public void RemoveCraft(Guid craftId)
        {
            DecorationPackCraft currentPackCraft = this.GetPackCraft(craftId);
            //非默认移除
            if (!currentPackCraft.Default)
                this.PackCraftEntities.Remove(currentPackCraft);
        }
        #endregion

        #region 删除套餐下架工艺 ——  void RemoveSku(Guid skuId)
        /// <summary>
        /// 删除套餐下架工艺
        /// </summary>
        public void RemoveAllCraft()
        {
            //下架非默认移除
            IEnumerable<DecorationPackCraft> packCrafts = this.PackCraftEntities.Where(x => !x.Shelved && !x.Default);
            foreach (DecorationPackCraft packCraft in packCrafts.ToList())
            {
                if (this.PackCraftEntities.Contains(packCraft))
                    this.PackCraftEntities.Remove(packCraft);
            }
        }
        #endregion

        #region 删除套餐指定下架商品集 ——  void RemoveSkus(List<Guid> skuIds)
        /// <summary>
        /// 删除套餐指定下架商品集
        /// </summary>
        public void RemoveSkus(List<Guid> skuIds)
        {
            //下架非默认移除
            IEnumerable<DecorationPackSku> packSkus = this.PackSkus.Where(x => skuIds.Contains(x.SkuId) && !x.Default);
            foreach (DecorationPackSku packSku in packSkus.ToList())
            {
                if (this.PackSkus.Contains(packSku))
                    this.PackSkus.Remove(packSku);
            }
        }
        #endregion

        #region 删除套餐指定下架工艺 ——  void RemoveCrafts(List<Guid> craftIds)
        /// <summary>
        /// 删除套餐指定下架工艺
        /// </summary>
        public void RemoveCrafts(List<Guid> craftIds)
        {
            //下架非默认移除
            IEnumerable<DecorationPackCraft> packCrafts = this.PackCraftEntities.Where(x => craftIds.Contains(x.CraftEntityId) && !x.Default);
            foreach (DecorationPackCraft packCraft in packCrafts.ToList())
            {
                if (this.PackCraftEntities.Contains(packCraft))
                    this.PackCraftEntities.Remove(packCraft);
            }
        }
        #endregion

        #region # 存在非默认商品 —— bool ExistsNoDefaultSku(List<Guid> skuIds)
        /// <summary>
        /// 存在非默认商品
        /// </summary>
        /// <param name="skuIds"></param>
        /// <returns></returns>
        public bool ExistsNoDefaultSku(List<Guid> skuIds)
        {
            return this.PackSkus.Any(x => skuIds.Contains(x.SkuId) && !x.Default);
        }
        #endregion

        #region # 存在非默认工艺 —— bool ExistsNoDefaultCraft(List<Guid> craftIds)
        /// <summary>
        /// 存在非默认工艺
        /// </summary>
        /// <param name="craftIds"></param>
        /// <returns></returns>
        public bool ExistsNoDefaultCraft(List<Guid> craftIds)
        {
            return this.PackCraftEntities.Any(x => craftIds.Contains(x.CraftEntityId) && !x.Default);
        }
        #endregion

        #region # 定价保存商品工艺成本价 —— void SaveCostPrice(Dictionary<Guid, decimal> skuInfos, Dictionary<Guid, decimal> craftEntityInfos)

        /// <summary>
        /// 定价保存商品工艺成本价
        /// </summary>
        /// <param name="skuInfos">商品成本价</param>
        /// <param name="craftEntityInfos">工艺成本价</param>
        public void SaveCostPrice(Dictionary<Guid, decimal> skuInfos, Dictionary<Guid, decimal> craftEntityInfos)
        {
            this.PackSkus.ForEach(x =>
            {
                if (skuInfos.ContainsKey(x.SkuId))
                    x.SetCostPrice(decimal.Round(skuInfos[x.SkuId], 2, MidpointRounding.AwayFromZero));
            });

            this.PackCraftEntities.ForEach(x =>
            {
                if (craftEntityInfos.ContainsKey(x.CraftEntityId))
                    x.SetCostPrice(decimal.Round(craftEntityInfos[x.CraftEntityId], 2, MidpointRounding.AwayFromZero));

            });
        }

        #endregion

        #endregion

        #region # 自动实现标签的增删 —— List<T> compare<T>(List<T> dataSources, List<T> parameter)
        ///// <summary>
        ///// 自动实现标签的增删
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="dataSources">要操作的数据源</param>
        ///// <param name="parameter">前端传递过来的参数</param>
        ///// <returns></returns>
        //private List<T> Compare<T>(List<T> dataSources, List<T> parameter)
        //{

        //    if (dataSources.Count < 1)
        //    {
        //        return parameter;
        //    }

        //    // A,B两个集合，(A-B)+(B-A)=A 和 B中所有不同的值
        //    //A={1,2,3,4},B={2,3,4,5} =>(A-B)+(B-A)={1,5}

        //    List<T> differences = dataSources.Except(parameter).ToList();

        //    differences.AddRange(parameter.Except(dataSources).ToList());


        //    //遍历差
        //    foreach (var differItem in differences)
        //    {
        //        //如果原来的数据存在于差中，就说明是删除项
        //        if (dataSources.Contains(differItem))
        //        {
        //            dataSources.Remove(differItem);
        //        }
        //        //不存在就添加
        //        else
        //        {
        //            dataSources.Add(differItem);
        //        }
        //    }
        //    if (differences.Count < 1)
        //    {
        //        return new List<T>();
        //    }
        //    dataSources = dataSources.Distinct().ToList();
        //    return dataSources;
        //}
        #endregion
    }
}
