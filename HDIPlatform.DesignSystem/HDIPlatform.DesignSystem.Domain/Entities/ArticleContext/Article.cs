using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HDIPlatform.DesignSystem.Domain.Entities.BaleContext;
using HDIPlatform.DesignSystem.Domain.EventSources.BaleContext;
using ShSoft.Common.PoweredByLee;
using ShSoft.Infrastructure.EntityBase;
using ShSoft.Infrastructure.EventBase.Mediator;

namespace HDIPlatform.DesignSystem.Domain.Entities.ArticleContext
{
    /// <summary>
    /// 文章
    /// </summary>
    [Serializable]
    public class Article : AggregateRootEntity
    {
        #region # 构造器

        #region 01.无参构造器

        /// <summary>
        /// 无参构造器
        /// </summary>
        protected Article()
        {

        }
        #endregion

        #region 02.创建文章分类构造器

        /// <summary>
        /// 创建文章分类构造器
        /// </summary>
        /// <param name="name">分类名称</param>
        /// <param name="tags"></param>
        /// <param name="content"></param>
        /// <param name="author"></param>
        /// <param name="authorId"></param>
        /// <param name="isIssue"></param>
        /// <param name="articleCategory"></param>
        /// <param name="sourceId"></param>
        /// <param name="packId"></param>
        /// <param name="spaceId"></param>
        public Article(string name, string content, string author, Guid authorId, List<string> tags, bool isIssue, ArticleCategory articleCategory, Guid? sourceId = null, Guid? packId = null, Guid? spaceId = null)
            : this()
        {
            this.Name = name;
            this.Content = content;
            this.Author = author;
            this.AuthorId = authorId;
            this.TagsJson = tags.IsNullOrEmpty() ? "" : tags.ToJson();
            this.SourceId = sourceId;
            this.PackId = packId;
            this.SpaceId = spaceId;
            this.IsIssue = isIssue;
            // this.ArticleCategory = articleCategory;
            if (isIssue)
            {
                this.IssueTime = DateTime.Now;
            }
            content = System.Web.HttpUtility.UrlDecode(content, System.Text.Encoding.GetEncoding("UTF-8"))
                .FilterHtml();
            this.SetKeywords(name + content + tags.ToJson() + articleCategory.Name);
        }
        #endregion

        #endregion

        #region # 属性
        #region 文章内容 ——  string Content
        /// <summary>
        /// 文章内容
        /// </summary>
        public string Content { get; private set; }
        #endregion

        #region 是否已发布 —— bool IsIssued
        /// <summary>
        /// 是否已发布
        /// </summary>
        public bool IsIssue { get; private set; }
        #endregion

        #region 发布时间 —— DateTime IssueTime
        /// <summary>
        /// 发布时间
        /// </summary>
        public DateTime? IssueTime { get; private set; }
        #endregion

        #region 作者 —— string Author
        /// <summary>
        /// 作者
        /// </summary>
        public string Author { get; private set; }
        #endregion

        #region 作者Id —— Guid AuthorId
        /// <summary>
        /// 作者Id
        /// </summary>
        public Guid AuthorId { get; private set; }
        #endregion

        #region 标签集json ——string TagsJson
        /// <summary>
        /// 标签集json
        /// </summary>
        public string TagsJson { get; private set; }
        #endregion

        #region 标签集 —— List<string> Tags

        /// <summary>
        /// 标签集
        /// </summary>
        public List<string> Tags
        {
            get
            {
                if (string.IsNullOrEmpty(TagsJson))
                {
                    return new List<string>();
                }

                return TagsJson.JsonToObject<List<string>>();
            }
        }

        #endregion


        #region 原文章Id(为空表示源文,不为空表示套餐关联的副本) —— Guid? SourceId
        /// <summary>
        /// 原文章Id(为空表示源文,不为空表示套餐关联的副本)
        /// </summary>
        public Guid? SourceId { get; private set; }
        #endregion

        #region 关联套餐Id ——  Guid? PackId
        /// <summary>
        /// 关联套餐Id
        /// </summary>
        public Guid? PackId { get; private set; }
        #endregion

        #region 关联套餐空间Id ——  Guid? SpaceId
        /// <summary>
        /// 关联套餐空间Id
        /// </summary>
        public Guid? SpaceId { get; private set; }
        #endregion

        #region 导航属性 - 文章分类 —— ArticleCategory ArticleCategory
        /// <summary>
        /// 导航属性 - 文章分类
        /// </summary>
        public virtual ArticleCategory ArticleCategory { get; private set; }
        #endregion

        #endregion

        #region # 方法

        /// <summary>
        /// 修改文章状态
        /// </summary>
        /// <param name="isIssue">是否发布</param>
        public void ModifyState(bool isIssue)
        {
            this.IsIssue = isIssue;
            if (isIssue)
            {
                this.IssueTime = DateTime.Now;
            }
        }

        /// <summary>
        /// 修改文章
        /// </summary>
        /// <param name="tags"></param>
        /// <param name="isIssue">是否发布</param>
        /// <param name="name"></param>
        /// <param name="content"></param>
        /// <param name="articleCategory"></param>
        public void Modify(string name, string content, List<string> tags, bool isIssue, ArticleCategory articleCategory)
        {
            this.Name = name;
            this.Content = content;
            this.TagsJson = tags.ToJson();
            //this.ArticleCategory = articleCategory;
            this.IsIssue = isIssue;
            if (isIssue)
            {
                this.IssueTime = DateTime.Now;
            }

            content = System.Web.HttpUtility.UrlDecode(content, System.Text.Encoding.GetEncoding("UTF-8"))
                .FilterHtml();
            this.SetKeywords(name + content + tags.ToJson() + articleCategory.Name);
        }


        /// <summary>
        /// 删除文章
        /// </summary>
        public void Remove()
        {
            this.ArticleCategory = null;
        }

        /// <summary>
        /// 设置套餐
        /// </summary>
        public void SetPackId(Guid packId)
        {
            this.PackId = packId;
        }
        /// <summary>
        /// 设置套餐空间
        /// </summary>
        public void SetSpaceId(Guid spaceId)
        {
            this.SpaceId = spaceId;
        }

        /// <summary>
        /// 设置文章分类
        /// </summary>
        public void SetArticleCategory(ArticleCategory articleCategory)
        {
            this.ArticleCategory = articleCategory;
        }

        #endregion
    }
}
