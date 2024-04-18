using parking_system.constant;
using parking_system.model;

namespace parking_system.service;

public interface IParkingService
{
    ParkingLot CreateParkingLot(int totalSlots);
    void ParkVehicle(string policeNumber, string color, VehicleType type);
    void UnparkVehicle(string policeNumber);

    List<ParkingSlot> GetParkingSlots();

    List<Vehicle> GetVehiclesByOddPoliceNumber(bool isOdd);
    List<Vehicle> GetVehiclesByType(VehicleType type);
    List<Vehicle> GetVehiclesByColor(string color);
    List<int> GetSlotNumbersForVehiclesByColor(string color);
    int GetSlotNumberForVehicleByPoliceNumber(string policeNumber);
}