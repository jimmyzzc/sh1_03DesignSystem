using HDIPlatform.DesignSystem.Domain.Entities.FixedMealContext;
using HDIPlatform.DesignSystem.Domain.IRepositories.FixedMealContext;
using ShSoft.Infrastructure.Repository.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HDIPlatform.DesignSystem.Repository.Implements.FixedMealContext
{
    /// <summary>
    /// 套餐模板商品SKU项仓储实现
    /// </summary>
    public class DecorationPackSkuRepository : EFRepositoryProvider<DecorationPackSku>, IDecorationPackSkuRepository
    {
        #region # 获取实体对象集合 —— override IQueryable<DecorationPackSku> FindAllInner()
        /// <summary>
        /// 获取实体对象集合
        /// </summary>
        /// <returns>实体对象集合</returns>
        protected override IQueryable<DecorationPackSku> FindAllInner()
        {
            return base.FindAllInner().Where(x => x.PackItem != null && !x.PackItem.Deleted).OrderBy(x => x.Sort);
        }
        #endregion

        #region # 根据套餐模板项获取商品SKU项列表 —— IEnumerable<DecorationPackSku> FindByPackItem(...
        /// <summary>
        /// 根据套餐模板项获取商品SKU项列表
        /// </summary>
        /// <param name="packItemId">套餐模板项Id</param>
        /// <returns>商品SKU项列表</returns>
        public IEnumerable<DecorationPackSku> FindByPackItem(Guid packItemId)
        {
            return this.Find(x => x.PackItem.Id == packItemId).AsEnumerable();
        }
        #endregion

        #region # 套餐模板下是否配置套餐模板商品SKU项 —— bool ExistsByPack(Guid packId)
        /// <summary>
        /// 套餐模板下是否配置套餐模板商品SKU项
        /// </summary>
        /// <param name="packId">套餐模板Id</param>
        /// <returns>是否配置</returns>
        public bool ExistsByPack(Guid packId)
        {
            return this.Exists(x => x.PackItem.PackId == packId);
        }
        #endregion

        #region # 获取套餐模板内下架商品数量 —— IDictionary<Guid, int> GetOffShelvedCount(IEnumerable<Guid> packIds)
        /// <summary>
        /// 获取套餐模板内下架商品数量
        /// </summary>
        /// <param name="packIds">套餐模板Id集</param>
        /// <returns>下架商品数量字典</returns>
        /// <remarks>IDictionary[Guid, int]，[套餐模板Id, 下架商品数量]</remarks>
        public IDictionary<Guid, int> GetOffShelvedCount(IEnumerable<Guid> packIds)
        {
            IDictionary<Guid, int> dictionary = new Dictionary<Guid, int>();

            foreach (Guid packId in packIds)
            {
                int offShelvedCount = this.Find(x => x.PackItem.PackId == packId && !x.Shelved).Select(s => s.SkuId).Distinct().Count();

                dictionary.Add(packId, offShelvedCount);
            }

            return dictionary;
        }
        #endregion

        #region # 获取套餐模板内变价商品数量 —— IDictionary<Guid, int> GetChangedCount(IEnumerable<Guid> packIds)
        /// <summary>
        /// 获取套餐模板内变价商品数量
        /// </summary>
        /// <param name="packIds">套餐模板Id集</param>
        /// <returns>变价商品数量字典</returns>
        /// <remarks>IDictionary[Guid, int]，[套餐模板Id, 变价商品数量]</remarks>
        public IDictionary<Guid, int> GetChangedCount(IEnumerable<Guid> packIds)
        {
            IDictionary<Guid, int> dictionary = new Dictionary<Guid, int>();

            foreach (Guid packId in packIds)
            {
                int changedCount = this.Find(x => x.PackItem.PackId == packId && x.Changed).Select(s => s.SkuId).Distinct().Count();

                dictionary.Add(packId, changedCount);
            }

            return dictionary;
        }

        #endregion

        #region # 获取总下架商品数量 —— int GetTotalOffShelvedCount(IList<Guid> packIds)
        /// <summary>
        /// 获取总下架商品数量(排除克隆套餐版本)
        /// </summary>
        /// <param name="packIds">套餐范围Id集（我的套餐范围内:集合|null：全部套餐）</param>
        /// <returns>总下架商品数量</returns>
        public int GetTotalOffShelvedCount(IList<Guid> packIds)
        {
            if (packIds == null)
                return this.Find(x => !x.Shelved && !x.PackItem.IsClone).Select(x => x.SkuId).Distinct().Count();
            return this.Find(x => (packIds.Contains(x.PackItem.PackId)) && !x.Shelved && !x.PackItem.IsClone).Select(x => x.SkuId).Distinct().Count();
        }
        #endregion

        #region # 获取总变价商品数量 —— int GetTotalOffShelvedCount(IList<Guid> packIds)
        /// <summary>
        /// 获取总变价商品数量(排除克隆套餐版本)
        /// </summary>
        /// <param name="packIds">套餐范围Id集（我的套餐范围内:集合|null：全部套餐）</param>
        /// <returns>总变价商品数量</returns>
        public int GetTotalChangedCount(IList<Guid> packIds)
        {
            if (packIds == null)
                return this.Find(x => x.Changed && !x.PackItem.IsClone).Select(x => x.SkuId).Distinct().Count();
            return this.Find(x => (packIds.Contains(x.PackItem.PackId)) && x.Changed && !x.PackItem.IsClone).Select(x => x.SkuId).Distinct().Count();
        }
        #endregion


        #region # 获取总下架商品SKU Id列表 —— IEnumerable<Guid> GetTotalOffShelvedSkus()
        /// <summary>
        /// 获取总下架商品SKU Id列表(排除克隆套餐版本)
        /// </summary>
        /// <returns>商品SKU Id列表</returns>
        public IEnumerable<Guid> GetTotalOffShelvedSkus()
        {
            return this.Find(x => !x.Shelved && !x.PackItem.IsClone).OrderBy(x => x.SkuId).Select(x => x.SkuId).Distinct().AsEnumerable();
        }
        #endregion

        #region # 获取总变价商品SKU Id列表 —— IEnumerable<Guid> GetTotalChangedSkus()
        /// <summary>
        /// 获取总变价商品SKU Id集列表
        /// </summary>
        /// <returns>商品SKU Id列表</returns>
        public IEnumerable<Guid> GetTotalChangedSkus()
        {
            return this.Find(x => x.Changed && !x.PackItem.IsClone).OrderBy(x => x.SkuId).Select(x => x.SkuId).Distinct().AsEnumerable();
        }

        #endregion

        #region # 获取套餐模板Id列表 —— IEnumerable<Guid> GetPackIds(Guid skuId)
        /// <summary>
        /// 获取套餐模板Id列表
        /// </summary>
        /// <param name="skuId">商品SKU Id</param>
        /// <returns>套餐模板Id列表</returns>
        public IEnumerable<Guid> GetPackIds(Guid skuId)
        {
            return this.Find(x => x.SkuId == skuId && !x.PackItem.IsClone).Select(x => x.PackItem.PackId).Distinct().AsEnumerable();
        }
        #endregion

        #region # 给定套餐模板空间下是否存在下架商品 —— IDictionary<Guid, bool> ExistsOffShelved(...
        /// <summary>
        /// 给定套餐模板空间下是否存在下架商品
        /// </summary>
        /// <param name="packSpaceIds">套餐模板空间Id</param>
        /// <returns>是否存在</returns>
        public IDictionary<Guid, bool> ExistsOffShelved(IEnumerable<Guid> packSpaceIds)
        {
            IDictionary<Guid, bool> dictionary = new Dictionary<Guid, bool>();

            foreach (Guid packSpaceId in packSpaceIds.Distinct())
            {
                bool anyOffShelved = this.Exists(x => !x.Shelved && x.PackItem.PackSpaceId == packSpaceId);
                dictionary.Add(packSpaceId, anyOffShelved);
            }

            return dictionary;
        }
        #endregion

        #region # 给定套餐模板空间下是否存在变价商品 —— IDictionary<Guid, bool> ExistsChanged(...
        /// <summary>
        /// 给定套餐模板空间下是否存在变价商品
        /// </summary>
        /// <param name="packSpaceIds">套餐模板空间Id</param>
        /// <returns>是否存在</returns>
        public IDictionary<Guid, bool> ExistsChanged(IEnumerable<Guid> packSpaceIds)
        {
            IDictionary<Guid, bool> dictionary = new Dictionary<Guid, bool>();

            foreach (Guid packSpaceId in packSpaceIds.Distinct())
            {
                bool anyOffShelved = this.Exists(x => x.Changed && x.PackItem.PackSpaceId == packSpaceId);
                dictionary.Add(packSpaceId, anyOffShelved);
            }

            return dictionary;
        }

        #endregion

        #region # 获取套餐商品SKU数量字典 —— IDictionary<Guid, float> GetSkuQuantities(Guid packId)
        /// <summary>
        /// 获取套餐商品SKU数量字典
        /// </summary>
        /// <param name="packId">套餐模板Id</param>
        /// <returns>商品SKU数量字典</returns>
        public IDictionary<Guid, float> GetSkuQuantities(Guid packId)
        {
            IDictionary<Guid, float> dictionary = new Dictionary<Guid, float>();

            IQueryable<DecorationPackSku> packSkus = this.Find(x => x.PackItem.PackId == packId && x.Default);

            foreach (DecorationPackSku packSku in packSkus)
            {
                if (dictionary.ContainsKey(packSku.SkuId))
                {
                    dictionary[packSku.SkuId] += packSku.PackItem.DefaultSkuQuantity ?? 0;
                }
                else
                {
                    dictionary.Add(packSku.SkuId, packSku.PackItem.DefaultSkuQuantity ?? 0);
                }
            }

            return dictionary;
        }
        #endregion

        #region # 根据商品SKU获取套餐模板项Id列表 —— IEnumerable<Guid> FindPackItemIds(Guid skuId)
        /// <summary>
        /// 根据商品SKU获取套餐模板项Id列表
        /// </summary>
        /// <param name="skuId">商品SKU Id</param>
        /// <returns></returns>
        public IEnumerable<Guid> FindPackItemIds(Guid skuId)
        {
            IQueryable<DecorationPackSku> packSkus = this.Find(x => x.SkuId == skuId && !x.PackItem.IsClone);

            return packSkus.Select(x => x.PackItem.Id).Distinct();
        }
        #endregion


        #region # 根据套餐获取商品SKU项列表 ——IDictionary<Guid,IEnumerable<DecorationPackSku>>  FindByPackId(...
        /// <summary>
        /// 根据套餐获取商品SKU项列表
        /// </summary>
        /// <param name="packId">套餐Id</param>
        /// <returns>选区|商品SKU项列表</returns>
        public IDictionary<Guid, IEnumerable<DecorationPackSku>> FindByPackId(Guid packId)
        {
            IDictionary<Guid, IEnumerable<DecorationPackSku>> dictionary = new Dictionary<Guid, IEnumerable<DecorationPackSku>>();
            IQueryable<DecorationPackSku> packSkus = this.Find(x => x.PackItem.PackId == packId);
            List<Guid> packItemIds = packSkus.Select(x => x.PackItem).Select(x => x.Id).Distinct().ToList();
            foreach (Guid packItemId in packItemIds)
            {
                dictionary.Add(packItemId, packSkus.Where(x => x.PackItem.Id == packItemId).AsEnumerable());
            }
            return dictionary;
        }

        #endregion

        #region # 获取套餐选区Id列表 —— IEnumerable<Guid> GetPackItemIdsBySkus(IEnumerable<Guid> skuIds)
        /// <summary>
        /// 获取套餐选区Id列表
        /// </summary>
        /// <param name="skuIds">SkuId集</param>
        /// <returns>套餐选区Id列表</returns>
        public IEnumerable<Guid> GetPackItemIdsBySkus(IEnumerable<Guid> skuIds)
        {
            return this.Find(x => skuIds.Contains(x.SkuId) && !x.PackItem.IsClone).Select(x => x.PackItem.Id).Distinct().AsEnumerable();
        }

        #endregion

        #region # 通过SkuId集获取套餐选区Id ——IEnumerable<Guid> GetPackItemIdBySkuIds(IEnumerable<Guid> skuIds)
        /// <summary>
        /// 通过SkuId集获取套餐选区Id
        /// </summary>
        /// <param name="skuIds">SkuId集</param>
        /// <returns>套餐选区Id集</returns>
        public IEnumerable<Guid> GetPackItemIdBySkuIds(IEnumerable<Guid> skuIds)
        {
            return this.Find(x => skuIds.Contains(x.SkuId) && !x.PackItem.IsClone).Select(x => x.PackItem.Id).Distinct().AsEnumerable();
        }
        #endregion


        #region # 获取套餐模板Id列表 —— IEnumerable<Guid> GetPackIdsBySkus(IEnumerable<Guid> skuIds)

        /// <summary>
        /// 获取套餐模板Id列表
        /// </summary>
        /// <param name="skuIds">SkuId集</param>
        /// <returns>套餐模板Id列表</returns>
        public IEnumerable<Guid> GetPackIdsBySkus(IEnumerable<Guid> skuIds)
        {
            return this.Find(x => skuIds.Contains(x.SkuId) && !x.PackItem.IsClone).Select(x => x.PackItem.PackId).Distinct().AsEnumerable();
        }

        #endregion

        #region # 获取总下架商品SKU Id列表 —— IEnumerable<Guid> GetTotalOffShelvedSkusByPage(int pageIndex, int pageSize, out int rowCount, out int pageCount);

        /// <summary>
        /// 获取总下架商品SKU Id集列表
        /// </summary>
        /// <returns>商品SKU Id列表</returns>
        public IEnumerable<Guid> GetTotalOffShelvedSkusByPage(int pageIndex, int pageSize, out int rowCount, out int pageCount)
        {

            //List<Guid> crafts =
            //    this.Find(x => !x.Shelved && !x.PackItem.IsClone).OrderBy(x => x.SkuId)
            //        .GroupBy(x => x.SkuId)
            //        .Select(x => x.Key)
            //        .ToList();

            //List<Guid> crafts = this.Find(x => !x.Shelved && !x.PackItem.IsClone).OrderBy(x => x.AddedTime).Select(x => x.SkuId).Distinct().AsEnumerable().ToList();

            List<DecorationPackSku> skus = this.Find(x => !x.Shelved && !x.PackItem.IsClone).OrderBy(x => x.AddedTime).ToList();
            List<Guid> crafts = skus.Select(x => x.SkuId).Distinct().ToList();
            //    .Select(x => x.SkuId).Distinct().AsEnumerable().ToList();
            rowCount = crafts.Count();
            pageCount = (int)Math.Ceiling(rowCount * 1.0 / pageSize);
            return crafts.Skip((pageIndex - 1) * pageSize).Take(pageSize);
        }
        #endregion

        #region # 获取总变价商品SKU Id列表 —— IEnumerable<Guid> GetTotalChangedSkuIdsByPage(int pageIndex, int pageSize, out int rowCount, out int pageCount);
        /// <summary>
        /// 获取总变价商品SKU Id集列表
        /// </summary>
        /// <returns>商品SKU Id列表</returns>
        public IEnumerable<Guid> GetTotalChangedSkuIdsByPage(int pageIndex, int pageSize, out int rowCount, out int pageCount)
        {
            //List<Guid> crafts = this.Find(x => x.Changed && !x.PackItem.IsClone).OrderBy(x => x.SkuId)
            //    .GroupBy(x => x.SkuId)
            //        .Select(x => x.Key)
            //        .ToList();
            List<DecorationPackSku> skus = this.Find(x => x.Changed && !x.PackItem.IsClone).OrderBy(x => x.AddedTime).ToList();
            List<Guid> crafts = skus.Select(x => x.SkuId).Distinct().ToList();
            rowCount = crafts.Count();
            pageCount = (int)Math.Ceiling(rowCount * 1.0 / pageSize);
            return crafts.Skip((pageIndex - 1) * pageSize).Take(pageSize);
        }

        #endregion

        #region # 获取总变价商品SKUId|选区Id|套餐Id列表 —— IEnumerable<Tuple<Guid, Guid, Guid>> GetTotalChangedSkuIdsByPage(int pageIndex, int pageSize, out int rowCount, out int pageCount);
        /// <summary>
        /// 获取总变价商品SKUId|选区Id|套餐Id列表
        /// </summary>
        /// <returns>商品SKUId|选区Id|套餐Id</returns>
        public IEnumerable<Tuple<Guid, Guid, Guid>> GetTotalChangedByPage(int pageIndex, int pageSize, out int rowCount, out int pageCount)
        {
            IEnumerable<DecorationPackSku> packSkus = this.Find(x => x.Changed && !x.PackItem.IsClone).AsEnumerable();
            List<Tuple<Guid, Guid, Guid>> crafts = packSkus.Select(x => new Tuple<Guid, Guid, Guid>(x.SkuId, x.PackItem.Id, x.PackItem.PackId)).Distinct().ToList();
            rowCount = crafts.Count();
            pageCount = (int)Math.Ceiling(rowCount * 1.0 / pageSize);
            return crafts.Skip((pageIndex - 1) * pageSize).Take(pageSize);
        }

        #endregion



        #region # 获取商品SKU 列表源选区 —— IEnumerable<DecorationPackSku> GetSkus(IEnumerable<Guid> skuIds)
        /// <summary>
        /// 获取商品SKU Id集列表源选区
        /// </summary>
        /// <returns>商品SKU 列表</returns>
        public IEnumerable<DecorationPackSku> GetSkus(IEnumerable<Guid> skuIds)
        {
            return this.Find(x => skuIds.Contains(x.SkuId) && !x.PackItem.IsClone).OrderBy(x => x.SkuId).AsEnumerable();
        }
        #endregion


        #region # 获取套餐模板Id列表(商品实体Id|选区Id|套餐Id|商品实体成本价) —— IEnumerable<Tuple<Guid, Guid, Guid,decimal>>   GetSkusPackIdsById(IEnumerable<Guid> skuIds)

        /// <summary>
        /// 获取套餐模板Id列表(商品实体Id|选区Id|套餐Id|商品实体成本价)
        /// </summary>
        /// <param name="skuIds">商品实体Id集</param>
        /// <returns>商品实体Id|选区Id|套餐Id|商品实体成本价</returns>
        public IEnumerable<Tuple<Guid, Guid, Guid, decimal>> GetSkusPackIdsById(IEnumerable<Guid> skuIds)
        {

            IEnumerable<DecorationPackSku> crafts = this.Find(x => skuIds.Contains(x.SkuId) && !x.PackItem.IsClone).AsEnumerable();
            return crafts.Select(x => new Tuple<Guid, Guid, Guid, decimal>(x.SkuId, x.PackItem.Id, x.PackItem.PackId, x.CostPrice));

        }

        #endregion

    }
}
