using System;
using ShSoft.ValueObjects.Enums;

namespace HDIPlatform.DesignSystem.Domain.IDomainServices
{
    /// <summary>
    /// 编号领域服务接口
    /// </summary>
    public interface INumberService
    {
        #region 示例：生成示例编号 —— string GenerateExampleNo()
        /// <summary>
        /// 示例：生成示例编号
        /// </summary>
        /// <returns>示例编号</returns>
        string GenerateExampleNo();
        #endregion

        #region # 生成套餐版本号 —— string GeneratePackVersionNo(Guid packId,string packType)
        /// <summary>
        /// 生成套餐版本号（套餐类型首拼两位流水日期）
        /// </summary>
        /// <param name="packId">源套餐Id</param>
        /// <param name="packType">套餐类型</param>
        /// <returns></returns>
        string GeneratePackVersionNo(Guid packId,string packType);
        #endregion
    }
}
