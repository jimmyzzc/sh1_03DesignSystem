using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using HDIPlatform.DesignSystem.IAppService.DTOs.Outputs.BaleContext;
using ShSoft.Infrastructure.DTOBase;

namespace HDIPlatform.DesignSystem.IAppService.DTOs.Outputs.ArticleContext
{
    /// <summary>
    /// 文章分类数据传输对象
    /// </summary>
    /// <remarks>勿忘[DataMember]</remarks>
    [DataContract(Namespace = "http://HDIPlatform.DesignSystem.IAppService.DTOs.Outputs.ArticleContext")]
    public class ArticleCategoryInfo:BaseDTO
    {

        #region 分类下文章数量 ——  int ArticleCount 
        /// <summary>
        /// 分类下文章数量
        /// </summary>
        [DataMember]
        public int ArticleCount { get; set; }
        #endregion

      

    }
}
