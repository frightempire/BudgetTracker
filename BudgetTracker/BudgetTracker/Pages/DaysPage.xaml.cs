using BudgetTracker.DataBaseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BudgetTracker.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DaysPage : ContentPage
    {
        public DaysPage()
        {
            IEnumerable<Day> days = App.GetDataBase().GetDays(BindingContext as Month);
            DaysListView.ItemsSource = days;

            var dayInfoBox = new DataTemplate(typeof(TextCell));
            dayInfoBox.SetBinding(TextCell.TextProperty, new Binding("DayDate", stringFormat: "0:MMMM d"));
            dayInfoBox.SetValue(TextCell.DetailProperty, GetDailyComsumptions(BindingContext as Day));

            DaysListView.ItemTemplate = dayInfoBox;
        }

        public double GetDailyComsumptions(Day day)
        {
            DataBase db = App.GetDataBase();
            return db.GetConsumptions(day).Sum(c => c.ConsumptionPrice);
        }

        async private void DaysListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var consumptionsPage = new ConsumptionsPage();
            consumptionsPage.BindingContext = e.Item;
            await Navigation.PushAsync(consumptionsPage);
        }
    }
}