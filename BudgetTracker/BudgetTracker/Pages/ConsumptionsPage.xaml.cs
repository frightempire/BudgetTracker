using BudgetTracker.DataBaseModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BudgetTracker.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ConsumptionsPage : ContentPage
    {
        private Label dailyComsumptions = new Label
        {
            FontSize = 25,
            HorizontalOptions = LayoutOptions.CenterAndExpand
        };

        private Label personalDailyConsumptions = new Label
        {
            FontSize = 25,
            HorizontalOptions = LayoutOptions.CenterAndExpand
        };

        private Label cooperativeDailyConsumptions = new Label
        {
            FontSize = 25,
            HorizontalOptions = LayoutOptions.CenterAndExpand
        };

        private Day pageDay;

        public ConsumptionsPage(Day day)
        {
            pageDay = day;
            SetConsumptions();

            List<Consumption> consumptions = App.GetDataBase().GetConsumptions(pageDay).ToList();
            var consumptionInfoBox = new DataTemplate(typeof(ConsumptionCustomCell));

            // Page content
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
                    personalDailyConsumptions,
                    plus,
                    cooperativeDailyConsumptions
                },
                Orientation = StackOrientation.Horizontal
            };

            StackLayout topLabels = new StackLayout
            {
                Children =
                {
                    consFirstRowLabels,
                    dailyComsumptions
                }
            };

            ListView consumptionsList = new ListView
            {
                ItemsSource = consumptions,
                ItemTemplate = consumptionInfoBox
            };

            Button addConsButton = new Button
            {
                Text = "Add consumption",
                BackgroundColor = Color.White,
                FontSize = 20
            };

            StackLayout pageStack = new StackLayout
            {
                Children =
                {
                    topLabels,
                    consumptionsList,
                    addConsButton
                }
            };

            // Modal window
            Label modalLabel = new Label
            {
                BackgroundColor = Color.Black,
                TextColor = Color.White,
                HorizontalOptions = LayoutOptions.Fill,
                FontSize = 20,
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center,
                Text = "Add consumption"
            };

            EntryCell consNameEntry = new EntryCell
            {
                Placeholder = "Name"
            };

            SwitchCell consCoop = new SwitchCell
            {
                Text = "Cooperative consumptions?"
            };

            EntryCell consPriceEntry = new EntryCell
            {
                Placeholder = "Price"
            };

            Button okButton = new Button
            {
                Text = "OK",
                HorizontalOptions = LayoutOptions.FillAndExpand
            };

            Button cancelButton = new Button
            {
                Text = "Cancel",
                HorizontalOptions = LayoutOptions.FillAndExpand
            };

            StackLayout modalButtons = new StackLayout
            {
                Children =
                {
                    okButton,
                    cancelButton
                },
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.FillAndExpand
            };

            TableView modalTable = new TableView
            {
                Intent = TableIntent.Form,
                Root = new TableRoot
                {
                    new TableSection
                    {
                        consNameEntry,
                        consCoop,
                        consPriceEntry
                    }
                }
            };

            ContentView modalContent = new ContentView
            {
                IsVisible = false,
                BackgroundColor = Color.White,
                Content = new StackLayout
                {
                    Children =
                    {
                        modalLabel,
                        modalTable,
                        modalButtons
                    }
                }
            };

            // Page content + Modal window
            Content = new AbsoluteLayout
            {
                Children =
                {
                    pageStack,
                    modalContent
                }
            };

            AbsoluteLayout.SetLayoutBounds(pageStack, new Rectangle(0, 0, 1, 1));
            AbsoluteLayout.SetLayoutFlags(pageStack, AbsoluteLayoutFlags.All);
            AbsoluteLayout.SetLayoutBounds(modalContent, new Rectangle(0.5, 0.3, 0.75, 0.45));
            AbsoluteLayout.SetLayoutFlags(modalContent, AbsoluteLayoutFlags.All);

            Title = "Потребление";

            # region Button events
            addConsButton.Clicked += (o, e) =>
            {
                modalContent.IsVisible = true;
                pageStack.BackgroundColor = Color.Gray;
            };

            okButton.Clicked += (o, e) =>
            {
                Regex regex = new Regex("^[0-9]+(.|,)[0-9]+|^[0-9]+$");
                if (regex.IsMatch(consPriceEntry.Text))
                {
                    modalContent.IsVisible = false;
                    pageStack.BackgroundColor = Color.Default;

                    Consumption newCons = new Consumption
                    {
                        ConsumptionName = consNameEntry.Text,
                        CooperationalConsumption = consCoop.On,
                        ConsumptionPrice = double.Parse(consPriceEntry.Text),
                        DayId = day.Id
                    };
                    App.GetDataBase().AddConsumption(newCons);

                    consNameEntry.Text = null;
                    consPriceEntry.Text = null;

                    // Modifying collection and labels
                    consumptions.Add(newCons);
                    SetConsumptions();
                    // Workaround with listView height not updating
                    consumptionsList.ItemsSource = null;
                    consumptionsList.ItemsSource = consumptions;
                }
            };

            cancelButton.Clicked += (o, e) =>
            {
                modalContent.IsVisible = false;
                pageStack.BackgroundColor = Color.White;

                consNameEntry.Text = null;
                consPriceEntry.Text = null;
            };
            #endregion
        }

        public class ConsumptionCustomCell: ViewCell
        {
            public ConsumptionCustomCell()
            {
                Label nameLabel = new Label
                {
                    FontSize = 12,
                    HorizontalOptions = LayoutOptions.StartAndExpand,
                    VerticalOptions = LayoutOptions.Center,
                    Margin = 5
                };

                Label priceLabel = new Label
                {
                    FontSize = 20,
                    TextColor = Color.DarkRed,
                    HorizontalOptions = LayoutOptions.EndAndExpand,
                    VerticalOptions = LayoutOptions.Center
                };

                Label coopLabel = new Label
                {
                    FontSize = 20,
                    HorizontalOptions = LayoutOptions.EndAndExpand,
                    VerticalOptions = LayoutOptions.Center,
                    Margin = 5
                };

                StackLayout info = new StackLayout
                {
                    Children =
                    {
                        nameLabel,
                        priceLabel,
                        coopLabel
                    },
                    Orientation = StackOrientation.Horizontal

                };

                nameLabel.SetBinding(Label.TextProperty, new Binding("ConsumptionName"));
                priceLabel.SetBinding(Label.TextProperty, new Binding("ConsumptionPrice", stringFormat: "{0} грн."));
                coopLabel.SetBinding(Label.TextProperty, new Binding("CooperationalConsumption") { Converter = new BooleanToSignConverter() });

                View = info;
                View.BackgroundColor = Color.White;
                View.HeightRequest = 50;
            }
        }

        public class BooleanToSignConverter : IValueConverter
        {
            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                bool coop = (bool)value;
                if (coop)
                    return "+";
                else
                    return "-";
            }

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                throw new Exception("Functionality not supported");
            }
        }

        public void SetConsumptions()
        {
            double personalConsumptions = GetDailyConsumptions(pageDay, false);
            double cooperativeConsumptions = GetDailyConsumptions(pageDay, true);
            double totalConsumptions = personalConsumptions + cooperativeConsumptions;
            personalDailyConsumptions.Text = personalConsumptions.ToString();
            cooperativeDailyConsumptions.Text = cooperativeConsumptions.ToString();
            dailyComsumptions.Text = totalConsumptions.ToString();
        }

        public double GetDailyConsumptions(Day day, bool coop)
        {
            DataBase db = App.GetDataBase();
            return db.GetConsumptions(day).Where(c => c.CooperationalConsumption == coop).Sum(c => c.ConsumptionPrice);
        }
    }
}