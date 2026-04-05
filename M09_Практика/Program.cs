using System;

public class RoomBookingSystem
{
    public void BookRoom(int roomNumber) => Console.WriteLine($"Забронирован номер {roomNumber}.");
    public void CancelBooking(int roomNumber) => Console.WriteLine($"Бронь номера {roomNumber} отменена.");
}

public class RestaurantSystem
{
    public void BookTable(int tableNumber) => Console.WriteLine($"Забронирован столик {tableNumber} в ресторане.");
    public void OrderFood(string food) => Console.WriteLine($"Заказана еда в номер: {food}.");
}

public class EventManagementSystem
{
    public void BookConferenceRoom(string roomName) => Console.WriteLine($"Забронирован конференц-зал: {roomName}.");
    public void BookEquipment(string equipment) => Console.WriteLine($"Заказано оборудование: {equipment}.");
}

public class CleaningService
{
    public void ScheduleCleaning(int roomNumber, string time) => Console.WriteLine($"Уборка номера {roomNumber} запланирована на {time}.");
    public void PerformCleaning(int roomNumber) => Console.WriteLine($"Уборка номера {roomNumber} выполнена по запросу.");
}

public class HotelFacade
{
    private RoomBookingSystem _roomSystem;
    private RestaurantSystem _restaurantSystem;
    private EventManagementSystem _eventSystem;
    private CleaningService _cleaningService;

    public HotelFacade()
    {
        _roomSystem = new RoomBookingSystem();
        _restaurantSystem = new RestaurantSystem();
        _eventSystem = new EventManagementSystem();
        _cleaningService = new CleaningService();
    }

    public void BookRoomWithServices(int roomNumber, string foodInfo, string cleaningTime)
    {
        Console.WriteLine($"\n--- Бронирование номера {roomNumber} с услугами ---");
        _roomSystem.BookRoom(roomNumber);
        _restaurantSystem.OrderFood(foodInfo);
        _cleaningService.ScheduleCleaning(roomNumber, cleaningTime);
    }

    public void OrganizeEvent(string eventRoom, string equipment, int[] participantRooms)
    {
        Console.WriteLine($"\n--- Организация мероприятия в {eventRoom} ---");
        _eventSystem.BookConferenceRoom(eventRoom);
        _eventSystem.BookEquipment(equipment);
        foreach (var room in participantRooms)
        {
            _roomSystem.BookRoom(room);
        }
    }

    public void BookTableWithTaxi(int tableNumber)
    {
        Console.WriteLine($"\n--- Ужин в ресторане ---");
        _restaurantSystem.BookTable(tableNumber);
        Console.WriteLine("Такси заказано ко входу в отель.");
    }

    public void CancelRoomBooking(int roomNumber)
    {
        Console.WriteLine($"\n--- Отмена бронирования ---");
        _roomSystem.CancelBooking(roomNumber);
    }

    public void RequestImmediateCleaning(int roomNumber)
    {
        Console.WriteLine($"\n--- Срочная уборка ---");
        _cleaningService.PerformCleaning(roomNumber);
    }
}

public class HotelClient
{
    public static void Main()
    {
        var hotel = new HotelFacade();
        hotel.BookRoomWithServices(101, "Завтрак", "10:00");
        hotel.OrganizeEvent("Зал А", "Проектор и микрофоны", new int[] { 201, 202, 203 });
        hotel.BookTableWithTaxi(5);
        hotel.RequestImmediateCleaning(101);
        hotel.CancelRoomBooking(203);
    }
}