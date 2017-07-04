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
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms;
using MyDataPick.Droid.Renderers;
using Android.Graphics;
using Android.Util;
using Java.Lang;
using Android.Icu.Util;
using MyDataPick.Droid.Helpers;
using MyDataPick.Views;
using System.ComponentModel;
using MyDataPick.Droid.Theme;

[assembly: ExportRenderer(typeof(MyDataPick.Views.MonthDateView), typeof(MonthDateViewRenderer))]
namespace MyDataPick.Droid.Renderers
{
    public class MonthDateViewRenderer : ViewRenderer<MyDataPick.Views.MonthDateView, Android.Views.View>,IDisposable
    {
        private static readonly int NUM_COLUMNS = 7;
        private static readonly int NUM_ROWS = 6;
        private Paint mPaint;
        private Android.Graphics.Color mDayColor = Android.Graphics.Color.ParseColor("#000000");
        private Android.Graphics.Color mSelectDayColor = Android.Graphics.Color.ParseColor("#ffffff");
        private Android.Graphics.Color mSelectBGColor = Android.Graphics.Color.ParseColor("#1FC2F3");
        private Android.Graphics.Color mCurrentColor = Android.Graphics.Color.ParseColor("#ff0000");
        private int mCurrYear, mCurrMonth, mCurrDay;
        private int mSelYear, mSelMonth, mSelDay;
        private int mColumnSize, mRowSize;
        private DisplayMetrics mDisplayMetrics;
        private int mDaySize = 18;
        private int weekRow;
        private int[,] daysString;
        private int mCircleRadius = 6;
        //private Action<int> dateClick;
        private Android.Graphics.Color mCircleColor = Android.Graphics.Color.ParseColor("#ff0000");
        private List<DateTime> daysHasThingList;

        public MyDataPick.Views.MonthDateView _element
        {
            get { return (MyDataPick.Views.MonthDateView)Element; }
        }

        protected override void OnAttachedToWindow()
        {
            base.OnAttachedToWindow();

            Log.Info("Life", "OnAttachedToWindow");
            mDisplayMetrics = Resources.DisplayMetrics;//.getDisplayMetrics();
            //Calendar calendar = Calendar.Instance;//.getInstance();
            mPaint = new Paint(PaintFlags.AntiAlias);
            mCurrYear = this.Element.Date.Year;// this.mSelYear;// calendar.Get(CalendarField.Year);//.get(Calendar.YEAR);
            mCurrMonth = this.Element.Date.Month;// this.mSelMonth;// calendar.Get(CalendarField.Month);//.get(Calendar.MONTH);
            mCurrDay = this.Element.Date.Day;// this.mSelDay;// calendar.Get(CalendarField.Date);//.get(Calendar.DATE);
            daysHasThingList = this.Element.Thing;

            SetSelectYearMonth(mCurrYear, mCurrMonth, mCurrDay);
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (e.PropertyName.Equals("Date"))
            {
                Log.WriteLine(LogPriority.Info, "Test", "OnElementPropertyChanged");
                MonthDateView monthDateView = (MonthDateView)sender;
                //daysHasThingList = monthDateView.Thing;
                SetSelectYearMonth(monthDateView.Date.Year, monthDateView.Date.Month, monthDateView.Date.Day);
                Invalidate();
            }
            else if (e.PropertyName.Equals("Thing"))
            {
                MonthDateView monthDateView = (MonthDateView)sender;
                daysHasThingList = monthDateView.Thing;
                //SetSelectYearMonth(monthDateView.Date.Year, monthDateView.Date.Month, monthDateView.Date.Day);
                Invalidate();
            }
        }

        protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec)
        {
            Log.WriteLine(LogPriority.Info, "Test", "OnMeasure");

            int widthSize = MeasureSpec.GetSize(widthMeasureSpec);
            MeasureSpecMode widthMode = MeasureSpec.GetMode(widthMeasureSpec);

            int heightSize = MeasureSpec.GetSize(heightMeasureSpec);
            MeasureSpecMode heightMode = MeasureSpec.GetMode(heightMeasureSpec);

            if (heightMode == MeasureSpecMode.AtMost)
            {
                heightSize = Convert.ToInt32(mDisplayMetrics.DensityDpi) * 200;
            }
            if (widthMode == MeasureSpecMode.AtMost)
            {
                widthSize = Convert.ToInt32(mDisplayMetrics.DensityDpi) * 300;
            }
            SetMeasuredDimension(widthSize, heightSize);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        protected override void OnDraw(Canvas canvas)
        {
            InitSize();
            daysString = new int[6,7];
            mPaint.TextSize = mDaySize * mDisplayMetrics.ScaledDensity;//.scaledDensity;
            string dayString;
            int mMonthDays = DateUtils.getMonthDays(mSelYear, mSelMonth);
            int weekNumber = DateUtils.getFirstDayWeek(mSelYear, mSelMonth);
            Log.WriteLine(LogPriority.Info, "DateView", "DateView:" + mSelMonth + "月1号周" + weekNumber);//.d("DateView", "DateView:" + mSelMonth + "月1号周" + weekNumber);
            for (int day = 0; day < mMonthDays; day++)
            {
                dayString = (day + 1) + "";
                int column = (day + weekNumber - 1) % 7;
                int row = (day + weekNumber - 1) / 7;
                daysString[row,column] = day + 1;
                int startX = (int)(mColumnSize * column + (mColumnSize - mPaint.MeasureText(dayString)) / 2);
                int startY = (int)(mRowSize * row + mRowSize / 2 - (mPaint.Ascent() + mPaint.Descent()) / 2);
                if (dayString.Equals(mSelDay + ""))
                {
                    //绘制背景色矩形
                    int startRecX = mColumnSize * column;
                    int startRecY = mRowSize * row;
                    int endRecX = startRecX + mColumnSize;
                    int endRecY = startRecY + mRowSize;
                    mPaint.Color = mSelectBGColor;//.SetColor(mSelectBGColor);
                    canvas.DrawRect(startRecX, startRecY, endRecX, endRecY, mPaint);
                    //记录第几行，即第几周
                    weekRow = row + 1;
                }
                //绘制事务圆形标志
                DrawCircle(row, column, mSelYear, mSelMonth, day + 1, canvas);
                if (dayString.Equals(mSelDay + ""))
                {
                    mPaint.Color = mSelectDayColor;
                }
                else if (dayString.Equals(mCurrDay + "") && mCurrDay != mSelDay && mCurrMonth == mSelMonth && mCurrYear == mSelYear)
                {
                    //正常月，选中其他日期，则今日为红色
                    mPaint.Color = mCurrentColor;
                }
                else
                {
                    mPaint.Color = mDayColor;
                }
                canvas.DrawText(dayString, startX, startY, mPaint);
            }
        }

        private int downX = 0, downY = 0;
        public override bool OnTouchEvent(MotionEvent e)
        {
            Android.Views.MotionEventActions eventCode = e.Action;//  event.getAction();
            switch (eventCode)
            {
                case MotionEventActions.Down:
                    downX = (int)e.GetX();//.X;
                    downY = (int)e.GetY();// ;
                    break;
                case MotionEventActions.Move:
                    break;
                case MotionEventActions.Up:
                    int upX = (int) e.GetX();
                    int upY = (int) e.GetY();
                    if (Java.Lang.Math.Abs(upX - downX) < 10 && Java.Lang.Math.Abs(upY - downY) < 10)
                    {//点击事件
                        PerformClick();
                        DoClickAction((upX + downX) / 2, (upY + downY) / 2);
                    }
                    break;
            }
            return true;
        }


        /**
         * 执行点击事件
         * @param x
         * @param y
         */
        private void DoClickAction(int x, int y)
        {
            int row = y / mRowSize;
            int column = x / mColumnSize;
            SetSelectYearMonth(mSelYear, mSelMonth, daysString[row,column]);
            Invalidate();
            //执行activity发送过来的点击处理事件
            if (this.Element.Click != null)
            {
                this.Element.Click.Invoke(mSelYear, mSelMonth, daysString[row, column]);//.onClickOnDate();
            }
        }

        private void DrawCircle(int row, int column, int year, int month, int day, Canvas canvas)
        {
            if (daysHasThingList != null && daysHasThingList.Count > 0)
            {
                if (!daysHasThingList.Contains(new DateTime(year, month, day))) return;
                mPaint.Color = mCircleColor;
                float circleX = (float)(mColumnSize * column + mColumnSize * 0.8);
                float circley = (float)(mRowSize * row + mRowSize * 0.2);
                canvas.DrawCircle(circleX, circley, mCircleRadius, mPaint);
            }
        }

        public override bool PerformClick()
        {
            return base.PerformClick();
        }

        /**
         * 初始化列宽行高
         */
        private void InitSize()
        {
            mColumnSize = this.Width / NUM_COLUMNS;// getWidth() / NUM_COLUMNS;
            mRowSize = this.Height / NUM_ROWS;// getHeight() / NUM_ROWS;
        }

        /**
         * 设置年月
         * @param year
         * @param month
         */
        private void SetSelectYearMonth(int year, int month, int day)
        {
            mSelYear = year;
            mSelMonth = month;
            mSelDay = day;
        }
    }
}