namespace MvcLogin.Models
{
    public class LoginHistory
    {
        public string Id { get; set; }
        public string FirstName {  get; set; }
        public string LastName { get; set; }
        public DateTime? LoginTime { get; set; }
        public string? UserName { get; set; }
    }
}
