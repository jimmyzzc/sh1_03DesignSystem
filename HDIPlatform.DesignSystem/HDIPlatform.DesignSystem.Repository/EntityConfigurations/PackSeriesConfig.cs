using System.Data.Entity.ModelConfiguration;
using HDIPlatform.DesignSystem.Domain.Entities.ConfigContext;
using HDIPlatform.DesignSystem.Domain.Entities.FixedMealContext;
using ShSoft.Infrastructure.Constants;

namespace HDIPlatform.DesignSystem.Repository.EntityConfigurations
{
    /// <summary>
    /// 套餐系列实体映射配置
    /// </summary>
    public class PackSeriesConfig : EntityTypeConfiguration<PackSeries>
    {
        /// <summary>
        /// 构造器
        /// </summary>
        public PackSeriesConfig()
        {
            //在此处写Fluent API表达式
            //this.HasRequired(temp => temp.GroupName);
            this.Property(s => s.GroupName).IsRequired();
            this.HasMany(temp => temp.DecorationPacks)
                .WithMany(temp => temp.PackSeries)
                .Map(map =>
                map.ToTable(string.Format("{0}DecorationPack_PackSeries", WebConfigSetting.TablePrefix)));
        }
    }
}
