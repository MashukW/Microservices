namespace Shared.Database.Entities
{
    public class Entity : IEntity
    {
        public int Id { get; set; }

        public bool IsInitial()
        {
            return Id == default;
        }
    }

    public class DateCreatedEntity : Entity, IDateCreated
    {
        public DateTime DateCreated { get; set; }
    }

    public class DateUpdatedEntity : Entity, IDateUpdated
    {
        public DateTime DateUpdated { get; set; }
    }

    public class DateTrackedEntity : Entity, IDateTrackedPublicEntity
    {
        public DateTime DateCreated { get; set; }

        public DateTime DateUpdated { get; set; }
    }
}
