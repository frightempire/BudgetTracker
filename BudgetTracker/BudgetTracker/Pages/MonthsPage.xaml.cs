using BudgetTracker.DataBaseModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
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
            var monthInfoBox = new DataTemplate(typeof(MonthCustomCell));

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

    public class MonthCustomCell: ViewCell
    {
        public MonthCustomCell()
        {
            Label cellLabel = new Label
            {
                FontSize = 30,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                HorizontalTextAlignment = TextAlignment.Center,
            };
            cellLabel.SetBinding(Label.TextProperty, new Binding("MonthDate", stringFormat: "{0:Y}"));
            cellLabel.SetBinding(Label.BackgroundColorProperty, new Binding("MonthDate") { Converter = new MonthDateToColorConverter()});

            View = cellLabel;
        }
    }

    public class MonthDateToColorConverter: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            DateTime date = (DateTime)value;
            switch (date.Month)
            {
                case 1:
                case 2:
                case 12:
                    return Color.DeepSkyBlue;
                case 3:
                case 4:
                case 5:
                    return Color.SpringGreen;
                case 6:
                case 7:
                case 8:
                    return Color.IndianRed;
                case 9:
                case 10:
                case 11:
                    return Color.LightGoldenrodYellow;
                default:
                    return Color.White;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new Exception("Functionality not supported");
        }
    }
}