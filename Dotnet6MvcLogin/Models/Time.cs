namespace MvcLogin.Models
{
    public class Time
    {
        public TimeSpan startHour { get; set; } = TimeSpan.FromHours(0); // 00:00
        public TimeSpan endHour { get; set; } = TimeSpan.FromHours(24) ; // Tương đương với 24 giờ và 59 phút

        



    }
}
