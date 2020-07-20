using System;
using ShSoft.Infrastructure.EntityBase;
using ShSoft.ValueObjects.Enums;

namespace HDIPlatform.DesignSystem.Domain.Entities.DimPackContext
{
    /// <summary>
    /// DIM户型套餐图纸
    /// </summary>
    [Serializable]
    public class DimPackDrawingFile : AggregateRootEntity
    {
        #region # 构造器

        #region 01.无参构造器
        /// <summary>
        /// 无参构造器
        /// </summary>
        protected DimPackDrawingFile() { }
        #endregion


        #endregion

        #region # 属性

        #region 套餐空间Id —— Guid? PackSpaceId
        /// <summary>
        /// 套餐空间Id(空时为户型图纸)
        /// </summary>
        public Guid? PackSpaceId { get; private set; }
        #endregion

        #region 套餐空间名称 —— string PackSpaceName
        /// <summary>
        /// 套餐空间名称
        /// </summary>
        public string PackSpaceName { get; private set; }
        #endregion

        #region 方案空间Id —— Guid? SchemeSpaceId
        /// <summary>
        /// 方案空间Id
        /// </summary>
        public Guid? SchemeSpaceId { get; private set; }
        #endregion

        #region 空间（默认true）—— bool IsSpace
        /// <summary>
        /// 空间（默认true|false为户型）
        /// </summary>
        public bool IsSpace { get; private set; }
        #endregion

        #region 文件Id —— string FileId
        /// <summary>
        /// 文件Id
        /// </summary>
        public string FileId { get; private set; }
        #endregion

        #region 文件名称 —— string FileName
        /// <summary>
        /// 文件名称
        /// </summary>
        public string FileName { get; private set; }
        #endregion

        #region 文件类型 —— SaleDrawingFileTypeNo FileType
        /// <summary>
        /// 文件类型
        /// </summary>
        public SaleDrawingFileTypeNo FileType { get; private set; }
        #endregion

        #region 文件描述 —— string Describe
        /// <summary>
        /// 文件描述
        /// </summary>
        public string Describe { get; private set; }
        #endregion

        #region 导航属性 - DIM套餐 —— DimPack DimPack
        /// <summary>
        /// 导航属性 - DIM套餐
        /// </summary>
        public virtual DimPack DimPack { get; private set; }
        #endregion

        #endregion

        #region # 方法




        #endregion
    }
}
