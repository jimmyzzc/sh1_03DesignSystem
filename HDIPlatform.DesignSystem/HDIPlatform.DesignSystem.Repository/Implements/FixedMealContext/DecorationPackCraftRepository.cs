using HDIPlatform.DesignSystem.Domain.Entities.FixedMealContext;
using HDIPlatform.DesignSystem.Domain.IRepositories.FixedMealContext;
using ShSoft.Infrastructure.Repository.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HDIPlatform.DesignSystem.Repository.Implements.FixedMealContext
{
    /// <summary>
    /// 套餐模板工艺项仓储实现
    /// </summary>
    public class DecorationPackCraftRepository : EFRepositoryProvider<DecorationPackCraft>, IDecorationPackCraftRepository
    {
        #region # 获取实体对象集合 —— override IQueryable<DecorationPackCraft> FindAllInner()
        /// <summary>
        /// 获取实体对象集合
        /// </summary>
        /// <returns>实体对象集合</returns>
        protected override IQueryable<DecorationPackCraft> FindAllInner()
        {
            return base.FindAllInner().Where(x => x.PackItem != null && !x.PackItem.Deleted).OrderBy(x => x.Sort);
        }
        #endregion

        #region # 根据套餐模板项获取工艺项列表 —— IEnumerable<DecorationPackCraft> FindByPackItem(...
        /// <summary>
        /// 根据套餐模板项获取工艺项列表
        /// </summary>
        /// <param name="packItemId">套餐模板项Id</param>
        /// <returns>工艺项列表</returns>
        public IEnumerable<DecorationPackCraft> FindByPackItem(Guid packItemId)
        {
            return this.Find(x => x.PackItem.Id == packItemId).AsEnumerable();
        }
        #endregion

        #region # 获取总下架工艺实体Id列表 —— IEnumerable<Guid> GetTotalOffShelvedCrafts()
        /// <summary>
        /// 获取总下架工艺实体Id列表(排除克隆套餐版本)
        /// </summary>
        /// <returns>工艺实体Id列表</returns>
        public IEnumerable<Guid> GetTotalOffShelvedCrafts()
        {
            return this.Find(x => !x.Shelved && !x.PackItem.IsClone).OrderBy(x => x.CraftEntityId).Select(x => x.CraftEntityId).Distinct().AsEnumerable();
        }
        #endregion

        #region # 获取套餐模板Id列表 —— IEnumerable<Guid> GetPackIds(Guid craftEntityId)
        /// <summary>
        /// 获取套餐模板Id列表(排除克隆套餐版本)
        /// </summary>
        /// <param name="craftEntityId">工艺实体Id</param>
        /// <returns>套餐模板Id列表</returns>
        public IEnumerable<Guid> GetPackIds(Guid craftEntityId)
        {
            return this.Find(x => x.CraftEntityId == craftEntityId && !x.PackItem.IsClone).Select(x => x.PackItem.PackId).Distinct().AsEnumerable();
        }
        #endregion

        #region # 获取套餐工艺工程量字典 —— IDictionary<Guid, float> GetCraftQuantities(Guid packId)
        /// <summary>
        /// 获取套餐工艺工程量字典
        /// </summary>
        /// <param name="packId">套餐模板Id</param>
        /// <returns>工艺工程量字典</returns>
        public IDictionary<Guid, float> GetCraftQuantities(Guid packId)
        {
            IDictionary<Guid, float> dictionary = new Dictionary<Guid, float>();

            IQueryable<DecorationPackCraft> packCrafts = this.Find(x => x.PackItem.PackId == packId && x.Default);

            foreach (DecorationPackCraft packCraft in packCrafts)
            {
                if (dictionary.ContainsKey(packCraft.CraftEntityId))
                {
                    dictionary[packCraft.CraftEntityId] += packCraft.PackItem.DefaultCraftQuantity ?? 0;
                }
                else
                {
                    dictionary.Add(packCraft.CraftEntityId, packCraft.PackItem.DefaultCraftQuantity ?? 0);
                }
            }

            return dictionary;
        }
        #endregion

        #region # 根据工艺实体获取套餐模板项Id列表 —— IEnumerable<Guid> FindPackItemIds(Guid craftEntityId)
        /// <summary>
        /// 根据工艺实体获取套餐模板项Id列表
        /// </summary>
        /// <param name="craftEntityId">工艺实体Id</param>
        /// <returns></returns>
        public IEnumerable<Guid> FindPackItemIds(Guid craftEntityId)
        {
            IQueryable<DecorationPackCraft> packCrafts = this.Find(x => x.CraftEntityId == craftEntityId);

            return packCrafts.Select(x => x.PackItem.Id).Distinct();
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
                int offShelvedCount = this.Find(x => x.PackItem.PackId == packId && !x.Shelved).Select(s => s.CraftEntityId).Distinct().Count();

                dictionary.Add(packId, offShelvedCount);
            }

            return dictionary;
        }

        #endregion

        #region # 获取总下架工艺实体Id数量 —— int GetTotalOffShelvedCraftCount(IList<Guid> packIds)
        /// <summary>
        /// 获取总下架工艺实体Id数量(排除克隆套餐版本)
        /// </summary>
        /// <param name="packIds">套餐范围Id集（我的套餐范围内:集合|null：全部套餐）</param>
        /// <returns>工艺实体Id数量</returns>
        public int GetTotalOffShelvedCraftCount(IList<Guid> packIds)
        {
            if (packIds == null)
                return this.Find(x => !x.Shelved && !x.PackItem.IsClone).Select(x => x.CraftEntityId).Distinct().Count();
            return this.Find(x => (packIds.Contains(x.PackItem.PackId)) && !x.Shelved && !x.PackItem.IsClone).Select(x => x.CraftEntityId).Distinct().Count();
        }
        #endregion

        #region # 给定套餐模板空间下是否存在下架工艺 —— IDictionary<Guid, bool> ExistsOffShelved(...
        /// <summary>
        /// 给定套餐模板空间下是否存在下架工艺
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

        #region # 获取套餐模板Id列表(排除克隆)—— IEnumerable<Guid> GetPackIdsByCrafts(IEnumerable<Guid> craftEntityIds)
        /// <summary>
        /// 获取套餐模板Id列表(排除克隆)
        /// </summary>
        /// <param name="craftEntityIds">工艺实体Id集</param>
        /// <returns>套餐模板Id列表</returns>
        public IEnumerable<Guid> GetPackIdsByCrafts(IEnumerable<Guid> craftEntityIds)
        {
            return this.Find(x => craftEntityIds.Contains(x.CraftEntityId) && !x.PackItem.IsClone).Select(x => x.PackItem.PackId).Distinct().AsEnumerable();
        }

        #endregion

        #region # 获取套餐选区Id列表 —— IEnumerable<Guid> GetPackItemIdsByCrafts(IEnumerable<Guid> craftEntityIds)
        /// <summary>
        /// 获取套餐选区Id列表
        /// </summary>
        /// <param name="craftEntityIds">工艺实体Id集</param>
        /// <returns>获取套餐选区Id列表</returns>
        public IEnumerable<Guid> GetPackItemIdsByCrafts(IEnumerable<Guid> craftEntityIds)
        {
            return this.Find(x => craftEntityIds.Contains(x.CraftEntityId)).Select(x => x.PackItem.Id).Distinct().AsEnumerable();
        }

        #endregion

        #region # 套餐模板下是否配置套餐模板工艺 —— bool ExistsByPack(Guid packId)
        /// <summary>
        /// 套餐模板下是否配置套餐模板工艺
        /// </summary>
        /// <param name="packId">套餐模板Id</param>
        /// <returns>是否配置</returns>
        public bool ExistsByPack(Guid packId)
        {
            return this.Exists(x => x.PackItem.PackId == packId);
        }
        #endregion


        #region # 获取总下架工艺实体Id列表 —— IEnumerable<Guid> GetTotalOffShelvedCraftsByPage(int pageIndex, int pageSize, out int rowCount, out int pageCount)

        /// <summary>
        /// 获取总下架工艺实体Id列表
        /// </summary>
        /// <returns>工艺实体Id列表</returns>
        public IEnumerable<Guid> GetTotalOffShelvedCraftsByPage(int pageIndex, int pageSize, out int rowCount, out int pageCount)
        {
            IEnumerable<DecorationPackCraft> skus = this.Find(x => !x.Shelved && !x.PackItem.IsClone).OrderBy(x => x.AddedTime).AsEnumerable();
            List<Guid> crafts = skus.Select(x => x.CraftEntityId).Distinct().ToList();
            rowCount = crafts.Count();
            pageCount = (int)Math.Ceiling(rowCount * 1.0 / pageSize);
            return crafts.Skip((pageIndex - 1) * pageSize).Take(pageSize);
        }
        #endregion

        #region # 获取套餐模板Id列表 —— IEnumerable<Tuple<Guid, Guid, Guid,decimal>>  GetCraftsPackIdsById(IEnumerable<Guid> craftEntityIds)

        /// <summary>
        /// 获取套餐模板Id列表
        /// </summary>
        /// <param name="craftEntityIds">工艺实体Id集</param>
        /// <returns>工艺实体Id|选区Id|套餐Id|工艺实体成本价</returns>
        public IEnumerable<Tuple<Guid, Guid, Guid, decimal>> GetCraftsPackIdsById(IEnumerable<Guid> craftEntityIds)
        {

            IEnumerable<DecorationPackCraft> crafts = this.Find(x => craftEntityIds.Contains(x.CraftEntityId) && !x.PackItem.IsClone).AsEnumerable();
            return crafts.Select(x => new Tuple<Guid, Guid, Guid, decimal>(x.CraftEntityId, x.PackItem.Id, x.PackItem.PackId, x.CostPrice));

        }

        #endregion


        #region # 获取总变价工艺实体Id列表 ——  IEnumerable<Guid> GetTotalChangedCraftsByPage(int pageIndex, int pageSize, out int rowCount, out int pageCount)
        /// <summary>
        /// 获取总变价工艺实体Id列表
        /// </summary>
        /// <returns>工艺实体Id列表</returns>
        public IEnumerable<Guid> GetTotalChangedCraftsByPage(int pageIndex, int pageSize, out int rowCount, out int pageCount)
        {
            //List<Guid> crafts = this.Find(x => x.Changed && !x.PackItem.IsClone).OrderBy(x => x.CraftEntityId)
            //    .GroupBy(x => x.CraftEntityId)
            //        .Select(x => x.Key)
            //        .ToList();
            IEnumerable<DecorationPackCraft> skus = this.Find(x => x.Changed && !x.PackItem.IsClone).OrderBy(x => x.AddedTime).AsEnumerable();
            List<Guid> crafts = skus.Select(x => x.CraftEntityId).Distinct().ToList();
            rowCount = crafts.Count();
            pageCount = (int)Math.Ceiling(rowCount * 1.0 / pageSize);
            return crafts.Skip((pageIndex - 1) * pageSize).Take(pageSize);
        }
        #endregion

        #region # 获取总变价工艺实体Id列表 —— IEnumerable<Guid> GetTotalOffShelvedCrafts()
        /// <summary>
        /// 获取总变价工艺实体Id列表
        /// </summary>
        /// <returns>工艺实体Id列表</returns>
        public IEnumerable<Guid> GetTotalChangedCrafts()
        {
            return this.Find(x => x.Changed && !x.PackItem.IsClone).OrderBy(x => x.CraftEntityId).Select(x => x.CraftEntityId).Distinct().AsEnumerable();
        }
        #endregion

        #region # 获取套餐模板内变价工艺数量 —— IDictionary<Guid, int> GetOffShelvedCount(IEnumerable<Guid> packIds)
        /// <summary>
        /// 获取套餐模板内变价工艺数量
        /// </summary>
        /// <param name="packIds">套餐模板Id集</param>
        /// <returns>变价工艺数量字典</returns>
        /// <remarks>IDictionary[Guid, int]，[套餐模板Id, 变价工艺数量]</remarks>
        public IDictionary<Guid, int> GetChangedCount(IEnumerable<Guid> packIds)
        {

            IDictionary<Guid, int> dictionary = new Dictionary<Guid, int>();

            foreach (Guid packId in packIds)
            {
                int offShelvedCount = this.Find(x => x.PackItem.PackId == packId && x.Changed).Select(s => s.CraftEntityId).Distinct().Count();

                dictionary.Add(packId, offShelvedCount);
            }

            return dictionary;
        }
        #endregion

        #region # 获取总变价工艺实体Id数量 —— int GetTotalOffShelvedCraftCount(IList<Guid> packIds)
        /// <summary>
        /// 获取总变价工艺实体Id数量
        /// </summary>
        /// <param name="packIds">套餐范围Id集（我的套餐范围内:集合|null：全部套餐）</param>
        /// <returns>工艺实体Id数量</returns>
        public int GetTotalChangedCraftCount(IList<Guid> packIds)
        {
            if (packIds == null)
                return this.Find(x => x.Changed && !x.PackItem.IsClone).Select(x => x.CraftEntityId).Distinct().Count();
            return this.Find(x => (packIds.Contains(x.PackItem.PackId)) && x.Changed && !x.PackItem.IsClone).Select(x => x.CraftEntityId).Distinct().Count();
        }
        #endregion

        #region # 给定套餐模板空间下是否存在变价工艺 —— IDictionary<Guid, bool> ExistsOffShelved(...
        /// <summary>
        /// 给定套餐模板空间下是否存在变价工艺
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

        #region # 获取套餐选区Id|CraftKey集 —— Dictionary<Guid, List<Guid>> GetPackItemIdCraftKeyIds(IEnumerable<Guid> craftEntityIds)
        /// <summary>
        /// 获取套餐选区Id|CraftKey集
        /// </summary>
        /// <param name="craftEntityIds">craftEntityId 集</param>
        /// <returns>套餐选区Id|CraftKey集</returns>
        public Dictionary<Guid, List<Guid>> GetPackItemIdCraftKeyIds(IEnumerable<Guid> craftEntityIds)
        {
            Dictionary<Guid, List<Guid>> dic = new Dictionary<Guid, List<Guid>>();
            //选区Id集
            List<Guid> itemIds = this.Find(x => craftEntityIds.Contains(x.CraftEntityId) && !x.PackItem.IsClone).Select(x => x.PackItem.Id).Distinct().ToList();
            foreach (Guid itemId in itemIds)
            {
                List<Guid> crafts = this.Find(x => !x.PackItem.IsClone && x.PackItem.Id == itemId && craftEntityIds.Contains(x.CraftEntityId)).Select(x => x.Id).ToList();
                dic.Add(itemId, crafts);
            }
            return dic;
        }
        #endregion


        #region # 获取工艺项列表源选区 —— IEnumerable<DecorationPackCraft> GetCrafts(IEnumerable<Guid> craftEntityIds)
        /// <summary>
        /// 获取工艺项列表源选区
        /// </summary>
        /// <param name="craftEntityIds">craftEntityId 集</param>
        /// <returns>工艺项列表</returns>
        public IEnumerable<DecorationPackCraft> GetCrafts(IEnumerable<Guid> craftEntityIds)
        {
            return this.Find(x => craftEntityIds.Contains(x.CraftEntityId) && !x.PackItem.IsClone).AsEnumerable();
        }

        #endregion


        #region # 获取总变价工艺实体Id|选区Id|套餐Id列表 ——  IEnumerable<Tuple<Guid, Guid, Guid>> GetTotalChangedCraftsByPage(int pageIndex, int pageSize, out int rowCount, out int pageCount)
        /// <summary>
        /// 获取总变价工艺实体Id|选区Id|套餐Id列表
        /// </summary>
        /// <returns>工艺实体Id|选区Id|套餐Id</returns>
        public IEnumerable<Tuple<Guid, Guid, Guid>> GetTotalChangedByPage(int pageIndex, int pageSize, out int rowCount, out int pageCount)
        {
            IEnumerable<DecorationPackCraft> packCrafts = this.Find(x => x.Changed && !x.PackItem.IsClone).AsEnumerable();
            List<Tuple<Guid, Guid, Guid>> crafts = packCrafts.Select(x => new Tuple<Guid, Guid, Guid>(x.CraftEntityId, x.PackItem.Id, x.PackItem.PackId)).Distinct().ToList();
            rowCount = crafts.Count();
            pageCount = (int)Math.Ceiling(rowCount * 1.0 / pageSize);
            return crafts.Skip((pageIndex - 1) * pageSize).Take(pageSize);
        }
        #endregion

        #region # 通过工艺Id集获取套餐选区Id ——IEnumerable<Guid> GetPackItemIdByCraftIds(IEnumerable<Guid> craftIds);
        /// <summary>
        /// 通过工艺Id集获取套餐选区Id
        /// </summary>
        /// <param name="craftIds">工艺Id集</param>
        /// <returns>套餐选区Id集</returns>
        public IEnumerable<Guid> GetPackItemIdByCraftIds(IEnumerable<Guid> craftIds)
        {
            return this.Find(x => craftIds.Contains(x.CraftEntityId) && !x.PackItem.IsClone).Select(x => x.PackItem.Id).Distinct().AsEnumerable();
        }
        #endregion
    }
}
