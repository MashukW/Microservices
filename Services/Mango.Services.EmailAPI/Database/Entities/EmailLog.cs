using Shared.Database.Entities;

namespace Mango.Services.EmailAPI.Database.Entities;

public class EmailLog : DateTrackedPublicEntity
{
    public EmailLog()
    {
        Email = string.Empty;
        Log = string.Empty;
    }

    public string Email { get; set; }

    public string Log { get; set; }
}
