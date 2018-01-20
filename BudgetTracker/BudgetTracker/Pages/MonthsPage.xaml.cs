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
    public partial class MonthsPage : ContentPage
    {
        public MonthsPage()
        {
            IEnumerable<Month> months = App.GetDataBase().GetMonths();
            MonthsListView.ItemsSource = months;

            var monthInfoBox = new DataTemplate(typeof(TextCell));
            monthInfoBox.SetBinding(TextCell.TextProperty, new Binding("MonthDate", stringFormat:"{0:D}"));
            monthInfoBox.SetValue(TextCell.DetailProperty, GetMonthlyConsumption(BindingContext as Month));

            MonthsListView.ItemTemplate = monthInfoBox;
        }

        public double GetMonthlyConsumption(Month month)
        {
            DataBase db = App.GetDataBase();
            IEnumerable<Day> days = db.GetDays(month);
            return days.Sum(d => db.GetConsumptions(d).Sum(c => c.ConsumptionPrice));
        }

        async private void MonthsListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var daysPage = new DaysPage();
            daysPage.BindingContext = e.Item;
            await Navigation.PushAsync(daysPage);
        }
    }
}