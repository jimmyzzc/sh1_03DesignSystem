using HDIPlatform.DesignSystem.Domain.Entities.ConfigContext;
using ShSoft.Infrastructure.RepositoryBase;
using System.Collections.Generic;

namespace HDIPlatform.DesignSystem.Domain.IRepositories.ConfigContext
{
    /// <summary>
    /// 套餐系列仓储接口
    /// </summary>
    public interface IPackSeriesRepository : IRepository<PackSeries>
    {
        #region 获取最大序号

        /// <summary>
        /// 获取最大序号
        /// </summary>
        /// <returns>最大序号</returns>
        int GetMaxSort();

        #endregion


        #region # 分页获取套餐系列列表 —— IEnumerable<PackSeries> FindByPage(string keywords, int pageIndex,int  pageSize, out int rowCount, out int pageCount)

        /// <summary>
        /// 分页获取套餐系列列表
        /// </summary>
        /// <param name="keywords">关键字</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页容量</param>
        /// <param name="rowCount">总记录条数</param>
        /// <param name="pageCount">总页数</param>
        /// <returns>套餐系列列表</returns>
        IEnumerable<PackSeries> FindByPage(string keywords, int pageIndex, int pageSize, out int rowCount, out int pageCount);
        #endregion

    }
}
