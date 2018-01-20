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
        public ConsumptionsPage()
        {
            IEnumerable<Consumption> comsunptions = App.GetDataBase().GetConsumptions(BindingContext as Day);
            ConsumptionsListView.ItemsSource = comsunptions;

            var consumptionInfoBox = new DataTemplate(typeof(TextCell));
            consumptionInfoBox.SetBinding(TextCell.TextProperty, new Binding("ConsumptionName"));
            consumptionInfoBox.SetBinding(TextCell.DetailProperty, new Binding("ConsumptionPrice", stringFormat: "0:C"));

            ConsumptionsListView.ItemTemplate = consumptionInfoBox;
        }

        async private void AddConsButton_Clicked(object sender, EventArgs e)
        {
            var addConsModal = new AddConsModal();
            addConsModal.BindingContext = BindingContext;
            await Navigation.PushModalAsync(addConsModal);
        }
    }
}