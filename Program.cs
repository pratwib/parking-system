﻿using parking_system.constant;
using parking_system.model;
using parking_system.repository;
using parking_system.service;

namespace parking_system;

public class Program
{
    private static readonly IParkingService ParkingService;

    static Program()
    {
        var parkingRepository = new ParkingRepository();
        ParkingService = new ParkingService(parkingRepository);
    }

    public static void Main(string[] args)
    {
        while (true)
        {
            Console.Write("$ ");
            string command = Console.ReadLine()!.Trim();

            try
            {
                ProcessCommand(command);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            if (command.ToLower() == "exit")
            {
                break;
            }
        }
    }

    private static void ProcessCommand(string command)
    {
        string[] parts = command.Split(' ');
        switch (parts[0].ToLower())
        {
            case "create_parking_lot":
                if (parts.Length != 2) throw new ArgumentException("Invalid command format.");
                ParkingService.CreateParkingLot(int.Parse(parts[1]));
                Console.WriteLine($"Created a parking lot with {parts[1]} slots");
                break;
            case "park":
                if (parts.Length != 4) throw new ArgumentException("Invalid command format.");
                ParkingService.ParkVehicle(parts[1], parts[2],
                    (VehicleType)Enum.Parse(typeof(VehicleType), parts[3], true));
                break;
            case "leave":
                if (parts.Length != 2) throw new ArgumentException("Invalid command format.");
                ParkingService.UnparkVehicle(GetPoliceNumberBySlotNumber(int.Parse(parts[1])));
                break;
            case "status":
                PrintParkingStatus();
                break;
            case "type_of_vehicles":
                if (parts.Length != 2) throw new ArgumentException("Invalid command format.");
                VehicleType type = (VehicleType)Enum.Parse(typeof(VehicleType), parts[1], true);
                int count = ParkingService.GetVehiclesByType(type).Count;
                Console.WriteLine($"Number of vehicles of type {type}: {count}");
                break;
            case "registration_numbers_for_vehicles_with_odd_plate":
                List<Vehicle> oddVehicles = ParkingService.GetVehiclesByOddPoliceNumber(true);
                string oddPlates = string.Join(", ", oddVehicles.Select(v => v.PoliceNumber));
                Console.WriteLine($"Registration numbers for vehicles with odd plates: {oddPlates}");
                break;
            case "registration_numbers_for_vehicles_with_even_plate":
                List<Vehicle> evenVehicles = ParkingService.GetVehiclesByOddPoliceNumber(false);
                string evenPlates = string.Join(", ", evenVehicles.Select(v => v.PoliceNumber));
                Console.WriteLine($"Registration numbers for vehicles with even plates: {evenPlates}");
                break;
            case "registration_numbers_for_vehicles_with_color":
                List<Vehicle> vehicles = ParkingService.GetVehiclesByColor(parts[1]);
                string plates = string.Join(", ", vehicles.Select(v => v.PoliceNumber));
                Console.WriteLine($"Registration numbers for vehicles with color {parts[1]}: {plates}");
                break;
            case "slot_numbers_for_vehicles_with_color":
                List<int> slotIdByColor = ParkingService.GetSlotNumbersForVehiclesByColor(parts[1]);
                string slotNumbers = string.Join(", ", slotIdByColor);
                Console.WriteLine($"Slot numbers for vehicles with color {parts[1]}: {slotNumbers}");
                break;
            case "slot_number_for_registration_number":
                int slotNumber = ParkingService.GetSlotNumberForVehicleByPoliceNumber(parts[1]);
                Console.WriteLine(slotNumber);
                break;
            case "exit":
                break;
            default:
                Console.WriteLine("Invalid command.");
                break;
        }
    }

    private static string GetPoliceNumberBySlotNumber(int slotNumber)
    {
        var slot = ParkingService.GetParkingSlots().FirstOrDefault(x => x.Id == slotNumber);
        if (slot == null) throw new Exception($"Slot number {slotNumber} not found.");
        return slot.Vehicle!.PoliceNumber;
    }

    private static void PrintParkingStatus()
    {
        var parkingSlots = ParkingService.GetParkingSlots();
        Console.WriteLine("Slot\t\tNo.\t\tType\t\tColour");
        Console.WriteLine("-------\t\t----\t\t----\t\t-------");

        foreach (var slot in parkingSlots)
        {
            Console.WriteLine(slot.Status == ParkingSlotStatus.Available
                ? $"{slot.Id}\t\t<< Slot Available >>"
                : $"{slot.Id}\t\t{slot.Vehicle!.PoliceNumber}\t{slot.Vehicle!.Type}\t\t{slot.Vehicle!.Color}");
        }
    }
}