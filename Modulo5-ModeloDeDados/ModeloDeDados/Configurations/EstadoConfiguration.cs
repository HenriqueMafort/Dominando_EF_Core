using Curso.Data;
using Curso.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Curso.Configuration
{
    public class EstadoConfiguration : IEntityTypeConfiguration<Estado>
    {
        public void Configure(EntityTypeBuilder<Estado> builder)
        {
            builder.HasOne(p => p.Governador)
            .WithOne(p => p.Estado)
            .HasForeignKey<Governador>(p => p.Id);


            builder.Navigation(p => p.Governador).AutoInclude();
            builder.Navigation(p => p.Cidades).AutoInclude();

            builder
            .HasMany(p => p.Cidades)
            .WithOne(p => p.Estado)
            //.OnDelete(DeleteBehavior.Restrict);
        }
    }
}