using parking_system.constant;
using parking_system.model;
using parking_system.repository;

namespace parking_system.service;

public class ParkingService : IParkingService
{
    private readonly IParkingRepository _parkingRepository;

    public ParkingService(IParkingRepository parkingRepository) => _parkingRepository = parkingRepository;

    public ParkingLot CreateParkingLot(int totalSlots)
    {
        if (totalSlots <= 0)
        {
            throw new ArgumentOutOfRangeException($"totalSlots", "<< total slots must be a positive number >>");
        }

        var parkingLot = new ParkingLot(totalSlots);
        for (int i = 1; i <= totalSlots; i++)
        {
            _parkingRepository.CreateParkingSlot(new ParkingSlot(i, ParkingSlotStatus.Available));
        }

        return parkingLot;
    }

    public void ParkVehicle(string policeNumber, string color, VehicleType type)
    {
        if (string.IsNullOrEmpty(policeNumber))
        {
            throw new ArgumentNullException($"policeNumber", "<< police number cannot be null or empty >>");
        }

        string[] parts = policeNumber.Split('-');
        if (parts.Length != 3)
        {
            throw new ArgumentException("<< invalid police number format >>");
        }

        if (color == null)
        {
            throw new ArgumentNullException($"color", "<< color cannot be null >>");
        }

        var availableSlot = _parkingRepository.GetAllParkingSlots()
            .FirstOrDefault(x => x.Status == ParkingSlotStatus.Available);
        if (availableSlot == null)
        {
            throw new Exception("<< sorry, parking lot is full >>");
        }

        var vehicle = new Vehicle(policeNumber, color, type);
        availableSlot.Status = ParkingSlotStatus.Occupied;
        availableSlot.Vehicle = vehicle;
        _parkingRepository.UpdateParkingSlot(availableSlot);

        Console.WriteLine($"Allocated slot number: {availableSlot.Id}");
    }

    public void UnparkVehicle(string policeNumber)
    {
        if (string.IsNullOrEmpty(policeNumber))
        {
            throw new ArgumentNullException($"policeNumber", "<< police number cannot be null or empty >>");
        }

        var occupiedSlot = _parkingRepository.GetAllParkingSlots().FirstOrDefault(x =>
            x.Status == ParkingSlotStatus.Occupied && x.Vehicle!.PoliceNumber == policeNumber);
        if (occupiedSlot == null)
        {
            throw new Exception("<< vehicle not found in the parking lot >>");
        }

        occupiedSlot.Status = ParkingSlotStatus.Available;
        occupiedSlot.Vehicle = null;
        _parkingRepository.UpdateParkingSlot(occupiedSlot);

        Console.WriteLine($"Slot number {occupiedSlot.Id} is free");
    }

    public List<ParkingSlot> GetParkingSlots()
    {
        return _parkingRepository.GetAllParkingSlots();
    }

    public List<Vehicle> GetVehiclesByOddPoliceNumber(bool isOdd)
    {
        var vehicles = _parkingRepository.GetAllParkingSlots()
            .Where(x => x.Vehicle != null)
            .Select(x => x.Vehicle)
            .ToList();

        vehicles = isOdd
            ? vehicles.Where(x => IsOddPoliceNumber(x!.PoliceNumber)).ToList()
            : vehicles.Where(x => !IsOddPoliceNumber(x!.PoliceNumber)).ToList();

        return vehicles!;
    }

    public List<Vehicle> GetVehiclesByType(VehicleType type)
    {
        var vehicles = _parkingRepository.GetAllParkingSlots()
            .Where(x => x.Vehicle != null && x.Vehicle.Type == type)
            .Select(x => x.Vehicle)
            .ToList();

        return vehicles!;
    }

    public List<Vehicle> GetVehiclesByColor(string color)
    {
        var vehicles = _parkingRepository.GetAllParkingSlots()
            .Where(x => x.Vehicle != null && x.Vehicle.Color == color)
            .Select(x => x.Vehicle)
            .ToList();

        return vehicles!;
    }

    public List<int> GetSlotNumbersForVehiclesByColor(string color)
    {
        var slotIdByColor = _parkingRepository.GetAllParkingSlots()
            .Where(x => x.Vehicle != null && x.Vehicle.Color == color)
            .Select(x => x.Id)
            .ToList();

        return slotIdByColor;
    }

    public int GetSlotNumberForVehicleByPoliceNumber(string policeNumber)
    {
        if (string.IsNullOrEmpty(policeNumber))
        {
            throw new ArgumentNullException($"policeNumber", "<< police number cannot be null or empty >>");
        }

        var occupiedSlot = _parkingRepository.GetAllParkingSlots().FirstOrDefault(x =>
            x.Status == ParkingSlotStatus.Occupied && x.Vehicle!.PoliceNumber == policeNumber);

        if (occupiedSlot == null)
        {
            throw new Exception($"<< slot not found >>");
        }

        return occupiedSlot.Id;
    }

    private bool IsOddPoliceNumber(string policeNumber)
    {
        string[] parts = policeNumber.Split('-');
        int middleDigit = int.Parse(parts[1]);
        return middleDigit % 2 != 0;
    }
}