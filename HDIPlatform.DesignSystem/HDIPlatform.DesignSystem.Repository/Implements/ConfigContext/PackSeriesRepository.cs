using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using HDIPlatform.DesignSystem.Domain.Entities.ConfigContext;
using HDIPlatform.DesignSystem.Domain.IRepositories.ConfigContext;
using ShSoft.Common.PoweredByLee;
using ShSoft.Infrastructure.Repository.EntityFramework;

namespace HDIPlatform.DesignSystem.Repository.Implements.ConfigContext
{
    /// <summary>
    /// 套餐系列仓储实现
    /// </summary>
    public class PackSeriesRepository : EFRepositoryProvider<PackSeries>, IPackSeriesRepository
    {

        #region 获取最大序号

        /// <summary>
        /// 获取最大序号
        /// </summary>
        /// <returns>最大序号</returns>
        public int GetMaxSort()
        {
            var packSeries = base.FindAllInner();
            if (packSeries.IsNullOrEmpty())
            {
                return 0;
            }

            return packSeries.Max(s => s.Sort);
        }

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
        public IEnumerable<PackSeries> FindByPage(string keywords, int pageIndex,int  pageSize, out int rowCount, out int pageCount)
        {
            Expression<Func<PackSeries, bool>> condition = x => string.IsNullOrEmpty(keywords) || x.Keywords.Contains(keywords);

            var packSeries = base.Find(condition).OrderBy(s=>s.Sort);
            rowCount = packSeries.Count();
            pageCount = (int)Math.Ceiling(rowCount * 1.0 / pageSize);
            return packSeries.Skip((pageIndex - 1) * pageSize).Take(pageSize);

        }

        #endregion
    }
}
