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
    public partial class ConsumptionsPage : ContentPage
    {
        public ConsumptionsPage(Day day)
        {
            Label dailyComsumptions = new Label();
            dailyComsumptions.Text = GetDailyComsumptions(day).ToString();

            IEnumerable<Consumption> comsumptions = App.GetDataBase().GetConsumptions(day);

            var consumptionInfoBox = new DataTemplate(typeof(TextCell));
            consumptionInfoBox.SetBinding(TextCell.TextProperty, new Binding("ConsumptionName"));
            consumptionInfoBox.SetBinding(TextCell.DetailProperty, new Binding("ConsumptionPrice", stringFormat: "{0:C}"));

            ListView consumptionsListView = new ListView
            {
                ItemsSource = comsumptions,
                ItemTemplate = consumptionInfoBox
            };

            Button addConsButton = new Button
            {
                Text = "Add consumption"
            };

            StackLayout pageLayout = new StackLayout
            {
                Children =
                {
                    dailyComsumptions,
                    consumptionsListView,
                    addConsButton
                }
            };

            // Modal window
            Label topModalLabel = new Label
            {
                BackgroundColor = Color.Black,
                HorizontalOptions = LayoutOptions.Fill,
                Text = "Add consumption"
            };

            Entry consNameEntry = new Entry
            {
                Placeholder = "Name"
            };

            Entry consPriceEntry = new Entry
            {
                Placeholder = "Price"
            };

            Button okButton = new Button
            {
                Text = "OK"
            };

            Button cancelButton = new Button
            {
                Text = "Cancel"
            };

            StackLayout modalButtonsLayout = new StackLayout
            {
                Children =
                {
                    okButton,
                    cancelButton
                }
            };

            ContentView modalContent = new ContentView
            {
                IsVisible = false,
                Content = new StackLayout
                {
                    Children =
                    {
                        topModalLabel,
                        consNameEntry,
                        consPriceEntry,
                        modalButtonsLayout
                    }
                }
            };

            // All page
            Content = new StackLayout
            {
                Children =
                {
                    pageLayout,
                    modalContent
                }
            };

            # region Events
            addConsButton.Clicked += (o, e) =>
            {
                modalContent.IsVisible = true;
            };

            okButton.Clicked += (o, e) =>
            {
                modalContent.IsVisible = false;
                App.GetDataBase().AddConsumption(new Consumption
                {
                    ConsumptionName = consNameEntry.Text,
                    ConsumptionPrice = double.Parse(consPriceEntry.Text),
                    DayId = day.Id
                });
            };

            cancelButton.Clicked += (o, e) =>
            {
                modalContent.IsVisible = false;
            };
            #endregion
        }

        public double GetDailyComsumptions(Day day)
        {
            DataBase db = App.GetDataBase();
            return db.GetConsumptions(day).Sum(c => c.ConsumptionPrice);
        }
    }
}