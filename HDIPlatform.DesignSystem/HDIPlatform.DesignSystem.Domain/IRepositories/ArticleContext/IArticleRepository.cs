
using System;
using System.Collections.Generic;
using HDIPlatform.DesignSystem.Domain.Entities.ArticleContext;
using ShSoft.Infrastructure.RepositoryBase;
using ShSoft.ValueObjects.Enums;

namespace HDIPlatform.DesignSystem.Domain.IRepositories.ArticleContext
{
    /// <summary>
    /// 文章仓储接口
    /// </summary>
    public interface IArticleRepository : IRepository<Article>
    {
        /// <summary>
        /// 是否存在标题相同的原文
        /// </summary>
        /// <param name="name">标题</param>
        /// <returns></returns>
        bool Exist(string name);

        /// <summary>
        /// 根据标题获取原文
        /// </summary>
        /// <param name="name">标题</param>
        /// <returns></returns>
        Article GetByName(string name);

        #region # 分页获取文章集 ——   IEnumerable<Article> GetArticlesByPage(string keywords, Guid? categoryId,bool? isIssue, int pageIndex, int pageSize, out int rowCount, out int pageCount);

        /// <summary>
        /// 分页获取文章集
        /// </summary>
        /// <param name="keywords">关键字</param>
        /// <param name="categoryId">分类Id</param>
        /// <param name="isIssue">是否已发布</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页容量</param>
        /// <param name="rowCount">总记录条数</param>
        /// <param name="pageCount">总页数</param>
        /// <returns>文章集</returns>
        IEnumerable<Article> GetArticlesByPage(string keywords, Guid? categoryId, bool? isIssue, int pageIndex, int pageSize, out int rowCount, out int pageCount);
        #endregion

        #region # 通过原文Id获取文章副本集 ——  IEnumerable<Article> GetArticlesBySourceId(Guid sourceId);

        /// <summary>
        /// 通过原文Id获取文章副本集
        /// </summary>
        /// <param name="sourceId">原文章Id</param>
        /// <returns>文章副本集</returns>
        IEnumerable<Article> GetArticlesBySourceId(Guid sourceId);
        #endregion

        #region # 通过套餐Id集获取文章集 ——  Dictionary<Guid, IEnumerable<Article>> GetArticlesByPackIds(IList<Guid> packIds)

        /// <summary>
        /// 通过套餐Id集获取文章集
        /// </summary>
        /// <param name="packIds">套餐Id集</param>
        /// <returns>文章集</returns>
        Dictionary<Guid, IEnumerable<Article>> GetArticlesByPackIds(IList<Guid> packIds);
        #endregion
        #region # 通过套餐空间Id获取文章集 ——        IEnumerable<Article> GetArticlesBySpaceId(Guid spaceId);

        /// <summary>
        /// 通过套餐空间Id获取文章集
        /// </summary>
        /// <param name="spaceId">套餐空间Id</param>
        /// <returns>文章集</returns>
        IEnumerable<Article> GetArticlesBySpaceId(Guid spaceId);
        #endregion


        #region # 通过套餐Id获取文章集 ——  IEnumerable<Article> GetArticlesByPackId(Guid packId);

        /// <summary>
        /// 通过套餐Id获取文章集
        /// </summary>
        /// <param name="packId">套餐Id</param>
        /// <returns>文章集</returns>
        IEnumerable<Article> GetArticlesByPackId(Guid packId);

        #endregion

    }
}
