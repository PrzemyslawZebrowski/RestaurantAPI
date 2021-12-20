using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kurs.Entities;
using Microsoft.EntityFrameworkCore;

namespace Kurs
{
    public class RestaurantSeeder
    {
        private RestaurantDbContext _dbContext;
        public RestaurantSeeder(RestaurantDbContext dbContext) 
        {
            _dbContext = dbContext;
        }
        public void Seed()
        {
            if (_dbContext.Database.CanConnect())
            {
                if (_dbContext.Restaurants.Any())
                {
                    var restaurants = GetRestaurants();
                    _dbContext.Restaurants.AddRange(restaurants);
                    _dbContext.SaveChanges();
                }
            }

        }

        private IEnumerable<Restaurant> GetRestaurants()
        {
            var restaurants = new List<Restaurant>()
            {
                new Restaurant()
                {
                    Name = "KFC",
                    Address = new Address()
                    {
                        City = "Katowice",
                        PostalCode = "40-225",
                        Street = "3 Maja 30"
                    },
                    Category = "Fast Food",
                    Description = 
                        "KFC is  is an American fast food restaurant chain headquartered" + 
                        " in Louisville, Kentucky that specializes in fried chicken. It is the world's" + 
                        " second-largest restaurant chain after McDonald's, with 22,621" + 
                        " locations globally in 150 countries as of December 2019.",
                    HasDelivery = true,
                    Dishes = new List<Dish>()
                    {
                        new Dish()
                        {
                            Name =  
                        }
                    }

                }
            };
            return restaurants;
        }
    }
}
