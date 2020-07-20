using System;
using System.Collections.Generic;
using ShSoft.Infrastructure.EntityBase;

namespace HDIPlatform.DesignSystem.Domain.Entities.DimPackContext
{
    /// <summary>
    /// Dim户型方案套餐
    /// </summary>
    [Serializable]
    public class DimPack : AggregateRootEntity
    {
        #region # 构造器

        #region 01.无参构造器
        /// <summary>
        /// 无参构造器
        /// </summary>
        protected DimPack() { }
        #endregion


        #endregion

        #region # 属性

        #region 选购封面 —— string Cover
        /// <summary>
        /// 选购封面
        /// </summary>
        public string Cover { get; private set; }
        #endregion

        #region 套餐浏览量 —— int Views
        /// <summary>
        /// 套餐浏览量
        /// </summary>
        public int Views { get; private set; }
        #endregion

        #region 套餐销售量 —— int Sales
        /// <summary>
        /// 套餐销售量
        /// </summary>
        public int Sales { get; private set; }
        #endregion

        #region 创建人 —— string Creater
        /// <summary>
        /// 创建人
        /// </summary>
        public string Creater { get; private set; }
        #endregion

        #region 创建人Id —— Guid CreaterId
        /// <summary>
        /// 创建人Id
        /// </summary>
        public Guid CreaterId { get; private set; }
        #endregion

        #region 源套餐Id —— Guid? SourcePackId
        /// <summary>
        /// 源套餐Id(源套餐一直为空)
        /// </summary>
        public Guid? SourcePackId { get; private set; }
        #endregion

        #region 版本号 —— string VersionNumber
        /// <summary>
        /// 版本号(源套餐一直为空)（套餐名称首拼套餐类型首拼两位流水日期）
        /// </summary>
        public string VersionNumber { get; private set; }
        #endregion

        #region DIM套餐Id —— Guid DesignPackId

        /// <summary>
        /// 设计套餐Id  
        /// </summary>
        public Guid DesignPackId { get; private set; }

        #endregion

        #region 是否克隆（默认false|DIM套餐有新版本则更改为true）—— bool IsClone
        /// <summary>
        /// 是否克隆（默认false|DIM套餐有新版本则更改为true）
        /// </summary>
        public bool IsClone { get; private set; }
        #endregion


        #region 导航属性 - 套餐空间集 —— ICollection<DimPackSpace> Spaces
        /// <summary>
        /// 导航属性 - 套餐空间集
        /// </summary>
        public virtual ICollection<DimPackSpace> Spaces { get; private set; }
        #endregion

        #region 导航属性 - 套餐图纸集 —— ICollection<DimPackDrawingFile> DimPackDrawingFiles
        /// <summary>
        /// 导航属性 - 套餐图纸集
        /// </summary>
        public virtual ICollection<DimPackDrawingFile> DimPackDrawingFiles { get; private set; }
        #endregion

        #region 导航属性 - 套餐树集 —— ICollection<DimPackTree> DimPackTrees
        /// <summary>
        /// 导航属性 - 套餐树集
        /// </summary>
        public virtual ICollection<DimPackTree> DimPackTrees { get; private set; }
        #endregion



        #endregion

        #region # 方法




        #endregion
    }
}
