using HDIPlatform.DesignSystem.Domain.Entities.BaleContext;
using HDIPlatform.DesignSystem.Domain.IRepositories.BaleContext;
using ShSoft.Infrastructure.Repository.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HDIPlatform.DesignSystem.Repository.Implements.BaleContext
{
    /// <summary>
    /// 套餐品类配置商品仓储实现
    /// </summary>
    public class BalePackProductRepository : EFRepositoryProvider<BalePackProduct>, IBalePackProductRepository
    {
        #region # 获取实体对象集合 —— override IQueryable<BalePackProduct> FindAllInner()
        /// <summary>
        /// 获取实体对象集合
        /// </summary>
        /// <returns>实体对象集合</returns>
        protected override IQueryable<BalePackProduct> FindAllInner()
        {
            return base.FindAllInner().Where(x => x.BalePackCategory != null && !x.BalePackCategory.Deleted && x.BalePackCategory.BalePackGroup != null && !x.BalePackCategory.BalePackGroup.Deleted);
        }
        #endregion

        #region 获取下架商品Id列表 —— IEnumerable<Guid> GetShelfOffProducts()

        /// <summary>
        /// 获取下架商品Id列表(排除克隆套餐的商品)
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Guid> GetShelfOffProducts()
        {
            return this.Find(s => !s.Shelved && !s.BalePackCategory.BalePackGroup.IsClone).Select(s => s.ProductId).Distinct().AsEnumerable();
        }
        #endregion

        #region 根据组Id品类Id获取品类已配置商品列表 —— IEnumerable<BalePackProduct> GetProductsByCategoryId(Guid groupId, Guid categoryId)

        /// <summary>
        /// 根据品类Id获取品类已配置商品列表
        /// </summary>
        /// <param name="groupId">组Id</param>
        /// <param name="categoryId">品类Id</param>
        /// <returns>品类已配置商品列表</returns>
        public IEnumerable<BalePackProduct> GetProductsByCategoryId(Guid groupId, Guid categoryId)
        {
            return base.Find(s => s.BalePackCategory.CategoryId == categoryId && s.BalePackCategory.BalePackGroup.Id == groupId).AsEnumerable();
        }
        #endregion

        #region 根据组Id品类Id获取品类已配置商品列表 —— IDictionary<Guid, bool> GetProducts(Guid groupId, Guid categoryId)

        /// <summary>
        /// 根据品类Id获取品类已配置商品列表
        /// </summary>
        /// <param name="groupId">组Id</param>
        /// <param name="categoryId">品类Id</param>
        /// <returns>IDictionary[Guid, bool]，[商品Id, 是否已上架]</returns>
        public IDictionary<Guid, bool> GetProducts(Guid groupId, Guid categoryId)
        {
            return
                base.Find(s => s.BalePackCategory.CategoryId == categoryId && s.BalePackCategory.BalePackGroup.Id == groupId)
                    .ToDictionary(s => s.ProductId, s => s.Shelved);
        }
        #endregion

        #region 根据商品Id获取组列表 —— IEnumerable<BalePackGroup> GetBalePackGroupsByProId(Guid productId)

        /// <summary>
        /// 根据商品Id获取组列表（排除克隆套餐）
        /// </summary>
        /// <param name="productId">商品Id</param>
        /// <returns>组列表</returns>
        public IEnumerable<BalePackGroup> GetBalePackGroupsByProId(Guid productId)
        {
            return this.Find(s => s.ProductId == productId && !s.BalePackCategory.BalePackGroup.IsClone).Select(s => s.BalePackCategory.BalePackGroup);
        }
        #endregion

        #region 根据选区Id集获取商品列表 —— IEnumerable<BalePackProduct> GetProducts(IEnumerable<Guid> choiceAreaIds)

        /// <summary>
        /// 根据选区Id集获取商品列表
        /// </summary>
        /// <param name="choiceAreaIds">选区Id集</param>
        /// <returns>商品列表</returns>
        public IEnumerable<BalePackProduct> GetProductsByAreaIds(IEnumerable<Guid> choiceAreaIds)
        {
            return base.Find(s => choiceAreaIds.Contains(s.BalePackCategory.BalePackGroup.ChoiceAreaId)).AsEnumerable();

        }
        #endregion

        #region 根据选区Id集获取下架商品列表 —— IEnumerable<BalePackProduct> GetShelfOffProductsByAreaIds(IEnumerable<Guid> choiceAreaIds)

        /// <summary>
        /// 根据选区Id集获取下架商品列表
        /// </summary>
        /// <param name="choiceAreaIds">选区Id集</param>
        /// <returns>下架商品列表</returns>
        public IEnumerable<BalePackProduct> GetShelfOffProductsByAreaIds(IEnumerable<Guid> choiceAreaIds)
        {
            return base.Find(s => choiceAreaIds.Contains(s.BalePackCategory.BalePackGroup.ChoiceAreaId) && !s.Shelved).AsEnumerable();

        }
        #endregion


        #region 根据选区Id集获取下架商品个数 —— int GetShelfOffProductsByAreaIds(IEnumerable<Guid> choiceAreaIds)

        /// <summary>
        /// 根据选区Id集获取下架商品个数
        /// </summary>
        /// <param name="choiceAreaIds">选区Id集</param>
        /// <returns>下架商品列表</returns>
        public int GetShelfOffProCountByAreaIds(IEnumerable<Guid> choiceAreaIds)
        {
            return base.Find(s => choiceAreaIds.Contains(s.BalePackCategory.BalePackGroup.ChoiceAreaId) && !s.Shelved).Select(s => s.ProductId).Distinct().Count();

        }
        #endregion

        #region 根据套餐选区组三级品类获取商品列表(包含下架商品) —— IEnumerable<BalePackProduct> GetProductsByPackId(IEnumerable<Guid> choiceAreaIds, Guid? groupId, Guid? categoryId)
        /// <summary>
        /// 根据套餐选区组三级品类获取商品列表(包含下架商品)
        /// </summary>
        /// <param name="choiceAreaIds">选区Id集</param>
        /// <param name="groupId">组Id</param>
        /// <param name="categoryId">品类Id</param>
        /// <returns>商品列表</returns>
        public IEnumerable<Guid> GetProductIdsByPackId(IEnumerable<Guid> choiceAreaIds, Guid? groupId, Guid? categoryId)
        {
            return base.Find(s => choiceAreaIds.Contains(s.BalePackCategory.BalePackGroup.ChoiceAreaId) &&
               (categoryId == null || s.BalePackCategory.CategoryId == categoryId) &&
                (groupId == null || s.BalePackCategory.BalePackGroup.Id == groupId)).Select(s => s.ProductId).AsEnumerable();
        }
        #endregion

        #region 根据商品Id获取组Id列表

        /// <summary>
        /// 根据商品Id获取组Id列表
        /// </summary>
        /// <param name="productId">商品Id</param>
        /// <returns>组Id列表</returns>
        public IEnumerable<Guid> GetBalePackGroupIds(Guid productId)
        {
            return this.Find(s => s.ProductId == productId).Select(s => s.BalePackCategory.BalePackGroup.Id).ToList();
        }
        #endregion
    }
}
