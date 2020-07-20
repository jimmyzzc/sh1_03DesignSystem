using ShSoft.Infrastructure.DTOBase;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace HDIPlatform.DesignSystem.IAppService.DTOs.Outputs.FixedMealContext
{
    /// <summary>
    /// 套餐模板方案数据传输对象
    /// </summary>
    /// <remarks>勿忘[DataMember]</remarks>
    [DataContract(Namespace = "http://HDIPlatform.DesignSystem.IAppService.DTOs.Outputs.FixedMealContext")]
    public class DecorationPackSchemeInfo : BaseDTO
    {
        #region 套餐模板Id —— Guid PackId
        /// <summary>
        /// 套餐模板Id
        /// </summary>
        [DataMember]
        public Guid PackId { get; set; }
        #endregion

        #region 封面 —— string Cover
        /// <summary>
        /// 封面
        /// </summary>
        [DataMember]
        public string Cover { get; set; }
        #endregion

        #region 描述 —— string Description
        /// <summary>
        /// 描述
        /// </summary>
        [DataMember]
        public string Description { get; set; }
        #endregion

        #region 方案概况图片集 —— IList<string> Images
        /// <summary>
        /// 方案概况图片集
        /// </summary>
        [DataMember]
        public IList<string> Images { get; set; }
        #endregion

        #region 方案概况描述 —— IList SchemeDescriptions
        /// <summary>
        /// 方案概况描述
        /// </summary>
        [DataMember]
        public IList<string> SchemeDescriptions { get; set; }
        #endregion

        #region 视频音频地址链接 —— string VideoAudioLink
        /// <summary>
        /// 视频音频地址链接
        /// </summary>
        [DataMember]
        public string VideoAudioLink { get; set; }
        #endregion

        #region 视频音频文件大小 —— string VideoAudioFileSize
        /// <summary>
        /// 视频音频文件大小
        /// </summary>
        [DataMember]
        public string VideoAudioFileSize { get; set; }
        #endregion

        #region 视频音频文件Id —— string VideoAudioFileId
        /// <summary>
        /// 视频音频文件Id
        /// </summary>
        [DataMember]
        public string VideoAudioFileId { get; set; }
        #endregion

        #region 视频音频文件扩展名 —— string VideoAudioFileSuffix
        /// <summary>
        /// 视频音频扩展名
        /// </summary>
        [DataMember]
        public string VideoAudiogFileSuffix { get; set; }
        #endregion

        #region 视频音频文件名称 —— string VideoAudioFileName
        /// <summary>
        /// 视频音频文件名称
        /// </summary>
        [DataMember]
        public string VideoAudioFileName { get; set; }
        #endregion

        #region 默认方案 —— bool IsDefault
        /// <summary>
        /// 默认方案
        /// </summary>
        [DataMember]
        public bool IsDefault { get; set; }
        #endregion

        #region 导航属性 - 套餐模板 —— DecorationPackInfo DecorationPackInfo
        /// <summary>
        /// 导航属性 - 套餐模板
        /// </summary>
        [DataMember]
        public DecorationPackInfo DecorationPackInfo { get; set; }
        #endregion
    }
}
