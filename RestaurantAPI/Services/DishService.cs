using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Entities;
using RestaurantAPI.Exceptions;
using RestaurantAPI.Models;

namespace RestaurantAPI.Services;

public interface IDishService
{
    int AddDish(int restaurantId, AddDishDto dto);
    DishDto GetById(int restaurantId, int dishId);
    IEnumerable<DishDto> GetAll(int restaurantId);
    void DeleteAll(int restaurantId);
    void DeleteById(int restaurantId, int dishId);
}

public class DishService : IDishService
{
    private readonly RestaurantDbContext _dbContext;
    private readonly IMapper _mapper;

    public DishService(RestaurantDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public int AddDish(int restaurantId, AddDishDto dto)
    {
        var restaurant = GetRestaurant(restaurantId);

        var dishEntity = _mapper.Map<Dish>(dto);
        dishEntity.RestaurantId = restaurantId;

        _dbContext.Dishes.Add(dishEntity);
        _dbContext.SaveChanges();

        return dishEntity.Id;
    }

    public DishDto GetById(int restaurantId, int dishId)
    {
        var restaurant = GetRestaurant(restaurantId);

        var dishEntity = restaurant.Dishes.FirstOrDefault(d => d.Id == dishId);
        if (dishEntity == null)
            throw new NotFoundException("Dish not found");

        var dishDto = _mapper.Map<DishDto>(dishEntity);
        return dishDto;
    }

    public IEnumerable<DishDto> GetAll(int restaurantId)
    {
        var restaurant = GetRestaurant(restaurantId);

        var dishes = restaurant.Dishes;

        if (!dishes.Any())
            throw new NotFoundException("Restaurant dishes not found");

        var dishDots = _mapper.Map<List<DishDto>>(dishes);
        return dishDots;
    }

    public void DeleteAll(int restaurantId)
    {
        var restaurant = GetRestaurant(restaurantId);

        _dbContext.Dishes.RemoveRange(restaurant.Dishes);
        _dbContext.SaveChanges();
    }

    public void DeleteById(int restaurantId, int dishId)
    {
        var restaurant = GetRestaurant(restaurantId);

        var dishToRemove = restaurant.Dishes.FirstOrDefault(d => d.Id == dishId);
        if (dishToRemove is null)
            throw new NotFoundException("Dish not found");

        _dbContext.Dishes.Remove(dishToRemove);
        _dbContext.SaveChanges();
    }

    private Restaurant GetRestaurant(int restaurantId)
    {
        var restaurant = _dbContext.Restaurants
            .Include(r => r.Dishes)
            .FirstOrDefault(r => r.Id == restaurantId);
        if (restaurant == null)
            throw new NotFoundException("Restaurant not found");

        return restaurant;
    }
}