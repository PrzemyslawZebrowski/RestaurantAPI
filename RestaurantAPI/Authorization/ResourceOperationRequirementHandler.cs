﻿using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using RestaurantAPI.Entities;

namespace RestaurantAPI.Authorization;

public class ResourceOperationRequirementHandler : AuthorizationHandler<ResourceOperationRequirement, Restaurant>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
        ResourceOperationRequirement requirement,
        Restaurant restaurant)
    {
        if (requirement.ResourceOpetration == ResourceOpetration.Read ||
            requirement.ResourceOpetration == ResourceOpetration.Create)
            context.Succeed(requirement);

        var userId = int.Parse(context.User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value);
        if (restaurant.CreatedById == userId) context.Succeed(requirement);
        return Task.CompletedTask;
    }
}