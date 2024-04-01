namespace Hedin.ChangeTires.Api.Utilities

{
    public static class Utils
    {
        public static bool IsValidCarType(string carType)
        {
            return carType == "Sedan" || carType == "SUV";
        }

        public static bool IsWeekend(DateTime date)
        {
            return date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday;
        }

        public static int CalculateDaysBetweenDates(DateTime start, DateTime end)
        {
            return (end - start).Days;
        }

        public static void LogMessage(string message)
        {
            Console.WriteLine($"Log: {message}");
        }
    }
}