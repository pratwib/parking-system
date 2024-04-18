using parking_system.model;

namespace parking_system.repository;

public interface IParkingRepository
{
    ParkingSlot CreateParkingSlot(ParkingSlot parkingSlot);
    ParkingSlot UpdateParkingSlot(ParkingSlot parkingSlot);
    List<ParkingSlot> GetAllParkingSlots();
}