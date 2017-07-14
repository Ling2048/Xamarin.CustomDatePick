using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;
using MyDataPick.iOS.Renderers;
using Xamarin.Forms;
using CoreGraphics;
using Xamarin.Forms.Platform.iOS;
using System.Drawing;

[assembly: ExportRenderer(typeof(MyDataPick.Views.WeekDayView), typeof(WeekDayViewRenderer))]
namespace MyDataPick.iOS.Renderers
{
    public class WeekDayViewRenderer : ViewRenderer<MyDataPick.Views.WeekDayView, UIView>
    {

        private NSString[] weekString = new NSString[] { new NSString("日"), new NSString("一"), new NSString("二"), new NSString("三"), new NSString("四"), new NSString("五"), new NSString("六") };

        public WeekDayViewRenderer()
        {
            //this.Frame = new CoreGraphics.CGRect(
            //this.PreservesSuperviewLayoutMargins = true;
            //this.LayoutMargins = new UIEdgeInsets(0, 0, 100, 0);
            //this.AutoresizingMask = UIViewAutoresizing.FlexibleWidth;
            //this.Init();
        }

        public override SizeRequest GetDesiredSize(double widthConstraint, double heightConstraint)
        {
            this.Frame = new CGRect(0, 0, widthConstraint, heightConstraint);
            this.Init();

            //UICollectionViewFlowLayout weekDayColectionLayout = new UICollectionViewFlowLayout();
            //weekDayColectionLayout.ItemSize = new CoreGraphics.CGSize(130, 130);// CGSizeMake(130, 130);
            //weekDayColectionLayout.ScrollDirection = UICollectionViewScrollDirection.Horizontal;
            //weekDayColectionLayout.MinimumLineSpacing = 0;

            //CGRect frame = new CoreGraphics.CGRect(0, 0, widthConstraint, heightConstraint);

            //UICollectionView weekDayCollection = new UICollectionView(frame, weekDayColectionLayout);

            ////weekDayCollection.SetCollectionViewLayout(weekDayColectionLayout, true);

            //weekDayCollection.BackgroundColor = UIColor.Blue;
            //weekDayCollection.ScrollsToTop = false;
            //weekDayCollection.ShowsHorizontalScrollIndicator = false;
            //weekDayCollection.ShowsVerticalScrollIndicator = false;

            //weekDayCollection.RegisterClassForCell(typeof(WeekCell), WeekCell.CellID);
            //WeekDataSource dataSource = new WeekDataSource();
            //weekDayCollection.Source = new WeekDataSource();
            ////weekDayCollection.RegisterClassForCell
            //weekDayCollection.ReloadData();



            return base.GetDesiredSize(widthConstraint, heightConstraint);
        }
        //public override UIViewAutoresizing AutoresizingMask { get => base.AutoresizingMask; set => base.AutoresizingMask = value; }
        private float mWeekSize = 20f;
        private int mStrokeWidth = 2;
        UIColor strokeColor = UIColor.FromRGB(204, 228, 242);
        UIColor mWeedayColor = UIColor.FromRGB(31, 194, 243);
        UIColor mWeekendColor = UIColor.FromRGB(250, 68, 81);

        public override void Draw(CGRect rect)
        {
            base.Draw(rect);

            Console.WriteLine("Draw");
            //UIFont font = UIFont.BoldSystemFontOfSize(18f);
            //UIColor.Blue.SetFill();
            //NSString str = new NSString("Test");
            //str.DrawString(this.Bounds, font);

            //进行画上下线

            float width = (float)rect.Width;
            float height = (float)rect.Height;
            float bottom = (float)rect.Bottom;
            strokeColor.SetFill();
            var path = new UIBezierPath();
            path.MoveTo(new PointF(0, 0));
            path.AddLineTo(new PointF(width, 0));
            path.AddLineTo(new PointF(width, mStrokeWidth));
            path.AddLineTo(new PointF(0, mStrokeWidth));
            path.AddLineTo(new PointF(0, 0));
            path.Fill();

            //画下横线
            path.MoveTo(new PointF(0, height - mStrokeWidth));
            path.AddLineTo(new PointF(width, height - mStrokeWidth));
            path.AddLineTo(new PointF(width, height));
            path.AddLineTo(new PointF(0, height));
            path.AddLineTo(new PointF(0, height - mStrokeWidth));
            path.Fill();

            UIFont font = UIFont.BoldSystemFontOfSize(mWeekSize);
            //paint.TextSize = mWeekSize;// * mDisplayMetrics.ScaledDensity;
            float columnWidth = width / 7;
            for (int i = 0; i < weekString.Length; i++)
            {
                NSString text = weekString[i];
                
                float fontWidth = (float)text.StringSize(font).Width;
                float startX = columnWidth * i + (columnWidth - fontWidth) / 2;
                int startY = (int)(height / 2 - ((float)text.StringSize(font).Height) / 2);
                string str = text.ToString();
                if (text.ToString().IndexOf("日") > -1 || text.ToString().IndexOf("六") > -1)
                {
                    mWeekendColor.SetFill();
                    //paint.Color = mWeekendColor;
                }
                else
                {
                    mWeedayColor.SetFill();
                    //paint.Color = mWeedayColor;
                }
                text.DrawString(new CGPoint(startX, startY), font);
                //canvas.DrawText(text, startX, startY, paint);
            }

            //CGContext contenxt = UIGraphics.GetCurrentContext();
            //Console.WriteLine("Draw");
            //UIGraphics.BeginImageContext(new CGSize(rect.Width, rect.Height));
            // UIGraphics.GetImageFromCurrentImageContext();
            //UIGraphics.EndImageContext();
            //UIImageView imageView = new UIImageView(UIGraphics.GetImageFromCurrentImageContext());
            //this.AddSubview(imageView);
            //Console.WriteLine("Init");

            //const float inset = 15f;
            //float width = (float)rect.Width;
            //float height = (float)rect.Height;
            //float bottom = (float)rect.Bottom;
            //UIColor.Blue.SetFill();
            //var path = new UIBezierPath();
            //path.MoveTo(new PointF(0, 0));
            //path.AddLineTo(new PointF(width - inset, 0));
            //path.AddLineTo(new PointF(width, height * 0.5f));
            //path.AddLineTo(new PointF(width - inset, bottom));
            //path.AddLineTo(new PointF(0, bottom));
            //path.AddLineTo(new PointF(0, 0));
            //path.Fill();
        }

    }

    public class WeekDataSource : UICollectionViewSource
    {
        public WeekDataSource()
        {
            Rows = new List<WeekElement>() {
                new WeekElement("日"),
                new WeekElement("一"),
                new WeekElement("二"),
                new WeekElement("三"),
                new WeekElement("四"),
                new WeekElement("五"),
                new WeekElement("六"),
            };
        }

        public List<WeekElement> Rows { get; private set; }

        public Single FontSize { get; set; }

        public SizeF ImageViewSize { get; set; }

        //public override Int32 NumberOfSections(UICollectionView collectionView)
        //{
        //    return 1;
        //}

        //override unm

        public override nint GetItemsCount(UICollectionView collectionView, nint section)
        {
            return Rows.Count;
        }

        public override Boolean ShouldHighlightItem(UICollectionView collectionView, NSIndexPath indexPath)
        {
            return true;
        }

        public override void ItemHighlighted(UICollectionView collectionView, NSIndexPath indexPath)
        {
            //var cell = (WeekCell)collectionView.CellForItem(indexPath);
            //cell.ImageView.Alpha = 0.5f;
        }

        public override void ItemUnhighlighted(UICollectionView collectionView, NSIndexPath indexPath)
        {
            //var cell = (WeekCell)collectionView.CellForItem(indexPath);
            //cell.ImageView.Alpha = 1;

            //WeekElement row = Rows[indexPath.Row];
            //row.Tapped.Invoke();
        }

        public override UICollectionViewCell GetCell(UICollectionView collectionView, NSIndexPath indexPath)
        {
            var cell = (WeekCell)collectionView.DequeueReusableCell(WeekCell.CellID, indexPath);

            WeekElement row = Rows[indexPath.Row];

            cell.UpdateRow(row, 20);

            return cell;
        }
    }

    public class WeekElement
    {
        public WeekElement(String week)
        {
            Week = week;
        }

        public String Week { get; set; }
    }

    public class WeekCell : UICollectionViewCell
    {
        public static NSString CellID = new NSString("WeekDataSource");

        [Export("initWithFrame:")]
        public WeekCell(RectangleF frame)
            : base(frame)
        {
            //ImageView = new UIImageView();
            //ImageView.Layer.BorderColor = UIColor.DarkGray.CGColor;
            //ImageView.Layer.BorderWidth = 1f;
            //ImageView.Layer.CornerRadius = 3f;
            //ImageView.Layer.MasksToBounds = true;
            //ImageView.ContentMode = UIViewContentMode.ScaleToFill;

            //ContentView.AddSubview(ImageView);

            LabelView = new UILabel();
            LabelView.BackgroundColor = UIColor.Clear;
            LabelView.TextColor = UIColor.Black;
            LabelView.TextAlignment = UITextAlignment.Center;

            ContentView.AddSubview(LabelView);
        }

        //public UIImageView ImageView { get; private set; }

        public UILabel LabelView { get; private set; }

        public void UpdateRow(WeekElement element, Single fontSize)
        {
            LabelView.Text = element.Week;

            LabelView.Font = UIFont.FromName("HelveticaNeue-Bold", fontSize);

        }
    }
}