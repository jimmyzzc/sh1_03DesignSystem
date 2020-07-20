using HDIPlatform.DesignSystem.IAppService.DTOs.Inputs;
using HDIPlatform.DesignSystem.IAppService.DTOs.Outputs.FixedMealContext;
using ShSoft.Infrastructure;
using ShSoft.Infrastructure.DTOBase;
using ShSoft.ValueObjects.Enums;
using System;
using System.Collections.Generic;
using System.ServiceModel;

namespace HDIPlatform.DesignSystem.IAppService.Interfaces
{
    /// <summary>
    /// 套餐模板服务契约接口
    /// </summary>
    [ServiceContract(Namespace = "http://HDIPlatform.DesignSystem.IAppService.Interfaces")]
    public interface IFixedMealContract : IApplicationService
    {
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
        [OperationContract]
        Guid CreateDecorationPack(string packName, DecorationPackType packType, DecorationPackKind packKind, DecorationPackMode packMode, bool newHouse,
            bool secondHandHouse, IEnumerable<Guid> packSeriesIds, Dictionary<Guid, string> stores, Guid operatorId, string @operator);
        #endregion

        #region # 修改套餐模板 —— void UpdateDecorationPack(Guid packId...
        /// <summary>
        /// 修改套餐模板
        /// </summary>
        /// <param name="packId">套餐模板Id</param>
        /// <param name="packName">套餐模板名称</param>
        [OperationContract]
        void UpdateDecorationPack(Guid packId, string packName);
        #endregion

        #region # 设置套餐模板排序 —— void SetDecorationPackSort(Guid packId, int sort)
        /// <summary>
        /// 设置套餐模板排序
        /// </summary>
        /// <param name="packId">套餐模板Id</param>
        /// <param name="sort">排序</param>
        [OperationContract]
        void SetDecorationPackSort(Guid packId, int sort);
        #endregion

        #region # 删除套餐模板 —— void RemoveDecorationPack(Guid packId)
        /// <summary>
        /// 删除套餐模板
        /// </summary>
        /// <param name="packId">套餐模板Id</param>
        [OperationContract]
        void RemoveDecorationPack(Guid packId);
        #endregion

        #region # 保存套餐模板方案空间集 —— void SaveDecorationPackSpaces(Guid packId...
        /// <summary>
        /// 保存套餐模板方案空间集
        /// </summary>
        /// <param name="packId">套餐模板Id</param>
        /// <param name="packSpaceParams">套餐模板方案空间参数模型集</param>
        [OperationContract]
        void SaveDecorationPackSpaces(Guid packId, IEnumerable<PackSpaceParam> packSpaceParams);
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
        [OperationContract]
        Guid CreatePackItem(string packItemName, Guid packId, Guid packSpaceId, CategoryArea categoryArea, IEnumerable<Guid> categoryIds);
        #endregion

        #region # 批量修改套餐模板项 —— void UpdatePackItems(IDictionary<Guid,PackItemParam> itemParams)
        /// <summary>
        /// 批量修改套餐模板项
        /// </summary>
        /// <param name="itemParams">套餐模板项参数模型字典</param>
        [OperationContract]
        void UpdatePackItems(IDictionary<Guid, PackItemParam> itemParams);
        #endregion

        #region # 添加商品SKU集至套餐模板项 —— void AddSkuItems(Guid packItemId, IDictionary<Guid, int>...

        /// <summary>
        /// 添加商品SKU集至套餐模板项
        /// </summary>
        /// <param name="packItemId">套餐模板项Id</param>
        /// <param name="skuItems">商品SKU字典</param>
        [OperationContract]
        void AddSkuItems(Guid packItemId, IEnumerable<DecorationPackSkuParam> skuItems);
        #endregion

        #region # 添加工艺实体集至套餐模板项 —— void AddCraftItems(Guid packItemId, IDictionary<Guid, int>...
        /// <summary>
        /// 添加工艺实体集至套餐模板项
        /// </summary>
        /// <param name="packItemId">套餐模板项Id</param>
        /// <param name="craftEntityItems">工艺实体字典</param>
        [OperationContract]
        void AddCraftItems(Guid packItemId, IEnumerable<DecorationPackCraftParam> craftEntityItems);
        #endregion

        #region # 设置选区内默认商品的工程量 ——  void SetDefaultSkuQuantity(Guid packItemId, Guid defaultSkuId, decimal skuQuantity, Dictionary<SkuCraftPosition, decimal> skuCraftPositions)
        /// <summary>
        /// 设置选区内默认商品的工程量
        /// </summary>
        /// <param name="packItemId">选区Id</param>
        /// <param name="defaultSkuId">默认SkuId</param>
        /// <param name="skuQuantity">默认Sku工程量</param>
        /// <param name="skuCraftPositions">放置位置</param>
        [OperationContract]
        void SetDefaultSkuQuantity(Guid packItemId, Guid defaultSkuId, decimal skuQuantity, Dictionary<SkuCraftPosition, decimal> skuCraftPositions);
        #endregion

        #region # 设置选区内默认工艺的工程量 ——  void SetDefaultCraftQuantity(Guid packItemId, Guid defaultCraftId, decimal craftQuantity, Dictionary<SkuCraftPosition, decimal> skuCraftPositions)
        /// <summary>
        /// 设置选区内默认工艺的工程量
        /// </summary>
        /// <param name="packItemId">选区Id</param>
        /// <param name="defaultCraftId">默认CraftId</param>
        /// <param name="craftQuantity">默认工艺工程量</param>
        /// <param name="skuCraftPositions">放置位置</param>
        [OperationContract]
        void SetDefaultCraftQuantity(Guid packItemId, Guid defaultCraftId, decimal craftQuantity, Dictionary<SkuCraftPosition, decimal> skuCraftPositions);
        #endregion

        #region # 克隆套餐模板项 —— void ClonePackItem(Guid packItemId, IDictionary<Guid, string>...
        /// <summary>
        /// 克隆套餐模板项
        /// </summary>
        /// <param name="packItemId">套餐模板项Id</param>
        /// <param name="packSpaceIdNames">套餐模板空间Id/套餐模板项名称字典</param>
        [OperationContract]
        void ClonePackItem(Guid packItemId, IDictionary<Guid, string> packSpaceIdNames);
        #endregion

        #region # 删除套餐模板项 —— void RemovePackItem(Guid packItemId)
        /// <summary>
        /// 删除套餐模板项
        /// </summary>
        /// <param name="packItemId">套餐模板项Id</param>
        [OperationContract]
        void RemovePackItem(Guid packItemId);
        #endregion

        #region # 按平方米定价 —— void SetPriceByUnit(Guid packId, decimal unitPrice...

        /// <summary>
        /// 按平方米定价
        /// </summary>
        /// <param name="packId">套餐模板Id</param>
        /// <param name="unitPrice">单价</param>
        /// <param name="buildingUnitPrice">按建筑面积定价</param>
        /// <param name="isUnitBuilding">平米建筑</param>
        /// <param name="isUnitActual">平米使用</param>
        /// <param name="unitSquare">平米定价最低购买使用面积</param>
        /// <param name="unitBuildingSquare">平米定价最低购买建筑面积</param>
        /// <param name="premium">整体超出部分单价</param>
        /// <param name="spacePricingParams">空间定价参数集</param>
        /// <param name="isWhole">是否整体定价超出</param>
        /// <param name="criterionSquare">标准面积（公式）</param>
        /// <param name="minApplicableSquare">套餐适用的最小使用面积</param>
        /// <param name="maxApplicableSquare">套餐适用的最大使用面积</param>
        /// <param name="isManageFee">设置管理费参考价</param>
        /// <param name="manageFee">管理费参考价</param>
        /// <param name="isWaterElectricityFee">设置水电预收参考价</param>
        /// <param name="waterElectricityFee">水电预收参考价</param>
        [OperationContract]
        void SetPriceByUnit(Guid packId, decimal unitPrice, decimal buildingUnitPrice, bool isUnitBuilding, bool isUnitActual, float unitSquare, float unitBuildingSquare, bool isWhole, float criterionSquare, decimal premium, float minApplicableSquare, float maxApplicableSquare, bool isManageFee, decimal manageFee, bool isWaterElectricityFee, decimal waterElectricityFee, IEnumerable<PackSchemeSpacePricingParam> spacePricingParams);
        #endregion

        #region # 整体定价 —— void SetPriceTotally(Guid packId, float square...

        /// <summary>
        /// 整体定价
        /// </summary>
        /// <param name="packId">套餐模板Id</param>
        /// <param name="isActual">是否整体使用面积定价</param>
        /// <param name="isWhole">是否整体定价超出</param>
        /// <param name="criterionSquare">标准面积（公式）</param>
        /// <param name="totalPrice">总价</param>
        /// <param name="unitPrice">超出部分单价</param>
        /// <param name="buildingTotalPrice"></param>
        /// <param name="spacePricingParams">空间定价参数集</param>
        /// <param name="buildingSquare">标准面积（建筑面积）</param>
        /// <param name="square">标准面积（使用面积）</param>
        /// <param name="isBuilding">是否整体建筑面积定价</param>
        /// <param name="minApplicableSquare">套餐适用的最小使用面积</param>
        /// <param name="maxApplicableSquare">套餐适用的最大使用面积</param>
        /// <param name="isManageFee">设置管理费参考价</param>
        /// <param name="manageFee">管理费参考价</param>
        /// <param name="isWaterElectricityFee">设置水电预收参考价</param>
        /// <param name="waterElectricityFee">水电预收参考价</param>
        [OperationContract]
        void SetPriceTotally(Guid packId, decimal totalPrice, decimal buildingTotalPrice, float square, float buildingSquare, bool isBuilding, bool isActual, bool isWhole, float criterionSquare, decimal unitPrice, float minApplicableSquare, float maxApplicableSquare, bool isManageFee, decimal manageFee, bool isWaterElectricityFee, decimal waterElectricityFee, IEnumerable<PackSchemeSpacePricingParam> spacePricingParams);
        #endregion

        #region # 上架套餐模板 —— void OnShelf(Guid packId)
        /// <summary>
        /// 上架套餐模板
        /// </summary>
        /// <param name="packId">套餐模板Id</param>
        [OperationContract]
        void OnShelf(Guid packId);
        #endregion

        #region # 下架套餐模板 —— void OffShelf(Guid packId)
        /// <summary>
        /// 下架套餐模板
        /// </summary>
        /// <param name="packId">套餐模板Id</param>
        [OperationContract]
        void OffShelf(Guid packId);
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
        [OperationContract]
        Guid CloneDecorationPack(Guid sourcePackId, string packName, DecorationPackType packType, DecorationPackKind packKind, DecorationPackMode packMode, bool newHouse,
            bool secondHandHouse, IEnumerable<Guid> packSeriesIds, Dictionary<Guid, string> stores, Guid operatorId, string @operator);
        #endregion

        #region # 创建套餐模板方案 —— void CreatePackScheme(Guid packId, string schemeName...
        /// <summary>
        /// 创建套餐模板方案
        /// </summary>
        /// <param name="packId">套餐模板Id</param>
        /// <param name="schemeName">套餐模板方案名称</param>
        /// <param name="cover">封面</param>
        /// <param name="description">封面描述</param>
        /// <param name="images">方案概况图片集</param>
        /// <param name="schemeDescriptions">方案概况描述</param>
        /// <param name="videoAudioLink">视频音频地址链接</param>
        /// <param name="videoAudioFileName">视频音频文件名称</param>
        /// <param name="videoAudioFileSize">视频音频文件大小</param>
        /// <param name="videoAudioFileId">视频音频文件Id</param>
        /// <param name="videoAudiogFileSuffix">视频音频文件扩展名</param>
        /// <param name="schemeSpaceParams">套餐模板方案空间参数模型集</param>
        [OperationContract]
        void CreatePackScheme(Guid packId, string schemeName, string cover, string description, IList<string> images, IList<string> schemeDescriptions, string videoAudioLink, string videoAudioFileSize, string videoAudioFileId, string videoAudiogFileSuffix, string videoAudioFileName, IEnumerable<PackSchemeSpaceParam> schemeSpaceParams);
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
        [OperationContract]
        void UpdatePackScheme(Guid packSchemeId, string schemeName, string cover, string description, IList<string> images, IList<string> schemeDescriptions, string videoAudioLink, string videoAudioFileSize, string videoAudioFileId, string videoAudiogFileSuffix, string videoAudioFileName, IEnumerable<PackSchemeSpaceParam> schemeSpaceParams);
        #endregion

        #region # 删除套餐模板方案 —— void RemovePackScheme(Guid packSchemeId)
        /// <summary>
        /// 删除套餐模板方案
        /// </summary>
        /// <param name="packSchemeId">套餐模板方案Id</param>
        [OperationContract]
        void RemovePackScheme(Guid packSchemeId);
        #endregion

        #region # 设置套餐模板选购封面(设置默认方案) —— void SetCover(Guid packId, Guid packSchemeId)

        /// <summary>
        /// 设置套餐模板选购封面(设置默认方案)
        /// </summary>
        /// <param name="packId">套餐模板Id</param>
        /// <param name="packSchemeId">方案Id</param>
        [OperationContract]
        void SetCover(Guid packId, Guid packSchemeId);
        #endregion

        #region # 保存套餐模板标签 —— void SavePackLabels(Guid packId, DecorationPackColor color...

        /// <summary>
        /// 保存套餐模板标签
        /// </summary>
        /// <param name="packId">套餐模板Id</param>
        /// <param name="color">颜色</param>
        /// <param name="layouts">居室集</param>
        /// <param name="styleNos">设计风格集（编号|名称）</param>
        /// <param name="houseTypes">户型集(户型Id|户型名称，楼盘Id，楼盘名称,楼盘地址)</param>
        /// <param name="newHouse">适用新房</param>
        /// <param name="secondHandHouse">适用二手房</param>
        /// <param name="packSeriesIds">套餐系列Id集</param>
        /// <param name="stores">门店集</param>
        [OperationContract]
        void SavePackLabels(Guid packId, DecorationPackColor color, IEnumerable<string> layouts, Dictionary<string, string> styleNos,
            IEnumerable<Tuple<Guid, string, Guid, string, string>> houseTypes, bool newHouse, bool secondHandHouse, IEnumerable<Guid> packSeriesIds, Dictionary<Guid, string> stores);
        #endregion

        #region # 替换商品SKU —— void ReplaceSku(Guid sourceSkuId, Guid targetSkuId...

        /// <summary>
        /// 替换商品SKU
        /// </summary>
        /// <param name="sourceSkuId">源商品SKU Id</param>
        /// <param name="targetSkuId">模板商品SKU Id</param>
        /// <param name="costPrice">新商品成本价</param>
        /// <param name="packIds">套餐模板Id集</param>
        [OperationContract]
        void ReplaceSku(Guid sourceSkuId, Guid targetSkuId, decimal costPrice, IEnumerable<Guid> packIds);
        #endregion

        #region # 删除商品SKU —— void RemoveSku(Guid sourceSkuId...
        /// <summary>
        /// 删除商品SKU
        /// </summary>
        /// <param name="sourceSkuId">源商品SKU Id</param>
        /// <param name="packIds">套餐模板Id集</param>
        [OperationContract]
        void RemoveSku(Guid sourceSkuId, IEnumerable<Guid> packIds);
        #endregion

        #region # 批量删除商品SKU —— void RemoveAllSku(Dictionary<Guid, List<Guid>> skuPackIds

        /// <summary>
        /// 批量删除商品SKU
        /// </summary>
        /// <param name="skuPackIds">商品SKU Id|套餐模板Id集</param>
        [OperationContract]
        void RemoveAllSku(Dictionary<Guid, List<Guid>> skuPackIds);
        #endregion

        #region # 删除工艺实体 —— void RemoveCraft(Guid sourceCraftEntityId...
        /// <summary>
        /// 删除工艺实体
        /// </summary>
        /// <param name="sourceCraftEntityId">源工艺实体Id</param>
        /// <param name="packIds">套餐模板Id集</param>
        [OperationContract]
        void RemoveCraft(Guid sourceCraftEntityId, IEnumerable<Guid> packIds);
        #endregion

        #region # 批量删除工艺实体 —— void RemoveAllCraft(Guid sourceCraftEntityId...

        /// <summary>
        /// 批量删除工艺实体
        /// </summary>
        /// <param name="craftPackIds">工艺实体Id|套餐模板Id集</param>
        [OperationContract]
        void RemoveAllCraft(Dictionary<Guid, List<Guid>> craftPackIds);
        #endregion

        #region # 替换工艺实体 —— void ReplaceCraft(Guid sourceCraftEntityId, Guid targetCraftEntityId...

        /// <summary>
        /// 替换工艺实体
        /// </summary>
        /// <param name="sourceCraftEntityId">源工艺实体Id</param>
        /// <param name="targetCraftEntityId">模板工艺实体Id</param>
        /// <param name="costPrice">新工艺成本价</param>
        /// <param name="packIds">套餐模板Id集</param>
        [OperationContract]
        void ReplaceCraft(Guid sourceCraftEntityId, Guid targetCraftEntityId, decimal costPrice, IEnumerable<Guid> packIds);
        #endregion

        #region # 关联大包/定制模板 —— void RelateBalePack(Guid decorationPackId, Guid balePackId)
        /// <summary>
        /// 关联大包/定制模板
        /// </summary>
        /// <param name="decorationPackId">套餐模板Id</param>
        /// <param name="balePackId">大包/定制模板Id</param>
        [OperationContract]
        void RelateBalePack(Guid decorationPackId, Guid balePackId);
        #endregion

        #region # 取消关联大包/定制模板 —— void CancelRelateBalePack(Guid decorationPackId...
        /// <summary>
        /// 取消关联大包/定制模板
        /// </summary>
        /// <param name="decorationPackId">套餐模板Id</param>
        /// <param name="balePackId">大包/定制模板Id</param>
        [OperationContract]
        void CancelRelateBalePack(Guid decorationPackId, Guid balePackId);
        #endregion

        #region # 设置套餐浏览量 —— void SetPackViews()
        /// <summary>
        /// 设置套餐浏览量
        /// </summary>
        [OperationContract]
        void SetPackViews(Guid packId);
        #endregion

        #region # 设置套餐销售量 —— void SetPackSales()
        /// <summary>
        /// 设置套餐销售量
        /// </summary>
        [OperationContract]
        void SetPackSales(Guid packId);
        #endregion

        #region # 添加套餐项三级品类范围（原三级品类集累加） —— void AddPackItemCategoryIds(Guid packItemId, IEnumerable<Guid> categoryIds)
        /// <summary>
        /// 添加套餐项三级品类范围（原三级品类集累加）
        /// </summary>
        /// <param name="packItemId">套餐项ID</param>
        /// <param name="categoryIds">三级品类集</param>
        [OperationContract]
        void AddPackItemCategoryIds(Guid packItemId, IEnumerable<Guid> categoryIds);
        #endregion

        #region # 删除套餐项三级品类配置 —— void DeletePackItemCategoryIds(Guid packItemId, Guid categoryId)
        /// <summary>
        /// 删除套餐项三级品类配置
        /// </summary>
        /// <param name="packItemId">套餐项ID</param>
        /// <param name="categoryId">三级品类</param>
        [OperationContract]
        void DeletePackItemCategoryIds(Guid packItemId, Guid categoryId);
        #endregion

        #region # 修改套餐模板项 —— void UpdatePackItem(Guid itemId, PackItemParam itemParam, IEnumerable<Guid> categoryIds)

        /// <summary>
        /// 修改套餐模板项
        /// </summary>
        /// <param name="itemId">模板项Id</param>
        /// <param name="itemParam">套餐模板项参数模型字典</param>
        /// <param name="categoryIds">品类集</param>
        [OperationContract]
        void UpdatePackItem(Guid itemId, PackItemParam itemParam, IEnumerable<Guid> categoryIds);
        #endregion

        #region # 上传产品说明书 —— void SetInstructions(Guid packId, string instructions, string instructionsName)
        /// <summary>
        /// 上传产品说明书
        /// </summary>
        /// <param name="packId">套餐Id</param>
        /// <param name="instructions">文件Id</param>
        /// <param name="instructionsName">产品说明书名称</param>
        [OperationContract]
        void SetInstructions(Guid packId, string instructions, string instructionsName);
        #endregion

        #region # 修改套餐模板项设计要求 —— void SetRequirement(Guid packItemId,string requirement)
        /// <summary>
        /// 修改套餐模板项设计要求
        /// </summary>
        /// <param name="packItemId">套餐项ID</param>
        /// <param name="requirement">设计要求</param>
        [OperationContract]
        void SetRequirement(Guid packItemId, string requirement);
        #endregion

        #region # 变价套餐模板商品SKU —— void ChangedDecorationPackSku(IDictionary<Guid, decimal> skuIdCostPrices)

        /// <summary>
        /// 变价套餐模板商品SKU
        /// </summary>
        /// <param name="skuIdCostPrices">商品SKU Id|商品工程量成本价</param>
        [OperationContract]
        void ChangedDecorationPackSku(IDictionary<Guid, decimal> skuIdCostPrices);
        #endregion

        #region # 变价套餐模板工艺 —— void ChangedDecorationPackCraft(IDictionary<Guid, decimal> craftEntityIdCostPrices)

        /// <summary>
        /// 变价套餐模板工艺      
        /// </summary>
        /// <param name="craftEntityIdCostPrices">工艺实体Id|工艺成本价</param>
        [OperationContract]
        void ChangedDecorationPackCraft(IDictionary<Guid, decimal> craftEntityIdCostPrices);
        #endregion


        #region # 导入套餐空间预算(DIM) ——  void ImportDecorationPack(Guid packId,IEnumerable<PackSpaceParam> packSpaceParams)

        /// <summary>
        /// 导入套餐空间预算(DIM)
        /// </summary>
        /// <param name="packId">套餐模板Id</param>
        /// <param name="packSpaceParams">套餐空间参数集</param>
        [OperationContract]
        void ImportDecorationPack(Guid packId, IEnumerable<PackSpaceParam> packSpaceParams);
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
        /// <param name="hasChangedSku">是否有变价商品|工艺</param>
        /// <param name="color">颜色</param>
        /// <param name="packType">套餐类型</param>
        /// <param name="packKind">套餐类别</param>
        /// <param name="packMode">套餐模式</param>
        /// <param name="status">状态</param>
        /// <param name="styleNos">设计风格集</param>
        /// <param name="spaceTypes">空间类型集</param>
        /// <param name="layouts">居室集</param>
        /// <param name="propertys">楼盘集</param>
        /// <param name="packSeriesIds">套餐系列集</param>
        /// <param name="stores">门店集</param>
        /// <param name="hasStores">是否有门店（全部,标签:true,其他:false）</param>
        /// <param name="createrId">套餐创建人Id？（全部：null,我的：当前登录人员）</param>
        /// <param name="sort">排序条件 true为倒序</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页容量</param>
        /// <returns>套餐模板列表</returns>
        [OperationContract]
        PageModel<DecorationPackInfo> GetDecorationPacks(string keywords, Guid? propertyId, float? applicableSquare, bool? isNewHouse, bool isBuildingPrice, decimal? minPrice, decimal? maxPrice, bool isBuildingSquare,
            float? minSquare, float? maxSquare, bool? hasOffSku, bool? hasChangedSku, DecorationPackColor? color, DecorationPackType? packType,
            DecorationPackKind? packKind, DecorationPackMode? packMode, ShelfStatus? status, IList<string> spaceTypes, IList<string> styleNos, IList<string> layouts,
            IList<Guid> propertys, IList<Guid> packSeriesIds, IList<Guid> stores, bool hasStores, Guid? createrId,Dictionary<string, bool> sort, int pageIndex, int pageSize);
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
        /// <param name="hasStores">是否有门店（全部,标签:true,其他:false）</param>
        /// <param name="sort">排序条件 true为倒序</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页容量</param>
        /// <returns>套餐模板列表</returns>
        [OperationContract]
        PageModel<DecorationPackInfo> GetDecorationPacksForSales(string keywords, Guid? propertyId, float? applicableSquare, bool? isNewHouse,
            bool? isBuildingPrice, decimal? minPrice, decimal? maxPrice, bool? isBuildingSquare, float? minSquare, float? maxSquare, DecorationPackType? packType, DecorationPackKind? packKind,
            DecorationPackMode? packMode, IList<string> spaceTypes, IList<string> styleNos, IList<string> layouts, bool hasPackSeries, IList<Guid> packSeriesIds, IList<Guid> stores, bool hasStores, 
            Dictionary<string, bool> sort, int pageIndex, int pageSize);

        #endregion

        #region # 获取套餐模板空间列表 —— IEnumerable<DecorationPackSpaceInfo> GetPackSpaces(Guid packId)
        /// <summary>
        /// 获取套餐模板空间列表
        /// </summary>
        /// <param name="packId">套餐模板Id</param>
        /// <returns>套餐模板空间列表</returns>
        [OperationContract]
        IEnumerable<DecorationPackSpaceInfo> GetPackSpaces(Guid packId);
        #endregion

        #region # 获取套餐模板项列表 —— IEnumerable<DecorationPackItemInfo> GetPackItems(Guid packId...
        /// <summary>
        /// 获取套餐模板项列表
        /// </summary>
        /// <param name="packId">套餐模板Id</param>
        /// <param name="packSpaceId">套餐模板空间Id</param>
        /// <returns>套餐模板项列表</returns>
        [OperationContract]
        IEnumerable<DecorationPackItemInfo> GetPackItems(Guid packId, Guid? packSpaceId);
        #endregion

        #region # 获取套餐模板项 ——  DecorationPackItemInfo GetDecorationPackItemById(Guid packItemId)
        /// <summary>
        /// 获取套餐模板项
        /// </summary>
        /// <param name="packItemId">套餐模板项Id</param>
        /// <returns></returns>
        [OperationContract]
        DecorationPackItemInfo GetDecorationPackItemById(Guid packItemId);
        #endregion

        #region # 获取套餐模板商品SKU项列表 —— IEnumerable<DecorationPackSkuInfo> GetPackSkus(...
        /// <summary>
        /// 获取套餐模板商品SKU项列表
        /// </summary>
        /// <param name="packItemId">套餐模板项Id</param>
        /// <returns>套餐模板商品SKU项列表</returns>
        [OperationContract]
        IEnumerable<DecorationPackSkuInfo> GetPackSkus(Guid packItemId);
        #endregion

        #region # 批量获取套餐模板商品SKU项列表 —— IDictionary<Guid, IEnumerable<DecorationPackSkuInfo>> GetBulkPackSkus(...

        /// <summary>
        /// 批量获取套餐模板商品SKU项列表
        /// </summary>
        /// <param name="packId">套餐Id</param>
        /// <param name="packItemIds">套餐模板项Id集</param>
        /// <returns>套餐模板商品SKU项字典</returns>
        /// IDictionary[Guid, IEnumerable[DecorationPackSkuInfo]]，[套餐模板项Id，套餐模板商品SKU项列表]
        [OperationContract]
        IDictionary<Guid, IEnumerable<DecorationPackSkuInfo>> GetBulkPackSkus(Guid packId, IEnumerable<Guid> packItemIds);
        #endregion

        #region # 批量获取套餐商品SKU项列表 —— IDictionary<Guid, IEnumerable<DecorationPackSkuInfo>> GetBulkPackSkusByPackId(...
        /// <summary>
        /// 批量获取套餐商品SKU项列表
        /// </summary>
        /// <param name="packId">套餐Id</param>
        /// <returns>套餐商品SKU项字典</returns>
        /// IDictionary[Guid, IEnumerable[DecorationPackSkuInfo]]，[套餐模板项Id，套餐模板商品SKU项列表]
        [OperationContract]
        IDictionary<Guid, IEnumerable<DecorationPackSkuInfo>> GetBulkPackSkusByPackId(Guid packId);
        #endregion

        #region # 获取套餐模板工艺项列表 —— IEnumerable<DecorationPackCraftInfo> GetPackCrafts(...
        /// <summary>
        /// 获取套餐模板工艺项列表
        /// </summary>
        /// <param name="packItemId">套餐模板项Id</param>
        /// <returns>套餐模板工艺项列表</returns>
        [OperationContract]
        IEnumerable<DecorationPackCraftInfo> GetPackCrafts(Guid packItemId);
        #endregion

        #region # 批量获取套餐模板工艺项列表 —— IDictionary<Guid, IEnumerable<DecorationPackCraftInfo>> GetBulkPackCrafts(...

        /// <summary>
        /// 批量获取套餐模板工艺项列表
        /// </summary>
        /// <param name="packId">套餐Id</param>
        /// <param name="packItemIds">套餐模板项Id</param>
        /// <returns>套餐模板工艺项字典</returns>
        /// IDictionary[Guid, IEnumerable[DecorationPackCraftInfo]]，[套餐模板项Id，套餐模板工艺项列表]
        [OperationContract]
        IDictionary<Guid, IEnumerable<DecorationPackCraftInfo>> GetBulkPackCrafts(Guid packId, IEnumerable<Guid> packItemIds);
        #endregion

        #region # 批量获取套餐工艺项列表 —— IDictionary<Guid, IEnumerable<DecorationPackCraftInfo>> GetBulkPackCraftsByPackId(...
        /// <summary>
        /// 批量获取套餐工艺项列表
        /// </summary>
        /// <param name="packId">套餐Id</param>
        /// <returns>套餐工艺项字典</returns>
        /// IDictionary[Guid, IEnumerable[DecorationPackCraftInfo]]，[套餐模板项Id，套餐模板工艺项列表]
        [OperationContract]
        IDictionary<Guid, IEnumerable<DecorationPackCraftInfo>> GetBulkPackCraftsByPackId(Guid packId);
        #endregion

        #region # 获取套餐模板方案列表 —— IEnumerable<DecorationPackSchemeInfo> GetPackSchemes(Guid packId)
        /// <summary>
        /// 获取套餐模板方案列表
        /// </summary>
        /// <param name="packId">套餐模板Id</param>
        /// <returns>套餐模板方案列表</returns>
        [OperationContract]
        IEnumerable<DecorationPackSchemeInfo> GetPackSchemes(Guid packId);
        #endregion

        #region # 获取套餐模板方案空间列表 —— IEnumerable<DecorationPackSchemeSpaceInfo> GetPackSchemeSpaces(...
        /// <summary>
        /// 获取套餐模板方案空间列表
        /// </summary>
        /// <param name="packSchemeId">套餐模板方案Id</param>
        /// <returns>方案空间列表</returns>
        [OperationContract]
        IEnumerable<DecorationPackSchemeSpaceInfo> GetPackSchemeSpaces(Guid packSchemeId);
        #endregion

        #region # 获取套餐模板内下架商品数量 —— IDictionary<Guid, int> GetOffShelvedCount(IEnumerable<Guid> packIds)
        /// <summary>
        /// 获取套餐模板内下架商品数量
        /// </summary>
        /// <param name="packIds">套餐模板Id集</param>
        /// <returns>下架商品数量字典</returns>
        /// <remarks>IDictionary[Guid, int]，[套餐模板Id, 下架商品数量]</remarks>
        [OperationContract]
        IDictionary<Guid, int> GetOffShelvedCount(IEnumerable<Guid> packIds);
        #endregion

        #region # 获取总下架商品数量 —— int GetTotalOffShelvedCount(Guid? createrId)
        /// <summary>
        /// 获取总下架商品数量
        /// </summary>
        /// <param name="createrId">套餐创建人Id？（全部：null,我的：当前登录人员）</param>
        /// <returns>总下架商品数量</returns>
        [OperationContract]
        int GetTotalOffShelvedCount(Guid? createrId);
        #endregion

        #region # 获取套餐模板 —— DecorationPackInfo GetDecorationPack(Guid packId)
        /// <summary>
        /// 获取套餐模板
        /// </summary>
        /// <param name="packId">套餐模板Id</param>
        /// <returns>套餐模板</returns>
        [OperationContract]
        DecorationPackInfo GetDecorationPack(Guid packId);
        #endregion

        #region # 获取套餐模板方案 —— DecorationPackSchemeInfo GetPackScheme(Guid packSchemeId)
        /// <summary>
        /// 获取套餐模板方案
        /// </summary>
        /// <param name="packSchemeId">套餐模板方案Id</param>
        /// <returns>套餐模板方案</returns>
        [OperationContract]
        DecorationPackSchemeInfo GetPackScheme(Guid packSchemeId);
        #endregion

        #region # 获取总下架商品SKU Id列表 —— IEnumerable<Guid> GetTotalOffShelvedSkus()
        /// <summary>
        /// 获取总下架商品SKU Id集列表
        /// </summary>
        /// <returns>商品SKU Id列表</returns>
        [OperationContract]
        IEnumerable<Guid> GetTotalOffShelvedSkus();
        #endregion

        #region # 根据商品SKU获取套餐模板列表 —— IEnumerable<DecorationPackInfo> GetDecorationPacksBySku(...
        /// <summary>
        /// 根据商品SKU获取套餐模板列表
        /// </summary>
        /// <param name="skuId">商品SKU Id</param>
        /// <returns>套餐模板列表</returns>
        [OperationContract]
        IEnumerable<DecorationPackInfo> GetDecorationPacksBySku(Guid skuId);
        #endregion

        #region # 获取总下架工艺实体Id列表 —— IEnumerable<Guid> GetTotalOffShelvedCrafts()
        /// <summary>
        /// 获取总下架工艺实体Id列表
        /// </summary>
        /// <returns>工艺实体Id列表</returns>
        [OperationContract]
        IEnumerable<Guid> GetTotalOffShelvedCrafts();
        #endregion

        #region # 获取总下架工艺实体Id列表 —— Dictionary<Guid, IEnumerable<DecorationPackInfo>> GetOffShelvedCraftsPacks()

        /// <summary>
        /// 获取总下架工艺实体Id列表
        /// </summary>
        /// <returns>工艺实体Id列表</returns>
        [OperationContract]
        Dictionary<Guid, IEnumerable<DecorationPackInfo>> GetOffShelvedCraftsPacks();
        #endregion

        #region # 获取总下架工艺实体Id列表 —— Dictionary<Guid, IEnumerable<DecorationPackInfo>> GetOffShelvedCraftsPacksByPage(Guid? createrId,int pageIndex, int pageSize)

        /// <summary>
        /// 获取总下架工艺实体Id列表
        /// </summary>
        /// <param name="createrId">套餐创建人Id？（全部：null,我的：当前登录人员）</param>
        /// <returns>工艺实体Id列表</returns>
        [OperationContract]
        Dictionary<Guid, IEnumerable<DecorationPackInfo>> GetOffShelvedCraftsPacksByPage(Guid? createrId, int pageIndex, int pageSize, out int rowCount, out int pageCount);
        #endregion

        #region # 获取总下架工艺数量 —— int GetTotalOffShelvedCraftCount(Guid? createrId)

        /// <summary>
        /// 获取总下架工艺数量
        /// </summary>
        /// <param name="createrId">套餐创建人Id？（全部：null,我的：当前登录人员）</param>
        /// <returns>总下架工艺数量</returns>
        [OperationContract]
        int GetTotalOffShelvedCraftCount(Guid? createrId);
        #endregion

        #region # 根据工艺实体获取套餐模板列表 —— IEnumerable<DecorationPackInfo> GetDecorationPacksByCraft(...
        /// <summary>
        /// 根据工艺实体获取套餐模板列表
        /// </summary>
        /// <param name="craftEntityId">工艺实体Id</param>
        /// <returns>套餐模板Id列表</returns>
        [OperationContract]
        IEnumerable<DecorationPackInfo> GetDecorationPacksByCraft(Guid craftEntityId);
        #endregion

        #region # 获取套餐商品SKU数量字典 —— IDictionary<Guid, float> GetSkuQuantities(Guid packId)
        /// <summary>
        /// 获取套餐商品SKU数量字典
        /// </summary>
        /// <param name="packId">套餐模板Id</param>
        /// <returns>商品SKU数量字典</returns>
        [OperationContract]
        IDictionary<Guid, float> GetSkuQuantities(Guid packId);
        #endregion

        #region # 获取套餐工艺工程量字典 —— IDictionary<Guid, float> GetCraftQuantities(Guid packId)
        /// <summary>
        /// 获取套餐工艺工程量字典
        /// </summary>
        /// <param name="packId">套餐模板Id</param>
        /// <returns>工艺工程量字典</returns>
        [OperationContract]
        IDictionary<Guid, float> GetCraftQuantities(Guid packId);
        #endregion

        #region # 获取套餐模板内下架工艺数量 —— IDictionary<Guid, int> GetOffShelvedCraftCount(IEnumerable<Guid> packIds)
        /// <summary>
        /// 获取套餐模板内下架工艺数量
        /// </summary>
        /// <param name="packIds">套餐模板Id集</param>
        /// <returns>下架工艺数量字典</returns>
        /// <remarks>IDictionary[Guid, int]，[套餐模板Id, 下架工艺数量]</remarks>
        [OperationContract]
        IDictionary<Guid, int> GetOffShelvedCraftCount(IEnumerable<Guid> packIds);
        #endregion

        #region # 获取固定源套餐标签集(居室|（设计风格编号|设计风格名称）|空间类型)—— Tuple<List<string>, Dictionary<string, string>, List<string>> GetSourceDecorationPackLabels()
        /// <summary>
        /// 获取固定源套餐标签集(居室|（设计风格编号|设计风格名称）|空间类型)
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        Tuple<List<string>, Dictionary<string, string>, List<string>> GetSourceDecorationPackLabels();
        #endregion

        #region # 根据套餐Id集获取套餐模板集 —— IEnumerable<DecorationPackInfo> GetDecorationPackByIds(IEnumerable<Guid> packIds)
        /// <summary>
        /// 根据套餐Id集获取套餐模板集
        /// </summary>
        /// <param name="packIds">套餐模板Id集合</param>
        /// <returns>套餐模板</returns>
        [OperationContract]
        IEnumerable<DecorationPackInfo> GetDecorationPackByIds(IEnumerable<Guid> packIds);
        #endregion

        #region # 获取总下架商品SKU列表 —— Dictionary<Guid, IEnumerable<DecorationPackInfo>> GetOffShelvedSkusPacks()

        /// <summary>
        /// 获取总下架商品SKU列表
        /// </summary>
        /// <returns>商品SKU列表</returns>
        [OperationContract]
        Dictionary<Guid, IEnumerable<DecorationPackInfo>> GetOffShelvedSkusPacks();
        #endregion

        #region # 获取总下架商品SKU列表 —— Dictionary<Guid, IEnumerable<DecorationPackInfo>> GetOffShelvedSkusPacksByPage(Guid? createrId,int pageIndex, int pageSize, out int rowCount, out int pageCount)
        /// <summary>
        /// 获取总下架商品SKU列表
        /// </summary>
        /// <param name="createrId">套餐创建人Id？（全部：null,我的：当前登录人员）</param>
        /// <returns>商品SKU列表</returns>
        [OperationContract]
        Dictionary<Guid, IEnumerable<DecorationPackInfo>> GetOffShelvedSkusPacksByPage(Guid? createrId, int pageIndex, int pageSize, out int rowCount,
            out int pageCount);
        #endregion


        #region # 根据源套餐Id获取套餐当前版本号—— DecorationPackInfo GetDecorationPackVersionNumber(Guid sourcePackId)
        /// <summary>
        /// 根据套餐Id获取套餐当前版本号
        /// </summary>
        /// <param name="sourcePackId">源套餐Id</param>
        /// <returns></returns>
        [OperationContract]
        DecorationPackInfo GetDecorationPackVersionNumber(Guid sourcePackId);
        #endregion

        #region # 根据套餐Id字典获取是否是最新版本及最新版本套餐状态—— Tuple<Guid,ShelfStatus,bool> GetNewestStatusByPackIds(Dictionary<Guid,Guid>  packDic)

        /// <summary>
        /// 根据套餐Id字典获取是否是最新版本及最新版本套餐状态
        /// </summary>
        /// <param name="packDic">Key:套餐Id , Value:源套餐Id</param>
        /// <returns>Item1:套餐Id , Item2:最新版本套餐状态 , Item3:是否最新版本</returns>
        [OperationContract]
        IEnumerable<Tuple<Guid, ShelfStatus, bool>> GetNewestStatusByPackIds(Dictionary<Guid, Guid> packDic);
        #endregion

        #region # 验证套餐模板名称是否存在 —— bool ExistsPackName(Guid? packId, string packName)
        /// <summary>
        /// 验证套餐模板名称是否存在
        /// </summary>
        /// <param name="packId">套餐模板Id</param>
        /// <param name="packName">套餐模板名称</param>
        /// <returns>是否存在</returns>
        [OperationContract]
        bool ExistsPackName(Guid? packId, string packName);
        #endregion

        #region # 验证套餐模板空间名称是否存在 —— bool ExistsPackSpaceName(Guid packId, Guid? packSpaceId...
        /// <summary>
        /// 验证套餐模板空间名称是否存在
        /// </summary>
        /// <param name="packId">套餐模板Id</param>
        /// <param name="packSpaceId">套餐模板空间Id</param>
        /// <param name="packSpaceName">套餐模板空间名称</param>
        /// <returns>是否存在</returns>
        [OperationContract]
        bool ExistsPackSpaceName(Guid packId, Guid? packSpaceId, string packSpaceName);
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
        [OperationContract]
        bool ExistsPackItemName(Guid packId, Guid packSpaceId, Guid? packItemId, string packItemName);
        #endregion

        #region # 验证套餐模板方案名称是否存在 —— bool ExistsPackSchemeName(Guid packId, Guid? packSchemeId...
        /// <summary>
        /// 验证套餐模板方案名称是否存在
        /// </summary>
        /// <param name="packId">套餐模板Id</param>
        /// <param name="packSchemeId">套餐模板方案Id</param>
        /// <param name="packSchemeName">套餐模板方案名称</param>
        /// <returns>是否存在</returns>
        [OperationContract]
        bool ExistsPackSchemeName(Guid packId, Guid? packSchemeId, string packSchemeName);
        #endregion

        #region # 验证套餐模板下是否配置商品SKU —— bool ExistsPackSku(Guid packId)
        /// <summary>
        /// 验证套餐模板下是否配置商品SKU
        /// </summary>
        /// <param name="packId">套餐模板Id</param>
        /// <returns>是否配置</returns>
        [OperationContract]
        bool ExistsPackSku(Guid packId);
        #endregion

        #region # 给定套餐模板空间下是否存在下架商品 —— IDictionary<Guid, bool> ExistsOffShelved(...
        /// <summary>
        /// 给定套餐模板空间下是否存在下架商品
        /// </summary>
        /// <param name="packSpaceIds">套餐模板空间Id</param>
        /// <returns>是否存在</returns>
        [OperationContract]
        IDictionary<Guid, bool> ExistsOffShelved(IEnumerable<Guid> packSpaceIds);
        #endregion

        #region # 给定套餐模板空间下是否存在下架工艺 —— IDictionary<Guid, bool> ExistsOffShelvedCraft(...
        /// <summary>
        /// 给定套餐模板空间下是否存在下架工艺
        /// </summary>
        /// <param name="packSpaceIds">套餐模板空间Id</param>
        /// <returns>是否存在</returns>
        [OperationContract]
        IDictionary<Guid, bool> ExistsOffShelvedCraft(IEnumerable<Guid> packSpaceIds);
        #endregion

        #region # 获取全部套餐商品SkuId集 —— IEnumerable<Guid> GetAllPackSkuIds()

        /// <summary>
        /// 获取全部套餐商品SkuId集
        /// </summary>
        /// <returns>商品SkuId集</returns>
        [OperationContract]
        IEnumerable<Guid> GetAllPackSkuIds();
        #endregion

        #region # 批量获取套餐工艺实体Id集 —— IEnumerable<Guid> GetAllPackCraftIds()

        /// <summary>
        /// 获取全部套餐工艺实体Id集
        /// </summary>
        /// <returns>工艺实体Id集</returns>
        [OperationContract]
        IEnumerable<Guid> GetAllPackCraftIds();
        #endregion

        #region # 获取套餐模板推荐项列表 —— IEnumerable<DecorationPackRecommendedItemInfo> GetPackRecommendedItems(Guid packId, Guid? packSpaceId);
        /// <summary>
        /// 获取套餐模板推荐项列表
        /// </summary>
        /// <param name="packId">套餐模板Id</param>
        /// <param name="packSpaceId">套餐模板空间Id</param>
        /// <returns>套餐模板推荐项列表</returns>
        [OperationContract]
        IEnumerable<DecorationPackRecommendedItemInfo> GetPackRecommendedItems(Guid packId, Guid? packSpaceId);
        #endregion

        //变价

        #region # 获取总变价商品数量 —— int GetTotalChangedCount(Guid? createrId)

        /// <summary>
        /// 获取总变价商品数量
        /// </summary>
        /// <param name="createrId">套餐创建人Id？（全部：null,我的：当前登录人员）</param>
        /// <returns>总变价商品数量</returns>
        [OperationContract]
        int GetTotalChangedCount(Guid? createrId);
        #endregion

        #region # 获取套餐模板内变价商品数量 —— IDictionary<Guid, int> GetChangedCount(IEnumerable<Guid> packIds)

        /// <summary>
        /// 获取套餐模板内变价商品数量
        /// </summary>
        /// <param name="packIds">套餐模板Id集</param>
        /// <returns>变价商品数量字典</returns>
        /// <remarks>IDictionary[Guid, int]，[套餐模板Id, 变价商品数量]</remarks>
        [OperationContract]
        IDictionary<Guid, int> GetChangedCount(IEnumerable<Guid> packIds);
        #endregion

        #region # 获取总变价商品SKU Id列表 —— IEnumerable<Guid> GetTotalChangedSkus()

        /// <summary>
        /// 获取总变价商品SKU Id集列表
        /// </summary>
        /// <returns>商品SKU Id列表</returns>
        [OperationContract]
        IEnumerable<Guid> GetTotalChangedSkus();
        #endregion

        #region # 获取总变价商品SKU列表 —— Dictionary<Guid, IEnumerable<DecorationPackInfo>> GetChangedSkusPacks()

        /// <summary>
        /// 获取总变价商品SKU列表
        /// </summary>
        /// <returns>商品SKU列表</returns>
        [OperationContract]
        Dictionary<Guid, IEnumerable<DecorationPackInfo>> GetChangedSkusPacks();
        #endregion

        #region # 获取总变价商品SKU列表 —— Dictionary<Guid, Dictionary<DecorationPackInfo, decimal>> GetChangedSkusPacksByPage(Guid? createrId,int pageIndex, int pageSize, out int rowCount, out int pageCount)

        /// <summary>
        /// 获取总变价商品SKU列表
        /// </summary>
        /// <param name="createrId">套餐创建人Id？（全部：null,我的：当前登录人员）</param>
        /// <returns>商品SKU列表</returns>
        [OperationContract]
        Dictionary<Guid, Dictionary<DecorationPackInfo, decimal>> GetChangedSkusPacksByPage(Guid? createrId, int pageIndex, int pageSize, out int rowCount, out int pageCount);

        #endregion

        #region # 给定套餐模板空间下是否存在变价商品 —— IDictionary<Guid, bool> ExistsChanged(...

        /// <summary>
        /// 给定套餐模板空间下是否存在变价商品
        /// </summary>
        /// <param name="packSpaceIds">套餐模板空间Id</param>
        /// <returns>是否存在</returns>
        [OperationContract]
        IDictionary<Guid, bool> ExistsChanged(IEnumerable<Guid> packSpaceIds);

        #endregion



        #region # 获取总变价工艺实体Id列表 —— IEnumerable<Guid> GetTotalChangedCrafts()
        /// <summary>
        /// 获取总变价工艺实体Id列表
        /// </summary>
        /// <returns>工艺实体Id列表</returns>
        [OperationContract]
        IEnumerable<Guid> GetTotalChangedCrafts();
        #endregion

        #region # 获取总变价工艺实体Id列表 —— Dictionary<Guid, IEnumerable<DecorationPackInfo>> GetChangedCraftsPacks()

        /// <summary>
        /// 获取总变价工艺实体Id列表
        /// </summary>
        /// <returns>工艺实体Id列表</returns>
        [OperationContract]
        Dictionary<Guid, IEnumerable<DecorationPackInfo>> GetChangedCraftsPacks();
        #endregion

        #region # 获取总变价工艺实体Id列表 —— Dictionary<Guid, Dictionary<DecorationPackInfo, decimal>> GetChangedCraftsPacksByPage(Guid? createrId,int pageIndex, int pageSize)

        /// <summary>
        /// 获取总变价工艺实体Id列表
        /// </summary>
        /// <param name="createrId">套餐创建人Id？（全部：null,我的：当前登录人员）</param>
        /// <returns>工艺实体Id列表</returns>
        [OperationContract]
        Dictionary<Guid, Dictionary<DecorationPackInfo, decimal>> GetChangedCraftsPacksByPage(Guid? createrId, int pageIndex, int pageSize, out int rowCount, out int pageCount);
        #endregion

        #region # 获取总变价工艺数量 —— int GetTotalChangedCraftCount(Guid? createrId)

        /// <summary>
        /// 获取总变价工艺数量
        /// </summary>
        /// <param name="createrId">套餐创建人Id？（全部：null,我的：当前登录人员）</param>
        /// <returns>总变价工艺数量</returns>
        [OperationContract]
        int GetTotalChangedCraftCount(Guid? createrId);
        #endregion

        #region # 获取套餐模板内变价工艺数量 —— IDictionary<Guid, int> GetChangedCraftCount(IEnumerable<Guid> packIds)
        /// <summary>
        /// 获取套餐模板内变价工艺数量
        /// </summary>
        /// <param name="packIds">套餐模板Id集</param>
        /// <returns>变价工艺数量字典</returns>
        /// <remarks>IDictionary[Guid, int]，[套餐模板Id, 变价工艺数量]</remarks>
        [OperationContract]
        IDictionary<Guid, int> GetChangedCraftCount(IEnumerable<Guid> packIds);
        #endregion

        #region # 给定套餐模板空间下是否存在变价工艺 —— IDictionary<Guid, bool> ExistsChangedCraft(...
        /// <summary>
        /// 给定套餐模板空间下是否存在变价工艺
        /// </summary>
        /// <param name="packSpaceIds">套餐模板空间Id</param>
        /// <returns>是否存在</returns>
        [OperationContract]
        IDictionary<Guid, bool> ExistsChangedCraft(IEnumerable<Guid> packSpaceIds);
        #endregion


        #region # 获取下架|变价 商品+工艺数量 ——Tuple<int, int> GetTotalOffShelvedChangedCount(Guid? createrId)

        /// <summary>
        /// 获取下架|变价 商品+工艺数量
        /// </summary>
        /// <param name="createrId">套餐创建人Id？（全部：null,我的：当前登录人员）</param>
        /// <returns>下架商品工艺数量合计|变价商品工艺合计</returns>
        [OperationContract]
        Tuple<int, int> GetTotalOffShelvedChangedCount(Guid? createrId);
        #endregion

        #region # 获取套餐模板内下架|变价的商品|工艺数量 —— IDictionary<Guid, Tuple<int, int, int, int>> GetOffShelvedChangedCountByIds(IEnumerable<Guid> packIds)

        /// <summary>
        /// 获取套餐模板内下架|变价的商品|工艺数量
        /// </summary>
        /// <param name="packIds">套餐Id集</param>
        /// <returns>套餐Id|下架商品数量|下架工艺数量|变价商品数量|变价工艺数量</returns>
        [OperationContract]
        IDictionary<Guid, Tuple<int, int, int, int>> GetOffShelvedChangedCountByIds(IEnumerable<Guid> packIds);
        #endregion

        #region  # 验证套餐模板下是否存在下架|变价的商品|工艺 —— IDictionary<Guid, Tuple<bool, bool, bool, bool>> ExistsOffShelvedChanged(IEnumerable<Guid> packSpaceIds)

        /// <summary>
        /// 验证套餐模板下是否存在下架|变价的商品|工艺
        /// </summary>
        /// <param name="packSpaceIds">套餐模板空间Id</param>
        /// <returns>套餐模板空间Id|存在下架商品|下架工艺|变价商品|变价工艺</returns>
        [OperationContract]
        IDictionary<Guid, Tuple<bool, bool, bool, bool>> ExistsOffShelvedChanged(IEnumerable<Guid> packSpaceIds);
        #endregion



        #region # 批量获取套餐选区集 —— IDictionary<Guid, IEnumerable<DecorationPackItemInfo>> GetDecorationPackItems(IEnumerable<Guid> packIds)
        /// <summary>
        /// 批量获取套餐选区集
        /// </summary>
        /// <param name="packIds">套餐Id集</param>
        /// <returns>套餐Id|选区集</returns>
        [OperationContract]
        IDictionary<Guid, IEnumerable<DecorationPackItemInfo>> GetDecorationPackItems(IEnumerable<Guid> packIds);
        #endregion

        #region # 批量获取套餐工艺项列表 —— Dictionary<Guid, Dictionary<Guid, IEnumerable<DecorationPackCraftInfo>>> GetBulkPackCraftsByPackIds(IEnumerable<Guid> packIds)

        /// <summary>
        /// 批量获取套餐工艺项列表
        /// </summary>
        /// <param name="packIds">套餐Id集</param>
        /// <returns>套餐工艺项字典</returns>
        /// IDictionary[Guid, Dictionary[Guid,IEnumerable[DecorationPackCraftInfo]]]，[套餐Id|套餐模板项Id|套餐模板工艺项列表]
        [OperationContract]
        Dictionary<Guid, Dictionary<Guid, IEnumerable<DecorationPackCraftInfo>>> GetBulkPackCraftsByPackIds(IEnumerable<Guid> packIds);

        #endregion

        #region # 批量获取套餐商品SKU项列表 —— IDictionary<Guid, IEnumerable<DecorationPackSkuInfo>> GetBulkPackSkusByPackId(...

        /// <summary>
        /// 批量获取套餐商品SKU项列表
        /// </summary>
        /// <param name="packIds">套餐Id集</param>
        /// <returns>套餐商品SKU项字典</returns>
        /// Dictionary[Guid, Dictionary[Guid,IEnumerable[DecorationPackSkuInfo]]]，[套餐Id|套餐模板项Id|套餐模板商品SKU项列表]
        [OperationContract]
        Dictionary<Guid, Dictionary<Guid, IEnumerable<DecorationPackSkuInfo>>> GetBulkPackSkusByPackIds(IEnumerable<Guid> packIds);
        #endregion

        #region 山河2020年新需求(2020-05-08)

        #region # 根据项目Id分页获取关联套餐模板列表 —— PageModel<DecorationPackInfo> GetDecorationPacksByProjectId(Guid projectId,string keywords, Guid? propertyId, float? applicableSquare, bool? isNewHouse,
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
        [OperationContract]
        PageModel<DecorationPackInfo> GetDecorationPacksByProjectId(Guid projectId, string keywords, Guid? propertyId, float? applicableSquare, bool? isNewHouse,
            bool? isBuildingPrice, decimal? minPrice, decimal? maxPrice, bool? isBuildingSquare, float? minSquare, float? maxSquare, DecorationPackType? packType, DecorationPackKind? packKind,
            DecorationPackMode? packMode, IList<string> spaceTypes, IList<string> styleNos, IList<string> layouts, bool hasPackSeries, IList<Guid> packSeriesIds, IList<Guid> stores, bool hasStores,
            Dictionary<string, bool> sort, int pageIndex, int pageSize);
        #endregion

        #endregion

        #region # 测试 Tuple —— void Test(Guid id)

        /// <summary>
        /// 测试 Tuple
        /// </summary>
        /// <param name="id"></param>
        [OperationContract]
        void Test(Guid id);
        #endregion

    }
}
