namespace MvcLogin.Models
{
    public class Users
    {
        public string? FName { get; set; }
        public string? LName { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }

        public string? IdDv { get; set; }
        public bool IsDataEntryAllowed { get; set; }
    }
}
