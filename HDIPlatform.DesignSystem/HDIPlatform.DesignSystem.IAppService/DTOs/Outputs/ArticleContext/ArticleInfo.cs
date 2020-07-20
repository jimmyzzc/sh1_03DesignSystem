using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using HDIPlatform.DesignSystem.IAppService.DTOs.Outputs.BaleContext;
using ShSoft.Infrastructure.DTOBase;

namespace HDIPlatform.DesignSystem.IAppService.DTOs.Outputs.ArticleContext
{
    /// <summary>
    /// 文章分类数据传输对象
    /// </summary>
    /// <remarks>勿忘[DataMember]</remarks>
    [DataContract(Namespace = "http://HDIPlatform.DesignSystem.IAppService.DTOs.Outputs.ArticleContext")]
    public class ArticleInfo : BaseDTO
    {

        #region 文章内容 ——  string Content
        /// <summary>
        /// 文章内容
        /// </summary>
        [DataMember]
        public string Content { get; set; }
        #endregion

        #region 是否已发布 —— bool IsIssued
        /// <summary>
        /// 是否已发布
        /// </summary>
        [DataMember]
        public bool IsIssue { get; set; }
        #endregion

        #region 发布时间 —— DateTime IssueTime
        /// <summary>
        /// 发布时间
        /// </summary>
        [DataMember]
        public DateTime? IssueTime { get; set; }
        #endregion

        #region 作者 —— string Author
        /// <summary>
        /// 作者
        /// </summary>
        [DataMember]
        public string Author { get; set; }
        #endregion

        #region 作者Id —— Guid AuthorId
        /// <summary>
        /// 作者Id
        /// </summary>
        [DataMember]
        public Guid AuthorId { get; set; }
        #endregion

        #region 标签集 —— List<string> Tags

        /// <summary>
        /// 标签集
        /// </summary>
        [DataMember]
        public List<string> Tags { get; set; }

        #endregion


        #region 原文章Id(为空表示源文,不为空表示套餐关联的副本) —— Guid? SourceId
        /// <summary>
        /// 原文章Id(为空表示源文,不为空表示套餐关联的副本)
        /// </summary>
        [DataMember]
        public Guid? SourceId { get; set; }
        #endregion

        #region 关联套餐Id ——  Guid? PackId
        /// <summary>
        /// 关联套餐Id
        /// </summary>
        [DataMember]
        public Guid? PackId { get; set; }
        #endregion
        #region 关联套餐空间Id ——  Guid? SpaceId
        /// <summary>
        /// 关联套餐空间Id
        /// </summary>
        [DataMember]
        public Guid? SpaceId { get; set; }
        #endregion

        #region 分类名称 —— string CategoryName
        /// <summary>
        /// 分类名称
        /// </summary>
        [DataMember]
        public string CategoryName { get; set; }
        #endregion
        #region 分类Id —— Guid CategoryId
        /// <summary>
        /// 分类Id
        /// </summary>
        [DataMember]
        public Guid CategoryId { get; set; }
        #endregion

    }
}
