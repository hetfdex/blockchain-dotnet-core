using System;

namespace blockchain_dotnet_core.API.Utils
{
    public static class TimestampUtils
    {
        public static long GetTimestamp() => (long)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
    }
}
