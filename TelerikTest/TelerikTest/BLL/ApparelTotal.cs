using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TelerikTest.DAL;
using TelerikTest.Entity.Basic;
using TelerikTest.Enum;

namespace TelerikTest.BLL
{
    public class ApparelTotal
    {
        public ApparelTotal(IRowDao rowDao)
        {
            this.RowDao = rowDao;
            this.ThisYear = DateTime.Now.Year;
            this.LastYear = this.ThisYear - 1;
        }

        public IRowDao RowDao { get; set; }

        private int ThisYear { get; set; }

        private int LastYear { get; set; }

        public List<CartesianDataPoint> GetSalesBySubLocation(SubLocation subLocation)
        {
            var lastYearSales = this.RowDao.GetYearSalesAtAssignedSubLocation(subLocation, this.LastYear);
            var thisYearSales = this.RowDao.GetYearSalesAtAssignedSubLocation(subLocation, this.ThisYear);

            return new List<CartesianDataPoint>()
            {
                new CartesianDataPoint()
                {
                    Category = this.LastYear.ToString(),
                    Value = this.Sum(lastYearSales.Select(x => x.SalesAmount)),
                },
                new CartesianDataPoint()
                {
                    Category = this.ThisYear.ToString(),
                    Value = this.Sum(thisYearSales.Select(x => x.SalesAmount)),
                },
            };
        }

        public List<CartesianDataPoint> GetSalesAllLocation()
        {
            var lastYearSales = this.RowDao.GetYearSales(this.LastYear);
            var thisYearSales = this.RowDao.GetYearSales(this.ThisYear);

            return new List<CartesianDataPoint>()
            {
                new CartesianDataPoint()
                {
                    Category = this.LastYear.ToString(),
                    Value = this.Sum(lastYearSales.Select(x => x.SalesAmount)),
                },
                new CartesianDataPoint()
                {
                    Category = this.ThisYear.ToString(),
                    Value = this.Sum(thisYearSales.Select(x => x.SalesAmount)),
                },
            };
        }

        private double Sum(IEnumerable<double> numbers)
        {
            double sum = 0;

            foreach (var number in numbers)
            {
                sum += number;
            }

            return sum;
        }
    }
}
