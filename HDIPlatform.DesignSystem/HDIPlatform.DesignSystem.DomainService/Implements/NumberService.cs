using System;
using System.Linq;
using System.Text;
using HDIPlatform.DesignSystem.Domain.IDomainServices;
using SD.Toolkits.NoGenerator.Facade;
using ShSoft.Library.Infrastr.Helpers.Spell;

namespace HDIPlatform.DesignSystem.DomainService.Implements
{
    /// <summary>
    /// 编号领域服务实现
    /// </summary>
    public class NumberService : INumberService
    {
        #region # 常量、字段及依赖注入构造器

        /// <summary>
        /// 编号生成器
        /// </summary>
        private readonly NumberGenerator _generator;

        /// <summary>
        /// 默认格式化日期
        /// </summary>
        private readonly string _defaultFormatDate;

        /// <summary>
        /// 默认流水号长度
        /// </summary>
        private readonly int _defaultLength;

        /// <summary>
        /// 构造器
        /// </summary>
        public NumberService()
        {
            this._generator = new NumberGenerator();
            this._defaultFormatDate = DateTime.Now.ToString("yyyyMMdd");
            this._defaultLength = 3;
        }

        #endregion

        #region # 示例：生成示例编号 —— string GenerateExampleNo()
        /// <summary>
        /// 示例：生成示例编号
        /// </summary>
        /// <returns>示例编号</returns>
        public string GenerateExampleNo()
        {
            return this._generator.GenerateNumber("EX", this._defaultFormatDate, null, this._defaultLength, "示例编号");
        }
        #endregion

        #region # 生成套餐版本号 —— string GeneratePackVersionNo(Guid packId, string packType)
        /// <summary>
        /// 生成套餐版本号（套餐类型首拼两位流水日期）
        /// </summary>
        /// <param name="packId">源套餐Id</param>
        /// <param name="packType">套餐类型</param>
        /// <returns></returns>
        public string GeneratePackVersionNo(Guid packId, string packType)
        {
            //string pSourceString = packName;
            ////编号前缀：套餐名称首拼_套餐类型枚举值
            //if (packName.Length >= 5)
            //{
            //    pSourceString = packName.Substring(0, 5);
            //}
            //string propertyFirst = ChineseSpell.GetFirstSpell(pSourceString)[0];
            //StringBuilder prefixBuilder = new StringBuilder();
            //prefixBuilder.Append(propertyFirst);
            //prefixBuilder.Append(packType);

            string prefix = packId.ToString();
            //+2位流水
            string number = this._generator.GenerateNumber(prefix, string.Empty, "套餐版本集", 2, "套餐版本编号");
            //+年+月+日
            string timeNow = DateTime.Now.ToString("yyMMdd");
            StringBuilder numberBuilder = new StringBuilder();
            string be = number.Substring(prefix.Length, 2);
            numberBuilder.Append(packType);
            numberBuilder.Append(be);
            numberBuilder.Append(timeNow);
            return numberBuilder.ToString();
        }

        #endregion
    }
}
