using System.Data.Entity.ModelConfiguration;
using HDIPlatform.DesignSystem.Domain.Entities.FixedMealContext;
using ShSoft.Infrastructure.Constants;

namespace HDIPlatform.DesignSystem.Repository.EntityConfigurations
{
    /// <summary>
    /// 整装套餐实体映射配置
    /// </summary>
    public class DecorationPackConfig : EntityTypeConfiguration<DecorationPack>
    {
        /// <summary>
        /// 构造器
        /// </summary>
        public DecorationPackConfig()
        {
            //在此处写Fluent API表达式
            this.HasMany(temp => temp.PackSeries)
                .WithMany(temp => temp.DecorationPacks)
                .Map(map =>
                map.ToTable(string.Format("{0}DecorationPack_PackSeries", WebConfigSetting.TablePrefix)));
        }
    }
}
