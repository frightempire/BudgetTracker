using BudgetTracker.DataBaseModels;
using BudgetTracker.Pages;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace BudgetTracker
{
    public partial class App : Application
    {
        public const string dbName = "budgetDb.db3";
        public static DataBase dataBase;

        public static DataBase GetDataBase()
        {
            return dataBase ?? (dataBase = new DataBase(dbName));
        }

        public App()
        {
            InitializeComponent();
            MainPage = new NavigationPage(new MonthsPage());
        }

        protected override void OnStart()
        { }

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
