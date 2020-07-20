using ShSoft.Common.PoweredByLee;
using ShSoft.Infrastructure.EntityBase;
using System;
using System.Collections.Generic;
using System.Linq;
using HDIPlatform.DesignSystem.Domain.EventSources.BaleContext;
using ShSoft.Infrastructure.EventBase.Mediator;

namespace HDIPlatform.DesignSystem.Domain.Entities.BaleContext
{
    /// <summary>
    /// 套餐选区内组
    /// </summary>
    [Serializable]
    public class BalePackGroup : AggregateRootEntity
    {
        #region # 构造器

        #region 01.无参构造器

        /// <summary>
        /// 无参构造器
        /// </summary>
        protected BalePackGroup()
        {
            this.BalePackCategorys = new HashSet<BalePackCategory>();
            this.IsClone = false;
        }
        #endregion

        #region 02.创建套餐选区内组构造器

        /// <summary>
        /// 创建套餐选区内组构造器
        /// </summary>
        /// <param name="choiceAreaId">选区Id</param>
        /// <param name="groupName">组名称</param>
        /// <param name="required">选购方式</param>
        public BalePackGroup(Guid choiceAreaId, string groupName, bool required)
            : this()
        {

            this.ChoiceAreaId = choiceAreaId;
            base.Name = groupName;
            this.Required = required;


        }
        #endregion

        #endregion

        #region # 属性

        #region 选区Id —— Guid ChoiceAreaId
        /// <summary>
        /// 选区Id
        /// </summary>
        public Guid ChoiceAreaId { get; private set; }
        #endregion


        #region 是否必选 —— bool Required
        /// <summary>
        /// 是否必选
        /// </summary>
        public bool Required { get; private set; }
        #endregion


        #region 是否克隆 —— bool IsClone
        /// <summary>
        /// 是否克隆
        /// </summary>
        public bool IsClone { get; private set; }
        #endregion

        #region 导航属性 - 套餐品类集 —— ICollection<BalePackCategory> BalePackCategorys
        /// <summary>
        /// 导航属性 - 套餐品类集
        /// </summary>
        public virtual ICollection<BalePackCategory> BalePackCategorys { get; private set; }
        #endregion

        #endregion

        #region # 方法

        #region 设置组内必选 —— void GroupIsRequired()
        /// <summary>
        /// 设置组内必选
        /// </summary>
        public void GroupIsRequired()
        {
            this.Required = true;
        }
        #endregion

        #region 设置组内非必选 —— void GroupIsNotRequired()

        /// <summary>
        /// 设置组内非必选
        /// </summary>
        public void GroupIsNotRequired()
        {
            this.Required = false;
        }
        #endregion

        #region 新增品类 —— void AddCategoryInGroup(IEnumerable<BalePackCategory> categoryInGroups)
        /// <summary>
        /// 新增品类
        /// </summary>
        /// <param name="categoryInGroups"></param>
        public void AddCategoryInGroup(IEnumerable<BalePackCategory> categoryInGroups)
        {
            #region # 验证

            categoryInGroups = categoryInGroups == null ? new BalePackCategory[0] : categoryInGroups.ToArray();

            if (!categoryInGroups.Any())
            {
                throw new ArgumentNullException("categoryInGroups", "套餐选区内品类集不可为空！");
            }
            if (categoryInGroups.Count() != categoryInGroups.DistinctBy(x => x.CategoryId).Count() || this.BalePackCategorys.Select(s => s.CategoryId).Intersect(categoryInGroups.Select(s => s.CategoryId)).Any())
            {
                throw new InvalidOperationException("同一子选区内，品类不可重复！");
            }
            #endregion
            this.BalePackCategorys.AddRange(categoryInGroups);
        }
        #endregion

        #region 删除品类 —— void DeleteCategoryInGroup(BalePackCategory categoryInGroup)
        /// <summary>
        /// 删除品类
        /// </summary>
        /// <param name="categoryInGroup"></param>
        public void DeleteCategoryInGroup(BalePackCategory categoryInGroup)
        {

            #region 最后一个品类删除验证
            //if (this.BalePackCategorys.Count == 1)
            //{
            //    //throw new InvalidOperationException(string.Format("套餐组内必须存在一个品类，请重新操作！"));
            //}
            #endregion

            categoryInGroup.DeleteBalePackAllProduct();
            this.BalePackCategorys.Remove(categoryInGroup);
        }
        #endregion

        #region 获取组内品类 —— BalePackCategory GetCategoryInGroup(Guid categoryId)

        /// <summary>
        /// 获取组内品类
        /// </summary>
        /// <param name="categoryId">三级品类Id</param>
        /// <returns></returns>
        public BalePackCategory GetCategoryInGroup(Guid categoryId)
        {
            BalePackCategory category = this.BalePackCategorys.SingleOrDefault(s => s.CategoryId == categoryId);

            #region # 验证

            if (category == null)
            {
                throw new ArgumentOutOfRangeException("categoryId", string.Format("Id为\"{0}\"的品类不存在！", categoryId));
            }

            #endregion

            return category;
        }
        #endregion

        #region 获取组内三级品类Id集及是否含有下架商品 —— Tuple<Guid, bool, bool> GetCategoryIds()

        /// <summary>
        /// 获取组内三级品类Id集及是否含有下架商品
        /// </summary>
        /// <returns>IEnumerable<Tuple<Guid, bool, bool>>，[组内三级品类Id列表, 品类内是否含下架商品true|有，false|无,品类下是否包含商品]</returns>
        public IEnumerable<Tuple<Guid, bool, bool>> GetCategoryIds()
        {

            return
                this.BalePackCategorys.OrderBy(s => s.AddedTime)
                    .Select(x => new Tuple<Guid, bool, bool>(x.CategoryId, x.BalePackProducts.Any(v => !v.Shelved), x.BalePackProducts.Any()));
            //.ToDictionary(s => s.CategoryId, s => s.BalePackProducts.Any(v => !v.Shelved));
        }

        #endregion

        #region 修改组名称 —— void ModifyName(string groupName)

        /// <summary>
        /// 修改组名称
        /// </summary>
        /// <param name="groupName">组名称</param>
        public void ModifyName(string groupName)
        {
            this.Name = groupName;
        }
        #endregion


        #region 删除所有品类 —— void DeleteAllCategoryInGroup( )
        /// <summary>
        /// 删除所有品类
        /// </summary>
        public void DeleteAllCategoryInGroup()
        {
            foreach (BalePackCategory categoryInGroup in this.BalePackCategorys.ToList())
            {
                categoryInGroup.DeleteBalePackAllProduct();
                this.BalePackCategorys.Remove(categoryInGroup);

            }
        }
        #endregion

        #region 设置选区Id —— void SetChoiceAreaId(Guid choiceAreaId)
        /// <summary>
        /// 设置选区Id(克隆套餐时)
        /// </summary>
        /// <param name="choiceAreaId"></param>
        public void SetChoiceAreaId(Guid choiceAreaId)
        {
            ChoiceAreaId = choiceAreaId;
        }
        #endregion

        #region 克隆套餐时设置是否克隆 —— void SetIsClone()
        /// <summary>
        /// 克隆套餐时设置是否克隆
        /// </summary>
        public void SetIsClone()
        {
            IsClone = true;
        }
        #endregion
        #endregion
    }
}
