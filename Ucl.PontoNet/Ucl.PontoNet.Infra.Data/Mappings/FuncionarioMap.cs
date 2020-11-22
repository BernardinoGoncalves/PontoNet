using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using Ucl.PontoNet.Domain.Entities;

namespace Ucl.PontoNet.Infra.Data.Mappings
{
    public class FuncionarioMap : IEntityTypeConfiguration<Funcionario>
    {
        public void Configure(EntityTypeBuilder<Funcionario> builder)
        {
            builder.ToTable("Funcionario");
            builder.HasKey(p => p.Matricula);

            builder.Property(p => p.Nome).IsRequired().HasColumnType("varchar(50)").HasMaxLength(50);
        }
    }
}
