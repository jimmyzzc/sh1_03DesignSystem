using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using HDIPlatform.DesignSystem.Domain.Entities.BaleContext;
using HDIPlatform.DesignSystem.Domain.IRepositories.BaleContext;
using SD.Toolkits.EntityFramework.Extensions;
using ShSoft.Common.PoweredByLee;
using ShSoft.Infrastructure.Repository.EntityFramework;
using ShSoft.ValueObjects.Enums;

namespace HDIPlatform.DesignSystem.Repository.Implements.BaleContext
{
    /// <summary>
    /// 大包/定制套餐仓储实现
    /// </summary>
    public class BalePackRepository : EFRepositoryProvider<BalePack>, IBalePackRepository
    {
        #region # 根据套餐状态标签分页获取大包套餐列表 —— IEnumerable<BalePack> FindBalePackByStatusLabel(string keywords...

        /// <summary>
        /// 根据套餐状态标签分页获取大包套餐列表
        /// </summary>
        /// <param name="keywords">关键字</param>
        /// <param name="status">套餐状态</param>
        /// <param name="labels">套餐标签</param>
        /// <param name="sort">排序条件 false为倒序</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页容量</param>
        /// <param name="rowCount">总记录条数</param>
        /// <param name="pageCount">总页数</param>
        /// <returns>大包套餐列表</returns>
        public IEnumerable<BalePack> FindBalePackByPage(string keywords, ShelfStatus? status, IList<string> labels, Dictionary<string, bool> sort, int pageIndex, int pageSize, out int rowCount, out int pageCount)
        {
            #region 标签处理
            IQueryable<BalePack> balePacks = base.FindAllInner();
            if (labels != null && labels.Any())
            {
                PredicateBuilder<BalePack> builder = new PredicateBuilder<BalePack>(s => false);

                foreach (string label in labels)
                {
                    string slabel = label;

                    builder.Or(s => s.LabelStr.Contains(slabel));
                }

                balePacks = balePacks.Where(builder.Build());
            }
            #endregion

            Expression<Func<BalePack, bool>> condition =
                s => (string.IsNullOrEmpty(keywords) || s.Keywords.Contains(keywords))
                     && (status == null || s.PackShelfStatus == status)
                     && s.BalePackType == BalePackType.Bale
                     && s.SourcePackId == null;
            balePacks = balePacks.Where(condition);
            //排序
            if (sort != null && sort.Any())
            {
                balePacks = balePacks.OrderBy(sort);
            }
            rowCount = balePacks.Count();
            pageCount = (int)Math.Ceiling(rowCount * 1.0 / pageSize);
            return balePacks.Skip((pageIndex - 1) * pageSize).Take(pageSize);
            //return balePacks.ToPage(s => true, pageIndex, pageSize, out rowCount, out pageCount);
        }
        #endregion

        #region # 根据套餐状态标签分页获取定制套餐列表 —— IEnumerable<BalePack> FindCustPackByStatusLabel(string keywords...

        /// <summary>
        /// 根据套餐状态标签分页获取定制套餐列表
        /// </summary>
        /// <param name="keywords">关键字</param>
        /// <param name="status">套餐状态</param>
        /// <param name="labels">套餐标签</param>
        /// <param name="sort">排序条件 false为倒序</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页容量</param>
        /// <param name="rowCount">总记录条数</param>
        /// <param name="pageCount">总页数</param>
        /// <returns>定制套餐列表</returns>
        public IEnumerable<BalePack> FindCustPackByPage(string keywords, ShelfStatus? status, IList<string> labels, Dictionary<string, bool> sort, int pageIndex, int pageSize, out int rowCount, out int pageCount)
        {
            #region 标签处理

            IQueryable<BalePack> balePacks = base.FindAllInner();
            if (labels != null && labels.Any())
            {
                PredicateBuilder<BalePack> builder = new PredicateBuilder<BalePack>(s => false);
                foreach (string label in labels)
                {
                    string slabel = label;
                    builder.Or(s => s.LabelStr.Contains(slabel));

                }
                balePacks = balePacks.Where(builder.Build());
            }

            #endregion

            Expression<Func<BalePack, bool>> condition =
               s => (string.IsNullOrEmpty(keywords) || s.Keywords.Contains(keywords))
               && (status == null || s.PackShelfStatus == status)
               && s.BalePackType == BalePackType.Customized
               && s.SourcePackId == null;

            balePacks = balePacks.Where(condition);
            //排序
            if (sort != null && sort.Any())
            {
                balePacks = balePacks.OrderBy(sort);
            }
            rowCount = balePacks.Count();
            pageCount = (int)Math.Ceiling(rowCount * 1.0 / pageSize);
            return balePacks.Skip((pageIndex - 1) * pageSize).Take(pageSize);
            //return balePacks.ToPage(s => true, pageIndex, pageSize, out rowCount, out pageCount);
        }

        #endregion

        #region # 根据套餐状态标签分页获取关联大包套餐列表 —— IEnumerable<BalePack> FindRelateBalePackByPage(string keywords...

        /// <summary>
        /// 根据套餐状态标签分页获取关联大包套餐列表
        /// </summary>
        /// <param name="type">套餐类型</param>
        /// <param name="keywords">关键字</param>
        /// <param name="status">套餐状态</param>
        /// <param name="labels">套餐标签</param>
        /// <param name="relateBaleIds">已关联大包套餐Id集</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页容量</param>
        /// <param name="rowCount">总记录条数</param>
        /// <param name="pageCount">总页数</param>
        /// <returns>大包套餐列表</returns>
        public IEnumerable<BalePack> FindRelateBalePackByPage(BalePackType? type, string keywords, ShelfStatus? status, IList<string> labels, IEnumerable<Guid> relateBaleIds, int pageIndex,
            int pageSize, out int rowCount, out int pageCount)
        {
            #region 标签处理
            IQueryable<BalePack> balePacks = base.FindAllInner();
            if (labels != null && labels.Any())
            {
                PredicateBuilder<BalePack> builder = new PredicateBuilder<BalePack>(s => false);

                foreach (string label in labels)
                {
                    string slabel = label;

                    builder.Or(s => s.LabelStr.Contains(slabel));
                }

                balePacks = balePacks.Where(builder.Build());
            }
            #endregion

            Expression<Func<BalePack, bool>> condition =
                s => (string.IsNullOrEmpty(keywords) || s.Keywords.Contains(keywords))
                     && (status == null || s.PackShelfStatus == status)
                     && (type == null || s.BalePackType == type)
                     && relateBaleIds.Contains(s.Id) && s.SourcePackId == null;

            balePacks = balePacks.Where(condition);

            rowCount = balePacks.Count();
            pageCount = (int)Math.Ceiling(rowCount * 1.0 / pageSize);
            return balePacks.Skip((pageIndex - 1) * pageSize).Take(pageSize);
            //return balePack.ToPage(s => true, pageIndex, pageSize, out rowCount, out pageCount);
        }
        #endregion

        #region # 根据套餐状态标签分页获取未关联大包套餐列表 —— IEnumerable<BalePack> FindNoRelateBalePackByPage(string keywords...

        /// <summary>
        /// 根据套餐状态标签分页获取未关联大包套餐列表
        /// </summary>
        /// <param name="type">套餐类型</param>
        /// <param name="keywords">关键字</param>
        /// <param name="status">套餐状态</param>
        /// <param name="labels">套餐标签</param>
        /// <param name="relateBaleIds">已关联大包套餐Id集</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页容量</param>
        /// <param name="rowCount">总记录条数</param>
        /// <param name="pageCount">总页数</param>
        /// <returns>大包套餐列表</returns>
        public IEnumerable<BalePack> FindNoRelateBalePackByPage(BalePackType? type, string keywords, ShelfStatus? status, IList<string> labels, IEnumerable<Guid> relateBaleIds,
            int pageIndex, int pageSize, out int rowCount, out int pageCount)
        {
            #region 标签处理
            IQueryable<BalePack> balePacks = base.FindAllInner();
            if (labels != null && labels.Any())
            {
                PredicateBuilder<BalePack> builder = new PredicateBuilder<BalePack>(s => false);

                foreach (string label in labels)
                {
                    string slabel = label;

                    builder.Or(s => s.LabelStr.Contains(slabel));
                }

                balePacks = balePacks.Where(builder.Build());
            }
            #endregion


            Expression<Func<BalePack, bool>> condition =
             s => (string.IsNullOrEmpty(keywords) || s.Keywords.Contains(keywords))
                  && (status == null || s.PackShelfStatus == status)
                  && (type == null || s.BalePackType == type)
                  && !relateBaleIds.Contains(s.Id) && s.SourcePackId == null;

            balePacks = balePacks.Where(condition);
            rowCount = balePacks.Count();
            pageCount = (int)Math.Ceiling(rowCount * 1.0 / pageSize);
            return balePacks.Skip((pageIndex - 1) * pageSize).Take(pageSize);
            //return balePacks.ToPage(s => true, pageIndex, pageSize, out rowCount, out pageCount);
        }
        #endregion

        #region # 根据源套餐Id获取套餐当前版本及Id ——  BalePack FindBalePackVersionNumber(Guid sourcePackId)
        /// <summary>
        /// 根据源套餐Id获取套餐当前版本及Id
        /// </summary>
        /// <param name="sourcePackId">源套餐Id</param>
        /// <returns></returns>
        public BalePack FindBalePackVersionNumber(Guid sourcePackId)
        {
            IQueryable<BalePack> balePacks = base.FindAllInner();
            Expression<Func<BalePack, bool>> condition =
             s => s.SourcePackId == sourcePackId;
            balePacks = balePacks.Where(condition).OrderByDescending(x => x.AddedTime).ThenByDescending(x => x.VersionNumber);
            return balePacks.FirstOrDefault();
        }

        #endregion

        #region # 根据源套餐Id集获取套餐最新版本集 —— Dictionary<Guid, DecorationPack> FindDecorationPackVersionNumbers(IEnumerable<Guid> sourcePackIds);

        /// <summary>
        /// 根据源套餐Id集获取套餐最新版本集
        /// </summary>
        /// <param name="sourcePackIds">源套餐Id集</param>
        /// <returns></returns>
        public IEnumerable<BalePack> FindBaleVersionNumbers(IEnumerable<Guid> sourcePackIds)
        {
            sourcePackIds = sourcePackIds.IsNullOrEmpty() ? new List<Guid>() : sourcePackIds.Distinct();

            Expression<Func<BalePack, bool>> condition = s => s.SourcePackId.HasValue && sourcePackIds.Contains(s.SourcePackId.Value) || sourcePackIds.Contains(s.Id);
            return base.Find(condition);
        }
        #endregion

        #region # 验证套餐名称是否重复 —— bool ExistsPackName(Guid? packId, string packName, BalePackType type)

        /// <summary>
        /// 验证套餐名称是否重复
        /// </summary>
        /// <param name="packId">套餐Id</param>
        /// <param name="packName">套餐名称</param>
        /// <param name="type">套餐类型</param>
        /// <returns></returns>
        public bool ExistsPackName(Guid? packId, string packName, BalePackType type)
        {
            return base.Exists(s => (packId == null || s.Id != packId) && s.Name == packName && s.BalePackType == type && !s.IsClone);
        }
        #endregion

        #region # 根据套餐Id获取套餐内所有选区Id列表 —— IEnumerable<Guid> GetChoiceAreaIdsByPackId(Guid packId)

        /// <summary>
        /// 根据套餐Id获取套餐内所有选区Id列表
        /// </summary>
        /// <param name="packId">套餐Id</param>
        /// <returns></returns>
        public IEnumerable<Guid> GetChoiceAreaIdsByPackId(Guid packId)
        {
            return base.Single(packId).ChoiceAreas.Where(x => !x.Deleted).Select(s => s.Id).ToList();
        }
        #endregion

    }
}
