using parking_system.model;

namespace parking_system.repository;

public class ParkingRepository : IParkingRepository
{
    private readonly List<ParkingSlot> _parkingSlots;

    public ParkingRepository() => _parkingSlots = new List<ParkingSlot>();

    public List<ParkingSlot> GetAllParkingSlots()
    {
        return _parkingSlots;
    }

    public ParkingSlot GetParkingSlotById(int id)
    {
        return _parkingSlots.FirstOrDefault(x => x.Id == id)!;
    }

    public ParkingSlot CreateParkingSlot(ParkingSlot parkingSlot)
    {
        _parkingSlots.Add(parkingSlot);
        return parkingSlot;
    }

    public ParkingSlot UpdateParkingSlot(ParkingSlot parkingSlot)
    {
        var existingParkingSlot = GetParkingSlotById(parkingSlot.Id);
        existingParkingSlot.Status = parkingSlot.Status;
        existingParkingSlot.Vehicle = parkingSlot.Vehicle;
        return existingParkingSlot;
    }
}