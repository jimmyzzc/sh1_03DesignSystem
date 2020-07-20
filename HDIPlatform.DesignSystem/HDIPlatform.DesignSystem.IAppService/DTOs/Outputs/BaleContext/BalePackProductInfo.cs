using System;
using System.Runtime.Serialization;
using ShSoft.Infrastructure.DTOBase;

namespace HDIPlatform.DesignSystem.IAppService.DTOs.Outputs.BaleContext
{
    /// <summary>
    /// 套餐品类配置商品数据传输对象
    /// </summary>
    /// <remarks>勿忘[DataMember]</remarks>
    [DataContract(Namespace = "http://HDIPlatform.DesignSystem.IAppService.DTOs.Outputs.BaleContext")]
    public class BalePackProductInfo : BaseDTO
    {

        #region 商品Id —— Guid ProductId
        /// <summary>
        /// 商品Id
        /// </summary>
        [DataMember]
        public Guid ProductId { get; set; }

        #endregion

        #region 商品是否已上架 —— bool Shelved
        /// <summary>
        /// 商品是否已上架
        /// </summary>
        [DataMember]
        public bool Shelved { get; set; }
        #endregion

        #region 导航属性 - 套餐组内品类 —— BalePackCategoryInfo BalePackCategoryInfo
        /// <summary>
        /// 导航属性 - 套餐组内品类
        /// </summary>
        [DataMember]
        public BalePackCategoryInfo BalePackCategoryInfo { get; set; }
        #endregion
    }
}
