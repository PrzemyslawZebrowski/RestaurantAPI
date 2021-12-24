using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;
using RestaurantAPI.Entities;

namespace RestaurantAPI
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
                if (!_dbContext.Restaurants.Any())
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
                    ContactEmail = "contact@kfc.com",
                    ContactNumber = "123444555",
                    HasDelivery = true,
                    Dishes = new List<Dish>()
                    {
                        new Dish()
                        {
                            Name = "GRANDER",
                            Description =
                                "A big Kaiser bun with a piece of fresh, hand-breaded chicken" +
                                " in the middle along with crunchy bacon, Cheddar cheese," +
                                " red onion, iceberg lettuce and two sauces: spicy BBQ and mild mayonnaise. ",
                            Price = 15.99M,

                        },

                        new Dish()
                        {
                            Name = "QURRITO",
                            Description =
                                "Crispy chicken Bites, cheddar cheese and smoky BBQ sauce" +
                                " grilled in fresh tortilla.",
                            Price = 12.99M,

                        },
                    }

                },

                new Restaurant()
                {
                    Name = "KFC",
                    Address = new Address()
                    {
                        City = "Gliwice",
                        PostalCode = "44-100",
                        Street = "Zwyciestwa 39"
                    },
                    Category = "Fast Food",
                    Description =
                        "McDonald's is an American fast food company, founded in 1940" +
                        " as a restaurant operated by Richard and Maurice McDonald," +
                        " in San Bernardino, California, United States.",
                    ContactEmail = "contact@mcdonalds.com",
                    ContactNumber = "999777333",
                    HasDelivery = true,
                    Dishes = new List<Dish>()
                    {
                        new Dish()
                        {
                            Name = "Big Mac",
                            Description =
                                "550 Cal. Mouthwatering perfection starts with two 100%" +
                                " pure beef patties and Big Mac® sauce sandwiched between" +
                                " a sesame seed bun. It’s topped off with pickles, crisp shredded lettuce," +
                                " finely chopped onion and American cheese for a 100%" +
                                " beef burger with a taste like no other.",
                            Price = 13.59M,

                        },

                        new Dish()
                        {
                            Name = "Chocolate Shake",
                            Description =
                                "520 Cal. Try the McDonald’s Chocolate Shake, the perfect sweet treat" +
                                " for any time of the day. Our chocolate shake is made with delicious " +
                                "soft serve, chocolate syrup and finished off with a creamy whipped topping.",
                            Price = 7.99M,

                        }

                    }
                }
            };
            return restaurants;
        }
    }
}
