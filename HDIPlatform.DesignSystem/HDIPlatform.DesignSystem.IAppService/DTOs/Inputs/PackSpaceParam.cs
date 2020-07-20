using ShSoft.ValueObjects.Enums;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace HDIPlatform.DesignSystem.IAppService.DTOs.Inputs
{
    /// <summary>
    /// 套餐模板空间参数模型
    /// </summary>
    /// <remarks>勿忘[DataMember]</remarks>
    [DataContract(Namespace = "http://HDIPlatform.DesignSystem.IAppService.DTOs.Inputs")]
    public struct PackSpaceParam
    {
        /// <summary>
        /// 套餐模板空间Id
        /// </summary>
        [DataMember]
        public Guid? SpaceId;

        /// <summary>
        /// 空间名称
        /// </summary>
        [DataMember]
        public string SpaceName;

        /// <summary>
        /// 空间面积
        /// </summary>
        [DataMember]
        public float Square;

        /// <summary>
        /// 空间类型
        /// </summary>
        [DataMember]
        public SpaceType SpaceType;

        /// <summary>
        /// 是否删除
        /// </summary>
        [DataMember]
        public bool IsDeleted;

        /// <summary>
        /// 套餐模板空间详情集
        /// </summary>
        [DataMember]
        public IEnumerable<DecorationPackSpaceDetailParam> SpaceDetailParams;

        /// <summary>
        /// 排序
        /// </summary>
        [DataMember]
        public int Sort;

        /// <summary>
        /// 套餐选区集
        /// </summary>
        [DataMember] public IEnumerable<PackItemParam> PackItemParams;

        /// <summary>
        /// 套餐推荐选区集
        /// </summary>
        [DataMember] public IEnumerable<PackRecommendedItemParam> PackRecommendedItemParams;
    }
}
