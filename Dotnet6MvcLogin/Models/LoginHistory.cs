using System.ComponentModel.DataAnnotations;

namespace MvcLogin.Models
{
    public class LoginHistory
    {
        public string Id { get; set; }
        public string FirstName {  get; set; }
        public string LastName { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy HH:mm:ss}")]

        public DateTime? LoginTime { get; set; }
        public string? UserName { get; set; }
    }
}
