using BudgetTracker.DataBaseModels;
using System;
using System.Collections.Generic;
using System.Globalization;
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
        private Label monthlyConsumptions = new Label
        {
            FontSize = 25,
            HorizontalOptions = LayoutOptions.CenterAndExpand
        };

        private Label personalMonthlyConsumptions = new Label()
        {
            FontSize = 25,
            HorizontalOptions = LayoutOptions.CenterAndExpand
        };

        private Label cooperativeMonthlyConsumptions = new Label()
        {
            FontSize = 25,
            HorizontalOptions = LayoutOptions.CenterAndExpand
        };

        private Month pageMonth;

        public DaysPage(Month month)
        {
            pageMonth = month;
            SetConsumptions();

            List<Day> days = App.GetDataBase().GetDays(pageMonth).ToList();
            var dayInfoBox = new DataTemplate(typeof(DayCustomCell));

            ListView daysListView = new ListView
            {
                ItemsSource = days,
                ItemTemplate = dayInfoBox
            };

            // Top labels
            Label plus = new Label
            {
                Text = "+",
                FontSize = 25,
                HorizontalOptions = LayoutOptions.CenterAndExpand
            };

            StackLayout consFirstRowLabels = new StackLayout
            {
                Children =
                {
                    personalMonthlyConsumptions,
                    plus,
                    cooperativeMonthlyConsumptions
                },
                Orientation = StackOrientation.Horizontal
            };

            StackLayout topLabels = new StackLayout
            {
                Children =
                {
                    consFirstRowLabels,
                    monthlyConsumptions
                }
            };

            // All page
            Content = new StackLayout
            {
                Children =
                {
                    topLabels,
                    daysListView
                }
            };

            Title = "Дни";

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
            personalMonthlyConsumptions.Text = personalConsumptions.ToString();
            cooperativeMonthlyConsumptions.Text = cooperativeConsumptions.ToString();
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

    public class DayCustomCell: ViewCell
    {
        public DayCustomCell()
        {
            Label cellLabel = new Label
            {
                FontSize = 30,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                HorizontalTextAlignment = TextAlignment.Center,
            };
            cellLabel.SetBinding(Label.TextProperty, new Binding("DayDate", stringFormat: "{0:M}"));
            cellLabel.SetBinding(Label.BackgroundColorProperty, new Binding("DayDate") { Converter = new DayDateToColorConverter() });

            View = cellLabel;
        }
    }

    public class DayDateToColorConverter: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            DateTime date = (DateTime)value;
            switch(date.DayOfWeek)
            {
                case DayOfWeek.Saturday:
                case DayOfWeek.Sunday:
                    return Color.DeepSkyBlue;
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