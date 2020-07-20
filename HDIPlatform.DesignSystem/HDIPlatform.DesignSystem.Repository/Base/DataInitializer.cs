using ShSoft.Infrastructure.RepositoryBase;
using System.Data.Entity;

namespace HDIPlatform.DesignSystem.Repository.Base
{
    /// <summary>
    /// 数据初始化器实现
    /// </summary>
    public class DataInitializer : IDataInitializer
    {
        private readonly DbContext _dbContext;

        public DataInitializer()
        {
            this._dbContext = DbSession.CommandInstance;
        }

        /// <summary>
        /// 初始化基础数据
        /// </summary>
        public void Initialize()
        {

        }
    }
}
