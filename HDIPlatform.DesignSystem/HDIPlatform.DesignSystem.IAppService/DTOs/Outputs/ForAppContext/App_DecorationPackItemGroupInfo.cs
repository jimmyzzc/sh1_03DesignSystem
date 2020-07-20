using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using ShSoft.Infrastructure.DTOBase;

namespace HDIPlatform.DesignSystem.IAppService.DTOs.Outputs.ForAppContext
{
    /// <summary>
    /// 套餐模板选区分组数据传输对象
    /// </summary>
    /// <remarks>勿忘[DataMember]</remarks>
    [DataContract(Namespace = "http://HDIPlatform.DesignSystem.IAppService.DTOs.Outputs.ForAppContext")]
    public class App_DecorationPackItemGroupInfo : BaseDTO
    {
        /// <summary>
        /// 套餐模板选区分组数据传输对象
        /// </summary>
        /// <param name="name">分组名称</param>
        public App_DecorationPackItemGroupInfo(string name)
        {
            base.Name = name;
            this.CraftInfos = new List<App_DecorationPackCraftInfo>();
            this.SkuInfos = new List<App_DecorationPackSkuInfo>();
        }

        #region 是否推荐 ——  bool IsRecommended
        /// <summary>
        /// 是否推荐
        /// </summary>
        [DataMember]
        public bool IsRecommended { get; set; }
        #endregion

        #region 套餐工艺集 —— IEnumerable<App_DecorationPackCraftInfo> CraftInfos
        /// <summary>
        /// 商品SKU Id
        /// </summary>
        [DataMember]
        public IList<App_DecorationPackCraftInfo> CraftInfos { get; set; }
        #endregion

        #region 套餐商品集 ——IEnumerable<App_DecorationPackSkuInfo> SkuInfos
        /// <summary>
        /// 三级品类Id
        /// </summary>
        [DataMember]
        public IList<App_DecorationPackSkuInfo> SkuInfos { get; set; }

        #endregion
        
    }
}
