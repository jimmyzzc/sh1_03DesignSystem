using ShSoft.Common.PoweredByLee;
using ShSoft.Infrastructure.EntityBase;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HDIPlatform.DesignSystem.Domain.Entities.FixedMealContext
{
    /// <summary>
    /// 套餐模板方案
    /// </summary>
    [Serializable]
    public class DecorationPackScheme : AggregateRootEntity
    {
        #region # 构造器

        #region 01.无参构造器
        /// <summary>
        /// 无参构造器
        /// </summary>
        protected DecorationPackScheme()
        {
            //初始化导航属性
            this.SchemeSpaces = new HashSet<DecorationPackSchemeSpace>();
        }
        #endregion

        #region 02.创建套餐模板方案构造器

        /// <summary>
        /// 创建套餐模板方案构造器
        /// </summary>
        /// <param name="schemeName">方案名称</param>
        /// <param name="packId">套餐模板Id</param>
        /// <param name="cover">封面</param>
        /// <param name="description">描述</param>
        /// <param name="images">方案概况图片集</param>
        /// <param name="schemeDescriptions">方案概况描述集</param>
        /// <param name="videoAudioLink">视频音频地址链接</param>
        /// <param name="videoAudioFileName">视频音频文件名称</param>
        /// <param name="videoAudioFileSize">视频音频文件大小</param>
        /// <param name="videoAudioFileId">视频音频文件Id</param>
        /// <param name="videoAudiogFileSuffix">视频音频文件扩展名</param>
        /// <param name="isDefault">默认方案</param>
        /// <param name="schemeSpaces">方案空间集</param>
        public DecorationPackScheme(string schemeName, Guid packId, string cover, string description, IList<string> images, IList<string> schemeDescriptions, string videoAudioLink, string videoAudioFileSize, string videoAudioFileId, string videoAudiogFileSuffix, string videoAudioFileName, bool isDefault, IEnumerable<DecorationPackSchemeSpace> schemeSpaces)
            : this()
        {
            #region # 验证

            schemeSpaces = schemeSpaces == null ? new DecorationPackSchemeSpace[0] : schemeSpaces.ToArray();

            if (schemeSpaces.Count() != schemeSpaces.DistinctBy(x => x.PackSpaceId).Count())
            {
                throw new ArgumentOutOfRangeException("schemeSpaces", "套餐模板空间不可重复！");
            }

            #endregion

            base.Name = schemeName;
            this.PackId = packId;
            this.Cover = cover;
            this.Description = description;
            this.ImagesStr = images.IsNullOrEmpty() ? null : images.ToJson();
            this.SchemeDescriptionsStr = schemeDescriptions.IsNullOrEmpty() ? null : schemeDescriptions.ToJson();
            this.VideoAudioLink = videoAudioLink;
            this.VideoAudioFileId = videoAudioFileId;
            this.VideoAudioFileName = videoAudioFileName;
            this.VideoAudioFileSize = videoAudioFileSize;
            this.VideoAudiogFileSuffix = videoAudiogFileSuffix;
            this.IsDefault = isDefault;
            this.SchemeSpaces.AddRange(schemeSpaces);
        }
        #endregion

        #endregion

        #region # 属性

        #region 套餐模板Id —— Guid PackId
        /// <summary>
        /// 套餐模板Id
        /// </summary>
        public Guid PackId { get; private set; }
        #endregion

        #region 封面 —— string Cover
        /// <summary>
        /// 封面
        /// </summary>
        public string Cover { get; private set; }
        #endregion

        #region 封面描述 —— string Description
        /// <summary>
        /// 封面描述
        /// </summary>
        public string Description { get; private set; }
        #endregion


        #region 只读属性 - 方案概况图片集 —— IList<string> Images
        /// <summary>
        /// 只读属性 - 方案概况图片集
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

        #region 内部属性 - 方案概况图片集序列化字符串 —— string ImagesStr
        /// <summary>
        /// 内部属性 - 方案概况图片集序列化字符串
        /// </summary>
        public string ImagesStr { get; private set; }
        #endregion

        #region 只读属性 - 方案概况图片集 —— IList<string> SchemeDescriptionsStr
        /// <summary>
        /// 只读属性 - 方案概况图片集
        /// </summary>
        public IList<string> SchemeDescriptions
        {
            get
            {
                return string.IsNullOrWhiteSpace(this.SchemeDescriptionsStr)
                    ? new string[0]
                    : this.SchemeDescriptionsStr.JsonToObject<IList<string>>();
            }
        }
        #endregion

        #region 方案概况描述集 —— string SchemeDescriptionsStr
        /// <summary>
        /// 方案概况描述集
        /// </summary>
        public string SchemeDescriptionsStr { get; private set; }
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


        #region 默认方案 —— bool IsDefault
        /// <summary>
        /// 默认方案
        /// </summary>
        public bool IsDefault { get; private set; }
        #endregion

        #region 导航属性 - 方案空间集 —— ICollection<DecorationPackSchemeSpace> SchemeSpaces
        /// <summary>
        /// 导航属性 - 方案空间集
        /// </summary>
        public virtual ICollection<DecorationPackSchemeSpace> SchemeSpaces { get; private set; }
        #endregion

        #endregion

        #region # 方法

        #region 修改套餐模板方案 —— void UpdateInfo(string schemeName, string cover, string description...

        /// <summary>
        /// 修改套餐模板方案
        /// </summary>
        /// <param name="schemeName">方案名称</param>
        /// <param name="cover">封面</param>
        /// <param name="description">描述</param>
        /// <param name="images">方案概况图片集</param>
        /// <param name="schemeDescription">方案概况描述</param>
        /// <param name="videoAudioLink">视频音频地址链接</param>
        /// <param name="videoAudioFileName">视频音频文件名称</param>
        /// <param name="videoAudioFileSize">视频音频文件大小</param>
        /// <param name="videoAudioFileId">视频音频文件Id</param>
        /// <param name="videoAudiogFileSuffix">视频音频文件扩展名</param> 
        /// <param name="schemeSpaces">方案空间集</param>
        public void UpdateInfo(string schemeName, string cover, string description, IList<string> images, IList<string> schemeDescriptions, string videoAudioLink, string videoAudioFileSize, string videoAudioFileId, string videoAudiogFileSuffix, string videoAudioFileName, IEnumerable<DecorationPackSchemeSpace> schemeSpaces)
        {
            #region # 验证

            schemeSpaces = schemeSpaces == null ? new DecorationPackSchemeSpace[0] : schemeSpaces.ToArray();

            if (schemeSpaces.Count() != schemeSpaces.DistinctBy(x => x.PackSpaceId).Count())
            {
                throw new ArgumentOutOfRangeException("schemeSpaces", "套餐模板空间不可重复！");
            }

            #endregion

            base.Name = schemeName;
            this.Cover = cover;
            this.Description = description;
            this.ImagesStr = images.IsNullOrEmpty() ? null : images.ToJson();
            this.SchemeDescriptionsStr = schemeDescriptions.IsNullOrEmpty() ? null : schemeDescriptions.ToJson();
            this.VideoAudioLink = videoAudioLink; 
            this.VideoAudioFileId = videoAudioFileId;
            this.VideoAudioFileName = videoAudioFileName;
            this.VideoAudioFileSize = videoAudioFileSize;
            this.VideoAudiogFileSuffix = videoAudiogFileSuffix;
            //先清空
            foreach (DecorationPackSchemeSpace schemeSpace in this.SchemeSpaces.ToArray())
            {
                this.SchemeSpaces.Remove(schemeSpace);
            }

            //再添加
            this.SchemeSpaces.AddRange(schemeSpaces);
        }
        #endregion

        #region 设置套餐模板Id —— void SetPackId(Guid packId)
        /// <summary>
        /// 设置套餐模板Id
        /// </summary>
        /// <param name="packId">套餐模板Id</param>
        /// <remarks>注意：克隆套餐模板时候用，勿乱用</remarks>
        public void SetPackId(Guid packId)
        {
            this.PackId = packId;
        }
        #endregion

        #region 获取套餐模板方案空间 —— DecorationPackSchemeSpace GetSchemeSpace(Guid packSpaceId)
        /// <summary>
        /// 获取套餐模板方案空间
        /// </summary>
        /// <param name="packSpaceId">套餐模板空间Id</param>
        /// <returns>套餐模板方案空间</returns>
        public DecorationPackSchemeSpace GetSchemeSpace(Guid packSpaceId)
        {
            DecorationPackSchemeSpace currentSchemeSpace = this.SchemeSpaces.SingleOrDefault(x => x.PackSpaceId == packSpaceId);

            //if (currentSchemeSpace == null)
            //{
            //    throw new ArgumentOutOfRangeException("packSpaceId", string.Format("套餐模板空间Id为\"{0}\"的套餐模板方案空间不存在！", packSpaceId));
            //}

            return currentSchemeSpace;
        }
        #endregion

        #region 删除套餐模板方案空间 —— void RemoveSchemeSpace(DecorationPackSchemeSpace schemeSpace)
        /// <summary>
        /// 删除套餐模板方案空间
        /// </summary>
        /// <param name="schemeSpace">套餐模板方案空间</param>
        public void RemoveSchemeSpace(DecorationPackSchemeSpace schemeSpace)
        {
            this.SchemeSpaces.Remove(schemeSpace);
        }
        #endregion

        #region  设置|取消方案默认 —— void SetDefault(bool isDefault)
        /// <summary>
        /// 设置|取消方案默认
        /// </summary>
        /// <param name="isDefault"></param>
        public void SetDefault(bool isDefault)
        {
            this.IsDefault = isDefault;
        }

        #endregion

        #endregion
    }
}
