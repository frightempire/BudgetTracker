using BudgetTracker.DataBaseModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BudgetTracker.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ConsumptionsPage : ContentPage
    {
        private Label dailyComsumptions = new Label();
        private Label personalDailyConsumptions = new Label();
        private Label cooperativeDailyConsumptions = new Label();
        private Day pageDay;

        public ConsumptionsPage(Day day)
        {
            pageDay = day;
            SetConsumptions();

            List<Consumption> consumptions = App.GetDataBase().GetConsumptions(day).ToList();

            var consumptionInfoBox = new DataTemplate(typeof(TextCell));
            consumptionInfoBox.SetBinding(TextCell.TextProperty, new Binding("ConsumptionName"));
            consumptionInfoBox.SetBinding(TextCell.DetailProperty, new Binding("ConsumptionPrice", stringFormat: "{0:C}"));

            ListView consumptionsListView = new ListView
            {
                ItemsSource = consumptions,
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
                    personalDailyConsumptions,
                    cooperativeDailyConsumptions,
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
                Content = new StackLayout
                {
                    Children =
                    {
                        topModalLabel,
                        modalTable,
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
                    CooperationalConsumption = consCoop.On,
                    ConsumptionPrice = double.Parse(consPriceEntry.Text),
                    DayId = day.Id
                };
                App.GetDataBase().AddConsumption(newCons);

                consNameEntry.Text = null;
                consPriceEntry.Text = null;

                // Modifying observable collection and label
                consumptions.Add(newCons);
                SetConsumptions();
            };

            cancelButton.Clicked += (o, e) =>
            {
                modalContent.IsVisible = false;
                consNameEntry.Text = null;
                consPriceEntry.Text = null;
            };
            #endregion
        }

        public void SetConsumptions()
        {
            double personalConsumptions = GetDailyConsumptions(pageDay, false);
            double cooperativeConsumptions = GetDailyConsumptions(pageDay, true);
            double totalConsumptions = personalConsumptions + cooperativeConsumptions;
            personalDailyConsumptions.Text = personalConsumptions.ToString() + " + ";
            cooperativeDailyConsumptions.Text = cooperativeConsumptions.ToString() + " = ";
            dailyComsumptions.Text = totalConsumptions.ToString();
        }

        public double GetDailyConsumptions(Day day, bool coop)
        {
            DataBase db = App.GetDataBase();
            return db.GetConsumptions(day).Where(c => c.CooperationalConsumption == coop).Sum(c => c.ConsumptionPrice);
        }
    }
}