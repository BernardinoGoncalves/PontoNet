using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ucl.PontoNet.Domain.Entities;

namespace Ucl.PontoNet.Infra.Data.Mappings
{
    public class PersonSampleMap : IEntityTypeConfiguration<PersonSample>
    {
        public void Configure(EntityTypeBuilder<PersonSample> builder)
        {
            builder.ToTable("PersonSamples");
            builder.HasKey(p => p.Id);
            // builder.Property(p => p.Id).HasColumnType("int").HasValueGenerator<IntKey>();;

            builder.Property(p => p.FirstName).IsRequired().HasColumnType("varchar(50)").HasMaxLength(50);
            builder.Property(p => p.LastName).IsRequired().HasColumnType("varchar(50)").HasMaxLength(50);
            builder.Property(p => p.DateBirth).IsRequired();
            builder.Property(p => p.Type).IsRequired();

        }
    }
}