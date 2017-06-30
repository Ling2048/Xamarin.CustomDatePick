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

            dp.monthDateView.Click = (year, month, day) => {
                this.DisplayAlert("标题", "点击了:" + year + "年" + month + "月" + day + "日", "取消");
            };
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            dp.Date = DateTime.Now.AddYears(-1);
        }
    }
}
