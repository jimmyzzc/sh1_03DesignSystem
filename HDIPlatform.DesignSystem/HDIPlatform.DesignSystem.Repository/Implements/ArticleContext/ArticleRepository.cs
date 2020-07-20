using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using HDIPlatform.DesignSystem.Domain.Entities.ArticleContext;
using HDIPlatform.DesignSystem.Domain.IRepositories.ArticleContext;
using ShSoft.Common.PoweredByLee;
using ShSoft.Infrastructure.Repository.EntityFramework;
using ShSoft.ValueObjects.Enums;

namespace HDIPlatform.DesignSystem.Repository.Implements.ArticleContext
{
    /// <summary>
    /// 文章仓储实现
    /// </summary>
    public class ArticleRepository : EFRepositoryProvider<Article>, IArticleRepository
    {
        /// <summary>
        /// 是否存在标题相同的原文
        /// </summary>
        /// <param name="name">标题</param>
        /// <returns></returns>
        public bool Exist(string name)
        {
            return this.Count(s => s.SourceId == null && s.PackId == null && s.Name == name) != 0;
        }

        /// <summary>
        /// 根据标题获取原文
        /// </summary>
        /// <param name="name">标题</param>
        /// <returns></returns>
        public Article GetByName(string name)
        {
            return this.SingleOrDefault(s => s.SourceId == null && s.PackId == null && s.Name == name);
        }

        #region # 分页获取文章集 ——   IEnumerable<Article> FindArticlesByPage(string keywords, Guid? categoryId,bool? isIssue, int pageIndex, int pageSize, out int rowCount, out int pageCount);

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
        public IEnumerable<Article> GetArticlesByPage(string keywords, Guid? categoryId, bool? isIssue, int pageIndex, int pageSize, out int rowCount, out int pageCount)
        {
            Expression<Func<Article, bool>> condition =
                s => (string.IsNullOrEmpty(keywords) || s.Keywords.Contains(keywords))
                     && (categoryId == null || s.ArticleCategory.Id == categoryId)
                     && (isIssue == null || s.IsIssue == isIssue)
                     && s.SourceId == null && s.PackId == null;
            IEnumerable<Article> articles = this.Find(condition).OrderBy(s => s.Sort).ThenByDescending(s => s.AddedTime);
            rowCount = articles.Count();
            pageCount = (int)Math.Ceiling(rowCount * 1.0 / pageSize);
            return articles.Skip((pageIndex - 1) * pageSize).Take(pageSize);
        }
        #endregion

        #region # 通过原文Id获取文章副本集 ——  IEnumerable<Article> GetArticlesBySourceId(Guid sourceId);

        /// <summary>
        /// 通过原文Id获取文章副本集
        /// </summary>
        /// <param name="sourceId">原文章Id</param>
        /// <returns>文章副本集</returns>
        public IEnumerable<Article> GetArticlesBySourceId(Guid sourceId)
        {
            return this.Find(s => s.SourceId == sourceId);
        }
        #endregion

        #region # 通过套餐Id集获取文章集 ——   Dictionary<Guid,IEnumerable<Article>> GetArticlesByPackIds(IEnumerable<Guid> packId);

        /// <summary>
        /// 通过套餐Id集获取文章集
        /// </summary>
        /// <param name="packIds">套餐Id集</param>
        /// <returns>文章集</returns>
        public Dictionary<Guid, IEnumerable<Article>> GetArticlesByPackIds(IList<Guid> packIds)
        {
            if (packIds.IsNullOrEmpty())
            {
                packIds = new List<Guid>();
            }
            var data = this.Find(s => s.PackId.HasValue && packIds.Contains(s.PackId.Value)).ToList();
            Dictionary<Guid, IEnumerable<Article>> result = new Dictionary<Guid, IEnumerable<Article>>();

            foreach (var item in packIds)
            {
                result.Add(item, data.Where(s => s.PackId == item));
            }
            return result;
        }
        #endregion

        #region # 通过套餐Id获取文章集 ——  IEnumerable<Article> GetArticlesByPackId(Guid packId);

        /// <summary>
        /// 通过套餐Id获取文章集
        /// </summary>
        /// <param name="packId">套餐Id</param>
        /// <returns>文章集</returns>
        public IEnumerable<Article> GetArticlesByPackId(Guid packId)
        {
            return this.Find(s => s.PackId == packId);
        }
        #endregion

        #region # 通过套餐空间Id获取文章集 ——        IEnumerable<Article> GetArticlesBySpaceId(Guid spaceId);

        /// <summary>
        /// 通过套餐空间Id获取文章集
        /// </summary>
        /// <param name="spaceId">套餐空间Id</param>
        /// <returns>文章集</returns>
        public IEnumerable<Article> GetArticlesBySpaceId(Guid spaceId)
        {
            return this.Find(s => s.SpaceId == spaceId);
        }
        #endregion
    }
}
