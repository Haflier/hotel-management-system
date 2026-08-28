using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Configuration;

public static class SeedData
{
    public static void Seed(ModelBuilder builder)
    {
        // Cities
        builder.Entity<City>().HasData(
            new City
            {
                Id = 1,
                Name = "Tehran"
            },
            new City
            {
                Id = 2,
                Name = "Mashhad"
            },
            new City
            {
                Id = 3,
                Name = "Isfahan"
            }
        );

        // Hotels
        builder.Entity<Hotel>().HasData(
            new Hotel
            {
                Id = 1,
                Name = "Grand Tehran Hotel",
                Description = "A luxury hotel in central Tehran.",
                Phone = "02112345678",
                Address = "Valiasr Street, Tehran",
                CityId = 1
            },
            new Hotel
            {
                Id = 2,
                Name = "Mashhad Palace Hotel",
                Description = "A comfortable hotel near the holy shrine.",
                Phone = "05112345678",
                Address = "Imam Reza Street, Mashhad",
                CityId = 2
            },
            new Hotel
            {
                Id = 3,
                Name = "Isfahan Heritage Hotel",
                Description = "A traditional hotel in historic Isfahan.",
                Phone = "03112345678",
                Address = "Naqsh-e Jahan Square, Isfahan",
                CityId = 3
            }
        );

        // Rooms
        builder.Entity<Room>().HasData(
            new Room
            {
                Id = 1,
                RoomNumber = "101",
                BedNumbers = 1,
                BasePricePerDay = 50,
                HotelId = 1
            },
            new Room
            {
                Id = 2,
                RoomNumber = "102",
                BedNumbers = 2,
                BasePricePerDay = 80,
                HotelId = 1
            },
            new Room
            {
                Id = 3,
                RoomNumber = "201",
                BedNumbers = 2,
                BasePricePerDay = 70,
                HotelId = 2
            },
            new Room
            {
                Id = 4,
                RoomNumber = "202",
                BedNumbers = 3,
                BasePricePerDay = 100,
                HotelId = 2
            },
            new Room
            {
                Id = 5,
                RoomNumber = "301",
                BedNumbers = 2,
                BasePricePerDay = 75,
                HotelId = 3
            }
        );

        // Food
        builder.Entity<Food>().HasData(
            new Food
            {
                Id = 1,
                Name = "Kebab",
                Description = "Traditional grilled kebab.",
                Price = 15,
                Meal = "Lunch"
            },
            new Food
            {
                Id = 2,
                Name = "Pizza",
                Description = "Cheese and tomato pizza.",
                Price = 12,
                Meal = "Dinner"
            },
            new Food
            {
                Id = 3,
                Name = "Breakfast",
                Description = "Traditional Iranian breakfast.",
                Price = 8,
                Meal = "Breakfast"
            }
        );

        // Drinks
        builder.Entity<Drink>().HasData(
            new Drink
            {
                Id = 1,
                Name = "Tea",
                Description = "Traditional Persian tea.",
                Price = 2,
                Meal = "Breakfast"
            },
            new Drink
            {
                Id = 2,
                Name = "Orange Juice",
                Description = "Fresh orange juice.",
                Price = 4,
                Meal = "Breakfast"
            },
            new Drink
            {
                Id = 3,
                Name = "Cola",
                Description = "Cold soft drink.",
                Price = 3,
                Meal = "Lunch"
            }
        );

        // Services
        builder.Entity<Service>().HasData(
            new Service
            {
                Id = 1,
                Name = "Room Cleaning",
                Description = "Daily room cleaning.",
                Price = 10
            },
            new Service
            {
                Id = 2,
                Name = "Breakfast Service",
                Description = "Breakfast delivered to the room.",
                Price = 15
            },
            new Service
            {
                Id = 3,
                Name = "Laundry",
                Description = "Laundry and ironing service.",
                Price = 12
            }
        );

        // Room-Service relationships
        builder.Entity<RoomService>().HasData(
            new RoomService
            {
                RoomId = 1,
                ServiceId = 1
            },
            new RoomService
            {
                RoomId = 1,
                ServiceId = 2
            },
            new RoomService
            {
                RoomId = 2,
                ServiceId = 1
            },
            new RoomService
            {
                RoomId = 2,
                ServiceId = 3
            },
            new RoomService
            {
                RoomId = 3,
                ServiceId = 1
            },
            new RoomService
            {
                RoomId = 4,
                ServiceId = 1
            },
            new RoomService
            {
                RoomId = 4,
                ServiceId = 2
            },
            new RoomService
            {
                RoomId = 5,
                ServiceId = 1
            }
        );
    }
}
