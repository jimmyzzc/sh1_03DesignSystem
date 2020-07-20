using HDIPlatform.DesignSystem.Domain.Entities.FixedMealContext;
using HDIPlatform.DesignSystem.IAppService.DTOs.Outputs.BaleContext;
using HDIPlatform.DesignSystem.IAppService.DTOs.Outputs.FixedMealContext;
using ShSoft.Common.PoweredByLee;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using HDIPlatform.DesignSystem.Domain.Mediators;
using HDIPlatform.DesignSystem.IAppService.DTOs.Outputs.ForAppContext;
using HDIPlatform.ResourceService.IAppService.DTOs.Outputs.CraftContext;
using HDIPlatform.ResourceService.IAppService.DTOs.Outputs.ProductContext;
using ServiceStack.Common.Web;

namespace HDIPlatform.DesignSystem.AppService.Maps
{
    /// <summary>
    /// 套餐模板相关映射工具类
    /// </summary>
    public static class DecorationPackMap
    {
        #region # 套餐模板映射 —— static DecorationPackInfo ToDTO(this DecorationPack pack)

        /// <summary>
        /// 套餐模板映射
        /// </summary>
        /// <param name="pack">套餐模板领域模型</param>
        /// <param name="packSchemes">套餐默认方案集</param>
        /// <returns>套餐模板数据传输对象</returns>
        public static DecorationPackInfo ToDTO(this DecorationPack pack, List<DecorationPackScheme> packSchemes)
        {
            DecorationPackScheme packScheme = packSchemes.SingleOrDefault(x => x.PackId == pack.Id);
            DecorationPackInfo packInfo = new DecorationPackInfo
            {
                Sort = (int)pack.Sort,
                Name = pack.Name,
                AddedTime = pack.AddedTime,
                BuildingSquare = pack.BuildingSquare,
                BuildingTotalPrice = pack.BuildingTotalPrice,
                BuildingUnitPrice = pack.BuildingUnitPrice,
                HouseTypes = pack.HouseTypes.ToList(),
                Propertys = pack.Propertys,
                UnitPrice = pack.UnitPrice,
                Status = pack.Status,
                Id = pack.Id,
                PackSeries = pack.PackSeries.Select(s => s.ToDTO()),
                Layouts = pack.Layouts,
                Color = pack.Color,
                //默认方案信息
                Cover = packScheme != null ? packScheme.Cover : null,
                CoverDescription = packScheme != null ? packScheme.Description : null,
                SchemeImages = packScheme != null ? packScheme.Images : new List<string>(),
                SchemeDescriptions = packScheme != null ? packScheme.SchemeDescriptions : null,
                SchemeVideoAudioLink = packScheme != null ? packScheme.VideoAudioLink : null,
                SchemeVideoAudioLinkFileId=packScheme != null ? packScheme.VideoAudioFileId:null,
                Creater = pack.Creater,
                CreaterId = pack.CreaterId,
                CriterionSquare = pack.CriterionSquare,
                HasChangedSku = pack.HasChangedSku,
                HasOffShelvedSku = pack.HasOffShelvedSku,
                Instructions = pack.Instructions,
                InstructionsName = pack.InstructionsName,
                IsActual = pack.IsActual,
                IsBuilding = pack.IsBuilding,
                IsClone = pack.IsClone,
                IsUnitActual = pack.IsUnitActual,
                IsUnitBuilding = pack.IsUnitBuilding,
                IsWhole = pack.IsWhole,
                MaxApplicableSquare = pack.MaxApplicableSquare,
                MinApplicableSquare = pack.MinApplicableSquare,
                NewHouse = pack.NewHouse,
                Number = pack.Number,
                PackKind = pack.PackKind,
                PackMode = pack.PackMode,
                PackType = pack.PackType,
                Premium = pack.Premium,
                Priced = pack.Priced,
                PricingType = pack.PricingType,
                Sales = pack.Sales,
                SecondHandHouse = pack.SecondHandHouse,
                SourcePackId = pack.SourcePackId,
                SpaceInfos = pack.SpaceInfos,
                SpaceTypes = pack.SpaceTypes,
                Square = pack.Square,
                StyleNos = pack.StyleNos,
                TotalPrice = pack.TotalPrice,
                UnitBuildingSquare = pack.UnitBuildingSquare,
                UnitBuildingTotalPrice = pack.UnitBuildingTotalPrice,
                UnitSquare = pack.UnitSquare,
                UnitTotalPrice = pack.UnitTotalPrice,
                VersionNumber = pack.VersionNumber,
                Views = pack.Views,
                Stores = pack.Stores,
                IsManageFee = pack.IsManageFee,
                IsWaterElectricityFee = pack.IsWaterElectricityFee,
                ManageFee = pack.ManageFee,
                WaterElectricityFee = pack.WaterElectricityFee
            };
            return packInfo;
        }

        #endregion

        #region # 套餐模板空间映射 —— static DecorationPackSpaceInfo ToDTO(this DecorationPackSpace packSpace)

        /// <summary>
        /// 套餐模板空间映射
        /// </summary>
        /// <param name="packSpace">套餐模板空间领域模型</param>
        /// <param name="offShelvedSkuDic">是否有下架商品</param>
        /// <param name="offShelvedCraftDic">是否有下架工艺</param>
        /// <param name="changedSkuDic">是否有变价商品</param>
        /// <param name="changedCraftDic">是否有变价工艺</param>
        /// <returns>套餐模板空间数据传输对象</returns>
        public static DecorationPackSpaceInfo ToDTO(this DecorationPackSpace packSpace, IDictionary<Guid, bool> offShelvedSkuDic, IDictionary<Guid, bool> offShelvedCraftDic, IDictionary<Guid, bool> changedSkuDic, IDictionary<Guid, bool> changedCraftDic)
        {
            DecorationPackSpaceInfo packSpaceInfo = Transform<DecorationPackSpace, DecorationPackSpaceInfo>.Map(packSpace);
            packSpaceInfo.DecorationPackInfo = packSpace.DecorationPack.ToDTO(new List<DecorationPackScheme>());
            packSpaceInfo.DecorationPackSpaceDetailInfos = packSpace.SpaceDetails.Select(x => x.ToDTO());
            packSpaceInfo.ExistsOffShelvedSkuCraft = offShelvedSkuDic[packSpace.Id] || offShelvedCraftDic[packSpace.Id];
            packSpaceInfo.ExistsChangedSkuCraft = changedSkuDic[packSpace.Id] || changedCraftDic[packSpace.Id];

            return packSpaceInfo;
        }
        #endregion

        #region # 套餐模板项映射 —— static DecorationPackItemInfo ToDTO(this DecorationPackItem packItem...
        /// <summary>
        /// 套餐模板项映射
        /// </summary>
        /// <param name="packItem">套餐模板项领域模型</param>
        /// <param name="decorationPackInfos">套餐模板字典</param>
        /// <returns>套餐模板项数据传输对象</returns>
        public static DecorationPackItemInfo ToDTO(this DecorationPackItem packItem)
        {
            DecorationPackItemInfo packItemInfo = Transform<DecorationPackItem, DecorationPackItemInfo>.Map(packItem);
            //packItemInfo.DecorationPackInfo = decorationPackInfos == null ? null : decorationPackInfos[packItem.PackId];
            return packItemInfo;
        }
        #endregion

        #region # 套餐模板项映射 —— static DecorationPackItemInfo ToDTO(this DecorationPackItem packItem...
        /// <summary>
        /// 套餐模板项映射
        /// </summary>
        /// <param name="packItem">套餐模板项领域模型</param>
        /// <param name="packs">套餐模板字典</param>
        /// <returns>套餐模板项数据传输对象</returns>
        public static DecorationPackItemInfo ToDTO(this DecorationPackItem packItem, IDictionary<Guid, DecorationPack> packs)
        {
            DecorationPackItemInfo packItemInfo = Transform<DecorationPackItem, DecorationPackItemInfo>.Map(packItem);

            DecorationPack pack = packs[packItem.PackId];
            packItemInfo.DecorationPackInfo = pack.ToDTO(new List<DecorationPackScheme>());
            packItemInfo.PackSpaceName = pack.Spaces.Single(x => x.Id == packItem.PackSpaceId).Name;


            return packItemInfo;
        }
        #endregion

        #region # 套餐模板推荐项映射 ——static DecorationPackRecommendedItemInfo ToDTO(this DecorationPackRecommendedItem packItem, IDictionary<Guid, DecorationPackSpace> packSpaces)
        /// <summary>
        /// 套餐模板推荐项映射
        /// </summary>
        /// <param name="packItem">套餐模板推荐项领域模型</param>
        /// <param name="packSpaces">套餐模板空间字典</param>
        /// <returns>套餐模板推荐项数据传输对象</returns>
        public static DecorationPackRecommendedItemInfo ToDTO(this DecorationPackRecommendedItem packItem, IDictionary<Guid, DecorationPackSpace> packSpaces)
        {
            DecorationPackRecommendedItemInfo packItemInfo = Transform<DecorationPackRecommendedItem, DecorationPackRecommendedItemInfo>.Map(packItem);

            packItemInfo.PackSpaceName = packSpaces[packItemInfo.PackSpaceId].Name;
            packItemInfo.PackRecommendedSkuInfos = packItem.PackRecommendedSkus.Select(s => s.ToDTO());
            return packItemInfo;
        }
        #endregion

        #region # 套餐模板推荐商品映射 ——static DecorationPackRecommendedSkuInfo ToDTO(this DecorationPackRecommendedSku sku)
        /// <summary>
        /// 套餐模板推荐商品映射
        /// </summary>
        /// <param name="sku">套餐模板推荐商品领域模型</param>
        /// <returns>套餐模板推荐商品数据传输对象</returns>
        public static DecorationPackRecommendedSkuInfo ToDTO(this DecorationPackRecommendedSku sku)
        {
            DecorationPackRecommendedSkuInfo packItemInfo = Transform<DecorationPackRecommendedSku, DecorationPackRecommendedSkuInfo>.Map(sku);

            return packItemInfo;
        }
        #endregion

        #region # 套餐模板商品SKU项映射 —— static DecorationPackSkuInfo ToDTO(this DecorationPackSku packSku)

        /// <summary>
        /// 套餐模板商品SKU项映射
        /// </summary>
        /// <param name="packSku">套餐模板商品SKU项领域模型</param>
        /// <param name="packs"></param>
        /// <returns>套餐模板商品SKU项数据传输对象</returns>
        public static DecorationPackSkuInfo ToDTO(this DecorationPackSku packSku, IDictionary<Guid, DecorationPack> packs)
        {
            DecorationPackSkuInfo packSkuInfo = Transform<DecorationPackSku, DecorationPackSkuInfo>.Map(packSku);


            packSkuInfo.PackItemInfo = packSku.PackItem.ToDTO(packs);

            return packSkuInfo;
        }
        #endregion

        #region # 套餐模板工艺项映射 —— static DecorationPackCraftInfo ToDTO(this DecorationPackCraft packCraft)

        /// <summary>
        /// 套餐模板工艺项映射
        /// </summary>
        /// <param name="packCraft">套餐模板工艺项领域模型</param>
        /// <param name="packs"></param>
        /// <returns>套餐模板工艺项数据传输对象</returns>
        public static DecorationPackCraftInfo ToDTO(this DecorationPackCraft packCraft, IDictionary<Guid, DecorationPack> packs)
        {
            DecorationPackCraftInfo packCraftInfo = Transform<DecorationPackCraft, DecorationPackCraftInfo>.Map(packCraft);

            packCraftInfo.PackItemInfo = packCraft.PackItem.ToDTO(packs);

            return packCraftInfo;
        }
        #endregion

        #region # 套餐模板方案映射 —— static DecorationPackSchemeInfo ToDTO(this DecorationPackScheme packScheme...

        /// <summary>
        /// 套餐模板方案映射
        /// </summary>
        /// <param name="packScheme">套餐模板方案领域模型</param>
        /// <param name="packs">套餐模板字典</param>
        /// <returns>套餐模板方案数据传输对象</returns>
        public static DecorationPackSchemeInfo ToDTO(this DecorationPackScheme packScheme, IDictionary<Guid, DecorationPack> packs)
        {
            DecorationPackSchemeInfo packSchemeInfo = Transform<DecorationPackScheme, DecorationPackSchemeInfo>.Map(packScheme);
            packSchemeInfo.DecorationPackInfo = packs == null ? null : packs[packScheme.PackId].ToDTO(new List<DecorationPackScheme>());

            return packSchemeInfo;
        }
        #endregion

        #region # 套餐模板方案空间映射 —— static DecorationPackSchemeSpaceInfo ToDTO(this DecorationPackSchemeSpace...
        /// <summary>
        /// 套餐模板方案空间映射
        /// </summary>
        /// <param name="packSchemeSpace">套餐模板方案空间领域模型</param>
        /// <returns>套餐模板方案空间数据传输对象</returns>
        public static DecorationPackSchemeSpaceInfo ToDTO(this DecorationPackSchemeSpace packSchemeSpace)
        {
            DecorationPackSchemeSpaceInfo packSchemeSpaceInfo = Transform<DecorationPackSchemeSpace, DecorationPackSchemeSpaceInfo>.Map(packSchemeSpace);
            packSchemeSpaceInfo.PackSchemeInfo = packSchemeSpace.PackScheme.ToDTO(null);

            return packSchemeSpaceInfo;
        }
        #endregion

        #region # 套餐模板方案空间映射 —— static DecorationPackSchemeSpaceInfo ToDTO(this DecorationPackSchemeSpace...
        /// <summary>
        /// 套餐模板方案空间映射
        /// </summary>
        /// <param name="spaceDetail">套餐模板方案空间领域模型</param>
        /// <returns>套餐模板方案空间数据传输对象</returns>
        public static DecorationPackSpaceDetailInfo ToDTO(this DecorationPackSpaceDetail spaceDetail)
        {
            DecorationPackSpaceDetailInfo spaceDetailInfo = Transform<DecorationPackSpaceDetail, DecorationPackSpaceDetailInfo>.Map(spaceDetail);

            return spaceDetailInfo;
        }
        #endregion

        #region # 套餐模板 - 大包/定制模板关系映射 —— static DecorationPack_BalePackInfo ToDTO(...
        /// <summary>
        /// 套餐模板 - 大包/定制模板关系映射
        /// </summary>
        /// <param name="relation">套餐模板 - 大包/定制模板关系领域模型</param>
        /// <param name="decorationPackInfos">套餐模板字典</param>
        /// <param name="balePackInfos">大包/定制模板字典</param>
        /// <returns>套餐模板 - 大包/定制模板关系数据传输对象</returns>
        public static DecorationPack_BalePackInfo ToDTO(this DecorationPack_BalePack relation, IDictionary<Guid, DecorationPackInfo> decorationPackInfos, IDictionary<Guid, BalePackInfo> balePackInfos)
        {
            DecorationPack_BalePackInfo relationInfo = Transform<DecorationPack_BalePack, DecorationPack_BalePackInfo>.Map(relation);
            relationInfo.DecorationPackInfo = decorationPackInfos[relation.DecorationPackId];
            relationInfo.BalePackInfo = balePackInfos[relation.BalePackId];

            return relationInfo;
        }
        #endregion

        #region # 套餐空间基础信息映射 —— static DecorationPackSpaceInfoForSale ToDto(this DecorationPackSpace space)

        /// <summary>
        /// 套餐空间基础信息映射
        /// </summary>
        /// <param name="space"></param>
        /// <returns></returns>
        public static DecorationPackSpaceBaseInfo ToDto(this DecorationPackSpace space)
        {
            DecorationPackSpaceBaseInfo packSpaceInfo = Transform<DecorationPackSpace, DecorationPackSpaceBaseInfo>.Map(space);
            return packSpaceInfo;
        }

        #endregion

        #region # 套餐模板(包含空间基本信息)映射 —— static DecorationPackInfo ToDTO(this DecorationPack pack)

        /// <summary>
        /// 套餐模板映射
        /// </summary>
        /// <param name="pack">套餐模板领域模型</param>
        /// <param name="packSchemes">套餐默认方案集</param>
        /// <returns>套餐模板数据传输对象</returns>
        public static DecorationPackInfo ToDtoAndSpaces(this DecorationPack pack, List<DecorationPackScheme> packSchemes)
        {
            DecorationPackScheme packScheme = packSchemes.SingleOrDefault(x => x.PackId == pack.Id);
            DecorationPackInfo packInfo = new DecorationPackInfo
            {
                Sort = (int)pack.Sort,
                Name = pack.Name,
                AddedTime = pack.AddedTime,
                BuildingSquare = pack.BuildingSquare,
                BuildingTotalPrice = pack.BuildingTotalPrice,
                BuildingUnitPrice = pack.BuildingUnitPrice,
                HouseTypes = pack.HouseTypes.ToList(),
                Propertys = pack.Propertys,
                UnitPrice = pack.UnitPrice,
                Status = pack.Status,
                Id = pack.Id,
                PackSeries = pack.PackSeries.Select(s => s.ToDTO()),
                Layouts = pack.Layouts,
                Color = pack.Color,
                //默认方案信息
                Cover = packScheme != null ? packScheme.Cover : null,
                CoverDescription = packScheme != null ? packScheme.Description : null,
                SchemeImages = packScheme != null ? packScheme.Images : new List<string>(),
                SchemeDescriptions = packScheme != null ? packScheme.SchemeDescriptions.ToList() : null,
                SchemeVideoAudioLink = packScheme != null ? packScheme.VideoAudioLink : null,
                SchemeVideoAudioLinkFileId = packScheme != null ? packScheme.VideoAudioFileId : null,
                Creater = pack.Creater,
                CreaterId = pack.CreaterId,
                CriterionSquare = pack.CriterionSquare,
                HasChangedSku = pack.HasChangedSku,
                HasOffShelvedSku = pack.HasOffShelvedSku,
                Instructions = pack.Instructions,
                InstructionsName = pack.InstructionsName,
                IsActual = pack.IsActual,
                IsBuilding = pack.IsBuilding,
                IsClone = pack.IsClone,
                IsUnitActual = pack.IsUnitActual,
                IsUnitBuilding = pack.IsUnitBuilding,
                IsWhole = pack.IsWhole,
                MaxApplicableSquare = pack.MaxApplicableSquare,
                MinApplicableSquare = pack.MinApplicableSquare,
                NewHouse = pack.NewHouse,
                Number = pack.Number,
                PackKind = pack.PackKind,
                PackMode = pack.PackMode,
                PackType = pack.PackType,
                Premium = pack.Premium,
                Priced = pack.Priced,
                PricingType = pack.PricingType,
                Sales = pack.Sales,
                SecondHandHouse = pack.SecondHandHouse,
                SourcePackId = pack.SourcePackId,
                SpaceInfos = pack.SpaceInfos,
                SpaceTypes = pack.SpaceTypes,
                Square = pack.Square,
                StyleNos = pack.StyleNos,
                TotalPrice = pack.TotalPrice,
                UnitBuildingSquare = pack.UnitBuildingSquare,
                UnitBuildingTotalPrice = pack.UnitBuildingTotalPrice,
                UnitSquare = pack.UnitSquare,
                UnitTotalPrice = pack.UnitTotalPrice,
                VersionNumber = pack.VersionNumber,
                Views = pack.Views,
                SpaceBaseInfos = pack.Spaces.Select(x => x.ToDto()).ToList(),
                Stores = pack.Stores,
                IsManageFee = pack.IsManageFee,
                IsWaterElectricityFee = pack.IsWaterElectricityFee,
                ManageFee = pack.ManageFee,
                WaterElectricityFee = pack.WaterElectricityFee
            };
            return packInfo;
        }

        #endregion

        #region App

        #region # 套餐模板映射 —— static App_DecorationPackInfo ToAppDTO(this DecorationPack pack)

        /// <summary>
        /// 套餐模板映射
        /// </summary>
        /// <param name="pack">套餐模板领域模型</param>
        /// <param name="packSchemes">套餐默认方案集</param>
        /// <returns>套餐模板数据传输对象</returns>
        public static App_DecorationPackInfo ToAppDTO(this DecorationPack pack, List<DecorationPackScheme> packSchemes)
        {
            DecorationPackScheme packScheme = packSchemes.SingleOrDefault(x => x.PackId == pack.Id);
            App_DecorationPackInfo packInfo = new App_DecorationPackInfo
            {
                Sort = (int)pack.Sort,
                Name = pack.Name,
                AddedTime = pack.AddedTime,
                BuildingSquare = pack.BuildingSquare,
                BuildingTotalPrice = pack.BuildingTotalPrice,
                BuildingUnitPrice = pack.BuildingUnitPrice,
                UnitPrice = pack.UnitPrice,
                Id = pack.Id,
                PackSeries = pack.PackSeries.Select(s => s.Name),
                PackSerieInfos = pack.PackSeries.ToDictionary(x => x.Name, x => x.Describe),
                Layouts = pack.Layouts,
                Color = pack.Color,

                //默认方案信息
                Cover = packScheme != null ? packScheme.Cover : null,
                CoverDescription = packScheme != null ? packScheme.Description : null,
                SchemeImages = packScheme != null ? packScheme.Images : new List<string>(),
                SchemeDescriptions = packScheme != null ? packScheme.SchemeDescriptions.ToList() : null,
                SchemeVideoAudioLink = packScheme != null ? packScheme.VideoAudioLink : null,
                SchemeVideoAudioLinkFileId = packScheme != null ? packScheme.VideoAudioFileId : null,
                CriterionSquare = pack.CriterionSquare,
                Instructions = pack.Instructions,
                InstructionsName = pack.InstructionsName,
                IsActual = pack.IsActual,
                IsBuilding = pack.IsBuilding,
                IsUnitActual = pack.IsUnitActual,
                IsUnitBuilding = pack.IsUnitBuilding,
                IsWhole = pack.IsWhole,
                MaxApplicableSquare = pack.MaxApplicableSquare,
                MinApplicableSquare = pack.MinApplicableSquare,
                NewHouse = pack.NewHouse,
                Number = pack.Number,
                PackKind = pack.PackKind,
                PackMode = pack.PackMode,
                PackType = pack.PackType,
                Premium = pack.Premium,
                Priced = pack.Priced,
                PricingType = pack.PricingType,
                Sales = pack.Sales,
                SecondHandHouse = pack.SecondHandHouse,
                SpaceInfos = pack.SpaceInfos,
                Square = pack.Square,
                StyleNos = pack.StyleNos,
                TotalPrice = pack.TotalPrice,
                UnitBuildingSquare = pack.UnitBuildingSquare,
                UnitBuildingTotalPrice = pack.UnitBuildingTotalPrice,
                UnitSquare = pack.UnitSquare,
                UnitTotalPrice = pack.UnitTotalPrice,
                Views = pack.Views,
                Propertys = pack.Propertys,
                Status = pack.Status,
                IsManageFee = pack.IsManageFee,
                IsWaterElectricityFee = pack.IsWaterElectricityFee,
                ManageFee = pack.ManageFee,
                WaterElectricityFee = pack.WaterElectricityFee
            };
            return packInfo;
        }
        #endregion

        #region # 套餐模板商品映射 —— static App_DecorationPackSkuInfo ToAppDTO(this DecorationPackSku packSku,ProductSkuInfo productSkuInfo,string spaceName)

        /// <summary>
        /// 套餐模板商品映射
        /// </summary>
        /// <param name="packSku">套餐模板商品领域模型</param>
        /// <param name="productSkuInfo">商品信息</param>
        /// <param name="decorationPackSpaces">空间集</param>
        /// <returns>套餐模板商品数据传输对象</returns>
        public static App_DecorationPackSkuInfo ToAppDTO(this DecorationPackSku packSku, ProductSkuInfo productSkuInfo, IEnumerable<DecorationPackSpace> decorationPackSpaces)
        {
            App_DecorationPackSkuInfo packSkuInfo = Transform<DecorationPackSku, App_DecorationPackSkuInfo>.Map(packSku);
            packSkuInfo.PackItemName = packSku.PackItem.Name;
            packSkuInfo.SpaceName = decorationPackSpaces.Single(s => s.Id == packSku.PackItem.PackSpaceId).Name;
            packSkuInfo.SpaceId = packSku.PackItem.PackSpaceId;
            packSkuInfo.Sort = decorationPackSpaces.Single(s => s.Id == packSku.PackItem.PackSpaceId).Sort;
            packSkuInfo.Name = productSkuInfo.Name;
            packSkuInfo.ProductType = productSkuInfo.ProductType;
            packSkuInfo.Brand = productSkuInfo.BrandName;
            packSkuInfo.Category = productSkuInfo.CategoryName;
            return packSkuInfo;
        }
        #endregion

        #region # 套餐模板推荐商品映射 —— static App_DecorationPackSkuInfo ToAppDTO(this DecorationPackSku packSku,ProductSkuInfo productSkuInfo,string spaceName)

        /// <summary>
        /// 套餐模板推荐商品映射
        /// </summary>
        /// <param name="packRecommendedSku">套餐模板推荐商品领域模型</param>
        /// <param name="productSkuInfo">推荐商品信息</param>
        /// <param name="decorationPackSpaces">空间集</param>
        /// <returns>套餐模板推荐商品数据传输对象</returns>
        public static App_DecorationPackSkuInfo ToAppDTO(this DecorationPackRecommendedSku packRecommendedSku, ProductSkuInfo productSkuInfo, List<DecorationPackSpace> decorationPackSpaces)
        {
            App_DecorationPackSkuInfo packSkuInfo = Transform<DecorationPackRecommendedSku, App_DecorationPackSkuInfo>.Map(packRecommendedSku);
            packSkuInfo.PackItemName = packRecommendedSku.PackRecommendedItem.Name;
            packSkuInfo.SpaceName = decorationPackSpaces.Single(s => s.Id == packRecommendedSku.PackRecommendedItem.PackSpaceId).Name;
            packSkuInfo.SpaceId = packRecommendedSku.PackRecommendedItem.PackSpaceId;
            packSkuInfo.Sort = decorationPackSpaces.Single(s => s.Id == packRecommendedSku.PackRecommendedItem.PackSpaceId).Sort;
            packSkuInfo.Name = productSkuInfo.Name;
            packSkuInfo.ProductType = productSkuInfo.ProductType;
            packSkuInfo.Brand = productSkuInfo.BrandName;
            packSkuInfo.Category = productSkuInfo.CategoryName;
            return packSkuInfo;
        }
        #endregion

        #region # 套餐模板工艺映射 —— static App_DecorationPackCraftInfo ToAppDTO(this DecorationPackCraft packCraft, CraftEntityInfo craftEntityInfo, string spaceName)

        /// <summary>
        /// 套餐模板工艺映射
        /// </summary>
        /// <param name="packCraft">套餐模板工艺领域模型</param>
        /// <param name="craftEntityInfo">工艺信息</param>
        /// <param name="decorationPackSpaces">空间集</param>
        /// <returns>decorationPackSpaces</returns>
        public static App_DecorationPackCraftInfo ToAppDTO(this DecorationPackCraft packCraft, CraftEntityInfo craftEntityInfo, IEnumerable<DecorationPackSpace> decorationPackSpaces)
        {
            App_DecorationPackCraftInfo packCraftInfo = Transform<DecorationPackCraft, App_DecorationPackCraftInfo>.Map(packCraft);
            packCraftInfo.PackItemName = packCraft.PackItem.Name;
            packCraftInfo.SpaceName = decorationPackSpaces.Single(s => s.Id == packCraft.PackItem.PackSpaceId).Name;
            packCraftInfo.SpaceId = packCraft.PackItem.PackSpaceId;
            packCraftInfo.Sort = decorationPackSpaces.Single(s => s.Id == packCraft.PackItem.PackSpaceId).Sort;
            packCraftInfo.Name = craftEntityInfo.Name;
            return packCraftInfo;
        }
        #endregion

        #region # 套餐空间效果图映射 —— static App_DecorationPackSpaceImageInfo ToAppDTO(this DecorationPackSpace space, IEnumerable<DecorationPackSchemeSpace> packSchemeSpaces)

        /// <summary>
        /// 套餐空间效果图映射
        /// </summary>
        /// <param name="space"></param>
        /// <param name="packSchemeSpaces"></param>
        /// <returns></returns>
        public static App_DecorationPackSpaceImageInfo ToAppDTO(this DecorationPackSpace space, IEnumerable<DecorationPackSchemeSpace> packSchemeSpaces)
        {
            App_DecorationPackSpaceImageInfo spaceImageInfo = Transform<DecorationPackSpace, App_DecorationPackSpaceImageInfo>.Map(space);
            spaceImageInfo.PackSpaceId = space.Id;
            spaceImageInfo.PackSpaceName = space.Name;
            spaceImageInfo.PackSchemeSpaceInfo = packSchemeSpaces.Any() ? packSchemeSpaces.Where(x => x.PackSpaceId == space.Id).Select(x => x.ToDTO()) : new List<DecorationPackSchemeSpaceInfo>();
            return spaceImageInfo;
        }

        #endregion

        #endregion
    }
}
