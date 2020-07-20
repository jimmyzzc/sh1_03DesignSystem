using System;
using System.Collections.Generic;
using HDIPlatform.DesignSystem.Domain.Entities.BaleContext;
using ShSoft.Infrastructure.RepositoryBase;

namespace HDIPlatform.DesignSystem.Domain.IRepositories.BaleContext
{
    /// <summary>
    /// 套餐选区内组仓储接口
    /// </summary>
    public interface IBalePackGroupRepository : IRepository<BalePackGroup>
    {
        #region # 根据选区Id获取选区内组列表 —— IEnumerable<BalePackGroup> FindByChoiceAreaId(Guid choiceAreaId)

        /// <summary>
        /// 根据选区Id获取选区内组列表
        /// </summary>
        /// <param name="choiceAreaId">选区Id</param>
        /// <returns>选区内组列表</returns>
        IEnumerable<BalePackGroup> FindByChoiceAreaId(Guid choiceAreaId);

        #endregion

        #region # 判断选区内组名称是否存在 —— bool ExistsGroupName(Guid choiceAreaId, Guid? groupId, string groupName)

        /// <summary>
        /// 判断选区内组名称是否存在
        /// </summary>
        /// <param name="choiceAreaId">选区Id</param>
        /// <param name="groupId">组Id</param>
        /// <param name="groupName">组名称</param>
        /// <returns></returns>
        bool ExistsGroupName(Guid choiceAreaId, Guid? groupId, string groupName);
        #endregion

        #region # 根据选区Id集获取组列表 —— IEnumerable<BalePackGroup> GetBalePackGroupByChoiceAreaIds(IEnumerable<Guid> choiceAreaIds)

        /// <summary>
        /// 根据选区Id集获取组列表
        /// </summary>
        /// <param name="choiceAreaIds">选区Id集</param>
        /// <returns>组列表</returns>
        IEnumerable<BalePackGroup> GetGroupByChoiceAreaIds(IEnumerable<Guid> choiceAreaIds);
        #endregion

        #region # 验证选区内组是否包含品类 —— bool ExistsCategory(IEnumerable<Guid> choiceAreaIds)
        /// <summary>
        /// 验证选区内组是否包含品类
        /// </summary>
        /// <param name="choiceAreaIds">选区Id集</param>
        /// <returns></returns>
        bool ExistsCategory(IEnumerable<Guid> choiceAreaIds);
        #endregion


        #region # 验证选区组内品类是否包含商品 ——  bool ExistsProduct(IEnumerable<Guid> choiceAreaIds)
        /// <summary>
        /// 验证选区组内品类是否包含商品
        /// </summary>
        /// <param name="choiceAreaIds">选区Id集</param>
        /// <returns></returns>
        bool ExistsProduct(IEnumerable<Guid> choiceAreaIds);
        #endregion

        #region # 验证大包套餐是否可以上架  —— bool IsBalePackShelfed(IEnumerable<Guid> choiceAreaIds)
        /// <summary>
        /// 验证大包套餐是否可以上架
        /// </summary>
        /// <param name="choiceAreaIds">选区Id集</param>
        /// <returns></returns>
        bool IsBalePackShelfed(IEnumerable<Guid> choiceAreaIds);
        #endregion

        #region # 验证定制套餐是否可以上架 —— bool IsCustomizedPackShelfed(IEnumerable<Guid> choiceAreaIds)
        /// <summary>
        /// 验证定制套餐是否可以上架
        /// </summary>
        /// <param name="choiceAreaIds">选区Id集</param>
        /// <returns></returns>
        bool IsCustomizedPackShelfed(IEnumerable<Guid> choiceAreaIds);
        #endregion

        #region # 根据选区Id集获取三级品类Id集 —— IEnumerable<Guid> GetCategoryIds(IEnumerable<Guid> choiceAreaIds)
        /// <summary>
        /// 根据选区Id集获取三级品类Id集
        /// </summary>
        /// <param name="choiceAreaIds"></param>
        /// <returns></returns>
        IEnumerable<Guid> GetCategoryIds(IEnumerable<Guid> choiceAreaIds);

        #endregion

    }
}
