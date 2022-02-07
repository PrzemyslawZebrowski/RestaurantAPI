using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using RestaurantAPI.Models;
using RestaurantAPI.Services;

namespace RestaurantAPI.Controllers;

[Route("api/restaurant/{restaurantId}/dish")]
[ApiController]
public class DishController : ControllerBase
{
    private readonly IDishService _dishService;

    public DishController(IDishService dishService)
    {
        _dishService = dishService;
    }

    [HttpDelete("{dishId}")]
    public ActionResult DeleteAll([FromRoute] int restaurantId, int dishId)
    {
        _dishService.DeleteById(restaurantId, dishId);

        return NoContent();
    }

    [HttpDelete]
    public ActionResult DeleteAll([FromRoute] int restaurantId)
    {
        _dishService.DeleteAll(restaurantId);

        return NoContent();
    }

    [HttpPost]
    public ActionResult AddDish([FromRoute] int restaurantId, [FromBody] AddDishDto dto)
    {
        var newDishId = _dishService.AddDish(restaurantId, dto);

        return Created($"api/restaurant/{restaurantId}/dish/{newDishId}", null);
    }

    [HttpGet("{dishId}")]
    public ActionResult<DishDto> GetById([FromRoute] int restaurantId, [FromRoute] int dishId)
    {
        var dishDto = _dishService.GetById(restaurantId, dishId);
        return Ok(dishDto);
    }

    [HttpGet]
    public ActionResult<IEnumerable<DishDto>> GetAll([FromRoute] int restaurantId)
    {
        var dishDtos = _dishService.GetAll(restaurantId);
        return Ok(dishDtos);
    }
}