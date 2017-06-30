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
using Android.Graphics;
using Android.Util;

namespace MyDataPick.Droid.Handler
{
    public class MyDataPick : View
    {
        ////上横线颜色
        //private Android.Graphics.Color mTopLineColor = Android.Graphics.Color.ParseColor("#CCE4F2");
        ////下横线颜色
        //private Android.Graphics.Color mBottomLineColor = Android.Graphics.Color.ParseColor("#CCE4F2");
        ////周一到周五的颜色
        //private Android.Graphics.Color mWeedayColor = Android.Graphics.Color.ParseColor("#1FC2F3");
        ////周六、周日的颜色
        //private Android.Graphics.Color mWeekendColor = Android.Graphics.Color.ParseColor("#fa4451");
        ////线的宽度
        //private int mStrokeWidth = 4;
        //private int mWeekSize = 14;
        //private Paint paint;
        //private DisplayMetrics mDisplayMetrics;
        //private String[] weekString = new String[] { "日", "一", "二", "三", "四", "五", "六" };
        public MyDataPick(Context context)
			: base (context)
		{
            Log.WriteLine(LogPriority.Info, "Test", "MyDataPick");
            //surfaceView = new SurfaceView(context);
            //AddView(surfaceView);

            //windowManager = Context.GetSystemService(Context.WindowService).JavaCast<IWindowManager>();

            //IsPreviewing = false;
            //holder = surfaceView.Holder;
            //holder.AddCallback(this);
            //paint = new Paint();
        }

        protected override void OnLayout(bool changed, int l, int t, int r, int b)
        {
            Log.WriteLine(LogPriority.Info, "Test", "OnLayout");
            //throw new NotImplementedException();
        }
    }
}