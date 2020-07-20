using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using HDIPlatform.DesignSystem.IAppService.DTOs.Outputs.ConfigContext;
using HDIPlatform.DesignSystem.IAppService.DTOs.Outputs.FixedMealContext;
using ShSoft.Infrastructure.DTOBase;
using ShSoft.ValueObjects.Enums;

namespace HDIPlatform.DesignSystem.IAppService.DTOs.Outputs.ForAppContext
{
    /// <summary>
    /// 套餐空间图片数据传输对象
    /// </summary>
    /// <remarks>勿忘[DataMember]</remarks>
    [DataContract(Namespace = "http://HDIPlatform.DesignSystem.IAppService.DTOs.Outputs.ForAppContext")]
    public class App_DecorationPackSpaceImageInfo
    {
        #region 套餐模板空间Id —— Guid PackSpaceId
        /// <summary>
        /// 套餐模板空间Id
        /// </summary>
        [DataMember]
        public Guid PackSpaceId { get; set; }
        #endregion

          #region 空间名称 —— string PackSpaceName
        /// <summary>
        /// 空间名称
        /// </summary>
        [DataMember]
        public string PackSpaceName { get; set; }
        #endregion

        #region 套餐方案空间集 —— IEnumerable<DecorationPackSchemeSpaceInfo> PackSchemeSpaceInfo
        /// <summary>
        /// 套餐方案空间集
        /// </summary>
        [DataMember]
        public IEnumerable<DecorationPackSchemeSpaceInfo> PackSchemeSpaceInfo { get; set; }
        #endregion
        
    }
}
