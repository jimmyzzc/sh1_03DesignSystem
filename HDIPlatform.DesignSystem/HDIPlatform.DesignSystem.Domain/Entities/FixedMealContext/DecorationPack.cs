using HDIPlatform.DesignSystem.Domain.EventSources.FixedMealContext;
using ShSoft.Common.PoweredByLee;
using ShSoft.Infrastructure.EntityBase;
using ShSoft.Infrastructure.EventBase.Mediator;
using ShSoft.ValueObjects.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using HDIPlatform.DesignSystem.Domain.Entities.ConfigContext;

namespace HDIPlatform.DesignSystem.Domain.Entities.FixedMealContext
{
    /// <summary>
    /// 套餐模板
    /// </summary>
    [Serializable]
    public class DecorationPack : AggregateRootEntity
    {
        #region # 构造器

        #region 01.无参构造器
        /// <summary>
        /// 无参构造器
        /// </summary>
        protected DecorationPack()
        {
            //初始化导航属性
            this.Spaces = new HashSet<DecorationPackSpace>();
            this.PackSeries = new HashSet<PackSeries>();
            //默认值
            this.Sort = 0f;
            this.Status = ShelfStatus.NotShelfed;
            this.Priced = false;
            this.Square = 0f;
            this.BuildingSquare = 0f;
            this.UnitPrice = 0m;
            this.TotalPrice = 0m;
            this.BuildingUnitPrice = 0m;
            this.BuildingTotalPrice = 0m;
            this.Color = DecorationPackColor.Transparent;
            this.HasOffShelvedSku = false;
            this.HasChangedSku = false;
            this.PricingType = null;
            this.Views = 0;
            this.Sales = 0;
            IsClone = false;
            IsBuilding = false;
            this.IsActual = false;
            this.IsUnitActual = false;
            this.IsUnitBuilding = false;
            this.UnitSquare = 0f;
            this.UnitBuildingSquare = 0f;
            this.UnitTotalPrice = 0m;
            this.UnitBuildingTotalPrice = 0m;
            this.AddedTime = DateTime.Now;
            this.IsManageFee = false;
            this.IsWaterElectricityFee = false;
            this.ManageFee = 0m;
            this.WaterElectricityFee = 0m;

        }
        #endregion

        #region 02.创建套餐模板构造器

        /// <summary>
        /// 创建套餐模板构造器
        /// </summary>
        /// <param name="packName">套餐模板名称</param>
        /// <param name="packType">套餐类型</param>
        /// <param name="packKind">套餐类别</param>
        /// <param name="packMode">套餐模式</param>
        /// <param name="newHouse">适用新房</param>
        /// <param name="secondHandHouse">适用二手房</param>
        /// <param name="packSeries">套餐系列</param>
        /// <param name="stores">门店集</param>
        /// <param name="creater">操作人名称</param>
        /// <param name="createrId">操作人Id</param>
        public DecorationPack(string packName, DecorationPackType packType, DecorationPackKind packKind, DecorationPackMode packMode, bool newHouse, bool secondHandHouse,
            IEnumerable<PackSeries> packSeries, Dictionary<Guid, string> stores, string creater, Guid createrId)
            : this()
        {
            base.Name = packName;
            this.PackType = packType;
            this.PackKind = packKind;
            this.PackMode = packMode;
            this.Creater = creater;
            this.CreaterId = createrId;
            this.NewHouse = newHouse;
            this.SecondHandHouse = secondHandHouse;
            this.PackSeries.AddRange(packSeries);
            this.SetStores(stores);
            //初始化关键字
            this.InitKeywords();
        }

        #endregion

        #endregion

        #region # 属性

        #region 创建时间 —— new DateTime AddedTime
        /// <summary>
        /// 创建时间
        /// </summary>
        public new DateTime AddedTime { get; private set; }
        #endregion

        #region 排序 —— new float Sort
        /// <summary>
        /// 排序
        /// </summary>
        public new float Sort { get; private set; }
        #endregion

        #region 套餐状态 —— ShelfStatus Status
        /// <summary>
        /// 套餐状态
        /// </summary>
        public ShelfStatus Status { get; private set; }
        #endregion

        #region 套餐类型 —— DecorationPackType PackType
        /// <summary>
        /// 套餐类型
        /// </summary>
        public DecorationPackType PackType { get; private set; }
        #endregion

        #region 套餐类别 —— DecorationPackKind PackKind
        /// <summary>
        /// 套餐类别
        /// </summary>
        public DecorationPackKind PackKind { get; private set; }
        #endregion

        #region 套餐模式 —— DecorationPackMode PackMode
        /// <summary>
        /// 套餐模式
        /// </summary>
        public DecorationPackMode PackMode { get; private set; }
        #endregion

        #region 套餐颜色 —— DecorationPackColor Color
        /// <summary>
        /// 套餐颜色
        /// </summary>
        public DecorationPackColor Color { get; private set; }
        #endregion

        #region 适用新房 —— bool NewHouse
        /// <summary>
        /// 适用新房
        /// </summary>
        public bool NewHouse { get; private set; }
        #endregion

        #region 适用二手房 —— bool SecondHandHouse
        /// <summary>
        /// 适用二手房
        /// </summary>
        public bool SecondHandHouse { get; private set; }
        #endregion

        #region 是否整体使用面积定价 —— bool IsActual
        /// <summary>
        /// 是否整体使用面积定价
        /// </summary>
        public bool IsActual { get; private set; }
        #endregion

        #region 是否平米使用面积定价 —— bool IsUnitActual
        /// <summary>
        /// 是否平米使用面积定价
        /// </summary>
        public bool IsUnitActual { get; private set; }
        #endregion

        #region 套餐适用的最小使用面积 —— float Square
        /// <summary>
        /// 套餐适用的最小使用面积
        /// </summary>
        public float MinApplicableSquare { get; internal set; }
        #endregion

        #region 套餐适用的最大使用面积—— float MaxApplicableSquare
        /// <summary>
        /// 套餐适用的最大使用面积
        /// </summary>
        public float MaxApplicableSquare { get; internal set; }
        #endregion


        #region 整体定价（使用面积）—— float Square
        /// <summary>
        /// 整体定价（使用面积）
        /// </summary>
        public float Square { get; internal set; }
        #endregion

        #region 平米定价最低购买使用面积—— float UnitSquare
        /// <summary>
        /// 平米定价最低购买使用面积
        /// </summary>
        public float UnitSquare { get; internal set; }
        #endregion

        #region 实际面积（使用面积）单价 —— decimal UnitPrice
        /// <summary>
        /// 实际面积（使用面积）单价
        /// </summary>
        public decimal UnitPrice { get; private set; }
        #endregion

        #region 整体定价：使用面积总价 —— decimal TotalPrice
        /// <summary>
        /// 整体定价：使用面积总价
        /// </summary>
        public decimal TotalPrice { get; private set; }
        #endregion

        #region 平米定价方式： 使用面积总价=最低购买面积*单价 —— decimal UnitTotalPrice
        /// <summary>
        /// 平米定价方式： 使用面积总价=最低购买面积*单价
        /// </summary>
        public decimal UnitTotalPrice { get; private set; }
        #endregion


        #region 是否平米整体建筑定价 —— bool IsUnitBuilding
        /// <summary>
        /// 是否平米整体建筑定价
        /// </summary>
        public bool IsUnitBuilding { get; private set; }
        #endregion

        #region 是否整体建筑定价 —— bool IsBuilding
        /// <summary>
        /// 是否整体建筑定价
        /// </summary>
        public bool IsBuilding { get; private set; }
        #endregion

        #region 整体定价（建筑面积）—— float BuildingSquare
        /// <summary>
        /// 整体定价（建筑面积）
        /// </summary>
        public float BuildingSquare { get; internal set; }
        #endregion

        #region 平米定价最低购买建筑面积—— float UnitBuildingSquare
        /// <summary>
        /// 平米定价最低购买建筑面积
        /// </summary>
        public float UnitBuildingSquare { get; internal set; }
        #endregion

        #region 平米定价方式： 建筑面积总价=最低购买面积*单价 —— decimal UnitBuildingTotalPrice
        /// <summary>
        /// 平米定价方式： 建筑面积总价=最低购买面积*单价
        /// </summary>
        public decimal UnitBuildingTotalPrice { get; private set; }
        #endregion

        #region 建筑面积单价 —— decimal BuildingUnitPrice
        /// <summary>
        /// 建筑面积单价
        /// </summary>
        public decimal BuildingUnitPrice { get; private set; }
        #endregion

        #region 整体定价：建筑面积总价 —— decimal BuildingTotalPrice
        /// <summary>
        /// 整体定价：建筑面积总价
        /// </summary>
        public decimal BuildingTotalPrice { get; private set; }
        #endregion


        #region 超出是否整体定价 —— bool IsWhole
        /// <summary>
        /// 超出是否整体定价
        /// </summary>
        public bool IsWhole { get; private set; }
        #endregion

        #region 整体 超出补单价 —— decimal Premium
        /// <summary>
        /// 整体 超出补单价
        /// </summary>
        public decimal Premium { get; private set; }
        #endregion

        #region 超出定价整体 标准面积 （公式）—— float CriterionSquare
        /// <summary>
        /// 超出定价整体 标准面积（公式）
        /// </summary>
        public float CriterionSquare { get; private set; }
        #endregion

        #region 是否已定价 —— bool Priced
        /// <summary>
        /// 是否已定价
        /// </summary>
        public bool Priced { get; private set; }
        #endregion

        #region 定价类型 —— DecorationPackPricingType? PricingType
        /// <summary>
        /// 定价类型
        /// </summary>
        public DecorationPackPricingType? PricingType { get; private set; }
        #endregion

        #region 是否有下架商品/工艺 —— bool HasOffShelvedSku
        /// <summary>
        /// 是否有下架商品/工艺
        /// </summary>
        public bool HasOffShelvedSku { get; private set; }
        #endregion

        #region 是否有变价商品/工艺 —— bool HasChangedSku
        /// <summary>
        /// 是否有变价商品/工艺
        /// </summary>
        public bool HasChangedSku { get; private set; }
        #endregion

        #region 套餐浏览量 —— int Views
        /// <summary>
        /// 套餐浏览量
        /// </summary>
        public int Views { get; private set; }
        #endregion

        #region 套餐销售量 —— int Sales
        /// <summary>
        /// 套餐销售量
        /// </summary>
        public int Sales { get; private set; }
        #endregion

        #region 创建人 —— string Creater
        /// <summary>
        /// 创建人
        /// </summary>
        public string Creater { get; private set; }
        #endregion

        #region 创建人Id —— Guid CreaterId
        /// <summary>
        /// 创建人Id
        /// </summary>
        public Guid CreaterId { get; private set; }
        #endregion

        #region 源套餐Id —— Guid? SourcePackId
        /// <summary>
        /// 源套餐Id(源套餐一直为空)
        /// </summary>
        public Guid? SourcePackId { get; private set; }
        #endregion

        #region 版本号 —— string VersionNumber
        /// <summary>
        /// 版本号(源套餐一直为空)（套餐名称首拼套餐类型首拼两位流水日期）
        /// </summary>
        public string VersionNumber { get; private set; }
        #endregion

        #region 是否克隆 —— bool IsClone
        /// <summary>
        /// 是否克隆
        /// </summary>
        public bool IsClone { get; private set; }
        #endregion

        #region 产品说明书Id —— string Instructions
        /// <summary>
        /// 产品说明书(文件Id)
        /// </summary>
        public string Instructions { get; private set; }
        #endregion

        #region 产品说明书名称 —— string InstructionsName
        /// <summary>
        /// 产品说明书名称
        /// </summary>
        public string InstructionsName { get; private set; }
        #endregion

        #region 设置管理费参考价 —— bool IsManageFee
        /// <summary>
        /// 设置管理费参考价
        /// </summary>
        public bool IsManageFee { get; private set; }
        #endregion

        #region 设置水电预收参考价 —— bool IsWaterElectricityFee
        /// <summary>
        /// 设置水电预收参考价
        /// </summary>
        public bool IsWaterElectricityFee { get; private set; }
        #endregion

        #region 管理费参考价 —— decimal ManageFee
        /// <summary>
        /// 管理费参考价
        /// </summary>
        public decimal ManageFee { get; private set; }
        #endregion

        #region 水电预收参考价 —— decimal WaterElectricityFee
        /// <summary>
        /// 水电预收参考价
        /// </summary>
        public decimal WaterElectricityFee { get; private set; }
        #endregion


        #region 只读属性 - 空间类型集 —— HashSet<string> SpaceTypes
        /// <summary>
        /// 只读属性 - 空间类型集
        /// </summary>
        public HashSet<string> SpaceTypes
        {
            get
            {
                return string.IsNullOrWhiteSpace(this.SpaceTypesStr)
                    ? new HashSet<string>()
                    : this.SpaceTypesStr.JsonToObject<HashSet<string>>();
            }
        }
        #endregion

        #region 内部属性 - 空间类型集序列化字符串 —— string SpaceTypesStr
        /// <summary>
        /// 内部属性 - 空间类型集序列化字符串
        /// </summary>
        public string SpaceTypesStr { get; private set; }
        #endregion

        #region 只读属性 - 居室集 —— IList<string> Layouts
        /// <summary>
        /// 只读属性 - 居室集
        /// </summary>
        public IList<string> Layouts
        {
            get
            {
                return string.IsNullOrWhiteSpace(this.LayoutsStr)
                    ? new string[0]
                    : this.LayoutsStr.JsonToObject<IList<string>>();
            }
        }
        #endregion

        #region 内部属性 - 居室集序列化字符串 —— string LayoutsStr
        /// <summary>
        /// 内部属性 - 居室集序列化字符串
        /// </summary>
        public string LayoutsStr { get; private set; }
        #endregion

        #region 只读属性 - 设计风格编号集 —— IList<string> StyleNos
        /// <summary>
        /// 只读属性 - 设计风格编号集
        /// </summary>
        public IList<string> StyleNos
        {
            get
            {
                return string.IsNullOrWhiteSpace(this.StyleNosStr)
                    ? new List<string>()
                    : this.StyleNosStr.JsonToObject<Dictionary<string, string>>().Keys.ToList();
            }
        }
        #endregion

        #region 内部属性 - 设计风格集序列化字符串 —— string  StyleNosStr
        /// <summary>
        /// 内部属性 - 设计风格集序列化字符串
        /// </summary>
        public string StyleNosStr { get; private set; }
        #endregion

        //#region 只读属性 - 楼盘Id集 —— IList<Guid> PropertyIds
        ///// <summary>
        ///// 只读属性 - 楼盘ID集
        ///// </summary>
        //public IList<Guid> PropertyIds
        //{
        //    get
        //    {
        //        return string.IsNullOrWhiteSpace(this.PropertysStr)
        //            ? new Guid[0]
        //            : this.PropertysStr.JsonToObject<IList<Guid>>();
        //    }
        //}
        //#endregion

        #region 只读属性 - 楼盘集 —— Dictionary<Guid, string> Propertys
        /// <summary>
        /// 只读属性 - 楼盘集
        /// </summary>
        public Dictionary<Guid, string> Propertys
        {
            get
            {
                return string.IsNullOrWhiteSpace(this.PropertysStr)
                    ? new Dictionary<Guid, string>()
                    : this.PropertysStr.JsonToObject<Dictionary<Guid, string>>();
            }
        }
        #endregion

        #region 内部属性 - 楼盘集序列化字符串 —— string  PropertysStr
        /// <summary>
        /// 内部属性 - 楼盘集(Id,名称)序列化字符串
        /// </summary>
        public string PropertysStr { get; private set; }
        #endregion

        #region 只读属性 - 户型集 ——  ICollection<Tuple<Guid, string, Guid, string, string>> HouseTypes
        /// <summary>
        /// 只读属性 - 户型集(户型Id|户型名称，楼盘Id，楼盘名称,楼盘地址)
        /// </summary>
        public ICollection<Tuple<Guid, string, Guid, string, string>> HouseTypes
        {
            get
            {
                return string.IsNullOrWhiteSpace(this.HouseTypesStr)
                    ? new List<Tuple<Guid, string, Guid, string, string>>()
                    : this.HouseTypesStr.JsonToObject<List<Tuple<Guid, string, Guid, string, string>>>();
            }
        }
        #endregion

        #region 内部属性 - 户型集序列化字符串 —— string  HouseTypesStr
        /// <summary>
        /// 内部属性 - 户型集(户型Id|户型名称，楼盘Id，楼盘名称,楼盘地址)序列化字符串
        /// </summary>
        public string HouseTypesStr { get; private set; }
        #endregion

        #region 只读属性 - 门店集 —— Dictionary<Guid, string> Stores
        /// <summary>
        /// 只读属性 - 门店集
        /// </summary>
        public Dictionary<Guid, string> Stores
        {
            get
            {
                return string.IsNullOrWhiteSpace(this.StoresStr)
                    ? new Dictionary<Guid, string>()
                    : this.StoresStr.JsonToObject<Dictionary<Guid, string>>();
            }
        }
        #endregion

        #region 内部属性 - 门店集序列化字符串 —— string  StoresStr
        /// <summary>
        /// 内部属性 - 门店集(Id,名称)序列化字符串
        /// </summary>
        public string StoresStr { get; private set; }
        #endregion

        #region 只读属性 - 套餐空间集 —— Dictionary<Guid, string> SpaceInfos
        /// <summary>
        /// 只读属性 - 套餐空间集
        /// </summary>
        public Dictionary<Guid, string> SpaceInfos
        {
            get
            {
                return !this.Spaces.Any()
                    ? new Dictionary<Guid, string>()
                    : this.Spaces.OrderBy(x => x.Sort).ToDictionary(x => x.Id, x => x.Name);
            }
        }
        #endregion

        #region 导航属性 - 套餐模板空间集 —— ICollection<DecorationPackSpace> Spaces
        /// <summary>
        /// 导航属性 - 套餐模板空间集
        /// </summary>
        public virtual ICollection<DecorationPackSpace> Spaces { get; private set; }
        #endregion

        #region 导航属性 - 套餐系列集 —— virtual ICollection<PackSeries> PackSeries
        /// <summary>
        /// 导航属性 - 套餐系列集
        /// </summary>
        public virtual ICollection<PackSeries> PackSeries { get; private set; }
        #endregion

        #endregion

        #region # 方法

        #region 修改套餐模板名称 —— void UpdateInfo(string packName)
        /// <summary>
        /// 修改套餐模板名称
        /// </summary>
        /// <param name="packName">套餐模板名称</param>
        public void UpdateInfo(string packName)
        {
            base.Name = packName;
            InitKeywords();
        }
        #endregion

        #region 添加套餐模板空间 —— void AddSpace(DecorationPackSpace space)
        /// <summary>
        /// 添加套餐模板空间
        /// </summary>
        /// <param name="space">套餐模板空间</param>
        public void AddSpace(DecorationPackSpace space)
        {
            this.Spaces.Add(space);

            //设置总面积
            //this.Square = this.Spaces.Sum(x => x.Square);

            //空间类型处理
            string spaceType = space.SpaceType.GetEnumMember();

            HashSet<string> spaceTypes = new HashSet<string>();

            foreach (string type in this.SpaceTypes)
            {
                spaceTypes.Add(type);
            }

            spaceTypes.Add(spaceType);

            this.SpaceTypesStr = spaceTypes.ToJson();
        }
        #endregion

        #region 删除套餐模板空间 —— void RemoveSpace(DecorationPackSpace space)
        /// <summary>
        /// 删除套餐模板空间
        /// </summary>
        /// <param name="space">套餐模板空间</param>
        public void RemoveSpace(DecorationPackSpace space)
        {
            this.Spaces.Remove(space);

            //设置总面积
            //this.Square = this.Spaces.Any() ? this.Spaces.Sum(x => x.Square) : 0;

            //空间类型处理
            if (this.Spaces.All(x => x.SpaceType != space.SpaceType))
            {
                this.SpaceTypes.Remove(space.SpaceType.GetEnumMember());
                this.SpaceTypesStr = this.SpaceTypes.ToJson();
            }

            //挂起领域事件
            EventMediator.Suspend(new PackSpaceDeletedEvent(this.Id, space.Id));
        }
        #endregion

        #region 按平方米定价 —— void SetPriceByUnit(decimal unitPrice, decimal buildingUnitPrice)

        /// <summary>
        /// 按平方米定价
        /// </summary>
        /// <param name="unitPrice">使用面积单价</param>
        /// <param name="buildingUnitPrice">建筑面积定价</param>
        /// <param name="isUnitBuilding">平米建筑</param>
        /// <param name="isUnitActual">平米使用</param>
        /// <param name="unitSquare">平米定价最低购买使用面积</param>
        /// <param name="unitBuildingSquare">平米定价最低购买建筑面积</param>
        /// <param name="isManageFee">设置管理费参考价</param>
        /// <param name="manageFee">管理费参考价</param>
        /// <param name="isWaterElectricityFee">设置水电预收参考价</param>
        /// <param name="waterElectricityFee">水电预收参考价</param>
        public void SetPriceByUnit(decimal unitPrice, decimal buildingUnitPrice, bool isUnitBuilding, bool isUnitActual, float unitSquare, float unitBuildingSquare, bool isManageFee, decimal manageFee, bool isWaterElectricityFee, decimal waterElectricityFee)
        {
            //验证
            //Assert.IsTrue(unitPrice >= 0, "定价不可小于0！");
            //Assert.IsTrue(buildingUnitPrice >= 0, "定价不可小于0！");
            Assert.IsTrue(buildingUnitPrice >= 0 || unitPrice >= 0, "定价不可小于0！使用面积定价与建筑面积定价二者至少选择一个填写！");

            Assert.IsFalse(isManageFee && manageFee <= 0, "管理费参考价请填写大于0的数！");
            Assert.IsFalse(isWaterElectricityFee && waterElectricityFee <= 0, "水电预收参考价请填写大于0的数！");

            this.UnitPrice = unitPrice;
            this.BuildingUnitPrice = buildingUnitPrice;
            //TODO  暂定 套餐整体面积和公式里标准面积一致
            //this.TotalPrice = Convert.ToDecimal(this.Square) * unitPrice;
            this.Priced = true;
            this.PricingType = DecorationPackPricingType.Unit;
            this.IsUnitBuilding = isUnitBuilding;
            this.IsUnitActual = isUnitActual;
            this.UnitSquare = unitSquare;
            this.UnitBuildingSquare = unitBuildingSquare;
            //平米定价方式 总价=最低购买面积*单价
            this.UnitBuildingTotalPrice = Decimal.Round((this.BuildingUnitPrice * Convert.ToDecimal(this.UnitBuildingSquare)), 2);
            this.UnitTotalPrice = Decimal.Round((this.UnitPrice * Convert.ToDecimal(this.UnitSquare)), 2);

            this.IsManageFee = isManageFee;
            this.ManageFee = manageFee;
            this.IsWaterElectricityFee = isWaterElectricityFee;
            this.WaterElectricityFee = waterElectricityFee;

            //清空整体定价
            this.ClearTotalPrice();

            //TODO　领域事件处理套餐商品|工艺成本价 更新
            this.HasChangedSku = false;
            EventMediator.Suspend(new PackPricedEvent(this.Id));
        }
        #endregion

        #region 整体定价 —— void SetPriceTotally(float square, decimal totalPrice...

        /// <summary>
        /// 整体定价
        /// </summary>
        /// <param name="square">标准面积（使用面积）</param>
        /// <param name="totalPrice">总价</param>
        /// <param name="buildingSquare">标准面积（建筑面积）</param>
        /// <param name="buildingTotalPrice">建筑面积总价</param>
        /// <param name="isBuilding">是否整体建筑面积定价</param>
        /// <param name="isActual">是否整体使用面积定价</param>
        /// <param name="isManageFee">设置管理费参考价</param>
        /// <param name="manageFee">管理费参考价</param>
        /// <param name="isWaterElectricityFee">设置水电预收参考价</param>
        /// <param name="waterElectricityFee">水电预收参考价</param>
        public void SetPriceTotally(float square, decimal totalPrice, float buildingSquare, decimal buildingTotalPrice, bool isBuilding, bool isActual, bool isManageFee, decimal manageFee, bool isWaterElectricityFee, decimal waterElectricityFee)
        {
            //验证
            if (isActual)
                Assert.IsTrue(totalPrice >= 0, "总价不可小于0！");
            if (isBuilding)
                Assert.IsTrue(buildingTotalPrice >= 0, "建筑面积总价不可小于0！");
            Assert.IsFalse(isManageFee && manageFee <= 0, "管理费参考价请填写大于0的数！");
            Assert.IsFalse(isWaterElectricityFee && waterElectricityFee <= 0, "水电预收参考价请填写大于0的数！");
            this.Square = square;
            this.TotalPrice = totalPrice;
            this.BuildingSquare = buildingSquare;
            this.BuildingTotalPrice = buildingTotalPrice;
            this.Priced = true;
            this.PricingType = DecorationPackPricingType.Total;
            this.IsBuilding = isBuilding;
            this.IsActual = isActual;
            this.IsManageFee = isManageFee;
            this.ManageFee = manageFee;
            this.IsWaterElectricityFee = isWaterElectricityFee;
            this.WaterElectricityFee = waterElectricityFee;

            //清空平米定价
            this.ClearUnitPrice();

            //TODO　领域事件处理套餐商品|工艺成本价 更新
            this.HasChangedSku = false;
            EventMediator.Suspend(new PackPricedEvent(this.Id));
        }
        #endregion

        #region 定价超出整体补价 —— void SetExceedPrice(float criterionSquare, decimal premium)
        /// <summary>
        /// 定价超出整体补价
        /// </summary>
        /// <param name="criterionSquare">标准面积（公式）</param>
        /// <param name="premium">整体 超出补单价</param>
        public void SetWholeExceedPrice(float criterionSquare, decimal premium)
        {
            Assert.IsTrue(criterionSquare >= 0, "面积不可小于0！");
            Assert.IsTrue(premium >= 0, "超出部分单价不可小于0！");

            this.CriterionSquare = criterionSquare;
            this.Premium = premium;
            this.IsWhole = true;
        }

        #endregion

        #region 修改空间超出补价定价 —— void SetSpacePrice(Guid packSpaceId, DecorationPackSpacePricingType spacePricingType, float criterionSquare, decimal spaceRatio, decimal unitPrice)

        /// <summary>
        /// 修改空间超出补价定价
        /// </summary>
        /// <param name="packSpaceId">空间Id</param>
        /// <param name="spacePricingType">套餐空间定价类型</param>
        /// <param name="criterionSquare">标准面积</param>
        /// <param name="spaceRatio">空间占比</param>
        /// <param name="unitPrice">超出面积单价</param>
        public void SetSpaceExceedPrice(Guid packSpaceId, DecorationPackSpacePricingType spacePricingType, float criterionSquare, decimal spaceRatio, decimal unitPrice)
        {
            DecorationPackSpace space = this.GetPackSpace(packSpaceId);
            space.SetPrice(spacePricingType, criterionSquare, spaceRatio, unitPrice);
            this.IsWhole = false;
        }

        #endregion



        #region 设置排序 —— void SetSort(float sort)
        /// <summary>
        /// 设置排序
        /// </summary>
        /// <param name="sort">排序</param>
        public void SetSort(float sort)
        {
            this.Sort = sort;
        }
        #endregion

        #region 设置套餐颜色 —— void SetColor(DecorationPackColor color)
        /// <summary>
        /// 设置套餐颜色
        /// </summary>
        /// <param name="color">颜色</param>
        public void SetColor(DecorationPackColor color)
        {
            this.Color = color;
        }
        #endregion

        #region 设置居室集 —— void SetLayouts(IEnumerable<string> layouts)
        /// <summary>
        /// 设置居室集
        /// </summary>
        /// <param name="layouts">居室集</param>
        public void SetLayouts(IEnumerable<string> layouts)
        {
            this.LayoutsStr = layouts.IsNullOrEmpty() ? null : layouts.ToJson();
            this.InitKeywords();
        }
        #endregion

        #region 设置设计风格集 —— void SetStyleNos(Dictionary<string, string> styleNos)
        /// <summary>
        /// 设置设计风格集
        /// </summary>
        /// <param name="styleNos">设计风格集</param>
        public void SetStyleNos(Dictionary<string, string> styleNos)
        {
            this.StyleNosStr = styleNos.IsNullOrEmpty() ? null : styleNos.ToJson();
            this.InitKeywords();
        }
        #endregion

        #region 设置楼盘集 —— void SetPropertys(Dictionary<Guid, string> propertys)
        /// <summary>
        /// 设置楼盘集
        /// </summary>
        /// <param name="propertys">楼盘集</param>
        public void SetPropertys(Dictionary<Guid, string> propertys)
        {

            this.PropertysStr = propertys.IsNullOrEmpty() ? null : propertys.ToJson();
            this.InitKeywords();
        }
        #endregion

        #region 设置户型集 —— void SetHouseTypes(IEnumerable<Guid> propertys)
        /// <summary>
        /// 设置户型集——清空再设置
        /// </summary>
        /// <param name="houseTypes">户型集(户型Id|户型名称，楼盘Id，楼盘名称,楼盘地址)</param>
        public void SetHouseTypes(IEnumerable<Tuple<Guid, string, Guid, string, string>> houseTypes)
        {
            this.HouseTypesStr = houseTypes.IsNullOrEmpty() ? null : houseTypes.ToJson();
            //设置楼盘集
            Dictionary<Guid, string> propertys = houseTypes.IsNullOrEmpty() ? null : houseTypes.GroupBy(x => x.Item3).ToDictionary(s => s.Key, s => s.First().Item4);
            this.PropertysStr = propertys.IsNullOrEmpty() ? null : propertys.ToJson();
            this.InitKeywords();
        }
        #endregion

        #region 设置适用范围 —— void SetApplicableScope(bool newHouse ,bool secondHandHouse)

        /// <summary>
        /// 设置适用范围
        /// </summary>
        /// <param name="newHouse">新房</param>
        /// <param name="secondHandHouse">二手房</param>
        public void SetApplicableScope(bool newHouse, bool secondHandHouse)
        {
            this.NewHouse = newHouse;
            this.SecondHandHouse = secondHandHouse;
            this.InitKeywords();
        }
        #endregion

        #region 设置适用范围 —— void SetApplicableSquare(bool newHouse ,bool secondHandHouse)

        /// <summary>
        /// 设置适用范围
        /// </summary>
        /// <param name="minApplicableSquare">套餐适用的最小使用面积</param>
        /// <param name="maxApplicableSquare">套餐适用的最大使用面积</param>
        public void SetApplicableSquare(float minApplicableSquare, float maxApplicableSquare)
        {
            this.MinApplicableSquare = minApplicableSquare;
            this.MaxApplicableSquare = maxApplicableSquare;
        }
        #endregion

        #region 设置套餐系列集 ——  void SetPackSeries(IEnumerable<PackSeries> packSeries)
        /// <summary>
        /// 设置套餐系列集
        /// </summary>
        /// <param name="packSeries">套餐系列集</param>
        public void SetPackSeries(IEnumerable<PackSeries> packSeries)
        {
            this.PackSeries.Clear();
            this.PackSeries.AddRange(packSeries);
            this.InitKeywords();
        }
        #endregion

        #region 设置是否有下架商品/工艺 —— void SetHasOffShelvedSku(bool has)
        /// <summary>
        /// 设置是否有下架商品/工艺
        /// </summary>
        /// <param name="has">是否有下架商品/工艺</param>
        public void SetHasOffShelvedSku(bool has)
        {
            this.HasOffShelvedSku = has;
        }
        #endregion

        #region 设置是否有变价商品/工艺 —— void SetHasChangedSku(bool has)
        /// <summary>
        /// 设置是否有变价商品/工艺
        /// </summary>
        /// <param name="has">是否有变价商品/工艺</param>
        public void SetHasChangedSku(bool has)
        {
            this.HasChangedSku = has;
        }
        #endregion

        #region 设置门店集 —— void SetPropertys(Dictionary<Guid, string> stores)
        /// <summary>
        /// 设置门店集-必填
        /// </summary>
        /// <param name="stores">门店集</param>
        public void SetStores(Dictionary<Guid, string> stores)
        {
            //if (stores.IsNullOrEmpty())
            //{
            //    throw new InvalidOperationException("请设置套餐适用于哪一个门店 ! ");
            //}

            this.StoresStr = stores.IsNullOrEmpty() ? null : stores.ToJson();
            this.InitKeywords();
        }
        #endregion

        #region 上架( 4、套餐必须定价) —— void OnShelf()
        /// <summary>
        /// 上架( 4、套餐必须定价)
        /// </summary>
        public void OnShelf()
        {
            //验证
            if (!this.Priced)
            {
                throw new InvalidOperationException("套餐模板未定价，不可上架！");
            }
            ////验证
            //if (!Spaces.Any())
            //{
            //    throw new InvalidOperationException("套餐模板不存在空间，不可上架！");
            //}

            this.Status = ShelfStatus.Shelfed;
            //TODO　克隆套餐副本
            EventMediator.Suspend(new OnShelfDecorationPackEvent(this.Id));
        }
        #endregion

        #region 下架 —— void OffShelf()
        /// <summary>
        /// 下架
        /// </summary>
        public void OffShelf()
        {
            this.Status = ShelfStatus.ShelfOff; ;
        }
        #endregion

        #region 上架不存版本—— void OnShelfNoClone()
        /// <summary>
        /// 上架不存版本
        /// </summary>
        public void OnShelfNoClone()
        {
            //验证
            if (!this.Priced)
            {
                throw new InvalidOperationException("套餐模板未定价，不可上架！");
            }

            this.Status = ShelfStatus.Shelfed;
        }
        #endregion

        #region 获取套餐模板空间 —— DecorationPackSpace GetPackSpace(Guid packSpaceId)
        /// <summary>
        /// 获取套餐模板空间
        /// </summary>
        /// <param name="packSpaceId">套餐模板空间Id</param>
        /// <returns>套餐模板空间</returns>
        public DecorationPackSpace GetPackSpace(Guid packSpaceId)
        {
            DecorationPackSpace currentPackSpace = this.Spaces.SingleOrDefault(x => x.Id == packSpaceId);

            #region # 验证

            if (currentPackSpace == null)
            {
                throw new ArgumentOutOfRangeException("packSpaceId", string.Format("Id为\"{0}\"的套餐模板空间不存在！", packSpaceId));
            }

            #endregion

            return currentPackSpace;
        }
        #endregion

        #region 设置浏览量 —— void SetViews()
        /// <summary>
        /// 设置浏览量
        /// </summary>
        public void SetViews()
        {
            this.Views++;
        }
        #endregion

        #region 设置销售量 —— void SetSales()
        /// <summary>
        /// 设置销售量
        /// </summary>
        public void SetSales()
        {
            this.Sales++;
        }
        #endregion

        #region 初始化关键字 —— void InitKeywords()
        /// <summary>
        /// 初始化关键字(套餐名称+楼盘名称)
        /// </summary>
        private void InitKeywords()
        {
            StringBuilder builder = new StringBuilder();
            //套餐名称
            builder.Append(base.Name);
            //楼盘集
            //Dictionary<Guid, string> propertys = this.HouseTypes.GroupBy(x => x.Item3).ToDictionary(s => s.Key, s => s.First().Item4);
            //楼盘名称
            if (!string.IsNullOrEmpty(this.PropertysStr))
                builder.Append(this.Propertys.Values.ToJson());
            //户型名称
            if (!string.IsNullOrEmpty(this.HouseTypesStr))
                builder.Append(this.HouseTypes.Select(x => x.Item2).ToJson());
            //套餐|造型
            builder.Append(this.PackKind.GetEnumMember());
            //单空间|空间组|家
            builder.Append(this.PackType.GetEnumMember());
            //基础|成品|基础+成品
            builder.Append(this.PackMode.GetEnumMember());
            //适用范围
            if (this.NewHouse)
                builder.Append("新房");
            if (this.SecondHandHouse)
                builder.Append("二手房");
            //门店名称
            if (!string.IsNullOrEmpty(this.StoresStr))
                builder.Append(this.Stores.Values.ToJson());
            //系列名称
            builder.Append(this.PackSeries.Select(s => s.Name).ToJson());
            //居室
            if (!string.IsNullOrEmpty(this.LayoutsStr))
                builder.Append(this.LayoutsStr);
            //设计风格
            if (!string.IsNullOrEmpty(this.StyleNosStr))
                builder.Append(this.StyleNosStr.JsonToObject<Dictionary<string, string>>().Values.ToJson());


            base.SetKeywords(builder.ToString());
        }
        #endregion

        #region 克隆套餐|复制套餐时设置源套餐Id，版本号 —— void SetSourcePackIdAndVersion(Guid? sourcePackId, string version)

        /// <summary>
        /// 克隆套餐|复制套餐时设置源套餐Id，版本号
        /// </summary>
        /// <param name="sourcePackId">源套餐Id</param>
        /// <param name="version">版本号</param>
        /// <param name="isClone">是否克隆|true克隆|false页面复制套餐</param>
        public void SetSourcePackIdAndVersion(Guid? sourcePackId, string version,bool isClone)
        {
            SourcePackId = sourcePackId;
            VersionNumber = version;
            IsClone = isClone;
            Views = 0;
            Sales = 0;
            this.AddedTime = DateTime.Now;
        }
        #endregion

        #region 设置套餐类型,套餐类别,套餐模式 —— void UpdatePackInfo(DecorationPackType packType, DecorationPackKind packKind, DecorationPackMode packMode, Guid operatorId, string @operator)
        /// <summary>
        ///  设置套餐类型,套餐类别,套餐模式（克隆时修改）
        /// </summary>
        /// <param name="packType">套餐类型</param>
        /// <param name="packKind">套餐类别</param>
        /// <param name="packMode">套餐模式</param>
        /// <param name="operatorId">操作人Id</param>
        /// <param name="operator">操作人名称</param>
        public void UpdatePackInfo(DecorationPackType packType, DecorationPackKind packKind, DecorationPackMode packMode, Guid operatorId, string @operator)
        {
            this.PackType = packType;
            this.PackKind = packKind;
            this.PackMode = packMode;
            this.Creater = @operator;
            this.CreaterId = operatorId;
            this.Status = ShelfStatus.NotShelfed;
        }

        #endregion


        #region  清空 空间超出定价 —— void ClearSpacesPrice()

        /// <summary>
        /// 清空 空间超出定价
        /// </summary>
        public void ClearSpacesPrice()
        {
            List<DecorationPackSpace> spaces = this.Spaces.ToList();
            foreach (DecorationPackSpace space in spaces)
            {
                space.ClearSpacePrice();
            }

        }
        #endregion

        #region 清空整体定价 —— void ClearTotalPrice()
        /// <summary>
        /// 清空整体定价
        /// </summary>
        public void ClearTotalPrice()
        {
            this.IsBuilding = false;
            this.IsActual = false;
            this.BuildingSquare = 0f;
            this.BuildingTotalPrice = 0m;
            this.TotalPrice = 0m;
            this.Square = 0f;
        }
        #endregion

        #region 清空平米定价 —— void ClearUnitPrice()
        /// <summary>
        /// 清空平米定价
        /// </summary>
        public void ClearUnitPrice()
        {
            this.IsUnitBuilding = false;
            this.IsUnitActual = false;
            this.UnitPrice = 0m;
            this.BuildingUnitPrice = 0m;
            this.UnitSquare = 0f;
            this.UnitBuildingSquare = 0f;
            this.UnitBuildingTotalPrice = 0m;
            this.UnitTotalPrice = 0m;
        }

        #endregion

        #region 上传产品说明书 —— void SetInstructions(string instructions, string instructionsName)
        /// <summary>
        /// 上传产品说明书
        /// </summary>
        /// <param name="instructions">产品说明书Id</param>
        /// <param name="instructionsName">产品说明书名称</param>
        public void SetInstructions(string instructions, string instructionsName)
        {
            this.Instructions = instructions;
            this.InstructionsName = instructionsName;

        }

        #endregion

        #endregion
    }
}
