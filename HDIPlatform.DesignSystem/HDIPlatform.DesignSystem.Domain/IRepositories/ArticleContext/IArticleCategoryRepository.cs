
using HDIPlatform.DesignSystem.Domain.Entities.ArticleContext;
using ShSoft.Infrastructure.RepositoryBase;

namespace HDIPlatform.DesignSystem.Domain.IRepositories.ArticleContext
{
    /// <summary>
    /// 文章分类仓储接口
    /// </summary>
    public interface IArticleCategoryRepository : IRepository<ArticleCategory>
    {
        /// <summary>
        /// 是否存在名称相同的分类
        /// </summary>
        /// <param name="name">名称</param>
        /// <returns></returns>
        bool Exist(string name);
        /// <summary>
        /// 根据名称获取分类
        /// </summary>
        /// <param name="name">名称</param>
        /// <returns></returns>
        ArticleCategory GetByName(string name);

    }
}
