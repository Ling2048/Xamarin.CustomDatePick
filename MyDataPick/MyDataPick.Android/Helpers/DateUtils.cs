using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Icu.Util;
using Android.Util;

namespace MyDataPick.Droid.Helpers
{
    public class DateUtils
    {
        /**
     * 通过年份和月份 得到当月的日子
     * 
     * @param year
     * @param month
     * @return
     */
        public static int getMonthDays(int year, int month)
        {
            //month++;
            switch (month)
            {
                case 1:
                case 3:
                case 5:
                case 7:
                case 8:
                case 10:
                case 12:
                    return 31;
                case 4:
                case 6:
                case 9:
                case 11:
                    return 30;
                case 2:
                    if (((year % 4 == 0) && (year % 100 != 0)) || (year % 400 == 0))
                    {
                        return 29;
                    }
                    else
                    {
                        return 28;
                    }
                default:
                    return -1;
            }
        }
        /**
         * 返回当前月份1号位于周几
         * @param year
         * 		年份
         * @param month
         * 		月份，传入系统获取的，不需要正常的
         * @return
         * 	日：1		一：2		二：3		三：4		四：5		五：6		六：7
         */
        public static int getFirstDayWeek(int year, int month)
        {
            //Calendar calendar = Calendar.Instance;//.getInstance();
            //calendar.Set(year, month, 1);
            DateTime first = new DateTime(year, month, 1);
            Log.WriteLine(LogPriority.Info, "DateView", "DateView:First:" + first.DayOfWeek);//.d());
            int week = (int)first.DayOfWeek + 1;
            return week;
        }

        /**
         * 根据列明获取周
         * @param column
         * @return
         */
        public static String getWeekName(int column)
        {
            switch (column)
            {
                case 0:
                    return "周日";
                case 1:
                    return "周一";
                case 2:
                    return "周二";
                case 3:
                    return "周三";
                case 4:
                    return "周四";
                case 5:
                    return "周五";
                case 6:
                    return "周六";
                default:
                    return "";
            }
        }
    }
}