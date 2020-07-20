
using HDIPlatform.DesignSystem.Domain.IRepositories;
using HDIPlatform.DesignSystem.Domain.Mediators;
using HDIPlatform.DesignSystem.IAppService.Interfaces;
using ShSoft.Common.PoweredByLee;
using ShSoft.Infrastructure.Global.Transaction;
using ShSoft.ValueObjects.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using HDIPlatform.DesignSystem.AppService.Maps;
using HDIPlatform.DesignSystem.Domain.Entities.HouseTypePackContext;
using HDIPlatform.DesignSystem.IAppService.DTOs.Inputs;
using HDIPlatform.DesignSystem.IAppService.DTOs.Outputs.HouseTypePackContext;
using ShSoft.Infrastructure.DTOBase;

namespace HDIPlatform.DesignSystem.AppService.Implements
{
    /// <summary>
    /// 户型套餐服务契约实现
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class HouseTypePackContract : IHouseTypePackContract
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
        public HouseTypePackContract(DomainServiceMediator svcMediator, RepositoryMediator repMediator, IUnitOfWorkDesign unitOfWork)
        {
            this._svcMediator = svcMediator;
            this._repMediator = repMediator;
            this._unitOfWork = unitOfWork;
        }

        #endregion


        //命令部分

        #region # 增加户型定价 —— void AddHouseTypePrice(IEnumerable<HouseTypePriceParam> houseTypePriceParams, Guid operatorId, string @operator)
        /// <summary>
        /// 增加户型定价
        /// </summary>
        /// <param name="houseTypePriceParams">户型定价模型</param>
        /// <param name="operatorId">操作人</param>
        /// <param name="operator">操作人</param>
        public void AddHouseTypePrice(IEnumerable<HouseTypePriceParam> houseTypePriceParams, Guid operatorId, string @operator)
        {
            houseTypePriceParams = houseTypePriceParams == null ? new HouseTypePriceParam[0] : houseTypePriceParams.ToArray();
            //校验 户型是否存在重复
            Assert.IsFalse(houseTypePriceParams.GroupBy(x => x.HouseTypeId).Count() < houseTypePriceParams.Count(), "存在重复户型，请修改");
            //校验 是否已经定价|已定价就修改
            //TODO　仓储查询根据户型Id
            List<Guid> houseTypeIds = houseTypePriceParams.Select(x => x.HouseTypeId).ToList();
            IEnumerable<HouseTypePrice> houseTypes = this._repMediator.HouseTypePriceRep.FindByHouseTypeIds(houseTypeIds);
            Assert.IsFalse(houseTypes.Any(), "户型已定价，不可重复定价");
            IEnumerable<HouseTypePrice> houseTypePrices = houseTypePriceParams.Select(
                  x => new HouseTypePrice(x.PropertyId, x.PropertyName, x.PropertyAddress, x.HouseTypeId, x.HouseTName, x.UnitPrice, operatorId, @operator));
            this._unitOfWork.RegisterAddRange(houseTypePrices);
            this._unitOfWork.UnitedCommit();
        }
        #endregion

        #region  # 编辑户型定价 ——  void UpdateHouseTypePrice(Dictionary<Guid, decimal> houseTypePrices, Guid operatorId, string @operator)
        /// <summary>
        /// 编辑户型定价
        /// </summary>
        /// <param name="houseTypePrices">户型定价集（户型定价Id|单价）</param>
        /// <param name="operatorId">操作人</param>
        /// <param name="operator">操作人</param>
        public void UpdateHouseTypePrice(Dictionary<Guid, decimal> houseTypePrices, Guid operatorId, string @operator)
        {
            houseTypePrices = houseTypePrices ?? new Dictionary<Guid, decimal>();

            houseTypePrices.ForEach(houseType =>
            {
                HouseTypePrice houseTypePrice = this._unitOfWork.Resolve<HouseTypePrice>(houseType.Key);
                //TODO  是否校验
                houseTypePrice.UpdateUnitPrice(houseType.Value, operatorId, @operator);

                this._unitOfWork.RegisterSave(houseTypePrice);
            });
            this._unitOfWork.UnitedCommit();
        }
        #endregion

        #region # 删除户型定价 —— void DeleteHouseTypePrice(Guid houseTypeId)
        /// <summary>
        /// 删除户型定价
        /// </summary>
        /// <param name="houseTypeId">户型定价Id</param>
        public void DeleteHouseTypePrice(Guid houseTypeId)
        {
            this._unitOfWork.RegisterRemove<HouseTypePrice>(houseTypeId);
            this._unitOfWork.UnitedCommit();
        }
        #endregion

        #region # 删除楼盘下所有户型定价 —— void DeleteHouseTypePriceByPId(Guid propertyId)
        /// <summary>
        /// 删除楼盘下所有户型定价
        /// </summary>
        /// <param name="propertyId">楼盘Id</param>
        public void DeleteHouseTypePriceByPId(Guid propertyId)
        {
            IEnumerable<HouseTypePrice> houseTypePrices = this._unitOfWork.ResolveHouseTypePrice(propertyId);
            List<Guid> houseTypePriceIds = houseTypePrices.Select(x => x.Id).ToList();
            //是否需要校验
            this._unitOfWork.RegisterRemoveRange<HouseTypePrice>(houseTypePriceIds);
            this._unitOfWork.UnitedCommit();
        }

        #endregion


        //查询部分


        #region # 测试 Tuple —— void Test(Guid id)
        /// <summary>
        /// 测试 Tuple
        /// </summary>
        /// <param name="id"></param>
        public void Test(Guid id)
        {

            //IEnumerable<Tuple<Guid, bool, bool>> r = this.GetCategoryIdsByGroup(id);
            //r.ForEach(i =>
            //{
            //    Guid cateId = i.Item1;
            //    bool isM = i.Item2;
            //    bool isE = i.Item3;
            //});

        }
        #endregion

        #region # 分页获取楼盘下户型定价 —— PageModel<HouseTypePriceInfo> GetHouseTypePrices(Guid propertyId, Dictionary<string, bool> sort, int pageIndex, int pageSize)
        /// <summary>
        /// 分页获取楼盘下户型定价
        /// </summary>
        /// <param name="propertyId">楼盘Id</param>
        /// <param name="sort"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public PageModel<HouseTypePriceInfo> GetHouseTypePrices(Guid propertyId, Dictionary<string, bool> sort, int pageIndex, int pageSize)
        {
             int rowCount, pageCount;
            IEnumerable<HouseTypePrice> houseTypePrices = this._repMediator.HouseTypePriceRep.FindByPage(propertyId, sort, pageIndex, pageSize,
                out rowCount, out pageCount);
            IEnumerable<HouseTypePriceInfo> houseTypePriceInfos = houseTypePrices.Select(x => x.ToDTO());

             return new PageModel<HouseTypePriceInfo>(houseTypePriceInfos, pageIndex, pageSize, pageCount, rowCount);
        }
    
        #endregion

        #region  # 分页获取定价户型楼盘列表 —— PageModel<HouseTypePriceInfo> GetPropertys(string keywords, Dictionary<string, bool> sort, int pageIndex, int pageSize)
        /// <summary>
        /// 分页获取定价户型楼盘列表
        /// </summary>
        /// <param name="keywords">关键字</param>
        /// <param name="sort"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public PageModel<HouseTypePriceInfo> GetPropertys(string keywords, Dictionary<string, bool> sort, int pageIndex, int pageSize)
        {
            int rowCount, pageCount;
            IEnumerable<HouseTypePrice> houseTypePrices = this._repMediator.HouseTypePriceRep.FindByPage(keywords, sort, pageIndex, pageSize,
                out rowCount, out pageCount);
            IEnumerable<HouseTypePriceInfo> houseTypePriceInfos = houseTypePrices.Select(x => x.ToDTO());

            return new PageModel<HouseTypePriceInfo>(houseTypePriceInfos, pageIndex, pageSize, pageCount, rowCount);
        }
        #endregion


    }
}
