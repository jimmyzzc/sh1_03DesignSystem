using HDIPlatform.DesignSystem.Domain.IRepositories.ArticleContext;
using HDIPlatform.DesignSystem.Domain.IRepositories.BaleContext;
using HDIPlatform.DesignSystem.Domain.IRepositories.ConfigContext;
using HDIPlatform.DesignSystem.Domain.IRepositories.FixedMealContext;
using HDIPlatform.DesignSystem.Domain.IRepositories.HouseTypePackContext;

namespace HDIPlatform.DesignSystem.Domain.Mediators
{
    /// <summary>
    /// 仓储中介者
    /// </summary>
    public sealed class RepositoryMediator
    {
        #region # 依赖注入构造器

        /// <summary>
        /// 依赖注入构造器
        /// </summary>
        public RepositoryMediator(IDecorationPackRepository decorationPackRep, IBalePackRepository balePackRep, IBalePackGroupRepository balePackGroupRep,
            IBalePackProductRepository balePackProductRep, IDecorationPackCraftRepository decorationPackCraftRep, IDecorationPackItemRepository decorationPackItemRep,
            IDecorationPackSchemeRepository decorationPackSchemeRep, IDecorationPackSchemeSpaceRepository decorationPackSchemeSpaceRep,
            IDecorationPackSkuRepository decorationPackSkuRep, IDecorationPackSpaceRepository decorationPackSpaceRep, IBalePackChoiceAreaRepository balePackChoiceAreaRep,
            IDecorationPack_BalePackRepository decorationPackBalePackRep, IHouseTypePriceRepository houseTypePriceRep, IPackSeriesRepository packSeriesRepository, IDecorationPackRecommendedItemRepository decorationPackRecommendedItemRep, IArticleRepository articleRepository, IArticleCategoryRepository articleCategoryRepository)
        {
            this.DecorationPackRep = decorationPackRep;
            this.BalePackRep = balePackRep;
            this.BalePackGroupRep = balePackGroupRep;
            this.BalePackProductRep = balePackProductRep;
            this.DecorationPackCraftRep = decorationPackCraftRep;
            this.DecorationPackItemRep = decorationPackItemRep;
            this.DecorationPackSchemeRep = decorationPackSchemeRep;
            this.DecorationPackSchemeSpaceRep = decorationPackSchemeSpaceRep;
            this.DecorationPackSkuRep = decorationPackSkuRep;
            this.DecorationPackSpaceRep = decorationPackSpaceRep;
            BalePackChoiceAreaRep = balePackChoiceAreaRep;
            this.DecorationPack_BalePackRep = decorationPackBalePackRep;
            HouseTypePriceRep = houseTypePriceRep;
            PackSeriesRepository = packSeriesRepository;
            this.DecorationPackRecommendedItemRep = decorationPackRecommendedItemRep;
            ArticleRepository = articleRepository;
            ArticleCategoryRepository = articleCategoryRepository;
        }

        #endregion

        #region # 属性

        /// <summary>
        /// 套餐系列仓储接口
        /// </summary>
        public IPackSeriesRepository PackSeriesRepository { get; private set; }

        /// <summary>
        /// 套餐模板仓储接口
        /// </summary>
        public IDecorationPackRepository DecorationPackRep { get; private set; }

        /// <summary>
        /// 套餐模板工艺项仓储接口
        /// </summary>
        public IDecorationPackCraftRepository DecorationPackCraftRep { get; private set; }

        /// <summary>
        /// 套餐模板项仓储接口
        /// </summary>
        public IDecorationPackItemRepository DecorationPackItemRep { get; private set; }

        /// <summary>
        /// 套餐模板推荐项仓储接口
        /// </summary>
        public IDecorationPackRecommendedItemRepository DecorationPackRecommendedItemRep { get; private set; }

        /// <summary>
        /// 套餐模板方案仓储接口
        /// </summary>
        public IDecorationPackSchemeRepository DecorationPackSchemeRep { get; private set; }

        /// <summary>
        /// 套餐模板方案空间仓储接口
        /// </summary>
        public IDecorationPackSchemeSpaceRepository DecorationPackSchemeSpaceRep { get; private set; }

        /// <summary>
        /// 套餐模板商品SKU项仓储接口
        /// </summary>
        public IDecorationPackSkuRepository DecorationPackSkuRep { get; private set; }

        /// <summary>
        /// 套餐模板空间仓储接口
        /// </summary>
        public IDecorationPackSpaceRepository DecorationPackSpaceRep { get; private set; }

        /// <summary>
        /// 套餐模板 - 大包/定制模板仓储接口
        /// </summary>
        public IDecorationPack_BalePackRepository DecorationPack_BalePackRep { get; private set; }


        /// <summary>
        /// 大包/定制套餐仓储接口
        /// </summary>
        public IBalePackRepository BalePackRep { get; private set; }

        /// <summary>
        /// 套餐选区内组仓储接口
        /// </summary>
        public IBalePackGroupRepository BalePackGroupRep { get; private set; }

        /// <summary>
        /// 套餐品类配置商品仓储接口
        /// </summary>
        public IBalePackProductRepository BalePackProductRep { get; private set; }


        /// <summary>
        /// 套餐选区仓储接口
        /// </summary>
        public IBalePackChoiceAreaRepository BalePackChoiceAreaRep { get; private set; }


        /// <summary>
        /// 户型定价仓储接口
        /// </summary>
        public IHouseTypePriceRepository HouseTypePriceRep { get; private set; }

        /// <summary>
        /// 文章仓储接口
        /// </summary>
        public IArticleRepository ArticleRepository { get; private set; }

        /// <summary>
        /// 文章分类仓储接口
        /// </summary>
        public IArticleCategoryRepository ArticleCategoryRepository { get; private set; }
        #endregion
    }
}
