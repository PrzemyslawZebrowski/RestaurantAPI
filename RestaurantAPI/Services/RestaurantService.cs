using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RestaurantAPI.Authorization;
using RestaurantAPI.Entities;
using RestaurantAPI.Exceptions;
using RestaurantAPI.Models;

namespace RestaurantAPI.Services;

public interface IRestaurantService
{
    RestaurantDto GetById(int id);
    PagedResults<RestaurantDto> GetAll(RestaurantQuery query);
    int Add(AddRestaurantDto dto);
    void DeleteById(int id);
    void UpdateById(int id, UpdateRestaurantDto restaurantDto);
}

public class RestaurantService : IRestaurantService
{
    private readonly IAuthorizationService _authorizationService;
    private readonly RestaurantDbContext _dbContext;
    private readonly ILogger<RestaurantService> _logger;
    private readonly IMapper _mapper;
    private readonly IUserContextService _userContextService;

    public RestaurantService(RestaurantDbContext dbContext, IMapper mapper, ILogger<RestaurantService> logger,
        IAuthorizationService authorizationService, IUserContextService userContextService)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _logger = logger;
        _authorizationService = authorizationService;
        _userContextService = userContextService;
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

    public PagedResults<RestaurantDto> GetAll(RestaurantQuery query)
    {
        var baseQuery = _dbContext.Restaurants
            .Include(r => r.Address)
            .Include(r => r.Dishes)
            .Where(r => query.SearchPhrase == null || r.Name.ToLower().Contains(query.SearchPhrase.ToLower())
                                                   || r.Description.ToLower().Contains(query.SearchPhrase.ToLower()));

        if (!string.IsNullOrEmpty(query.SortBy))
        {
            var columnsSelector = new Dictionary<string, Expression<Func<Restaurant, object>>>
            {
                {nameof(Restaurant.Name), r => r.Name},
                {nameof(Restaurant.Category), r => r.Category},
                {nameof(Restaurant.Description), r => r.Description}
            };
            var selectedColumn = columnsSelector[query.SortBy];

            baseQuery = query.SortDirection == SortDirection.ASC
                ? baseQuery.OrderBy(selectedColumn)
                : baseQuery.OrderByDescending(selectedColumn);
        }

        var restaurants = baseQuery
            .Skip(query.PageSize * (query.PageNumber - 1))
            .Take(query.PageSize)
            .ToList();

        if (!restaurants.Any())
            throw new NotFoundException("Restaurant not found");

        var restaurantDtos = _mapper.Map<List<RestaurantDto>>(restaurants);
        var totalItemCount = baseQuery.Count();
        var result = new PagedResults<RestaurantDto>(restaurantDtos, query.PageSize, totalItemCount, query.PageNumber);
        return result;
    }

    public int Add(AddRestaurantDto dto)
    {
        var restaurant = _mapper.Map<Restaurant>(dto);

        restaurant.CreatedById = _userContextService.GetUserId.Value;

        _dbContext.Restaurants.Add(restaurant);
        _dbContext.SaveChanges();

        return restaurant.Id;
    }

    public void DeleteById(int id)
    {
        _logger.LogError($"Restaurant with id {id} DELETE action invoked");
        var restaurant = _dbContext.Restaurants.FirstOrDefault(r => r.Id == id);

        if (restaurant is null)
            throw new NotFoundException("Restaurant not found");

        var authorizationResult = _authorizationService.AuthorizeAsync(_userContextService.User, restaurant,
            new ResourceOperationRequirement(ResourceOpetration.Delete)).Result;

        if (!authorizationResult.Succeeded) throw new ForbidException("Forbidden resource");

        _dbContext.Restaurants.Remove(restaurant);
        _dbContext.SaveChanges();
    }

    public void UpdateById(int id, UpdateRestaurantDto restaurantDto)
    {
        var restaurant = _dbContext.Restaurants.FirstOrDefault(r => r.Id == id);

        if (restaurant is null)
            throw new NotFoundException("Restaurant not found");

        var authorizationResult = _authorizationService.AuthorizeAsync(_userContextService.User, restaurant,
            new ResourceOperationRequirement(ResourceOpetration.Update)).Result;

        if (!authorizationResult.Succeeded) throw new ForbidException("Forbidden resource");

        restaurant.Name = restaurantDto.Name;
        restaurant.Description = restaurantDto.Description;
        restaurant.HasDelivery = restaurantDto.HasDelivery;

        _dbContext.SaveChanges();
    }
}