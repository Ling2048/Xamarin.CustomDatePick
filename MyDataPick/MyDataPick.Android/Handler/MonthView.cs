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
using Android.Util;
using Android.Graphics;
using Android.Icu.Util;
using MyDataPick.Droid.Helpers;

namespace MyDataPick.Droid.Handler
{
    public abstract class MonthView : View
    {
        protected int NUM_COLUMNS = 7;
        protected int NUM_ROWS = 6;
        protected Paint paint;
        //protected IDayTheme theme;
        //private IMonthLisener monthLisener;
        //private IDateClick dateClick;
        protected int currYear, currMonth, currDay;
        protected int selYear, selMonth, selDay;
        private int leftYear, leftMonth, leftDay;
        private int rightYear, rightMonth, rightDay;
        private int[,] daysString;
        protected float columnSize, rowSize, baseRowSize;
        private int mTouchSlop;
        protected float density;
        private int indexMonth;
        private int width;
        //protected List<CalendarInfo> calendarInfos = new ArrayList<CalendarInfo>();
        private int downX = 0, downY = 0;
        private Scroller mScroller;
        private int smoothMode;

        public MonthView(Context context, IAttributeSet attrs) : base(context, attrs)
        {
            density = Resources.DisplayMetrics.Density;// getResources().getDisplayMetrics().density;
            mScroller = new Scroller(context);
            mTouchSlop = ViewConfiguration.Get(context).ScaledDoubleTapSlop;//.GetScaledTouchSlop();
            Calendar calendar = Calendar.Instance;//.GetInstance();
            currYear = calendar.Get(Calendar.Year);
            currMonth = calendar.Get(Calendar.Month);
            currDay = calendar.Get(Calendar.Date);
            paint = new Paint(PaintFlags.AntiAlias);
            SetSelectDate(currYear, currMonth, currDay);
            SetLeftDate();
            SetRightDate();
            //createTheme();
            baseRowSize = rowSize = 70;// theme == null ? 70 : theme.dateHeight();
            smoothMode = 0;// theme == null ? 0 : theme.smoothMode();
        }

        protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec)
        {
            int widthSize = MeasureSpec.GetSize(widthMeasureSpec);
            MeasureSpecMode widthMode = MeasureSpec.GetMode(widthMeasureSpec);

            if (widthMode == MeasureSpecMode.AtMost)
            {
                widthSize = (int)(300 * density);
            }
            width = widthSize;
            NUM_ROWS = 6; //本来是想根据每月的行数，动态改变控件高度，现在为了使滑动的左右两边效果相同，不适用getMonthRowNumber();
            int heightSize = (int)(NUM_ROWS * baseRowSize);
            SetMeasuredDimension(widthSize, heightSize);
        }

        protected override void OnDraw(Canvas canvas)
        {
            canvas.DrawColor(Color.AliceBlue);
            if (smoothMode == 1)
            {
                DrawDate(canvas, selYear, selMonth, indexMonth * width, 0);
                return;
            }
            //绘制上一月份
            DrawDate(canvas, leftYear, leftMonth, (indexMonth - 1) * width, 0);
            //绘制下一月份
            DrawDate(canvas, rightYear, rightMonth, (indexMonth + 1) * width, 0);
            //绘制当前月份
            DrawDate(canvas, selYear, selMonth, indexMonth * width, 0);
        }

        private void DrawDate(Canvas canvas, int year, int month, int startX, int startY)
        {
            canvas.Save();
            canvas.Translate(startX, startY);
            NUM_ROWS = GetMonthRowNumber(year, month);
            columnSize = Width * 1.0F / NUM_COLUMNS;
            rowSize = Height * 1.0F / NUM_ROWS;
            daysString = new int[6,7];
            int mMonthDays = DateUtils.getMonthDays(year, month);
            int weekNumber = DateUtils.getFirstDayWeek(year, month);
            int column, row;
            DrawLines(canvas, NUM_ROWS);
            for (int day = 0; day < mMonthDays; day++)
            {
                column = (day + weekNumber - 1) % 7;
                row = (day + weekNumber - 1) / 7;
                daysString[row,column] = day + 1;
                DrawBG(canvas, column, row, daysString[row,column]);
                DrawDecor(canvas, column, row, year, month, daysString[row,column]);
                DrawRest(canvas, column, row, year, month, daysString[row,column]);
                DrawText(canvas, column, row, year, month, daysString[row,column]);
            }
            canvas.Restore();//.restore();
        }

        protected abstract void DrawLines(Canvas canvas, int rowsCount);
        protected abstract void DrawBG(Canvas canvas, int column, int row, int day);
        protected abstract void DrawDecor(Canvas canvas, int column, int row, int year, int month, int day);
        protected abstract void DrawRest(Canvas canvas, int column, int row, int year, int month, int day);
        protected abstract void DrawText(Canvas canvas, int column, int row, int year, int month, int day);

        private int lastMoveX;
        public override bool OnTouchEvent(MotionEvent e)
        {
            MotionEventActions eventCode = e.Action;//.getAction();
            switch(eventCode){
                case MotionEventActions.Down:
                    downX = (int)e.GetX();// GetX();
                    downY = (int)e.GetY();
                    break;
                case MotionEventActions.Move:
                    if(smoothMode == 1)break;
                    int dx = (int) (downX - e.GetX());
                    if(Math.Abs(dx) > mTouchSlop){
                        int moveX = dx + lastMoveX;
                        SmoothScrollTo(moveX, 0);
                    }
                    break;
                case MotionEventActions.Up:
                    int upX = (int) e.GetX();
                    int upY = (int) e.GetY();
                    if(upX-downX > 0 && Math.Abs(upX-downX) > mTouchSlop*10){//左滑
                        if(smoothMode == 0){
                            SetLeftDate();
                            indexMonth--;
                        }else{
                            //onLeftClick();
                        }
                    }else if(upX-downX < 0 && Math.Abs(upX-downX) > mTouchSlop*10){//右滑
                        if(smoothMode == 0){
                            SetRightDate();
                            indexMonth++;
                        }else{
                            //onRightClick();
                        }
                    }else if(Math.Abs(upX-downX) < 10 && Math.Abs(upY - downY) < 10){//点击事件
                        //performClick();
                        //doClickAction((upX + downX)/2,(upY + downY)/2);
                    }
                    if(smoothMode == 0) {
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
            mScroller.StartScroll(mScroller.FinalX, mScroller.FinalY, dx, dy, 500);
            Invalidate();//这里必须调用invalidate()才能保证computeScroll()会被调用，否则不一定会刷新界面，看不到滚动效果
        }

        /**
         * 设置选中的月份
         * @param year
         * @param month
         */
        protected void SetSelectDate(int year, int month, int day)
        {
            selYear = year;
            selMonth = month;
            selDay = day;
        }

        private void SetRightDate()
        {
            int year = selYear;
            int month = selMonth;
            int day = selDay;
            if (month == 11)
            {//若果是12月份，则变成1月份
                year = selYear + 1;
                month = 0;
            }
            else if (DateUtils.getMonthDays(year, month + 1) < day)
            {//向右滑动，当前月天数小于左边的
             //如果当前日期为该月最后一点，当向前推的时候，就需要改变选中的日期
                month = month + 1;
                day = DateUtils.getMonthDays(year, month);
            }
            else
            {
                month = month + 1;
            }
            SetSelectDate(year, month, day);
            computeDate();
        }

        private void SetLeftDate()
        {
            int year = selYear;
            int month = selMonth;
            int day = selDay;
            if (month == 0)
            {//若果是1月份，则变成12月份
                year = selYear - 1;
                month = 11;
            }
            else if (DateUtils.getMonthDays(year, month - 1) < day)
            {//向左滑动，当前月天数小于左边的
             //如果当前日期为该月最后一点，当向前推的时候，就需要改变选中的日期
                month = month - 1;
                day = DateUtils.getMonthDays(year, month);
            }
            else
            {
                month = month - 1;
            }
            SetSelectDate(year, month, day);
            computeDate();
        }

        private void computeDate()
        {
            if (selMonth == 0)
            {
                leftYear = selYear - 1;
                leftMonth = 11;
                rightYear = selYear;
                rightMonth = selMonth + 1;
            }
            else if (selMonth == 11)
            {
                leftYear = selYear;
                leftMonth = selMonth - 1;
                rightYear = selYear + 1;
                rightMonth = 0;
            }
            else
            {
                leftYear = selYear;
                leftMonth = selMonth - 1;
                rightYear = selYear;
                rightMonth = selMonth + 1;
            }
            //if (monthLisener != null)
            //{
            //    monthLisener.setTextMonth();
            //}
        }

        protected int GetMonthRowNumber(int year, int month)
        {
            int monthDays = DateUtils.getMonthDays(year, month);
            int weekNumber = DateUtils.getFirstDayWeek(year, month);
            return (monthDays + weekNumber - 1) % 7 == 0 ? (monthDays + weekNumber - 1) / 7 : (monthDays + weekNumber - 1) / 7 + 1;
        }
    }
}