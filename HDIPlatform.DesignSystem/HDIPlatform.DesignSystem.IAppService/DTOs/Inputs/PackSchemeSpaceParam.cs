using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace HDIPlatform.DesignSystem.IAppService.DTOs.Inputs
{
    /// <summary>
    /// 套餐模板方案空间参数模型
    /// </summary>
    /// <remarks>勿忘[DataMember]</remarks>
    [DataContract(Namespace = "http://HDIPlatform.DesignSystem.IAppService.DTOs.Inputs")]
    public struct PackSchemeSpaceParam
    {
        /// <summary>
        /// 套餐模板空间Id
        /// </summary>
        [DataMember]
        public Guid PackSpaceId;

        /// <summary>
        /// 描述
        /// </summary>
        [DataMember]
        public List<string> Descriptions;

        /// <summary>
        /// 排序
        /// </summary>
        [DataMember]
        public int Sort;

        /// <summary>
        /// 图片集
        /// </summary>
        [DataMember]
        public IList<string> Images;

        #region 视频音频地址链接 —— string VideoAudioLink
        /// <summary>
        /// 视频音频地址链接
        /// </summary>
        [DataMember]
        public string VideoAudioLink;
        #endregion

        #region 视频音频文件大小 —— string VideoAudioFileSize

        /// <summary>
        /// 视频音频文件大小
        /// </summary>
        [DataMember] 
        public string VideoAudioFileSize;
        #endregion

        #region 视频音频文件Id —— string VideoAudioFileId

        /// <summary>
        /// 视频音频文件Id
        /// </summary>
        [DataMember] public string VideoAudioFileId;
        #endregion

        #region 视频音频文件扩展名 —— string VideoAudioFileSuffix

        /// <summary>
        /// 视频音频扩展名
        /// </summary>
        [DataMember] public string VideoAudiogFileSuffix;
        #endregion

        #region 视频音频文件名称 —— string VideoAudioFileName

        /// <summary>
        /// 视频音频文件名称
        /// </summary>
        [DataMember] public string VideoAudioFileName;

        #endregion
    }
}
