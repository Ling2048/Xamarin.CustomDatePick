using System;
using System.Drawing;

using CoreGraphics;
using Foundation;
using UIKit;
using MyDataPick.iOS.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(MyDataPick.Views.ScrollMonthDateViewEx), typeof(ScrollMonthDateViewExRenderer))]
namespace MyDataPick.iOS.Renderers
{

    public class ScrollMonthDateViewExRenderer : ViewRenderer<MyDataPick.Views.ScrollMonthDateViewEx, UIView>
    {
        UIColor mDayColor = UIColor.FromRGB(0, 0, 0);
        UIColor mSelectDayColor = UIColor.FromRGB(255, 255, 255);
        UIColor mSelectBGColor = UIColor.FromRGB(31, 194, 243);
        UIColor mCurrentColor = UIColor.FromRGB(255, 0, 0);
        UIColor mCircleColor = UIColor.FromRGB(255, 0, 0);
        UIColor oDayColor = UIColor.Gray;
        private int[,] daysString;
        private int mDaySize = 20;
        private static readonly int NUM_COLUMNS = 7;
        private static int NUM_ROWS = 6;
        private int mColumnSize, mRowSize;
        private int mSelYear, mSelMonth, mSelDay;
        private int weekRow;

        //UIScrollView scrollView = new UIScrollView();

        public ScrollMonthDateViewExRenderer() { }

        public override SizeRequest GetDesiredSize(double widthConstraint, double heightConstraint)
        {
            //scrollView.Frame = new CGRect(0, 0, widthConstraint, heightConstraint);
            //this.Frame = new CGRect(0, 0, widthConstraint, heightConstraint);
            this.Layer.ShouldRasterize = true;
            this.mColumnSize = (int)widthConstraint / NUM_COLUMNS;
            mRowSize = (int)heightConstraint / NUM_ROWS;// getHeight() / NUM_ROWS;
            //scrollView.AddSubview(this);

            return base.GetDesiredSize(widthConstraint, heightConstraint);
        }

        public override void Draw(CGRect rect)
        {
            Console.WriteLine("ScrollMonthDateViewExRenderer_Draw");
            DrawDate(DateTime.Now, 0, 0);
            base.Draw(rect);
        }

        private void DrawDate(DateTime dateTime, float startX, float startY)
        {

            int year = dateTime.Year;
            int month = dateTime.Month;

            #region 画时间
            daysString = new int[6, 7];
            UIFont font = UIFont.BoldSystemFontOfSize(mDaySize);
            //mPaint.TextSize = mDaySize * mDisplayMetrics.ScaledDensity;//.scaledDensity;
            string dayString;
            int pMonthDays = getMonthDays(dateTime.AddMonths(-1).Year, dateTime.AddMonths(-1).Month);
            int mMonthDays = getMonthDays(year, month);
            int weekNumber = (int)new DateTime(year, month, 1).DayOfWeek + 1;// DateUtils.getFirstDayWeek(year, month);
            int row = 0, column = 0;
            int firstDay = 0;
            bool isPreviousDone = true;
            if (weekNumber == 1)
            {
                firstDay = pMonthDays - 7;
            }
            else
            {
                firstDay = pMonthDays - weekNumber + 1;
            };
            oDayColor.SetFill();
            for (int day = firstDay; row < 6; row++)
            {
                for (; column < 7; day++, column++)
                {
                    if (day == pMonthDays && isPreviousDone)
                    {
                        day = 0;
                        isPreviousDone = false;
                    }
                    else if (day == mMonthDays && !isPreviousDone)
                    {
                        day = 0;
                        oDayColor.SetFill();
                        isPreviousDone = true;
                    }

                    dayString = (day + 1).ToString();
                    daysString[row, column] = day + 1;
                    //startX = columnWidth * i + (columnWidth - fontWidth) / 2;
                    //startY = (int)(height / 2 - ((float)text.StringSize(font).Height) / 2);
                    float fontWidth = (float)day.ToString().StringSize(font).Width;
                    startX = (mColumnSize * column + (mColumnSize - fontWidth) / 2);
                    startY = (mRowSize * row + mRowSize / 2 - (float)day.ToString().StringSize(font).Height / 2);
                    Console.WriteLine(day + "," + day.ToString().StringSize(font).Width + "," + fontWidth);
                    if (day == 9)
                    {
                        //Console.WriteLine("day==10," + day.ToString().StringSize(font).Width + "," + fontWidth);
                        startX -= fontWidth / 2;// (int)(mColumnSize - fontWidth) / 4;
                    }


                    if (dayString.Equals(mSelDay + "") && !isPreviousDone)
                    {
                        //绘制背景色矩形
                        int startRecX = mColumnSize * column;
                        int startRecY = mRowSize * row;
                        int endRecX = startRecX + mColumnSize;
                        int endRecY = startRecY + mRowSize;
                        mSelectBGColor.SetFill();//.SetColor(mSelectBGColor);
                        //this.DrawRect(new CGRect(startRecX, startRecY, endRecX, endRecY), UIViewPrintFormatter.);
                        //canvas.DrawRect(startRecX, startRecY, endRecX, endRecY, mPaint);
                        //记录第几行，即第几周
                        weekRow = row + 1;
                    }
                    //绘制事务圆形标志
                    //if (!isPreviousDone) DrawCircle(row, column, year, month, day + 1, canvas);
                    if (dayString.Equals(mSelDay + "") && !isPreviousDone)
                    {

                        mSelectDayColor.SetFill();
                    }
                    //else if (dayString.Equals(mCurrDay + "") && mCurrDay != mSelDay && mCurrMonth == this.Element.Date.Month && mCurrYear == this.Element.Date.Year && !isPreviousDone)
                    //{
                    //    //正常月，选中其他日期，则今日为红色
                    //    mCurrentColor.SetFill();
                    //}
                    else if (!isPreviousDone)
                    {
                        mDayColor.SetFill();
                    }
                    new NSString(dayString).DrawString(new CGPoint(startX, startY),font);
                    //canvas.DrawText(dayString, startX, startY, mPaint);

                    continue;
                }

                column = 0;
            }
            #endregion
            //canvas.Restore();//.restore();
        }

        public override void TouchesEnded(NSSet touches, UIEvent evt)
        {
            CGPoint point = (evt.AllTouches.AnyObject as UITouch).LocationInView(this);
            Console.WriteLine(point.X + "," + point.Y);
            //this.SetNeedsDisplay();
            base.TouchesEnded(touches, evt);
        }

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
    }

    public class MyUIScrollView : UIScrollView
    {
        Action<NSSet, UIEvent> MyTouchesEnded;

        public override void TouchesEnded(NSSet touches, UIEvent evt)
        {
            if (MyTouchesEnded != null) MyTouchesEnded.Invoke(touches, evt);
            base.TouchesEnded(touches, evt);
        }
    }
}