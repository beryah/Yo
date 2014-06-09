using System;
using System.Collections.Generic;
using System.Linq;
using TelerikTest.Entity.Basic;
using TelerikTest.Enum;

namespace TelerikTest.DAL
{
    public class MonthRowDao : IRowDao
    {
        private List<RowInfo> RowData
        {
            get
            {
                var app = App.Current as App;
                return app.Data;
            }
        }

        public IEnumerable<RowInfo> GetYearSalesAtAssignedSubLocation(SubLocation subLocation, int year)
        {
            return this.RowData.Where(x => x.SubLocation == subLocation && x.Year == year && x.Month == DateTime.Now.Month);
        }

        public IEnumerable<RowInfo> GetYearSales(int year)
        {
            return this.RowData.Where(x => x.Year == year && x.Month == DateTime.Now.Month);
        }

        public IEnumerable<RowInfo> GetSubLocationSales(SubLocation subLocation)
        {
            return this.RowData.Where(x => x.SubLocation == subLocation && x.Month == DateTime.Now.Month);
        }

        public IEnumerable<RowInfo> GetStoreSalesAtAssignedSubLocation(string store, SubLocation subLocation)
        {
            return this.RowData.Where(x => x.Store == store && x.SubLocation == subLocation && x.Month == DateTime.Now.Month);
        }

        public IEnumerable<RowInfo> GetSalesWhere_SubLocation_Brand(SubLocation subLocation, string brand)
        {
            return this.RowData.Where(x => x.SubLocation == subLocation && x.Month == DateTime.Now.Month && x.Brand == brand);
        }

        public IEnumerable<RowInfo> GetStoreSalesWhere_SubLocation_Brand(string store, SubLocation subLocation, string brand)
        {
            return this.RowData.Where(x => x.Store == store && x.SubLocation == subLocation && x.Month == DateTime.Now.Month && x.Brand == brand);
        }

        public IEnumerable<RowInfo> GetSalesWhere_SubLocation_Brand_Category(SubLocation subLocation, string brand, string category)
        {
            return this.RowData.Where(x => x.SubLocation == subLocation && x.Month == DateTime.Now.Month && x.Brand == brand && x.Category == category);
        }

        public IEnumerable<RowInfo> GetStoreSalesWhere_SubLocation_Brand_Category(string store, SubLocation subLocation, string brand, string category)
        {
            return this.RowData.Where(x => x.Store == store && x.SubLocation == subLocation && x.Month == DateTime.Now.Month && x.Brand == brand && x.Category == category);
        }
    }
}
