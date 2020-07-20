using System;
using System.Collections.Generic;
using HDIPlatform.DesignSystem.Domain.Entities.FixedMealContext;
using ShSoft.Infrastructure.EntityBase;

namespace HDIPlatform.DesignSystem.Domain.Entities.ConfigContext
{
    /// <summary>
    /// 套餐系列
    /// </summary>
    [Serializable]
    public class PackSeries : AggregateRootEntity
    {
        #region # 构造器

        #region 01.无参构造器
        /// <summary>
        /// 无参构造器
        /// </summary>
        protected PackSeries()
        {
            this.DecorationPacks = new HashSet<DecorationPack>();
        }
        #endregion

        #region 02.创建套餐模板构造器

        /// <summary>
        /// 创建套餐系列构造器
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="describe">描述</param>
        /// <param name="sort">排序</param>
        /// <param name="groupName">分组名</param>
        public PackSeries(string name, string describe, int sort, string groupName)
            : this()
        {
            base.Name = name;
            this.Describe = describe;
            base.Sort = sort;
            GroupName = groupName;
            base.SetKeywords(name + describe + groupName);
        }
        #endregion

        #endregion

        #region # 属性

        #region 描述 —— string Describe
        /// <summary>
        /// 描述
        /// </summary>
        public string Describe { get; private set; }
        #endregion

        #region 分组名 —— string GroupName
        /// <summary>
        /// 分组名
        /// </summary>
        public string GroupName { get; private set; }
        #endregion

        #region 导航属性 - 关联套餐集 —— virtual ICollection<DecorationPack> DecorationPacks
        /// <summary>
        /// 导航属性 - 套餐系列集
        /// </summary>
        public virtual ICollection<DecorationPack> DecorationPacks { get; private set; }
        #endregion

        #endregion

        #region # 方法

        #region 修改套餐系列信息 —— void Update(string name, string describe, int sort, string groupName)

        /// <summary>
        /// 修改系列信息
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="describe">描述</param>
        /// <param name="sort">序号</param>
        /// <param name="groupName">分组名</param>
        public void Update(string name, string describe, int sort, string groupName)
        {
            base.Name = name;
            this.Describe = describe;
            base.Sort = sort;
            GroupName = groupName;
            base.SetKeywords(name + describe + groupName);
        }
        #endregion

        #endregion
    }
}
