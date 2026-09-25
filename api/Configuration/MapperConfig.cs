using api.DTOs.ApiUser;
using api.DTOs.City;
using api.DTOs.Drink;
using api.DTOs.Factor;
using api.DTOs.Food;
using api.DTOs.Hotel;
using api.DTOs.Order;
using api.DTOs.OrderItem;
using api.DTOs.Reservation;
using api.DTOs.Room;
using api.DTOs.Service;
using api.DTOsa.Reservation;
using api.Models;
using AutoMapper;

namespace api.Configuration
{
    public class MapperConfig : Profile
    {
        public MapperConfig()
        {
            CreateMap<City, CityDto>().ReverseMap();

            CreateMap<Hotel, HotelDto>().ReverseMap();
            CreateMap<Hotel, HotelDetailDto>().ReverseMap();
            CreateMap<Hotel, HotelBaseDto>().ReverseMap();
            CreateMap<Hotel, HotelForCityDto>().ReverseMap();
            CreateMap<Hotel, CreateHotelRequestDto>().ReverseMap();
            CreateMap<Hotel, UpdateHotelRequestDto>().ReverseMap();

            CreateMap<ApiUser, ApiUserDto>().ReverseMap();
            CreateMap<ApiUser, AuthResponseDto>().ReverseMap();
            CreateMap<ApiUser, LoginDto>().ReverseMap();

            CreateMap<Room, RoomDto>()
                .ForMember(
                    dest => dest.ReservedDates,
                    opt => opt.MapFrom(src => GetReservedDates(src))
                )
                .ReverseMap();

            CreateMap<Room, RoomBaseDto>()
                .ForMember(
                    dest => dest.ReservedDates,
                    opt => opt.MapFrom(src => GetReservedDates(src))
                )
                .ReverseMap();

            CreateMap<Room, RoomDetailDto>()
                .ForMember(
                    dest => dest.ReservedDates,
                    opt => opt.MapFrom(src => GetReservedDates(src))
                )
                .ReverseMap();

            CreateMap<Room, CreateRoomRequestDto>().ReverseMap();
            CreateMap<Room, UpdateRoomRequestDto>().ReverseMap();
            CreateMap<Room, RoomForHotelDto>().ReverseMap();

            CreateMap<RoomService, RoomServiceDto>().ReverseMap();

            CreateMap<Service, ServiceDto>().ReverseMap();
            CreateMap<Service, ServiceBaseDto>().ReverseMap();
            CreateMap<Service, ServiceDetailDto>().ReverseMap();
            CreateMap<Service, CreateServiceRequestDto>().ReverseMap();
            CreateMap<Service, UpdateServiceRequestDto>().ReverseMap();

            CreateMap<Reservation, ReservationDto>().ReverseMap();
            CreateMap<Reservation, ReservationBaseDto>().ReverseMap();
            CreateMap<Reservation, CreateReservationRequestDto>().ReverseMap();
            CreateMap<Reservation, ReservationForRoomDto>().ReverseMap();

            CreateMap<Drink, DrinkDto>().ReverseMap();
            CreateMap<Drink, DrinkBaseDto>().ReverseMap();
            CreateMap<Drink, CreateDrinkRequestDto>().ReverseMap();
            CreateMap<Drink, UpdateDrinkRequestDto>().ReverseMap();

            CreateMap<Food, FoodDto>().ReverseMap();
            CreateMap<Food, FoodBaseDto>().ReverseMap();
            CreateMap<Food, CreateFoodRequestDto>().ReverseMap();
            CreateMap<Food, UpdateFoodRequestDto>().ReverseMap();

            CreateMap<OrderItem, OrderItemDto>().ReverseMap();
            CreateMap<OrderItem, OrderItemBaseDto>().ReverseMap();
            CreateMap<OrderItem, CreateOrderItemRequestDto>().ReverseMap();
            CreateMap<OrderItem, UpdateOrderItemRequestDto>().ReverseMap();
            CreateMap<OrderItem, OrderItemForOrderDto>()
                .ForMember(dest => dest.FoodName,
                    opt => opt.MapFrom(src => src.Food != null ? src.Food.Name : null))
                .ForMember(dest => dest.DrinkName,
                    opt => opt.MapFrom(src => src.Drink != null ? src.Drink.Name : null))
                .ReverseMap();

            CreateMap<Order, OrderDto>().ReverseMap();
            CreateMap<Order, OrderBaseDto>().ReverseMap();

            CreateMap<Factor, FactorDto>().ReverseMap();
            CreateMap<Factor, FactorBaseDto>().ReverseMap();
            CreateMap<CreateFactorRequestDto, Factor>()
                .ForMember(
                    dest => dest.CreatedAt,
                    opt => opt.MapFrom(_ => DateTime.UtcNow)
                );
            CreateMap<Factor, UpdateFactorRequestDto>().ReverseMap();
        }

        private static List<DateTime> GetReservedDates(Room room)
        {
            return room.Reservations
                .SelectMany(r =>
                    Enumerable.Range(
                        0,
                        (r.CheckOutDate.Date - r.CheckinDate.Date).Days
                    )
                    .Select(i => r.CheckinDate.Date.AddDays(i))
                )
                .Distinct()
                .OrderBy(d => d)
                .ToList();
        }
    }
}
