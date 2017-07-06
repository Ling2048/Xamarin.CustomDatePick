using MyDataPick.Converters;
using MyDataPick.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace MyDataPick.Views
{
    public class MyDataPick : ContentView
    {
        public static readonly BindableProperty CommandProperty = BindableProperty.Create("Command", typeof(ICommand), typeof(MyDataPick), null, propertyChanged: (bo, o, n) => ((MyDataPick)bo).OnCommandChanged());

        public static readonly BindableProperty CommandParameterProperty = BindableProperty.Create("CommandParameter", typeof(object), typeof(MyDataPick), null,
            propertyChanged: (bindable, oldvalue, newvalue) => ((MyDataPick)bindable).CommandCanExecuteChanged(bindable, EventArgs.Empty));

        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        public object CommandParameter
        {
            get { return GetValue(CommandParameterProperty); }
            set { SetValue(CommandParameterProperty, value); }
        }

        public static readonly BindableProperty DateProperty = BindableProperty.Create(
                    propertyName: "Date",
                    returnType: typeof(DateTime),
                    declaringType: typeof(MyDataPick),
                    defaultValue: DateTime.Now);

        void OnCommandChanged()
        {
            if (Command != null)
            {
                Command.CanExecuteChanged += CommandCanExecuteChanged;
                CommandCanExecuteChanged(this, EventArgs.Empty);
            }
        }

        void CommandCanExecuteChanged(object sender, EventArgs eventArgs)
        {
            ICommand cmd = Command;
            if (cmd != null) cmd.CanExecute(CommandParameter);
        }

        public DateTime Date
        {
            get { return (DateTime)GetValue(DateProperty); }
            set { SetValue(DateProperty, value); }
        }

        public static readonly BindableProperty ThingProperty = BindableProperty.Create(
           propertyName: "Thing",
           returnType: typeof(List<ThingModel>),
           declaringType: typeof(MonthDateView),propertyChanged: (a,b,c) => { },
           defaultValue:  null);

        public List<ThingModel> Thing
        {
            get { return (List<ThingModel>)GetValue(ThingProperty); }
            set { SetValue(ThingProperty, value); }
        }

        public static readonly BindableProperty ClickProperty = BindableProperty.Create(
            propertyName: "Click",
            returnType: typeof(Action<int, int, int>),
            declaringType: typeof(MonthDateView),
            defaultValue: null);

        public Action<int, int, int> Click
        {
            get { return (Action<int, int, int>)GetValue(ClickProperty); }
            set { SetValue(ClickProperty, value); }
        }

        private WeekDayView weekDayView;
        ScrollMonthDateViewEx scrollMonthDateViewEx;

        public MyDataPick() { }

        protected override void OnPropertyChanging(string propertyName = null)
        {
            if (propertyName == CommandProperty.PropertyName)
            {
                ICommand cmd = Command;
                if (cmd != null)
                    cmd.CanExecuteChanged -= CommandCanExecuteChanged;
            }
            base.OnPropertyChanging(propertyName);
        }

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

                    weekDayView = new WeekDayView() { BackgroundColor = bg, HeightRequest = unit };
                    scrollMonthDateViewEx = new ScrollMonthDateViewEx() { BackgroundColor = bg, HeightRequest = (this.HeightRequest - unit * 2), Date = now };

                    //上下月按钮与标题
                    Button previous = new Button() { Text = "＜", BackgroundColor = btnBg, TextColor = btnText, BorderRadius = 0, WidthRequest = unit, HorizontalOptions = LayoutOptions.Start };
                    Button next = new Button() { Text = "＞", BackgroundColor = btnBg, TextColor = btnText, BorderRadius = 0, WidthRequest = unit, HorizontalOptions = LayoutOptions.End };
                    Label year = new Label() { TextColor = Color.Black, HorizontalOptions = LayoutOptions.CenterAndExpand, VerticalOptions = LayoutOptions.Center };

                    Binding binding = new Binding();
                    binding.Source = scrollMonthDateViewEx;
                    binding.Path = "Date";
                    binding.Converter = new Date2StringConverter();
                    binding.Mode = BindingMode.OneWay;
                    year.SetBinding(Label.TextProperty, binding);

                    previous.Clicked += (sender,e) => {
                        scrollMonthDateViewEx.Date = scrollMonthDateViewEx.Date.AddMonths(-1);
                    };

                    next.Clicked += (sender, e) => {
                        scrollMonthDateViewEx.Date = scrollMonthDateViewEx.Date.AddMonths(1);
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

                    //组装
                    Content = new StackLayout()
                    {
                        Orientation = StackOrientation.Vertical,
                        Children = { yearView, weekDayView, scrollMonthDateViewEx },
                        Spacing = 0,
                    };
                }
            }
            else if (propertyName.Equals("Date"))
            {
                scrollMonthDateViewEx.Date = this.Date;
            }
            else if (propertyName.Equals("Thing"))
            {
                scrollMonthDateViewEx.Thing = this.Thing;
            }
            else if (propertyName.Equals("Click"))
            {
                scrollMonthDateViewEx.Click = this.Click;
            }
        }

        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);
        }

    }
}
