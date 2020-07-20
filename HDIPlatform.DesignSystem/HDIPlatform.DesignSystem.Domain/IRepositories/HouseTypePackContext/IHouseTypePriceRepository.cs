using System;
using System.Collections.Generic;
using System.Linq;
using HDIPlatform.DesignSystem.Domain.Entities.HouseTypePackContext;
using ShSoft.Infrastructure.RepositoryBase;

namespace HDIPlatform.DesignSystem.Domain.IRepositories.HouseTypePackContext
{
    /// <summary>
    /// 户型定价仓储接口
    /// </summary>
    public interface IHouseTypePriceRepository : IRepository<HouseTypePrice>
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
        IEnumerable<HouseTypePrice> FindByPage(Guid propertyId, Dictionary<string, bool> sort, int pageIndex, int pageSize, out int rowCount, out int pageCount);
        
        #endregion

        #region # 根据户型Id集获取户型定价 —— IEnumerable<HouseTypePrice> FindByHouseTypeIds(IEnumerable<Guid> houseTypeIds)

        /// <summary>
        /// 根据户型Id集获取户型定价
        /// </summary>
        /// <param name="houseTypeIds">户型Id集</param>
        /// <returns></returns>
        IEnumerable<HouseTypePrice> FindByHouseTypeIds(IEnumerable<Guid> houseTypeIds);

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
        IEnumerable<HouseTypePrice> FindByPage(string keywords, Dictionary<string, bool> sort, int pageIndex, int pageSize, out int rowCount,
            out int pageCount);

        #endregion


    }
}
