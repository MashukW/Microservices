namespace Shared.Database.Entities
{
    public class PublicEntity : IPublicEntity
    {
        public int Id { get; set; }

        public Guid PublicId { get; set; }

        public bool IsInitial()
        {
            return PublicId == default || Id == default;
        }
    }

    public class DateCreatedPublicEntity : PublicEntity, IDateCreated
    {
        public DateTime DateCreated { get; set; }
    }

    public class DateUpdatedPublicEntity : PublicEntity, IDateUpdated
    {
        public DateTime DateUpdated { get; set; }
    }

    public class DateTrackedPublicEntity : PublicEntity, IDateTrackedPublicEntity
    {
        public DateTime DateCreated { get; set; }

        public DateTime DateUpdated { get; set; }
    }
}
