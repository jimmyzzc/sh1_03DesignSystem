using HDIPlatform.DesignSystem.IAppService.DTOs.Outputs.BaleContext;
using ShSoft.Infrastructure;
using ShSoft.Infrastructure.DTOBase;
using ShSoft.ValueObjects.Enums;
using System;
using System.Collections.Generic;
using System.ServiceModel;
using HDIPlatform.DesignSystem.IAppService.DTOs.Outputs.ArticleContext;

namespace HDIPlatform.DesignSystem.IAppService.Interfaces
{
    /// <summary>
    /// 文章服务契约接口
    /// </summary>
    [ServiceContract(Namespace = "http://HDIPlatform.DesignSystem.IAppService.Interfaces")]
    public interface IArticleContract : IApplicationService
    {
        //命令部分

        #region # 添加文章分类 —— void AddArticleCategory(string name,int sort);
        /// <summary>
        /// 添加文章分类
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="sort">排序</param>
        [OperationContract]
        ArticleCategoryInfo AddArticleCategory(string name, int sort);
        #endregion
        #region # 修改文章分类 —— void ModifyArticleCategory(Guid id ,string name,int sort);

        /// <summary>
        /// 修改文章分类
        /// </summary>
        /// <param name="id">分类Id</param>
        /// <param name="name">名称</param>
        /// <param name="sort">排序</param>
        [OperationContract]
        ArticleCategoryInfo ModifyArticleCategory(Guid id ,string name, int sort);
        #endregion
        #region # 删除文章分类 —— void DeleteArticleCategory(Guid id);
        /// <summary>
        /// 删除文章分类
        /// </summary>
        /// <param name="id">分类Id</param>
        [OperationContract]
        void DeleteArticleCategory(Guid id);
        #endregion
        #region # 添加文章 —— void AddArticle(string name, string content, string author, Guid authorId, List<string> tags, bool isIssue, Guid categoryId, Guid? sourceId, Guid? packId, Guid? spaceId);

        /// <summary>
        /// 添加文章
        /// </summary>
        /// <param name="name">标题</param>
        /// <param name="content">内容</param>
        /// <param name="author">作者</param>
        /// <param name="authorId">作者Id</param>
        /// <param name="tags">标签集</param>
        /// <param name="isIssue">是否发布</param>
        /// <param name="categoryId">分类Id</param>
        /// <param name="sourceId">原文Id</param>
        /// <param name="packId">关联套餐Id</param>
        /// <param name="spaceId">关联套餐空间Id</param>
        [OperationContract]
        void AddArticle(string name, string content, string author, Guid authorId, List<string> tags, bool isIssue, Guid categoryId, Guid? sourceId, Guid? packId, Guid? spaceId);
        #endregion
        #region # 修改文章 —— void ModifyArticle(Guid id,string name, string content, List<string> tags, bool isIssue, Guid categoryId);

        /// <summary>
        /// 修改文章
        /// </summary>
        /// <param name="id">文章Id</param>
        /// <param name="name">标题</param>
        /// <param name="content">内容</param>
        /// <param name="tags">标签集</param>
        /// <param name="isIssue">是否发布</param>
        /// <param name="categoryId">分类Id</param>
        [OperationContract]
        void ModifyArticle(Guid id,string name, string content, List<string> tags, bool isIssue, Guid categoryId);
        #endregion
        #region # 修改文章状态 —— void ModifyArticleState(Guid id, bool isIssue);

        /// <summary>
        /// 修改文章状态
        /// </summary>
        /// <param name="id">文章Id</param>
        /// <param name="isIssue">是否发布</param>
        [OperationContract]
        void ModifyArticleState(Guid id, bool isIssue);
        #endregion
        #region # 删除文章 —— void DeleteArticle(Guid id);

        /// <summary>
        /// 删除文章
        /// </summary>
        /// <param name="id">文章Id</param>
        [OperationContract]
        void DeleteArticle(Guid id);
        #endregion

        //查询部分

        #region # 获取文章分类集 ——  IEnumerable<ArticleCategoryInfo> GetArticleCategoryInfos();
        /// <summary>
        /// 获取文章分类集
        /// </summary>
        /// <returns>文章分类集</returns>
        [OperationContract]
        IEnumerable<ArticleCategoryInfo> GetArticleCategoryInfos();
        #endregion
        #region # 获取文章集 ——    IEnumerable<ArticleInfo> GetArticles(string keywords , Guid? categoryId,bool? isIssue, int pageIndex, int pageSize);

        /// <summary>
        /// 获取文章集
        /// </summary>
        /// <param name="keywords">关键字</param>
        /// <param name="categoryId">文章分类Id</param>
        /// <param name="isIssue">是否已发布</param>
        /// <param name="pageIndex">页号</param>
        /// <param name="pageSize">条数</param>
        /// <returns>文章集</returns>
        [OperationContract]
        PageModel<ArticleInfo> GetArticles(string keywords, Guid? categoryId, bool? isIssue, int pageIndex, int pageSize);
        #endregion
        #region # 通过原文Id获取文章副本集 ——    IEnumerable<ArticleInfo> GetArticlesBySourceId(Guid sourceId);
        /// <summary>
        /// 通过原文Id获取文章副本集
        /// </summary>
        /// <param name="sourceId">原文Id</param>
        /// <returns>文章集</returns>
        [OperationContract]
        IEnumerable<ArticleInfo> GetArticlesBySourceId(Guid sourceId);
        #endregion
        #region # 通过套餐Id获取文章集 ——    IEnumerable<ArticleInfo> GetArticlesByPackId(Guid packId);

        /// <summary>
        /// 通过套餐Id获取文章集
        /// </summary>
        /// <param name="packId">套餐Id</param>
        /// <returns>文章集</returns>
        [OperationContract]
        IEnumerable<ArticleInfo> GetArticlesByPackId(Guid packId);
        #endregion
        #region # 通过套餐Id集获取文章集 —— Dictionary<Guid, IEnumerable<ArticleInfo>> GetArticlesByPackIds(IEnumerable<Guid> packIds)

        /// <summary>
        /// 通过套餐Id集获取文章集
        /// </summary>
        /// <param name="packIds">套餐Id集</param>
        /// <returns>文章集</returns>
        [OperationContract]
        Dictionary<Guid, IEnumerable<ArticleInfo>> GetArticlesByPackIds(IEnumerable<Guid> packIds);
        #endregion
        #region # 通过套餐空间Id获取文章集 ——   IEnumerable<ArticleInfo> GetArticlesBySpaceId(Guid spaceId);

        /// <summary>
        /// 通过套餐空间Id获取文章集
        /// </summary>
        /// <param name="spaceId">套餐空间Id</param>
        /// <returns>文章集</returns>
        [OperationContract]
        IEnumerable<ArticleInfo> GetArticlesBySpaceId(Guid spaceId);
        #endregion
        #region # 获取文章 ——   ArticleInfo GetArticle(Guid id);
        /// <summary>
        /// 获取文章
        /// </summary>
        /// <param name="id">文章Id</param>
        /// <returns>文章集</returns>
        [OperationContract]
        ArticleInfo GetArticle(Guid id);
        #endregion
    }
}
