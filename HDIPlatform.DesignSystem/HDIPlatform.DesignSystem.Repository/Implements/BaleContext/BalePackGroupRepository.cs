using HDIPlatform.DesignSystem.Domain.Entities.BaleContext;
using HDIPlatform.DesignSystem.Domain.IRepositories.BaleContext;
using ShSoft.Infrastructure.Repository.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HDIPlatform.DesignSystem.Repository.Implements.BaleContext
{
    /// <summary>
    /// 套餐选区内组仓储实现
    /// </summary>
    public class BalePackGroupRepository : EFRepositoryProvider<BalePackGroup>, IBalePackGroupRepository
    {

        #region # 根据选区Id获取选区内组列表 —— IEnumerable<BalePackGroup> FindByChoiceAreaId(Guid choiceAreaId)

        /// <summary>
        /// 根据选区Id获取选区内组列表
        /// </summary>
        /// <param name="choiceAreaId">选区Id</param>
        /// <returns>选区内组列表</returns>
        public IEnumerable<BalePackGroup> FindByChoiceAreaId(Guid choiceAreaId)
        {
            return base.Find(s => s.ChoiceAreaId == choiceAreaId).AsEnumerable();
        }
        #endregion

        #region # 判断选区内组名称是否存在 —— bool ExistsGroupName(Guid choiceAreaId, Guid? groupId, string groupName)

        /// <summary>
        /// 判断选区内组名称是否存在
        /// </summary>
        /// <param name="choiceAreaId">选区Id</param>
        /// <param name="groupId">组Id</param>
        /// <param name="groupName">组名称</param>
        /// <returns></returns>
        public bool ExistsGroupName(Guid choiceAreaId, Guid? groupId, string groupName)
        {

            return base.Exists(s => s.ChoiceAreaId == choiceAreaId && (groupId == null || s.Id != groupId) && s.Name == groupName);
        }
        #endregion

        #region # 根据选区Id集获取组列表 —— IEnumerable<BalePackGroup> GetBalePackGroupByChoiceAreaIds(IEnumerable<Guid> choiceAreaIds)

        /// <summary>
        /// 根据选区Id集获取组列表
        /// </summary>
        /// <param name="choiceAreaIds">选区Id集</param>
        /// <returns></returns>
        public IEnumerable<BalePackGroup> GetGroupByChoiceAreaIds(IEnumerable<Guid> choiceAreaIds)
        {

            return base.Find(s => choiceAreaIds.Contains(s.ChoiceAreaId));
        }
        #endregion


        #region # 验证选区是否包含组

        /// <summary>
        /// 验证选区是否包含组
        /// </summary>
        /// <param name="choiceAreaIds">选区Id集</param>
        /// <returns></returns>
        public bool ExistsGroup(IEnumerable<Guid> choiceAreaIds)
        {
            IQueryable<BalePackGroup> groups = this.Find(s => choiceAreaIds.Contains(s.ChoiceAreaId));

            return groups.Count() >= choiceAreaIds.Count();
        }
        #endregion


        #region # 验证选区内组是否包含品类

        /// <summary>
        /// 验证选区内组是否包含品类
        /// </summary>
        /// <param name="choiceAreaIds">选区Id集</param>
        /// <returns></returns>
        public bool ExistsCategory(IEnumerable<Guid> choiceAreaIds)
        {
            IQueryable<BalePackGroup> groups = this.Find(s => choiceAreaIds.Contains(s.ChoiceAreaId));

            return groups.All(s => s.BalePackCategorys.Any());
        }
        #endregion


        #region # 验证选区组内品类是否包含商品

        /// <summary>
        /// 验证选区组内品类是否包含商品
        /// </summary>
        /// <param name="choiceAreaIds">选区Id集</param>
        /// <returns></returns>
        public bool ExistsProduct(IEnumerable<Guid> choiceAreaIds)
        {
            IQueryable<BalePackGroup> groups = this.Find(s => choiceAreaIds.Contains(s.ChoiceAreaId));
            IQueryable<BalePackCategory> categorys = groups.SelectMany(s => s.BalePackCategorys);

            return categorys.All(s => s.BalePackProducts.Any());
        }

        #endregion


        #region # 验证大包套餐是否可以上架
        /// <summary>
        /// 验证大包套餐是否可以上架
        /// </summary>
        /// <param name="choiceAreaIds">选区Id集</param>
        /// <returns></returns>
        public bool IsBalePackShelfed(IEnumerable<Guid> choiceAreaIds)
        {
            //套餐是否包含选区
            bool isShelfed = choiceAreaIds.Any();
            if (isShelfed)
            {
                IQueryable<BalePackGroup> groups = this.Find(s => choiceAreaIds.Contains(s.ChoiceAreaId));
                //选区是否包含组
                isShelfed = groups.Count() >= choiceAreaIds.Count();
                //组内是否包含品类
                if (isShelfed)
                    isShelfed = groups.All(s => s.BalePackCategorys.Any());
            }
            return isShelfed;
        }
        #endregion

        #region # 验证定制套餐是否可以上架
        /// <summary>
        /// 验证定制套餐是否可以上架
        /// </summary>
        /// <param name="choiceAreaIds">选区Id集</param>
        /// <returns></returns>
        public bool IsCustomizedPackShelfed(IEnumerable<Guid> choiceAreaIds)
        {
            //套餐是否包含选区
            bool isShelfed = choiceAreaIds.Any();
            if (isShelfed)
            {
                //选区是否包含组
                IQueryable<BalePackGroup> groups = this.Find(s => choiceAreaIds.Contains(s.ChoiceAreaId));
                isShelfed = groups.Count() >= choiceAreaIds.Count();
                //组内是否包含品类
                if (isShelfed)
                    isShelfed = groups.All(s => s.BalePackCategorys.Any());
                if (isShelfed)
                {
                    IQueryable<BalePackCategory> categorys = groups.SelectMany(s => s.BalePackCategorys);
                    isShelfed = categorys.All(s => s.BalePackProducts.Any());
                }
            }
            return isShelfed;
        }
        #endregion


        #region # 根据选区Id集获取三级品类Id集 —— IEnumerable<Guid> GetCategoryIds(IEnumerable<Guid> choiceAreaIds)
        /// <summary>
        /// 根据选区Id集获取三级品类Id集
        /// </summary>
        /// <param name="choiceAreaIds"></param>
        /// <returns></returns>
        public IEnumerable<Guid> GetCategoryIds(IEnumerable<Guid> choiceAreaIds)
        {
            IQueryable<BalePackGroup> groups = this.Find(s => choiceAreaIds.Contains(s.ChoiceAreaId));
            IEnumerable<Guid> categoryIds = groups.SelectMany(x => x.BalePackCategorys).Select(x => x.CategoryId);
            return categoryIds;
        }

        #endregion
    }
}
