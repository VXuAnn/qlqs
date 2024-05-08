using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MvcLogin.Models
{
    public class THDV
    {
        public string? Id { get; set; }
        [Required]
        [DisplayName("Tên trực ban")]
        public string? TenTB { get; set; }
        [DisplayName("Nội vụ vệ sinh")]
        public string? Nvvs { get; set; }
        [DisplayName("Canh gác")]
        public string? CanhGac { get; set; }
        [DisplayName("Ghi chú")]
        public string? GhiChu { get; set; }
        [DisplayName("Đơn vị")]
        public string? IdDv { get; set; }
        [DisplayName("Ngày")] 
        

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? Ngay { get; set; }
      
    }
}
