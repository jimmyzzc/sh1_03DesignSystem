using ShSoft.Infrastructure.EntityBase;
using ShSoft.ValueObjects.Enums;
using System;

namespace HDIPlatform.DesignSystem.Domain.Entities.FixedMealContext
{
    /// <summary>
    /// 套餐模板 - 大包/定制模板关系
    /// </summary>
    [Serializable]
    public class DecorationPack_BalePack : AggregateRootEntity
    {
        #region # 构造器

        #region 01.无参构造器
        /// <summary>
        /// 无参构造器
        /// </summary>
        protected DecorationPack_BalePack() { }
        #endregion

        #region 02.创建套餐模板 - 大包/定制模板关系构造器
        /// <summary>
        /// 创建套餐模板 - 大包/定制模板关系构造器
        /// </summary>
        /// <param name="decorationPackId">套餐模板Id</param>
        /// <param name="balePackId">大包/定制模板Id</param>
        /// <param name="balePackType">大包/定制模板类型</param>
        public DecorationPack_BalePack(Guid decorationPackId, Guid balePackId, BalePackType balePackType)
            : this()
        {
            this.DecorationPackId = decorationPackId;
            this.BalePackId = balePackId;
            this.BalePackType = balePackType;
        }
        #endregion

        #endregion

        #region # 属性

        #region 套餐模板Id —— Guid DecorationPackId
        /// <summary>
        /// 套餐模板Id
        /// </summary>
        public Guid DecorationPackId { get; private set; }
        #endregion

        #region 大包/定制模板Id —— Guid BalePackId
        /// <summary>
        /// 大包/定制模板Id
        /// </summary>
        public Guid BalePackId { get; private set; }
        #endregion

        #region 大包/定制模板类型 —— BalePackType BalePackType
        /// <summary>
        /// 大包/定制模板类型
        /// </summary>
        public BalePackType BalePackType { get; private set; }
        #endregion

        #endregion

        #region # 方法

        /// <summary>
        /// 克隆固定套餐时修改套餐Id
        /// </summary>
        /// <param name="decorationPackId"></param>
        public void SetDecorationPackId(Guid decorationPackId)
        {
            this.DecorationPackId = decorationPackId;
        }


        #endregion
    }
}
