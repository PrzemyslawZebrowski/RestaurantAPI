using Microsoft.AspNetCore.Authorization;

namespace RestaurantAPI.Authorization;

public class MinimumCreatedRestaurantsRequirement : IAuthorizationRequirement
{
    public MinimumCreatedRestaurantsRequirement(int minimumRestaurantsCreated)
    {
        MinimumRestaurantsCreated = minimumRestaurantsCreated;
    }

    public int MinimumRestaurantsCreated { get; }
}