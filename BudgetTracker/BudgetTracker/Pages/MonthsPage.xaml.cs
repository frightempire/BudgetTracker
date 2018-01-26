using BudgetTracker.DataBaseModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            AdjustDataBase();

            List<Month> months = App.GetDataBase().GetMonths().ToList();

            var monthInfoBox = new DataTemplate(typeof(TextCell));
            monthInfoBox.SetBinding(TextCell.TextProperty, new Binding("MonthDate", stringFormat: "{0:Y}"));

            ListView monthsListView = new ListView
            {
                ItemsSource = months,
                ItemTemplate = monthInfoBox
            };

            Content = new ScrollView
            {
                Content = new StackLayout
                {
                    Children =
                    {
                        monthsListView
                    }
                }
            };

            monthsListView.ItemSelected += (o, e) => {
                if (e.SelectedItem != null)
                {
                    var daysPage = new DaysPage(e.SelectedItem as Month);
                    monthsListView.SelectedItem = null;
                    Navigation.PushAsync(daysPage);
                }
            };
        }

        // OnStart lifecycle method not working
        public void AdjustDataBase()
        {
            DataBase db = App.GetDataBase();

            IEnumerable<Month> months = db.GetMonths();
            if (months.Count() == 0 || months.OrderBy(m => m.MonthDate).Last().MonthDate.Month != DateTime.Today.Month)
                db.AddMonth();

            Month lastMonth = months.OrderBy(m => m.MonthDate).Last();

            IEnumerable<Day> days = db.GetDays(lastMonth);
            if (days.Count() == 0 || days.OrderBy(d => d.DayDate).Last().DayDate.Day != DateTime.Today.Day)
                db.AddDay(lastMonth);
        }
    }
}