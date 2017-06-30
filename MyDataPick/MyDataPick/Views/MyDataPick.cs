using MyDataPick.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MyDataPick.Views
{
    public class MyDataPick : ContentView
    {

        public static readonly BindableProperty DateProperty = BindableProperty.Create(
                    propertyName: "Date",
                    returnType: typeof(DateTime),
                    declaringType: typeof(MyDataPick),
                    defaultValue: DateTime.Now);

        public DateTime Date
        {
            get { return (DateTime)GetValue(DateProperty); }
            set { SetValue(DateProperty, value); }
        }

        public WeekDayView weekDayView;
        public MonthDateView monthDateView;

        public MyDataPick() { }

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            if (propertyName.Equals("HeightRequest"))
            {
                if (Content == null)
                {
                    double unit = this.HeightRequest / 7;
                    Color bg = this.BackgroundColor;
                    Color btnBg = Color.FromHex("1FC2F3");
                    Color btnText = new Color(243, 254, 252);
                    DateTime now = DateTime.Now;
                    List<DateTime> thing = new List<DateTime>() {
                        new DateTime(2017,6,10),
                        new DateTime(2017,6,11),
                        new DateTime(2017,6,12),
                        new DateTime(2017,6,13),
                        new DateTime(2017,6,14),
                        new DateTime(2017,6,15),
                        new DateTime(2017,6,16),
                        new DateTime(2017,6,17),
                        new DateTime(2017,6,18),
                        new DateTime(2017,6,19),
                        new DateTime(2017,6,20),
                    };

                    weekDayView = new WeekDayView() { BackgroundColor = bg, HeightRequest = unit };
                    monthDateView = new MonthDateView() { BackgroundColor = bg, HeightRequest = (this.HeightRequest - unit * 2), Date = now, Thing = thing };

                    Button previous = new Button() { Text = "＜", BackgroundColor = btnBg, TextColor = btnText, BorderRadius = 0, WidthRequest = unit, HorizontalOptions = LayoutOptions.Start };
                    Button next = new Button() { Text = "＞", BackgroundColor = btnBg, TextColor = btnText, BorderRadius = 0, WidthRequest = unit, HorizontalOptions = LayoutOptions.End };
                    Label year = new Label() { TextColor = Color.Black, HorizontalOptions = LayoutOptions.CenterAndExpand, VerticalOptions = LayoutOptions.Center };

                    Binding binding = new Binding();
                    binding.Source = monthDateView;
                    binding.Path = "Date";
                    binding.Converter = new Date2StringConverter();
                    binding.Mode = BindingMode.OneWay;

                    year.SetBinding(Label.TextProperty, binding);


                    previous.Clicked += (sender,e) => {
                        monthDateView.Date = monthDateView.Date.AddMonths(-1);
                        //year.Text = monthDateView.Date.ToString("yyyy年 MM月");
                    };

                    next.Clicked += (sender, e) => {
                        monthDateView.Date = monthDateView.Date.AddMonths(1);
                        //year.Text = monthDateView.Date.ToString("yyyy年 MM月");
                    };

                    StackLayout yearView = new StackLayout()
                    {
                        Orientation = StackOrientation.Horizontal,
                        Children =
                        {
                            previous,
                            year,
                            next
                        },
                        BackgroundColor = bg,
                        Spacing = 0,
                        HeightRequest = unit
                    };

                    Content = new StackLayout()
                    {
                        Orientation = StackOrientation.Vertical,
                        Children = { yearView,weekDayView, monthDateView },
                        Spacing = 0,
                    };
                }
            }
            else if (propertyName.Equals("Date"))
            {
                monthDateView.Date = this.Date;
            }
        }

        protected override void OnChildAdded(Element child)
        {
            base.OnChildAdded(child);
        }
    }
}
