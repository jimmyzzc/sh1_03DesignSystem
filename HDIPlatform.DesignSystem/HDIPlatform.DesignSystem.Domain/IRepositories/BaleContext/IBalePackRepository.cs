using HDIPlatform.DesignSystem.Domain.Entities.BaleContext;
using ShSoft.Infrastructure.RepositoryBase;
using ShSoft.ValueObjects.Enums;
using System;
using System.Collections.Generic;

namespace HDIPlatform.DesignSystem.Domain.IRepositories.BaleContext
{
    /// <summary>
    /// 大包/定制套餐仓储接口
    /// </summary>
    public interface IBalePackRepository : IRepository<BalePack>
    {

        #region # 根据套餐状态标签分页获取大包套餐列表 —— IEnumerable<BalePack> FindBalePackByStatusLabel(string keywords...

        /// <summary>
        /// 根据套餐状态标签分页获取大包套餐列表
        /// </summary>
        /// <param name="keywords">关键字</param>
        /// <param name="status">套餐状态</param>
        /// <param name="labels">套餐标签</param>
        /// <param name="sort">排序条件 true为倒序</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页容量</param>
        /// <param name="rowCount">总记录条数</param>
        /// <param name="pageCount">总页数</param>
        /// <returns>大包套餐列表</returns>
        IEnumerable<BalePack> FindBalePackByPage(string keywords, ShelfStatus? status, IList<string> labels, Dictionary<string, bool> sort, int pageIndex, int pageSize, out int rowCount, out int pageCount);
        #endregion

        #region # 根据套餐状态标签分页获取定制套餐列表 —— IEnumerable<BalePack> FindCustPackByStatusLabel(string keywords...

        /// <summary>
        /// 根据套餐状态标签分页获取定制套餐列表
        /// </summary>
        /// <param name="keywords">关键字</param>
        /// <param name="status">套餐状态</param>
        /// <param name="labels">套餐标签</param>
        /// <param name="sort">排序条件 true为倒序</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页容量</param>
        /// <param name="rowCount">总记录条数</param>
        /// <param name="pageCount">总页数</param>
        /// <returns>定制套餐列表</returns>
        IEnumerable<BalePack> FindCustPackByPage(string keywords, ShelfStatus? status, IList<string> labels, Dictionary<string, bool> sort, int pageIndex, int pageSize, out int rowCount, out int pageCount);
        #endregion

        #region # 根据套餐状态标签分页获取关联大包套餐列表 —— IEnumerable<BalePack> FindRelateBalePackByPage(string keywords...

        /// <summary>
        /// 根据套餐状态标签分页获取关联大包套餐列表
        /// </summary>
        /// <param name="type">套餐类型</param>
        /// <param name="keywords">关键字</param>
        /// <param name="status">套餐状态</param>
        /// <param name="labels">套餐标签</param>
        /// <param name="relateBaleIds">已关联大包套餐Id集</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页容量</param>
        /// <param name="rowCount">总记录条数</param>
        /// <param name="pageCount">总页数</param>
        /// <returns>大包套餐列表</returns>
        IEnumerable<BalePack> FindRelateBalePackByPage(BalePackType? type, string keywords, ShelfStatus? status, IList<string> labels, IEnumerable<Guid> relateBaleIds, int pageIndex, int pageSize, out int rowCount, out int pageCount);
        #endregion

        #region # 根据套餐状态标签分页获取未关联大包套餐列表 —— IEnumerable<BalePack> FindNoRelateBalePackByPage(string keywords...

        /// <summary>
        /// 根据套餐状态标签分页获取未关联大包套餐列表
        /// </summary>
        /// <param name="type">套餐类型</param>
        /// <param name="keywords">关键字</param>
        /// <param name="status">套餐状态</param>
        /// <param name="labels">套餐标签</param>
        /// <param name="relateBaleIds">已关联大包套餐Id集</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页容量</param>
        /// <param name="rowCount">总记录条数</param>
        /// <param name="pageCount">总页数</param>
        /// <returns>大包套餐列表</returns>
        IEnumerable<BalePack> FindNoRelateBalePackByPage(BalePackType? type, string keywords, ShelfStatus? status, IList<string> labels, IEnumerable<Guid> relateBaleIds, int pageIndex, int pageSize, out int rowCount, out int pageCount);
        #endregion

        #region # 根据源套餐Id获取套餐当前版本及Id ——  BalePack FindBalePackVersionNumber(Guid sourcePackId)
        /// <summary>
        /// 根据源套餐Id获取套餐当前版本及Id
        /// </summary>
        /// <param name="sourcePackId">源套餐Id</param>
        /// <returns></returns>
        BalePack FindBalePackVersionNumber(Guid sourcePackId);
        #endregion

        #region # 根据源套餐Id集获取套餐最新版本集 ——   IEnumerable<BalePack> FindBaleVersionNumbers(IEnumerable<Guid> sourcePackIds);
        /// <summary>
        /// 根据源套餐Id集获取套餐最新版本集
        /// </summary>
        /// <param name="sourcePackIds">源套餐Id集</param>
        /// <returns></returns>
        IEnumerable<BalePack> FindBaleVersionNumbers(IEnumerable<Guid> sourcePackIds);
        #endregion


        #region # 验证套餐名称是否重复 —— bool ExistsPackName(Guid? packId, string packName, BalePackType type)

        /// <summary>
        /// 验证套餐名称是否重复
        /// </summary>
        /// <param name="packId">套餐Id</param>
        /// <param name="packName">套餐名称</param>
        /// <param name="type">套餐类型</param>
        /// <returns></returns>
        bool ExistsPackName(Guid? packId, string packName, BalePackType type);

        #endregion

        #region # 根据套餐Id获取套餐内所有选区Id列表 —— IEnumerable<Guid> GetChoiceAreaIdsByPackId(Guid packId)

        /// <summary>
        /// 根据套餐Id获取套餐内所有选区Id列表
        /// </summary>
        /// <param name="packId">套餐Id</param>
        /// <returns></returns>
        IEnumerable<Guid> GetChoiceAreaIdsByPackId(Guid packId);
        #endregion
    }
}
