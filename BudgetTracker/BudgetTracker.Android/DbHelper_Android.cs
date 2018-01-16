using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

using Xamarin.Forms;

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