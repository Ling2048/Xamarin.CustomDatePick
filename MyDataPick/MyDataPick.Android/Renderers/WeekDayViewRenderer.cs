using System;
using Android.Hardware;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using MyDataPick.Droid.Renderers;
using Android.Util;
using Android.Graphics;
using Android.Views;
using MyDataPick.Views;

[assembly: ExportRenderer(typeof(MyDataPick.Views.WeekDayView), typeof(WeekDayViewRenderer))]
namespace MyDataPick.Droid.Renderers
{
    public class WeekDayViewRenderer : ViewRenderer<MyDataPick.Views.WeekDayView, Android.Views.View>
    {

        //上横线颜色
        private Android.Graphics.Color mTopLineColor = Android.Graphics.Color.ParseColor("#CCE4F2");
        //下横线颜色
        private Android.Graphics.Color mBottomLineColor = Android.Graphics.Color.ParseColor("#CCE4F2");
        //周一到周五的颜色
        private Android.Graphics.Color mWeedayColor = Android.Graphics.Color.ParseColor("#1FC2F3");
        //周六、周日的颜色
        private Android.Graphics.Color mWeekendColor = Android.Graphics.Color.ParseColor("#fa4451");
        //线的宽度
        private int mStrokeWidth = 4;
        private int mWeekSize = 14;
        private Paint paint;
        private DisplayMetrics mDisplayMetrics;
        private String[] weekString = new String[] { "日", "一", "二", "三", "四", "五", "六" };

        protected override void OnAttachedToWindow()
        {
            base.OnAttachedToWindow();
            mDisplayMetrics = Resources.DisplayMetrics;// getResources().getDisplayMetrics();
            paint = new Paint(PaintFlags.AntiAlias);
        }

        protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec)
        {

            int widthSize = MeasureSpec.GetSize(widthMeasureSpec);
            MeasureSpecMode widthMode = MeasureSpec.GetMode(widthMeasureSpec);

            int heightSize = MeasureSpec.GetSize(heightMeasureSpec);
            MeasureSpecMode heightMode = MeasureSpec.GetMode(heightMeasureSpec);

            if (heightMode == MeasureSpecMode.AtMost)
            {
                heightSize = Convert.ToInt32(mDisplayMetrics.DensityDpi) * 30;
            }
            if (widthMode == MeasureSpecMode.AtMost)
            {
                widthSize = Convert.ToInt32(mDisplayMetrics.DensityDpi) * 300;
            }
            SetMeasuredDimension(widthSize, heightSize);
        }

        //public override void Draw(Canvas canvas)
        //{
        //    Log.WriteLine(LogPriority.Info, "Test", "Draw");
        //    //base.Draw(canvas);
        //}

        protected override void OnDraw(Canvas canvas)
        {

            int width = this.Width;// // getWidth();
            int height = this.Height;// getHeight();
            //进行画上下线
            paint.SetStyle(Paint.Style.Stroke);
            paint.Color = mTopLineColor;
            paint.StrokeWidth = mStrokeWidth;//.setStrokeWidth(mStrokeWidth);
            canvas.DrawLine(0, 0, width, 0, paint);

            //画下横线
            paint.Color = mBottomLineColor;//SetColor(mBottomLineColor);
            canvas.DrawLine(0, height, width, height, paint);
            paint.SetStyle(Paint.Style.Fill);
            paint.TextSize = mWeekSize * mDisplayMetrics.ScaledDensity;
            int columnWidth = width / 7;
            for (int i = 0; i < weekString.Length; i++)
            {
                String text = weekString[i];
                int fontWidth = (int)paint.MeasureText(text);
                int startX = columnWidth * i + (columnWidth - fontWidth) / 2;
                int startY = (int)(height / 2 - (paint.Ascent() + paint.Descent()) / 2);
                if (text.IndexOf("日") > -1 || text.IndexOf("六") > -1)
                {
                    paint.Color = mWeekendColor;
                }
                else
                {
                    paint.Color = mWeedayColor;
                }
                canvas.DrawText(text, startX, startY, paint);
            }
        }

    }
}