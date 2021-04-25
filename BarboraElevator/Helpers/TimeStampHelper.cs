using System;

namespace BarboraElevator.Helpers
{
    public class TimeStampHelper
    {
        public static long GetCurrentTimeStamp()
        {
            return new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();
        }
    }
}
