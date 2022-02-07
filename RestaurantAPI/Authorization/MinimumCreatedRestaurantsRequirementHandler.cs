using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using RestaurantAPI.Entities;

namespace RestaurantAPI.Authorization;

public class MinimumCreatedRestaurantsRequirementHandler : AuthorizationHandler<MinimumCreatedRestaurantsRequirement>
{
    private readonly RestaurantDbContext _dbContext;

    public MinimumCreatedRestaurantsRequirementHandler(RestaurantDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
        MinimumCreatedRestaurantsRequirement requirement)
    {
        var userId = int.Parse(context.User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value);
        var createdRestaurants = _dbContext.Restaurants.Count(r => r.CreatedById == userId);

        if (createdRestaurants >= requirement.MinimumRestaurantsCreated) context.Succeed(requirement);
        return Task.CompletedTask;
    }
}