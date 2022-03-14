using Mango.Services.EmailAPI.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Mango.Services.EmailAPI.Database.EntityConfigurations;

public class EmailLogEntityConfiguration : IEntityTypeConfiguration<EmailLog>
{
    public void Configure(EntityTypeBuilder<EmailLog> builder)
    {
        builder.ToTable($"{nameof(EmailLog)}s");

        builder.HasKey(p => p.Id);
        builder.Property(p => p.PublicId).IsRequired();
        builder.Property(p => p.DateCreated).IsRequired();
        builder.Property(p => p.DateUpdated).IsRequired();
    }
}
