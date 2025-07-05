namespace APTXHub.Extentions
{
    public static class DateTimeExtensions
    {
        public static string ToTimeAgo(this DateTime dateTime)
        {
            TimeSpan timeSpan = DateTime.UtcNow - dateTime;

            return timeSpan switch
            {
                var t when t.TotalDays >= 365 => $"{(int)(t.TotalDays / 365)}y",
                var t when t.TotalDays >= 30 => $"{(int)(t.TotalDays / 30)}mo ", 
                var t when t.TotalDays >= 1 => $"{(int)t.TotalDays}d",
                var t when t.TotalHours >= 1 => $"{(int)t.TotalHours}h",
                var t when t.TotalMinutes >= 1 => $"{(int)t.TotalMinutes}m",
                _ => "Just now"
            };
        }
    }
}
