namespace Shared.Database.Entities
{
    public interface IPublicEntity : IEntity
    {
        public Guid PublicId { get; set; }
    }
}
