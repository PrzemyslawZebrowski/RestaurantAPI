using Microsoft.AspNetCore.Authorization;

namespace RestaurantAPI.Authorization
{
    public enum ResourceOpetration
    {
        Create,
        Read,
        Update,
        Delete,
    }
    public class ResourceOperationRequirement : IAuthorizationRequirement
    {
        public ResourceOperationRequirement(ResourceOpetration resourceOpetration)
        {
            ResourceOpetration = resourceOpetration;
        }

        public ResourceOpetration ResourceOpetration { get; }
    }
}
