using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RestaurantAPI.Authorization;
using RestaurantAPI.Entities;
using RestaurantAPI.Exceptions;
using RestaurantAPI.Models;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace RestaurantAPI.Services
{
    public interface IRestaurantService
    {
        RestaurantDto GetById(int id);
        IEnumerable<RestaurantDto> GetAll();
        int Add(AddRestaurantDto dto, int userId);
        void DeleteById(int id, ClaimsPrincipal user);
        void UpdateById(int id, UpdateRestaurantDto restaurantDto, ClaimsPrincipal user);
    }

    public class RestaurantService : IRestaurantService
    {
        private readonly RestaurantDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<RestaurantService> _logger;
        private readonly IAuthorizationService _authorizationService;

        public RestaurantService(RestaurantDbContext dbContext, IMapper mapper, ILogger<RestaurantService> logger,
            IAuthorizationService authorizationService)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
            _authorizationService = authorizationService;
        }
        public RestaurantDto GetById(int id)
        {
            var restaurant = _dbContext
                .Restaurants
                .Include(r => r.Address)
                .Include(r => r.Dishes)
                .FirstOrDefault(r => r.Id == id);
            if (restaurant is null)
                throw new NotFoundException("Restaurant not found");

            var result = _mapper.Map<RestaurantDto>(restaurant);
            return result;
        }

        public IEnumerable<RestaurantDto> GetAll()
        {
            var restaurants = _dbContext.Restaurants
                .Include(r => r.Address)
                .Include(r => r.Dishes)
                .ToList();

            if (!restaurants.Any())
                throw new NotFoundException("Restaurant not found");

            var result = _mapper.Map<List<RestaurantDto>>(restaurants);
            return result;
        }

        public int Add(AddRestaurantDto dto, int userId)
        {
            var restaurant = _mapper.Map<Restaurant>(dto);

            restaurant.CreatedById = userId;

            _dbContext.Restaurants.Add(restaurant);
            _dbContext.SaveChanges();

            return restaurant.Id;
        }

        public void DeleteById(int id, ClaimsPrincipal user)
        {
            _logger.LogError($"Restaurant with id {id} DELETE action invoked");
            var restaurant = _dbContext.Restaurants.FirstOrDefault(r => r.Id == id);

            if (restaurant is null)
                throw new NotFoundException("Restaurant not found");

            var authorizationResult = _authorizationService.AuthorizeAsync(user, restaurant,
                new ResourceOperationRequirement(ResourceOpetration.Delete)).Result;

            if (!authorizationResult.Succeeded)
            {
                throw new ForbidException("Forbidden resource");
            }

            _dbContext.Restaurants.Remove(restaurant);
            _dbContext.SaveChanges();
        }

        public void UpdateById(int id, UpdateRestaurantDto restaurantDto, ClaimsPrincipal user)
        {
            var restaurant = _dbContext.Restaurants.FirstOrDefault(r => r.Id == id);

            if (restaurant is null)
                throw new NotFoundException("Restaurant not found");

            var authorizationResult = _authorizationService.AuthorizeAsync(user, restaurant,
                new ResourceOperationRequirement(ResourceOpetration.Update)).Result;

            if (!authorizationResult.Succeeded)
            {
                throw new ForbidException("Forbidden resource");
            }

            restaurant.Name = restaurantDto.Name;
            restaurant.Description = restaurantDto.Description;
            restaurant.HasDelivery = restaurantDto.HasDelivery;

            _dbContext.SaveChanges();
        }
    }
}
