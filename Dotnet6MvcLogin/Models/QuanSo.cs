using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MvcLogin.Models
{
    public class QuanSo
    {
        public int IdBcqs { get; set; }
        [DisplayName("Đơn vị")]
        public string? IdDv { get; set; }
        [DisplayName("Ngày")]
        [DataType(DataType.Date)]
        public DateTime? Ngay { get; set; }
        [DisplayName("Tổng Quân số")]
        public int? TongQs { get; set; } 
        [DisplayName("Quân số vắng")]
        public int? QsVang { get; set; } 
        [DisplayName("Đào ngũ")]
        public int? DaoNgu { get; set; } 
        [DisplayName("Đi viện")]
        public int? DiVien { get; set; } 
        [DisplayName("Bệnh xá")]
        public int? BenhXa { get; set; } 
        [DisplayName("Đi học")]
        public int? DiHoc { get; set; } 
        [DisplayName("Đi thực tế")]
        public int? DiThucTe { get; set; } 
        [DisplayName("Đi thực tập")]
        public int? DiThucTap { get; set; } 
        [DisplayName("Đi tranh thủ")]
        public int? DiTt { get; set; } 
        [DisplayName("Đi công tác")]
        public int? DiCtac { get; set; } 
        [DisplayName("Thai sản")]
        public int? ThaiSan { get; set; } 
        [DisplayName("Lý do khác")]
        public int? LyDoKhac { get; set; } 
        [DisplayName("Chú thích")]
        public string? ChuThich { get; set; }
     

        //public virtual DonVi? IdDvNavigation { get; set; }
    }
}
