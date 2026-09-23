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

        // ============================================================
        // GRAND TEHRAN HOTEL - FOOD
        // ============================================================

        builder.Entity<Food>().HasData(
            new Food
            {
                Id = 1,
                Name = "Chelo Kebab Koobideh",
                Description = "Two grilled koobideh kebabs served with saffron rice, grilled tomato and butter.",
                Price = 18,
                Meal = "Lunch",
                HotelId = 1
            },
            new Food
            {
                Id = 2,
                Name = "Chelo Kebab Barg",
                Description = "Grilled marinated beef fillet served with saffron rice and grilled tomato.",
                Price = 24,
                Meal = "Lunch",
                HotelId = 1
            },
            new Food
            {
                Id = 3,
                Name = "Ghormeh Sabzi",
                Description = "Persian herb stew with kidney beans, dried lime and tender beef, served with rice.",
                Price = 16,
                Meal = "Lunch",
                HotelId = 1
            },
            new Food
            {
                Id = 4,
                Name = "Fesenjan",
                Description = "Chicken cooked in a rich walnut and pomegranate sauce, served with saffron rice.",
                Price = 19,
                Meal = "Dinner",
                HotelId = 1
            },
            new Food
            {
                Id = 5,
                Name = "Zereshk Polo Ba Morgh",
                Description = "Saffron rice with barberries and roasted chicken.",
                Price = 17,
                Meal = "Dinner",
                HotelId = 1
            },
            new Food
            {
                Id = 6,
                Name = "Joojeh Kebab",
                Description = "Saffron-marinated grilled chicken pieces served with rice and grilled vegetables.",
                Price = 17,
                Meal = "Dinner",
                HotelId = 1
            },
            new Food
            {
                Id = 7,
                Name = "Mast-o-Khiar",
                Description = "Persian yogurt with cucumber, dried mint and herbs.",
                Price = 7,
                Meal = "Starter",
                HotelId = 1
            },
            new Food
            {
                Id = 8,
                Name = "Kashk-e Bademjan",
                Description = "Roasted eggplant dip topped with whey, fried onions, walnuts and mint.",
                Price = 9,
                Meal = "Starter",
                HotelId = 1
            },
            new Food
            {
                Id = 9,
                Name = "Persian Breakfast",
                Description = "Fresh bread, feta cheese, walnuts, cucumber, tomato, butter and honey.",
                Price = 11,
                Meal = "Breakfast",
                HotelId = 1
            },
            new Food
            {
                Id = 10,
                Name = "Saffron Ice Cream",
                Description = "Traditional Persian ice cream flavored with saffron, rose water and pistachios.",
                Price = 8,
                Meal = "Dessert",
                HotelId = 1
            }
        );

        // ============================================================
        // GRAND TEHRAN HOTEL - DRINKS
        // ============================================================

        builder.Entity<Drink>().HasData(
            new Drink
            {
                Id = 1,
                Name = "Persian Tea",
                Description = "Traditional black tea served hot.",
                Price = 3,
                Meal = "Breakfast",
                HotelId = 1
            },
            new Drink
            {
                Id = 2,
                Name = "Fresh Orange Juice",
                Description = "Freshly squeezed orange juice.",
                Price = 5,
                Meal = "Breakfast",
                HotelId = 1
            },
            new Drink
            {
                Id = 3,
                Name = "Pomegranate Juice",
                Description = "Fresh pomegranate juice.",
                Price = 6,
                Meal = "Lunch",
                HotelId = 1
            },
            new Drink
            {
                Id = 4,
                Name = "Doogh",
                Description = "Traditional chilled yogurt drink with dried mint.",
                Price = 4,
                Meal = "Lunch",
                HotelId = 1
            },
            new Drink
            {
                Id = 5,
                Name = "Saffron Sherbet",
                Description = "Chilled Persian saffron and rose water drink.",
                Price = 5,
                Meal = "Dinner",
                HotelId = 1
            },
            new Drink
            {
                Id = 6,
                Name = "Mint Lemonade",
                Description = "Fresh lemon juice, mint and sparkling water.",
                Price = 5,
                Meal = "Dinner",
                HotelId = 1
            },
            new Drink
            {
                Id = 7,
                Name = "Espresso",
                Description = "Freshly brewed single espresso.",
                Price = 4,
                Meal = "Breakfast",
                HotelId = 1
            },
            new Drink
            {
                Id = 8,
                Name = "Americano",
                Description = "Espresso with hot water.",
                Price = 5,
                Meal = "Breakfast",
                HotelId = 1
            }
        );

        // ============================================================
        // MASHHAD PALACE HOTEL - FOOD
        // ============================================================

        builder.Entity<Food>().HasData(
            new Food
            {
                Id = 11,
                Name = "Shishlik",
                Description = "Tender grilled lamb chops marinated with saffron and served with rice.",
                Price = 27,
                Meal = "Lunch",
                HotelId = 2
            },
            new Food
            {
                Id = 12,
                Name = "Chelo Kebab Mashhadi",
                Description = "Grilled kebab served with saffron rice, grilled tomato and fresh herbs.",
                Price = 19,
                Meal = "Lunch",
                HotelId = 2
            },
            new Food
            {
                Id = 13,
                Name = "Mashhadi Gheymeh",
                Description = "Split pea stew with beef, tomato and dried lime served with rice.",
                Price = 15,
                Meal = "Lunch",
                HotelId = 2
            },
            new Food
            {
                Id = 14,
                Name = "Reshteh Polo",
                Description = "Persian rice with noodles, raisins and dates served with tender chicken.",
                Price = 16,
                Meal = "Lunch",
                HotelId = 2
            },
            new Food
            {
                Id = 15,
                Name = "Baghali Polo Ba Mahicheh",
                Description = "Saffron rice with fava beans and dill served with slow-cooked lamb shank.",
                Price = 25,
                Meal = "Dinner",
                HotelId = 2
            },
            new Food
            {
                Id = 16,
                Name = "Kebab Soltani",
                Description = "Combination of koobideh and barg kebab served with saffron rice.",
                Price = 26,
                Meal = "Dinner",
                HotelId = 2
            },
            new Food
            {
                Id = 17,
                Name = "Ash Reshteh",
                Description = "Traditional thick soup with beans, herbs, noodles and whey.",
                Price = 10,
                Meal = "Starter",
                HotelId = 2
            },
            new Food
            {
                Id = 18,
                Name = "Mirza Ghasemi",
                Description = "Smoked eggplant with tomato, garlic and egg.",
                Price = 9,
                Meal = "Starter",
                HotelId = 2
            },
            new Food
            {
                Id = 19,
                Name = "Hotel Breakfast",
                Description = "Eggs, cheese, butter, honey, jam, fresh bread, cucumber and tomato.",
                Price = 12,
                Meal = "Breakfast",
                HotelId = 2
            },
            new Food
            {
                Id = 20,
                Name = "Sholeh Zard",
                Description = "Traditional Persian saffron rice pudding with cinnamon and pistachios.",
                Price = 7,
                Meal = "Dessert",
                HotelId = 2
            }
        );

        // ============================================================
        // MASHHAD PALACE HOTEL - DRINKS
        // ============================================================

        builder.Entity<Drink>().HasData(
            new Drink
            {
                Id = 11,
                Name = "Saffron Tea",
                Description = "Black tea infused with saffron.",
                Price = 4,
                Meal = "Breakfast",
                HotelId = 2
            },
            new Drink
            {
                Id = 12,
                Name = "Pomegranate Juice",
                Description = "Freshly prepared pomegranate juice.",
                Price = 6,
                Meal = "Lunch",
                HotelId = 2
            },
            new Drink
            {
                Id = 13,
                Name = "Doogh",
                Description = "Cold traditional yogurt drink with mint.",
                Price = 4,
                Meal = "Lunch",
                HotelId = 2
            },
            new Drink
            {
                Id = 14,
                Name = "Barberry Juice",
                Description = "Refreshing juice made from Iranian barberries.",
                Price = 5,
                Meal = "Lunch",
                HotelId = 2
            },
            new Drink
            {
                Id = 15,
                Name = "Rose Water Sherbet",
                Description = "Chilled rose water drink with a delicate floral flavor.",
                Price = 5,
                Meal = "Dinner",
                HotelId = 2
            },
            new Drink
            {
                Id = 16,
                Name = "Lemon Mint Cooler",
                Description = "Fresh lemon, mint and sparkling water served chilled.",
                Price = 5,
                Meal = "Dinner",
                HotelId = 2
            },
            new Drink
            {
                Id = 17,
                Name = "Turkish Coffee",
                Description = "Strong traditionally brewed coffee.",
                Price = 4,
                Meal = "Breakfast",
                HotelId = 2
            },
            new Drink
            {
                Id = 18,
                Name = "Cappuccino",
                Description = "Espresso with steamed milk and foam.",
                Price = 6,
                Meal = "Breakfast",
                HotelId = 2
            }
        );

        // ============================================================
        // ISFAHAN HERITAGE HOTEL - FOOD
        // ============================================================

        builder.Entity<Food>().HasData(
            new Food
            {
                Id = 21,
                Name = "Beryani",
                Description = "Traditional Isfahani minced lamb and herbs served with freshly baked bread.",
                Price = 18,
                Meal = "Lunch",
                HotelId = 3
            },
            new Food
            {
                Id = 22,
                Name = "Khoresht Mast",
                Description = "Sweet and creamy Isfahani yogurt dessert made with saffron and orange zest.",
                Price = 9,
                Meal = "Dessert",
                HotelId = 3
            },
            new Food
            {
                Id = 23,
                Name = "Khoresht Fesenjan",
                Description = "Chicken in walnut and pomegranate sauce served with saffron rice.",
                Price = 19,
                Meal = "Lunch",
                HotelId = 3
            },
            new Food
            {
                Id = 24,
                Name = "Khoresht Alu",
                Description = "Tender beef and yellow plum stew served with Persian saffron rice.",
                Price = 17,
                Meal = "Lunch",
                HotelId = 3
            },
            new Food
            {
                Id = 25,
                Name = "Kashk-e Bademjan",
                Description = "Smoked eggplant topped with whey, walnuts, mint and fried onions.",
                Price = 9,
                Meal = "Starter",
                HotelId = 3
            },
            new Food
            {
                Id = 26,
                Name = "Dolmeh Barg",
                Description = "Grape leaves stuffed with rice, herbs and seasoned meat.",
                Price = 12,
                Meal = "Starter",
                HotelId = 3
            },
            new Food
            {
                Id = 27,
                Name = "Adas Polo",
                Description = "Persian rice with lentils, raisins and dates served with roasted chicken.",
                Price = 16,
                Meal = "Dinner",
                HotelId = 3
            },
            new Food
            {
                Id = 28,
                Name = "Joojeh Kebab",
                Description = "Saffron-marinated chicken kebab served with rice and grilled vegetables.",
                Price = 17,
                Meal = "Dinner",
                HotelId = 3
            },
            new Food
            {
                Id = 29,
                Name = "Traditional Isfahani Breakfast",
                Description = "Fresh bread, local cheese, walnuts, honey, cream, cucumber and tomato.",
                Price = 11,
                Meal = "Breakfast",
                HotelId = 3
            },
            new Food
            {
                Id = 30,
                Name = "Gaz",
                Description = "Traditional Isfahani nougat made with pistachios and rose water.",
                Price = 8,
                Meal = "Dessert",
                HotelId = 3
            }
        );

        // ============================================================
        // ISFAHAN HERITAGE HOTEL - DRINKS
        // ============================================================

        builder.Entity<Drink>().HasData(
            new Drink
            {
                Id = 21,
                Name = "Persian Black Tea",
                Description = "Traditional Iranian black tea served hot.",
                Price = 3,
                Meal = "Breakfast",
                HotelId = 3
            },
            new Drink
            {
                Id = 22,
                Name = "Sour Cherry Juice",
                Description = "Refreshing traditional sour cherry juice.",
                Price = 5,
                Meal = "Lunch",
                HotelId = 3
            },
            new Drink
            {
                Id = 23,
                Name = "Doogh",
                Description = "Chilled yogurt drink with mint and a lightly salted flavor.",
                Price = 4,
                Meal = "Lunch",
                HotelId = 3
            },
            new Drink
            {
                Id = 24,
                Name = "Pomegranate Juice",
                Description = "Fresh pomegranate juice served chilled.",
                Price = 6,
                Meal = "Lunch",
                HotelId = 3
            },
            new Drink
            {
                Id = 25,
                Name = "Rose Water Sherbet",
                Description = "Traditional chilled rose water drink.",
                Price = 5,
                Meal = "Dinner",
                HotelId = 3
            },
            new Drink
            {
                Id = 26,
                Name = "Saffron Lemonade",
                Description = "Fresh lemon drink infused with saffron.",
                Price = 5,
                Meal = "Dinner",
                HotelId = 3
            },
            new Drink
            {
                Id = 27,
                Name = "Persian Cardamom Tea",
                Description = "Black tea brewed with cardamom and saffron.",
                Price = 4,
                Meal = "Breakfast",
                HotelId = 3
            },
            new Drink
            {
                Id = 28,
                Name = "Turkish Coffee",
                Description = "Strong traditionally brewed coffee served in a small cup.",
                Price = 4,
                Meal = "Breakfast",
                HotelId = 3
            }
        );

        // ============================================================
        // SERVICES
        // ============================================================

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
