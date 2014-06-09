using System;
using System.Collections.Generic;
using TelerikTest.Entity.Basic;
using TelerikTest.Enum;

namespace TelerikTest.DAL
{
    public interface IRowDao
    {
        IEnumerable<RowInfo> GetYearSalesAtAssignedSubLocation(SubLocation subLocation, int year);

        IEnumerable<RowInfo> GetYearSales(int year);

        IEnumerable<RowInfo> GetSubLocationSales(SubLocation subLocation);

        IEnumerable<RowInfo> GetStoreSalesAtAssignedSubLocation(string store, SubLocation subLocation);

        IEnumerable<RowInfo> GetSalesWhere_SubLocation_Brand(SubLocation subLocation, string brand);

        IEnumerable<RowInfo> GetStoreSalesWhere_SubLocation_Brand(string store, SubLocation subLocation, string brand);

        IEnumerable<RowInfo> GetSalesWhere_SubLocation_Brand_Category(SubLocation subLocation, string brand, string category);

        IEnumerable<RowInfo> GetStoreSalesWhere_SubLocation_Brand_Category(string store, SubLocation subLocation, string brand, string category);
    }
}
