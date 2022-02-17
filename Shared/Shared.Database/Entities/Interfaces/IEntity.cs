namespace Shared.Database.Entities
{
    public interface IEntity
    {
        public int Id { get; set; }

        bool IsInitial();
    }
}
