using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Entities;


namespace RestaurantAPI.Authorization
{
    public class MinimumCreatedRestaurantsRequirementHandler : AuthorizationHandler<MinimumCreatedRestaurantsRequirement>
    {
        private readonly RestaurantDbContext _dbContext;

        public MinimumCreatedRestaurantsRequirementHandler(RestaurantDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, MinimumCreatedRestaurantsRequirement requirement)
        {
            int userId = int.Parse(context.User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value);
            int createdRestaurants = _dbContext.Restaurants.Count(r => r.CreatedById == userId);

            if (createdRestaurants >= requirement.MinimumRestaurantsCreated)
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }
    }
}
