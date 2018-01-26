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
        private Label personalMonthlyConsumptions = new Label();
        private Label cooperativeMonthlyConsumptions = new Label();
        private Month pageMonth;

        public DaysPage(Month month)
        {
            pageMonth = month;
            SetConsumptions();

            List<Day> days = App.GetDataBase().GetDays(pageMonth).ToList();

            var dayInfoBox = new DataTemplate(typeof(TextCell));
            dayInfoBox.SetBinding(TextCell.TextProperty, new Binding("DayDate", stringFormat: "{0:M}"));

            ListView daysListView = new ListView
            {
                ItemsSource = days,
                ItemTemplate = dayInfoBox
            };

            Content = new StackLayout
            {
                Children =
                {
                    personalMonthlyConsumptions,
                    cooperativeMonthlyConsumptions,
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

        public void SetConsumptions()
        {
            double personalConsumptions = GetMonthlyConsumptions(pageMonth, false);
            double cooperativeConsumptions = GetMonthlyConsumptions(pageMonth, true);
            double totalConsumptions = personalConsumptions + cooperativeConsumptions;
            personalMonthlyConsumptions.Text = personalConsumptions.ToString() + " + ";
            cooperativeMonthlyConsumptions.Text = cooperativeConsumptions.ToString() + " = ";
            monthlyConsumptions.Text = totalConsumptions.ToString();
        }

        public double GetMonthlyConsumptions(Month month, bool coop)
        {
            DataBase db = App.GetDataBase();
            IEnumerable<Day> days = db.GetDays(month);
            return days.Sum(d => db.GetConsumptions(d).Where(c => c.CooperationalConsumption == coop).Sum(c => c.ConsumptionPrice));
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            SetConsumptions();
        }
    }
}