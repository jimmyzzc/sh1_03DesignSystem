using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using HDIPlatform.DesignSystem.IAppService.DTOs.Outputs.BaleContext;
using ShSoft.Infrastructure.DTOBase;

namespace HDIPlatform.DesignSystem.IAppService.DTOs.Outputs.ConfigContext
{
    /// <summary>
    /// 套餐系列数据传输对象
    /// </summary>
    /// <remarks>勿忘[DataMember]</remarks>
    [DataContract(Namespace = "http://HDIPlatform.DesignSystem.IAppService.DTOs.Outputs.ConfigContext")]
    public class PackSeriesInfo:BaseDTO
    {

        #region 描述 —— string Describe
        /// <summary>
        /// 描述
        /// </summary>
        [DataMember]
        public string Describe { get; set; }
        #endregion

        #region 分组名 —— string GroupName
        /// <summary>
        /// 分组名
        /// </summary>
        [DataMember]
        public string GroupName { get; set; }
        #endregion

    }
}
