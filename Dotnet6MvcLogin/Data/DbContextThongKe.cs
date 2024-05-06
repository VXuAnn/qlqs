using Microsoft.EntityFrameworkCore;
using ThongKe.Models;

namespace ThongKeDataChart.Data
{
    public class DbContextThongKe : DbContext
    {
        public DbContextThongKe(DbContextOptions<DbContextThongKe> options) : base(options)
        {

        }
        public DbSet<ThongkeEntity> BaoCaoQuanSo { get; set; }
    }
}
