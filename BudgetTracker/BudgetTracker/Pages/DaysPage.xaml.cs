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
        public DaysPage(Month month)
        {
            Label monthlyConsumptions = new Label();
            monthlyConsumptions.Text = GetMonthlyConsumption(month).ToString();

            IEnumerable<Day> days = App.GetDataBase().GetDays(month);

            var dayInfoBox = new DataTemplate(typeof(TextCell));
            dayInfoBox.SetBinding(TextCell.TextProperty, new Binding("DayDate", stringFormat: "{0:MMMM d}"));

            ListView daysListView = new ListView
            {
                ItemsSource = days,
                ItemTemplate = dayInfoBox
            };

            Content = new StackLayout
            {
                Children =
                {
                    monthlyConsumptions,
                    daysListView
                }
            };

            daysListView.ItemSelected += (o, e) =>
            {
                var consumptionsPage = new ConsumptionsPage(e.SelectedItem as Day);
                Navigation.PushAsync(consumptionsPage);
            };
        }

        public double GetMonthlyConsumption(Month month)
        {
            DataBase db = App.GetDataBase();
            IEnumerable<Day> days = db.GetDays(month);
            return days.Sum(d => db.GetConsumptions(d).Sum(c => c.ConsumptionPrice));
        }
    }
}