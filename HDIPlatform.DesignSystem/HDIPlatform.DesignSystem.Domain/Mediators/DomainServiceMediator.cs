using HDIPlatform.DesignSystem.Domain.IDomainServices;

namespace HDIPlatform.DesignSystem.Domain.Mediators
{
    /// <summary>
    /// 领域服务中介者
    /// </summary>
    public sealed class DomainServiceMediator
    {
        /// <summary>
        /// 依赖注入构造器
        /// </summary>
        public DomainServiceMediator(INumberService numberSvc, IDecorationPackService decorationPackSvc, IPackSeriesService packSeriesService)
        {
            this.NumberSvc = numberSvc;
            this.DecorationPackSvc = decorationPackSvc;
            PackSeriesService = packSeriesService;
        }

        /// <summary>
        /// 编号领域服务接口
        /// </summary>
        public INumberService NumberSvc { get; private set; }

        /// <summary>
        /// 套餐模板领域服务接口
        /// </summary>
        public IDecorationPackService DecorationPackSvc { get; private set; }

        /// <summary>
        /// 套餐系列领域服务接口
        /// </summary>
        public IPackSeriesService PackSeriesService { get; private set; }
    }
}
