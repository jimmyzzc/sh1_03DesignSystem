using HDIPlatform.DesignSystem.AppService.Maps;
using HDIPlatform.DesignSystem.Domain.Entities.BaleContext;
using HDIPlatform.DesignSystem.Domain.IRepositories;
using HDIPlatform.DesignSystem.Domain.Mediators;
using HDIPlatform.DesignSystem.IAppService.DTOs.Outputs.BaleContext;
using HDIPlatform.DesignSystem.IAppService.Interfaces;
using ShSoft.Common.PoweredByLee;
using ShSoft.Infrastructure.DTOBase;
using ShSoft.Infrastructure.Global.Transaction;
using ShSoft.ValueObjects.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.ServiceModel;
using HDIPlatform.DesignSystem.Domain.Entities.ConfigContext;
using HDIPlatform.DesignSystem.Domain.Entities.FixedMealContext;
using HDIPlatform.DesignSystem.IAppService.DTOs.Outputs.ConfigContext;
using HDIPlatform.SupplierSystem.IAppService.DTOs.Outputs;
using HDIPlatform.SupplierSystem.IAppService.Interfaces;

namespace HDIPlatform.DesignSystem.AppService.Implements
{
    /// <summary>
    /// 套餐配置契约实现
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class ConfigContract : IConfigContract
    {
        #region # 字段及依赖注入构造器

        /// <summary>
        /// 仓储中介者
        /// </summary>
        private readonly RepositoryMediator _repMediator;

        /// <summary>
        /// 单元事务
        /// </summary>
        private readonly IUnitOfWorkDesign _unitOfWork;

        /// <summary>
        /// 领域服务中介者
        /// </summary>
        private readonly DomainServiceMediator _serviceMediator;

        /// <summary>
        /// 依赖注入构造器
        /// </summary>
        /// <param name="repMediator">仓储中介者</param>
        /// <param name="unitOfWork">单元事务</param>
        /// <param name="serviceMediator">领域服务中介者</param>
        public ConfigContract(RepositoryMediator repMediator, IUnitOfWorkDesign unitOfWork, DomainServiceMediator serviceMediator)
        {

            this._repMediator = repMediator;
            this._unitOfWork = unitOfWork;
            _serviceMediator = serviceMediator;
        }

        #endregion

        //命令部分

        #region 套餐系列

        #region 创建套餐系列 --  void CreatePackSeries(string name, string describe, string groupName);

        /// <summary>
        /// 创建套餐系列
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="describe">描述</param>
        /// <param name="groupName">分组名</param>
        public PackSeriesInfo CreatePackSeries(string name, string describe, string groupName)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("套餐系列名称不可为空 ! ");
            }
            int maxSort = _repMediator.PackSeriesRepository.GetMaxSort();
            PackSeries packSeries = new PackSeries(name, describe, maxSort + 1, groupName);
            this._unitOfWork.RegisterAdd(packSeries);
            this._unitOfWork.Commit();
            return packSeries.ToDTO();
        }

        #endregion

        #region 修改套餐系列 --  void UpdatePackSeries(Guid packSeriesId,string name,string describe,string sort, int sort, string groupName)

        /// <summary>
        /// 修改套餐系列
        /// </summary>
        /// <param name="packSeriesId">套餐系列Id</param>
        /// <param name="name">名称</param>
        /// <param name="describe">描述</param>
        /// <param name="sort">序号</param>
        /// <param name="groupName">分组名</param>
        public void UpdatePackSeries(Guid packSeriesId, string name, string describe, int sort, string groupName)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("套餐系列名称不可为空 ! ");
            }
            PackSeries packSeries = _unitOfWork.Resolve<PackSeries>(packSeriesId);
            int originalSort = packSeries.Sort;

            packSeries.Update(name, describe, sort, groupName);

            _unitOfWork.RegisterSave(packSeries);
            _unitOfWork.Commit();

            if (originalSort != sort)
            {
                _serviceMediator.PackSeriesService.ReSort();
            }
        }

        #endregion

        #region 删除套餐系列 --    void DeletePackSeries(Guid packSeriesId);

        /// <summary>
        /// 删除套餐系列
        /// </summary>
        /// <param name="packSeriesId">套餐系列Id</param>
        public void DeletePackSeries(Guid packSeriesId)
        {
            PackSeries packSeries = _unitOfWork.Resolve<PackSeries>(packSeriesId);
            packSeries.DecorationPacks.Clear();
            _unitOfWork.RegisterSave(packSeries);
            _unitOfWork.RegisterRemove<PackSeries>(packSeriesId);
            _unitOfWork.Commit();
            _serviceMediator.PackSeriesService.ReSort();
        }

        #endregion

        #endregion

        //查询部分

        #region 套餐系列 

        #region 分页查询套餐系列集 -- PageModel<PackSeriesInfo> GetPackSeries(string keywords, int pageIndex, int pageSize);

        /// <summary>
        /// 分页查询套餐系列集
        /// </summary>
        /// <param name="keywords">关键字</param>
        /// <param name="pageIndex">页号</param>
        /// <param name="pageSize">页容量</param>
        /// <returns>套餐系列集</returns>
        public PageModel<PackSeriesInfo> GetPackSeries(string keywords, int pageIndex, int pageSize)
        {
            int rowCount, pageCount;

            IEnumerable<PackSeries> packSeries = this._repMediator.PackSeriesRepository.FindByPage(keywords, pageIndex, pageSize, out rowCount, out pageCount);
            IEnumerable<PackSeriesInfo> packSeriesInfos = packSeries.Select(x => x.ToDTO());

            return new PageModel<PackSeriesInfo>(packSeriesInfos, pageIndex, pageSize, pageCount, rowCount);
        }

        #endregion


        #region 查询套餐系列标签及套餐数量集 -- Dictionary<PackSeriesInfo,int> GetPackSeriesCounts();

        /// <summary>
        /// 查询套餐系列标签及套餐数量集
        /// </summary>
        /// <param name="propertyId">关键字</param>
        /// <param name="applicableSquare">页号</param>
        /// <param name="isNewHouse">页容量</param>
        /// <returns>套餐系列标签及套餐数量集</returns>
        public Dictionary<PackSeriesInfo, int> GetPackSeriesCounts(Guid? propertyId, float? applicableSquare, bool? isNewHouse)
        {
            int rowCount, pageCount;

            var result = new Dictionary<PackSeriesInfo, int>();

            var allPacks = this._repMediator.DecorationPackRep.FindByPage(string.Empty, propertyId, applicableSquare, isNewHouse, null, null, null, null, null, null,
                null, null, null, null, null, null, ShelfStatus.Shelfed, null, null, null, null, true, null, null, true, null, null, 1, Int32.MaxValue, out rowCount, out pageCount).ToList();

            var allPackIds = allPacks.Select(s => s.Id).ToList();

            result.Add(new PackSeriesInfo { Name = "全部" }, rowCount);

            var packSeries = allPacks.SelectMany(s => s.PackSeries).DistinctBy(s => s.Id);

            foreach (var series in packSeries)
            {
                result.Add(series.ToDTO(), series.DecorationPacks.Select(s => s.Id).Intersect(allPackIds).Count());
            }

            int noSeriesPackCount = allPacks.Count(s => !s.PackSeries.Any());

            result.Add(new PackSeriesInfo { Name = "其他" }, noSeriesPackCount);
            return result;
        }

        #endregion

        #region 查询套餐分组系列标签及套餐数量集 --IEnumerable<Tuple<string, int, Dictionary<PackSeriesInfo, int>>> GetPackGroupSeriesCounts(Guid? propertyId, float? applicableSquare, bool? isNewHouse)

        /// <summary>
        /// 查询套餐分组系列标签及套餐数量集
        /// </summary>
        /// <param name="propertyId">关键字</param>
        /// <param name="applicableSquare">页号</param>
        /// <param name="isNewHouse">页容量</param>
        /// <returns>套餐分组系列标签及套餐数量集（分组名|排序|系列集）</returns>
        public IEnumerable<Tuple<string, int, Dictionary<PackSeriesInfo, int>>> GetPackGroupSeriesCounts(Guid? propertyId, float? applicableSquare, bool? isNewHouse)
        {
            int rowCount, pageCount;
            var allPacks = this._repMediator.DecorationPackRep.FindByPage(string.Empty, propertyId, applicableSquare, isNewHouse, null, null, null, null, null, null,
                null, null, null, null, null, null, ShelfStatus.Shelfed, null, null, null, null, true, null, null, true, null, null, 1, Int32.MaxValue, out rowCount, out pageCount).ToList();

            var allPackIds = allPacks.Select(s => s.Id).ToList();
            var packSeries = allPacks.SelectMany(s => s.PackSeries).DistinctBy(s => s.Id).ToList();
            List<Tuple<string, int, Dictionary<PackSeriesInfo, int>>> list = new List<Tuple<string, int, Dictionary<PackSeriesInfo, int>>>();
            list.Add(new Tuple<string, int, Dictionary<PackSeriesInfo, int>>("全部", 0, new Dictionary<PackSeriesInfo, int>() { { new PackSeriesInfo { Name = "全部" }, rowCount } }));
            packSeries.GroupBy(x => x.GroupName).ForEach(group =>
            {
                var result = new Dictionary<PackSeriesInfo, int>();
                foreach (var series in group)
                {
                    result.Add(series.ToDTO(), series.DecorationPacks.Select(s => s.Id).Intersect(allPackIds).Count());
                }

                Tuple<string, int, Dictionary<PackSeriesInfo, int>> t = new Tuple<string, int, Dictionary<PackSeriesInfo, int>>(group.Key,
                    group.OrderBy(x => x.Sort).First().Sort, result);
                list.Add(t);

            });

            int noSeriesPackCount = allPacks.Count(s => !s.PackSeries.Any());
            list.Add(new Tuple<string, int, Dictionary<PackSeriesInfo, int>>("其他", Int32.MaxValue, new Dictionary<PackSeriesInfo, int>() { { new PackSeriesInfo { Name = "其他" }, noSeriesPackCount } }));

            return list;
        }

        #endregion


        #endregion


        #region # 测试 Tuple —— void Test(Guid id)
        /// <summary>

        /// 测试 Tuple
        /// </summary>
        /// <param name="id"></param>
        public void Test(Guid id)
        {

            var n = this.GetPackSeriesCounts(null, null, null);
            var nn = this.GetPackGroupSeriesCounts(null, null, null);


        }
        #endregion
    }
}
