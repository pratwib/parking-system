using parking_system.constant;

namespace parking_system.model;

public class ParkingLot
{
    public int TotalSlots { get; set; }
    public int AvailableSlots { get; set; }

    public ParkingLot(int totalSlots)
    {
        TotalSlots = totalSlots;
        AvailableSlots = totalSlots;
    }
}