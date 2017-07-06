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
using MyDataPick.Droid.Renderers;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms;
using Android.Graphics;
using MyDataPick.Droid.Helpers;
using Android.Util;
using System.ComponentModel;
using MyDataPick.Views;
using MyDataPick.Models;

[assembly: ExportRenderer(typeof(MyDataPick.Views.ScrollMonthDateViewEx), typeof(ScrollMonthDateViewExRenderer))]
namespace MyDataPick.Droid.Renderers
{
    public class ScrollMonthDateViewExRenderer : ViewRenderer<MyDataPick.Views.ScrollMonthDateViewEx, Android.Views.View>, IDisposable
    {
        private static readonly int NUM_COLUMNS = 7;
        private static int NUM_ROWS = 6;
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
        private List<ThingModel> daysHasThingList;

        private int width;
        private int indexMonth;
        private Scroller mScroller;
        private int downX = 0, downY = 0;
        private int mTouchSlop;
        private int smoothMode;
        protected float columnSize, rowSize, baseRowSize;
        protected float density;
        //DateTime mSelDate;
        //DateTime mLeftDate;
        //DateTime mRightDate;

        public MyDataPick.Views.ScrollMonthDateViewEx _element
        {
            get { return (MyDataPick.Views.ScrollMonthDateViewEx)Element; }
        }

        protected override void OnAttachedToWindow()
        {
            base.OnAttachedToWindow();

            Log.Info("Life", "OnAttachedToWindow");
            density = Resources.DisplayMetrics.Density;
            mDisplayMetrics = Resources.DisplayMetrics;//.getDisplayMetrics();
            //Calendar calendar = Calendar.Instance;//.getInstance();
            mPaint = new Paint(PaintFlags.AntiAlias);
            mCurrYear = this.Element.Date.Year;// this.mSelYear;// calendar.Get(CalendarField.Year);//.get(Calendar.YEAR);
            mCurrMonth = this.Element.Date.Month;// this.mSelMonth;// calendar.Get(CalendarField.Month);//.get(Calendar.MONTH);
            mCurrDay = this.Element.Date.Day;// this.mSelDay;// calendar.Get(CalendarField.Date);//.get(Calendar.DATE);
            daysHasThingList = this.Element.Thing;
            mScroller = new Scroller(this.Context);
            mTouchSlop = ViewConfiguration.Get(this.Context).ScaledTouchSlop;//.GetScaledTouchSlop();
            smoothMode = 0;
            baseRowSize = rowSize = 70;
            SetSelectYearMonth(mCurrYear, mCurrMonth, mCurrDay);
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (e.PropertyName.Equals("Date"))
            {
                Log.WriteLine(LogPriority.Info, "Test", "OnElementPropertyChanged");
                ScrollMonthDateViewEx monthDateView = (ScrollMonthDateViewEx)sender;
                //daysHasThingList = monthDateView.Thing;
                SetSelectYearMonth(monthDateView.Date.Year, monthDateView.Date.Month, monthDateView.Date.Day);
                Invalidate();
            }
            else if (e.PropertyName.Equals("Thing"))
            {
                ScrollMonthDateViewEx monthDateView = (ScrollMonthDateViewEx)sender;
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
            this.ClipBounds = new Rect(0, 0, widthSize, heightSize);//控制显示区域
            if (widthMode == MeasureSpecMode.AtMost)
            {
                widthSize = (int)(300 * density);
            }
            width = widthSize;
            NUM_ROWS = 6; //本来是想根据每月的行数，动态改变控件高度，现在为了使滑动的左右两边效果相同，不适用getMonthRowNumber();
            heightSize = (int)(NUM_ROWS * baseRowSize);
            SetMeasuredDimension(widthSize, heightSize);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        protected override void OnDraw(Canvas canvas)
        {
            InitSize();
            //绘制上一月份
            DrawDate(canvas, this.Element.Date.AddMonths(-1).Year, this.Element.Date.AddMonths(-1).Month, (indexMonth - 1) * width, 0);
            //canvas.DrawColor(Android.Graphics.Color.Transparent, PorterDuff.Mode.Clear);
            //绘制下一月份
            DrawDate(canvas, this.Element.Date.AddMonths(1).Year, this.Element.Date.AddMonths(1).Month, (indexMonth + 1) * width, 0);
            //绘制当前月份
            DrawDate(canvas, this.Element.Date.Year, this.Element.Date.Month, indexMonth * width, 0);
        }

        private void DrawDate(Canvas canvas, int year, int month, int startX, int startY)
        {
            canvas.Save();
            canvas.Translate(startX, startY);
            #region 画时间
            daysString = new int[6, 7];
            mPaint.TextSize = mDaySize * mDisplayMetrics.ScaledDensity;//.scaledDensity;
            string dayString;
            int mMonthDays = DateUtils.getMonthDays(year, month);
            int weekNumber = DateUtils.getFirstDayWeek(year, month);
            int row = 0;
            if (weekNumber == 6)
            {

            }
            else
            {

            }
            Log.WriteLine(LogPriority.Info, "DateView", "DateView:" + mSelMonth + "月1号周" + weekNumber);//.d("DateView", "DateView:" + mSelMonth + "月1号周" + weekNumber);
            for (int day = 0; day < mMonthDays; day++)
            {
                dayString = (day + 1) + "";
                int column = (day + weekNumber - 1) % 7;
                row = (day + weekNumber - 1) / 7;
                if (weekNumber == 6)
                {
                    row += 1;
                }
                daysString[row, column] = day + 1;
                startX = (int)(mColumnSize * column + (mColumnSize - mPaint.MeasureText(dayString)) / 2);
                startY = (int)(mRowSize * row + mRowSize / 2 - (mPaint.Ascent() + mPaint.Descent()) / 2);
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
                DrawCircle(row, column, year, month, day + 1, canvas);
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
            #endregion
            canvas.Restore();//.restore();
        }

        private int lastMoveX;
        public override bool OnTouchEvent(MotionEvent e)
        {
            MotionEventActions eventCode = e.Action;//.getAction();
            switch (eventCode)
            {
                case MotionEventActions.Down:
                    downX = (int)e.GetX();// GetX();
                    downY = (int)e.GetY();
                    break;
                case MotionEventActions.Move:
                    float dxx = downX - e.GetX();
                    int dx = (int)(downX - e.GetX());
                    if (Math.Abs(dx) > mTouchSlop)
                    {
                        int moveX = dx + lastMoveX;
                        SmoothScrollTo(moveX, 0);
                    }
                    break;
                case MotionEventActions.Up:
                    int upX = (int)e.GetX();
                    int upY = (int)e.GetY();
                    if (upX - downX > 0 && Math.Abs(upX - downX) > mTouchSlop)
                    {//左滑
                        if (smoothMode == 0)
                        {
                            //SetLeftDate();
                            indexMonth--;
                            this.Element.Date = this.Element.Date.AddMonths(-1);
                        }
                        else
                        {
                            //onLeftClick();
                        }
                    }
                    else if (upX - downX < 0 && Math.Abs(upX - downX) > mTouchSlop)
                    {//右滑
                        if (smoothMode == 0)
                        {
                            //SetRightDate();
                            indexMonth++;
                            this.Element.Date = this.Element.Date.AddMonths(1);
                        }
                        else
                        {
                            //onRightClick();
                        }
                    }
                    else if (Math.Abs(upX - downX) < 10 && Math.Abs(upY - downY) < 10)
                    {//点击事件
                        PerformClick();
                        DoClickAction((upX + downX) / 2, (upY + downY) / 2);
                    }
                    if (smoothMode == 0)
                    {
                        lastMoveX = indexMonth * width;
                        SmoothScrollTo(width * indexMonth, 0);
                    }
                    break;
            }
            return true;
        }

        //调用此方法滚动到目标位置
        public void SmoothScrollTo(int fx, int fy)
        {
            int dx = fx - mScroller.FinalX;//.getFinalX();
            int dy = fy - mScroller.FinalY;//.getFinalY();
            SmoothScrollBy(dx, dy);
        }

        //调用此方法设置滚动的相对偏移
        public void SmoothScrollBy(int dx, int dy)
        {
            //设置mScroller的滚动偏移量
            mScroller.StartScroll(mScroller.FinalX, mScroller.FinalY, dx, dy, 200);
            Invalidate();//这里必须调用invalidate()才能保证computeScroll()会被调用，否则不一定会刷新界面，看不到滚动效果
        }

        public override void ComputeScroll()
        {
            if (mScroller.ComputeScrollOffset())
            {
                ScrollTo(mScroller.CurrX, mScroller.CurrY);
                Invalidate();
            }
            //base.ComputeScroll();
        }

        //private void SetRightDate()
        //{
        //    int year = mSelYear;
        //    int month = mSelMonth;
        //    int day = mSelDay;
        //    if (month == 11)
        //    {//若果是12月份，则变成1月份
        //        year = mSelYear + 1;
        //        month = 0;
        //    }
        //    else if (DateUtils.getMonthDays(year, month + 1) < day)
        //    {//向右滑动，当前月天数小于左边的
        //     //如果当前日期为该月最后一点，当向前推的时候，就需要改变选中的日期
        //        month = month + 1;
        //        day = DateUtils.getMonthDays(year, month);
        //    }
        //    else
        //    {
        //        month = month + 1;
        //    }
        //    SetSelectYearMonth(year, month, day);
        //    computeDate();
        //}

        //private void SetLeftDate()
        //{
        //    int year = mSelYear;
        //    int month = mSelMonth;
        //    int day = mSelDay;
        //    if (month == 0)
        //    {//若果是1月份，则变成12月份
        //        year = mSelYear - 1;
        //        month = 11;
        //    }
        //    else if (DateUtils.getMonthDays(year, month - 1) < day)
        //    {//向左滑动，当前月天数小于左边的
        //     //如果当前日期为该月最后一点，当向前推的时候，就需要改变选中的日期
        //        month = month - 1;
        //        day = DateUtils.getMonthDays(year, month);
        //    }
        //    else
        //    {
        //        month = month - 1;
        //    }
        //    SetSelectYearMonth(year, month, day);
        //    computeDate();
        //}

        //private void computeDate()
        //{
        //    if (mSelMonth == 0)
        //    {
        //        leftYear = mSelYear - 1;
        //        leftMonth = 11;
        //        rightYear = mSelYear;
        //        rightMonth = mSelMonth + 1;
        //    }
        //    else if (mSelMonth == 11)
        //    {
        //        leftYear = mSelYear;
        //        leftMonth = mSelMonth - 1;
        //        rightYear = mSelYear + 1;
        //        rightMonth = 0;
        //    }
        //    else
        //    {
        //        leftYear = mSelYear;
        //        leftMonth = mSelMonth - 1;
        //        rightYear = mSelYear;
        //        rightMonth = mSelMonth + 1;
        //    }
        //    //if (monthLisener != null)
        //    //{
        //    //    monthLisener.setTextMonth();
        //    //}
        //}


        /**
         * 执行点击事件
         * @param x
         * @param y
         */
        private void DoClickAction(int x, int y)
        {
            int row = y / mRowSize;
            int column = x / mColumnSize;
            SetSelectYearMonth(mSelYear, mSelMonth, daysString[row, column]);
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
                DateTime date = new DateTime(year, month, day);
                var models = from model in daysHasThingList
                where model.Date == date
                select model;
                if (models.Count() <= 0) return;
                if (models.First().IsDone) mPaint.Color = Android.Graphics.Color.LawnGreen;
                else mPaint.Color = mCircleColor;
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