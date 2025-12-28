namespace Mike.CSharp;

public record Car(string Make, string Model, int Year);

public static class CarExtensions
{
    extension(Car car)
    {
        public static void Honk()
        {
            Console.WriteLine("Beep beep beep!");
        }

        public static void Start()
        {
            Console.WriteLine("The car has started!.");
        }
    }
}