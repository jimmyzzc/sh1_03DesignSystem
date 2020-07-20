using System.Linq;
using HDIPlatform.DesignSystem.Domain.Entities.ArticleContext;
using HDIPlatform.DesignSystem.IAppService.DTOs.Outputs.ArticleContext;
using ShSoft.Common.PoweredByLee;

namespace HDIPlatform.DesignSystem.AppService.Maps
{
    /// <summary>
    /// 大包/定制套餐相关映射工具类
    /// </summary>
    public static class ArticleMap
    {

        #region 文章分类映射 —— static ArticleCategoryInfo ToDTO(this ArticleCategory articleCategory)

        /// <summary>
        ///  文章分类映射
        /// </summary>
        /// <param name="articleCategory"></param>
        /// <returns></returns>
        public static ArticleCategoryInfo ToDTO(this ArticleCategory articleCategory)
        {
            if (articleCategory == null)
            {
                return null;
            }
            ArticleCategoryInfo info = Transform<ArticleCategory, ArticleCategoryInfo>.Map(articleCategory);
            info.ArticleCount = articleCategory.Articles.Count(s=>s.SourceId == null && s.PackId == null);
            return info;
        }

        #endregion

        #region 文章映射 —— static ArticleInfo ToDTO(this Article article)

        /// <summary>
        /// 文章映射
        /// </summary>
        /// <param name="article"></param>
        /// <returns></returns>
        public static ArticleInfo ToDTO(this Article article)
        {

            if (article == null)
            {
                return null;
            }
            ArticleInfo info = Transform<Article, ArticleInfo>.Map(article);
            info.CategoryId = article.ArticleCategory.Id;
            info.CategoryName = article.ArticleCategory.Name;
            return info;
        }
        #endregion


    }
}
