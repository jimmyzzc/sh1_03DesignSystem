using System;
using System.Collections.Generic;
using HDIPlatform.DesignSystem.Domain.Entities.BaleContext;
using ShSoft.Infrastructure.RepositoryBase;

namespace HDIPlatform.DesignSystem.Domain.IRepositories.BaleContext
{
    /// <summary>
    /// 套餐品类配置商品仓储接口
    /// </summary>
    public interface IBalePackProductRepository : IRepository<BalePackProduct>
    {

        #region 获取下架商品 —— IEnumerable<Guid> GetShelfOffProducts()

        /// <summary>
        /// 获取下架商品
        /// </summary>
        /// <returns></returns>
        IEnumerable<Guid> GetShelfOffProducts();
        #endregion


        #region 根据组Id品类Id获取品类已配置商品列表 —— IEnumerable<BalePackProduct> GetProductsByCategoryId(Guid groupId, Guid categoryId)

        /// <summary>
        /// 根据组Id品类Id获取品类已配置商品列表
        /// </summary>
        /// <param name="groupId">组Id</param>
        /// <param name="categoryId">品类Id</param>
        /// <returns>品类已配置商品列表</returns>
        IEnumerable<BalePackProduct> GetProductsByCategoryId(Guid groupId, Guid categoryId);

        #endregion

        #region 根据组Id品类Id获取品类已配置商品列表 —— IDictionary<Guid, bool> GetProducts(Guid groupId, Guid categoryId)

        /// <summary>
        /// 根据组Id品类Id获取品类已配置商品列表
        /// </summary>
        /// <param name="groupId">组Id</param>
        /// <param name="categoryId">品类Id</param>
        /// <returns>IDictionary[Guid, bool]，[商品Id, 是否已上架]</returns>
        IDictionary<Guid, bool> GetProducts(Guid groupId, Guid categoryId);

        #endregion

        #region 根据商品Id获取组列表 —— IEnumerable<BalePackGroup> GetBalePackGroupsByProId(Guid productId)

        /// <summary>
        /// 根据商品Id获取组列表
        /// </summary>
        /// <param name="productId">商品Id</param>
        /// <returns>组列表</returns>
        IEnumerable<BalePackGroup> GetBalePackGroupsByProId(Guid productId);
        #endregion

        #region 根据选区Id集获取商品列表 —— IEnumerable<BalePackProduct> GetProducts(IEnumerable<Guid> choiceAreaIds)

        /// <summary>
        /// 根据选区Id集获取商品列表
        /// </summary>
        /// <param name="choiceAreaIds">选区Id集</param>
        /// <returns>商品列表</returns>
        IEnumerable<BalePackProduct> GetProductsByAreaIds(IEnumerable<Guid> choiceAreaIds);
        #endregion

        #region 根据选区Id集获取下架商品列表 —— IEnumerable<BalePackProduct> GetShelfOffProductsByAreaIds(IEnumerable<Guid> choiceAreaIds)

        /// <summary>
        /// 根据选区Id集获取下架商品列表
        /// </summary>
        /// <param name="choiceAreaIds">选区Id集</param>
        /// <returns>下架商品列表</returns>
        IEnumerable<BalePackProduct> GetShelfOffProductsByAreaIds(IEnumerable<Guid> choiceAreaIds);
        #endregion

        #region 根据选区Id集获取下架商品个数 —— int GetShelfOffProductsByAreaIds(IEnumerable<Guid> choiceAreaIds)

        /// <summary>
        /// 根据选区Id集获取下架商品个数
        /// </summary>
        /// <param name="choiceAreaIds">选区Id集</param>
        /// <returns>下架商品列表</returns>
        int GetShelfOffProCountByAreaIds(IEnumerable<Guid> choiceAreaIds);
        #endregion

        #region 根据套餐选区组三级品类获取商品列表(包含下架商品) —— IEnumerable<BalePackProduct> GetProductsByPackId(IEnumerable<Guid> choiceAreaIds, Guid? groupId, Guid? categoryId);
        /// <summary>
        /// 根据套餐选区组三级品类获取商品列表(包含下架商品)
        /// </summary>
        /// <param name="choiceAreaIds">选区Id集</param>
        /// <param name="groupId">组Id</param>
        /// <param name="categoryId">品类Id</param>
        /// <returns>商品列表</returns>
        IEnumerable<Guid> GetProductIdsByPackId(IEnumerable<Guid> choiceAreaIds, Guid? groupId, Guid? categoryId);

        #endregion

        #region 根据商品Id获取组Id列表
        /// <summary>
        /// 根据商品Id获取组Id列表
        /// </summary>
        /// <param name="productId">商品Id</param>
        /// <returns>组Id列表</returns>
        IEnumerable<Guid> GetBalePackGroupIds(Guid productId);
        #endregion
    }
}
