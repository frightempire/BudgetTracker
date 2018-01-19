using BudgetTracker.DataBaseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BudgetTracker
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MonthsPage : ContentPage
    {
        public MonthsPage()
        {
            IEnumerable<Month> months = App.GetDataBase().GetMonths();
            var infoBox = new DataTemplate(typeof(TextCell));

            infoBox.SetBinding(TextCell.TextProperty, new Binding("MonthDate").StringFormat);
            infoBox.SetBinding(TextCell.DetailProperty, new Binding());
        }
    }
}