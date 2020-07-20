using ShSoft.Common.PoweredByLee;
using ShSoft.Infrastructure.EntityBase;
using System;
using System.Collections.Generic;

namespace HDIPlatform.DesignSystem.Domain.Entities.FixedMealContext
{
    /// <summary>
    /// 套餐模板方案空间
    /// </summary>
    [Serializable]
    public class DecorationPackSchemeSpace : PlainEntity
    {
        #region # 构造器

        #region 01.无参构造器
        /// <summary>
        /// 无参构造器
        /// </summary>
        protected DecorationPackSchemeSpace() { }
        #endregion

        #region 02.创建套餐模板方案空间构造器

        /// <summary>
        /// 创建套餐模板方案空间构造器
        /// </summary>
        /// <param name="packSpaceId">套餐空间Id</param>
        /// <param name="descriptions">描述集</param>
        /// <param name="sort">排序</param>
        /// <param name="images">图片集</param>
        /// <param name="videoAudioLink">视频音频地址链接</param>
        /// <param name="videoAudioFileName">视频音频文件名称</param>
        /// <param name="videoAudioFileSize">视频音频文件大小</param>
        /// <param name="videoAudioFileId">视频音频文件Id</param>
        /// <param name="videoAudiogFileSuffix">视频音频文件扩展名</param>
        public DecorationPackSchemeSpace(Guid packSpaceId, IList<string> descriptions, int sort, IList<string> images, string videoAudioLink, string videoAudioFileSize, string videoAudioFileId, string videoAudiogFileSuffix, string videoAudioFileName)
            : this()
        {
            this.PackSpaceId = packSpaceId;
            this.DescriptionsStr = descriptions.IsNullOrEmpty() ? null : descriptions.ToJson();
            base.Sort = sort;
            this.ImagesStr = images.IsNullOrEmpty()? null : images.ToJson();
            this.VideoAudioLink = videoAudioLink;
            this.VideoAudioFileId = videoAudioFileId;
            this.VideoAudioFileName = videoAudioFileName;
            this.VideoAudioFileSize = videoAudioFileSize;
            this.VideoAudiogFileSuffix = videoAudiogFileSuffix;
        }
        #endregion

        #endregion

        #region # 属性

        #region 套餐模板空间Id —— Guid PackSpaceId
        /// <summary>
        /// 套餐模板空间Id
        /// </summary>
        public Guid PackSpaceId { get; private set; }
        #endregion
        #region 只读属性 - 描述集 —— IList<string> Descriptions
        /// <summary>
        /// 只读属性 - 描述集
        /// </summary>
        public IList<string> Descriptions
        {
            get
            {
                return string.IsNullOrWhiteSpace(this.DescriptionsStr)
                    ? new string[0]
                    : this.DescriptionsStr.JsonToObject<IList<string>>();
            }
        }
        #endregion

        #region 描述 —— string DescriptionsStr
        /// <summary>
        /// 描述
        /// </summary>
        public string DescriptionsStr { get; private set; }
        #endregion

        #region 只读属性 - 图片集 —— IList<string> Images
        /// <summary>
        /// 只读属性 - 图片集
        /// </summary>
        public IList<string> Images
        {
            get
            {
                return string.IsNullOrWhiteSpace(this.ImagesStr)
                    ? new string[0]
                    : this.ImagesStr.JsonToObject<IList<string>>();
            }
        }
        #endregion

        #region 内部属性 - 图片集序列化字符串 —— string ImagesStr
        /// <summary>
        /// 内部属性 - 图片集序列化字符串
        /// </summary>
        public string ImagesStr { get; private set; }
        #endregion

        #region 视频音频地址链接 —— string VideoAudioLink
        /// <summary>
        /// 视频音频地址链接
        /// </summary>
        public string VideoAudioLink { get; private set; }
        #endregion

        #region 视频音频文件大小 —— string VideoAudioFileSize
        /// <summary>
        /// 视频音频文件大小
        /// </summary>
        public string VideoAudioFileSize { get; private set; }
        #endregion

        #region 视频音频文件Id —— string VideoAudioFileId
        /// <summary>
        /// 视频音频文件Id
        /// </summary>
        public string VideoAudioFileId { get; private set; }
        #endregion

        #region 视频音频文件扩展名 —— string VideoAudioFileSuffix
        /// <summary>
        /// 视频音频扩展名
        /// </summary>
        public string VideoAudiogFileSuffix { get; private set; }
        #endregion

        #region 视频音频文件名称 —— string VideoAudioFileName
        /// <summary>
        /// 视频音频文件名称
        /// </summary>
        public string VideoAudioFileName { get; private set; }
        #endregion

        #region 导航属性 - 套餐模板方案 —— DecorationPackScheme PackScheme
        /// <summary>
        /// 导航属性 - 套餐模板方案
        /// </summary>
        public virtual DecorationPackScheme PackScheme { get; private set; }
        #endregion

        #endregion

        #region # 方法

        #region 设置套餐模板空间Id —— void SetPackSpaceId(Guid packSpaceId)
        /// <summary>
        /// 设置套餐模板空间Id
        /// </summary>
        /// <param name="packSpaceId">套餐模板空间Id</param>
        /// <remarks>注意：克隆套餐模板时候用，勿乱用</remarks>
        public void SetPackSpaceId(Guid packSpaceId)
        {
            this.PackSpaceId = packSpaceId;
        }
        #endregion


        #endregion
    }
}
