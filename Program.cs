using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleParkingSystem
{
    class Program
    {
        static void Main(string[] args)
        {
            ParkingLot parkingLot = null;

            while (true)
            {
                Console.WriteLine("PARKING SYSTEM");
                Console.WriteLine("1. Create Parking Lot");
                Console.WriteLine("2. Park Vehicle");
                Console.WriteLine("3. Leave Parking Lot");
                Console.WriteLine("4. Check Status");
                Console.WriteLine("5. Report - Number of Vehicles by Type");
                Console.WriteLine("6. Report - Number of Vehicles by Plate Odd/Even");
                Console.WriteLine("7. Report - Number of Vehicles by Color");
                Console.WriteLine("8. Find Slot Number by Registration Number");
                Console.WriteLine("9. Exit");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        Console.WriteLine("Enter number of parking slots:");
                        int totalSlots;
                        if (int.TryParse(Console.ReadLine(), out totalSlots))
                        {
                            parkingLot = new ParkingLot(totalSlots);
                            Console.WriteLine($"Created a parking lot with {totalSlots} slots");
                        }
                        else
                        {
                            Console.WriteLine("Invalid input. Please enter a valid number.");
                        }
                        break;
                    case "2":
                        if (parkingLot != null)
                        {
                            Console.WriteLine("Enter vehicle details (e.g., Registration No Colour Type):");
                            string[] vehicleDetails = Console.ReadLine().Split(' ');
                            if (vehicleDetails.Length == 3)
                            {
                                string registrationNumber = vehicleDetails[0];
                                string color = vehicleDetails[1];
                                string type = vehicleDetails[2];

                                string result = parkingLot.ParkVehicle(registrationNumber, color, type);
                                Console.WriteLine(result);
                            }
                            else
                            {
                                Console.WriteLine("Invalid input. Please provide registration number, color, and type.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Parking lot has not been created yet.");
                        }
                        break;
                    case "3":
                        if (parkingLot != null)
                        {
                            Console.WriteLine("Enter slot number to leave:");
                            int slotNumber;
                            if (int.TryParse(Console.ReadLine(), out slotNumber))
                            {
                                string result = parkingLot.Leave(slotNumber);
                                Console.WriteLine(result);
                            }
                            else
                            {
                                Console.WriteLine("Invalid input. Please enter a valid slot number.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Parking lot has not been created yet.");
                        }
                        break;
                    case "4":
                        if (parkingLot != null)
                        {
                            string status = parkingLot.CheckStatus();
                            Console.WriteLine(status);
                        }
                        else
                        {
                            Console.WriteLine("Parking lot has not been created yet.");
                        }
                        break;
                    case "5":
                        if (parkingLot != null)
                        {
                            string type = Console.ReadLine();
                            int count = parkingLot.GetNumberOfVehiclesByType(type);
                            Console.WriteLine(count);
                        }
                        else
                        {
                            Console.WriteLine("Parking lot has not been created yet.");
                        }
                        break;
                    case "6":
                        if (parkingLot != null)
                        {
                            string result = parkingLot.GetVehiclesByPlateOddEven();
                            Console.WriteLine(result);
                        }
                        else
                        {
                            Console.WriteLine("Parking lot has not been created yet.");
                        }
                        break;
                    case "7":
                        if (parkingLot != null)
                        {
                            string color = Console.ReadLine();
                            string result = parkingLot.GetNumberOfVehiclesByColor(color);
                            Console.WriteLine(result);
                        }
                        else
                        {
                            Console.WriteLine("Parking lot has not been created yet.");
                        }
                        break;
                    case "8":
                        if (parkingLot != null)
                        {
                            string registrationNumber = Console.ReadLine();
                            string result = parkingLot.GetSlotNumberByRegistrationNumber(registrationNumber);
                            Console.WriteLine(result);
                        }
                        else
                        {
                            Console.WriteLine("Parking lot has not been created yet.");
                        }
                        break;
                    case "9":
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Please select a valid option.");
                        break;
                }
            }
        }
    }

    class ParkingLot
    {
        private int totalSlots;
        private Dictionary<int, Vehicle> parkingSlots;

        public ParkingLot(int totalSlots)
        {
            this.totalSlots = totalSlots;
            parkingSlots = new Dictionary<int, Vehicle>();
        }

        public string ParkVehicle(string registrationNumber, string color, string type)
        {
            if (parkingSlots.Count < totalSlots)
            {
                int slotNumber = GetNextAvailableSlot();
                parkingSlots[slotNumber] = new Vehicle(registrationNumber, color, type);
                return $"Allocated slot number: {slotNumber}";
            }
            else
            {
                return "Sorry, parking lot is full";
            }
        }

        public string Leave(int slotNumber)
        {
            if (parkingSlots.ContainsKey(slotNumber))
            {
                parkingSlots.Remove(slotNumber);
                return $"Slot number {slotNumber} is free";
            }
            else
            {
                return $"Slot number {slotNumber} is not occupied";
            }
        }

        public string CheckStatus()
        {
            if (parkingSlots.Count > 0)
            {
                Console.WriteLine("Slot No.\tRegistration No\tColour\tType");
                foreach (var kvp in parkingSlots)
                {
                    Console.WriteLine($"{kvp.Key}\t{kvp.Value.RegistrationNumber}\t{kvp.Value.Color}\t{kvp.Value.Type}");
                }
                return string.Empty;
            }
            else
            {
                return "Parking lot is empty";
            }
        }

        public int GetNumberOfVehiclesByType(string type)
        {
            return parkingSlots.Count(v => v.Value.Type.Equals(type, StringComparison.OrdinalIgnoreCase));
        }

        public string GetVehiclesByPlateOddEven()
        {
            string oddNumbers = string.Join(", ", parkingSlots.Where(v => int.Parse(v.Value.RegistrationNumber.Substring(v.Value.RegistrationNumber.Length - 1)) % 2 != 0)
                .Select(v => v.Value.RegistrationNumber));
            string evenNumbers = string.Join(", ", parkingSlots.Where(v => int.Parse(v.Value.RegistrationNumber.Substring(v.Value.RegistrationNumber.Length - 1)) % 2 == 0)
                .Select(v => v.Value.RegistrationNumber));

            return $"Odd Plate: {oddNumbers}\nEven Plate: {evenNumbers}";
        }

        public string GetNumberOfVehiclesByColor(string color)
        {
            int count = parkingSlots.Count(v => v.Value.Color.Equals(color, StringComparison.OrdinalIgnoreCase));
            return $"{count}";
        }

        public string GetSlotNumberByRegistrationNumber(string registrationNumber)
        {
            var vehicle = parkingSlots.FirstOrDefault(v => v.Value.RegistrationNumber.Equals(registrationNumber, StringComparison.OrdinalIgnoreCase));
            if (vehicle.Key != 0)
            {
                return $"{vehicle.Key}";
            }
            else
            {
                return "Not found";
            }
        }

        private int GetNextAvailableSlot()
        {
            for (int i = 1; i <= totalSlots; i++)
            {
                if (!parkingSlots.ContainsKey(i))
                {
                    return i;
                }
            }
            return -1;
        }
    }

    class Vehicle
    {
        public string RegistrationNumber { get; }
        public string Color { get; }
        public string Type { get; }

        public Vehicle(string registrationNumber, string color, string type)
        {
            RegistrationNumber = registrationNumber;
            Color = color;
            Type = type;
        }
    }
}
