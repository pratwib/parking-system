using parking_system.constant;

namespace parking_system.model;

public class Vehicle
{
    public string PoliceNumber { get; set; }
    public VehicleType Type { get; set; }
    public string Color { get; set; }
    public DateTime EntryTime { get; set; }

    public Vehicle(string policeNumber, VehicleType type, string color)
    {
        PoliceNumber = policeNumber;
        Type = type;
        Color = color;
        EntryTime = DateTime.Now;
    }
}