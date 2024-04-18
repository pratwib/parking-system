using parking_system.constant;

namespace parking_system.model;

public class ParkingSlot
{
    public int Id { get; set; }
    public ParkingSlotStatus Status { get; set; }
    public Vehicle? Vehicle { get; set; }

    public ParkingSlot(int id, ParkingSlotStatus status)
    {
        Id = id;
        Status = status;
        Vehicle = null;
    }
}