using BudgetTracker.DataBaseModels;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace BudgetTracker
{
    public class DataBase
    {
        static SQLiteConnection dataBase;

        public DataBase(string fileName)
        {
            string dbPath = DependencyService.Get<IDbHelper>().GetDbPath(fileName);
            dataBase = new SQLiteConnection(dbPath);
            dataBase.CreateTable<Month>();
            dataBase.CreateTable<Day>();
            dataBase.CreateTable<Consumption>();
        }

        public IEnumerable<Month> GetMonths()
        {
            return dataBase.Table<Month>();
        }

        public IEnumerable<Day> GetDays(Month month)
        {
            return dataBase.Table<Day>().Where(d => d.MonthId == month.Id);
        }

        public IEnumerable<Consumption> GetConsumptions(Day day)
        {
            return dataBase.Table<Consumption>().Where(c => c.DayId == day.Id);
        }

        public void AddMonth()
        {
            dataBase.Insert(new Month {
                MonthDate = DateTime.Today
            });
        }

        public void AddDay(Month month)
        {
            dataBase.Insert(new Day {
                DayDate = DateTime.Today,
                MonthId = month.Id
            });
        }

        public void AddConsumption(Consumption item)
        {
            dataBase.Insert(item);
        }
    }
}
