using System.Collections.Generic;
using System.Linq;
using TelerikTest.DAL;
using TelerikTest.Entity.Basic;
using TelerikTest.Enum;

namespace TelerikTest.BLL
{
    public class ApparelLocation
    {
        public ApparelLocation(IRowDao rowDao)
        {
            this.RowDao = rowDao;
        }

        private IRowDao RowDao { get; set; }

        public List<PieDataPoint> GetSalesAmountByStore(SubLocation subLocation)
        {
            var subLocationSales = this.RowDao.GetSubLocationSales(subLocation);

            var stores = subLocationSales.Select(x => x.Store).Distinct().OrderBy(x => x);

            return stores.Select(x => this.CaculateSalesAmountByStore(x, subLocationSales)).ToList();
        }

        public List<CartesianDataPoint> GetSalesAmountByBrand(SubLocation subLocation)
        {
            var subLocationSales = this.RowDao.GetSubLocationSales(subLocation);

            var brands = subLocationSales.Select(x => x.Brand).Distinct().OrderBy(x => x);

            return brands.Select(x => this.CaculateSalesAmountByBrand(x, subLocationSales, subLocation)).ToList();
        }

        public List<CartesianDataPoint> GetStoreSalesAmountByBrand(string store, SubLocation subLocation)
        {
            var storeSalesAtAssignedSubLocation = this.RowDao.GetStoreSalesAtAssignedSubLocation(store, subLocation);

            var brands = storeSalesAtAssignedSubLocation.Select(x => x.Brand).Distinct().OrderBy(x => x);

            return brands.Select(x => this.CaculateSalesAmountByBrand(x, storeSalesAtAssignedSubLocation, subLocation)).ToList();
        }

        public List<CartesianDataPoint> GetSalesAmountByCategory(SubLocation subLocation, string brand)
        {
            var subLocationSales = this.RowDao.GetSalesWhere_SubLocation_Brand(subLocation, brand);

            var categories = subLocationSales.Select(x => x.Category).Distinct().OrderBy(x => x);

            return categories.Select(x => this.CaculateSalesAmountByCategory(x, subLocationSales, subLocation)).ToList();
        }

        public List<CartesianDataPoint> GetStoreSalesAmountByCategory(string store, SubLocation subLocation, string brand)
        {
            var storeSalesAtAssignedSubLocation = this.RowDao.GetStoreSalesWhere_SubLocation_Brand(store, subLocation, brand);

            var categories = storeSalesAtAssignedSubLocation.Select(x => x.Category).Distinct().OrderBy(x => x);

            return categories.Select(x => this.CaculateSalesAmountByCategory(x, storeSalesAtAssignedSubLocation, subLocation)).ToList();
        }

        public List<CartesianDataPoint> GetSalesAmountByProduct(SubLocation subLocation, string brand, string category)
        {
            var subLocationSales = this.RowDao.GetSalesWhere_SubLocation_Brand_Category(subLocation, brand, category);

            var products = subLocationSales.Select(x => x.Product).Distinct().OrderBy(x => x);

            return products.Select(x => this.CaculateSalesAmountByProduct(x, subLocationSales, subLocation)).ToList();
        }

        public List<CartesianDataPoint> GetStoreSalesAmountByProduct(string store, SubLocation subLocation, string brand, string category)
        {
            var storeSalesAtAssignedSubLocation = this.RowDao.GetStoreSalesWhere_SubLocation_Brand_Category(store, subLocation, brand, category);

            var products = storeSalesAtAssignedSubLocation.Select(x => x.Product).Distinct().OrderBy(x => x);

            return products.Select(x => this.CaculateSalesAmountByProduct(x, storeSalesAtAssignedSubLocation, subLocation)).ToList();
        }

        #region private

        private PieDataPoint CaculateSalesAmountByStore(string store, IEnumerable<RowInfo> subLocationSales)
        {
            var sales = subLocationSales.Where(x => x.Store == store).Select(x => x.SalesAmount);

            return new PieDataPoint()
            {
                Label = store,
                Value = this.Sum(sales),
            };
        }

        private CartesianDataPoint CaculateSalesAmountByBrand(string brand, IEnumerable<RowInfo> subLocationSales, SubLocation subLocation)
        {
            var sales = subLocationSales.Where(x => x.Brand == brand).Select(x => x.SalesAmount);

            return new CartesianDataPoint()
            {
                Category = brand,
                Value = this.Sum(sales),
                SubLocation = subLocation,
            };
        }

        private CartesianDataPoint CaculateSalesAmountByCategory(string category, IEnumerable<RowInfo> subLocationSales, SubLocation subLocation)
        {
            var sales = subLocationSales.Where(x => x.Category == category).Select(x => x.SalesAmount);

            return new CartesianDataPoint()
            {
                Category = category,
                Value = this.Sum(sales),
                SubLocation = subLocation,
            };
        }

        private CartesianDataPoint CaculateSalesAmountByProduct(string product, IEnumerable<RowInfo> subLocationSales, SubLocation subLocation)
        {
            var sales = subLocationSales.Where(x => x.Product == product).Select(x => x.SalesAmount);

            return new CartesianDataPoint()
            {
                Category = product,
                Value = this.Sum(sales),
                SubLocation = subLocation,
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

        #endregion
    }
}
