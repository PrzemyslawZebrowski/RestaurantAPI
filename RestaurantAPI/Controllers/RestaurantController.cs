using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Entities;
using RestaurantAPI.Models;

namespace RestaurantAPI.Controllers
{
    [Route("api/restaurant")]

    public class RestaurantController : ControllerBase
    {
        private IMapper _mapper;
        private RestaurantDbContext _dbContext { get; set; }

        public RestaurantController(RestaurantDbContext dbContext, IMapper mapper)
        {
            _mapper = mapper;
            _dbContext = dbContext;
        }

        [HttpGet]
        public ActionResult<IEnumerable<RestaurantDto>> GetAll()
        {
            var restaurants = _dbContext.Restaurants
                .Include(r=> r.Address)
                .Include(r => r.Dishes)
                .ToList();

            if (!restaurants.Any())
                return NotFound();
            var restaurantsDtos = _mapper.Map<List<RestaurantDto>>(restaurants);
            return Ok(restaurantsDtos);
        }
        [HttpGet("{id}")]
        public ActionResult<RestaurantDto> GetById([FromRoute] int id)
        {
            var restaurant = _dbContext
                .Restaurants
                .Include(r => r.Address)
                .Include(r => r.Dishes)
                .FirstOrDefault(r => r.Id == id);
            if (restaurant == default)
                return NotFound();
            var restaurantDto = _mapper.Map<RestaurantDto>(restaurant);
            return Ok(restaurantDto);
        }
    }
}
