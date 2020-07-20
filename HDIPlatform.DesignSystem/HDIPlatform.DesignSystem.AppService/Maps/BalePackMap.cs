using System;
using System.Collections.Generic;
using System.Linq;
using HDIPlatform.DesignSystem.Domain.Entities.BaleContext;
using HDIPlatform.DesignSystem.Domain.Mediators;
using HDIPlatform.DesignSystem.IAppService.DTOs.Outputs.BaleContext;
using ShSoft.Common.PoweredByLee;
using ShSoft.ValueObjects.Enums;

namespace HDIPlatform.DesignSystem.AppService.Maps
{
    /// <summary>
    /// 大包/定制套餐相关映射工具类
    /// </summary>
    public static class BalePackMap
    {

        #region 大包/定制套餐映射 —— static BalePackInfo ToDTO(this BalePack balePack)

        /// <summary>
        ///  大包/定制套餐映射
        /// </summary>
        /// <param name="balePack"></param>
        /// <returns></returns>
        public static BalePackInfo ToDTO(this BalePack balePack)
        {
            BalePackInfo balePackInfo = Transform<BalePack, BalePackInfo>.Map(balePack);
            return balePackInfo;
        }

        #endregion

        #region 套餐选区映射 —— static ChoiceAreaInfo ToDTO(this ChoiceArea choiceArea)

        /// <summary>
        /// 套餐选区映射
        /// </summary>
        /// <param name="choiceArea"></param>
        /// <param name="repMediator"></param>
        /// <returns></returns>
        public static BalePackChoiceAreaInfo ToDTO(this BalePackChoiceArea choiceArea, RepositoryMediator repMediator)
        {
            BalePackChoiceAreaInfo choiceAreaInfo = Transform<BalePackChoiceArea, BalePackChoiceAreaInfo>.Map(choiceArea);
            choiceAreaInfo.BalePackInfo = choiceArea.BalePack.ToDTO();
            if (choiceArea.BalePack.BalePackType == BalePackType.Customized)
            {
                choiceAreaInfo.ExistsSku = repMediator.BalePackGroupRep.IsCustomizedPackShelfed(new List<Guid> { choiceArea.Id });
            }
            if (choiceArea.BalePack.BalePackType == BalePackType.Bale)
            {
                choiceAreaInfo.ExistsSku = repMediator.BalePackGroupRep.IsBalePackShelfed(new List<Guid> { choiceArea.Id });
            }
            return choiceAreaInfo;
        }
        #endregion

        #region 选区组映射 —— static GroupInChoiceAreaInfo ToDTO(this GroupInChoiceArea group)

        /// <summary>
        /// 选区组映射
        /// </summary>
        /// <param name="group"></param>
        /// <returns></returns>
        public static BalePackGroupInfo ToDTO(this BalePackGroup group)
        {
            BalePackGroupInfo groupInChoiceAreaInfo = Transform<BalePackGroup, BalePackGroupInfo>.Map(group);
            //定制套餐
            groupInChoiceAreaInfo.ExistsSku = group.BalePackCategorys.All(s => s.BalePackProducts.Any());
            return groupInChoiceAreaInfo;
        }

        #endregion

        #region 组品类映射 —— static CategoryInGroupInfo ToDTO(this CategoryInGroup categoryInGroup)

        /// <summary>
        /// 组品类映射
        /// </summary>
        /// <param name="categoryInGroup"></param>
        /// <returns></returns>
        public static BalePackCategoryInfo ToDTO(this BalePackCategory categoryInGroup)
        {
            BalePackCategoryInfo categoryInGroupInfo = Transform<BalePackCategory, BalePackCategoryInfo>.Map(categoryInGroup);
            categoryInGroupInfo.BalePackGroupInfo = categoryInGroup.BalePackGroup.ToDTO();
            return categoryInGroupInfo;
        }
        #endregion

        #region 品类商品映射 —— static BalePackProductInfo ToDTO(this BalePackProduct product)

        /// <summary>
        /// 品类商品映射
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        public static BalePackProductInfo ToDTO(this BalePackProduct product)
        {
            BalePackProductInfo balePackProductInfo = Transform<BalePackProduct, BalePackProductInfo>.Map(product);
            balePackProductInfo.BalePackCategoryInfo = product.BalePackCategory.ToDTO();
            return balePackProductInfo;
        }

        #endregion


    }
}
