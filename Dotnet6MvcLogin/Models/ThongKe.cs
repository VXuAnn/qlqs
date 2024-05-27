using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThongKe.Models
{

        public class ThongkeEntity
        {
            [Key]
            public string id_dv { get; set; }
            public DateTime ngay { get; set; }
            public int tong_qs { get; set; }
            public int qs_vang { get; set; }
            public int dao_ngu { get; set; }
            public int di_vien { get; set; }
            public int benh_xa { get; set; }
            public int di_hoc { get; set; }
            public int di_thuc_te { get; set; }
            public int di_thuc_tap { get; set; }
            public int di_tt { get; set; }
            public int di_ctac { get; set; }
            public int thai_san { get; set; }
            public int ly_do_khac { get; set; }
    }

}
