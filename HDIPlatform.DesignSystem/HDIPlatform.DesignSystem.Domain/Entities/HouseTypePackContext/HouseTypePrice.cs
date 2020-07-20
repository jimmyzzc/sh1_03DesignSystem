using System;
using System.Text;
using ShSoft.Infrastructure.EntityBase;

namespace HDIPlatform.DesignSystem.Domain.Entities.HouseTypePackContext
{
    /// <summary>
    /// 户型定价
    /// </summary>
    public class HouseTypePrice : AggregateRootEntity
    {
        #region # 构造器

        #region 01.无参构造器
        /// <summary>
        /// 无参构造器
        /// </summary>
        protected HouseTypePrice() { }
        #endregion

        #region 02.创建套餐模板构造器

        /// <summary>
        /// 户型定价
        /// </summary>
        /// <param name="propertyId">楼盘Id</param>
        /// <param name="propertyName">楼盘名称</param>
        /// <param name="propertyAddress">楼盘地址</param>
        /// <param name="houseTypeId">户型Id</param>
        /// <param name="houseTName">户型名称</param>
        /// <param name="unitPrice">使用面积单价</param>
        /// <param name="operatorId">操作人</param>
        /// <param name="operator">操作人</param>
        public HouseTypePrice(Guid propertyId, string propertyName, string propertyAddress, Guid houseTypeId, string houseTName, decimal unitPrice, Guid operatorId, string @operator)
            : this()
        {
            this.PropertyId = propertyId;
            this.PropertyName = propertyName;
            this.PropertyAddress = propertyAddress;
            this.HouseTypeId = houseTypeId;
            this.HouseTName = houseTName;
            this.UnitPrice = unitPrice;
            Operator = @operator;
            OperatorId = operatorId;
            this.InitKeywords();
        }

        #endregion

        #endregion

        #region # 属性


        #region 楼盘Id —— Guid PropertyId
        /// <summary>
        /// 楼盘Id
        /// </summary>
        public Guid PropertyId { get; private set; }
        #endregion

        #region 楼盘名称 —— string PropertyName
        /// <summary>
        /// 楼盘名称
        /// </summary>
        public string PropertyName { get; private set; }
        #endregion

        #region 楼盘地址 —— string PropertyAddress
        /// <summary>
        /// 楼盘地址
        /// </summary>
        public string PropertyAddress { get; private set; }
        #endregion

        #region 户型Id —— Guid HouseTypeId
        /// <summary>
        /// 户型Id
        /// </summary>
        public Guid HouseTypeId { get; private set; }
        #endregion

        #region 户型名称 —— string HouseTypeName
        /// <summary>
        /// 户型名称
        /// </summary>
        public string HouseTName { get; private set; }
        #endregion

        #region 使用面积单价 —— decimal UnitPrice
        /// <summary>
        /// 使用面积单价
        /// </summary>
        public decimal UnitPrice { get; private set; }
        #endregion

        #region 操作人Id —— Guid OperatorId
        /// <summary>
        /// 操作人Id
        /// </summary>
        public Guid OperatorId { get; private set; }
        #endregion

        #region 操作人 —— string Operator
        /// <summary>
        /// 操作人
        /// </summary>
        public string Operator { get; private set; }
        #endregion

        #endregion

        #region # 方法

        #region # 修改单价 —— UpdateUnitPrice(decimal unitPrice)

        /// <summary>
        /// 修改单价
        /// </summary>
        /// <param name="unitPrice">单价</param>
        /// <param name="operatorId">操作人</param>
        /// <param name="operator">操作人</param>
        public void UpdateUnitPrice(decimal unitPrice, Guid operatorId, string @operator)
        {
            this.UnitPrice = unitPrice;
            Operator = @operator;
            OperatorId = operatorId;
        }
        #endregion

        #region 初始化关键字 —— void InitKeywords()
        /// <summary>
        /// 初始化关键字
        /// </summary>
        private void InitKeywords()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(this.PropertyName);

            base.SetKeywords(builder.ToString());
        }
        #endregion
        #endregion
    }
}
