using MyDataPick.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MyDataPick.Views
{
    public class ScrollMonthDateViewEx : View, System.ComponentModel.INotifyPropertyChanged
    {
        public static readonly BindableProperty DateProperty = BindableProperty.Create(
            propertyName: "Date",
            returnType: typeof(DateTime),
            declaringType: typeof(MonthDateView),
            defaultValue: DateTime.Now);

        public DateTime Date
        {
            get { return (DateTime)GetValue(DateProperty); }
            set { SetValue(DateProperty, value); }
        }

        public static readonly BindableProperty ThingProperty = BindableProperty.Create(
            propertyName: "Thing",
            returnType: typeof(List<ThingModel>),
            declaringType: typeof(MonthDateView),
            defaultValue: null);

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
    }
}
