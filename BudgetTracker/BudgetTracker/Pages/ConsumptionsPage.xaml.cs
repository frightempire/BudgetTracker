using BudgetTracker.DataBaseModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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
            double dailyCons = GetDailyComsumptions(day);
            dailyComsumptions.Text = dailyCons.ToString();

            List<Consumption> enumerableConsumptions = App.GetDataBase().GetConsumptions(day).ToList();

            var consumptionInfoBox = new DataTemplate(typeof(TextCell));
            consumptionInfoBox.SetBinding(TextCell.TextProperty, new Binding("ConsumptionName"));
            consumptionInfoBox.SetBinding(TextCell.DetailProperty, new Binding("ConsumptionPrice", stringFormat: "{0:C}"));

            ListView consumptionsListView = new ListView
            {
                ItemsSource = enumerableConsumptions,
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
                Consumption newCons = new Consumption
                {
                    ConsumptionName = consNameEntry.Text,
                    ConsumptionPrice = double.Parse(consPriceEntry.Text),
                    DayId = day.Id
                };
                App.GetDataBase().AddConsumption(newCons);

                consNameEntry.Text = null;
                consPriceEntry.Text = null;

                // Modifying observable collection and label
                enumerableConsumptions.Add(newCons);
                dailyCons += newCons.ConsumptionPrice;
                dailyComsumptions.Text = dailyCons.ToString();
            };

            cancelButton.Clicked += (o, e) =>
            {
                modalContent.IsVisible = false;
                consNameEntry.Text = null;
                consPriceEntry.Text = null;
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