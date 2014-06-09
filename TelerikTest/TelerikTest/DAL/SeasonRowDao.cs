using System;
using System.Collections.Generic;
using System.Linq;
using TelerikTest.Entity.Basic;
using TelerikTest.Enum;

namespace TelerikTest.DAL
{
    public class SeasonRowDao : IRowDao
    {
        private readonly int[] Q1 = { 1, 2, 3 };
        private readonly int[] Q2 = { 4, 5, 6, };
        private readonly int[] Q3 = { 7, 8, 9, };
        private readonly int[] Q4 = { 10, 11, 12 };

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
            var currentSeason = this.GetCurrentSeason(DateTime.Now.Month);

            return this.RowData.Where(
                x => x.SubLocation == subLocation &&
                     x.Year == year &&
                     (x.Month == currentSeason[0] || x.Month == currentSeason[1] || x.Month == currentSeason[2]));
        }

        public IEnumerable<RowInfo> GetYearSales(int year)
        {
            var currentSeason = this.GetCurrentSeason(DateTime.Now.Month);

            return this.RowData.Where(
                x => x.Year == year &&
                     (x.Month == currentSeason[0] || x.Month == currentSeason[1] || x.Month == currentSeason[2]));
        }

        public IEnumerable<RowInfo> GetSubLocationSales(SubLocation subLocation)
        {
            var currentSeason = this.GetCurrentSeason(DateTime.Now.Month);

            return this.RowData.Where(x => x.SubLocation == subLocation &&
                     (x.Month == currentSeason[0] || x.Month == currentSeason[1] || x.Month == currentSeason[2]));
        }

        public IEnumerable<RowInfo> GetStoreSalesAtAssignedSubLocation(string store, SubLocation subLocation)
        {
            var currentSeason = this.GetCurrentSeason(DateTime.Now.Month);

            return this.RowData.Where(x => x.Store == store && x.SubLocation == subLocation &&
                     (x.Month == currentSeason[0] || x.Month == currentSeason[1] || x.Month == currentSeason[2]));
        }

        public IEnumerable<RowInfo> GetSalesWhere_SubLocation_Brand(SubLocation subLocation, string brand)
        {
            var currentSeason = this.GetCurrentSeason(DateTime.Now.Month);

            return this.RowData.Where(x => x.SubLocation == subLocation && x.Brand == brand &&
                     (x.Month == currentSeason[0] || x.Month == currentSeason[1] || x.Month == currentSeason[2]));
        }

        public IEnumerable<RowInfo> GetStoreSalesWhere_SubLocation_Brand(string store, SubLocation subLocation, string brand)
        {
            var currentSeason = this.GetCurrentSeason(DateTime.Now.Month);

            return this.RowData.Where(x => x.Store == store && x.SubLocation == subLocation && x.Brand == brand &&
                     (x.Month == currentSeason[0] || x.Month == currentSeason[1] || x.Month == currentSeason[2]));
        }

        public IEnumerable<RowInfo> GetSalesWhere_SubLocation_Brand_Category(SubLocation subLocation, string brand, string category)
        {
            var currentSeason = this.GetCurrentSeason(DateTime.Now.Month);

            return this.RowData.Where(x => x.SubLocation == subLocation && x.Brand == brand && x.Category == category &&
                     (x.Month == currentSeason[0] || x.Month == currentSeason[1] || x.Month == currentSeason[2]));
        }

        public IEnumerable<RowInfo> GetStoreSalesWhere_SubLocation_Brand_Category(string store, SubLocation subLocation, string brand, string category)
        {
            var currentSeason = this.GetCurrentSeason(DateTime.Now.Month);

            return this.RowData.Where(x => x.Store == store && x.SubLocation == subLocation && x.Brand == brand && x.Category == category &&
                     (x.Month == currentSeason[0] || x.Month == currentSeason[1] || x.Month == currentSeason[2]));
        }

        private int[] GetCurrentSeason(int month)
        {
            switch (month)
            {
                case 1:
                    return this.Q1;
                case 2:
                    return this.Q1;
                case 3:
                    return this.Q1;
                case 4:
                    return this.Q2;
                case 5:
                    return this.Q2;
                case 6:
                    return this.Q2;
                case 7:
                    return this.Q3;
                case 8:
                    return this.Q3;
                case 9:
                    return this.Q3;
                case 10:
                    return this.Q4;
                case 11:
                    return this.Q4;
                case 12:
                    return this.Q4;
                default:
                    return this.Q1;
            }
        }
    }
}
