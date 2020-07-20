using System;
using System.Collections.Generic;
using System.Linq;
using HDIPlatform.DesignSystem.Domain.Entities.BaleContext;
using HDIPlatform.DesignSystem.Domain.EventSources.BaleContext;
using ShSoft.Common.PoweredByLee;
using ShSoft.Infrastructure.EntityBase;
using ShSoft.Infrastructure.EventBase.Mediator;

namespace HDIPlatform.DesignSystem.Domain.Entities.ArticleContext
{
    /// <summary>
    /// 文章分类
    /// </summary>
    [Serializable]
    public class ArticleCategory : AggregateRootEntity
    {
        #region # 构造器

        #region 01.无参构造器

        /// <summary>
        /// 无参构造器
        /// </summary>
        protected ArticleCategory()
        {
          this.Articles = new HashSet<Article>();
        }
        #endregion


        #region 02.创建文章分类构造器

        /// <summary>
        /// 创建文章分类构造器
        /// </summary>
        /// <param name="name">分类名称</param>
        /// <param name="sort">排序</param>
        public ArticleCategory(string name,int sort)
            : this()
        {
            this.Name = name;
            this.Sort = sort;
        }
        #endregion

        #endregion

        #region # 属性

        #region 导航属性 - 文章集 ——  ICollection<Article> Articles
        /// <summary>
        /// 导航属性 - 文章集
        /// </summary>
        public virtual ICollection<Article> Articles { get; private set; }
        #endregion

        #endregion

        #region # 方法

        #region 修改分类 —— void Modify(string name , int sort)

        /// <summary>
        /// 修改分类
        /// </summary>
        /// <param name="name">分类名称</param>
        /// <param name="sort">排序</param>
        public void Modify(string name, int sort)
        {
            this.Name = name;
            this.Sort = sort;
        }
        #endregion

        #region 添加文章 —— void AddArticle(Article article)

        /// <summary>
        /// 添加文章
        /// </summary>
        /// <param name="article">文章</param>
        public void AddArticle(Article article)
        {
            if (this.Articles.Any(s=>s.Id == article.Id))
            {
                throw new InvalidOperationException("操作失败 , 相同Id的文章已存在 , 不可重复添加 !");
            }
            this.Articles.Add(article);
        }
        #endregion

        #region 删除文章 —— void RemoveArticle(Guid articleId)

        /// <summary>
        /// 删除文章
        /// </summary>
        /// <param name="articleId">文章Id</param>
        public void RemoveArticle(Guid articleId)
        {
            Article article = this.Articles.SingleOrDefault(s => s.Id == articleId);
            if (article == null)
            {
                throw new InvalidOperationException("操作失败 , 文章不存在 , 无法删除 !");
            }
            this.Articles.Remove(article);
        }
        #endregion

        #endregion
    }
}
