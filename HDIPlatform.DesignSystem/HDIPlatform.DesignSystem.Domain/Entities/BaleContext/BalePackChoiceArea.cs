using ShSoft.Infrastructure.EntityBase;
using System;

namespace HDIPlatform.DesignSystem.Domain.Entities.BaleContext
{
    /// <summary>
    /// 套餐选区
    /// </summary>
    [Serializable]
    public class BalePackChoiceArea : PlainEntity
    {
        #region # 构造器

        #region 01.无参构造器
        /// <summary>
        /// 无参构造器
        /// </summary>
        protected BalePackChoiceArea() { }
        #endregion

        #region 02.创建选区构造器
        /// <summary>
        /// 创建选区构造器
        /// </summary>
        /// <param name="choiceAreaName">选区名称</param>
        public BalePackChoiceArea(string choiceAreaName)
            : this()
        {
            base.Name = choiceAreaName;

        }
        #endregion
        #endregion

        #region # 属性

        #region 导航属性 - 套餐 —— BalePack BalePack
        /// <summary>
        /// 导航属性 - 套餐
        /// </summary>
        public virtual BalePack BalePack { get; private set; }
        #endregion

        #endregion

        #region # 方法


        #region 修改选区名称 —— void ModifyAreaName(string choiceAreaName)

        /// <summary>
        /// 修改选区名称
        /// </summary>
        /// <param name="choiceAreaName">选区名称</param>
        public void ModifyAreaName(string choiceAreaName)
        {

            base.Name = choiceAreaName;
        }
        #endregion

        #endregion
    }
}
