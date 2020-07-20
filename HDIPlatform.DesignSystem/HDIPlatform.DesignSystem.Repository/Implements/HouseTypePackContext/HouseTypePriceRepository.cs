using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using HDIPlatform.DesignSystem.Domain.Entities.HouseTypePackContext;
using HDIPlatform.DesignSystem.Domain.IRepositories.HouseTypePackContext;
using ShSoft.Infrastructure.Repository.EntityFramework;
using SD.Toolkits.EntityFramework.Extensions;

namespace HDIPlatform.DesignSystem.Repository.Implements.HouseTypePackContext
{
    /// <summary>
    /// 户型定价仓储实现
    /// </summary>
    public class HouseTypePriceRepository : EFRepositoryProvider<HouseTypePrice>, IHouseTypePriceRepository
    {

        #region # 分页获取户型定价根据楼盘Id —— IEnumerable<HouseTypePrice> FindByPage(Guid propertyId, Dictionary<string, bool> sort, int pageIndex, int pageSize, out int rowCount, out int pageCount)
        /// <summary>
        /// 分页获取户型定价根据楼盘Id
        /// </summary>
        /// <param name="propertyId">楼盘Id</param>
        /// <param name="sort">排序条件 true为倒序</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页容量</param>
        /// <param name="rowCount">总记录条数</param>
        /// <param name="pageCount">总页数</param>
        /// <returns></returns>
        public IEnumerable<HouseTypePrice> FindByPage(Guid propertyId, Dictionary<string, bool> sort, int pageIndex, int pageSize, out int rowCount, out int pageCount)
        {

            IQueryable<HouseTypePrice> houseTypePrices = this.FindAllInner().Where(x => x.PropertyId == propertyId);
            //排序
            if (sort != null && sort.Any())
            {
                houseTypePrices = houseTypePrices.OrderBy(sort);
            }
            rowCount = houseTypePrices.Count();
            pageCount = (int)Math.Ceiling(rowCount * 1.0 / pageSize);
            return houseTypePrices.Skip((pageIndex - 1) * pageSize).Take(pageSize);
        }
        #endregion

        #region # 根据户型Id集获取户型定价 —— IEnumerable<HouseTypePrice> FindByHouseTypeIds(IEnumerable<Guid> houseTypeIds)
        /// <summary>
        /// 根据户型Id集获取户型定价
        /// </summary>
        /// <param name="houseTypeIds">户型Id集</param>
        /// <returns></returns>
        public IEnumerable<HouseTypePrice> FindByHouseTypeIds(IEnumerable<Guid> houseTypeIds)
        {
            return this.FindAllInner().Where(x => houseTypeIds.Contains(x.HouseTypeId)).AsEnumerable();

        }
        #endregion

        #region # 分页获取户型定价根据楼盘Id —— IEnumerable<HouseTypePrice> FindByPage(string keywords, Dictionary<string, bool> sort, int pageIndex, int pageSize, out int rowCount, out int pageCount)
        /// <summary>
        /// 分页获取户型定价根据楼盘Id
        /// </summary>
        /// <param name="keywords">关键字</param>
        /// <param name="sort">排序条件 true为倒序</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页容量</param>
        /// <param name="rowCount">总记录条数</param>
        /// <param name="pageCount">总页数</param>
        /// <returns></returns>
        public IEnumerable<HouseTypePrice> FindByPage(string keywords, Dictionary<string, bool> sort, int pageIndex, int pageSize, out int rowCount, out int pageCount)
        {

            IQueryable<HouseTypePrice> houseTypePrices =
                this.FindAllInner().Where(x => (string.IsNullOrEmpty(keywords) || x.Keywords.Contains(keywords))).GroupBy(x => x.PropertyId).Select(x => x.First());
            //排序
            if (sort != null && sort.Any())
            {
                houseTypePrices = houseTypePrices.OrderBy(sort);
            }
            rowCount = houseTypePrices.Count();
            pageCount = (int)Math.Ceiling(rowCount * 1.0 / pageSize);
            return houseTypePrices.Skip((pageIndex - 1) * pageSize).Take(pageSize);
        }
        #endregion

    }
}
