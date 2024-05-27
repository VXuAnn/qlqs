
using Microsoft.AspNetCore.Mvc;
using ThongKeDataChart.Data;
namespace MvcLogin.Controllers
{
    public class ThongKeRecordController : Controller
    {

        private DbContextThongKe _context;
        public ThongKeRecordController(DbContextThongKe context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult UpdateAdditionalChartData(DateTime? startDate, DateTime? endDate, string idDv)
        {
            if (startDate == null || endDate == null || idDv == null)
            {
                return BadRequest("Thiếu thông tin ngày hoặc đơn vị.");
            }

         
            List<DateTime> dateRange = Enumerable.Range(0, 1 + endDate.Value.Subtract(startDate.Value).Days)
                                                 .Select(offset => startDate.Value.AddDays(offset))
                                                 .ToList();

           
            int totalDaoNgu = 0;
            int totalOm = 0;
            int totalDiHoc = 0;
            int totalLyDoKhac = 0;

            
            foreach (var date in dateRange)
            {
                var dataForDay = _context.BaoCaoQuanSo
                               .Where(q => q.ngay.Date == date.Date && q.id_dv == idDv)
                               .Select(q => new
                               {
                                   tongDaoNgu = q.dao_ngu,
                                   tongOm = q.di_vien + q.benh_xa,
                                   tongDiHoc = q.di_hoc + q.di_thuc_te + q.di_thuc_tap,
                                   tongLyDoKhac = q.di_tt + q.di_ctac + q.thai_san + q.ly_do_khac
                               })
                               .FirstOrDefault();

               
                if (dataForDay != null)
                {
                    totalDaoNgu += dataForDay.tongDaoNgu;
                    totalOm += dataForDay.tongOm;
                    totalDiHoc += dataForDay.tongDiHoc;
                    totalLyDoKhac += dataForDay.tongLyDoKhac;
                }
            }

           
            int grandTotal = totalDaoNgu + totalOm + totalDiHoc + totalLyDoKhac;

            if (grandTotal == 0)
            {
                return Json(new { labels = new string[] { }, data = new double[] { } });
            }

           
            var piechartData = new
            {
                labels = new string[] { "Đảo ngũ ", "Ốm", "Đi học", "Lý do khác" },
                data = new double[] {
            (totalDaoNgu / (double)grandTotal) * 100,
            (totalOm / (double)grandTotal) * 100,
            (totalDiHoc / (double)grandTotal) * 100,
            (totalLyDoKhac / (double)grandTotal) * 100
        }
            };
            return Json(piechartData);
        }
        [HttpGet]
        public IActionResult UpdateAdditionalChartData2(DateTime? startDate, DateTime? endDate)
        {
            if (startDate == null || endDate == null)
            {
                return BadRequest("Thiếu thông tin ngày hoặc đơn vị.");
            }

          
            List<DateTime> dateRange = Enumerable.Range(0, 1 + endDate.Value.Subtract(startDate.Value).Days)
                                                 .Select(offset => startDate.Value.AddDays(offset))
                                                 .ToList();

          
            int totalDaoNgu = 0;
            int totalOm = 0;
            int totalDiHoc = 0;
            int totalLyDoKhac = 0;

            foreach (var date in dateRange)
            {
                var dataForDay = _context.BaoCaoQuanSo
                               .Where(q => q.ngay.Date == date.Date)
                               .Select(q => new
                               {
                                   tongDaoNgu = q.dao_ngu,
                                   tongOm = q.di_vien + q.benh_xa,
                                   tongDiHoc = q.di_hoc + q.di_thuc_te + q.di_thuc_tap,
                                   tongLyDoKhac = q.di_tt + q.di_ctac + q.thai_san + q.ly_do_khac
                               })
                               .FirstOrDefault();

               
                if (dataForDay != null)
                {
                    totalDaoNgu += dataForDay.tongDaoNgu;
                    totalOm += dataForDay.tongOm;
                    totalDiHoc += dataForDay.tongDiHoc;
                    totalLyDoKhac += dataForDay.tongLyDoKhac;
                }
            }

       
            int grandTotal = totalDaoNgu + totalOm + totalDiHoc + totalLyDoKhac;

            if (grandTotal == 0)
            {
                return Json(new { labels = new string[] { }, data = new double[] { } });
            }

         
            var piechartData = new
            {
                labels = new string[] { "Đảo ngũ ", "Ốm", "Đi học", "Lý do khác" },
                data = new double[] {
            (totalDaoNgu / (double)grandTotal) * 100,
            (totalOm / (double)grandTotal) * 100,
            (totalDiHoc / (double)grandTotal) * 100,
            (totalLyDoKhac / (double)grandTotal) * 100
        }
            };
            return Json(piechartData);
        }


        [HttpGet]
        public IActionResult UpdateChart(DateTime? startDate, DateTime? endDate, string idDv)
        {
            if (startDate == null || endDate == null || idDv == null)
            {
                return BadRequest("Thiếu thông tin ngày hoặc đơn vị.");
            }

            List<DateTime> dateRange = Enumerable.Range(0, 1 + endDate.Value.Subtract(startDate.Value).Days)
                                                  .Select(offset => startDate.Value.AddDays(offset))
                                                  .ToList();

            List<int> dataForChart = new List<int>();
            List<int> dataForQsVang = new List<int>();

            foreach (var date in dateRange)
            {
                var dataForDay = _context.BaoCaoQuanSo
                                         .Where(q => q.ngay.Date == date.Date && q.id_dv == idDv)
                                         .Select(q => q.tong_qs)
                                         .Sum();

                if (dataForDay != null)
                {
                    dataForChart.Add(dataForDay);
                }
                else
                {
                    dataForChart.Add(0);
                }

         
                var dataQsVangForDay = _context.BaoCaoQuanSo
                                              .Where(q => q.ngay.Date == date.Date && q.id_dv == idDv)
                                              .Select(q => q.qs_vang)
                                              .Sum();

                if (dataQsVangForDay != null)
                {
                    dataForQsVang.Add(dataQsVangForDay);
                }
                else
                {
                    dataForQsVang.Add(0);
                }
            }

            var chartData = new
            {
                labels = dateRange.Select(date => date.ToString("dd/MM/yyyy")),
                dataTongQS = dataForChart, 
                dataQsVang = dataForQsVang 
            };

            return Json(chartData);
        }

        [HttpGet]
        public IActionResult UpdateChartForHocVien(DateTime? startDate, DateTime? endDate)
        {
            if (startDate == null || endDate == null)
            {
                return BadRequest("Thiếu thông tin ngày.");
            }

        
            List<DateTime> dateRange = Enumerable.Range(0, 1 + endDate.Value.Subtract(startDate.Value).Days)
                                                 .Select(offset => startDate.Value.AddDays(offset))
                                                 .ToList();

           
            List<int> dataForChart = new List<int>();
            List<int> dataForQsVang = new List<int>();

            foreach (var date in dateRange)
            {
                var totalTongQS = _context.BaoCaoQuanSo
                                          .Where(q => q.ngay.Date == date.Date)
                                          .Sum(q => (int?)q.tong_qs) ?? 0; 

                var totalQsVang = _context.BaoCaoQuanSo
                                          .Where(q => q.ngay.Date == date.Date)
                                          .Sum(q => (int?)q.qs_vang) ?? 0; 

                dataForChart.Add(totalTongQS);
                dataForQsVang.Add(totalQsVang);
            }

           
            var chartData = new
            {
                labels = dateRange.Select(date => date.ToString("dd/MM/yyyy")).ToArray(),
                dataTongQS = dataForChart.ToArray(),
                dataQsVang = dataForQsVang.ToArray()
            };

            return Json(chartData);
        }



        public IActionResult Index()
        {
            var quanso = _context.BaoCaoQuanSo.ToList();

            return View(quanso);
        }
    }
}
