using ShSoft.Infrastructure.DTOBase;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace HDIPlatform.DesignSystem.IAppService.DTOs.Outputs.FixedMealContext
{
    /// <summary>
    /// 套餐模板方案空间数据传输对象
    /// </summary>
    /// <remarks>勿忘[DataMember]</remarks>
    [DataContract(Namespace = "http://HDIPlatform.DesignSystem.IAppService.DTOs.Outputs.FixedMealContext")]
    public class DecorationPackSchemeSpaceInfo : BaseDTO
    {
        #region 套餐模板空间Id —— Guid PackSpaceId
        /// <summary>
        /// 套餐模板空间Id
        /// </summary>
        [DataMember]
        public Guid PackSpaceId { get; set; }
        #endregion

        #region 描述 —— IList Descriptions
        /// <summary>
        /// 描述
        /// </summary>
        [DataMember]
        public IList<string> Descriptions { get; set; }
        #endregion

        #region 图片集 —— IList<string> Images
        /// <summary>
        /// 图片集
        /// </summary>
        [DataMember]
        public IList<string> Images { get; set; }
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

        #region 导航属性 - 套餐模板方案 —— DecorationPackSchemeInfo PackSchemeInfo
        /// <summary>
        /// 导航属性 - 套餐模板方案
        /// </summary>
        [DataMember]
        public virtual DecorationPackSchemeInfo PackSchemeInfo { get; set; }
        #endregion

        //#region  套餐模板空间 —— DecorationPackSpaceInfo PackSpaceInfo
        ///// <summary>
        /////套餐模板空间
        ///// </summary>
        //[DataMember]
        //public DecorationPackSpaceInfo PackSpaceInfo { get; set; }
        //#endregion
    }
}
