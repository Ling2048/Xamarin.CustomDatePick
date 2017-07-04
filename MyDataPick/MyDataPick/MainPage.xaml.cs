using MyDataPick.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MyDataPick
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            List<ThingModel> thing = new List<ThingModel>() {
                new ThingModel(){ Date = new DateTime(2017,7,10), IsDone = true},
                new ThingModel(){ Date = new DateTime(2017,7,11), IsDone = false},
                new ThingModel(){ Date = new DateTime(2017,7,12), IsDone = true},
                new ThingModel(){ Date = new DateTime(2017,7,13), IsDone = false},
                new ThingModel(){ Date = new DateTime(2017,7,14), IsDone = true},
                new ThingModel(){ Date = new DateTime(2017,7,15), IsDone = false},
                new ThingModel(){ Date = new DateTime(2017,7,16), IsDone = true},
                new ThingModel(){ Date = new DateTime(2017,7,17), IsDone = false},
                new ThingModel(){ Date = new DateTime(2017,7,18), IsDone = true},
                new ThingModel(){ Date = new DateTime(2017,7,19), IsDone = true},
            };
            Binding binding = new Binding();
            binding.Source = thing;
            binding.Mode = BindingMode.OneWay;
            dp.SetBinding(MyDataPick.Views.MyDataPick.ThingProperty, binding);
            //dp.Thing = thing;

            DateTime ClickTime = DateTime.MinValue;

            dp.Click = (year, month, day) => {

                if (ClickTime.CompareTo(DateTime.MinValue) == 0)
                {
                    Task.Run(async () => {
                        ClickTime = new DateTime(year, month, day);
                        await Task.Delay(500);
                        ClickTime = DateTime.MinValue;
                    });
                }
                else
                {
                    DateTime date = new DateTime(year, month, day);

                    if (ClickTime.CompareTo(new DateTime(year, month, day)) != 0) return;

                    var models = from model in thing
                                 where model.Date == date
                                 select model;

                    if (models.Count() > 0)
                    {
                        thing.RemoveAt(thing.IndexOf(models.First()));
                    }
                    else
                    {
                        
                        thing.Insert(thing.Count, new ThingModel() {  Date = ClickTime, IsDone = false});
                    }
                    ClickTime = DateTime.MinValue;
                }

                //this.DisplayAlert("标题", "点击了:" + year + "年" + month + "月" + day + "日", "取消");
            };
        }
    }
}
