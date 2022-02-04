using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantAPI.Models;
using RestaurantAPI.Services;
using System.Collections.Generic;
using System.Security.Claims;

namespace RestaurantAPI.Controllers
{
    [Route("api/restaurant")]
    [ApiController]
    [Authorize]
    public class RestaurantController : ControllerBase
    {
        private readonly IRestaurantService _restaurantService;

        public RestaurantController(IRestaurantService restaurantService)
        {
            _restaurantService = restaurantService;
        }

        [HttpPut("{id}")]
        public ActionResult UpdateById([FromRoute] int id, [FromBody] UpdateRestaurantDto dto)
        {
            _restaurantService.UpdateById(id, dto);

            return Ok();
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteById([FromRoute] int id)
        {
            _restaurantService.DeleteById(id);

            return NoContent();
        }

        [HttpPost]
        [Authorize(Roles = "Manager,Admin")]
        public ActionResult AddRestaurant([FromBody] AddRestaurantDto dto)
        {
            var userId = int.Parse(User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value);

            int id = _restaurantService.Add(dto);

            return Created($"/api/restaurant/{id}", null);
        }

        [HttpGet]
        [Authorize(Policy = "MoreThan2CreatedRestaurants")]
        public ActionResult<IEnumerable<RestaurantDto>> GetAll()
        {
            var restaurantsDtos = _restaurantService.GetAll();

            return Ok(restaurantsDtos);
        }

        [HttpGet("{id}")]
        public ActionResult<RestaurantDto> GetById([FromRoute] int id)
        {
            var restaurantDto = _restaurantService.GetById(id);

            return Ok(restaurantDto);
        }
    }
}
