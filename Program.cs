using Mike.CSharp;

var car = new Car("Toyota", "Camry", 2020);

car.Start();

var newCar = car with { Year = 2021 };

var truck = new Car("Ford", "F-150", 2019);

Console.WriteLine($"Car Make: {car.Make}, Model: {car.Model}, Year: {car.Year}");
Console.WriteLine($"New Car Make: {newCar.Make}, Model: {newCar.Model}, Year: {newCar.Year}");

truck.Honk();