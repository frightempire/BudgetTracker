using System;
using System.IO;
using Xamarin.Forms;
using BudgetTracker.Droid;

[assembly: Dependency(typeof(DbHelper_Android))]
namespace BudgetTracker.Droid
{
    class DbHelper_Android: IDbHelper
    {
        public string GetDbPath(string dbName)
        {
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), dbName);
        }
    }
}