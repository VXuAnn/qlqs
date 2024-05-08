using System.ComponentModel.DataAnnotations;

namespace MvcLogin.Models
{
    public class Combinecs
    {
        public QuanSo QuanSoModel { get; set; }
        public THDV THDVModel { get; set; }
        public Users userS { get; set; }
        public string? Uname { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy HH:mm:ss}")]
        public DateTime? LastActionTime { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy HH:mm:ss}")]
        public string? LastAction { get; set; }
    }
}
