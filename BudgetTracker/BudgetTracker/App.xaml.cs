using BudgetTracker.DataBaseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace BudgetTracker
{
    public partial class App : Application
    {
        public const string dbName = "budgetDb.db";
        public static DataBase dataBase;

        public static DataBase GetDataBase()
        {
            return dataBase ?? (dataBase = new DataBase(dbName));
        }

        public App()
        {
            InitializeComponent();
            MainPage = new MonthsPage();
        }

        protected override void OnStart()
        {
            DataBase db = GetDataBase();

            IEnumerable<Month> months = db.GetMonths();
            if (months.Count() == 0 || months.OrderBy(m => m.MonthDate).Last().MonthDate.Month != DateTime.Today.Month)
                db.AddMonth();

            Month lastMonth = months.OrderBy(m => m.MonthDate).Last();

            IEnumerable<Day> days = db.GetDays(lastMonth);
            if (days.Count() == 0 || days.OrderBy(d => d.DayDate).Last().DayDate.Day != DateTime.Today.Day)
                db.AddDay(lastMonth);
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
