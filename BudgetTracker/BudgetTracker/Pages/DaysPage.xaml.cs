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
        private Label monthlyConsumptions = new Label();
        private Month pageMonth;

        public DaysPage(Month month)
        {
            pageMonth = month;

            monthlyConsumptions.Text = GetMonthlyConsumption(pageMonth).ToString();

            List<Day> days = App.GetDataBase().GetDays(pageMonth).ToList();

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
                if (e.SelectedItem != null)
                {
                    var consumptionsPage = new ConsumptionsPage(e.SelectedItem as Day);
                    daysListView.SelectedItem = null;
                    Navigation.PushAsync(consumptionsPage);
                }
            };
        }

        public double GetMonthlyConsumption(Month month)
        {
            DataBase db = App.GetDataBase();
            IEnumerable<Day> days = db.GetDays(month);
            return days.Sum(d => db.GetConsumptions(d).Sum(c => c.ConsumptionPrice));
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            monthlyConsumptions.Text = GetMonthlyConsumption(pageMonth).ToString();
        }
    }
}