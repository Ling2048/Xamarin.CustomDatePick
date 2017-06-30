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

namespace MyDataPick.Droid.Theme
{
    public interface IDayTheme
    {
        /**
        * 选中日期的背景色
        * @return 16进制颜色值 hex color
        */
        int ColorSelectBG();

        /**
         * 选中日期的颜色
         * @return 16进制颜色值 hex color
         */
        int ColorSelectDay();

        /**
         * 今天日期颜色
         * @return 16进制颜色值 hex color
         */
        int ColorToday();

        /**
         * 日历的整个背景色
         * @return
         */
        int ColorMonthView();

        /**
         * 工作日的颜色
         * @return
         */
        int ColorWeekday();

        /**
         * 周末的颜色
         * @return
         */
        int ColorWeekend();

        /**
         * 事务装饰颜色
         * @return  16进制颜色值 hex color
         */
        int ColorDecor();

        /**
         * 假日颜色
         * @return 16进制颜色值 hex color
         */
        int ColorRest();

        /**
         * 班颜色
         * @return  16进制颜色值 hex color
         */
        int ColorWork();

        /**
         * 描述文字颜色
         * @return 16进制颜色值 hex color
         */
        int ColorDesc();

        /**
         * 日期大小
         * @return
         */
        int SizeDay();

        /**
         * 描述文字大小
         * @return
         */
        int SizeDesc();

        /**
         * 装饰器大小
         * @return
         */
        int SizeDecor();
        /**
         * 日期高度
         * @return
         */
        int DateHeight();

        /**
         * 线条颜色
         * @return
         */
        int ColorLine();

        /**
         * 滑动模式  0是渐变滑动方式，1是没有滑动方式
         * @return
         */
        int SmoothMode();
    }
}