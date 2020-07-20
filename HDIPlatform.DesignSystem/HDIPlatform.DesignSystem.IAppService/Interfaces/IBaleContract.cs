using HDIPlatform.DesignSystem.IAppService.DTOs.Outputs.BaleContext;
using ShSoft.Infrastructure;
using ShSoft.Infrastructure.DTOBase;
using ShSoft.ValueObjects.Enums;
using System;
using System.Collections.Generic;
using System.ServiceModel;

namespace HDIPlatform.DesignSystem.IAppService.Interfaces
{
    /// <summary>
    /// 大包服务契约接口
    /// </summary>
    [ServiceContract(Namespace = "http://HDIPlatform.DesignSystem.IAppService.Interfaces")]
    public interface IBaleContract : IApplicationService
    {
        //命令部分

        #region # 创建大包套餐 ——  Guid CreateBalePack(string packName, Guid operatorId, string @operator)

        /// <summary>
        /// 创建大包套餐
        /// </summary>
        /// <param name="packName">大包套餐名称</param>
        /// <param name="operatorId">操作人Id</param>
        /// <param name="operator">操作人名称</param>
        /// <returns>套餐Id</returns>
        [OperationContract]
        Guid CreateBalePack(string packName, Guid operatorId, string @operator);
        #endregion

        #region # 创建定制套餐 —— Guid CrateCustomizedPack(string packName)

        /// <summary>
        /// 创建定制套餐
        /// </summary>
        /// <param name="packName">定制套餐名称</param>
        /// <param name="operatorId">操作人Id</param>
        /// <param name="operator">操作人名称</param>
        /// <returns>套餐Id</returns>
        [OperationContract]
        Guid CrateCustomizedPack(string packName, Guid operatorId, string @operator);
        #endregion

        #region # 设置套餐比例 —— void SetPackDiscountRatio(Guid packId, float discountRatio)

        /// <summary>
        /// 设置套餐比例
        /// </summary>
        /// <param name="packId">套餐Id</param>
        /// <param name="discountRatio">套餐比例</param>
        [OperationContract]
        void SetPackDiscountRatio(Guid packId, float discountRatio);
        #endregion

        #region # 设置套餐标签 ——  void SetPackLabel(Guid packId, IEnumerable<string> labels)

        /// <summary>
        /// 设置套餐标签
        /// </summary>
        /// <param name="packId">套餐Id</param>
        /// <param name="labels">标签集</param>
        [OperationContract]
        void SetPackLabel(Guid packId, IEnumerable<string> labels);
        #endregion

        #region # 设置套餐封面 —— void SetPackCoverDrawing(Guid packId, string coverDrawing)

        /// <summary>
        /// 设置套餐封面
        /// </summary>
        /// <param name="packId">套餐Id</param>
        /// <param name="coverDrawing">封面图</param>
        [OperationContract]
        void SetPackCoverDrawing(Guid packId, string coverDrawing);
        #endregion

        #region # 设置套餐下架 —— void SetPackShelfOff(Guid packId)

        /// <summary>
        /// 下架套餐
        /// </summary>
        /// <param name="packId">套餐Id</param>
        [OperationContract]
        void SetPackShelfOff(Guid packId);
        #endregion

        #region # 设置套餐上架 —— void SetPackShelfed(Guid packId)

        /// <summary>
        /// 上架套餐
        /// </summary>
        /// <param name="packId">套餐Id</param>
        [OperationContract]
        void SetPackShelfed(Guid packId);
        #endregion

        #region # 修改大包套餐名称及折扣比例 —— void ModifyBalePackNameAndRatio(Guid packId,string packName, float discountRatio)

        /// <summary>
        /// 修改大包套餐名称及折扣比例
        /// </summary>
        /// <param name="packId">套餐Id</param>
        /// <param name="packName">套餐名称</param>
        /// <param name="discountRatio">折扣比例</param>
        [OperationContract]
        void ModifyBalePackNameAndRatio(Guid packId, string packName, float discountRatio);

        #endregion

        #region # 修改定制套餐名称及折扣比例 —— void ModifyCustPackNameAndRatio(Guid packId,string packName, float discountRatio)

        /// <summary>
        /// 修改定制套餐名称及折扣比例
        /// </summary>
        /// <param name="packId">套餐Id</param>
        /// <param name="packName">套餐名称</param>
        /// <param name="discountRatio">折扣比例</param>
        [OperationContract]
        void ModifyCustPackNameAndRatio(Guid packId, string packName, float discountRatio);

        #endregion

        #region # 删除套餐 —— void RemovePack(Guid packId)

        /// <summary>
        /// 删除套餐
        /// </summary>
        /// <param name="packId">套餐Id</param>
        [OperationContract]
        void RemovePack(Guid packId);

        #endregion

        #region # 新增选区 ——  Guid CreateChoiceArea(Guid packId, string choiceAreaName)

        /// <summary>
        /// 新增选区
        /// </summary>
        /// <param name="packId">套餐Id</param>
        /// <param name="choiceAreaName">选区名称</param>
        [OperationContract]
        Guid CreateChoiceArea(Guid packId, string choiceAreaName);
        #endregion

        #region # 修改选区名称 —— void UpdateChoiceAreaName(Guid packId,Guid choiceAreaId, string choiceAreaName)

        /// <summary>
        /// 修改选区名称
        /// </summary>
        /// <param name="packId">套餐Id</param>
        /// <param name="choiceAreaId">选区Id</param>
        /// <param name="choiceAreaName">选区名称</param>
        [OperationContract]
        void UpdateChoiceAreaName(Guid packId, Guid choiceAreaId, string choiceAreaName);
        #endregion

        #region # 删除选区 —— void RemoveChoiceArea(Guid choiceAreaId)

        /// <summary>
        /// 删除选区
        /// </summary>
        /// <param name="packId">套餐Id</param>
        /// <param name="choiceAreaId">选区Id</param>
        [OperationContract]
        void RemoveChoiceArea(Guid packId, Guid choiceAreaId);
        #endregion

        #region # 新增组 —— void CreateGroupInChoiceArea(Guid choiceAreaId, string groupName...

        /// <summary>
        /// 新增组
        /// </summary>
        /// <param name="choiceAreaId">选区Id</param>
        /// <param name="groupName">组名</param>
        /// <param name="required">是否必选</param>
        /// <param name="categoryIds">品类Id集</param>
        [OperationContract]
        Guid CreateGroupInChoiceArea(Guid choiceAreaId, string groupName, bool required);
        #endregion

        #region # 修改组名称及选购方式 —— void UpdateGroupNameAndIsRequired(Guid groupId, string groupName, bool isRequired)

        /// <summary>
        /// 修改组名称及选购方式
        /// </summary>
        /// <param name="groupId">组Id</param>
        /// <param name="groupName">组名称</param>
        /// <param name="isRequired">选购方式true|必选，false|推荐</param>
        [OperationContract]
        void UpdateGroupNameAndIsRequired(Guid groupId, string groupName, bool isRequired);
        #endregion

        #region # 删除组 —— void RemoveGroup(Guid groupId)

        /// <summary>
        /// 删除组
        /// </summary>
        /// <param name="groupId">组Id</param>
        [OperationContract]
        void RemoveGroup(Guid groupId);
        #endregion

        #region # 新增品类 —— void CreateCategory(Guid groupId, IEnumerable<Guid> categoryIds)

        /// <summary>
        /// 新增品类
        /// </summary>
        /// <param name="groupId">组Id</param>
        /// <param name="categoryIds">品类Id集</param>
        [OperationContract]
        void CreateCategory(Guid groupId, IEnumerable<Guid> categoryIds);
        #endregion

        #region # 删除品类 —— void RemoveCategory(Guid groupId, Guid categoryId)

        /// <summary>
        /// 删除品类
        /// </summary>
        /// <param name="groupId">组Id</param>
        /// <param name="categoryId">品类Id</param>
        [OperationContract]
        void RemoveCategory(Guid groupId, Guid categoryId);
        #endregion

        #region # 新增品类下商品 —— void CreateBalePackProduct(Guid groupId, Guid categoryId...

        /// <summary>
        /// 新增品类下商品集
        /// </summary>
        /// <param name="groupId">组Id</param>
        /// <param name="categoryId">品类Id</param>
        /// <param name="productIds">商品Id集</param>
        [OperationContract]
        void CreateBalePackProduct(Guid groupId, Guid categoryId, IEnumerable<Guid> productIds);

        #endregion

        #region # 移除品类下已配置商品 —— void RemoveBalePackProduct(Guid groupId...

        /// <summary>
        /// 移除品类下已配置商品
        /// </summary>
        /// <param name="groupId">组Id</param>
        /// <param name="categoryId">品类Id</param>
        /// <param name="productIds">商品Id集</param>
        [OperationContract]
        void RemoveBalePackProduct(Guid groupId, Guid categoryId, IEnumerable<Guid> productIds);
        #endregion

        #region # 替换下架商品 —— void ChangedProduct(Guid shelfOffProductId, IEnumerable<Guid> packIds, Guid productId)

        /// <summary>
        /// 替换下架商品
        /// </summary>
        /// <param name="shelfOffProductId">下架商品Id</param>
        /// <param name="packIds">套餐Id集</param>
        /// <param name="productId">商品Id</param>
        [OperationContract]
        void ChangedProduct(Guid shelfOffProductId, IEnumerable<Guid> packIds, Guid productId);
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

        //查询部分

        #region # 获取下架商品Id列表 —— IEnumerable<Guid> GetShelfOffProducts()

        /// <summary>
        /// 获取下架商品
        /// </summary>
        /// <returns>下架商品Id集</returns>
        [OperationContract]
        IEnumerable<Guid> GetShelfOffProducts();
        #endregion

        #region # 获取下架商品所在套餐列表 —— IDictionary<Guid, string> GetPackIdAndName(Guid productId)

        /// <summary>
        /// 获取下架商品所在套餐列表
        /// </summary>
        /// <param name="productId">下架商品Id</param>
        /// <returns>IDictionary[Guid, string]，[套餐Id, 套餐名称]</returns>
        [OperationContract]
        IDictionary<Guid, string> GetPackIdAndName(Guid productId);
        #endregion

        #region # 根据组Id品类Id获取品类已配置商品列表 —— IEnumerable<BalePackProductInfo> GetProductsByCategoryId(Guid groupId, Guid categoryId)

        /// <summary>
        /// 根据组Id品类Id获取品类已配置商品列表
        /// </summary>
        /// <param name="groupId">组Id</param>
        /// <param name="categoryId">品类Id</param>
        /// <returns>品类已配置商品列表</returns>
        [OperationContract]
        IEnumerable<BalePackProductInfo> GetProductsByCategoryId(Guid groupId, Guid categoryId);

        #endregion

        #region # 根据组Id品类Id获取品类已配置商品列表 —— IDictionary<Guid, bool> GetProducts(Guid groupId, Guid categoryId)

        /// <summary>
        /// 根据组Id品类Id获取品类已配置商品列表
        /// </summary>
        /// <param name="groupId">组Id</param>
        /// <param name="categoryId">品类Id</param>
        /// <returns>IDictionary[Guid, bool]，[商品Id, 是否已上架]</returns>
        [OperationContract]
        IDictionary<Guid, bool> GetProducts(Guid groupId, Guid categoryId);

        #endregion

        #region # 根据组Id获取组内三级品类Id列表及是否含下架商品及是否包含商品 ——  IEnumerable<Tuple<Guid, bool, bool>> GetCategoryIdsByGroup(Guid groupId)

        /// <summary>
        /// 根据组Id获取组内三级品类Id列表及是否含下架商品及是否包含商品
        /// </summary>
        /// <param name="groupId">组Id</param>
        /// <returns>IEnumerable<Tuple<Guid, bool, bool>>，[组内三级品类Id列表, 品类内是否含下架商品true|有，false|无,品类内是否还有商品]</returns>
        [OperationContract]
        IEnumerable<Tuple<Guid, bool, bool>> GetCategoryIdsByGroup(Guid groupId);

        #endregion

        #region # 测试Tuple —— Test(Guid id);
        /// <summary>
        /// 测试Tuple
        /// </summary>
        /// <param name="id"></param>
        [OperationContract]
        void Test(Guid id);
        #endregion

        #region # 根据套餐Id获取套餐内选区列表 —— IEnumerable<ChoiceAreaInfo> GetChoiceAreas(Guid packId)

        /// <summary>
        /// 根据套餐Id获取套餐内选区列表
        /// </summary>
        /// <param name="packId">套餐Id</param>
        /// <returns>套餐内选区列表</returns>
        [OperationContract]
        IEnumerable<BalePackChoiceAreaInfo> GetChoiceAreas(Guid packId);

        #endregion

        #region # 根据套餐Id获取套餐内选区列表 —— IDictionary<Guid, string> GetChoiceAreasByPackId(Guid packId)

        /// <summary>
        /// 根据套餐Id获取套餐内选区列表
        /// </summary>
        /// <param name="packId">套餐Id</param>
        /// <returns>IDictionary[Guid, string]，[选区Id, 选区名称]</returns>
        [OperationContract]
        IDictionary<Guid, string> GetChoiceAreasByPackId(Guid packId);

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
        [OperationContract]
        PageModel<BalePackInfo> GetBalePackByPage(string keywords, ShelfStatus? status, IList<string> labels, Dictionary<string, bool> sort, int pageIndex, int pageSize);
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
        [OperationContract]
        PageModel<BalePackInfo> GetCustPackByPage(string keywords, ShelfStatus? status, IList<string> labels, Dictionary<string, bool> sort, int pageIndex, int pageSize);
        #endregion

        #region # 根据选区Id获取选区内组列表 —— IEnumerable<GroupInChoiceAreaInfo> GetGroupsByChoiceAreaId(Guid choiceAreaId)

        /// <summary>
        /// 根据选区Id获取选区内组列表
        /// </summary>
        /// <param name="choiceAreaId">选区Id</param>
        /// <returns>选区内组列表</returns>
        [OperationContract]
        IEnumerable<BalePackGroupInfo> GetGroupsByChoiceAreaId(Guid choiceAreaId);

        #endregion

        #region # 根据套餐Id获取套餐内组列表 —— IEnumerable<GroupInChoiceAreaInfo> GetGroupsByPackId(Guid packId)

        /// <summary>
        /// 根据套餐Id获取套餐内组列表
        /// </summary>
        /// <param name="packId">套餐Id</param>
        /// <returns>套餐内组列表</returns>
        [OperationContract]
        IEnumerable<BalePackGroupInfo> GetGroupsByPackId(Guid packId);
        #endregion

        #region # 根据套餐获取大包|定制套餐选区树 —— IEnumerable<BalePackAreaTreeInfo> GetBalePackAreaTreeInfo(Guid packId)

        /// <summary>
        /// 根据套餐获取大包|定制套餐选区树（选区-组-品类-品牌）
        /// </summary>
        /// <param name="packId"></param>
        /// <returns></returns>
        [OperationContract]
        IEnumerable<BalePackAreaTreeInfo> GetBalePackAreaTreeInfo(Guid packId);
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
        [OperationContract]
        PageModel<BalePackInfo> GetRelateBalePackByDecoId(Guid decoId, BalePackType? type, string keywords, ShelfStatus? status, IList<string> labels, int pageIndex, int pageSize);
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
        [OperationContract]
        PageModel<BalePackInfo> GetNoRelateBalePackByDecoId(Guid decoId, BalePackType? type, string keywords, ShelfStatus? status, IList<string> labels, int pageIndex, int pageSize);
        #endregion

        #region # 根据套餐Id获取套餐 —— GetBalePackInfoById(Guid packId)

        /// <summary>
        /// 根据套餐Id获取套餐
        /// </summary>
        /// <param name="packId">套餐Id</param>
        /// <returns></returns>
        [OperationContract]
        BalePackInfo GetBalePackInfoById(Guid packId);
        #endregion

        #region # 根据套餐Id批量获取套餐 —— IEnumerable<BalePackInfo> GetBalePackInfoByIds(IEnumerable<Guid> packIds)
        /// <summary>
        /// 根据套餐Id批量获取套餐
        /// </summary>
        /// <param name="packIds">套餐Ids</param>
        /// <returns></returns>
        [OperationContract]
        IEnumerable<BalePackInfo> GetBalePackInfoByIds(IEnumerable<Guid> packIds);
        #endregion

        #region # 根据套餐Id获取套餐下架商品 —— IDictionary<Guid, int> GetPackShelfOffProCount(IEnumerable<Guid> packIds)

        /// <summary>
        /// 根据套餐Id获取套餐下架商品
        /// </summary>
        /// <returns>IDictionary[Guid, string]，[套餐Id, 下架商品个数]</returns>
        [OperationContract]
        IDictionary<Guid, int> GetPackShelfOffProCount(IEnumerable<Guid> packIds);
        #endregion

        //#region # 根据套餐Id获取选区及是否有下架商品
        ///// <summary>
        ///// 根据套餐Id获取选区及是否有下架商品
        ///// </summary>
        ///// <param name="packId">套餐Id</param>
        ///// <returns></returns>
        //IDictionary<Guid, bool> GetChoiceAreasShelved(Guid packId);
        //#endregion

        #region # 根据套餐选区组三级品类获取套餐下商品列表 —— IEnumerable<Guid> GetProductsByPackId(Guid packId, Guid? choiceAreaId, Guid? groupId, Guid? categoryId)

        /// <summary>
        /// 获取套餐下商品
        /// </summary>
        /// <param name="packId">套餐Id</param>
        /// <param name="choiceAreaId">选区Id</param>
        /// <param name="groupId">组Id</param>
        /// <param name="categoryId">三级品类Id</param>
        /// <returns></returns>
        [OperationContract]
        IEnumerable<Guid> GetProductsByPackId(Guid packId, Guid? choiceAreaId, Guid? groupId, Guid? categoryId);
        #endregion


        #region # 根据源套餐Id获取套餐当前版本号—— BalePackInfo GetPackVersionNumber(Guid sourcePackId)
        /// <summary>
        /// 根据源套餐Id获取套餐当前版本号
        /// </summary>
        /// <param name="sourcePackId">源套餐Id</param>
        /// <returns></returns>
        [OperationContract]
        BalePackInfo GetPackVersionNumber(Guid sourcePackId);
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

        #region # 验证大包套餐名称是否存在 —— bool ExistsBalePackName(Guid? packId, string packName)

        /// <summary>
        /// 验证大包套餐名称是否存在
        /// </summary>
        /// <param name="packId">套餐Id</param>
        /// <param name="packName">套餐名称</param>
        /// <returns></returns>
        [OperationContract]
        bool ExistsBalePackName(Guid? packId, string packName);
        #endregion

        #region # 验证定制套餐名称是否存在 —— bool ExistsCustPackName(Guid? packId, string packName)

        /// <summary>
        /// 验证定制套餐名称是否存在
        /// </summary>
        /// <param name="packId">套餐Id</param>
        /// <param name="packName">套餐名称</param>
        /// <returns></returns>
        [OperationContract]
        bool ExistsCustPackName(Guid? packId, string packName);
        #endregion

        #region # 验证选区名称是否存在 ——  bool ExistsChoiceAreaName(Guid packId, Guid? choiceAreaId, string choiceAreaName)
        /// <summary>
        /// 验证选区名称是否存在
        /// </summary>
        /// <param name="packId">套餐Id</param>
        /// <param name="choiceAreaId">选区Id</param>
        /// <param name="choiceAreaName">选区名称</param>
        /// <returns></returns>
        [OperationContract]
        bool ExistsChoiceAreaName(Guid packId, Guid? choiceAreaId, string choiceAreaName);
        #endregion

        #region  # 验证大包套餐组内是否有品类 ——  bool ExistsCategoryByPackId(Guid packId)

        /// <summary>
        /// 验证大包套餐组内是否有品类
        /// </summary>
        /// <param name="packId">套餐Id</param>
        /// <returns></returns>
        [OperationContract]
        bool ExistsCategoryByPackId(Guid packId);
        #endregion

        #region # 验证定制套餐组内品类下是否有商品 —— bool ExistsProductByPackId(Guid packId)

        /// <summary>
        /// 验证定制套餐组内下是否有商品
        /// </summary>
        /// <param name="packId">套餐Id</param>
        /// <returns></returns>
        [OperationContract]
        bool ExistsProductByPackId(Guid packId);
        #endregion

        #region # 验证分组名称是否存在 —— bool ExistsGroupName(Guid choiceAreaId, Guid? groupId, string groupName)

        /// <summary>
        /// 验证分组名称是否存在
        /// </summary>
        /// <param name="choiceAreaId">选区Id</param>
        /// <param name="groupId">组Id</param>
        /// <param name="groupName">组名称</param>
        /// <returns></returns>
        [OperationContract]
        bool ExistsGroupName(Guid choiceAreaId, Guid? groupId, string groupName);

        #endregion



        #region # 批量获取大包|定制套餐选区树（选区-组-品类|定制商品集-品牌） —— Dictionary<Guid, IEnumerable<BalePackAreaTreeInfo>> GetBalePackAreaTreeInfos(IEnumerable<Guid> packIds)

        /// <summary>
        /// 批量获取大包|定制套餐选区树（选区-组-品类|定制商品集-品牌）
        /// </summary>
        /// <param name="packIds">套餐Id集</param>
        /// <returns>套餐id|（选区-组-品类|定制商品集-品牌）</returns>
        [OperationContract]
        Dictionary<Guid, IEnumerable<BalePackAreaTreeInfo>> GetBalePackAreaTreeInfos(IEnumerable<Guid> packIds);

        #endregion

    }
}
