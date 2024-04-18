using parking_system.constant;
using parking_system.model;

namespace parking_system.service;

public interface IParkingService
{
    ParkingLot CreateParkingLot(int totalSlots);
    void ParkVehicle(string policeNumber, VehicleType type, string color);
    void UnparkVehicle(string policeNumber);

    List<ParkingSlot> GetParkingSlots();
    int GetAvailableSlots();
    int GetOccupiedSlots();

    List<Vehicle> GetVehiclesByOddPoliceNumber(bool isOdd);
    List<Vehicle> GetVehiclesByType(VehicleType type);
    List<Vehicle> GetVehiclesByColor(string color);
    Dictionary<string, List<int>> GetSlotNumbersForVehiclesByColor();
    int GetSlotNumberForVehicleByPoliceNumber(string policeNumber);
}