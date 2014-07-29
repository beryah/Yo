using System;
using System.Linq;

namespace UnitTestProject1
{
    public static class DateTimeExtensionMethod
    {
        #region Fields

        private static readonly int startYear = 1901;
        private static readonly int endYear = 2050;

        /// <summary>
        /// 農曆每個月的天數 ( 紀錄 1901.1.1 到 2050.12.31 的每一個月 )
        /// 農曆每月只能是 29 或 30 天，一年用 12（或13）個二進制位表示，對應位元為 1 表 30 天，否則為 29 天
        /// </summary>
        private static readonly int[] gLunarMonthDay = {
              0x4ae0, 0xa570, 0x5268, 0xd260, 0xd950, 0x6aa8, 0x56a0, 0x9ad0, 0x4ae8, 0x4ae0, //1910
              0xa4d8, 0xa4d0, 0xd250, 0xd548, 0xb550, 0x56a0, 0x96d0, 0x95b0, 0x49b8, 0x49b0, //1920
              0xa4b0, 0xb258, 0x6a50, 0x6d40, 0xada8, 0x2b60, 0x9570, 0x4978, 0x4970, 0x64b0, //1930
              0xd4a0, 0xea50, 0x6d48, 0x5ad0, 0x2b60, 0x9370, 0x92e0, 0xc968, 0xc950, 0xd4a0, //1940
              0xda50, 0xb550, 0x56a0, 0xaad8, 0x25d0, 0x92d0, 0xc958, 0xa950, 0xb4a8, 0x6ca0, //1950
              0xb550, 0x55a8, 0x4da0, 0xa5b0, 0x52b8, 0x52b0, 0xa950, 0xe950, 0x6aa0, 0xad50, //1960
              0xab50, 0x4b60, 0xa570, 0xa570, 0x5260, 0xe930, 0xd950, 0x5aa8, 0x56a0, 0x96d0, //1970
              0x4ae8, 0x4ad0, 0xa4d0, 0xd268, 0xd250, 0xd528, 0xb540, 0xb6a0, 0x96d0, 0x95b0, //1980
              0x49b0, 0xa4b8, 0xa4b0, 0xb258, 0x6a50, 0x6d40, 0xada0, 0xab60, 0x9370, 0x4978, //1990
              0x4970, 0x64b0, 0x6a50, 0xea50, 0x6b28, 0x5ac0, 0xab60, 0x9368, 0x92e0, 0xc960, //2000
              0xd4a8, 0xd4a0, 0xda50, 0x5aa8, 0x56a0, 0xaad8, 0x25d0, 0x92d0, 0xc958, 0xa950, //2010
              0xb4a0, 0xb550, 0xb550, 0x55a8, 0x4ba0, 0xa5b0, 0x52b8, 0x52b0, 0xa930, 0x74a8, //2020
              0x6aa0, 0xad50, 0x4da8, 0x4b60, 0x9570, 0xa4e0, 0xd260, 0xe930, 0xd530, 0x5aa0, //2030
              0x6b50, 0x96d0, 0x4ae8, 0x4ad0, 0xa4d0, 0xd258, 0xd250, 0xd520, 0xdaa0, 0xb5a0, //2040
              0x56d0, 0x4ad8, 0x49b0, 0xa4b8, 0xa4b0, 0xaa50, 0xb528, 0x6d20, 0xada0, 0x55b0 }; //2050

        /// <summary>
        /// 農曆每年潤月的月份 ( 紀錄 1901.1.1 到 2050.12.31 的每一個月 )
        /// 如沒有則為0，每個 byte 儲存兩年的資料
        /// </summary>
        private static readonly byte[] gLunarMonth = {
              0x00, 0x50, 0x04, 0x00, 0x20, //1910
              0x60, 0x05, 0x00, 0x20, 0x70, //1920
              0x05, 0x00, 0x40, 0x02, 0x06, //1930
              0x00, 0x50, 0x03, 0x07, 0x00, //1940
              0x60, 0x04, 0x00, 0x20, 0x70, //1950
              0x05, 0x00, 0x30, 0x80, 0x06, //1960
              0x00, 0x40, 0x03, 0x07, 0x00, //1970
              0x50, 0x04, 0x08, 0x00, 0x60, //1980
              0x04, 0x0a, 0x00, 0x60, 0x05, //1990
              0x00, 0x30, 0x80, 0x05, 0x00, //2000
              0x40, 0x02, 0x07, 0x00, 0x50, //2010
              0x04, 0x09, 0x00, 0x60, 0x04, //2020
              0x00, 0x20, 0x60, 0x05, 0x00, //2030
              0x30, 0xb0, 0x06, 0x00, 0x50, //2040
              0x02, 0x07, 0x00, 0x50, 0x03 }; //2050

        #endregion Fields

        /// <summary>
        /// 計算國曆日期 date 的農曆日期 (有效期間為：1901年1月---2050年12月)
        /// </summary>
        /// <param name="source">欲計算的國曆日期</param>
        /// <returns>農曆日期</returns>
        public static ChineseLunarDateTime GetChineseLunarDate(this DateTime source)
        {
            // 驗證輸入年分是否在計算範圍內
            if ((source.Year < startYear) || (source.Year > endYear))
            {
                throw new NotImplementedException();
            }

            // 計算自 1901 年起經過的天數
            var spanDays = (source - new DateTime(startYear, 1, 1)).Days;

            // 陽曆1901年2月19日為農曆1901年正月初一
            // 陽曆1901年1月1日到2月19日共有49天
            if (spanDays < 49)
            {
                if (spanDays < 19)
                {
                    return new ChineseLunarDateTime(startYear - 1, 11, 11 + spanDays, false);
                }
                else
                {
                    return new ChineseLunarDateTime(startYear - 1, 12, spanDays - 18, false);
                }
            }
            else
            {
                // 下面從農曆1901年正月初一算起
                spanDays = spanDays - 49;

                var year = startYear;
                var month = 1;
                var day = 1;
                var isLeap = false;

                // 計算年
                var tmp = LunarYearDays(year);
                while (spanDays >= tmp)
                {
                    spanDays = spanDays - tmp;
                    year++;
                    tmp = LunarYearDays(year);
                }

                // 計算月
                tmp = LunarMonthDays(year, month) & 0xFFFF; //取低位
                var leap = GetLeapMonth(year);
                while (spanDays >= tmp)
                {
                    spanDays = spanDays - tmp;

                    // 若該月份有閏月，需再計算閏月天數
                    if (month == leap)
                    {
                        tmp = (LunarMonthDays(year, month) >> 16) & 0xFFFF; //取高位

                        if (spanDays < tmp)
                        {
                            isLeap = true;
                            break;
                        }

                        spanDays = spanDays - tmp;
                    }

                    month++;
                    tmp = LunarMonthDays(year, month) & 0xFFFF; //取低位
                }

                // 計算日
                day = day + spanDays;

                return new ChineseLunarDateTime(year, month, day, isLeap);
            }
        }

        /// <summary>
        /// 取得農曆 year 年的總天數 (有效期間為：1901年1月---2050年12月)
        /// </summary>
        /// <param name="year">農曆年</param>
        /// <returns>農曆 year 年的總天數</returns>
        private static int LunarYearDays(int year)
        {
            int days = 0;

            foreach (var i in Enumerable.Range(1, 12))
            {
                var tmp = LunarMonthDays(year, i);
                days = days + (tmp >> 16) & 0xFFFF; //取高位
                days = days + (tmp) & 0xFFFF; //取低位
            }

            return days;
        }

        /// <summary>
        /// 取回農曆 year 年與農曆 month 月的農曆天數
        /// 如果 month 為閏月，高字為該閏月的天數，否則高字為0，例如2014年9月為閏月，回傳值的高字為閏9月的天數，低字為9月的天數
        /// (有效期間為：1901年1月---2050年12月)
        /// </summary>
        /// <param name="year">農曆年份</param>
        /// <param name="month">農曆月份</param>
        /// <returns>農曆 year 年與農曆 month 月的農曆天數</returns>
        private static int LunarMonthDays(int year, int month)
        {
            var leap = GetLeapMonth(year);

            // 如果該年分有閏月，且 month 在該閏月之後
            var bit = (month > leap) && (leap > 0) ? 15 - month : 16 - month;

            // 低字表示 month 農曆月份的天數，查詢 gLunarMonthDay 判斷為 大月(30天) 或 小月(29天)
            var low = (gLunarMonthDay[year - startYear] & (1 << bit)) > 0 ? 30 : 29;

            // 高字表示 month 農曆閏月份的天數，若非閏月則為 0 ，若為閏月則查詢 gLunarMonthDay 判斷為 大月(30天) 或 小月(29天)
            var height = month != leap ? 0 : (gLunarMonthDay[year - startYear] & (1 << (bit - 1))) > 0 ? 30 : 29;

            // 合成為int
            return ((low) | (height) << 16);
        }

        /// <summary>
        /// 返回農曆 year 年(西元年) 的閏月月份，如沒有返回 0 (有效期間為：1901年1月---2050年12月)
        /// </summary>
        /// <param name="year">農曆年份</param>
        /// <returns>農曆 year 年(西元年) 的閏月月份</returns>
        private static int GetLeapMonth(int year)
        {
            var flag = gLunarMonth[(year - startYear) / 2];

            if ((year - startYear) % 2 == 0)
            {
                return flag >> 4;
            }
            else
            {
                return flag & 0x0F;
            }
        }
    }
}