using System;
using System.Collections.Generic;
using HDIPlatform.DesignSystem.Domain.Entities.BaleContext;
using ShSoft.Infrastructure.RepositoryBase;

namespace HDIPlatform.DesignSystem.Domain.IRepositories.BaleContext
{
    /// <summary>
    /// 大包套餐选区仓储接口
    /// </summary>
    public interface IBalePackChoiceAreaRepository : IRepository<BalePackChoiceArea>
    {
        #region 根据选区Id集获取套餐列表 —— IDictionary<Guid, string> GetPackByChoiceAreaIds(IEnumerable<Guid> choiceAreaIds)

        /// <summary>
        /// 根据选区Id集获取套餐列表
        /// </summary>
        /// <param name="choiceAreaIds">选区Id集</param>
        /// <returns>IDictionary[Guid, string]，[套餐Id, 套餐名称]</returns>
        /// <returns></returns>
        IDictionary<Guid, string> GetPackByChoiceAreaIds(IEnumerable<Guid> choiceAreaIds);

        #endregion

        #region 验证套餐内选区名称是否重复 —— bool ExistsChoiceAreaName(Guid packId, Guid? choiceAreaId, string choiceAreaName)

        /// <summary>
        /// 验证套餐内选区名称是否重复
        /// </summary>
        /// <param name="packId">套餐Id</param>
        /// <param name="choiceAreaId">选区Id</param>
        /// <param name="choiceAreaName">选区名称</param>
        /// <returns></returns>
        bool ExistsChoiceAreaName(Guid packId, Guid? choiceAreaId, string choiceAreaName);
        #endregion
    }
}
