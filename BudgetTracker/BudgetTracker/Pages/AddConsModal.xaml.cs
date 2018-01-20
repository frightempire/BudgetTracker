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
    public partial class AddConsModal : ContentPage
    {
        public AddConsModal()
        { }

        private void SubmitConsButton_Clicked(object sender, EventArgs e)
        {
            DataBase db = App.GetDataBase();
            db.AddConsumption(new Consumption {
                ConsumptionName = ConsumptionNameEntry.Text,
                ConsumptionPrice = double.Parse(ConsumptionPriceEntry.Text),
                DayId = (BindingContext as Day).Id
            });
        }
    }
}