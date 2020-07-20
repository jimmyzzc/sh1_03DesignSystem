using HDIPlatform.DesignSystem.Domain.IRepositories;
using HDIPlatform.DesignSystem.Domain.Mediators;
using HDIPlatform.DesignSystem.IAppService.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Security.Policy;
using System.ServiceModel;
using System.Text;
using HDIPlatform.DesignSystem.AppService.Maps;
using HDIPlatform.DesignSystem.Domain.Entities.ArticleContext;
using HDIPlatform.DesignSystem.IAppService.DTOs.Outputs.ArticleContext;
using ShSoft.Infrastructure.DTOBase;
using ShSoft.Infrastructure.Global.Transaction;

namespace HDIPlatform.DesignSystem.AppService.Implements
{
    /// <summary>
    /// 大包服务契约实现
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class ArticleContract : IArticleContract
    {
        #region # 字段及依赖注入构造器

        /// <summary>
        /// 领域服务中介者
        /// </summary>
        private readonly DomainServiceMediator _svcMediator;

        /// <summary>
        /// 仓储中介者
        /// </summary>
        private readonly RepositoryMediator _repMediator;

        /// <summary>
        /// 单元事务
        /// </summary>
        private readonly IUnitOfWorkDesign _unitOfWork;

        /// <summary>
        /// 依赖注入构造器
        /// </summary>
        /// <param name="svcMediator">领域服务中介者</param>
        /// <param name="repMediator">仓储中介者</param>
        /// <param name="unitOfWork">单元事务</param>
        public ArticleContract(DomainServiceMediator svcMediator, RepositoryMediator repMediator, IUnitOfWorkDesign unitOfWork)
        {
            this._svcMediator = svcMediator;
            this._repMediator = repMediator;
            this._unitOfWork = unitOfWork;
        }

        #endregion


        //命令部分

        #region # 添加文章分类 —— void AddArticleCategory(string name,int sort);

        /// <summary>
        /// 添加文章分类
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="sort">排序</param>
        public ArticleCategoryInfo AddArticleCategory(string name, int sort)
        {
            if (this._repMediator.ArticleCategoryRepository.Exist(name))
            {
                throw new InvalidOperationException("操作失败 , 文章分类名称重复 !");
            }
            ArticleCategory category = new ArticleCategory(name, sort);
            this._unitOfWork.RegisterAdd(category);
            this._unitOfWork.Commit();
            return category.ToDTO();
        }
        #endregion
        #region # 修改文章分类 —— void ModifyArticleCategory(Guid id ,string name,int sort);

        /// <summary>
        /// 修改文章分类
        /// </summary>
        /// <param name="id">分类Id</param>
        /// <param name="name">名称</param>
        /// <param name="sort">排序</param>
        public ArticleCategoryInfo ModifyArticleCategory(Guid id, string name, int sort)
        {
            ArticleCategory category = this._unitOfWork.Resolve<ArticleCategory>(id);
            ArticleCategory checkName = this._repMediator.ArticleCategoryRepository.GetByName(name);
            if (checkName != null && checkName.Id != id && checkName.Name == name)
            {
                throw new InvalidOperationException("操作失败 , 文章分类名称重复 !");
            }
            category.Modify(name, sort);
            this._unitOfWork.RegisterSave(category);
            this._unitOfWork.Commit();
            return category.ToDTO();
        }
        #endregion
        #region # 删除文章分类 —— void DeleteArticleCategory(Guid id);

        /// <summary>
        /// 删除文章分类
        /// </summary>
        /// <param name="id">分类Id</param>
        public void DeleteArticleCategory(Guid id)
        {
            ArticleCategory category = this._repMediator.ArticleCategoryRepository.SingleOrDefault(id);
            if (category == null)
            {
                throw new InvalidOperationException("操作失败 , 文章分类Id不存在 !");
            }
            if (category.Articles.Any())
            {
                throw new InvalidOperationException("操作失败 ,  删除文章分类前请先删除分类下的文章 !");
            }
            this._unitOfWork.RegisterRemove<ArticleCategory>(id);
            this._unitOfWork.Commit();
        }
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
        public void AddArticle(string name, string content, string author, Guid authorId, List<string> tags, bool isIssue, Guid categoryId, Guid? sourceId, Guid? packId, Guid? spaceId)
        {
            if (sourceId == null && packId != null)
            {
                throw new InvalidOperationException("操作失败 , 原文章Id不可为空 !");
            }
            if (sourceId != null && packId == null)
            {
                throw new InvalidOperationException("操作失败 , 关联套餐Id不可为空 !");
            }
            ArticleCategory category = this._unitOfWork.Resolve<ArticleCategory>(categoryId);
            if (category == null)
            {
                throw new InvalidOperationException("操作失败 , 文章分类Id不存在 !");
            }
            Article article = new Article(name, content, author, authorId, tags, isIssue, category, sourceId, packId, spaceId);
            category.AddArticle(article);
            this._unitOfWork.RegisterSave(category);
            this._unitOfWork.UnitedCommit();
        }
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
        public void ModifyArticle(Guid id, string name, string content, List<string> tags, bool isIssue, Guid categoryId)
        {
            Article article = this._unitOfWork.Resolve<Article>(id);
            ArticleCategory category = this._unitOfWork.Resolve<ArticleCategory>(categoryId);
            if (category == null)
            {
                throw new InvalidOperationException("操作失败 , 文章分类Id不存在 !");
            }

            //            article.ArticleCategory.Articles.Remove(article);
            article.Modify(name, content, tags, isIssue, category);
            if (categoryId != article.ArticleCategory.Id)
            {
                category.AddArticle(article);
                this._unitOfWork.RegisterSave(category);
            }
            this._unitOfWork.RegisterSave(category);
            this._unitOfWork.Commit();
        }
        #endregion
        #region # 修改文章状态 —— void ModifyArticleState(Guid id, bool isIssue);

        /// <summary>
        /// 修改文章状态
        /// </summary>
        /// <param name="id">文章Id</param>
        /// <param name="isIssue">是否发布</param>
        public void ModifyArticleState(Guid id, bool isIssue)
        {
            Article article = this._unitOfWork.Resolve<Article>(id);
            article.ModifyState(isIssue);
            this._unitOfWork.RegisterSave(article);
            this._unitOfWork.Commit();
        }
        #endregion
        #region # 删除文章 —— void DeleteArticle(Guid id);

        /// <summary>
        /// 删除文章
        /// </summary>
        /// <param name="id">文章Id</param>
        public void DeleteArticle(Guid id)
        {
            Article article = this._repMediator.ArticleRepository.SingleOrDefault(id);
            if (article == null)
            {
                throw new InvalidOperationException("操作失败 , 文章Id不存在 !");
            }
            ArticleCategory category = this._unitOfWork.Resolve<ArticleCategory>(article.ArticleCategory.Id);
            if (category == null)
            {
                throw new InvalidOperationException("操作失败 , 文章分类Id不存在 !");
            }

            category.RemoveArticle(id);
            this._unitOfWork.RegisterRemove<Article>(id);
            this._unitOfWork.RegisterSave(category);
            this._unitOfWork.Commit();

        }
        #endregion

        //查询部分

        #region # 获取文章分类集 ——  IEnumerable<ArticleCategoryInfo> GetArticleCategoryInfos();

        /// <summary>
        /// 获取文章分类集
        /// </summary>
        /// <returns>文章分类集</returns>
        public IEnumerable<ArticleCategoryInfo> GetArticleCategoryInfos()
        {
            return this._repMediator.ArticleCategoryRepository.FindAll().OrderBy(s => s.Sort).ThenBy(s => s.Name).Select(s => s.ToDTO());
        }
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
        public PageModel<ArticleInfo> GetArticles(string keywords, Guid? categoryId, bool? isIssue, int pageIndex, int pageSize)
        {
            int rowCount, pageCount;

            IEnumerable<Article> articles = this._repMediator.ArticleRepository.GetArticlesByPage(keywords, categoryId, isIssue, pageIndex, pageSize, out rowCount, out pageCount);
            IEnumerable<ArticleInfo> articleInfos = articles.Select(s => s.ToDTO());

            return new PageModel<ArticleInfo>(articleInfos, pageIndex, pageSize, pageCount, rowCount);
        }
        #endregion
        #region # 通过原文Id获取文章副本集 ——    IEnumerable<ArticleInfo> GetArticlesBySourceId(Guid sourceId);

        /// <summary>
        /// 通过原文Id获取文章副本集
        /// </summary>
        /// <param name="sourceId">原文Id</param>
        /// <returns>文章集</returns>
        public IEnumerable<ArticleInfo> GetArticlesBySourceId(Guid sourceId)
        {
            return this._repMediator.ArticleRepository.GetArticlesBySourceId(sourceId).Select(s => s.ToDTO());
        }
        #endregion
        #region # 通过套餐Id获取文章集 ——    IEnumerable<ArticleInfo> GetArticlesByPackId(Guid packId);

        /// <summary>
        /// 通过套餐Id获取文章集
        /// </summary>
        /// <param name="packId">套餐Id</param>
        /// <returns>文章集</returns>
        public IEnumerable<ArticleInfo> GetArticlesByPackId(Guid packId)
        {
            return this._repMediator.ArticleRepository.GetArticlesByPackId(packId).Select(s => s.ToDTO());
        }
        #endregion
        #region # 通过套餐Id集获取文章集 —— Dictionary<Guid, IEnumerable<ArticleInfo>> GetArticlesByPackIds(IEnumerable<Guid> packIds)

        /// <summary>
        /// 通过套餐Id集获取文章集
        /// </summary>
        /// <param name="packIds">套餐Id集</param>
        /// <returns>文章集</returns>
        public Dictionary<Guid, IEnumerable<ArticleInfo>> GetArticlesByPackIds(IEnumerable<Guid> packIds)
        {
            var result = new Dictionary<Guid, IEnumerable<ArticleInfo>>();
            var data =  this._repMediator.ArticleRepository.GetArticlesByPackIds(packIds.ToList());
            foreach (var item in data)
            {
                result.Add(item.Key, item.Value.Select(s => s.ToDTO()));
            }
            return result;
        }
        #endregion
        #region # 通过套餐空间Id获取文章集 ——   IEnumerable<ArticleInfo> GetArticlesBySpaceId(Guid spaceId);

        /// <summary>
        /// 通过套餐空间Id获取文章集
        /// </summary>
        /// <param name="spaceId">套餐空间Id</param>
        /// <returns>文章集</returns>
        public IEnumerable<ArticleInfo> GetArticlesBySpaceId(Guid spaceId)
        {
            return this._repMediator.ArticleRepository.GetArticlesBySpaceId(spaceId).Select(s => s.ToDTO());
        }
        #endregion
        #region # 获取文章 ——   ArticleInfo GetArticle(Guid id);

        /// <summary>
        /// 获取文章
        /// </summary>
        /// <param name="id">文章Id</param>
        /// <returns>文章集</returns>
        public ArticleInfo GetArticle(Guid id)
        {
            return this._repMediator.ArticleRepository.SingleOrDefault(id).ToDTO();
        }
        #endregion

    }
}
