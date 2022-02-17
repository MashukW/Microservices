using Shared.Database.Entities;

namespace Shared.Database
{
    public static class EntityHelper
    {
        public static bool IsDefault(this IPublicEntity publicEntity)
        {
            return publicEntity.PublicId == Guid.Empty;
        }

        public static bool IsDefault(this IEntity entity)
        {
            return entity.Id == default;
        }
    }
}
