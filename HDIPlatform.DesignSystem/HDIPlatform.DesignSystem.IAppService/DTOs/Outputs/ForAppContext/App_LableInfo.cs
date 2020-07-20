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
    /// 套餐标签数据传输对象
    /// </summary>
    /// <remarks>勿忘[DataMember]</remarks>
    [DataContract(Namespace = "http://HDIPlatform.DesignSystem.IAppService.DTOs.Outputs.ForAppContext")]
    public class App_LableInfo
    {
        #region 套餐系列集 —— Dictionary<Guid,string> PackSeries
        /// <summary>
        /// 套餐系列集
        /// </summary>
        [DataMember]
        public IEnumerable<Tuple<Guid, string, string>> PackSeries { get; set; }
        #endregion

        #region 居室集 —— IList<string> Layouts
        /// <summary>
        /// 居室集
        /// </summary>
        [DataMember]
        public IList<string> Layouts { get; set; }
        #endregion

        #region 设计风格集 —— Dictionary<string,string> Styles
        /// <summary>
        /// 设计风格集
        /// </summary>
        [DataMember]
        public Dictionary<string, string> Styles { get; set; }
        #endregion

        #region 套餐标签——楼盘-户型集 —— Dictionary<Guid, Dictionary<Guid, string>> PHouseTypes
        /// <summary>
        /// 套餐标签——楼盘-户型集
        /// </summary>
        [DataMember]
        public Dictionary<Guid, Dictionary<Guid, string>> PHouseTypes { get; set; }
        #endregion

        #region 套餐标签——楼盘集 —— Dictionary<Guid, string> Propertys
        /// <summary>
        /// 套餐标签——楼盘集
        /// </summary>
        [DataMember]
        public Dictionary<Guid, string> Propertys { get; set; }
        #endregion

        #region 套餐标签户型集 ——  IEnumerable<Tuple<Guid, string, Guid, string, string>> HouseTypes
        /// <summary>
        /// 套餐标签户型集(户型Id|户型名称，楼盘Id，楼盘名称,楼盘地址)
        /// </summary>
        [DataMember]
        public IEnumerable<Tuple<Guid, string, Guid, string, string>> HouseTypes { get; set; }
        #endregion

        #region 套餐分组系列集 —— IEnumerable<Tuple<string, int, IEnumerable<Tuple<Guid, string, string, int, string>>>> PackGroupSeries
        /// <summary>
        /// 套餐分组系列集（分组名称|排序|系列集（系列Id|名称|描述|排序|分组名））
        /// </summary>
        [DataMember]
        public IEnumerable<Tuple<string, int, IEnumerable<Tuple<Guid, string, string, int, string>>>> PackGroupSeries { get; set; }
        #endregion


        #region 筑家帮-全部楼盘集 —— IList<Guid> AllPropertyIds
        /// <summary>
        /// 筑家帮-全部楼盘集
        /// </summary>
        [DataMember]
        public IList<Guid> AllPropertyIds { get; set; }
        #endregion

        #region 筑家帮-不可上报楼盘集 —— IList<Guid> ExceptPropertyIds
        /// <summary>
        /// 筑家帮-不可上报楼盘集
        /// </summary>
        [DataMember]
        public IList<Guid> ExceptPropertyIds { get; set; }
        #endregion

        #region 筑家帮-人员保护期内协议楼盘集 —— IList<Guid> FilePropertyIds
        /// <summary>
        /// 筑家帮-人员保护期内协议楼盘集
        /// </summary>
        [DataMember]
        public IList<Guid> FilePropertyIds { get; set; }
        #endregion

        #region 筑家帮-所有保护期内协议楼盘集 —— IList<Guid> AllFilePropertyIds
        /// <summary>
        /// 筑家帮-所有保护期内协议楼盘集
        /// </summary>
        [DataMember]
        public IList<Guid> AllFilePropertyIds { get; set; }
        #endregion

        #region 筑家帮-可上报楼盘集 —— IList<Guid> OperatorPropertyIds
        /// <summary>
        /// 筑家帮-可上报楼盘集
        /// </summary>
        [DataMember]
        public IList<Guid> OperatorPropertyIds { get; set; }
        #endregion

        #region 筑家帮——楼盘-户型集 —— Dictionary<Guid, Dictionary<Guid, string>> OperatorPHouseTypes
        /// <summary>
        /// 筑家帮——楼盘-户型集
        /// </summary>
        [DataMember]
        public Dictionary<Guid, Dictionary<Guid, string>> OperatorPHouseTypes { get; set; }
        #endregion

        #region 筑家帮——楼盘集 —— Dictionary<Guid, string> OperatorPropertys
        /// <summary>
        /// 筑家帮——楼盘集(楼盘Id|名称)
        /// </summary>
        [DataMember]
        public Dictionary<Guid, string> OperatorPropertys { get; set; }
        #endregion

        #region 筑家帮——是否楼盘人员 —— bool IsProperty
        /// <summary>
        /// 筑家帮——是否楼盘人员
        /// </summary>
        [DataMember]
        public bool IsProperty { get; set; }
        #endregion

        #region 筑家帮——楼盘集(楼盘Id|名称|地址) —— Dictionary<Guid, Tuple<string,string>> OperatorPropertyInfos
        /// <summary>
        /// 筑家帮——楼盘集(楼盘Id|名称|地址)
        /// </summary>
        [DataMember]
        public Dictionary<Guid, Tuple<string,string>> OperatorPropertyInfos { get; set; }
        #endregion

    }
}
