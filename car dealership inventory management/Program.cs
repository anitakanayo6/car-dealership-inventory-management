using System;
using System.Collections.Generic;

class Program
{
    static void Main()
    {
        Dealership dealership = new Dealership();

        dealership.AddCar("Toyota", "Truck", 2022, "1ABCD19EFGHI109186", 3690);
        dealership.AddCar("Honda", "Accord", 1999, "2ABCD19EFGHI109187", 24600);
        dealership.AddCar("Ford", "Mustang", 2022, "3ABCD19EFGHI109188", 102030);
        dealership.AddCar("Porsche", "951", 2024, "4ABCD19EFGHI109189", 123456);

        Console.WriteLine("\nCar Inventory:");
        dealership.DisplayCars();


        string searchVin = "1ABCD19EFGHI109186";
        Car foundCar = dealership.SearchCarByVin(searchVin);
        if (foundCar != null)
        {
            Console.WriteLine($"\nFound Car: {foundCar.Make} {foundCar.Model} ({foundCar.Year})");
        }
        else
        {
            Console.WriteLine($"Car with VIN {searchVin} not found.");
        }


        while (true)
        {
            Console.WriteLine("\nEnter car details (or type 'exit' to quit):");

            Console.WriteLine("Make: ");
            string make = Console.ReadLine();
            if (make.ToLower() == "exit") break;
            if (make.ToLower() == "view")
            {
                dealership.DisplayCars();
                continue;

                Console.WriteLine("Model: ");
                string model = Console.ReadLine();
                if (model.ToLower() == "exit") break;

                Console.WriteLine("Year: ");
                if (!int.TryParse(Console.ReadLine(), out int year) || year < 1886 || year > DateTime.Now.Year)
                {
                    Console.WriteLine("Invalid year. Please try again.");
                    continue;
                }

                Console.WriteLine("VIN: ");
                string vin = Console.ReadLine();
                if (vin.ToLower() == "exit") break;

                Console.WriteLine("Price: ");
                if (!decimal.TryParse(Console.ReadLine(), out decimal price) || price < 0)
                {
                    Console.WriteLine("Invalid price. Please try again.");
                    continue;
                }

                try
                {
                    dealership.AddCar(make, model, year, vin, price);
                    Console.WriteLine("Car added successfully!\n");
                }
                catch (ArgumentNullException ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
                catch (FormatException ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An unexpected error occurred: {ex.Message}");
                }

                Console.WriteLine("\nUpdated Car Inventory:");
                dealership.DisplayCars();
            }
        }
    }

    class Car
    {
        public string Make { get; }
        public string Model { get; }
        public int Year { get; }
        public string VIN { get; }
        public decimal Price { get; }

        public Car(string make, string model, int year, string vin, decimal price)
        {
            Make = make;
            Model = model;
            Year = year;
            VIN = vin;
            Price = price;
        }

        ~Car()
        {

        }
    }

    class Dealership
    {
        private List<Car> cars = new List<Car>();
        public static int TotalCars { get; private set; } = 0;

        public void AddCar(string make, string model, int year, string vin, decimal price)
        {
            if (string.IsNullOrEmpty(vin))
                throw new ArgumentNullException("VIN cannot be null or empty.");

            if (year < 1886 || year > DateTime.Now.Year)
                throw new FormatException("Year is invalid.");

            if (cars.Exists(c => c.VIN == vin))
                throw new Exception("A car with this VIN already exists.");

            Car newCar = new Car(make, model, year, vin, price);
            cars.Add(newCar);
            TotalCars++;
        }

        public void AddCar(string make, string model, int year, string vin, decimal price, string optionalFeature)
        {
            AddCar(make, model, year, vin, price);
            Console.WriteLine($"Added car with optional feature: {optionalFeature}");
        }

        public Car SearchCarByVin(string vin)
        {
            return SearchCarByVinRecursive(cars, vin);
        }

        private Car SearchCarByVinRecursive(List<Car> cars, string vin)
        {
            if (cars.Count == 0) return null;

            Car currentCar = cars[0];
            if (currentCar.VIN == vin) return currentCar;

            return SearchCarByVinRecursive(cars.GetRange(1, cars.Count - 1), vin);
        }

        public void DisplayCars()
        {
            foreach (var car in cars)
            {
                Console.WriteLine($"{car.Make} {car.Model} ({car.Year}) - VIN: {car.VIN}, Price: ${car.Price}");
                Console.WriteLine(GetCarAsciiArt(car.Make, car.Model));
            }
        }

        private string GetCarAsciiArt(string make, string model)
        {
            switch (make.ToLower())
            {
                case "toyota":
                    if (model.ToLower() == "truck")
                    {
                        return @"
       ______
  ___//  __ \___
 |  _  |  |  _  |
'-(_)------(_)--'
                    ";
                    }
                    break;
                case "honda":
                    return @"
       ______
  ___//  __ \___
 |  _  |  |  _  |
'-(_)------(_)--'
                ";
                case "ford":
                    return @"
       ______
  ___//  __ \___
 |  _  |  |  _  |
'-(_)------(_)--'
                ";
                case "porsche":
                    return @"
       ______
  ___//  __ \___
 |  _  |  |  _  |
'-(_)------(_)--'
                ";
                default:
                    return @"
       ______
  ___//  __ \___
 |  _  |  |  _  |
'-(_)------(_)--'
                ";
            }
            return string.Empty;
        }
    }

    public static class CarExtensions
    {
        static void DisplayDetails(this Car car)
        {
            Console.WriteLine($"Make: {car.Make}, Model: {car.Model}, Year: {car.Year}, VIN: {car.VIN}, Price: ${car.Price}");
        }
    }
}
