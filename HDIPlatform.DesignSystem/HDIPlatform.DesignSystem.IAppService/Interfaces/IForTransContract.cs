using ShSoft.Infrastructure;
using System;
using System.Collections.Generic;
using System.ServiceModel;

namespace HDIPlatform.DesignSystem.IAppService.Interfaces
{
    /// <summary>
    /// For事务流转服务契约接口
    /// </summary>
    [ServiceContract(Namespace = "http://HDIPlatform.DesignSystem.IAppService.Interfaces")]
    public interface IForTransContract : IApplicationService
    {
        #region # 下架套餐模板商品SKU —— void OffShelfDecorationPackSku(Guid skuId)
        /// <summary>
        /// 下架套餐模板商品SKU
        /// </summary>
        /// <param name="skuId">商品SKU Id</param>
        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        void OffShelfDecorationPackSku(Guid skuId);
        #endregion

        #region # 重新上架套餐模板商品SKU —— void ReOnShelfDecorationPackSku(Guid skuId)
        /// <summary>
        /// 重新上架套餐模板商品SKU
        /// </summary>
        /// <param name="skuId">商品SKU Id</param>
        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        void ReOnShelfDecorationPackSku(Guid skuId);
        #endregion

        #region # 下架套餐模板工艺 —— void OffShelfDecorationPackCraft(Guid craftEntityId)
        /// <summary>
        /// 下架套餐模板工艺
        /// </summary>
        /// <param name="craftEntityId">工艺实体Id</param>
        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        void OffShelfDecorationPackCraft(Guid craftEntityId);
        #endregion

        #region # 重新上架套餐模板工艺 —— void ReOnShelfDecorationPackCraft(Guid craftEntityId)
        /// <summary>
        /// 重新上架套餐模板工艺
        /// </summary>
        /// <param name="craftEntityId">工艺实体Id</param>
        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        void ReOnShelfDecorationPackCraft(Guid craftEntityId);
        #endregion

        #region # 下架定制套餐商品SKU —— void OffShelfBalePackSku(Guid skuId)
        /// <summary>
        /// 下架定制套餐商品SKU
        /// </summary>
        /// <param name="skuId">商品SKU Id</param>
        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        void OffShelfBalePackSku(Guid skuId);
        #endregion

        #region # 重新上架定制套餐商品SKU ——void ReOnShelfBalePackSku(Guid skuId)
        /// <summary>
        /// 重新上架定制套餐商品SKU
        /// </summary>
        /// <param name="skuId">商品SKU Id</param>
        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        void ReOnShelfBalePackSku(Guid skuId);
        #endregion



        #region # 工艺是否在套餐中使用 —— bool IsCraftInPack(IEnumerable<Guid> craftEntityIds)
        /// <summary>
        /// 工艺是否在套餐中使用
        /// </summary>
        /// <param name="craftEntityIds">工艺实体Id集</param>
        /// <returns></returns>
        [OperationContract]
        bool IsCraftInPack(IEnumerable<Guid> craftEntityIds);
        #endregion
    }
}
 