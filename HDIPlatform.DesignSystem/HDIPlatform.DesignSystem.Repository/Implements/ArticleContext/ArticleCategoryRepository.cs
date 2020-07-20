using HDIPlatform.DesignSystem.Domain.Entities.ArticleContext;
using HDIPlatform.DesignSystem.Domain.IRepositories.ArticleContext;
using ShSoft.Infrastructure.Repository.EntityFramework;

namespace HDIPlatform.DesignSystem.Repository.Implements.ArticleContext
{
    /// <summary>
    /// 文章分类仓储实现
    /// </summary>
    public class ArticleCategoryRepository : EFRepositoryProvider<ArticleCategory>, IArticleCategoryRepository
    {
        /// <summary>
        /// 是否存在名称相同的分类
        /// </summary>
        /// <param name="name">名称</param>
        /// <returns></returns>
        public bool Exist(string name)
        {
            return this.Count(s => s.Name == name) != 0;
        }

        /// <summary>
        /// 根据名称获取分类
        /// </summary>
        /// <param name="name">名称</param>
        /// <returns></returns>
        public ArticleCategory GetByName(string name)
        {
            return this.SingleOrDefault(s => s.Name == name);
        }

    }
}
