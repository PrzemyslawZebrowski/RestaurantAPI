using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RestaurantAPI.Entities;

namespace RestaurantAPI.Controllers
{
    [Route("api/restaurant")]

    public class RestaurantController : ControllerBase
    {
        private RestaurantDbContext dbContext { get; set; }

        public RestaurantController(RestaurantDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Restaurant>> GetAll()
        {
            var restaurants = dbContext.Restaurants.ToList();
            if (!restaurants.Any())
                return NotFound();
            return Ok(restaurants);
        }
        [HttpGet("{id}")]
        public ActionResult<Restaurant> GetById([FromRoute] int id)
        {
            var restaurant = dbContext
                .Restaurants
                .FirstOrDefault(r => r.Id == id);
            if (restaurant == default)
                return NotFound();
            return Ok(restaurant);
        }
    }
}
