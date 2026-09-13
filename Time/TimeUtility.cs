namespace Odysape
{
    public static class TimeUtility
    {
        private const int Tick_Per_Hour = 25; // each game hour = 25 tick = 5s real time
        private const int Tick_Per_Day = 600; // each game day = 600 tick = 2m real time
        public static int ConvertHourToTick(int hours)
        {
            return hours * Tick_Per_Hour;
        }

        public static int ConvertDayToTick(int days)
        {
            return days * Tick_Per_Day;
        }

        public static float ConvertPerHourToPerTick(float value)
        {
            return value / Tick_Per_Hour;
        }

        public static float ConvertPerDayToPerTick(float value)
        {
            return value / Tick_Per_Day;
        }
    }
}
