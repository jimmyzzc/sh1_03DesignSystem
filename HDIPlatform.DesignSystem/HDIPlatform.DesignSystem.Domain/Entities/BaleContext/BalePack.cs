using ShSoft.Common.PoweredByLee;
using ShSoft.Infrastructure.EntityBase;
using ShSoft.ValueObjects.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HDIPlatform.DesignSystem.Domain.EventSources.BaleContext;
using HDIPlatform.DesignSystem.Domain.EventSources.FixedMealContext;
using ShSoft.Infrastructure.EventBase.Mediator;

namespace HDIPlatform.DesignSystem.Domain.Entities.BaleContext
{
    /// <summary>
    /// 大包/定制套餐
    /// </summary>
    [Serializable]
    public class BalePack : AggregateRootEntity
    {
        #region # 构造器

        #region 01.无参构造器

        /// <summary>
        /// 无参构造器
        /// </summary>
        protected BalePack()
        {

            //初始化导航属性
            this.ChoiceAreas = new HashSet<BalePackChoiceArea>();
            //this.DiscountHistorys = new HashSet<DiscountHistory>();
            //默认值
            this.PackShelfStatus = ShelfStatus.NotShelfed;
            this.HasOffShelvedSku = false;
            this.Views = 0;
            this.Sales = 0;
            IsClone = false;
            this.AddedTime = DateTime.Now;
        }
        #endregion

        #region 02.创建套餐构造器
        /// <summary>
        /// 创建套餐构造器
        /// </summary>
        /// <param name="packName">套餐名称</param>
        /// <param name="packType">套餐类型</param>
        /// <param name="creater">操作人名称</param>
        /// <param name="createrId">操作人Id</param>
        public BalePack(string packName, BalePackType packType, string creater, Guid createrId)
            : this()
        {
            base.Name = packName;
            this.BalePackType = packType;
            this.Creater = creater;
            this.CreaterId = createrId;
            this.InitKeywords();

        }
        #endregion

        #endregion

        #region # 属性
        #region 创建时间 —— new DateTime AddedTime
        /// <summary>
        /// 创建时间
        /// </summary>
        public new DateTime AddedTime { get; private set; }
        #endregion

        #region  折扣比例 —— float DiscountRatio
        /// <summary>
        /// 折扣比例
        /// </summary>
        public float DiscountRatio { get; private set; }

        #endregion

        #region 套餐状态 —— ShelfStatus PackShelfStatus

        /// <summary>
        /// 套餐状态
        /// </summary>
        public ShelfStatus PackShelfStatus { get; private set; }
        #endregion

        #region 套餐类型 ——BalePackType BalePackType

        /// <summary>
        /// 套餐类型
        /// </summary>
        public BalePackType BalePackType { get; private set; }
        #endregion

        #region 内部属性 - 标签集序列化字符串  —— string LabelStr
        /// <summary>
        /// 标签
        /// </summary>
        public string LabelStr { get; private set; }
        #endregion

        #region 只读属性 - 标签集 —— HashSet<string> Labels
        /// <summary>
        /// 只读属性 - 标签
        /// </summary>
        public HashSet<string> Labels
        {
            get
            {
                if (string.IsNullOrWhiteSpace(this.LabelStr))
                {
                    return new HashSet<string>();
                }
                return this.LabelStr.JsonToObject<HashSet<string>>();
            }
        }
        #endregion

        #region 套餐浏览量 —— int Views
        /// <summary>
        /// 套餐浏览量
        /// </summary>
        public int Views { get; private set; }
        #endregion

        #region 套餐销售量 —— int Sales
        /// <summary>
        /// 套餐销售量
        /// </summary>
        public int Sales { get; private set; }
        #endregion

        #region  封面 —— string CoverDrawing

        /// <summary>
        /// 套餐封面
        /// </summary>
        public string CoverDrawing { get; private set; }
        #endregion

        #region 是否有下架商品 —— bool HasOffShelvedSku
        /// <summary>
        /// 是否有下架商品
        /// </summary>
        public bool HasOffShelvedSku { get; private set; }
        #endregion

        #region 创建人 —— string Creater
        /// <summary>
        /// 创建人
        /// </summary>
        public string Creater { get; private set; }
        #endregion

        #region 创建人Id —— Guid CreaterId
        /// <summary>
        /// 创建人Id
        /// </summary>
        public Guid CreaterId { get; private set; }
        #endregion

        #region 源套餐Id —— Guid? SourcePackId
        /// <summary>
        /// 源套餐Id(源套餐一直为空)
        /// </summary>
        public Guid? SourcePackId { get; private set; }
        #endregion

        #region 版本号 —— string VersionNumber
        /// <summary>
        /// 版本号(源套餐一直为空)（套餐名称首拼套餐类型首拼两位流水日期）
        /// </summary>
        public string VersionNumber { get; private set; }
        #endregion

        #region 是否克隆 —— bool IsClone
        /// <summary>
        /// 是否克隆
        /// </summary>
        public bool IsClone { get; private set; }
        #endregion

        #region 导航属性 - 套餐选区集 —— ICollection<ChoiceArea> ChoiceAreas
        /// <summary>
        /// 导航属性 - 套餐选区集
        /// </summary>
        public virtual ICollection<BalePackChoiceArea> ChoiceAreas { get; private set; }
        #endregion

        #endregion

        #region # 方法

        #region 初始化关键字 —— void InitKeywords()
        /// <summary>
        /// 初始化关键字
        /// </summary>
        private void InitKeywords()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(base.Name);

            base.SetKeywords(builder.ToString());
        }
        #endregion

        #region 设置套餐折扣比例 ——  void SetDiscountRatio(float discountRatio)
        /// <summary>
        /// 设置套餐折扣比例
        /// </summary>
        /// <param name="discountRatio">折扣比例</param>
        public void SetDiscountRatio(float discountRatio)
        {
            Assert.IsTrue(discountRatio.IsPercentage(), "折扣比例必须大于0，请重新配置");
            this.DiscountRatio = discountRatio;
        }
        #endregion

        #region 设置套餐上架 —— void SetPackShelfed()
        /// <summary>
        /// 设置套餐上架
        /// </summary>
        public void SetPackShelfed()
        {
            Assert.IsTrue(this.DiscountRatio.IsPercentage(), "折扣比例必须大于0");
            this.PackShelfStatus = ShelfStatus.Shelfed;

            //TODO　克隆套餐副本
            EventMediator.Suspend(new OnShelfBalePackEvent(this.Id));
        }
        #endregion

        #region 设置套餐下架 —— void SetPackShelfOff()
        /// <summary>
        /// 设置套餐下架
        /// </summary>
        public void SetPackShelfOff()
        {
            this.PackShelfStatus = ShelfStatus.ShelfOff;
        }
        #endregion

        #region 设置套餐标签 —— void SetLabelStr(IEnumerable<string> labels)

        /// <summary>
        /// 设置套餐标签
        /// </summary>
        /// <param name="labels"></param>
        public void SetLabelStr(IEnumerable<string> labels)
        {
            this.LabelStr = labels.IsNullOrEmpty() ? string.Empty : labels.ToJson();
        }
        #endregion

        #region 设置套餐封面 —— void SetCoverDrawing(Guid coverDrawingId)

        /// <summary>
        /// 设置套餐封面
        /// </summary>
        /// <param name="coverDrawing">封面</param>
        public void SetCoverDrawing(string coverDrawing)
        {
            this.CoverDrawing = coverDrawing;
        }
        #endregion

        #region 新增套餐选区 —— void AddChoiceArea(ChoiceArea choiceArea)

        /// <summary>
        /// 新增套餐选区
        /// </summary>
        /// <param name="choiceArea"></param>
        public void AddChoiceArea(BalePackChoiceArea choiceArea)
        {
            this.ChoiceAreas.Add(choiceArea);
        }

        #endregion

        #region 删除套餐选区 —— void DeleteChoiceArea(ChoiceArea choiceArea)

        /// <summary>
        /// 删除套餐选区
        /// </summary>
        /// <param name="choiceArea"></param>
        public void DeleteChoiceArea(BalePackChoiceArea choiceArea)
        {
            #region 最后一个选区删除验证
            //if (this.ChoiceAreas.Count == 1)
            //{
            //    throw new InvalidOperationException("套餐必须存在一个选区，请重新操作！");
            //}
            #endregion
            this.ChoiceAreas.Remove(choiceArea);
        }

        #endregion

        #region 修改套餐名称及比例 —— void ModifyPackNameAndRatio(string packName, float discountRatio)

        /// <summary>
        /// 修改套餐名称及比例
        /// </summary>
        /// <param name="packName">套餐名称</param>
        /// <param name="discountRatio">折扣比例</param>
        public void ModifyPackNameAndRatio(string packName, float discountRatio)
        {
            base.Name = packName;
            this.SetDiscountRatio(discountRatio);

            this.InitKeywords();
        }
        #endregion

        #region 设置是否有下架商品 —— void SetHasOffShelvedSku(bool has)
        /// <summary>
        /// 设置是否有下架商品
        /// </summary>
        /// <param name="has">是否有下架商品</param>
        public void SetHasOffShelvedSku(bool has)
        {
            this.HasOffShelvedSku = has;
        }
        #endregion

        #region 获取套餐内选区 —— BalePackChoiceArea GetChoiceAreaById(Guid choiceAreaId)

        /// <summary>
        /// 获取套餐内选区
        /// </summary>
        /// <param name="choiceAreaId">选区Id</param>
        /// <returns>选区</returns>
        public BalePackChoiceArea GetChoiceAreaById(Guid choiceAreaId)
        {
            BalePackChoiceArea choiceArea = this.ChoiceAreas.SingleOrDefault(s => s.Id == choiceAreaId);

            #region # 验证

            if (choiceArea == null)
            {
                throw new ArgumentOutOfRangeException("choiceAreaId", string.Format("Id为\"{0}\"的套餐选区不存在！", choiceAreaId));
            }

            #endregion

            return choiceArea;
        }
        #endregion

        #region 设置浏览量 —— void SetViews()
        /// <summary>
        /// 设置浏览量
        /// </summary>
        public void SetViews()
        {
            this.Views++;
        }
        #endregion

        #region 设置销售量 —— void SetSales()
        /// <summary>
        /// 设置销售量
        /// </summary>
        public void SetSales()
        {
            this.Sales++;
        }
        #endregion

        #region 克隆套餐时设置源套餐Id，版本号 —— void SetSourcePackIdAndVersion(Guid sourcePackId, string version)
        /// <summary>
        /// 克隆套餐时设置源套餐Id，版本号
        /// </summary>
        /// <param name="sourcePackId">源套餐Id</param>
        /// <param name="version">版本号</param>
        public void SetSourcePackIdAndVersion(Guid sourcePackId, string version)
        {
            SourcePackId = sourcePackId;
            VersionNumber = version;
            IsClone = true;
            this.Sales = 0;
            this.Views = 0;
            this.AddedTime = DateTime.Now;

        }
        #endregion

        #endregion
    }
}
